using GalaSoft.MvvmLight.CommandWpf;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.Views.RowVM;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.Views;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Messages;
using HERBS_PRODUKCJA.ViewModel;
using System.Collections.Generic;
using System;

namespace HERBS_PRODUKCJA
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ProdukcjaViewModel : CrudVMBase
    {
        public string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        //public string kod_firmy = "FZL_sp";
        public ObservableCollection<ViewVM> Views { get; set; }
        public ObservableCollection<CommandVM> Commands { get; set; }
       
        public ProdukcjaVM SelectedProdukcja {

            get;set;
        }
        public ObservableCollection<ProdukcjaVM> Produkcje { get; set; }

        private RelayCommand _doubleClickCommand;
        public RelayCommand DoubleClickCommand
        {
            get
            {
                return _doubleClickCommand
                    ?? (_doubleClickCommand = new RelayCommand(
                    () => Messenger.Default.Send(new NotificationMessage("ProdukcjaDetale"))));
            }
        }

        ProdukcjaVM _ProdukcjaInfo;
        public ProdukcjaVM ProdukcjaInfo
        {
            get { return _ProdukcjaInfo; }
            set
            {
                _ProdukcjaInfo = value;
                RaisePropertyChanged("ProdukcjaInfo");
            }
        }
        private bool isCanAdd;
        public bool IsCanAdd
        {
            get { return isCanAdd; }
            set
            {
                isCanAdd = value;
                RaisePropertyChanged("IsCanAdd");
            }
        }
        private string _PROD_search;
        public string PROD_search
        {
            get { return _PROD_search; }
            set
            {
                _PROD_search = value;
                RaisePropertyChanged("PROD_search");
            }
        }

        private string _PROD_year;
        public string PROD_year
        {
            get { return _PROD_year; }
            set
            {
                _PROD_year = value;
                RaisePropertyChanged("PROD_year");
            }
        }

        public int okres { get; set; }

        public RelayCommand OtwarteCommand { get; set; }
        public RelayCommand ZakonczoneCommand { get; set; }
        public RelayCommand ZatwierdzoneCommand { get; set; }
        public RelayCommand WszystkieCommand { get; set; }
        public RelayCommand CzescioweCommand { get; set; }
        public RelayCommand LaczenieCommand { get; set; }
        public RelayCommand SearchProdCommand { get; set; }
        public RelayCommand WstawSpecCommand { get; set; }
        public RelayCommand YearChangedCommand { get; set; }
        public List<string> ProdYears { get; set; }


        protected override void GetData()
        {
            //Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            using (db = new FZLEntities1()) {
                ObservableCollection<ProdukcjaVM> _produkcje = new ObservableCollection<ProdukcjaVM>();
                var procukcje = (from c in db.PROD
                                 where c.okres == okres
                                 where c.typ == 1
                                 where c.kod_firmy == kod_firmy
                                 where c.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where c.status != 4 //wyświetl tylko te które nie są wrzucone do archiwum
                                 orderby c.data
                                 select c).ToList();
                foreach (PROD p in procukcje)
                {
                    _produkcje.Add(new ProdukcjaVM { IsNew = false, Produkcja = p });
                }
                Produkcje = _produkcje;
                RaisePropertyChanged("Produkcje");
            }
          //  ThrobberVisible = Visibility.Collapsed;
        }

        protected  void GetData(string key)
        {

            //System.Windows.Forms.MessageBox.Show(key);  
            //Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            using (db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaVM> _produkcje = new ObservableCollection<ProdukcjaVM>();
                var procukcje = (from c in db.PROD
                                 where c.okres == okres
                                 where c.typ == 1
                                 where c.kod_firmy == kod_firmy
                                // where c.opis.Contains(key.ToString()) 
                                 where c.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where c.status != 4 //wyświetl tylko te które nie są wrzucone do archiwum
                                 orderby c.data
                                 select c).ToList();
                procukcje = procukcje.Where(x => (x.opis != null && x.opis.ToUpper().Contains(key.ToUpper())) || x.nazwa.Contains(key)).ToList();
                foreach (PROD p in procukcje)
                {
                    _produkcje.Add(new ProdukcjaVM { IsNew = false, Produkcja = p });
                }
                Produkcje = _produkcje;
                RaisePropertyChanged("Produkcje");
            }
            //  ThrobberVisible = Visibility.Collapsed;
        }

        protected override void EditCurrent()
        {
            _ProdukcjaInfo = SelectedProdukcja;
            //MessageBox.Show(ProdukcjaInfo.Produkcja.nazwa);  
            ProdukcjaDetaleViewWindow window = new ProdukcjaDetaleViewWindow();
            window.Show();
            Messenger.Default.Send<ProdukcjaVM>(SelectedProdukcja);
            // this.RaisePropertyChanged("SelectedProdukcja");
            //Views[0].NavigateExecute();
            //RaisePropertyChanged("SelectedProdukcja");
            //  Messenger.Default.Register<CommandMessage>(this, (action) => HandleCommand(action));
        }
        protected override void Insert()
        {
            // _ProdukcjaInfo = SelectedProdukcja;
            //  MessageBox.Show(ProdukcjaInfo.Produkcja.nazwa);
            ProdukcjaDetaleViewWindow window = new ProdukcjaDetaleViewWindow();
            window.Owner = App.Current.MainWindow;


            window.ShowDialog();
            Messenger.Default.Send<ProdukcjaVM>(new ProdukcjaVM());
            // this.RaisePropertyChanged("SelectedProdukcja");
            //Views[0].NavigateExecute();
            //RaisePropertyChanged("SelectedProdukcja");
            //  Messenger.Default.Register<CommandMessage>(this, (action) => HandleCommand(action));

        }
       
        protected override void DeleteCurrent()
        {
            UserMessage msg = new UserMessage();
            if (SelectedProdukcja != null)
            {
                int NumLines = NumberOfOrderLines();
                if (NumLines > 0)
                {
                    msg.Message = string.Format("Cannot delete - there are {0} Order Lines for this Product", NumLines);
                }
                else if (NumLines < 0)
                {
                    msg.Message = "Cannot delete since not committed to database yet";
                }
                else
                {
                    db.PROD.Remove(SelectedProdukcja.Produkcja);
                    Produkcje.Remove(SelectedProdukcja);
                    RaisePropertyChanged("Produkcje");
                    msg.Message = "Deleted";
                }
            }
            else
            {
                msg.Message = "No Product selected to delete";
            }
            Messenger.Default.Send<UserMessage>(msg);
        }
        private int NumberOfOrderLines()
        {
            var prod = db.PROD.Find(SelectedProdukcja.Produkcja.id);
            if (prod == null)
            {
                return -1;
            }
            // Count how many Order Lines there are for the Product
            int linesCount = db.Entry(prod)
                               .Collection(p => p.PRODDK)
                               .Query()
                               .Count();
            return linesCount;
        }

        private void PrzepiszSpecyfikacje()
        {
            foreach (ProdukcjaVM p in Produkcje)
            {
                p.WstawSpecyfikacjeDoZakonczonegoZlecena(p.Produkcja.id);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ProdukcjaViewModel()
            :base()
        {
            GetProdYEARS();
            _PROD_year = System.DateTime.Now.Year.ToString();
            RaisePropertyChanged("PROD_year");
            this.okres = int.Parse(_PROD_year);
            IsCanAdd = LoginViewViewModel.sprUprawnienie("produkcja_canadd");
            //SelectedProdukcja.Produkcja.nazwa = "Test";
            //Messenger.Default.Send<ProdukcjaVM>(SelectedProdukcja);
            ObservableCollection<ViewVM> views = new ObservableCollection<ViewVM>
            {
                new ViewVM{ ViewDisplay="ProdukcjaDetale", ViewType = typeof(ProdukcjaDetaleView), ViewModelType = typeof(ProdukcjaDetaleViewModel)},
            };

            Views = views;
            RaisePropertyChanged("Views");
            views[0].NavigateExecute();

            ObservableCollection<CommandVM> commands = new ObservableCollection<CommandVM>
            {
               // new CommandVM{ CommandDisplay="Insert", IconGeometry=Application.Current.Resources["InsertIcon"] as Geometry , Message=new CommandMessage{ Command =CommandType.Insert}},
               // new CommandVM{ CommandDisplay="Edit", IconGeometry=Application.Current.Resources["EditIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Edit}},
               // new CommandVM{ CommandDisplay="Delete", IconGeometry=Application.Current.Resources["DeleteIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Delete}},
               // new CommandVM{ CommandDisplay="Commit", IconGeometry=Application.Current.Resources["SaveIcon"] as Geometry , Message=new CommandMessage{ Command = CommandType.Commit}},
              //  new CommandVM{ CommandDisplay="Refresh", Message=new CommandMessage{ Command = CommandType.Refresh}},
                new CommandVM{ CommandDisplay="Edit", Message=new CommandMessage{ Command = CommandType.Edit}},
                new CommandVM{ CommandDisplay="Insert", Message=new CommandMessage{ Command = CommandType.Insert}},
                new CommandVM{ CommandDisplay="Edit", Message=new CommandMessage{ Command = CommandType.Refresh}},

            };
            Commands = commands;
            RaisePropertyChanged("Commands");
            Messenger.Default.Register<UserMessage>(this, prodvm => this.RefreshData());
            OtwarteCommand = new RelayCommand(GetOtwarte);
            ZakonczoneCommand = new RelayCommand(GetZakonczone);
            ZatwierdzoneCommand = new RelayCommand(GetZatwierdzone);
            WszystkieCommand = new RelayCommand(GetWszystkie);
            LaczenieCommand = new RelayCommand(LaczenieZlecen);
           CzescioweCommand = new RelayCommand(GetCzesciowe);
            SearchProdCommand = new RelayCommand(SearchProd);
            WstawSpecCommand = new RelayCommand(PrzepiszSpecyfikacje);
            YearChangedCommand = new RelayCommand(ChangeYear);
            //RaisePropertyChanged("SelectedProdukcja");
            GetData();
        }

        private void ChangeYear()
        {
            this.okres = int.Parse(PROD_year);
           
            GetData();
        }

        private void SearchProd()
        {
            GetData(PROD_search);
        }

        private void LaczenieZlecen()
        {
            ProdukcjaLaczenieWindow window = new ProdukcjaLaczenieWindow();
            window.Show();
        }

        public void GetOtwarte()
        {
            //Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            using (db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaVM> _produkcje = new ObservableCollection<ProdukcjaVM>();
                var procukcje = (from c in db.PROD
                                 where c.okres == okres
                                 where c.typ == 1
                                 where c.kod_firmy == kod_firmy
                                 where c.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where c.status != 4 //wyświetl te które nie są złączone
                                 where c.zakonczono != 1
                                 where c.akceptowano != 1
                                 orderby c.datadk
                                 select c).ToList();
                foreach (PROD p in procukcje)
                {
                    _produkcje.Add(new ProdukcjaVM { IsNew = false, Produkcja = p });
                }
                Produkcje = _produkcje;
                RaisePropertyChanged("Produkcje");
            }
            //  ThrobberVisible = Visibility.Collapsed;
        }
        public void GetZakonczone()
        {
            //Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            using (db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaVM> _produkcje = new ObservableCollection<ProdukcjaVM>();
                var procukcje = (from c in db.PROD
                                 where c.okres == okres
                                 where c.typ == 1
                                 where c.kod_firmy == kod_firmy
                                 where c.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where c.zakonczono == 1
                                 where c.akceptowano != 1
                                 orderby c.datadk
                                 select c).ToList();
                foreach (PROD p in procukcje)
                {
                    _produkcje.Add(new ProdukcjaVM { IsNew = false, Produkcja = p });
                }
                Produkcje = _produkcje;
                RaisePropertyChanged("Produkcje");
            }
            //  ThrobberVisible = Visibility.Collapsed;
        }

        public void GetCzesciowe()
        {
            //Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            using (db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaVM> _produkcje = new ObservableCollection<ProdukcjaVM>();
                var procukcje = (from c in db.PROD
                                 where c.okres == okres
                                 where c.typ == 1
                                 where c.kod_firmy == kod_firmy
                                 where c.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where c.status != 4 //wyświetl te które nie są złączone
                                 //where c.zakonczono == 1
                                 where c.czesciowa == 1
                                 orderby c.data
                                 select c).ToList();
                foreach (PROD p in procukcje)
                {
                    _produkcje.Add(new ProdukcjaVM { IsNew = false, Produkcja = p });
                }
                Produkcje = _produkcje;
                RaisePropertyChanged("Produkcje");
            }
            //  ThrobberVisible = Visibility.Collapsed;
        }

        public void GetZatwierdzone()
        {
            //Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            using (db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaVM> _produkcje = new ObservableCollection<ProdukcjaVM>();
                var procukcje = (from c in db.PROD
                                 where c.okres == okres
                                 where c.typ == 1
                                 where c.kod_firmy == kod_firmy
                                 where c.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                 
                                 where c.akceptowano == 1
                                 orderby c.data
                                 select c).ToList();
                foreach (PROD p in procukcje)
                {
                    _produkcje.Add(new ProdukcjaVM { IsNew = false, Produkcja = p });
                }
                Produkcje = _produkcje;
                RaisePropertyChanged("Produkcje");
            }
            //  ThrobberVisible = Visibility.Collapsed;
        }

        public void GetWszystkie()
        {
            GetData();
        }

        public void GetProdYEARS()
        {
            using (db = new FZLEntities1())
            {
                ProdYears = db.PROD.Where(x => x.kod_firmy == kod_firmy).Select(s => s.data.Value.Year.ToString()).Distinct().ToList();
                RaisePropertyChanged("ProdYears");
            }
        }


        }
}