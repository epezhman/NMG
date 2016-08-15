using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;


namespace NMG.DAL
{
    
    
    public partial class Table {
        
        public Table() {
			ColumnTableIds = new HashedSet<Column>();
        }
        
        public virtual int Id { get; set; }
        
        public virtual Database Database { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual Iesi.Collections.Generic.ISet<Column> ColumnTableIds { get; set; }
    }
}
