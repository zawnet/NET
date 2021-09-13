using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaKhVM : VMBase
    {

        public KH ProdukcjaKh { get; set; }
        private string _adres;
        public string Adres
        {
            get
            {
                return _adres;
            }
            set
            {
                _adres = value;
            }
        }

        public ProdukcjaKhVM()
        {
            ProdukcjaKh = new KH();
        }

        public string getAdres()
        {
            string adres = "";
            if (ProdukcjaKh != null && ProdukcjaKh.ulica != null && ProdukcjaKh.dom != null)
            {
                adres = ProdukcjaKh.ulica + " " + ProdukcjaKh.dom;
                if (ProdukcjaKh.lokal != null && ProdukcjaKh.lokal != "")
                    adres += "\\" + ProdukcjaKh.lokal;
                adres += " " + ProdukcjaKh.kodpocz + " " + ProdukcjaKh.miejscowosc;
            }
            return adres;
        }
    }
}
