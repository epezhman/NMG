using System;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Iesi.Collections.Generic;
using NHibernate.Mapping;


namespace NMG.DAL
{
    
    
    public partial class Column {
        
        public Column() { }
        
        public virtual int Id { get; set; }
        
        public virtual Table Table { get; set; }
        
        public virtual string Name { get; set; }
        
        public virtual string CSharpType { get; set; }
        
        public virtual bool PrimaryKey { get; set; }
        
        public virtual bool ForeignKey { get; set; }
        
        public virtual bool Nullable { get; set; }
        
        public virtual bool UniqueKey { get; set; }
        
        public virtual bool InList { get; set; }
        
        public virtual bool InSort { get; set; }
        
        public virtual bool InSearch { get; set; }
        
        public virtual bool InLookUpTable { get; set; }
        
        public virtual bool InLookUpCombo { get; set; }
        
        public virtual string Regx { get; set; }
        
        public virtual System.Nullable<int> BeginRange { get; set; }
        
        public virtual System.Nullable<int> EndRange { get; set; }
        
        public virtual string PersianName { get; set; }
        
        public virtual System.Nullable<int> DataLength { get; set; }
        
        public virtual string ConstraintName { get; set; }
    }
}
