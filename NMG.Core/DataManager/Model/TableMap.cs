using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;

namespace NMG.DAL
{
    
    
    public partial class TableMap : ClassMap<Table> {
        
        public TableMap() {
			Table("Table");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			References(x => x.Database).Column("DatabaseId");
			Map(x => x.Name).Column("Name").Not.Nullable().Length(200);
			HasMany(x => x.ColumnTableIds).KeyColumn("TableId");
        }
    }
}
