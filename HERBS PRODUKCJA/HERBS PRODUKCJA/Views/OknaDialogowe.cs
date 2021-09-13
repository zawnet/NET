using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.CommandWpf;

namespace HERBS_PRODUKCJA.Views
{
    public abstract class DialogBox : FrameworkElement, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nazwaWłasności)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));
        }
        #endregion
        protected Action execute = null;
        public string Caption { get; set; }
        public string o { get; set; }
        protected ICommand show;
        public virtual ICommand Show
        {
            get
            {
                if (show == null) show = new RelayCommand(execute);
                return show;
            }
        }
    }

    public class SimpleMessageDialogBox : DialogBox
    {
        public SimpleMessageDialogBox()
        {

            
           
          MessageBox.Show((string)o, Caption);
            
        }
    }
}
