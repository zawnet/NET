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
    
    public partial class PRODDP_MONIT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODDP_MONIT()
        {
            this.GS1_KODY = new HashSet<GS1_KODY>();
            this.PROD_PRODMGPW = new HashSet<PROD_PRODMGPW>();
            this.PROD_MZ_SPEC = new HashSet<PROD_MZ_SPEC>();
        }
    
        public int id { get; set; }
        public Nullable<short> lp { get; set; }
        public int id_proddp { get; set; }
        public string kodtw { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public string godzina { get; set; }
        public Nullable<double> ilosc { get; set; }
        public Nullable<double> ilewopakowaniu { get; set; }
        public Nullable<int> id_opakowania_rodzaje { get; set; }
        public Nullable<int> id_opakowania { get; set; }
        public string opakowanie_nazwa { get; set; }
        public Nullable<double> ilosc_opakowania { get; set; }
        public Nullable<double> waga_opakowania { get; set; }
        public Nullable<int> id_opakowania_rodzaje2 { get; set; }
        public string opakowanie_nazwa2 { get; set; }
        public Nullable<double> ilosc_opakowania2 { get; set; }
        public Nullable<double> waga_opakowania_2 { get; set; }
        public Nullable<int> id_miejsca_skladowania { get; set; }
        public string miejsce_skladowania { get; set; }
        public string oznaczenie { get; set; }
        public Nullable<System.DateTime> termin_przydatnosci { get; set; }
        public Nullable<int> id_opakowania2 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GS1_KODY> GS1_KODY { get; set; }
        public virtual MAGAZYNY MAGAZYNY { get; set; }
        public virtual OPAKOWANIA_RODZAJE OPAKOWANIA_RODZAJE { get; set; }
        public virtual OPAKOWANIA_RODZAJE OPAKOWANIA_RODZAJE1 { get; set; }
        public virtual PROD_HMTW PROD_HMTW { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROD_PRODMGPW> PROD_PRODMGPW { get; set; }
        public virtual PRODDP PRODDP { get; set; }
        public virtual PRODDP_MONIT PRODDP_MONIT1 { get; set; }
        public virtual PRODDP_MONIT PRODDP_MONIT2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROD_MZ_SPEC> PROD_MZ_SPEC { get; set; }
    }
}
