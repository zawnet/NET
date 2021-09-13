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
using System.Windows.Navigation;
using System.Windows.Shapes;
using HERBS_PRODUKCJA.ViewModel;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for HistoriaTowarowView.xaml
    /// </summary>
    public partial class HistoriaTowarowView : UserControl
    {
       

        public HistoriaTowarowView()
        {
            HistoriaTowarowWindow parent = Application.Current.Windows.OfType<HistoriaTowarowWindow>().First();
            this.DataContext = parent.produkcjaTowaryViewModel;
            
            InitializeComponent();
        }
    }
}
