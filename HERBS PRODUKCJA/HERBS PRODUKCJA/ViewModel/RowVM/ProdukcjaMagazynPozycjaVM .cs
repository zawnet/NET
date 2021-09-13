using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Support;
using System.Collections.ObjectModel;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaMagazynPozycjaVM : VMBase
    {
        public PROD_MZ ProdukcjaMZ { get; set; }
        
        




        UZYTKOWNICY user = (UZYTKOWNICY)App.Current.Properties["UserLoged"];

        public ProdukcjaMagazynPozycjaVM()
        {
            
        }

       

    }
}
