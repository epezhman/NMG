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
    public class SearchModelGenerator : AbstractGenerator
    {
        private readonly ApplicationPreferences appPrefs;

        public SearchModelGenerator(ApplicationPreferences appPrefs, Table table)
            : base(appPrefs.FolderPath, "Domain", appPrefs.TableName, appPrefs.NameSpace, appPrefs.AssemblyName, appPrefs.Sequence, table, appPrefs)
        {
            this.appPrefs = appPrefs;
            this.nameSpace = appPrefs.ControllerNamespace;
        }

        public override void Generate()
        {
            var classNameSearchModels = string.Format("{0}{1}SearchModel", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var className = string.Format("{0}{1}", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var compileUnit = GetCompileUnit(classNameSearchModels, className);
            WriteToFile(compileUnit, classNameSearchModels);
        }

        public CodeCompileUnit GetCompileUnit(string classNameController, string className)
        {
            var codeGenerationHelper = new CodeGenerationHelper();
            var compileUnit = new CodeCompileUnit();
            if (appPrefs.IncludePaging)
            {
                compileUnit = codeGenerationHelper.GetCodeCompileUnitWithInheritanceAndInterface(appPrefs.NameSpace, classNameController, "");
            }
            else
            {
                if (appPrefs.WithKendo)
                {

                    compileUnit = codeGenerationHelper.GetCodeCompileUnitWithInheritanceAndInterface(appPrefs.NameSpace, classNameController, "KendoPaging");
                }
                else
                {
                    compileUnit = codeGenerationHelper.GetCodeCompileUnitWithInheritanceAndInterface(appPrefs.NameSpace, classNameController, "PagingItems");
                }
            }
            var mapper = new DataTypeMapper();
            var newType = compileUnit.Namespaces[0].Types[0];
            foreach (var column in this.Table.Columns.Where(x => x.InSearch))
            {
                newType.Members.Add(codeGenerationHelper.GeneratetSearchModel(column.Name, column.MappedDataType, appPrefs, column.Name, tableName));
            }
            if (appPrefs.IncludePaging)
            {
                newType.Members.Add(codeGenerationHelper.GeneratetSearchModel("PageIndex", "System.Int32", appPrefs, string.Empty, string.Empty));
                newType.Members.Add(codeGenerationHelper.GeneratetSearchModel("PageSize", "System.Int32", appPrefs, string.Empty, string.Empty));
            }
            var codeConstructor = new CodeConstructor();
            codeConstructor.Attributes = MemberAttributes.Public;
            if (appPrefs.IncludePaging)
            {
                codeConstructor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("PageIndex"), new CodePrimitiveExpression(0)));
                codeConstructor.Statements.Add(new CodeAssignStatement(new CodeVariableReferenceExpression("PageSize"), new CodePrimitiveExpression(0)));
                newType.Members.Add(codeConstructor);
            }
            return compileUnit;
        }

        private void WriteToFile(CodeCompileUnit compileUnit, string classNameController)
        {
            var provider = GetCodeDomProvider();
            var sourceFile = GetCompleteFilePath(provider, classNameController);
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

        private string AddStandardHeader(string entireContent)
        {
            StringBuilder builder = new StringBuilder();
            var references = "using System; using System.Collections.Generic; using System.Linq; using System.Web; using APS.Common.Models; using System.ComponentModel.DataAnnotations;";
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

        private string GetCompleteFilePath(CodeDomProvider provider, string className)
        {
            if (!Directory.Exists(string.Format(@"{0}\SearchModels", filePath)))
            {
                Directory.CreateDirectory(string.Format(@"{0}\SearchModels", filePath));
            }
            string fileName = string.Format(@"{0}\SearchModels\{1}", filePath, className);
            return provider.FileExtension[0] == '.'
                       ? fileName + provider.FileExtension
                       : fileName + "." + provider.FileExtension;
        }

        private CodeDomProvider GetCodeDomProvider()
        {
            return new CSharpCodeProvider();
        }
    }
}
