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
    
    public partial class MZ_HM
    {
        public int id { get; set; }
        public string id_hm { get; set; }
        public Nullable<int> flag { get; set; }
        public Nullable<byte> subtypi { get; set; }
        public Nullable<int> typi { get; set; }
        public Nullable<int> id_mghm { get; set; }
        public Nullable<int> super { get; set; }
        public Nullable<byte> lp { get; set; }
        public Nullable<int> idkh { get; set; }
        public Nullable<int> idtw { get; set; }
        public string kod { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public string opis { get; set; }
        public string opisdod { get; set; }
        public Nullable<double> ilosc { get; set; }
        public string jm { get; set; }
        public Nullable<byte> grupaceni { get; set; }
        public Nullable<double> cena { get; set; }
        public Nullable<double> wartNetto { get; set; }
        public Nullable<int> idpozkoryg { get; set; }
        public Nullable<byte> rejestri { get; set; }
        public Nullable<int> idlongname { get; set; }
        public Nullable<byte> magazyn { get; set; }
        public Nullable<double> iloscjedn { get; set; }
        public Nullable<double> wartTowaru { get; set; }
        public string osoba { get; set; }
        public Nullable<double> iloscwp { get; set; }
        public Nullable<int> idhandl { get; set; }
        public Nullable<double> iloscdorozl { get; set; }
        public Nullable<double> przychod { get; set; }
        public Nullable<double> rozchod { get; set; }
        public Nullable<double> wartk { get; set; }
        public Nullable<byte> lwartki { get; set; }
        public string schemat { get; set; }
        public string jmwp { get; set; }
        public string zapas { get; set; }
    
        public virtual MG_HM MG_HM { get; set; }
    }
}
