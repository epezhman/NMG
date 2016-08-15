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
    public class ResourceGenerator : AbstractGenerator
    {
        private readonly ApplicationPreferences appPrefs;

        public ResourceGenerator(ApplicationPreferences appPrefs, Table table)
            : base(appPrefs.FolderPath, "Domain", appPrefs.TableName, appPrefs.NameSpace, appPrefs.AssemblyName, appPrefs.Sequence, table, appPrefs)
        {
            this.appPrefs = appPrefs;
        }

        public override void Generate()
        {
            if (appPrefs.ResourceGeneration)
            {
                var className = string.Format("{0}{1}", appPrefs.ClassNamePrefix, Formatter.FormatSingular(Table.Name));
                if (!Directory.Exists(string.Format(@"{0}\Resource", appPrefs.FolderPath)))
                {
                    Directory.CreateDirectory(string.Format(@"{0}\Resource", appPrefs.FolderPath));
                }
                using (var writer = new ResXResourceWriter(string.Format(@"{0}\Resource\{1}{2}{3}.resx", appPrefs.FolderPath, appPrefs.TablePrefix, tableName, appPrefs.TablePost)))
                {
                    foreach (var pk in Table.PrimaryKey.Columns)
                    {
                        if (string.IsNullOrEmpty(pk.PersianName))
                        {
                            writer.AddResource(pk.Name, pk.Name);
                        }
                        else
                        {
                            writer.AddResource(pk.Name, pk.PersianName);
                        }
                    }
                    foreach (var fk in Table.ForeignKeys.Where(fk => !string.IsNullOrEmpty(fk.References)))
                    {
                        if (fk.References != className)
                        {
                            writer.AddResource(fk.UniquePropertyName, fk.UniquePropertyName);
                        }
                        else
                        {
                            writer.AddResource("Parent" + fk.UniquePropertyName, "Parent" + fk.UniquePropertyName);
                        }
                    }
                    foreach (var column in Table.Columns.Where(x => x.IsPrimaryKey != true))
                    {
                        if (!appPrefs.IncludeForeignKeys && column.IsForeignKey)
                            continue;
                        if (string.IsNullOrEmpty(column.PersianName))
                        {
                            writer.AddResource(column.Name, column.Name);
                        }
                        else
                        {
                            writer.AddResource(column.Name, column.PersianName);
                        }
                    }
                    writer.Generate();
                    writer.Close();
                }
                string[] unmatchedElements;
                var codeProvider = new CSharpCodeProvider();
                var code = System.Resources.Tools.StronglyTypedResourceBuilder.Create(string.Format(@"{0}\Resource\{1}{2}{3}.resx", filePath, appPrefs.TablePrefix, tableName, appPrefs.TablePost), appPrefs.TablePrefix + tableName + appPrefs.TablePost, appPrefs.ResourceReference, codeProvider, true, out unmatchedElements);
                //using (StreamWriter writer = new StreamWriter(string.Format(@"{0}\Resource\{1}{2}{3}.Designer.cs", appPrefs.FolderPath, appPrefs.TablePrefix, tableName, appPrefs.TablePost), false, System.Text.Encoding.UTF8))
                //{
                //    codeProvider.GenerateCodeFromCompileUnit(code, writer,
                //        new System.CodeDom.Compiler.CodeGeneratorOptions());
                //}
            }
            UpdateDatabaseMessages();
        }

        public void UpdateDatabaseMessages()
        {
            var dc = MetaDataGenerator.GetMetaDataGeneratorDC();
            foreach (var column in Table.Columns.Where(x => x.IsPrimaryKey != true))
            {
                if (!appPrefs.IncludeForeignKeys && column.IsForeignKey)
                    continue;
                if (column.DataLength.HasValue && column.DataLength.Value > 0)
                {
                    if (!dc.Messages.Any(x => x.Name == appPrefs.MaxLengthName + column.DataLength.Value))
                    {
                        var obj = new NMG.Core.DataManager.DataContext.Message
                        {
                            Type = 2,
                            Name = appPrefs.MaxLengthName + column.DataLength.Value,
                            FaName = "حداکثر طول {0} " + column.DataLength.Value + " حرف است",
                            EnName = appPrefs.MaxLengthName + column.DataLength.Value + "{0}"
                        };
                        dc.Messages.InsertOnSubmit(obj);
                        dc.SubmitChanges();
                    }
                }
                if (!column.IsNullable)
                {
                    if (!dc.Messages.Any(x => x.Name == appPrefs.Required))
                    {
                        var obj = new NMG.Core.DataManager.DataContext.Message
                        {
                            Type = 2,
                            Name = appPrefs.Required,
                            FaName = "{0} ضروری است",
                            EnName = appPrefs.Required + "{0}"
                        };
                        dc.Messages.InsertOnSubmit(obj);
                        dc.SubmitChanges();
                    }
                }
                if (column.BeginOfRange.HasValue && column.EndOfRange.HasValue)
                {
                    if (!dc.Messages.Any(x => x.Name == appPrefs.Required + "_" + column.BeginOfRange + "_" + column.EndOfRange))
                    {
                        var obj = new NMG.Core.DataManager.DataContext.Message
                        {
                            Type = 2,
                            Name = appPrefs.RangeError,
                            FaName = "محدوده {0} " + column.BeginOfRange + " و " + column.EndOfRange + " است",
                            EnName = appPrefs.Required + "_" + column.BeginOfRange + "_" + column.EndOfRange + "{0}{1}"
                        };
                        dc.Messages.InsertOnSubmit(obj);
                        dc.SubmitChanges();
                    }
                }
                if (!string.IsNullOrEmpty(column.RegX))
                {
                    if (!dc.Messages.Any(x => x.Name == appPrefs.RegxError))
                    {
                        var obj = new NMG.Core.DataManager.DataContext.Message
                        {
                            Type = 2,
                            Name = appPrefs.RegxError,
                            FaName = "فرمت {0} اشتباه است",
                            EnName = appPrefs.RegxError + "{0}",
                        };
                        dc.Messages.InsertOnSubmit(obj);
                        dc.SubmitChanges();
                    }
                }

            }
        }

        public static void GenerateMessageResources(string FolderPath, string CommonResourseName, string ErrorResourse, string HeaderResource, string ResourceReference, string DatabaseName, List<Table> tables)
        {
            var dc = MetaDataGenerator.GetMetaDataGeneratorDC();
            var errorEntries = new Dictionary<string, string>();
            var messageEntries = new Dictionary<string, string>();
            var tablHeaderEntries = new Dictionary<string, string>();
            foreach (var item in dc.Messages.Where(x => x.Type == 1))
            {
                if (!messageEntries.ContainsKey(item.Name))
                {
                    if (!string.IsNullOrEmpty(item.FaName))
                    {
                        messageEntries.Add(item.Name, item.FaName);
                    }
                    else
                    {
                        messageEntries.Add(item.Name, item.Name);
                    }
                }
            }

            foreach (var item in dc.Messages.Where(x => x.Type == 2))
            {
                if (!errorEntries.ContainsKey(item.Name))
                {
                    if (!string.IsNullOrEmpty(item.FaName))
                    {
                        errorEntries.Add(item.Name, item.FaName);
                    }
                    else
                    {
                        errorEntries.Add(item.Name, item.Name);
                    }
                }
            }

            foreach (var table in tables)
            {
                foreach (var column in table.Columns.Where(x => x.InList))
                {
                    if (!(column.IsPrimaryKey || column.IsForeignKey))
                    {
                        if (!tablHeaderEntries.ContainsKey(column.Name))
                        {
                            if (!string.IsNullOrEmpty(column.PersianName))
                            {
                                tablHeaderEntries.Add(column.Name, column.PersianName);
                            }
                            else
                            {
                                tablHeaderEntries.Add(column.Name, column.Name);
                            }
                        }
                    }
                }

                foreach (var fk in table.ForeignKeys)
                {
                    if (table.Columns.Any(x => x.Name == fk.Name && x.InList))
                    {
                        if (!tablHeaderEntries.ContainsKey(fk.UniquePropertyName))
                        {
                            var fa = table.Columns.First(x => x.Name == fk.Name).PersianName;
                            if (!string.IsNullOrEmpty(fa))
                            {                                
                                tablHeaderEntries.Add(fk.UniquePropertyName, fa);
                            }
                            else
                            {
                                tablHeaderEntries.Add(fk.UniquePropertyName, fk.Name);
                            }
                        }
                    }
                }
            }     
     
            using (var writer = new ResXResourceWriter(string.Format(@"{0}\Resource\{1}.resx", FolderPath, CommonResourseName)))
            {
                foreach (var entry in messageEntries)
                {
                    writer.AddResource(entry.Key.ToString(), entry.Value);
                }
                writer.Generate();
                writer.Close();
                writer.Dispose();
            }

            if (tablHeaderEntries.Count > 0)
            {
                using (var writer = new ResXResourceWriter(string.Format(@"{0}\Resource\{1}.resx", FolderPath, HeaderResource)))
                {
                    foreach (var entry in tablHeaderEntries)
                    {
                        writer.AddResource(entry.Key.ToString(), entry.Value);
                    }
                    writer.Generate();
                    writer.Close();
                    writer.Dispose();
                }
            }

            using (var writer = new ResXResourceWriter(string.Format(@"{0}\Resource\{1}.resx", FolderPath, ErrorResourse)))
            {                
                foreach (var entry in errorEntries)
                {
                    writer.AddResource(entry.Key.ToString(), entry.Value);                   
                }
                writer.Generate();
                writer.Close();
                writer.Dispose();
            }          

            //string[] unmatchedElements;
            //var codeProvider = new CSharpCodeProvider();
            //var code = System.Resources.Tools.StronglyTypedResourceBuilder.Create(string.Format(@"{0}\Resource\{1}.resx", FolderPath, ErrorResourse), ErrorResourse, ResourceReference, codeProvider, true, out unmatchedElements);
            ////using (StreamWriter writer = new StreamWriter(string.Format(@"{0}\Resource\{1}.Designer.cs", FolderPath, ErrorResourse), false, System.Text.Encoding.UTF8))
            ////{
            ////    codeProvider.GenerateCodeFromCompileUnit(code, writer,
            ////        new System.CodeDom.Compiler.CodeGeneratorOptions());
            ////}
            //codeProvider = new CSharpCodeProvider();
            //code = System.Resources.Tools.StronglyTypedResourceBuilder.Create(string.Format(@"{0}\Resource\{1}.resx", FolderPath, CommonResourseName), CommonResourseName, ResourceReference, codeProvider, true, out unmatchedElements);
            ////using (StreamWriter writer = new StreamWriter(string.Format(@"{0}\Resource\{1}.Designer.cs", FolderPath, CommonResourseName), false, System.Text.Encoding.UTF8))
            ////{
            ////    codeProvider.GenerateCodeFromCompileUnit(code, writer,
            ////        new System.CodeDom.Compiler.CodeGeneratorOptions());
            ////}
            //if (tablHeaderEntries.Count > 0)
            //{
            //    codeProvider = new CSharpCodeProvider();
            //    code = System.Resources.Tools.StronglyTypedResourceBuilder.Create(string.Format(@"{0}\Resource\{1}.resx", FolderPath, HeaderResource), HeaderResource, ResourceReference, codeProvider, true, out unmatchedElements);
            //    using (StreamWriter writer = new StreamWriter(string.Format(@"{0}\Resource\{1}.Designer.cs", FolderPath, HeaderResource), false, System.Text.Encoding.UTF8))
            //    {
            //        codeProvider.GenerateCodeFromCompileUnit(code, writer,
            //            new System.CodeDom.Compiler.CodeGeneratorOptions());
            //    }
            //}
        }

    }
}
