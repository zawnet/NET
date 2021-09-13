using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaMaszynaParametrWartVM : VMBase
    {
        public PROD_MASZYNY_PARAM_WART Wartosc { get; set; }
        public PROD_MASZYNY_PW MaszynaPW { get; set; }

        public void UsunParametr()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                db.PROD_MASZYNY_PARAM_WART.Remove(Wartosc);
                db.SaveChanges();
            }
        }

        public void WyliczWartoscPracy()
        {

        }
    }
}
