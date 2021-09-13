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
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for ProdukcjaTowaryView.xaml
    /// </summary>
    public partial class ProdukcjaTowaryView : UserControl
    {
        public ProdukcjaTowaryView()
        {
            this.DataContext = new ProdukcjaTowaryViewModel();
            InitializeComponent();
        }
        public bool isChanging = false;
        async private void txtSSNoFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            // entry flag
            if (isChanging)
            {
                return;
            }
            isChanging = true;
            await Task.Delay(1500);

            // do your stuff here or call a function

            // exit flag
            isChanging = false;
        }
    }
}
