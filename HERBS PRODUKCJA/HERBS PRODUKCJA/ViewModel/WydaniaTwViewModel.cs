using GalaSoft.MvvmLight.Command;
using HERBS_PRODUKCJA.Helpers;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class WydaniaTwViewModel : CrudVMBase
    {
        public SpecyfikacjePakowaniaLista SpecListaControl;
        public SpecyfikacjaPakowaniaDokument SpecDokumentControl;

        public string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();

        public RelayCommand DodajWZSpecCommand { get; set; }
        public RelayCommand WstawKupujacyCommand { get; set; }
        public RelayCommand WstawDostawaCommand { get; set; }
        public RelayCommand WstawTWCommand { get; set; }
        public RelayCommand UsunTWCommand { get; set; }
        public RelayCommand ZapiszWZCommand { get; set; }
        public RelayCommand WyjdzWZCommand { get; set; }
        public RelayCommand DodajOpakCommand { get; set; }
        public RelayCommand UsunOpakCommand { get; set; }
        public RelayCommand DrukujWZCommand { get; set; }
        public RelayCommand SelectOpakChangeCommand { get; set; }

        public ObservableCollection<WZ_SPECYFIKACJE_OPAKOWANIA> WzOpakowania { get; set; }
        public ObservableCollection<OPAKOWANIA_RODZAJE> Opakowania { get; set; }

        private List<WZ_SPECYFIKACJE_OPAKOWANIA> OpakowaniaDoUsuniecia;

        private RelayCommand<object> _SelectedTowChangeCommand;
        public RelayCommand<object> SelectedTowChangeCommand
        {
            get
            {
                return _SelectedTowChangeCommand ?? (_SelectedTowChangeCommand = new RelayCommand<object>(
                         p =>
                         {
                             // Execute delegate
                             WczytajOpakowaniaTowaru((p as WZ_SPECYFIKACJE_TOWARY));
                             //PobierzHistorieDostawy((p as ProdukcjaMGDwVM));
                             //PobierzZlecenia((p as ProdukcjaMGDwVM));
                         },
                         p =>
                         {
                             // CanExecute delegate
                             return true;
                         }));
            }
        }

        private void WczytajOpakowaniaTowaru(WZ_SPECYFIKACJE_TOWARY wZ_SPECYFIKACJE_TOWARY)
        {
            if (wZ_SPECYFIKACJE_TOWARY != null)
            {
               // _SelectedWZSPECTowarTMP = _SelectedWZSPECTowar;
                if (wZ_SPECYFIKACJE_TOWARY.WZ_SPECYFIKACJE_OPAKOWANIA != null)
                {
                   // _SelectedWZSPECTowar = wZ_SPECYFIKACJE_TOWARY;
                    WzOpakowania = new ObservableCollection<WZ_SPECYFIKACJE_OPAKOWANIA>();
                    if (wZ_SPECYFIKACJE_TOWARY.WZ_SPECYFIKACJE_OPAKOWANIA != null)
                    {
                        foreach (var obj in wZ_SPECYFIKACJE_TOWARY.WZ_SPECYFIKACJE_OPAKOWANIA)
                        {
                            WzOpakowania.Add(obj);
                        }
                    }

                }
                else
                {
                    WzOpakowania = new ObservableCollection<WZ_SPECYFIKACJE_OPAKOWANIA>();
                }
            }
            else
            {
                WzOpakowania = new ObservableCollection<WZ_SPECYFIKACJE_OPAKOWANIA>();
            }

            RaisePropertyChanged("WzOpakowania");
            RaisePropertyChanged("WydanieTowaru");
            //MessageBox.Show(wZ_SPECYFIKACJE_TOWARY.nazwatw);
            //throw new NotImplementedException();
        }

        private UserControl _ControlView;
        public UserControl ControlView
        {
            get { return _ControlView; }
            set
            {
                _ControlView = value;
                RaisePropertyChanged("ControlView");
            }
        }

        private WZ_SPECYFIKACJE_TOWARY _SelectedWZSPECTowar;
        private WZ_SPECYFIKACJE_TOWARY _SelectedWZSPECTowarTMP;
        public WZ_SPECYFIKACJE_TOWARY SelectedWZSPECTowar
        {
            get { return _SelectedWZSPECTowar; }
            set
            {
                _SelectedWZSPECTowar = value;
                RaisePropertyChanged("SelectedWZSPECTowar");
            }
        }
        private WZ_SPECYFIKACJE_OPAKOWANIA _SelectedOPAK;
        public WZ_SPECYFIKACJE_OPAKOWANIA SelectedOPAK
        {
            get { return _SelectedOPAK; }
            set
            {
                _SelectedOPAK = value;
                RaisePropertyChanged("SelectedOPAK");
            }
        }

        public WZ_SPECYFIKACJE SelectedWZSPEC
        {

            get; set;
        }

       

        private RelayCommand<int> _EdycjaWZSpecCommand;
        public RelayCommand<int> EdycjaWZSpecCommand
        {
            get
            {
                return _EdycjaWZSpecCommand ?? (_EdycjaWZSpecCommand = new RelayCommand<int>(
                         p =>
                         {
                        // Execute delegate
                        EdycjaWZSpec(p);

                         },
                         p =>
                         {
                        // CanExecute delegate
                        return true;
                         }));
            }
        }

        private void EdycjaWZSpec(int id)
        {
            ObservableCollection<WZ_SPECYFIKACJE_TOWARY> Towary = new ObservableCollection<WZ_SPECYFIKACJE_TOWARY>();
            _SelectedWZSPECTowar = null;
            WzOpakowania = new ObservableCollection<WZ_SPECYFIKACJE_OPAKOWANIA>();
            
            if (id != null)
            {
                var wz = db.WZ_SPECYFIKACJE.Find(id);
                 
                foreach (var obj in wz.WZ_SPECYFIKACJE_TOWARY)
                {
                    Towary.Add(obj);
                    obj.lp = (byte)(Towary.IndexOf(obj)+1);
                }

                _wydanieTowaru = new WydanieTowaruVM { WZSPEC = wz, WZSPEC_TOWARY = Towary, IsNew = false };
            }

            RaisePropertyChanged("WzOpakowania");
            RaisePropertyChanged("WydanieTowaru");
            SpecDokumentControl = new SpecyfikacjaPakowaniaDokument(this);
            _ControlView = SpecDokumentControl;

           
            RaisePropertyChanged("ControlView");
           
        }

        public ObservableCollection<WZ_SPECYFIKACJE> WZSpecLista { get; set; }

        private WydanieTowaruVM _wydanieTowaru;
        public WydanieTowaruVM WydanieTowaru
        {
            get { return _wydanieTowaru; }
            set
            {
                _wydanieTowaru = value;
                RaisePropertyChanged("WydanieTowaru");
            }
        }

        private void WstawKupujacego()
        {
          
            WyborKhWindow dialog = new WyborKhWindow();
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                WstawNabywce(dialog.selKH);
            }
        }

        private void WstawOdbierajacego()
        {
            WyborKhWindow dialog = new WyborKhWindow();
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                WstawOdbiorce(dialog.selKH);
            }
        }
        public void PokazTW()
        {
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborTwWindow dialog = new WyborTwWindow();
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                wstawPozycjeTW(dialog.selTW);
            }

            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

        private void wstawPozycjeTW(List<ProdukcjaTwVM> selTW)
        {
            if(_wydanieTowaru.WZSPEC_TOWARY == null)
            {
                _wydanieTowaru.WZSPEC_TOWARY = new ObservableCollection<WZ_SPECYFIKACJE_TOWARY>();
            }
            WZ_SPECYFIKACJE_TOWARY towar;
            foreach (ProdukcjaTwVM tw in selTW)
            {
                towar = new WZ_SPECYFIKACJE_TOWARY();
                towar.WZ_SPECYFIKACJE = _wydanieTowaru.WZSPEC;
                towar.nazwatw = tw.ProdukcjaTW.nazwa;
                towar.idtw = tw.ProdukcjaTW.id;
                towar.kodtw = tw.ProdukcjaTW.kod;
                _wydanieTowaru.WZSPEC_TOWARY.Add(towar);
                RaisePropertyChanged("WydanieTowaru");


            }
           
        }

        private void WstawOdbiorce(ProdukcjaKhVM selKH)
        {
            _wydanieTowaru.WZSPEC.odid      =   (int)selKH.ProdukcjaKh.id;
            _wydanieTowaru.WZSPEC.odkod     =   selKH.ProdukcjaKh.kod;
            _wydanieTowaru.WZSPEC.odnazwa   = selKH.ProdukcjaKh.nazwa;
            _wydanieTowaru.WZSPEC.odadres   =   selKH.ProdukcjaKh.ulica;
            _wydanieTowaru.WZSPEC.oddom     =   selKH.ProdukcjaKh.dom;
            _wydanieTowaru.WZSPEC.odlokal   =   selKH.ProdukcjaKh.lokal;
            _wydanieTowaru.WZSPEC.odkodpocz =   selKH.ProdukcjaKh.kodpocz;
            _wydanieTowaru.WZSPEC.odmiasto  =   selKH.ProdukcjaKh.miejscowosc;
            _wydanieTowaru.WZSPEC.odkraj    =   selKH.ProdukcjaKh.krajNazwa;
            _wydanieTowaru.WZSPEC.odnip     =   selKH.ProdukcjaKh.nip;

            _wydanieTowaru.OdbiorcaAdres = selKH.ProdukcjaKh.ulica + " " + selKH.ProdukcjaKh.dom + ",  " + selKH.ProdukcjaKh.kodpocz + " " + selKH.ProdukcjaKh.miejscowosc;
            RaisePropertyChanged("WydanieTowaru");
        }

        private void WstawNabywce(ProdukcjaKhVM selKH)
        {
           
            _wydanieTowaru.WZSPEC.khid      =   (int)selKH.ProdukcjaKh.id;
            _wydanieTowaru.WZSPEC.khkod     =   selKH.ProdukcjaKh.kod;
            _wydanieTowaru.WZSPEC.khnazwa   =   selKH.ProdukcjaKh.nazwa;
            _wydanieTowaru.WZSPEC.khadres   =   selKH.ProdukcjaKh.ulica;
            _wydanieTowaru.WZSPEC.khdom     =   selKH.ProdukcjaKh.dom;
            _wydanieTowaru.WZSPEC.khlokal   =   selKH.ProdukcjaKh.lokal;
            _wydanieTowaru.WZSPEC.khkodpocz =   selKH.ProdukcjaKh.kodpocz;
            _wydanieTowaru.WZSPEC.khmiasto  =   selKH.ProdukcjaKh.miejscowosc;
            _wydanieTowaru.WZSPEC.khkraj    =   selKH.ProdukcjaKh.krajNazwa;
            _wydanieTowaru.WZSPEC.khnip     =   selKH.ProdukcjaKh.nip;

            _wydanieTowaru.NabwcaAdres = selKH.ProdukcjaKh.ulica + " " + selKH.ProdukcjaKh.dom + ",  " + selKH.ProdukcjaKh.kodpocz + " "+ selKH.ProdukcjaKh.miejscowosc;
            RaisePropertyChanged("WydanieTowaru");
        }

        private void Zapisz()
        {
            int idSpec;
            idSpec = ZapiszDane();
            db = new FZLEntities1();
            PobierzDane();
            
           
            EdycjaWZSpec(idSpec);
            if(_SelectedWZSPECTowarTMP != null)
            {
                //_SelectedWZSPECTowar = _SelectedWZSPECTowarTMP;
                //WczytajOpakowaniaTowaru(_SelectedWZSPECTowarTMP);
            }
           
           // RaisePropertyChanged("SelectedWZSPECTowar");
            RaisePropertyChanged("WydanieTowaru");
            
           
           
           
           
        }

        private int ZapiszDane()
        {
            if(!(_wydanieTowaru.WZSPEC.id > 0))
            {
                int month_nr = _wydanieTowaru.WZSPEC.data.Value.Month;
                int year_nr = _wydanieTowaru.WZSPEC.data.Value.Year;
                int day_nr = _wydanieTowaru.WZSPEC.data.Value.Day;
                _wydanieTowaru.WZSPEC.serianr = (from c in db.WZ_SPECYFIKACJE
                                                 where c.data.Value.Month == _wydanieTowaru.WZSPEC.data.Value.Month
                                                 where c.data.Value.Year == _wydanieTowaru.WZSPEC.data.Value.Year
                                                 select c.serianr).Max();
                if (_wydanieTowaru.WZSPEC.serianr == null || _wydanieTowaru.WZSPEC.serianr < 1)
                {
                    _wydanieTowaru.WZSPEC.serianr = 1;
                }
                else
                {
                    _wydanieTowaru.WZSPEC.serianr += 1;
                }
                _wydanieTowaru.WZSPEC.kod = _wydanieTowaru.WZSPEC.serianr + "-"+day_nr.ToString("D2") + month_nr.ToString("D2") + _wydanieTowaru.WZSPEC.data.Value.ToString("yy");

                db.WZ_SPECYFIKACJE.Add(_wydanieTowaru.WZSPEC);
            }
            foreach (var towar in _wydanieTowaru.WZSPEC_TOWARY)
            {
                if (!(towar.id > 0))
                {
                    db.WZ_SPECYFIKACJE_TOWARY.Add(towar);
                }
                ObliczWagiPozycji(towar);
                foreach (var opak in towar.WZ_SPECYFIKACJE_OPAKOWANIA)
                {
                    if(!(opak.id >0 ))
                    {
                        //opak.opakowanie_nazwa = db.OPAKOWANIA_RODZAJE.Where(x => x.id == opak.id_opakowania).FirstOrDefault().nazwa_ang;
                        //opak.id_specwz = _wydanieTowaru.WZSPEC.id;
                        db.WZ_SPECYFIKACJE_OPAKOWANIA.Add(opak);
                    }
                    opak.opakowanie_nazwa = db.OPAKOWANIA_RODZAJE.Where(x => x.id == opak.id_opakowania).FirstOrDefault().nazwa_ang;
                    opak.id_specwz = _wydanieTowaru.WZSPEC.id;
                }
            }

            //jezeli są jakies opakowania do usuniaecia to usun
            foreach(WZ_SPECYFIKACJE_OPAKOWANIA sopak in OpakowaniaDoUsuniecia)
            {
                db.WZ_SPECYFIKACJE_OPAKOWANIA.Remove(sopak);
            }

            db.SaveChanges();


            return _wydanieTowaru.WZSPEC.id;
        }

        private void PobierzDane()
        {
            db = new FZLEntities1();
            WZSpecLista = new ObservableCollection<WZ_SPECYFIKACJE>();
            OpakowaniaDoUsuniecia = new List<WZ_SPECYFIKACJE_OPAKOWANIA>();
            //using (var db2 = new FZLEntities1())
            // {
            var query = db.WZ_SPECYFIKACJE.Where(x => x.kod_firmy == kod_firmy).ToList();
            foreach(var obj in query)
            {
                WZSpecLista.Add(obj);
            }
           // }
            RaisePropertyChanged("WZSpecLista");
          
        }

        private void DodajWZ()
        {
            SpecDokumentControl = new SpecyfikacjaPakowaniaDokument(this);
            _ControlView = SpecDokumentControl;
            
            //MessageBox.Show("Dodaj");
            RaisePropertyChanged("ControlView");
            _wydanieTowaru = new WydanieTowaruVM { WZSPEC = new WZ_SPECYFIKACJE(), WZSPEC_TOWARY = new ObservableCollection<WZ_SPECYFIKACJE_TOWARY>(), WZSPEC_OPAKOWANIA = new ObservableCollection<WZ_SPECYFIKACJE_OPAKOWANIA>(), IsNew = true };
            _wydanieTowaru.WZSPEC.kod_firmy = kod_firmy;
            _wydanieTowaru.WZSPEC.data = DateTime.Now;
            RaisePropertyChanged("WydanieTowaru");
            
        }
        private void WyjdzWZ()
        {
            _ControlView = SpecListaControl;
            _SelectedWZSPECTowar = null;
            _SelectedWZSPECTowarTMP = null;
            WzOpakowania = null;
            OpakowaniaDoUsuniecia.Clear();
            PobierzDane();
            RaisePropertyChanged("ControlView");
        }

        private void UsunOpakowanie()
        {
           if(_SelectedOPAK != null && _SelectedOPAK.id > 0 && _SelectedOPAK.id_wztow == _SelectedWZSPECTowar.id)
            {
                OpakowaniaDoUsuniecia.Add(_SelectedOPAK);
                _SelectedWZSPECTowar.WZ_SPECYFIKACJE_OPAKOWANIA.Remove(_SelectedOPAK);
            }
            WczytajOpakowaniaTowaru(_SelectedWZSPECTowar);

            RaisePropertyChanged("SelectedWZSPECTowar");
            RaisePropertyChanged("WydanieTowaru");
            
        }

        private void DodajOpakowanie()
        {
            if (_SelectedWZSPECTowar != null)
            {
              
                var obj = new WZ_SPECYFIKACJE_OPAKOWANIA();

               // obj.WZ_SPECYFIKACJE_TOWARY = SelectedWZSPECTowar;
                _SelectedWZSPECTowar.WZ_SPECYFIKACJE_OPAKOWANIA.Add(obj);
                WzOpakowania.Add(obj);
            }
            RaisePropertyChanged("SelectedWZSPECTowar");
            RaisePropertyChanged("WydanieTowaru");

        }
        protected void PobierzOpakowania()
        {
           
            var query = (from c in db.OPAKOWANIA_RODZAJE
                         select c).AsNoTracking().ToList();
            Opakowania = new ObservableCollection<OPAKOWANIA_RODZAJE>();
            foreach (OPAKOWANIA_RODZAJE opak in query)
            {
                Opakowania.Add(opak);
            }
            
            this.RaisePropertyChanged("Opakowania");
        }
        private void ZmienRodzajOpakowania()
        {
            MessageBox.Show("test");
        }

        private void ObliczWagiPozycji(WZ_SPECYFIKACJE_TOWARY towar)
        {
            double? wnetto, wbrutto, wopakowania;
            wnetto = 0;
            wopakowania = 0;
            foreach (var opak in towar.WZ_SPECYFIKACJE_OPAKOWANIA)
            {
                if (opak.ilosc_opakowania > 0)
                {
                    if(opak.waga_opakowania > 0)
                    {
                        wopakowania += opak.ilosc_opakowania * opak.waga_opakowania;
                    }

                    if (opak.opakowanie_nazwa != "pallets")
                    {

                        if (opak.ilewopakowaniu > 0)
                        {
                            wnetto += (opak.ilosc_opakowania * opak.ilewopakowaniu);
                        }
                        else
                        {
                            MessageBox.Show("Podano niekompletne dane do wyliczeń !" + "\\n" + "Sprawdź czy podałeś ilosc opakowania oraz ilość w opakowaniu.");
                        }
                    }
                }
                
                
            }
            if (wnetto > 0)
            {
                towar.wnetto = wnetto;
                
            }
            if(wopakowania > 0 && wnetto > 0)
            {
                towar.wbrutto = wnetto + wopakowania;
            }
            RaisePropertyChanged("WydanieTowaru");
        }

        public DataSet PrzygotujDaneDoRaportu()
        {
            //FZLDataSetTableAdapters.PRODTableAdapter prodta = new FZLDataSetTableAdapters.PRODTableAdapter();

            var WZSPECYFIKACJE = db.WZ_SPECYFIKACJE.Where(i=>i.id ==_wydanieTowaru.WZSPEC.id).ToDataTable();
            var WZSPECYFIKACJE_TOWARY = db.WZ_SPECYFIKACJE_TOWARY.Where(i => i.id_wzspec == _wydanieTowaru.WZSPEC.id).ToDataTable();
            var WZSPECYFIKACJE_OPAKOWANIA = db.WZ_SPECYFIKACJE_OPAKOWANIA.Where(i => i.id_specwz == _wydanieTowaru.WZSPEC.id).ToDataTable();
            var firma = db.FIRMA.Where(x => x.kod == kod_firmy).ToDataTable();
            var opakowania_rodzaje = db.OPAKOWANIA_RODZAJE.ToDataTable();
            DataSet ds = new DataSet();

            WZSPECYFIKACJE.TableName = "WZ_SPECYFIKACJE";
            ds.Tables.Add(WZSPECYFIKACJE);

            WZSPECYFIKACJE_TOWARY.TableName = "WZ_SPECYFIKACJE_TOWARY";
            ds.Tables.Add(WZSPECYFIKACJE_TOWARY);

            WZSPECYFIKACJE_OPAKOWANIA.TableName = "WZ_SPECYFIKACJE_OPAKOWANIA";
            ds.Tables.Add(WZSPECYFIKACJE_OPAKOWANIA);

            opakowania_rodzaje.TableName = "OPAKOWANIA_RODZAJE";
            ds.Tables.Add(opakowania_rodzaje);

            firma.TableName = "FIRMA";
            ds.Tables.Add(firma);


            return ds;
        }

        public void Raport()
        {
            string url1 = url1 = @"Raporty\PackingList.frx";
       
           
            
            if (url1 != "")
            {
                ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
                dialog.ShowDialog();
            }

        }

        private void Drukuj()
        {
            Raport();
        }

        public WydaniaTwViewModel() : base()
        {
            
            PobierzDane();
            PobierzOpakowania();
            if (_wydanieTowaru == null)
            {
               
            }
            WstawKupujacyCommand    =   new RelayCommand(WstawKupujacego);
            WstawDostawaCommand     =   new RelayCommand(WstawOdbierajacego);
            WstawTWCommand          =   new RelayCommand(PokazTW);
            ZapiszWZCommand         =   new RelayCommand(Zapisz);
            DodajWZSpecCommand      =   new RelayCommand(DodajWZ);
            WyjdzWZCommand          =   new RelayCommand(WyjdzWZ);
            SelectOpakChangeCommand = new RelayCommand(ZmienRodzajOpakowania);
            SpecListaControl        =   new SpecyfikacjePakowaniaLista(this);
            DodajOpakCommand        =   new RelayCommand(DodajOpakowanie);
            UsunOpakCommand         =   new RelayCommand(UsunOpakowanie);
            DrukujWZCommand         = new RelayCommand(Drukuj);

            _ControlView = SpecListaControl;
        }

       
    }
}
