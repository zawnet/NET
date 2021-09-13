using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaDwVM : VMBase
    {
        public PROD_HMDW ProdukcjaDW { get; set; }
        public PRODDP ProdukcjaPRODDP { get; set; }
       //  public ObservableCollection<PROD_DW> ProdDWs { get; set; }
        public ProdukcjaDwVM()
        {
            ProdukcjaDW = new PROD_HMDW();


        }
    }
}
