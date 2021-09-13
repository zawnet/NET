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
using HERBS_PRODUKCJA.ViewModel;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for HistoriaTowarowWindow.xaml
    /// </summary>
    public partial class HistoriaTowarowWindow : Window
    {
        public ProdukcjaTowaryViewModel produkcjaTowaryViewModel { get; set; }

        public HistoriaTowarowWindow()
        {
            InitializeComponent();
        }

        public HistoriaTowarowWindow(ProdukcjaTowaryViewModel produkcjaTowaryViewModel)
        {
            this.produkcjaTowaryViewModel = produkcjaTowaryViewModel;
            InitializeComponent();
        }
    }
}
