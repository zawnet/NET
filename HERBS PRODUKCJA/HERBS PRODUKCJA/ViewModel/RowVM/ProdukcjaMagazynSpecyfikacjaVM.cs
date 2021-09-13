using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaMagazynSpecyfikacjaVM : VMBase
    {
        public PROD_MZ_SPEC Specyfikacja { get; set; }
        public short? lp { get; set; }

        public ProdukcjaMagazynSpecyfikacjaVM()
        {
            Specyfikacja = new PROD_MZ_SPEC();
        }

    }
}
