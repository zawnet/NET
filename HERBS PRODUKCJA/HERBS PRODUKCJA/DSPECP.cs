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
    
    public partial class DSPECP
    {
        public int id { get; set; }
        public int id_poz_dk { get; set; }
        public Nullable<int> id_dk { get; set; }
        public string opakowanie { get; set; }
        public string nazwa_opakowania { get; set; }
        public Nullable<int> liczba_opakowania { get; set; }
        public Nullable<float> waga_opakowania { get; set; }
        public Nullable<float> ileWOpakowaniu { get; set; }
        public string jednostka_opakowania { get; set; }
        public Nullable<System.DateTime> data_dodania { get; set; }
        public Nullable<int> id_dspec { get; set; }
    
        public virtual DSPEC DSPEC { get; set; }
    }
}
