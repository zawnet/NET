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
    
    public partial class MG_WG_POWIAZANIA
    {
        public int id { get; set; }
        public int id_mg { get; set; }
        public int id_poz { get; set; }
        public int id_dost { get; set; }
        public int idtw { get; set; }
        public Nullable<double> ilosc { get; set; }
        public string jm { get; set; }
        public System.DateTime data { get; set; }
        public string zapas { get; set; }
    
        public virtual MG_WG_POZYCJE MG_WG_POZYCJE { get; set; }
    }
}