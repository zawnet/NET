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
using HERBS_PRODUKCJA.Views;
using System.ComponentModel;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for WyborProdMGDwWindow.xaml
    /// </summary>
    public partial class WyborProdMGDwWindow : Window
    {
        public List<ProdukcjaMGDwVM> selDW { get; set; }

        public WyborProdMGDwWindow()
        {
           
            InitializeComponent();
            

        }
    }
}
