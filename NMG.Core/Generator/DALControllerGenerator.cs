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
    public class DALControllerGenerator : AbstractGenerator
    {
        private readonly ApplicationPreferences appPrefs;

        public DALControllerGenerator(ApplicationPreferences appPrefs, Table table)
            : base(appPrefs.FolderPath, "Domain", appPrefs.TableName, appPrefs.NameSpace, appPrefs.AssemblyName, appPrefs.Sequence, table, appPrefs)
        {
            this.appPrefs = appPrefs;
            this.nameSpace = appPrefs.ControllerNamespace;
        }

        public override void Generate()
        {
            var classNameController = string.Format("{0}{1}Controller", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var className = string.Format("{0}{1}", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var compileUnit = GetCompileUnit(classNameController, className);
            WriteToFile(compileUnit, classNameController);
        }

        public CodeCompileUnit GetCompileUnit(string classNameController, string className)
        {
            var codeGenerationHelper = new CodeGenerationHelper();
            var compileUnit = new CodeCompileUnit();
            compileUnit = codeGenerationHelper.GetCodeCompileUnitWithInheritanceAndInterface(nameSpace, classNameController, "BaseController");
            var mapper = new DataTypeMapper();
            var newType = compileUnit.Namespaces[0].Types[0];
            if (appPrefs.AuthorizeEnable)
            {
                var codeAttrDecl = new CodeAttributeDeclaration { Name = "Authorize" };
                newType.CustomAttributes.Add(codeAttrDecl);
            }
            newType.Members.Add(codeGenerationHelper.GenerateIndexDAL(className, appPrefs, 1));
            newType.Members.Add(codeGenerationHelper.GenerateIndexDAL(className, appPrefs, 2));
            newType.Members.Add(codeGenerationHelper.GenerateSearchFiltersDAL(this.Table, className, appPrefs));
            newType.Members.Add(codeGenerationHelper.GenerateIXDAL(this.Table, className, appPrefs));
            newType.Members.Add(codeGenerationHelper.GenerateCreateMethodDAL(className, this.Table, appPrefs));
            newType.Members.Add(codeGenerationHelper.GenerateCreateSubmitMethodDAL(className, this.Table, appPrefs));
            newType.Members.Add(codeGenerationHelper.GenerateEditMethodDAL(className, this.Table, appPrefs));
            newType.Members.Add(codeGenerationHelper.GenerateEditSubmitMethodDAL(className, this.Table, appPrefs));
            newType.Members.Add(codeGenerationHelper.GenerateDeleteMethodDAL(className, this.Table, appPrefs));
            newType.Members.Add(codeGenerationHelper.GenerateDeleteSubmitMethodDAL(className, this.Table, appPrefs));
            if (appPrefs.GenerateLookUps)
            {
                foreach (var column in this.Table.Columns.Where(x => x.InLookUpCombo))
                {
                    newType.Members.Add(codeGenerationHelper.GeneratetLookUpComboMethodDAL(className, this.Table, column, appPrefs));
                }

                foreach (var column in this.Table.Columns.Where(x => x.InLookUpLabel))
                {
                    newType.Members.Add(codeGenerationHelper.GenerateLookUpLabelMethodDAL(className, this.Table, column, appPrefs));
                }
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
            entireContent = AddStandardHeader(entireContent, appPrefs.ControllerReferences);
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
            builder.AppendLine(string.Format("using {0};", appPrefs.DALNamespace));
            builder.Append(entireContent);
            return builder.ToString();
        }

        private string GetCompleteFilePath(CodeDomProvider provider, string className)
        {
            if (!Directory.Exists(string.Format(@"{0}\DALControllers\", filePath)))
            {
                Directory.CreateDirectory(string.Format(@"{0}\DALControllers\", filePath));
            }
            string fileName = string.Format(@"{0}\DALControllers\{1}", filePath, className);
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
