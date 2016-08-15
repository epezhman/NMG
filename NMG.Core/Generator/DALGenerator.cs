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
    public class DALGenerator : AbstractGenerator
    {
        private readonly ApplicationPreferences appPrefs;

        public DALGenerator(ApplicationPreferences appPrefs, Table table)
            : base(appPrefs.FolderPath, "Domain", appPrefs.TableName, appPrefs.NameSpace, appPrefs.AssemblyName, appPrefs.Sequence, table, appPrefs)
        {
            this.appPrefs = appPrefs;
            this.nameSpace = appPrefs.DALNamespace;
        }

        public override void Generate()
        {
            GenerateLinqDAL();
            GenerateLinqNHibernate();
        }

        public void GenerateLinqDAL()
        {
            var classNameController = string.Format("{0}{1}DAL", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            //var className = string.Format("{0}{1}", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var className = string.Format("{0}", Formatter.FormatSingular(Table.Name));
            var compileUnit = GetCompileUnit(classNameController, className, 1);
            WriteToFile(compileUnit, classNameController, 1);
        }

        public void GenerateLinqNHibernate()
        {
            var classNameController = string.Format("{0}{1}DAL", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            //var className = string.Format("{0}{1}", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var className = string.Format("{0}", Formatter.FormatSingular(Table.Name));
            var compileUnit = GetCompileUnit(classNameController, className, 2);
            WriteToFile(compileUnit, classNameController, 2);
        }

        public CodeCompileUnit GetCompileUnit(string classNameController, string className, byte ORMType)
        {
            var codeGenerationHelper = new CodeGenerationHelper();
            var compileUnit = new CodeCompileUnit();
            compileUnit = codeGenerationHelper.GetCodeCompileUnitWithInheritanceAndInterface(nameSpace, classNameController, "");
            var newType = compileUnit.Namespaces[0].Types[0];
            newType.IsPartial = true;
            newType = codeGenerationHelper.GenerateGetDALMethod(className, this.Table, appPrefs, newType, ORMType);
            newType = codeGenerationHelper.GenerateGetAllDALMethod(className, this.Table, appPrefs, newType, ORMType);
            newType = codeGenerationHelper.GenerateFindDALMethod(className, this.Table, appPrefs, newType, ORMType);
            newType = codeGenerationHelper.GenerateAddDALMethod(className, this.Table, appPrefs, newType, ORMType);
            newType = codeGenerationHelper.GenerateUpdateDALMethod(className, this.Table, appPrefs, newType, ORMType);
            newType = codeGenerationHelper.GenerateDeleteDALMethod(className, this.Table, appPrefs, newType, ORMType);

            return compileUnit;
        }

        private void WriteToFile(CodeCompileUnit compileUnit, string classNameController, byte ORMType)
        {
            var provider = GetCodeDomProvider();
            var sourceFile = GetCompleteFilePath(provider, classNameController, ORMType);
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
            entireContent = AddStandardHeader(entireContent, appPrefs.DALReferences);
            entireContent = FixAutoProperties(entireContent);
            using (var writer = new StreamWriter(sourceFile))
            {
                writer.Write(entireContent);
            }
        }

        private static string RemoveComments(string entireContent)
        {
            int end = entireContent.LastIndexOf("----------");
            entireContent = entireContent.Remove(0, end + 10);
            return entireContent;
        }

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

        private string AddStandardHeader(string entireContent, string references)
        {
            StringBuilder builder = new StringBuilder();
            var eachReference = references.Split(new char[] { ' ', ';' });
            foreach (var reference in eachReference)
            {
                if (!string.IsNullOrEmpty(reference) && reference.ToLower() != "using")
                {
                    builder.AppendLine(string.Format("using {0};", reference));
                }
            }
            builder.AppendLine(string.Format("using {0};", appPrefs.NameSpace));
            builder.Append(entireContent);
            return builder.ToString();
        }

        private string GetCompleteFilePath(CodeDomProvider provider, string className, byte ORMType)
        {
            if (ORMType == 1)
            {
                if (!Directory.Exists(string.Format(@"{0}\DALs-Linq\", filePath)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\DALs-Linq\", filePath));
                }
                string fileName = string.Format(@"{0}\DALs-Linq\{1}", filePath, className);
                return provider.FileExtension[0] == '.'
                           ? fileName + provider.FileExtension
                           : fileName + "." + provider.FileExtension;
            }
            else if (ORMType == 2)
            {
                if (!Directory.Exists(string.Format(@"{0}\DALs-NHibernate\", filePath)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\DALs-NHibernate\", filePath));
                }
                string fileName = string.Format(@"{0}\DALs-NHibernate\{1}", filePath, className);
                return provider.FileExtension[0] == '.'
                           ? fileName + provider.FileExtension
                           : fileName + "." + provider.FileExtension;
            }
            else
                return String.Empty;
        }

        private CodeDomProvider GetCodeDomProvider()
        {
            return new CSharpCodeProvider();
        }
    }
}
