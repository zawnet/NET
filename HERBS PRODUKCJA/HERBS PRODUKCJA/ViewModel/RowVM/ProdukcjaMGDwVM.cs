using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaMGDwVM : VMBase
    {
        public PROD_MGDW ProdukcjaDW { get; set; }
        public PRODDP ProdukcjaPRODDP { get; set; }
        public ProdukcjaDWList ProdukcjaDW_List { get; set; }
        public class ProdukcjaDWList
        {
            public int id { get; set; }
            public string kod { get; set; }
            public Nullable<int> typi { get; set; }
            public Nullable<int> idpozpz { get; set; }
            public Nullable<int> idkh { get; set; }
            public string khkod { get; set; }
            public string khnazwa { get; set; }
            public Nullable<int> idtw { get; set; }
            public string kodtw { get; set; }
            public string nazwatw { get; set; }
            public Nullable<System.DateTime> data { get; set; }
            public Nullable<double> ilosc { get; set; }
            public Nullable<double> iloscprod { get; set; }
            public Nullable<double> iloscdost { get; set; }
            public Nullable<double> stan { get; set; }
            public Nullable<double> iloscpz { get; set; }
            public string typdw { get; set; }
            public string nr_partii { get; set; }
            public string kod_firmy { get; set; }
            public string frakcja { get; set; }
            public string opis { get; set; }

            public ProdukcjaDWList()
            {

            }

        }

       //  public ObservableCollection<PROD_DW> ProdDWs { get; set; }
        public ProdukcjaMGDwVM()
        {
            ProdukcjaDW = new PROD_MGDW();
            ProdukcjaDW_List = new ProdukcjaDWList();
  
        }


        
    }
}
