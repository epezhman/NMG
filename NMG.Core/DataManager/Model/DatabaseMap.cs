using System; 
using System.Collections.Generic; 
using System.Text; 
using FluentNHibernate.Mapping;

namespace NMG.DAL
{
    
    
    public partial class DatabaseMap : ClassMap<Database> {
        
        public DatabaseMap() {
			Table("Database");
			LazyLoad();
			Id(x => x.Id).GeneratedBy.Identity().Column("Id");
			Map(x => x.Name).Column("Name").Not.Nullable().Length(200);
			Map(x => x.ConnectionString).Column("ConnectionString").Length(500);
			HasMany(x => x.TableDatabaseIds).KeyColumn("DatabaseId");
        }
    }
}
