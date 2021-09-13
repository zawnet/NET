using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.Helpers;
//using HERBS_PRODUKCJA.InteliboxDataProviders;
using HERBS_PRODUKCJA.Messages;
using HERBS_PRODUKCJA.ViewModel;
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


namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for ProdukcjaDetaleView.xaml
    /// </summary>
    public partial class ProdukcjaDetaleView : UserControl
    {
        
        public ProdukcjaDetaleView()
        {
           // 
            InitializeComponent();
            this.DataContext = new ProdukcjaDetaleViewModel();
          //  LinqToEntitiesProvider = new TWResultsProvider();
          

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
           // this.EditBox.UpdateDocumentBindings();
        }
    }
}
