using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.Helpers
{
    public class ProdMGTyp
    {
        private string _kod;

        public string Kod
        {
            get { return _kod; }
            set { _kod = value; }
        }


        private string _nazwa;
        public string Nazwa
        {
            get { return _nazwa; }
            set { _nazwa = value; }
        }
    }
}
