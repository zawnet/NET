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
    
    public partial class PRODDP_FK
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PRODDP_FK()
        {
            this.PROD_PW = new HashSet<PROD_PW>();
        }
    
        public int id { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public Nullable<int> id_proddk { get; set; }
        public int id_prod { get; set; }
        public Nullable<int> id_poz { get; set; }
        public Nullable<int> lp { get; set; }
        public Nullable<int> idtw { get; set; }
        public string kodtw { get; set; }
        public string typ_produktu { get; set; }
        public string khnazwa { get; set; }
        public string nazwatw { get; set; }
        public Nullable<double> ilosc { get; set; }
        public string jm { get; set; }
        public Nullable<double> cena { get; set; }
        public Nullable<double> planowana_cena { get; set; }
        public Nullable<double> cena_wytowrzenia { get; set; }
        public Nullable<double> wartosc { get; set; }
        public Nullable<double> wskaznik { get; set; }
        public Nullable<double> kurs { get; set; }
        public Nullable<System.DateTime> data_kursu { get; set; }
        public string waluta { get; set; }
        public string opis { get; set; }
        public Nullable<int> iddw { get; set; }
        public Nullable<int> idproddw { get; set; }
        public string koddw { get; set; }
        public string nr_partii { get; set; }
        public Nullable<int> id_partii { get; set; }
        public string frakcja { get; set; }
        public string rodzaj_dk { get; set; }
        public Nullable<int> id_hm { get; set; }
        public byte ztw { get; set; }
        public Nullable<double> wilgotnosc { get; set; }
        public Nullable<double> czystosc { get; set; }
        public Nullable<byte> rozkruszek { get; set; }
        public string kodpaskowy { get; set; }
        public Nullable<double> produkcja_koszt { get; set; }
        public Nullable<double> surowiec_koszt { get; set; }
        public Nullable<byte> cena_typ { get; set; }
    
        public virtual PARTIE PARTIE { get; set; }
        public virtual PROD_HMDW PROD_HMDW { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROD_PW> PROD_PW { get; set; }
        public virtual PRODDK PRODDK { get; set; }
    }
}
