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
    
    public partial class SUROWCE_TW
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUROWCE_TW()
        {
            this.SUROWCE = new HashSet<SUROWCE>();
        }
    
        public int id { get; set; }
        public string kod_tw { get; set; }
        public string nazwa { get; set; }
        public string nazwa_tw { get; set; }
        public Nullable<long> idtw { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SUROWCE> SUROWCE { get; set; }
    }
}
