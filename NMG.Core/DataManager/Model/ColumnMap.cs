using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;


namespace NMG.DAL
{
    
    
    public partial class ColumnMap : ClassMap<Column> {
        
        public ColumnMap() {
			Table("Column");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			References(x => x.Table).Column("TableId");
			Map(x => x.Name).Column("Name").Not.Nullable().Length(200);
			Map(x => x.CSharpType).Column("CSharpType").Not.Nullable().Length(200);
			Map(x => x.PrimaryKey).Column("PrimaryKey").Not.Nullable();
			Map(x => x.ForeignKey).Column("ForeignKey").Not.Nullable();
			Map(x => x.Nullable).Column("Nullable").Not.Nullable();
			Map(x => x.UniqueKey).Column("UniqueKey").Not.Nullable();
			Map(x => x.InList).Column("InList").Not.Nullable();
			Map(x => x.InSort).Column("InSort").Not.Nullable();
			Map(x => x.InSearch).Column("InSearch").Not.Nullable();
			Map(x => x.InLookUpTable).Column("InLookUpTable").Not.Nullable();
			Map(x => x.InLookUpCombo).Column("InLookUpCombo").Not.Nullable();
			Map(x => x.Regx).Column("Regx").Length(500);
			Map(x => x.BeginRange).Column("BeginRange");
			Map(x => x.EndRange).Column("EndRange");
			Map(x => x.PersianName).Column("PersianName").Length(250);
			Map(x => x.DataLength).Column("DataLength");
			Map(x => x.ConstraintName).Column("ConstraintName").Length(250);
        }
    }
}
