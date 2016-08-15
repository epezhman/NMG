using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using NMG.Core.Domain;
using System.Text;
using System.Resources;
using System.Collections;
using System.Collections.Generic;


namespace NMG.Core.Generator
{
    public class AnnotationGenerator : AbstractGenerator
    {
        private readonly ApplicationPreferences appPrefs;
        private readonly Language language;

        public AnnotationGenerator(ApplicationPreferences appPrefs, Table table)
            : base(appPrefs.FolderPath, "Domain", appPrefs.TableName, appPrefs.NameSpace, appPrefs.AssemblyName, appPrefs.Sequence, table, appPrefs)
        {
            this.appPrefs = appPrefs;
            language = appPrefs.Language;
        }

        public override void Generate()
        {
            var className = string.Format("{0}{1}_Annotation", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var compileUnit = GetCompileUnit(className);
            WriteToFile(compileUnit, className);
        }

        public CodeCompileUnit GetCompileUnit(string className)
        {
            var codeGenerationHelper = new CodeGenerationHelper();
            var compileUnit = codeGenerationHelper.GetCodeCompileUnitWithInheritanceAndInterface(nameSpace, className, appPrefs.InheritenceAndInterfaces);
            var mapper = new DataTypeMapper();
            var newType = compileUnit.Namespaces[0].Types[0];

            newType.IsPartial = true;

            foreach (var pk in Table.PrimaryKey.Columns)
            {
                var mapFromDbType = mapper.MapFromDBType(this.appPrefs.ServerType, pk.DataType, pk.DataLength, pk.DataPrecision, pk.DataScale);
                var prop = codeGenerationHelper.CreateAutoPropertyWithVirtualConcern(mapFromDbType.ToString(), Formatter.FormatText(pk.Name), Table.Name, this.appPrefs, appPrefs.IncludeVirtual);
                newType.Members.Add(prop);
            }

            // Note that a foreign key referencing a primary within the same table will end up giving you a foreign key property with the same name as the table.
            /*foreach (var fk in Table.ForeignKeys.Where(fk => !string.IsNullOrEmpty(fk.References)))
            {

                if (fk.References != className)
                {
                    newType.Members.Add(codeGenerationHelper.CreateAutoProperty(appPrefs.ClassNamePrefix + Formatter.FormatSingular(fk.References), Formatter.FormatSingular(fk.UniquePropertyName), Table.Name, this.appPrefs, appPrefs.UseLazy));
                }
                else
                {
                    newType.Members.Add(codeGenerationHelper.CreateAutoProperty(appPrefs.ClassNamePrefix + Formatter.FormatSingular(fk.References), Formatter.FormatSingular("Parent" + fk.UniquePropertyName), Table.Name, this.appPrefs, appPrefs.UseLazy));
                }
            }*/

            foreach (var column in Table.Columns.Where(x => x.IsPrimaryKey != true))
            {
                if (!appPrefs.IncludeForeignKeys && column.IsForeignKey)
                    continue;
                var mapFromDbType = mapper.MapFromDBType(this.appPrefs.ServerType, column.DataType, column.DataLength, column.DataPrecision, column.DataScale);
                var prop = codeGenerationHelper.CreateAutoPropertyWithVirtualConcern(mapFromDbType, Formatter.FormatText(column.Name), column.IsNullable, column.DataLength, Table.Name, column, this.appPrefs, appPrefs.IncludeVirtual);
                newType.Members.Add(prop);
            }

            var constructorStatements = new CodeStatementCollection();
            /*foreach (var hasMany in Table.HasManyRelationships)
            {
                newType.Members.Add(codeGenerationHelper.CreateAutoProperty(appPrefs.ForeignEntityCollectionType + "<" + appPrefs.ClassNamePrefix + Formatter.FormatSingular(hasMany.Reference) + ">", Formatter.FormatPlural(hasMany.Reference + hasMany.ReferenceColumn), appPrefs.UseLazy));
                constructorStatements.Add(new CodeSnippetStatement(string.Format(TABS + "{0} = new {1}<{2}{3}>();", Formatter.FormatPlural(hasMany.Reference + hasMany.ReferenceColumn), codeGenerationHelper.InstatiationObject(appPrefs.ForeignEntityCollectionType), appPrefs.ClassNamePrefix, Formatter.FormatSingular(hasMany.Reference))));
            }*/

            var constructor = new CodeConstructor { Attributes = MemberAttributes.Public };
            constructor.Statements.AddRange(constructorStatements);
            newType.Members.Add(constructor);

            return compileUnit;
        }

        private void WriteToFile(CodeCompileUnit compileUnit, string className)
        {
            var provider = GetCodeDomProvider();
            var sourceFile = GetCompleteFilePath(provider, className);
            using (provider)
            {
                var streamWriter = new StreamWriter(sourceFile);
                var textWriter = new IndentedTextWriter(streamWriter, "    ");
                using (textWriter)
                {
                    using (streamWriter)
                    {
                        var options = new CodeGeneratorOptions { BlankLinesBetweenMembers = true };
                        provider.GenerateCodeFromCompileUnit(compileUnit, textWriter, options);
                    }
                }
            }
            CleanupGeneratedFile(sourceFile);
        }

        private void CleanupGeneratedFile(string sourceFile)
        {
            string entireContent;
            using (var reader = new StreamReader(sourceFile))
            {
                entireContent = reader.ReadToEnd();
            }
            entireContent = RemoveComments(entireContent);
            entireContent = AddStandardHeader(entireContent);
            entireContent = FixAutoProperties(entireContent);
            entireContent = AddMetadata(entireContent);
            using (var writer = new StreamWriter(sourceFile))
            {
                writer.Write(entireContent);
            }
        }

        private string AddMetadata(string entireContent)
        {            
            StringBuilder builder = new StringBuilder();
            string tempAnnotation = string.Format("{0}{1}_Annotation", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            string temp = string.Format("{0}{1}", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            builder.AppendLine(string.Format(" [MetadataType(typeof({0}))]", tempAnnotation));
            builder.AppendLine(string.Format("\tpublic partial class {0}", temp));
            builder.AppendLine("\t{");
            builder.AppendLine("\t}");
            builder.AppendLine("");
            builder.Append("\t");
            entireContent = entireContent.Insert(entireContent.IndexOf("public") - 1, builder.ToString());
            return entireContent;
        }

        // Hack : Auto property generator is not there in CodeDom.
        private static string FixAutoProperties(string entireContent)
        {
            // Do NOT mess with this... 
            //Indomitable: Just a little :)
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            builder.Append("        }");
            entireContent = entireContent.Replace(builder.ToString(), "{ }");
            builder.Clear();
            builder.AppendLine("{");
            builder.AppendLine("            get {");
            builder.AppendLine("            }");
            builder.AppendLine("            set {");
            builder.AppendLine("            }");
            builder.Append("        }");
            entireContent = entireContent.Replace(builder.ToString(), "{ get; set; }");
            return entireContent;
        }

        private string AddStandardHeader(string entireContent)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("using System;");
            builder.AppendLine("using System.Text;");
            builder.AppendLine("using System.Collections.Generic;");
            builder.AppendLine("using System.ComponentModel.DataAnnotations;");
            builder.Append(entireContent);
            return builder.ToString();
        }

        private static string RemoveComments(string entireContent)
        {
            int end = entireContent.LastIndexOf("----------");
            entireContent = entireContent.Remove(0, end + 10);
            return entireContent;
        }

        private string GetCompleteFilePath(CodeDomProvider provider, string className)
        {
            if (!Directory.Exists(string.Format(@"{0}\Annotations", filePath)))
            {
                Directory.CreateDirectory(string.Format(@"{0}\Annotations", filePath));
            }
            string fileName = string.Format(@"{0}\Annotations\", filePath) + className;
            return provider.FileExtension[0] == '.'
                       ? fileName + provider.FileExtension
                       : fileName + "." + provider.FileExtension;
        }

        private CodeDomProvider GetCodeDomProvider()
        {
            return language == Language.CSharp ? (CodeDomProvider)new CSharpCodeProvider() : new VBCodeProvider();
        }
    }
}