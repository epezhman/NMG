using NMG.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace NMG.Core.Generator
{

    public class MetaDataGenerator
    {
        public  static string ConnectionString
        {
            get
            {
                var s = ConfigurationManager.AppSettings["MetaDataDb"];
                if(string.IsNullOrEmpty(s))
                     s = "Data Source=192.168.15.164;Initial Catalog=NMGTableMetaData;User ID=sa;Password=nikimoshi";
                return s;
            }
        }

        public static NMG.Core.DataManager.DataContext.NMGTableMetaDataDataContext GetMetaDataGeneratorDC()
        {
            var dc = new NMG.Core.DataManager.DataContext.NMGTableMetaDataDataContext(ConnectionString);
            if (!dc.DatabaseExists())
            {
                dc.CreateDatabase();
                dc.Dispose();
                dc = new NMG.Core.DataManager.DataContext.NMGTableMetaDataDataContext(ConnectionString);
            }
            return dc;
        }
       
        public static Column UpdateColumnMetaData(Table table, Column column)
        {
            var dc = GetMetaDataGeneratorDC();
            if (dc.Columns.Any(x => x.Name == column.Name && x.Table.Name == table.Name && x.Table.Database.Name == table.DatabaseName))
            {
                var oldColumn = dc.Columns.First(x => x.Name == column.Name && x.Table.Name == table.Name && x.Table.Database.Name == table.DatabaseName);
                column.BeginOfRange = oldColumn.BeginRange;
                column.DataLength = oldColumn.DataLength;
                column.EndOfRange = oldColumn.EndRange;
                column.InList = oldColumn.InList;
                column.InLookUpCombo = oldColumn.InLookUpCombo;
                column.InLookUpLabel = oldColumn.InLookUpLabel;
                column.InSearch = oldColumn.InSearch;
                column.InSort = oldColumn.InSort;
                column.PersianName = oldColumn.PersianName;
                column.RegX = oldColumn.Regx;
                column.CheckRepeptiveError = oldColumn.CheckRepepetiveError;
            }
            return column;
        }

        public static void UpdateDatabaseMetaData(Table table, Column column)
        {
            var dc = GetMetaDataGeneratorDC();
            if (dc.Columns.Any(x => x.Name == column.Name && x.Table.Name == table.Name && x.Table.Database.Name == table.DatabaseName))
            {
                var oldColumn = dc.Columns.Single(x => x.Name == column.Name && x.Table.Name == table.Name && x.Table.Database.Name == table.DatabaseName);
                oldColumn.BeginRange = column.BeginOfRange;
                oldColumn.DataLength = column.DataLength;
                oldColumn.EndRange = column.EndOfRange;
                oldColumn.InList = column.InList;
                oldColumn.InLookUpCombo = column.InLookUpCombo;
                oldColumn.InLookUpLabel = column.InLookUpLabel;
                oldColumn.InSearch = column.InSearch;
                oldColumn.InSort = column.InSort;
                oldColumn.PersianName = column.PersianName;
                oldColumn.Regx = column.RegX;
                oldColumn.CheckRepepetiveError = oldColumn.CheckRepepetiveError;
                dc.SubmitChanges();
            }
            else
            {
                var database = new NMG.Core.DataManager.DataContext.Database();
                var dbTable = new NMG.Core.DataManager.DataContext.Table();
                if (dc.Databases.Any(x => x.Name == table.DatabaseName))
                {
                    database = dc.Databases.Single(x => x.Name == table.DatabaseName);
                }
                else
                {
                    database = new NMG.Core.DataManager.DataContext.Database
                    {
                        Name = table.DatabaseName
                    };
                    dc.Databases.InsertOnSubmit(database);
                    dc.SubmitChanges();
                }
                if (dc.Tables.Any(x => x.Name == table.Name))
                {
                    dbTable = dc.Tables.Single(x => x.Name == table.Name);
                }
                else
                {
                    dbTable = new NMG.Core.DataManager.DataContext.Table
                    {
                        Name = table.Name,
                        Database = database
                    };
                    dc.Tables.InsertOnSubmit(dbTable);
                    dc.SubmitChanges();
                }
                var dbColumn = new NMG.Core.DataManager.DataContext.Column
                {
                    BeginRange = column.BeginOfRange,
                    ConstraintName = column.ConstraintName,
                    CSharpType = column.DataType.ToString(),
                    DataLength = column.DataLength,
                    EndRange = column.EndOfRange,
                    ForeignKey = column.IsForeignKey,
                    InList = column.InSort,
                    InLookUpCombo = column.InLookUpCombo,
                    InLookUpLabel = column.InLookUpLabel,
                    InSearch = column.InSearch,
                    InSort = column.InSort,
                    Name = column.Name,
                    Nullable = column.IsNullable,
                    PersianName = column.PersianName,
                    PrimaryKey = column.IsPrimaryKey,
                    Regx = column.RegX,
                    Table = dbTable,
                    UniqueKey = column.IsUnique,
                    CheckRepepetiveError = column.CheckRepeptiveError
                };
                dc.Columns.InsertOnSubmit(dbColumn);
                dc.SubmitChanges();
            }

            return;
        }

        public static void UpdateAllDatabaseMetaData(Table table)
        {
            if (table.Columns.Count > 0)
            {
                var dc = GetMetaDataGeneratorDC();
                foreach (var column in table.Columns)
                {
                    if (dc.Columns.Any(x => x.Name == column.Name && x.Table.Name == table.Name && x.Table.Database.Name == table.DatabaseName))
                    {
                        var oldColumn = dc.Columns.Single(x => x.Name == column.Name && x.Table.Name == table.Name && x.Table.Database.Name == table.DatabaseName);
                        oldColumn.BeginRange = column.BeginOfRange;
                        oldColumn.DataLength = column.DataLength;
                        oldColumn.EndRange = column.EndOfRange;
                        oldColumn.InList = column.InList;
                        oldColumn.InLookUpCombo = column.InLookUpCombo;
                        oldColumn.InLookUpLabel = column.InLookUpLabel;
                        oldColumn.InSearch = column.InSearch;
                        oldColumn.InSort = column.InSort;
                        oldColumn.PersianName = column.PersianName;
                        oldColumn.Regx = column.RegX;
                        oldColumn.CheckRepepetiveError = column.CheckRepeptiveError;
                        dc.SubmitChanges();
                    }
                    else
                    {
                        var database = new NMG.Core.DataManager.DataContext.Database();
                        var dbTable = new NMG.Core.DataManager.DataContext.Table();
                        if (dc.Databases.Any(x => x.Name == table.DatabaseName))
                        {
                            database = dc.Databases.Single(x => x.Name == table.DatabaseName);
                        }
                        else
                        {
                            database = new NMG.Core.DataManager.DataContext.Database
                            {
                                Name = table.DatabaseName
                            };
                            dc.Databases.InsertOnSubmit(database);
                            dc.SubmitChanges();
                        }
                        if (dc.Tables.Any(x => x.Name == table.Name))
                        {
                            dbTable = dc.Tables.Single(x => x.Name == table.Name);
                        }
                        else
                        {
                            dbTable = new NMG.Core.DataManager.DataContext.Table
                            {
                                Name = table.Name,
                                Database = database
                            };
                            dc.Tables.InsertOnSubmit(dbTable);
                            dc.SubmitChanges();
                        }
                        var dbColumn = new NMG.Core.DataManager.DataContext.Column
                        {
                            BeginRange = column.BeginOfRange,
                            ConstraintName = column.ConstraintName,
                            CSharpType = column.DataType.ToString(),
                            DataLength = column.DataLength,
                            EndRange = column.EndOfRange,
                            ForeignKey = column.IsForeignKey,
                            InList = column.InList,
                            InLookUpCombo = column.InLookUpCombo,
                            InLookUpLabel = column.InLookUpLabel,
                            InSearch = column.InSearch,
                            InSort = column.InSort,
                            Name = column.Name,
                            Nullable = column.IsNullable,
                            PersianName = column.PersianName,
                            PrimaryKey = column.IsPrimaryKey,
                            Regx = column.RegX,
                            Table = dbTable,
                            UniqueKey = column.IsUnique,
                            CheckRepepetiveError = column.CheckRepeptiveError
                        };
                        dc.Columns.InsertOnSubmit(dbColumn);
                        dc.SubmitChanges();
                    }
                }
                return;
            }
        }

        public static string GetAvailableLookUpCombos(string databaseName, string tableName)
        {
            var dc = GetMetaDataGeneratorDC();
            if (dc.Columns.Any(x => x.Table.Name == tableName && x.Table.Database.Name == databaseName && x.InLookUpCombo))
            {
                var columnQuialified = dc.Columns.Where(x => x.Table.Name == tableName && x.Table.Database.Name == databaseName && x.InLookUpLabel);
                if (columnQuialified.Any(x => x.Name.ToLower() == "title"))
                {
                    return "Title";
                }
                else
                {
                    return columnQuialified.First().Name;
                }
            }
            return string.Empty;

        }

        public static string GetAvailableLookUpLabels(string databaseName, string tableName)
        {
            var dc = GetMetaDataGeneratorDC();
            if (dc.Columns.Any(x => x.Table.Name == tableName && x.Table.Database.Name == databaseName && x.InLookUpLabel))
            {
                var columnQuialified = dc.Columns.Where(x => x.Table.Name == tableName && x.Table.Database.Name == databaseName && x.InLookUpLabel);
                if (columnQuialified.Any(x => x.Name.ToLower() == "title"))
                {
                    return "Title";
                }
                else
                {
                    return columnQuialified.First().Name;
                }
            }
            return string.Empty;

        }
    }
}
