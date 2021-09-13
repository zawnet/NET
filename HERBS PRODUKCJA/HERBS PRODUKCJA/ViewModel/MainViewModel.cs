using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.Messages;
using HERBS_PRODUKCJA.Model;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Views;
using HERBS_PRODUKCJA.Views.RowVM;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace HERBS_PRODUKCJA.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<ViewVM> Views { get; set; }
        public ObservableCollection<CommandVM> Commands { get; set; }
        public RelayCommand DisplayMessageCommand { get; private set; }
        private RelayCommand _produkcjaButonCommand;
        public RelayCommand ProdukcjaButonCommand
        {
            get
            {
                return _produkcjaButonCommand
                    ?? (_produkcjaButonCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("Produkcja"))));
            }
        }
        //private readonly IDataService _dataService;



        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            //App.Current.Properties["kod_firmy"] = "FZL_sp";
            ObservableCollection<ViewVM> views = new ObservableCollection<ViewVM>
            {
                new ViewVM{ ViewDisplay="Produkcja", ViewType = typeof(ProdukcjaView), ViewModelType = typeof(ProdukcjaViewModel)},
                new ViewVM{ ViewDisplay="Magazyn", ViewType = typeof(ProdukcjaMagazynView), ViewModelType = typeof(ProdukcjaMagazynViewModel)},
                 new ViewVM{ ViewDisplay="Towary", ViewType = typeof(ProdukcjaTowaryView), ViewModelType = typeof(ProdukcjaTowaryViewModel)},
                 new ViewVM{ ViewDisplay="Partie", ViewType = typeof(PartieSYMView), ViewModelType = typeof(PartieSYMView)},
                 new ViewVM{ ViewDisplay="Wyjazdy Tow.", ViewType = typeof(DisplayControl), ViewModelType = typeof(WydaniaTwViewModel)},
            };
            Views = views;
            RaisePropertyChanged("Views");
            views[0].NavigateExecute();

            // Views[0].Navigate.e
            ObservableCollection<CommandVM> commands = new ObservableCollection<CommandVM>
            {
               // new CommandVM{ CommandDisplay="Insert", IconGeometry=Application.Current.Resources["InsertIcon"] as Geometry , Message=new CommandMessage{ Command =CommandType.Insert}},
               // new CommandVM{ CommandDisplay="Edit", IconGeometry=Application.Current.Resources["EditIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Edit}},
               // new CommandVM{ CommandDisplay="Delete", IconGeometry=Application.Current.Resources["DeleteIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Delete}},
               // new CommandVM{ CommandDisplay="Commit", IconGeometry=Application.Current.Resources["SaveIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Commit}},
                new CommandVM{ CommandDisplay="Refresh", Message=new CommandMessage{ Command = CommandType.Refresh}}

            };
            Commands = commands;
            RaisePropertyChanged("Commands");
            
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}