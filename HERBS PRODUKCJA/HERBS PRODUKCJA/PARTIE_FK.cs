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
    
    public partial class PARTIE_FK
    {
        public int id { get; set; }
        public int id_partii { get; set; }
        public string nr_partii { get; set; }
        public Nullable<int> iddk_lab { get; set; }
        public Nullable<int> idpoz_lab { get; set; }
        public string koddk_fki { get; set; }
        public Nullable<int> iddk_fki { get; set; }
        public Nullable<int> idpoz_fki { get; set; }
        public Nullable<int> idtw_fki { get; set; }
        public string twopis { get; set; }
        public Nullable<double> ilosc { get; set; }
        public string jm { get; set; }
        public string kod_firmy { get; set; }
        public string typ_dk { get; set; }
        public Nullable<System.DateTime> dataop { get; set; }
        public string osoba { get; set; }
    
        public virtual MG MG { get; set; }
        public virtual PARTIE PARTIE { get; set; }
    }
}
