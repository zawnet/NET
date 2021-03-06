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
    
    public partial class MG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MG()
        {
            this.MG_PARAMETRY_WART = new HashSet<MG_PARAMETRY_WART>();
            this.MG_POZYCJE = new HashSet<MG_POZYCJE>();
            this.OWSBZ = new HashSet<OWSBZ>();
            this.PARTIE_FK = new HashSet<PARTIE_FK>();
        }
    
        public int id { get; set; }
        public Nullable<int> rodzaj { get; set; }
        public Nullable<bool> info { get; set; }
        public string osoba { get; set; }
        public string kod { get; set; }
        public string seria { get; set; }
        public Nullable<int> serianr { get; set; }
        public Nullable<short> okres { get; set; }
        public string nazwa { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public Nullable<System.DateTime> datasp { get; set; }
        public string opis { get; set; }
        public Nullable<long> khid { get; set; }
        public string khkod { get; set; }
        public string khnazwa { get; set; }
        public string khadres { get; set; }
        public string khdom { get; set; }
        public string khlokal { get; set; }
        public string khmiasto { get; set; }
        public string khkodpocz { get; set; }
        public string khnip { get; set; }
        public Nullable<byte> ok { get; set; }
        public Nullable<byte> puste { get; set; }
        public Nullable<System.DateTime> termin { get; set; }
        public Nullable<double> netto { get; set; }
        public string typ_dk { get; set; }
        public Nullable<double> wartoscWz { get; set; }
        public Nullable<short> magazyn { get; set; }
        public Nullable<double> przychod { get; set; }
        public Nullable<double> rozchod { get; set; }
        public Nullable<double> wartk { get; set; }
        public string schemat { get; set; }
        public string zapas { get; set; }
        public string partia { get; set; }
        public Nullable<int> id_partia { get; set; }
        public Nullable<short> exp_fki { get; set; }
        public Nullable<int> id_fki { get; set; }
        public string kod_fki { get; set; }
        public string kod_firmy { get; set; }
        public string partia_tyrb { get; set; }
        public Nullable<int> id_tw_got { get; set; }
        public string nazwa_tw_got { get; set; }
        public Nullable<double> ilosc_wg { get; set; }
        public Nullable<int> id_firma { get; set; }
        public Nullable<int> id_mgprod { get; set; }
    
        public virtual FIRMA FIRMA { get; set; }
        public virtual MG MG1 { get; set; }
        public virtual MG MG2 { get; set; }
        public virtual MG_RODZAJE MG_RODZAJE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG_PARAMETRY_WART> MG_PARAMETRY_WART { get; set; }
        public virtual PARTIE PARTIE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MG_POZYCJE> MG_POZYCJE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OWSBZ> OWSBZ { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARTIE_FK> PARTIE_FK { get; set; }
    }
}
