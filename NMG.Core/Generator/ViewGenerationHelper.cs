using System;
using System.IO;
using System.Linq;
using NMG.Core.Domain;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace NMG.Core.Generator
{
    public class ViewGenerationHelper
    {

        public string GnerateEditCreate(Table table, string typeOfView, ApplicationPreferences appPref, string className, bool bootstrap = false)
        {
            var view = string.Empty;
            var tableRow = string.Empty;
            var tableRowFKey = string.Empty;
            var tableRowNumber = string.Empty;
            var tableRowLookUp = string.Empty;
            var tableRowForeign = string.Empty;
            var dropDown = string.Empty;
            if (bootstrap)
            {
                if (typeOfView == "edit")
                {
                    if (appPref.UseAjax)
                    {
                        using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\EditBootstrapTemplate.cshtml"))
                        {
                            view = fileStream.ReadToEnd();
                        }
                    }
                    else
                    {
                        using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\EditTemplateBootstrapNoDialog.cshtml"))
                        {
                            view = fileStream.ReadToEnd();
                        }
                    }
                }
                else
                {
                    if (appPref.UseAjax)
                    {
                        using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\CreateBootstrapTemplate.cshtml"))
                        {
                            view = fileStream.ReadToEnd();
                        }
                    }
                    else
                    {
                        using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\CreateBootstrapTemplateNoDialog.cshtml"))
                        {
                            view = fileStream.ReadToEnd();
                        }
                    }
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowBootstrap.cshtml"))
                {
                    tableRow = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\tableRowBootstrapFKey.cshtml"))
                {
                    tableRowFKey = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowBootstrapNumber.cshtml"))
                {
                    tableRowNumber = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowBootstrapLookUp.cshtml"))
                {
                    tableRowLookUp = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowBootstrapForiegn.cshtml"))
                {
                    tableRowForeign = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableDropDownBootstrap.cshtml"))
                {
                    dropDown = fileStream.ReadToEnd();
                }
            }
            else
            {
                if (typeOfView == "edit")
                {
                    if (appPref.UseAjax)
                    {
                        using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\EditTemplate.cshtml"))
                        {
                            view = fileStream.ReadToEnd();
                        }
                    }
                    else
                    {
                        using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\EditTemplateNoDialog.cshtml"))
                        {
                            view = fileStream.ReadToEnd();
                        }
                    }
                }
                else
                {
                    if (appPref.UseAjax)
                    {
                        using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\CreateTemplate.cshtml"))
                        {
                            view = fileStream.ReadToEnd();
                        }
                    }
                    else
                    {
                        using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\CreateTemplateNoDialog.cshtml"))
                        {
                            view = fileStream.ReadToEnd();
                        }
                    }
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRow.cshtml"))
                {
                    tableRow = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowFKey.cshtml"))
                {
                    tableRowFKey = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowNumber.cshtml"))
                {
                    tableRowNumber = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowLookUp.cshtml"))
                {
                    tableRowLookUp = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowForiegn.cshtml"))
                {
                    tableRowForeign = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableDropDown.cshtml"))
                {
                    dropDown = fileStream.ReadToEnd();
                }
            }
            var tableBody = new StringBuilder();
            foreach (var column in table.Columns.Where(x => x.DataType != "uniqueidentifier"))
            {
                if (!(column.IsForeignKey))
                {
                    if (column.DataType.EndsWith("int") && column.IsPrimaryKey && !column.IdentitySpecification && typeOfView == "create")
                    {
                        tableBody.AppendLine(string.Format(tableRowNumber, column.Name));
                    }
                    else if (column.IsPrimaryKey && !column.IdentitySpecification && typeOfView == "create")
                    {
                        tableBody.AppendLine(string.Format(tableRow, column.Name));
                    }
                    else if (column.DataType.EndsWith("int") && !column.IsPrimaryKey)
                    {
                        tableBody.AppendLine(string.Format(tableRowNumber, column.Name));
                    }
                    else if ((column.DataType == "datetime" || column.DataType == "date") && column.Name.ToLower() == "createdate" && appPref.SystemDefaults && !column.IsPrimaryKey)
                    {
                        //Skip this one
                    }
                    else if (!column.IsPrimaryKey)
                    {
                        tableBody.AppendLine(string.Format(tableRow, column.Name));
                    }

                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (appPref.ORMView == 1)
                    {
                        if (fk.References != className)
                        {
                            var tableInf = MetaDataGenerator.GetAvailableLookUpCombos(table.DatabaseName, fk.UniquePropertyName);
                            if (string.IsNullOrEmpty(tableInf))
                            {
                                //tableBody.AppendLine(string.Format(tableRowFKey, fk.UniquePropertyName, fk.UniquePropertyName + ".Id"));
                                tableBody.AppendLine(string.Format(dropDown, fk.UniquePropertyName, fk.Name, fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName));
                            }
                            else
                            {
                                //tableBody.AppendLine(string.Format(tableRowLookUp, fk.UniquePropertyName, "Get" + fk.UniquePropertyName + tableInf + "List", fk.UniquePropertyName, string.Empty));
                                tableBody.AppendLine(string.Format(dropDown, fk.UniquePropertyName, fk.Name, fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName));
                            }
                        }
                        else
                        {
                            //tableBody.AppendLine(string.Format(tableRowLookUp, ("Parent" + fk.UniquePropertyName), "Get" + fk.UniquePropertyName + "List", fk.UniquePropertyName, string.Empty));
                            tableBody.AppendLine(string.Format(dropDown, ("Parent" + fk.UniquePropertyName), fk.Name, fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName));

                        }

                    }
                    else if (appPref.ORMView == 2)
                    {
                        if (fk.References != className)
                        {
                            var tableInf = MetaDataGenerator.GetAvailableLookUpCombos(table.DatabaseName, fk.UniquePropertyName);
                            if (string.IsNullOrEmpty(tableInf))
                            {
                                //tableBody.AppendLine(string.Format(tableRowFKey, fk.UniquePropertyName, fk.UniquePropertyName + ".Id"));
                                tableBody.AppendLine(string.Format(dropDown, fk.UniquePropertyName, fk.UniquePropertyName + ".Id", fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName));
                            }
                            else
                            {
                                //tableBody.AppendLine(string.Format(tableRowLookUp, fk.UniquePropertyName, "Get" + fk.UniquePropertyName + tableInf + "List", fk.UniquePropertyName, string.Empty));
                                tableBody.AppendLine(string.Format(dropDown, fk.UniquePropertyName, fk.UniquePropertyName + ".Id", fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName));
                            }
                        }
                        else
                        {
                            //tableBody.AppendLine(string.Format(tableRowLookUp, ("Parent" + fk.UniquePropertyName), "Get" + fk.UniquePropertyName + "List", fk.UniquePropertyName, string.Empty));
                            tableBody.AppendLine(string.Format(dropDown, ("Parent" + fk.UniquePropertyName), "Parent" + fk.UniquePropertyName + ".Id", fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName));

                        }
                    }

                }
            }
            if (typeOfView == "create")
            {
                view = string.Format(view, appPref.NameSpace + "." + className, "Create", className, string.Empty, tableBody, appPref.ResourceReference, appPref.CommonResourseName, string.Empty);
            }
            if (typeOfView == "edit")
            {
                var IdName = string.Empty;
                if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
                {
                    IdName = string.Format("@Html.HiddenFor(model => model.{0})", table.Columns.First(x => x.DataType == "uniqueidentifier").Name);
                }
                else
                {
                    IdName = string.Format("@Html.HiddenFor(model => model.{0})", table.PrimaryKey.Columns.First().Name);
                }
                view = string.Format(view, appPref.NameSpace + "." + className, "Edit", className, string.Empty, tableBody, appPref.ResourceReference, appPref.CommonResourseName, IdName);
            }
            return view;
        }

        public string GnerateDelete(Table table, string typeOfView, ApplicationPreferences appPref, string className, bool bootstrap = false)
        {
            var view = string.Empty;
            var tableRow = string.Empty;
            var tableRowLookUp = string.Empty;
            if (bootstrap)
            {
                if (appPref.UseAjax)
                {
                    using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\DeleteBootstrapTemplate.cshtml"))
                    {
                        view = fileStream.ReadToEnd();
                    }
                }
                else
                {
                    using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\DeleteBootstrapTemplateNoUI.cshtml"))
                    {
                        view = fileStream.ReadToEnd();
                    }
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowBootstrapDisplay.cshtml"))
                {
                    tableRow = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowBootstrapLookUp.cshtml"))
                {
                    tableRowLookUp = fileStream.ReadToEnd();
                }
            }
            else
            {
                if (appPref.UseAjax)
                {
                    using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\DeleteTemplate.cshtml"))
                    {
                        view = fileStream.ReadToEnd();
                    }
                }
                else
                {
                    using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\DeleteTemplateNoUI.cshtml"))
                    {
                        view = fileStream.ReadToEnd();
                    }
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowDisplay.cshtml"))
                {
                    tableRow = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableRowDisplayLookUp.cshtml"))
                {
                    tableRowLookUp = fileStream.ReadToEnd();
                }
            }
            var tableBody = new StringBuilder();
            foreach (var column in table.Columns.Where(x => x.DataType != "uniqueidentifier"))
            {
                if (!(column.IsPrimaryKey || column.IsForeignKey))
                {
                    tableBody.AppendLine(string.Format(tableRow, column.Name));
                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (!string.IsNullOrEmpty(fk.References))
                {
                    if (fk.References != className)
                    {
                        var tableInf = MetaDataGenerator.GetAvailableLookUpLabels(table.DatabaseName, fk.UniquePropertyName);
                        if (string.IsNullOrEmpty(tableInf))
                        {
                            //tableBody.AppendLine(string.Format(tableRow, fk.UniquePropertyName, "Parent" + fk.UniquePropertyName + ".Id"));
                            if (string.IsNullOrEmpty(tableInf))
                            {
                                tableBody.AppendLine(string.Format(tableRowLookUp, fk.UniquePropertyName, "Get" + fk.References + "Title", fk.References, string.Empty, fk.UniquePropertyName + ".Id"));
                            }
                            else
                            {
                                tableBody.AppendLine(string.Format(tableRowLookUp, fk.UniquePropertyName, "Get" + fk.References + tableInf, fk.References, string.Empty, fk.UniquePropertyName + ".Id"));
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(tableInf))
                            {
                                tableBody.AppendLine(string.Format(tableRowLookUp, fk.UniquePropertyName, "Get" + fk.UniquePropertyName + "Title", fk.UniquePropertyName, string.Empty, fk.UniquePropertyName + ".Id"));
                            }
                            else
                            {
                                tableBody.AppendLine(string.Format(tableRowLookUp, fk.UniquePropertyName, "Get" + fk.UniquePropertyName + tableInf, fk.UniquePropertyName, string.Empty, fk.UniquePropertyName + ".Id"));
                            }
                        }
                    }
                    else
                    {
                        tableBody.AppendLine(string.Format(tableRowLookUp, "Parent" + fk.UniquePropertyName, "Get" + fk.UniquePropertyName + "Title", fk.UniquePropertyName, string.Empty, "Parent" + fk.UniquePropertyName + ".Id"));
                    }
                }
            }
            var IdName = string.Empty;
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                IdName = string.Format("@Html.HiddenFor(model => model.{0})", table.Columns.First(x => x.DataType == "uniqueidentifier").Name);
            }
            else
            {
                IdName = string.Format("@Html.HiddenFor(model => model.{0})", table.PrimaryKey.Columns.First().Name);
            }
            view = string.Format(view, appPref.NameSpace + "." + className, "Delete", className, string.Empty, appPref.ResourceReference, appPref.CommonResourseName, tableBody, IdName);
            return view;
        }

        public string GnerateLookUpCombo(string tableName, string columnName)
        {
            var view = string.Empty;
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\DropDown.cshtml"))
            {
                view = fileStream.ReadToEnd();
            }
            view = string.Format(view, tableName + columnName, columnName);
            return view;
        }

        public string GnerateIndexView(Table table, ApplicationPreferences appPref, string className, bool bootstarp = false)
        {
            var view = string.Empty;
            if (bootstarp)
            {
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\IndexBootstrap.cshtml"))
                {
                    view = fileStream.ReadToEnd();
                }
            }
            else
            {
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\Index.cshtml"))
                {
                    view = fileStream.ReadToEnd();
                }
            }
            view = string.Format(view, appPref.NameSpace + "." + className + "SearchModel", className, string.Empty, className + "SearchModelView", appPref.ResourceReference, appPref.CommonResourseName);
            return view;
        }


        public string GnerateIndexKendoView(Table table, ApplicationPreferences appPref, string className)
        {
            var view = string.Empty;
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\IndexKendo.cshtml"))
            {
                view = fileStream.ReadToEnd();
            }
            view = string.Format(view, appPref.NameSpace + "." + className + "SearchModel", className, string.Empty, className + "SearchModelView", appPref.ResourceReference, appPref.CommonResourseName);
            return view;
        }

        public string GnerateIXView(Table table, ApplicationPreferences appPref, string className, bool bootstap = false)
        {
            var view = string.Empty;
            var tableHeaderTemplate = string.Empty;
            var tableBodyTamplate = string.Empty;
            var tableCellLookUpTamplate = string.Empty;
            var tableBodyTimeTamplate = string.Empty;
            var tableHeader = new StringBuilder();
            var tableBody = new StringBuilder();
            if (bootstap)
            {
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\IXBootstrap.cshtml"))
                {
                    view = fileStream.ReadToEnd();
                }
            }
            else
            {
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\IX.cshtml"))
                {
                    view = fileStream.ReadToEnd();
                }
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableHeader.cshtml"))
            {
                tableHeaderTemplate = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableCell.cshtml"))
            {
                tableBodyTamplate = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableCellTime.cshtml"))
            {
                tableBodyTimeTamplate = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableCellLookUpLabel.cshtml"))
            {
                tableCellLookUpTamplate = fileStream.ReadToEnd();
            }
            foreach (var column in table.Columns.Where(x => x.InList))
            {
                if (!(column.IsPrimaryKey || column.IsForeignKey))
                {
                    tableHeader.AppendLine(string.Format(tableHeaderTemplate, appPref.ResourceReference, column.Name, table.Name));
                    if (column.DataType == "date" || column.DataType == "datetime")
                    {
                        if (column.IsNullable)
                        {
                            tableBody.AppendLine(string.Format(tableBodyTimeTamplate, column.Name + ".Value.ToPersianDate()", column.Name));
                        }
                        else
                        {
                            tableBody.AppendLine(string.Format(tableBodyTimeTamplate, column.Name + ".ToPersianDate()", column.Name));
                        }

                    }
                    else
                    {
                        tableBody.AppendLine(string.Format(tableBodyTamplate, column.Name));
                    }
                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (table.Columns.Any(x => x.Name == fk.Name && x.InList))
                {
                    tableHeader.AppendLine(string.Format(tableHeaderTemplate, appPref.ResourceReference, fk.UniquePropertyName, table.Name));
                    var lookup = MetaDataGenerator.GetAvailableLookUpLabels(table.DatabaseName, fk.References);
                    if (string.IsNullOrEmpty(lookup))
                    {
                        tableBody.AppendLine(string.Format(tableBodyTamplate, fk.UniquePropertyName + "Id"));
                    }
                    else
                    {
                        tableBody.AppendLine(string.Format(tableCellLookUpTamplate, "Get" + fk.References + lookup, fk.References, string.Empty));
                    }
                }
            }
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                view = string.Format(view, appPref.NameSpace + "." + className, tableHeader, tableBody, className, appPref.ResourceReference, appPref.CommonResourseName, string.Empty, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, "@using " + appPref.ControllerNamespace.Substring(0, appPref.ControllerNamespace.LastIndexOf('.')), appPref.NameSpace);
            }
            else
            {
                view = string.Format(view, appPref.NameSpace + "." + className, tableHeader, tableBody, className, appPref.ResourceReference, appPref.CommonResourseName, string.Empty, "Id", "@using " + appPref.ControllerNamespace.Substring(0, appPref.ControllerNamespace.LastIndexOf('.')), appPref.NameSpace);
            }
            return view;
        }

        public string GnerateIXFlexiView(Table table, ApplicationPreferences appPref, string className)
        {
            var view = string.Empty;
            var tableHeaderTemplate = string.Empty;
            var tableBodyTamplate = string.Empty;
            var tableBodyTimeTamplate = string.Empty;
            var tableCellLookUpTamplate = string.Empty;
            var tableHeader = new StringBuilder();
            var tableBody = new StringBuilder();
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\IXFelixigrid.cshtml"))
            {
                view = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableHeader.cshtml"))
            {
                tableHeaderTemplate = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableCell.cshtml"))
            {
                tableBodyTamplate = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableCellTime.cshtml"))
            {
                tableBodyTimeTamplate = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableCellLookUpLabel.cshtml"))
            {
                tableCellLookUpTamplate = fileStream.ReadToEnd();
            }
            foreach (var column in table.Columns.Where(x => x.InList))
            {
                if (!(column.IsPrimaryKey || column.IsForeignKey))
                {
                    tableHeader.AppendLine(string.Format(tableHeaderTemplate, appPref.ResourceReference, column.Name, table.Name));
                    if (column.DataType == "date" || column.DataType == "datetime")
                    {
                        if (column.IsNullable)
                        {
                            tableBody.AppendLine(string.Format(tableBodyTimeTamplate, column.Name + ".Value.ToPersianDate()", column.Name));
                        }
                        else
                        {
                            tableBody.AppendLine(string.Format(tableBodyTimeTamplate, column.Name + ".ToPersianDate()", column.Name));
                        }
                    }
                    else
                    {
                        tableBody.AppendLine(string.Format(tableBodyTamplate, column.Name));
                    }
                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (table.Columns.Any(x => x.Name == fk.Name && x.InList))
                {
                    tableHeader.AppendLine(string.Format(tableHeaderTemplate, appPref.ResourceReference, fk.UniquePropertyName, table.Name));
                    var lookup = MetaDataGenerator.GetAvailableLookUpLabels(table.DatabaseName, fk.References);
                    if (string.IsNullOrEmpty(lookup))
                    {
                        tableBody.AppendLine(string.Format(tableBodyTamplate, fk.UniquePropertyName + "Id"));
                    }
                    else
                    {
                        tableBody.AppendLine(string.Format(tableCellLookUpTamplate, "Get" + fk.References + lookup, fk.References, string.Empty));
                    }
                }
            }
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                view = string.Format(view, appPref.NameSpace + "." + className, tableHeader, tableBody, className, appPref.ResourceReference, appPref.CommonResourseName, string.Empty, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, "@using " + appPref.ControllerNamespace.Substring(0, appPref.ControllerNamespace.LastIndexOf('.')), appPref.NameSpace);
            }
            else
            {
                view = string.Format(view, appPref.NameSpace + "." + className, tableHeader, tableBody, className, appPref.ResourceReference, appPref.CommonResourseName, string.Empty, "Id", "@using " + appPref.ControllerNamespace.Substring(0, appPref.ControllerNamespace.LastIndexOf('.')), appPref.NameSpace);
            }
            return view;
        }

        public string GnerateIXKnedoView(Table table, ApplicationPreferences appPref, string className)
        {
            var view = string.Empty;
            var tableHeaderTemplate = string.Empty;
            var columnTemplate = string.Empty;
            var columnSchemaTemplate = string.Empty;
            var columnBody = new StringBuilder();
            var columnSchemaBody = new StringBuilder();
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\IXKendo.cshtml"))
            {
                view = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\KendoColumn.cshtml"))
            {
                columnTemplate = fileStream.ReadToEnd();
            }
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\KendoSchema.cshtml"))
            {
                columnSchemaTemplate = fileStream.ReadToEnd();
            }
            foreach (var column in table.Columns.Where(x => x.InList))
            {
                if (!(column.IsPrimaryKey || column.IsForeignKey))
                {
                    if (column.DataType == "date" || column.DataType == "datetime")
                    {
                        columnBody.AppendLine(string.Format(columnTemplate, column.Name, column.Name));
                    }
                    else
                    {
                        columnBody.AppendLine(string.Format(columnTemplate, column.Name, column.Name));
                    }
                    columnSchemaBody.AppendLine(string.Format(columnSchemaTemplate, column.Name));
                }
            }
            foreach (var fk in table.ForeignKeys)
            {
                if (table.Columns.Any(x => x.Name == fk.Name && x.InList))
                {

                }
            }
            if (table.Columns.Any(x => x.DataType == "uniqueidentifier"))
            {
                view = string.Format(view, appPref.NameSpace + "." + className + "SearchModel", columnBody.ToString(), className, table.Columns.First(x => x.DataType == "uniqueidentifier").Name, columnSchemaBody.ToString());
            }
            else
            {
                view = string.Format(view, appPref.NameSpace + "." + className + "SearchModel", columnBody.ToString(), className, "Id", columnSchemaBody.ToString());
            }
            return view;
        }

        public string GnerateSearchView(Table table, ApplicationPreferences appPref, string className, bool bootstrap = false)
        {
            var view = string.Empty;
            var tableBody = new StringBuilder();
            var tableCell = string.Empty;
            var tableCellNum = string.Empty;
            var tableRow = string.Empty;
            if (bootstrap)
            {
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\SearchBootstrapModelView.cshtml"))
                {
                    view = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\SearchBootstrapCell.cshtml"))
                {
                    tableCell = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\SearchBootstrapCellNumber.cshtml"))
                {
                    tableCellNum = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\SearchBootstrapRow.cshtml"))
                {
                    tableRow = fileStream.ReadToEnd();
                }
            }
            else
            {
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\SearchModelView.cshtml"))
                {
                    view = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\SearchTableCell.cshtml"))
                {
                    tableCell = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\SearchTableCellNumber.cshtml"))
                {
                    tableCellNum = fileStream.ReadToEnd();
                }
                using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\SearchTableRow.cshtml"))
                {
                    tableRow = fileStream.ReadToEnd();
                }
            }
            var tb1 = string.Empty; var tb2 = string.Empty; var tb3 = string.Empty;
            var flag = true;
            int turning = 1;
            for (int i = 0; i < table.Columns.Where(x => x.InSearch).Count(); i++)
            {
                if (i % 3 == 0)
                {
                    if (i != 0)
                    {
                        tableBody.AppendLine(string.Format(tableRow, tb1, tb2, tb3));
                        flag = false;
                    }
                    if (table.Columns.Where(x => x.InSearch).ToList()[i].DataType.EndsWith("int"))
                    {
                        tb1 = GetSearchCellString(tableCellNum, table.Columns.Where(x => x.InSearch).ToList()[i], table, appPref, className);
                        flag = true;
                        turning = 1;
                    }
                    else
                    {
                        tb1 = GetSearchCellString(tableCell, table.Columns.Where(x => x.InSearch).ToList()[i], table, appPref, className);
                        flag = true;
                        turning = 1;

                    }
                }
                else if (i % 3 == 1)
                {
                    if (table.Columns.Where(x => x.InSearch).ToList()[i].DataType.EndsWith("int"))
                    {
                        tb2 = GetSearchCellString(tableCellNum, table.Columns.Where(x => x.InSearch).ToList()[i], table, appPref, className);
                        flag = true;
                        turning = 2;
                    }
                    else
                    {
                        tb2 = GetSearchCellString(tableCell, table.Columns.Where(x => x.InSearch).ToList()[i], table, appPref, className);
                        flag = true;
                        turning = 2;
                    }
                }
                else if (i % 3 == 2)
                {
                    if (table.Columns.Where(x => x.InSearch).ToList()[i].DataType.EndsWith("int"))
                    {
                        tb3 = GetSearchCellString(tableCellNum, table.Columns.Where(x => x.InSearch).ToList()[i], table, appPref, className);
                        flag = true;
                        turning = 3;
                    }
                    else
                    {
                        tb3 = GetSearchCellString(tableCell, table.Columns.Where(x => x.InSearch).ToList()[i], table, appPref, className);
                        flag = true;
                        turning = 3;
                    }
                }
            }
            if (flag)
            {
                if (turning == 1)
                {
                    tableBody.AppendLine(string.Format(tableRow, tb1, string.Empty, string.Empty));
                }
                else if (turning == 1)
                {
                    tableBody.AppendLine(string.Format(tableRow, tb1, tb2, string.Empty));
                }
                else if (turning == 1)
                {
                    tableBody.AppendLine(string.Format(tableRow, tb1, tb2, tb3));
                }
            }
            view = string.Format(view, appPref.NameSpace + "." + className + "SearchModel", tableBody, appPref.ResourceReference, appPref.CommonResourseName);
            return view;
        }

        public string GetSearchCellString(string cell, Column column, Table table, ApplicationPreferences appPref, string className)
        {
            var dropDown = string.Empty;
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\TableDropDown.cshtml"))
            {
                dropDown = fileStream.ReadToEnd();
            }
            return string.Format(cell, column.Name);
            if (!column.IsForeignKey)
            {
                return string.Format(cell, column.Name);
            }
            else
            {
                var fk = table.ForeignKeys.First(x => x.Name == column.Name);
                if (fk.References != className)
                {
                    var tableInf = MetaDataGenerator.GetAvailableLookUpCombos(table.DatabaseName, fk.UniquePropertyName);
                    if (string.IsNullOrEmpty(tableInf))
                    {
                        return string.Format(dropDown, fk.UniquePropertyName, fk.UniquePropertyName + ".Id", fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName);
                    }
                    else
                    {
                        return string.Format(dropDown, fk.UniquePropertyName, fk.UniquePropertyName + ".Id", fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName);
                    }
                }
                else
                {
                    return string.Format(dropDown, ("Parent" + fk.UniquePropertyName), "Parent" + fk.UniquePropertyName + ".Id", fk.UniquePropertyName + "s", appPref.ResourceReference, appPref.CommonResourseName);

                }
            }
        }

        public static void GnerateMenuView(string filePath, string databaseName)
        {
            var dc = MetaDataGenerator.GetMetaDataGeneratorDC();
            var tableRow = string.Empty;
            using (var fileStream = new StreamReader(@"..\..\..\NMG.Core\Models\Link.cshtml"))
            {
                tableRow = fileStream.ReadToEnd();
            }
            var linkbody = new StringBuilder();
            foreach (var table in dc.Tables.Where(x => x.Database.Name == databaseName))
            {
                linkbody.AppendLine(string.Format(tableRow, table.Name, table.Name, string.Empty));
            }
            if (!Directory.Exists(string.Format(@"{0}\Views\{1}", filePath, "Shared")))
            {
                Directory.CreateDirectory(string.Format(@"{0}\Views\{1}", filePath, "Shared"));
            }
            string fileName = string.Format(@"{0}\Views\{1}\{2}.cshtml", filePath, "Shared", "Menu");
            using (var file = new StreamWriter(fileName))
            {
                file.Write(linkbody);
            }
        }


    }
}
