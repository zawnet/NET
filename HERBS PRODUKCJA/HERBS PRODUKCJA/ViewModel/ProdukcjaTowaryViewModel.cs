using GalaSoft.MvvmLight.Command;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Views;
using HERBS_PRODUKCJA.Views.RowVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class ProdukcjaTowaryViewModel : CrudVMBase
    {

        public string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();

        public string mg_typ;
        public string typ_produktu;
            
        public string MG_TYP
        {
            get { return mg_typ; }
            set
            {
                mg_typ = value;
                RaisePropertyChanged("MG_TYP");
            }
        }

        public ObservableCollection<ViewVM> Views { get; set; }
        public ProdukcjaMGDwVM SelectedProdDW { get; set; }
        public List<ProdukcjaMGDwVM> WybraneDostawy { get; set; }
        public List<ProdukcjaMGDwVM> ProdDWs { get; set; }
        public List<ProdukcjaMGDwVM> ProdDWs_Data { get; set; }
        public RelayCommand WybierzCommand { get; set; }
        public RelayCommand OdswiezCommand { get; set; }
        public RelayCommand HistoriaCommand { get; set; }
        public RelayCommand RaportCommand { get; set; }
        public RelayCommand WydaniaCommand { get; set; }

        private ICollection<PROD_MZ_SPEC> _specyfikacje;
        public ICollection<PROD_MZ_SPEC> Specyfikacje
        {
            get
            {
                return _specyfikacje;
            }
            set
            {
                _specyfikacje = value;
                RaisePropertyChanged("Specyfikacje");
            }
        }

        private ICollection<PROD_MGPW> _HistoriaDostawy;
        public ICollection<PROD_MGPW> HistoriaDostawy
        {
            get
            {
                return _HistoriaDostawy;
            }
            set
            {
                _HistoriaDostawy = value;
                RaisePropertyChanged("HistoriaDostawy");
            }
        }


        private ICollection<HistoriaZlecenia> _HistoriaDostawyZlecenia;
        public ICollection<HistoriaZlecenia> HistoriaDostawyZlecenia
        {
            get
            {
                return _HistoriaDostawyZlecenia;
            }
            set
            {
                _HistoriaDostawyZlecenia = value;
                RaisePropertyChanged("HistoriaDostawyZlecenia");
            }
        }


        private RelayCommand<object> _SelectedItemChangedCommand;
        public RelayCommand<object> SelectedItemChangedCommand
        {
           get { 
           return _SelectedItemChangedCommand ?? (_SelectedItemChangedCommand = new RelayCommand<object>(
                    p =>
                    {
                        // Execute delegate
                        PobierzSPEC((p as ProdukcjaMGDwVM));
                      
                    },
                    p =>
                    {
                        // CanExecute delegate
                        return true;
                    }));
            }
        }
        private RelayCommand<object> _SelectedMgDWChangedCommand;
        public RelayCommand<object> SelectedMgDWChangedCommand
        {
            get
            {
                return _SelectedMgDWChangedCommand ?? (_SelectedMgDWChangedCommand = new RelayCommand<object>(
                         p =>
                         {
                        // Execute delegate
                        PobierzHistorieDostawy((p as ProdukcjaMGDwVM));
                             PobierzZlecenia((p as ProdukcjaMGDwVM));
                         },
                         p =>
                         {
                        // CanExecute delegate
                        return true;
                         }));
            }
        }


        private RelayCommand<object> _commandShowPopup;
        public RelayCommand<object> CommandShowPopup
        {
            get
            {
                return _commandShowPopup ?? (_commandShowPopup = new RelayCommand<object>(
                    p =>
                    {
                        // Execute delegate
                        (p as Popup).IsOpen = true;
                    },
                    p =>
                    {
                        // CanExecute delegate
                        return true;
                    }));
            }
        }

        private RelayCommand<object> _ustalTypProduktuCommand;
        public RelayCommand<object> UstalTypProduktuCommand
        {
            get
            {
                return _ustalTypProduktuCommand ?? (_ustalTypProduktuCommand = new RelayCommand<object>(
                    p =>
                    {
                        // Execute delegate
                        UstalTypyProduktow((p as String));
                      
                    },
                    p =>
                    {
                        // CanExecute delegate
                        return true;
                    }));
            }
        }

        private RelayCommand<object> _WydajCommand;
        public RelayCommand<object> WydajCommand
        {
            get
            {
                return _WydajCommand ?? (_WydajCommand = new RelayCommand<object>(
                    p =>
                    {
                        // Execute delegate
                        PrzekazDoWZ((p as String));

                    },
                    p =>
                    {
                        // CanExecute delegate
                        return true;
                    }));
            }
        }

        private RelayCommand<object> _SpecyfikacjaEditCommand;
        public RelayCommand<object> SpecyfikacjaEditCommand
        {
            get
            {
                return _SpecyfikacjaEditCommand ?? (_SpecyfikacjaEditCommand= new RelayCommand<object>(
                    p =>
                    {
                        // Execute delegate
                        EdytujSpecyfikacje((p as ProdukcjaMGDwVM));
                    },
                    p =>
                    {
                        // CanExecute delegate
                        return true;
                    }));
            }
        }

        private void EdytujSpecyfikacje(ProdukcjaMGDwVM produkcjaMGDwVM)
        {
            if (produkcjaMGDwVM != null && produkcjaMGDwVM.ProdukcjaDW.PROD_MZ != null)
                try
                {

                    ProdukcjaMagazynPozycjaVM pozVM = new ProdukcjaMagazynPozycjaVM();
                    pozVM.ProdukcjaMZ = (from mz in db.PROD_MZ
                                         where mz.id == produkcjaMGDwVM.ProdukcjaDW.PROD_MZ.id
                                         select mz).First();

                    ProdukcjaSpecyfikacjaPakowaniaWindow widnow = new ProdukcjaSpecyfikacjaPakowaniaWindow(pozVM);
                    widnow.ShowDialog();

                }
                catch
                {
                    throw;
                }
        }

        private RelayCommand<string> _pobierzWgTypuCommand;
        /// <summary>
        /// Gets the TestAgainCommand.
        /// </summary>
        public RelayCommand<string> PobierzWgTypuCommand
        {
            get
            {
                return _pobierzWgTypuCommand?? (_pobierzWgTypuCommand = new RelayCommand<string>(
                    p =>
                    {
                        // Execute delegate
                        throw new NotImplementedException();
                    },
                    p =>
                    {
                        // CanExecute delegate
                        throw new NotImplementedException();
                    }));
            }
        }

        private bool _AllSelected;
        public bool AllSelected
        {
            get { return _AllSelected; }
            set
            {
                _AllSelected = value;
                ProdDWs.ToList().ForEach(x => x.IsSelected = value);
                RaisePropertyChanged("AllSelected");
            }
        }

        private string dw;
        public string DW
        {
            get { return dw; }
            set
            {
                dw = value;
                RaisePropertyChanged("DW");
            }
        }

        private string kh;
        public string KH
        {
            get { return kh; }
            set
            {
                kh = value;
                RaisePropertyChanged("KH");
            }
        }
        private string tw;
        public string TW
        {
            get { return tw; }
            set
            {
                tw = value;
                RaisePropertyChanged("TW");
            }
        }
       
        private string _tWName;
        public string TWName
        {
            get { return _tWName; }
            set
            {
                _tWName = value;
                RaisePropertyChanged("TWName");
                //on text changed - clear all values
                // SelectedWorkOrder = null;
            }
        }

        private string _serchTxt;
        public string serchTxt
        {
            get { return _serchTxt; }
            set
            {
                _serchTxt = value;
                RaisePropertyChanged("serchTxt");
                //on text changed - clear all values
                // SelectedWorkOrder = null;
            }
        }

        //Obsługa Hostorii dostaw--------------------------------
        public ICollectionView ProdDWView { set; get; }
        IList<ProdukcjaMGDwVM> DostawyProdMG { get; set; }
        public ICollectionView TowaryView { set; get; }
        public List<PROD_MGDW> TowaryList { get; set; }
        public ProdukcjaMGDwVM SelectedMgDW
        {
            get; set;
        }
        //-------------------------------------------------------
        public RelayCommand GetWorkOrderCommand
        {
            get; set;
        }
        public RelayCommand serchProdMgDWCommand
        {
            get; set;
        }

       
        private bool CanGetWorkOrder()
        {
            string name = (TWName ?? "").ToString();
            return !string.IsNullOrWhiteSpace(name);
        }

        private void GetWorkOrder(object parameter)
        {
            string name = TWName;

        }
        protected void SearchData()
        {

            ProdDWs = (from c in ProdDWs_Data
                            where (
                            (c.ProdukcjaDW.kodtw != null && c.ProdukcjaDW.kodtw.ToUpper().Contains(_tWName.ToUpper()) )
                            || (c.ProdukcjaDW.nazwatw != null && c.ProdukcjaDW.nazwatw.ToUpper().Contains(_tWName.ToUpper()) )
                            || (c.ProdukcjaDW.nr_partii != null && c.ProdukcjaDW.nr_partii.ToUpper().Contains(_tWName.ToUpper()) )
                            )
                            select c).ToList();
          
            RaisePropertyChanged("ProdDWs");

        }
        protected async override void GetData()
        {

            double iloscprod;
            string typdw = "";
            string typ_produktu = "";
            typ_produktu = mg_typ;

            string[] kodtws = { };


            iloscprod = 0;
            List<ProdukcjaMGDwVM> _proddws = new List<ProdukcjaMGDwVM>();
            db = new FZLEntities1();
            List<PROD_MGDW> dostawy = new List<PROD_MGDW>();
            
            if (typ_produktu != "ALL")
            {
                dostawy = (from p in db.PROD_MGDW
                           where p.kod_firmy == kod_firmy
                           //where kodtws.Any(t => p.kodtw.Contains(t)) == false
                           where p.typ_produktu == typ_produktu
                           where ((p.stan) > 0)
                           orderby p.nazwatw
                           select p).ToList();
                
            }
            else
            {
                dostawy = (from p in db.PROD_MGDW

                           where p.kod_firmy == kod_firmy
                           //where kodtws.Any(t => p.kodtw.Contains(t)) == false
                           //where p.typ_produktu == typ_produktu
                           where ((p.stan) > 0)
                           orderby p.nazwatw
                           select p).ToList();
            }


            /*
            if (mg_typ == null)
                typdw = "PZS";
            else if (mg_typ == "PZS")
            {
                typdw = "PZS";
                var statuses = new[] { "surowiec", "zakup", "zew" };
                kodtws = statuses;
            }
            else if (mg_typ == "SWP")
            {
                typdw = "PZS";
            }
            else if (mg_typ == "PP")
            {
                typdw = "PWWG";
                var statuses = new[] { "PP"};
                kodtws = statuses;

            }
            else if (mg_typ == "PU")
            {
                typdw = "PWWG";
                var statuses = new[] { "PU" };
                kodtws = statuses;

            }
            else if (mg_typ == "WG")
            {
                typdw = "PWWG";
                var statuses = new[] { "PP", "PU", "surowiec"};
                kodtws = statuses;
            }
            else if (mg_typ == "O")
            {
                typdw = "PZO";
               
            }

            db = new FZLEntities1();
            List<PROD_MGDW> dostawy;
            if (kodtws.Count() > 0)
            {
                if (mg_typ == "WG")
                {
                    dostawy = (from p in db.PROD_MGDW

                               where p.kod_firmy == kod_firmy
                               where kodtws.Any(t => p.kodtw.Contains(t)) == false
                               where p.typdw == typdw
                               where ((p.stan) > 0)
                               orderby p.nazwatw
                               select p).ToList();
                }
                else
                {
                    dostawy = (from p in db.PROD_MGDW

                               where p.kod_firmy == kod_firmy
                               where kodtws.Any(t => p.kodtw.Contains(t)) == true
                               where p.typdw == typdw
                               where ((p.stan) > 0)
                               orderby p.nazwatw
                               select p).ToList();
                }
                
                
            }
            else
            {
                dostawy = (from p in db.PROD_MGDW

                               where p.kod_firmy == kod_firmy
                              // where kodtws.Any(t => p.kodtw.Contains(t)) == true
                               where p.typdw == typdw
                               where ((p.stan) > 0)
                               orderby p.nazwatw
                               select p).ToList();

            }
            */
            

            foreach (PROD_MGDW prod in dostawy)
            {

               
                if (prod.stan > 0)
                    _proddws.Add(new ProdukcjaMGDwVM { IsNew = false, ProdukcjaDW = prod, ProdukcjaDW_List =  new ProdukcjaMGDwVM.ProdukcjaDWList() });
            }

            foreach(ProdukcjaMGDwVM obj in _proddws)
            {
                iloscprod = SumujRezerwacjeDostawy(obj.ProdukcjaDW.id);

                obj.ProdukcjaDW_List.iloscdost = (obj.ProdukcjaDW.iloscdost - iloscprod);
                PROD_PRODMGPW pw = obj.ProdukcjaDW.PROD_PRODMGPW.FirstOrDefault();
                if (pw != null)
                {
                    obj.ProdukcjaDW_List.frakcja = pw.PRODDP.frakcja;
                }
                var prodmz = obj.ProdukcjaDW.PROD_MZ;
                if (prodmz != null)
                {
                    if(prodmz.PROD_MG.opis != "")
                    {
                        obj.ProdukcjaDW_List.opis = prodmz.PROD_MG.opis + "\n";
                    }
                    obj.ProdukcjaDW_List.opis += prodmz.opisdod;
                }
                
            }

            ProdDWs_Data = _proddws;
            ProdDWs = _proddws;
            RaisePropertyChanged("ProdDWs");
            if (WybraneDostawy != null && WybraneDostawy.Count() > 0)
            {
                WybraneDostawy.Clear();
            }

        }



        public double SumujRezerwacjeDostawy(int iddw)
        {
            var typydk = new[] { "RWS", "RWO", "WZWG" };

            var suma = (from d in db.PROD_PRODMGPW
                        where d.id_prodmgdw == iddw
                        where (d.id_dkprodmg == null || (d.id_dkprodmg > 0 && typydk.Any(t => d.PROD_MZ.PROD_MG.typ_dk.Contains(t)) == false) && (d.id_proddp > 0 && d.PRODDP.rodzaj_dk == "RW"))
                        select d.ilosc).ToList();
            if (suma.Count() == 0)
                return 0;
            else
                return (double)suma.Sum();
        

    }

        public void GetSelected()
        {
            if (this.WybraneDostawy != null && this.WybraneDostawy.Count > 0)
            {

            }
            else
            {
                this.WybraneDostawy = new List<ProdukcjaMGDwVM>();
            }
            foreach (ProdukcjaMGDwVM obj in ProdDWs)
            {
                if (obj.IsSelected)
                {
                    obj.ProdukcjaDW.stan = obj.ProdukcjaDW.iloscpz - SumujRezerwacjeDostawy(obj.ProdukcjaDW.id);
                    if (!WybraneDostawy.Contains(obj))
                    {
                        WybraneDostawy.Add(obj);
                    }
                }
                else
                {
                    if (WybraneDostawy.Contains(obj))
                    {
                        WybraneDostawy.Remove(obj);
                    }
                           
                      
                   
                }
            }
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
           // WyborProdMGDwWindow parent = Application.Current.Windows.OfType<WyborProdMGDwWindow>().First();
           // parent.selDW = WybraneDostawy;
           // parent.DialogResult = true;
          //  parent.Close();
            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

        public void UstalTypyProduktow(String typ)
        {
            GetSelected();
            if(WybraneDostawy != null && WybraneDostawy.Count>0)
            {
                foreach (ProdukcjaMGDwVM obj in WybraneDostawy)
                {
                    obj.ProdukcjaDW.typ_produktu = typ;
                    obj.ProdukcjaDW.PROD_MZ.typ_produktu = typ;
                    obj.IsSelected = false;
                    //WybraneDostawy.Remove(obj);
                }
                db.SaveChanges();
                
                GetData();
            }
            else
            {
                MessageBox.Show("Brak wybranych produktów");
            }
           
        }

        public void PobierzDW(string typ)
        {
            System.Windows.MessageBox.Show(typ);
            mg_typ = typ;
            GetData();
        }

        public void PrzekazDoWZ(string tymdk)
        {
            GetSelected();
            string info;
            ProdukcjaMagazynDokumentViewModel vm = new ProdukcjaMagazynDokumentViewModel();
            if (WybraneDostawy != null && WybraneDostawy.Count > 0)
            {
                info = "";
                ProdukcjaMagazynVM ProdukcjaMagazynDokument = new ProdukcjaMagazynVM { ProdukcjaMG = new PROD_MG() };
                foreach (ProdukcjaMGDwVM obj in WybraneDostawy)
                {
                    info += obj.ProdukcjaDW.kod + '\n';
                    
                }
                ProdukcjaMagazynDokument.ProdukcjaMG.kod_firmy = kod_firmy;
                ProdukcjaMagazynDokument.ProdukcjaMG.data = System.DateTime.Now;
                vm.PobierzProdMGTypy();

                ProdukcjaMagazynDokument.ProdukcjaMG.typ_dk = "WZWG";
                


                vm.ProdukcjaMagazyn = ProdukcjaMagazynDokument;
               
                vm.SProdMgTyp = vm.ProdMGTypy.FirstOrDefault(x => x.Kod == vm.ProdukcjaMagazyn.ProdukcjaMG.typ_dk);
                vm.wstawPozycjeDW(WybraneDostawy,2);
                //RaisePropertyChanged("ProdukcjaMagazyn");

                ProdukcjaMagazynWindow dialog = new ProdukcjaMagazynWindow(vm);
                dialog.ShowDialog();
                GetData();
                //MessageBox.Show(info);

               
                vm.ProdukcjaMagazyn = ProdukcjaMagazynDokument;


            }
            else
            {
                MessageBox.Show("Brak wybranych produktów");
            }
           

            
           

        }

        public ProdukcjaTowaryViewModel(): base()
        {
            OdswiezCommand = new RelayCommand(GetData);
            HistoriaCommand = new RelayCommand(Historia);
            _pobierzWgTypuCommand = new RelayCommand<string>(i => PobierzDW(i));
            //SelectedItemChangedCommand = new RelayCommand<ProdukcjaMGDwVM>(i=>PobierzSPEC(i));
            GetData();
            GetWorkOrderCommand = new RelayCommand(SearchData);
            serchProdMgDWCommand = new RelayCommand(SearchHistoriaDostawy);
            RaportCommand = new RelayCommand(Raport);
            WydaniaCommand = new RelayCommand(Wydania);
        }

        private void Wydania()
        {
            SpecyfikacjaWzWindow window = new SpecyfikacjaWzWindow();
            window.ShowDialog();

        }

        private void SearchHistoriaDostawy()
        {
            GetALLTwList(serchTxt);
        }

        private void Historia()
        {
            //throw new NotImplementedException();
            GetALLTwList();
            HistoriaTowarowWindow dialog = new HistoriaTowarowWindow(this);
            dialog.ShowDialog();

        }


        private void PobierzHistorieDostawy(ProdukcjaMGDwVM produkcjaMGDwVM)
        {

           //  PROD_MGPW test = new PROD_MGPW();
            //   test.PROD_MZ.
            if (produkcjaMGDwVM != null)
            {
                if(produkcjaMGDwVM.ProdukcjaDW.PROD_MGPW != null)
                {
                    HistoriaDostawy = produkcjaMGDwVM.ProdukcjaDW.PROD_MGPW;
                    RaisePropertyChanged("HistoriaDostawy");
                }
            }
        }

        private void PobierzZlecenia(ProdukcjaMGDwVM produkcjaMGDwVM)
        {

           //  PROD_PRODMGPW test = new PROD_PRODMGPW();
           //    test.PRODDP.prod
            if (produkcjaMGDwVM != null)
            {
                if (produkcjaMGDwVM.ProdukcjaDW.PROD_PRODMGPW != null)
                {
                    HistoriaDostawyZlecenia = (from pw in db.PROD_PRODMGPW
                                        from dp in db.PRODDP
                                        from prod in db.PROD
                                        where pw.id_prodmgdw == produkcjaMGDwVM.ProdukcjaDW.id
                                        where pw.id_proddp == dp.id
                                        where dp.id_prod == prod.id
                                        select new HistoriaZlecenia{id = prod.id, ilosc = pw.ilosc, data=(DateTime)prod.data, nazwa=prod.nazwa, opis=prod.opis,
                                        osoba=prod.osoba,
                                        charakter= dp.rodzaj_dk.Contains("RW") ?  "WYDANY DO PRODUKCJI" : "OTRZYMANY PO PRODUKCJI"
                                        }).ToList();
                        
                        
                    RaisePropertyChanged("HistoriaDostawyZlecenia");
                }
            }
        }

        public DataSet PrzygotujDaneDoRaportu()
        {
          
            DataSet ds = new DataSet();
            // Create 2 DataTable instances.
            DataTable prodDW_dt = new DataTable("REPORT_PRODMGDW");
            prodDW_dt.Columns.Add("typ_produktu");
            prodDW_dt.Columns.Add("kod");
            prodDW_dt.Columns.Add("nr_partii");
            prodDW_dt.Columns.Add("data");
            prodDW_dt.Columns.Add("khnazwa");
            prodDW_dt.Columns.Add("kodtw");
            prodDW_dt.Columns.Add("iloscpz");
            prodDW_dt.Columns.Add("stan");
            prodDW_dt.Columns.Add("iloscprod");
            prodDW_dt.Columns.Add("iloscdost");
            prodDW_dt.Columns.Add("frakcja");
            prodDW_dt.Columns.Add("opis");

            foreach(ProdukcjaMGDwVM dw in ProdDWs)
            {
                prodDW_dt.Rows.Add(dw.ProdukcjaDW.typ_produktu, dw.ProdukcjaDW.kod, dw.ProdukcjaDW.nr_partii, dw.ProdukcjaDW.data, dw.ProdukcjaDW.khnazwa, dw.ProdukcjaDW.kodtw, dw.ProdukcjaDW.iloscpz, dw.ProdukcjaDW.stan, dw.ProdukcjaDW.iloscprod, dw.ProdukcjaDW_List.iloscdost, dw.ProdukcjaDW_List.frakcja, dw.ProdukcjaDW_List.opis);  
            }

            ds.Tables.Add(prodDW_dt);
            return ds;
        }

        public void Raport()
        {
            string url1 = @"Raporty\StanyTowarowPRODMG.frx";
            
            if (url1 != "")
            {
                ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
                dialog.ShowDialog();
            }

        }

        private void PobierzSPEC(ProdukcjaMGDwVM i)
        {
           // PROD_MGPW test = new PROD_MGPW();
         //   test.PROD_MZ.PROD_MG.kod
            if(i != null)
            {
                if(i.ProdukcjaDW.PROD_MZ.PROD_MZ_SPEC != null)
                {
                    Specyfikacje = i.ProdukcjaDW.PROD_MZ.PROD_MZ_SPEC;
                    RaisePropertyChanged("Specyfikacje");
                }
                //MessageBox.Show(i.ProdukcjaDW.PROD_MZ.kod);
            }

            //throw new NotImplementedException();
        }

        // pobiera liste wszystkich towarow do jakich byla dostawa
        private void GetALLTwList(string filter = "")
        {

            ObservableCollection<ProdukcjaMGDwVM> _dostawy = new ObservableCollection<ProdukcjaMGDwVM>();

          //  using (FZLEntities1 db2 = new FZLEntities1())
          //  {
                if (filter == null || filter == "")
                {
                    this.TowaryList =
                (from twdw in db.PROD_MGDW
                 where twdw.kod_firmy == kod_firmy
                 // group twdw by twdw.idtw into GroupTW
                 orderby twdw.kodtw,twdw.data
                 
                 select twdw).ToList();
                }
                else
                {
                    this.TowaryList =
              (from twdw in db.PROD_MGDW
               where twdw.kod_firmy == kod_firmy
               where (twdw.kodtw.Contains(filter) || twdw.nr_partii.Contains(filter) || twdw.khkod.Contains(filter))
               // group twdw by twdw.idtw into GroupTW
               orderby twdw.kodtw, twdw.data
               select twdw).ToList();
                }
                foreach (PROD_MGDW dw in TowaryList)
                {

                    _dostawy.Add(new ProdukcjaMGDwVM { IsNew = false, ProdukcjaDW = dw });

                }
                TowaryView = CollectionViewSource.GetDefaultView(_dostawy);
                TowaryView.GroupDescriptions.Add(new PropertyGroupDescription("ProdukcjaDW.kodtw"));
                RaisePropertyChanged("TowaryView");

            //}

        }

        public void RemoveGroup()
        {
            TowaryView.GroupDescriptions.Clear();
            TowaryView.GroupDescriptions.Add(new PropertyGroupDescription("noGroup"));
        }

        public void GroupByCustomer()
        {
            TowaryView.GroupDescriptions.Clear();
            TowaryView.GroupDescriptions.Add(new PropertyGroupDescription("ProdukcjaDW.twid"));
        }

        public void GroupByYearMonth()
        {
            TowaryView.GroupDescriptions.Clear();
            TowaryView.GroupDescriptions.Add(new PropertyGroupDescription("orderYearMonth"));
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

        public class HistoriaZlecenia {
           public int id { get; set; }
            public string nazwa { get; set; }
            public DateTime data { get; set; }
            public double ilosc { get; set; }
            public string opis { get; set; }
            public string osoba { get; set; }
            public string charakter { get; set; }

            public HistoriaZlecenia()
            {

            }
        }

    }


    

    
    


}
