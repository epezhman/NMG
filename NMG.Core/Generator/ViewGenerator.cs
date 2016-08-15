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
    public class ViewGenerator : AbstractGenerator
    {
        private readonly ApplicationPreferences appPrefs;

        public ViewGenerator(ApplicationPreferences appPrefs, Table table)
            : base(appPrefs.FolderPath, "Domain", appPrefs.TableName, appPrefs.NameSpace, appPrefs.AssemblyName, appPrefs.Sequence, table, appPrefs)
        {
            this.appPrefs = appPrefs;
            this.nameSpace = appPrefs.ControllerNamespace;
        }

        public override void Generate()
        {
            var viewGenerationHelper = new ViewGenerationHelper();
            var classNameView = string.Format("{0}{1}", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var className = string.Format("{0}{1}", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
            var view = string.Empty;
            if (appPrefs.WithKendo)
            {
                view = viewGenerationHelper.GnerateIndexKendoView(this.Table, appPrefs, className);
            }
            else
            {
                view = viewGenerationHelper.GnerateIndexView(this.Table, appPrefs, className);
            }
            WriteToFile(view, className, "Index");
            if (appPrefs.WithKendo)
            {
                view = viewGenerationHelper.GnerateIXKnedoView(this.Table, appPrefs, className);
            }
            else
            {
                view = viewGenerationHelper.GnerateIXView(this.Table, appPrefs, className);
            }
            WriteToFile(view, className, "IX");
            view = viewGenerationHelper.GnerateSearchView(this.Table, appPrefs, className);
            WriteToFile(view, className, className + "SearchModelView");
            if (appPrefs.GenerateCRUD)
            {
                view = viewGenerationHelper.GnerateEditCreate(this.Table, "create", appPrefs, className);
                WriteToFile(view, className, "Create");
                view = viewGenerationHelper.GnerateEditCreate(this.Table, "edit", appPrefs, className);
                WriteToFile(view, className, "Edit");
                view = viewGenerationHelper.GnerateDelete(this.Table, "delete", appPrefs, className);
                WriteToFile(view, className, "Delete");
            }
            if (appPrefs.GenerateLookUps)
            {
                foreach (var column in this.Table.Columns.Where(x => x.InLookUpCombo))
                {
                    view = viewGenerationHelper.GnerateLookUpCombo(tableName, column.Name);
                    WriteToFile(view, className, "Get" + tableName + column.Name + "List");
                }
            }
            if (appPrefs.Bootstrap)
            {
                view = viewGenerationHelper.GnerateIndexView(this.Table, appPrefs, className, true); 
                WriteToFile(view, className, "Index", true);
                view = viewGenerationHelper.GnerateIXView(this.Table, appPrefs, className , true);
                WriteToFile(view, className, "IX", true);
                view = viewGenerationHelper.GnerateSearchView(this.Table, appPrefs, className, true); 
                WriteToFile(view, className, className + "SearchModelView", true);
                view = viewGenerationHelper.GnerateEditCreate(this.Table, "create", appPrefs, className , true);
                WriteToFile(view, className, "Create", true);
                view = viewGenerationHelper.GnerateEditCreate(this.Table, "edit", appPrefs, className, true);
                WriteToFile(view, className, "Edit", true);
                view = viewGenerationHelper.GnerateDelete(this.Table, "delete", appPrefs, className , true);
                WriteToFile(view, className, "Delete", true);

            }
        }

        private void WriteToFile(string view, string className, string viewName, bool bootstrap = false)
        {
            var sourceFile = string.Empty;
            if (bootstrap)
            {
                sourceFile = GetCompleteFilePath("cshtml", className, viewName , true);
            }
            else
            {
                sourceFile = GetCompleteFilePath("cshtml", className, viewName);
            }
            using (var file = new StreamWriter(sourceFile, false, Encoding.UTF8))
            {
                file.Write(view);
            }
        }

        private string GetCompleteFilePath(string fileExtension, string className, string typeName , bool bootstrap = false)
        {
            if (bootstrap)
            {
                if (!Directory.Exists(string.Format(@"{0}\ViewsBootStrap\{1}", filePath, className)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\ViewsBootStrap\{1}", filePath, className));
                }
                string fileName = string.Format(@"{0}\ViewsBootStrap\{1}\{2}", filePath, className, typeName);
                return fileExtension[0] == '.'
                           ? fileName + fileExtension
                           : fileName + "." + fileExtension;
            }
            else
            {
                if (!Directory.Exists(string.Format(@"{0}\Views\{1}", filePath, className)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\Views\{1}", filePath, className));
                }
                string fileName = string.Format(@"{0}\Views\{1}\{2}", filePath, className, typeName);
                return fileExtension[0] == '.'
                           ? fileName + fileExtension
                           : fileName + "." + fileExtension;
            }
           
        }

    }
}
