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
    
    public partial class PROD_MG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROD_MG()
        {
            this.PROD_MZ = new HashSet<PROD_MZ>();
        }
    
        public int id { get; set; }
        public string kod { get; set; }
        public string kod_firmy { get; set; }
        public Nullable<int> khid { get; set; }
        public int serial { get; set; }
        public int okres { get; set; }
        public string khnazwa { get; set; }
        public string khkod { get; set; }
        public string khadres { get; set; }
        public string khdom { get; set; }
        public string khlokal { get; set; }
        public string khmiasto { get; set; }
        public string khkodpocz { get; set; }
        public string khnip { get; set; }
        public string osoba { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public string nazwa { get; set; }
        public Nullable<System.DateTime> termin { get; set; }
        public string typ_dk { get; set; }
        public Nullable<byte> exp_fki { get; set; }
        public Nullable<int> id_handl { get; set; }
        public string kod_handl { get; set; }
        public string osoba_mod { get; set; }
        public Nullable<System.DateTime> data_mod { get; set; }
        public string opis { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROD_MZ> PROD_MZ { get; set; }
    }
}