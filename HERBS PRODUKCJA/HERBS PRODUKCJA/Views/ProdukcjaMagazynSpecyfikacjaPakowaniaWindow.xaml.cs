using HERBS_PRODUKCJA.ViewModel.RowVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for ProdukcjaSpecyfikacjaPakowaniaWindow.xaml
    /// </summary>
    public partial class ProdukcjaSpecyfikacjaPakowaniaWindow : Window
    {
        public ProdukcjaMagazynPozycjaVM PozycjaMZ { get; set; }
        public List<PROD_MZ_SPEC> SPecyfikacja;
        public ProdukcjaSpecyfikacjaPakowaniaWindow()
        {

            InitializeComponent();

        }

        public ProdukcjaSpecyfikacjaPakowaniaWindow(ProdukcjaMagazynPozycjaVM obj)
        {
            SPecyfikacja = new List<PROD_MZ_SPEC>();
            this.PozycjaMZ = obj;
          
            InitializeComponent();

        }
    }
}
