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
    
    public partial class KONTRAKT_UMOWY_WART_PARAM
    {
        public long id { get; set; }
        public long id_umowy { get; set; }
        public Nullable<long> id_param { get; set; }
        public string nazwa_param { get; set; }
        public string wart_param { get; set; }
    
        public virtual KONTRAKT_PARAM KONTRAKT_PARAM { get; set; }
        public virtual KONTRAKT_UMOWY KONTRAKT_UMOWY { get; set; }
    }
}