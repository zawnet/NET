//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HERBS_PRODUKCJA
{
    using System;
    using System.Collections.Generic;
    
    public partial class PRODDK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODDK()
        {
            this.PRODDP_FK = new HashSet<PRODDP_FK>();
            this.PRODDP = new HashSet<PRODDP>();
        }
    
        public int id { get; set; }
        public int id_dk { get; set; }
        public int id_prod { get; set; }
        public Nullable<float> ilosc_odpad { get; set; }
        public string tabela { get; set; }
        public string typ_dk { get; set; }
        public string kod_dk { get; set; }
        public Nullable<System.DateTime> data { get; set; }
    
        public virtual PROD PROD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODDP_FK> PRODDP_FK { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRODDP> PRODDP { get; set; }
    }
}