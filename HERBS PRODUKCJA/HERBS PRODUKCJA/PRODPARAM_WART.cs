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
    
    public partial class PRODPARAM_WART
    {
        public int id_prod { get; set; }
        public int id_param { get; set; }
        public Nullable<float> wartosc_licz { get; set; }
        public string wartosc_txt { get; set; }
        public int wart { get; set; }
    
        public virtual PROD PROD { get; set; }
        public virtual PRODPARAM PRODPARAM { get; set; }
    }
}