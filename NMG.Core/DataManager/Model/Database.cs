using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;


namespace NMG.DAL
{
    
    
    public partial class Database {
        
        public Database() {
			TableDatabaseIds = new HashedSet<Table>();
        }
        
        public virtual int Id { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual string ConnectionString { get; set; }
        
        public virtual Iesi.Collections.Generic.ISet<Table> TableDatabaseIds { get; set; }
    }
}
