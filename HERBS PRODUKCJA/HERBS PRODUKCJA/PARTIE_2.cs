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
    
    public partial class PARTIE_2
    {
        public int id { get; set; }
        public int partia_id { get; set; }
        public string nr_partii1 { get; set; }
        public string nr_partii2 { get; set; }
        public Nullable<byte> grupa { get; set; }
        public int serial { get; set; }
        public Nullable<int> idtw { get; set; }
        public string kod_tw { get; set; }
        public string nazwa_tw { get; set; }
        public Nullable<System.DateTime> dodano { get; set; }
        public Nullable<System.DateTime> modyfikowano { get; set; }
    
        public virtual PARTIE PARTIE { get; set; }
        public virtual TW TW { get; set; }
    }
}