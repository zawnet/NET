using GalaSoft.MvvmLight.Command;
using HERBS_PRODUKCJA.Commands;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class ProdukcjaLaczenieViewModel : CrudVMBase
    {
        public string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        public ProdukcjaVM SelectedProdukcja
        {

            get; set;
        }
        IList<ProdukcjaPozycjeVM> Produkcje { get; set; }
        private RelayCommand _doubleClickCommand;
        public RelayCommand SelCommand { get; set; }
        public RelayCommand SzukajCommand { get; set; }
        public ICollectionView ProdukcjeView { set; get; }
        ProdukcjaVM ProdukcjaInfo;
        UZYTKOWNICY user = (UZYTKOWNICY)App.Current.Properties["UserLoged"];

        String _towar;
        public string  towar
        {
            get { return _towar ; }
            set
            {
                _towar = value;
                RaisePropertyChanged("towar");
            }
        }
        String _dostawa;
        public string dostawa
        {
            get { return _dostawa; }
            set
            {
                _dostawa = value;
                RaisePropertyChanged("dostawa");
            }
        }
        String _dostawca;
        public string dostawca
        {
            get { return _dostawca; }
            set
            {
                _dostawca = value;
                RaisePropertyChanged("dostawca");
            }
        }

        public ProdukcjaLaczenieViewModel():base()
        {
            towar = "";
            dostawa = "";
            dostawca = "";
            // GetOtwarteProdukcje();
            GetOtwarteProdukcjeByPozycje();
            groupByCustomerCommand = new GroupByCustomerCommand(this);
            groupByYearMonthCommand = new GroupByYearMonthCommand(this);
            removeGroupCommand = new RemoveGroupCommand(this);
            SelCommand = new RelayCommand(WybierzZlecenia);
            SzukajCommand = new RelayCommand(SzukajZlecen);
        }
        

        private void InicjujFiltry()
        {
             
        }

        private void SzukajZlecen()
        {
            GetOtwarteProdukcjeByPozycje(this.towar, this.dostawa, this.dostawca);
        }

        private void WybierzZlecenia()
        {
           string kody_zlecen="";

            foreach (ProdukcjaPozycjeVM element in Produkcje)
            {
                if (element.IsChecked)
                {
                    kody_zlecen += element.Produkcja.nazwa+"   "+element.Produkcja.opis+ "\n";
                    
                }
            }
            var result = MessageBox.Show(
                        "Operacja zamyka zlecenia i przenosi je do nowego.\nCzy jesteś pewien, że chcesz połączyć te zlecenia:\n\n"+kody_zlecen,
                        "Akceptacja", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {

                //MessageBox.Show("Łączenie");

                NowaProdukcja();
                ProdukcjaVM old; ;

                foreach (ProdukcjaPozycjeVM element in Produkcje)
                {
                    if (element.IsChecked)
                    {

                       // MessageBox.Show(element.Produkcja.nazwa);
                        old = new ProdukcjaVM(element.Produkcja.id);

                        old.ZlaczDo(ProdukcjaInfo.Produkcja);

                    }

                }
                GetOtwarteProdukcjeByPozycje();
                groupByCustomerCommand = new GroupByCustomerCommand(this);
                groupByYearMonthCommand = new GroupByYearMonthCommand(this);
                removeGroupCommand = new RemoveGroupCommand(this);
            }
        }
        private void NowaProdukcja()
        {
            using (FZLEntities1 dbx = new FZLEntities1())
            {
                ProdukcjaInfo = new ProdukcjaVM();
               
                    ProdukcjaInfo.Produkcja.data = DateTime.Now;
                    ProdukcjaInfo.Produkcja.okres = int.Parse(DateTime.Now.Year.ToString());
                    ProdukcjaInfo.Produkcja.serial = ProdukcjaInfo.pobierzKolejnyNumerSerii((int)ProdukcjaInfo.Produkcja.okres);
                    ProdukcjaInfo.Produkcja.nazwa = "ZP/" + ProdukcjaInfo.Produkcja.serial.ToString() + "/" + ProdukcjaInfo.Produkcja.okres.ToString();
                    ProdukcjaInfo.Produkcja.kod = "ZP/" + ProdukcjaInfo.Produkcja.serial.ToString() + "/" + ProdukcjaInfo.Produkcja.okres.ToString();
                    ProdukcjaInfo.Produkcja.osoba = user.nazwa;
                    dbx.PROD.Add(ProdukcjaInfo.Produkcja);
                
                try
                {
                    dbx.SaveChanges();
                    //ProdukcjaInfo.IsNew = false;

                    // msg.Message = "Database Updated";
                }
                catch (Exception e)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        ErrorMessage = e.InnerException.GetBaseException().ToString();
                    }
                    MessageBox.Show(ErrorMessage);
                }
            }
        }

        public void GetOtwarteProdukcje()
        {
            //IList<Order> orders = new Orders();
            
            // Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            using (db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaPozycjeVM> _produkcje = new ObservableCollection<ProdukcjaPozycjeVM>();
                var procukcje = (from c in db.PRODDP
                                 from dp in db.PROD
                                 where c.id_prod == dp.id
                                 where dp.typ == 1
                                 where dp.kod_firmy == kod_firmy
                                 where dp.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where dp.status != 4 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where dp.zakonczono != 1
                                 where dp.akceptowano != 1
                                 orderby dp.datadk
                                 select c).ToList();
                foreach (PRODDP p in procukcje)
                {
                    
                    _produkcje.Add(new ProdukcjaPozycjeVM { IsNew = false, Pozycja = p });
                    _produkcje.Last().GetProd();
                }
                Produkcje = _produkcje;

                ProdukcjeView = CollectionViewSource.GetDefaultView(Produkcje);
                ProdukcjeView.GroupDescriptions.Add(new PropertyGroupDescription("noGroup"));

               

                //groupByCustomerCommand = new GroupByCustomerCommand(this);
               // groupByYearMonthCommand = new GroupByYearMonthCommand(this);
                //removeGroupCommand = new RemoveGroupCommand(this);

               

                RaisePropertyChanged("ProdukcjeView");
            }
        }

        public void GetOtwarteProdukcjeByPozycje()
        {
            //IList<Order> orders = new Orders();

            // Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            using (db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaPozycjeVM> _produkcje = new ObservableCollection<ProdukcjaPozycjeVM>();
                var procukcje = (from c in db.PRODDP
                                 from dp in db.PROD
                                 where c.id_prod == dp.id
                                 where dp.typ == 1
                                 where dp.kod_firmy == kod_firmy
                                 where dp.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where dp.status != 4 //wyświetl tylko te które nie są wrzucone do archiwum
                                 where dp.zakonczono != 1
                                 where dp.akceptowano != 1
                                 orderby dp.datadk
                                 select c).ToList();
                foreach (PRODDP p in procukcje)
                {
                    _produkcje.Add(new ProdukcjaPozycjeVM { IsNew = false, Pozycja = p });
                    _produkcje.Last().GetProd();
                }
                Produkcje = _produkcje;

                ProdukcjeView = CollectionViewSource.GetDefaultView(Produkcje);
                ProdukcjeView.GroupDescriptions.Add(new PropertyGroupDescription("Produkcja.nazwa"));



                //groupByCustomerCommand = new GroupByCustomerCommand(this);
                //groupByYearMonthCommand = new GroupByYearMonthCommand(this);
                //removeGroupCommand = new RemoveGroupCommand(this);

                RaisePropertyChanged("ProdukcjeView");
            }
        }
        public void GetOtwarteProdukcjeByPozycje(String towar, String dostawa, String dostawca)
        {
            //IList<Order> orders = new Orders();

            // Produkcje = new ObservableCollection<ProdukcjaVM>();
            //Produkcje.Clear();
            // ThrobberVisible = Visibility.Visible;
            List<PRODDP> procukcje = new List<PRODDP>();
            using (db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaPozycjeVM> _produkcje = new ObservableCollection<ProdukcjaPozycjeVM>();
                if (towar != "")
                {
                    procukcje = (from c in db.PRODDP
                                     from dp in db.PROD
                                     where c.nazwatw.ToUpper().Contains(towar.ToUpper())
                                     where c.id_prod == dp.id
                                     where dp.typ == 1
                                     where dp.kod_firmy == kod_firmy
                                     where dp.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                     where dp.status != 4 //wyświetl tylko te które nie są wrzucone do archiwum
                                     where dp.zakonczono != 1
                                     where dp.akceptowano != 1
                                     orderby dp.datadk
                                     select c).ToList();
                }
                else
                {
                    procukcje = (from c in db.PRODDP
                                     from dp in db.PROD
                                     
                                     where c.id_prod == dp.id
                                     where dp.typ == 1
                                     where dp.kod_firmy == kod_firmy
                                     where dp.status != 2 //wyświetl tylko te które nie są wrzucone do archiwum
                                     where dp.status != 4 //wyświetl tylko te które nie są wrzucone do archiwum
                                     where dp.zakonczono != 1
                                     where dp.akceptowano != 1
                                     orderby dp.datadk
                                     select c).ToList();
                }                
            
                foreach (PRODDP p in procukcje)
                {
                    
                        _produkcje.Add(new ProdukcjaPozycjeVM { IsNew = false, Pozycja = p });
                        _produkcje.Last().GetProd();
                    
                }
                Produkcje = _produkcje;

                ProdukcjeView = CollectionViewSource.GetDefaultView(Produkcje);
                ProdukcjeView.GroupDescriptions.Add(new PropertyGroupDescription("Produkcja.nazwa"));



                //groupByCustomerCommand = new GroupByCustomerCommand(this);
                //groupByYearMonthCommand = new GroupByYearMonthCommand(this);
                //removeGroupCommand = new RemoveGroupCommand(this);

                RaisePropertyChanged("ProdukcjeView");
            }
        }
        public void RemoveGroup()
        {
            ProdukcjeView.GroupDescriptions.Clear();
            ProdukcjeView.GroupDescriptions.Add(new PropertyGroupDescription("noGroup"));
        }

        public void GroupByCustomer()
        {
            ProdukcjeView.GroupDescriptions.Clear();
            ProdukcjeView.GroupDescriptions.Add(new PropertyGroupDescription("Produkcja.nazwa"));
        }

        public void GroupByYearMonth()
        {
            ProdukcjeView.GroupDescriptions.Clear();
            ProdukcjeView.GroupDescriptions.Add(new PropertyGroupDescription("orderYearMonth"));
        }

        public ICommand groupByCustomerCommand
        {
            get;
            private set;
        }

        public ICommand groupByYearMonthCommand
        {
            get;
            private set;
        }

        public ICommand removeGroupCommand
        {
            get;
            private set;
        }

    }
    public class GroupsToTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ReadOnlyObservableCollection<object>)
            {
                var items = (ReadOnlyObservableCollection<object>)value;
                Decimal total = 0;
                foreach (ProdukcjaPozycjeVM element in items)
                {
                    total += 1;
                }
                return total.ToString();
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class GroupsToOpisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ReadOnlyObservableCollection<object>)
            {
                var items = (ReadOnlyObservableCollection<object>)value;
                String opis = "";
                foreach (ProdukcjaPozycjeVM element in items)
                {
                    element.GetProd();
                    opis = element.Produkcja.opis;
                }
                return opis;



            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public class SelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is ReadOnlyObservableCollection<object>)
            {
                var items = (ReadOnlyObservableCollection<object>)value;
                String opis = "";
                foreach (ProdukcjaPozycjeVM element in items)
                {
                    element.GetProd();
                    opis = element.Produkcja.opis;
                }
                return opis;



            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
