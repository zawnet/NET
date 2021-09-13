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
using HERBS_PRODUKCJA.ViewModel.RowVM;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for ProdukcjaMaszynaParamMonitWindow.xaml
    /// </summary>
    public partial class ProdukcjaMaszynaParamMonitWindow : Window
    {
        public ProdukcjaMaszynaVM Maszyna { get; set; }
        public ProdukcjaMaszynaParametrMonitVM Monit { get; set; }
        public ProdukcjaMaszynaParamMonitWindow()
        {
            InitializeComponent();
            Monit = new ProdukcjaMaszynaParametrMonitVM();

        }
        public ProdukcjaMaszynaParamMonitWindow(ProdukcjaMaszynaVM m)
        {
            this.Maszyna = m;
            Monit = new ProdukcjaMaszynaParametrMonitVM();
            InitializeComponent();          
        }


        public ProdukcjaMaszynaParamMonitWindow(ProdukcjaMaszynaParametrMonitVM m)
        {
            this.Monit= m;
            //this.Maszyna = maszyna; 
            //Monit = new ProdukcjaMaszynaParametrMonitVM();
            InitializeComponent();
        }
    }
}
