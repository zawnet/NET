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
    
    public partial class MG_RODZAJE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MG_RODZAJE()
        {
            this.MG = new HashSet<MG>();
        }
    
        public int id { get; set; }
        public string nazwa { get; set; }
        public string symbol { get; set; }
        public byte typ { get; set; }
        public string format { get; set; }
        public string schemat { get; set; }
        public Nullable<double> wartosc { get; set; }
        public string pole1 { get; set; }
        public byte nadawaj_partie { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG> MG { get; set; }
    }
}
