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
    /// Interaction logic for ProdukcjaMagazynDokumentView.xaml
    /// </summary>
    public partial class ProdukcjaMagazynDokumentView : UserControl
    {
        public ProdukcjaMagazynDokumentView()
        {
            ProdukcjaMagazynWindow parent = Application.Current.Windows.OfType<ProdukcjaMagazynWindow>().First();
            this.DataContext =  parent.vm;
            InitializeComponent();
        }

        private void DataGrid_TextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //BindingExpression binding = pozycje.GetBindingExpression(DataGridCell.TagProperty);
            //binding.UpdateSource();
        }
    }
}
