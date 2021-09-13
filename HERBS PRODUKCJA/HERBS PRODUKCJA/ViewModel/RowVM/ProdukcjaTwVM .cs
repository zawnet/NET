using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaTwVM : VMBase
    {
        public PROD_HMTW ProdukcjaTW { get; set; }
      //  public ObservableCollection<PROD_DW> ProdDWs { get; set; }
        public ProdukcjaTwVM()
        {
            ProdukcjaTW = new PROD_HMTW();


        }
    }
}
