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
    
    public partial class PROD_MASZYNY_MONIT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PROD_MASZYNY_MONIT()
        {
            this.PROD_MASZYNY_KOSZTY = new HashSet<PROD_MASZYNY_KOSZTY>();
        }
    
        public int id { get; set; }
        public int id_prod { get; set; }
        public Nullable<int> id_prod_maszyny_pw { get; set; }
        public string param_nazwa { get; set; }
        public string param_wart { get; set; }
        public Nullable<int> id_param { get; set; }
        public Nullable<System.DateTime> data { get; set; }
        public string czas { get; set; }
        public Nullable<double> ilosc { get; set; }
        public string uzytkownik { get; set; }
        public System.DateTime utworzono { get; set; }
        public string frakcja { get; set; }
        public string uwagi { get; set; }
        public Nullable<System.DateTime> rozpoczecie_data { get; set; }
        public string rozpoczecie_godzina { get; set; }
        public Nullable<System.DateTime> zakonczenie_data { get; set; }
        public string zakonczenie_godzina { get; set; }
        public Nullable<double> cena { get; set; }
        public Nullable<double> wartosc { get; set; }
        public Nullable<int> ilosc_pracownikow { get; set; }
    
        public virtual PROD PROD { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PROD_MASZYNY_KOSZTY> PROD_MASZYNY_KOSZTY { get; set; }
        public virtual PROD_MASZYNY_PW PROD_MASZYNY_PW { get; set; }
    }
}