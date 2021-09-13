using HERBS_PRODUKCJA;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HERBS_PRODUKCJA.Model;
using GalaSoft.MvvmLight.Command;
using HERBS_PRODUKCJA.Views.RowVM;
using HERBS_PRODUKCJA.Views;
using System.Windows;
using HERBS_PRODUKCJA.Helpers;
using System.ComponentModel;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class WyborProdMGDwViewModel : CrudVMBase
    {
        private string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        public ObservableCollection<ViewVM> Views { get; set; }
        public ProdukcjaMGDwVM SelectedProdDW { get; set; }
        public List<ProdukcjaMGDwVM> WybraneDostawy { get; set; }
        public List<ProdukcjaMGDwVM.ProdukcjaDWList> dostawyDostepne { get; set; }
        public ObservableCollection<ProdukcjaMGDwVM> ProdDWs { get; set; }
        public ObservableCollection<ProdukcjaMGDwVM> SrchProdDWs { get; set; }

        public RelayCommand WybierzCommand { get; set; }
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


        public RelayCommand GetWorkOrderCommand
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
            if (this.dostawyDostepne == null)
            {
                PobierzDostawy();
            }
            string search = TWName;
            double iloscprod;
            iloscprod = 0;
            List<ProdukcjaMGDwVM.ProdukcjaDWList> dostawySrc = new List<ProdukcjaMGDwVM.ProdukcjaDWList>();
            ProdDWs = new ObservableCollection<ProdukcjaMGDwVM>();
            if (search.Length > 2 && this.dostawyDostepne != null)
            {
                // using (FZLEntities1 db2 = new FZLEntities1())
                // {
                dostawySrc = (
                    from t in this.dostawyDostepne
                    where
                    (t.kod.ToUpper().Contains(search.ToUpper())
                    ||
                    t.khnazwa != null && t.khnazwa.ToUpper().Contains(search.ToUpper())
                    ||
                    (t.kodtw != null && t.kodtw.ToUpper().Contains(search.ToUpper()))

                     ||
                    (t.nr_partii != null && t.nr_partii.ToUpper().Contains(search.ToUpper()))
                    )
                    select t).ToList();
               /*     
                    this.dostawyDostepne.Where(t => (
            t.kod.ToUpper().Contains(search.ToUpper()) ||
            t.khnazwa.ToUpper().Contains(search.ToUpper()) ||
            t.kodtw.ToUpper().Contains(search.ToUpper()))
            ).ToList();
        */
               /// &&
            ///    (t.iloscdost) > 0
             //   &&
             //   (t.kod_firmy == kod_firmy)).ToList();

                    foreach (ProdukcjaMGDwVM.ProdukcjaDWList prod in dostawySrc)
                    {

                        //iloscprod = SumujRezerwacjeDostawy(prod.id);
                      //  prod.iloscdost = (prod.iloscdost - iloscprod);
                        // MessageBox.Show(iloscprod.ToString());
                       // if (prod.stan > 0)
                     //   {
                            //_proddws.Add(new ProdukcjaMGDwVM { IsNew = false, ProdukcjaDW = prod, ProdukcjaPRODDP = Functions.PobierzZlecenieDlaDostawy(prod, db) });
                            ProdDWs.Add(new ProdukcjaMGDwVM { IsNew = false, ProdukcjaDW_List = prod, ProdukcjaDW = db.PROD_MGDW.Where(x=>x.id==prod.id).FirstOrDefault() });
                     //   }
                    }
                   // ProdDWs = new ObservableCollection<ProdukcjaMGDwVM>();
                   // ProdDWs = _proddws;
                    WyborProdMGDwWindow parent = Application.Current.Windows.OfType<WyborProdMGDwWindow>().First();
                    ;
                    if (parent.selDW != null && parent.selDW.Count > 0)
                    {

                        foreach (ProdukcjaMGDwVM obj in parent.selDW)
                        {
                            foreach (ProdukcjaMGDwVM obj2 in ProdDWs)
                                if (obj.ProdukcjaDW.id == obj2.ProdukcjaDW.id)
                                {
                                    obj2.IsSelected = true;
                                }
                        }

                    }
                }
         //   }
          RaisePropertyChanged("ProdDWs");

        }
        public void PobierzDostawy()
        {
            double iloscprod;
            iloscprod = 0;
            //ObservableCollection<ProdukcjaMGDwVM> _proddws = new ObservableCollection<ProdukcjaMGDwVM>();
            ProdDWs = new ObservableCollection<ProdukcjaMGDwVM>();
            this.dostawyDostepne = new List<ProdukcjaMGDwVM.ProdukcjaDWList>();

            

                
                var query1 = (from x in db.PROD_MGDW
                 where x.kod_firmy == kod_firmy
                              where x.iloscdost > 0
                              join y in db.PROD_PRODMGPW
                              on x.idpozpz equals y.id_dkprodmg
                              where y.id_prodmgdw == x.id
                              select new ProdukcjaMGDwVM.ProdukcjaDWList
                              {
                                  id = x.id,
                                  kod = x.kod,
                                  kodtw = x.kodtw,
                                  idtw = x.idtw,
                                  ilosc = x.ilosc,
                                  iloscdost = x.iloscdost,
                                  iloscprod = x.iloscprod,
                                  iloscpz = x.iloscpz,
                                  stan = x.stan,
                                  nr_partii = x.nr_partii,
                                  khkod = x.khkod,
                                  khnazwa = x.khnazwa,
                                  nazwatw = x.nazwatw,
                                  data = x.data,
                                  idpozpz = x.idpozpz,
                                  idkh = x.idkh,
                                  frakcja = y.PRODDP.frakcja
                              }
                              ).Union(from x in db.PROD_MGDW
                                      


                                      where x.kod_firmy == kod_firmy
                                        where !db.PROD_PRODMGPW.Any(f => f.id_dkprodmg == x.idpozpz && f.id_prodmgdw == x.id)
                                      where ((x.iloscdost) > 0)
                                      select new ProdukcjaMGDwVM.ProdukcjaDWList
                                      {
                                          id = x.id,
                                          kod = x.kod,
                                          kodtw = x.kodtw,
                                          idtw = x.idtw,
                                          ilosc = x.ilosc,
                                          iloscdost = x.iloscdost,
                                          iloscprod = x.iloscprod,
                                          iloscpz = x.iloscpz,
                                          stan = x.stan,
                                          nr_partii = x.nr_partii,
                                          khkod = x.khkod,
                                          khnazwa = x.khnazwa,
                                          nazwatw = x.nazwatw,
                                          data = x.data,
                                          idpozpz = x.idpozpz,
                                          idkh = x.idkh,
                                          frakcja = ""
                                      }

                                      ).OrderByDescending(v => v.kodtw).OrderByDescending(v => v.data).ToList();

                /*
                 var query1 = (from x in db.PROD_MGDW.AsEnumerable()

                               where x.kod_firmy == kod_firmy

                                      where ((x.iloscdost) > 0 )

                                      select new PROD_MGDW
                              {
                                  id = x.id,
                                  kod = x.kod,
                                  kodtw = x.kodtw,
                                  idtw = x.idtw,
                                  ilosc = x.ilosc,
                                  iloscdost = x.iloscdost,
                                  iloscprod = x.iloscprod,
                                  iloscpz = x.iloscpz,
                                  stan = x.stan,
                                  nr_partii = x.nr_partii,
                                  khkod = x.khkod,
                                  khnazwa = x.khnazwa,
                                  nazwatw = x.nazwatw,
                                  data = x.data,
                                  idpozpz = x.idpozpz,
                                  idkh = x.idkh
                              }).Union((from x in db.PROD_MGDW.AsEnumerable()
                                        from y in db.PROD_PRODMGPW.AsEnumerable()

                                        where (x.idpozpz == y.id_dkprodmg )
                                        where y.id_prodmgdw == x.id
                                        where x.kod_firmy == kod_firmy
                                        where ((x.iloscdost) > 0)

                                        select new PROD_MGDW
                                        {
                                            id = x.id,
                                            kod = x.kod,
                                            kodtw = x.kodtw,
                                            idtw = x.idtw,
                                            ilosc = x.ilosc,
                                            iloscdost = x.iloscdost,
                                            iloscprod = x.iloscprod,
                                            iloscpz = x.iloscpz,
                                            stan = x.stan,
                                            nr_partii = x.nr_partii,
                                            khkod = x.khkod,
                                            khnazwa = x.khnazwa,
                                            nazwatw = x.nazwatw,
                                            data = x.data,
                                            idpozpz = x.idpozpz,
                                            idkh = x.idkh,
                                            frakcja = y.PRODDP.frakcja
                                        })

                                  ).OrderByDescending(v=>v.kodtw).OrderByDescending(v=>v.data).ToList();
                */
                this.dostawyDostepne = query1;

                foreach (ProdukcjaMGDwVM.ProdukcjaDWList prod in this.dostawyDostepne)
                {

                
                    iloscprod = SumujRezerwacjeDostawy(prod.id);
                    prod.iloscdost = (prod.iloscdost - iloscprod);
                    if (prod.stan > 0)
                    {
                        ProdDWs.Add(new ProdukcjaMGDwVM { IsNew = false, ProdukcjaDW_List = prod, ProdukcjaDW = db.PROD_MGDW.Where(x => x.id == prod.id).FirstOrDefault() });
                        //_proddws.Add(new ProdukcjaMGDwVM { IsNew = false, ProdukcjaDW = prod });
                    }
                }
            foreach (ProdukcjaMGDwVM obj in ProdDWs)
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
                    if (prodmz.PROD_MG.opis != "")
                    {
                        obj.ProdukcjaDW_List.opis = prodmz.PROD_MG.opis + "\n";
                    }
                    obj.ProdukcjaDW_List.opis += prodmz.opisdod;
                }

            }
            RaisePropertyChanged("ProdDWs");
            
            //ProdDWs = _proddws;
            WyborProdMGDwWindow parent = Application.Current.Windows.OfType<WyborProdMGDwWindow>().First();
            ;
            /*
            if (parent.selDW != null && parent.selDW.Count > 0)
            {

                foreach (ProdukcjaMGDwVM obj in parent.selDW)
                {
                    foreach (ProdukcjaMGDwVM obj2 in ProdDWs)
                        if (obj.ProdukcjaDW.id == obj2.ProdukcjaDW.id)
                        {
                            obj2.IsSelected = true;
                        }
                }
            }
            */
        }

        public double SumujRezerwacjeDostawy(int iddw)
        {
            var typydk = new[] { "RWS", "RWO", "WZWG" };
            double suma = 0;

            var powiazania = (from d in db.PROD_PRODMGPW

                        where d.id_prodmgdw == iddw
                              //where (d.id_dkprodmg == null || (d.id_dkprodmg > 0 && typydk.Any(t => d.PROD_MZ.PROD_MG.typ_dk.Contains(t)) == false) && (d.id_proddp > 0 && d.PRODDP.rodzaj_dk == "RW"))
                              where (d.id_dkprodmg == null)
                              select d.ilosc).ToList();
            if (powiazania.Count() == 0)
            { suma = 0; }
                
            else
            { suma = powiazania.Sum(); }
                return suma;

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
                if (obj.IsSelected)
                {
                    obj.ProdukcjaDW_List.stan = obj.ProdukcjaDW_List.iloscpz - SumujRezerwacjeDostawy(obj.ProdukcjaDW_List.id);
                    WybraneDostawy.Add(obj);
                }
            
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborProdMGDwWindow parent = Application.Current.Windows.OfType<WyborProdMGDwWindow>().First();
            parent.selDW = WybraneDostawy;
            parent.DialogResult = true;
            parent.Close();
            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

        public WyborProdMGDwViewModel()
            : base()
        {
             PobierzDostawy();

            WybierzCommand = new RelayCommand(GetSelected);
            GetWorkOrderCommand = new RelayCommand(SearchData);
            //  if (!IsInDesignMode)
            //  {
           
               
                /*
                ObservableCollection<ViewVM> views = new ObservableCollection<ViewVM>
            {
                new ViewVM{ ViewDisplay="ProdukcjaDetale", ViewType = typeof(ProdukcjaDetaleView), ViewModelType = typeof(ProdukcjaDetaleViewModel)},

            };

                Views = views;
                RaisePropertyChanged("Views");
                */
         //   }
            
        }
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor
                    .FromProperty(prop, typeof(FrameworkElement))
                    .Metadata.DefaultValue;
            }
        }
    }
}
