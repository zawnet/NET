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
    
    public partial class MG_WG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MG_WG()
        {
            this.MG_WG_POZYCJE = new HashSet<MG_WG_POZYCJE>();
        }
    
        public int id { get; set; }
        public System.DateTime data { get; set; }
        public string kod { get; set; }
        public Nullable<byte> rodzaj { get; set; }
        public int typ { get; set; }
        public int serianr { get; set; }
        public short okres { get; set; }
        public long khid { get; set; }
        public string khkod { get; set; }
        public string khnazwa { get; set; }
        public string khadres { get; set; }
        public string khdom { get; set; }
        public string khlokal { get; set; }
        public string khmiasto { get; set; }
        public string khkodpocz { get; set; }
        public string khnip { get; set; }
        public int magazyn { get; set; }
        public string uwagi { get; set; }
        public string kod_firmy { get; set; }
    
        public virtual KH KH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG_WG_POZYCJE> MG_WG_POZYCJE { get; set; }
    }
}