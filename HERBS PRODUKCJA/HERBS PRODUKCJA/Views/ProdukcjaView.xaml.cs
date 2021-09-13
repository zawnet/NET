using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ProdukcjaView.xaml
    /// </summary>
    public partial class ProdukcjaView : UserControl
    {
        public ProdukcjaView()
        {
            InitializeComponent();
            this.DataContext = new ProdukcjaViewModel();
            //Messenger.Default.Register<NavigateMessage>(this, (action) => ShowUserControl(action));
            ((ProdukcjaViewModel)DataContext).PropertyChanged += ViewModel_PropertyChanged;
        }
        void ViewModel_PropertyChanged(object s, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Produkcje")
            {
                prod.Items.Refresh();
            }
        }

    }
}
