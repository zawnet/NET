using System.Windows;
using HERBS_PRODUKCJA.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.Views;
using System.Windows.Controls;
using HERBS_PRODUKCJA.Messages;
//using HERBS_PRODUKCJA.InteliboxDataProviders;
using HERBS_PRODUKCJA.Helpers;
using GalaSoft.MvvmLight.Ioc;

namespace HERBS_PRODUKCJA
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       // public TWResultsProvider TWProvider
   //     {
  //          get;
   //         private set;
    //    }
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            // Messenger.Default.Register<NavigateMessage>(this, (action) => ShowUserControl(action));
            Style = (Style)FindResource(typeof(Window));
            Messenger.Default.Register<NavigateMessage>(this, (action) => ShowUserControl(action));
            //Closing += (s, e) => ViewModelLocator.Cleanup();
            Messenger.Default.Register<OpenWindowMessage>(
              this,
              message => {
                  if (message.Type == WindowType.kModal)
                  {
                      MessageBox.Show("Dupa");
                      var modalWindowVM = SimpleIoc.Default.GetInstance<WyborDwViewModel>();
                      // modalWindowVM. = message.Argument;
                      var modalWindow = new ProdukcjaDostawyWindow()
                      {
                          DataContext = modalWindowVM
                      };
                      var result = modalWindow.ShowDialog() ?? false;
                      Messenger.Default.Send(result ? "Accepted" : "Rejected");
                  }
                  else
                  {

                      var uniqueKey = System.Guid.NewGuid().ToString();
                      var nonModalWindowVM = SimpleIoc.Default.GetInstance<WyborDwViewModel>(uniqueKey);
                      //nonModalWindowVM.MyText = message.Argument;
                      var nonModalWindow = new ProdukcjaDostawyWindow()
                      {
                          DataContext = nonModalWindowVM
                      };
                      nonModalWindow.Closed += (sender, args) => SimpleIoc.Default.Unregister(uniqueKey);
                      nonModalWindow.Show();

                  }
              });
            // Messenger.Default.Register<UserMessage>(this, (action) => ReceiveUserMessage(action));

        }

        public void HostWindowInFrame(Frame fraContainer, Window win)
        {
            object tmp = win.Content;
            win.Content = null;
            fraContainer.Content = new ContentControl() { Content = tmp };
        }

        private void ShowUserControl(NavigateMessage nm)
        {
            nm.View.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            nm.View.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            EditFrame.Content = nm.View;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}