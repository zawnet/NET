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
    
    public partial class KONTRAKT_PARAM
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KONTRAKT_PARAM()
        {
            this.KONTRAKT_UMOWY_WART_PARAM = new HashSet<KONTRAKT_UMOWY_WART_PARAM>();
        }
    
        public long id { get; set; }
        public long id_kontrakt { get; set; }
        public string param_nazwa { get; set; }
        public string param_wartosc { get; set; }
        public byte aktywny { get; set; }
    
        public virtual KONTRAKT KONTRAKT { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KONTRAKT_UMOWY_WART_PARAM> KONTRAKT_UMOWY_WART_PARAM { get; set; }
    }
}