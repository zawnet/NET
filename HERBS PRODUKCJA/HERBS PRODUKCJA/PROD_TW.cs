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
    
    public partial class PROD_TW
    {
        public int id { get; set; }
        public string kod_firmy { get; set; }
        public Nullable<int> id_fk { get; set; }
        public Nullable<byte> flag { get; set; }
        public Nullable<byte> subtypi { get; set; }
        public Nullable<byte> znaczniki { get; set; }
        public Nullable<int> rodzaj { get; set; }
        public Nullable<int> katalog { get; set; }
        public string info { get; set; }
        public string osoba { get; set; }
        public string kod { get; set; }
        public string nazwa { get; set; }
        public string kodpaskowy { get; set; }
        public Nullable<byte> vatspi { get; set; }
        public Nullable<double> stan { get; set; }
        public string jm { get; set; }
        public Nullable<System.DateTime> utorzono { get; set; }
        public Nullable<System.DateTime> modyfikowano { get; set; }
    }
}