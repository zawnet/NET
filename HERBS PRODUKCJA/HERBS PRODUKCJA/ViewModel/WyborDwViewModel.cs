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

namespace HERBS_PRODUKCJA.ViewModel
{
    public class WyborDwViewModel : CrudVMBase
    {
        private string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        public ObservableCollection<ViewVM> Views { get; set; }
        public ProdukcjaDwVM SelectedProdDW { get; set; }
        public List<ProdukcjaDwVM> WybraneDostawy { get; set; }
        public ObservableCollection<ProdukcjaDwVM> ProdDWs { get; set; }
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
            string search = TWName;
            double iloscprod;
            iloscprod = 0;
            ObservableCollection<ProdukcjaDwVM> _proddws = new ObservableCollection<ProdukcjaDwVM>();

            var dostawy = db.PROD_HMDW.Where(t => (
        t.kod.ToUpper().Contains(search.ToUpper()) ||
        t.khnazwa.ToUpper().Contains(search.ToUpper()) ||
        t.kodtw.ToUpper().Contains(search.ToUpper()) )
        &&
        (t.iloscdosp) > 0
        &&
        t.kod_firmy == kod_firmy).ToList();

            foreach (PROD_HMDW prod in dostawy)
            {

                iloscprod = SumujRezerwacjeDostawy(prod.id);
                prod.stan = (prod.iloscdosp - iloscprod);
               // MessageBox.Show(iloscprod.ToString());
                if (prod.stan > 0)
                    _proddws.Add(new ProdukcjaDwVM { IsNew = false, ProdukcjaDW = prod, ProdukcjaPRODDP = Functions.PobierzPozycjePrzezDokument(prod.iddkpz) });
            }
            ProdDWs = new ObservableCollection<ProdukcjaDwVM>();
            ProdDWs = _proddws;
            WyborDwWindow parent = Application.Current.Windows.OfType<WyborDwWindow>().First();
            ;
            if (parent.selDW != null && parent.selDW.Count > 0)
            {

                foreach (ProdukcjaDwVM obj in parent.selDW)
                {
                    foreach (ProdukcjaDwVM obj2 in ProdDWs)
                        if (obj.ProdukcjaDW.id == obj2.ProdukcjaDW.id)
                        {
                            obj2.IsSelected = true;
                        }
                }

            }
            RaisePropertyChanged("ProdDWs");

        }
        protected async override void GetData()
        {
            double iloscprod;
            iloscprod = 0;
            ObservableCollection<ProdukcjaDwVM> _proddws = new ObservableCollection<ProdukcjaDwVM>();
            var dostawy = await (from p in db.PROD_HMDW
                                 
                                 where p.kod_firmy == kod_firmy
                                 
                                  where ((p.iloscdosp) > 0 )
                                  orderby p.opistw
                                  select p).ToListAsync();
            
            foreach (PROD_HMDW prod in dostawy)
            {
                
                iloscprod = SumujRezerwacjeDostawy(prod.id);
                prod.stan = (prod.iloscdosp - iloscprod);
                if(prod.stan > 0)
                    _proddws.Add(new ProdukcjaDwVM{ IsNew=false, ProdukcjaDW = prod, ProdukcjaPRODDP = Functions.PobierzPozycjePrzezDokument(prod.iddkpz) } );
            }
            ProdDWs = _proddws;
            WyborDwWindow parent = Application.Current.Windows.OfType<WyborDwWindow>().First();
            ;
            if (parent.selDW != null && parent.selDW.Count > 0)
            {

                foreach (ProdukcjaDwVM obj in parent.selDW)
                {
                    foreach (ProdukcjaDwVM obj2 in ProdDWs)
                        if (obj.ProdukcjaDW.id == obj2.ProdukcjaDW.id)
                        {
                            obj2.IsSelected = true;
                        }
                }

            }
            RaisePropertyChanged("ProdDWs");
           
        }

        public double SumujRezerwacjeDostawy(int iddw)
        {
            var statusy = new[] { 0, 1};
           var suma  = (from d in db.PROD_PW
                        from d2 in db.PROD
                        where d.id_prod == d2.id
                        where d2.exp_hm != 1
                        where d2.status != 2
                        where d.id_proddw == iddw
                        select d.ilosc).Sum();
            if (suma == null)
                return 0;
            else
                return (double)suma;

        }

        public void GetSelected()
        {
            if (this.WybraneDostawy != null && this.WybraneDostawy.Count > 0)
            {

            }
            else
            {
                this.WybraneDostawy = new List<ProdukcjaDwVM>();
            }
            foreach (ProdukcjaDwVM obj in ProdDWs)
                if (obj.IsSelected)
                {
                    obj.ProdukcjaDW.stan = obj.ProdukcjaDW.iloscdosp - SumujRezerwacjeDostawy(obj.ProdukcjaDW.id);
                    WybraneDostawy.Add(obj);
                }
            
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborDwWindow parent = Application.Current.Windows.OfType<WyborDwWindow>().First();
            parent.selDW = WybraneDostawy;
            parent.DialogResult = true;
            parent.Close();
            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

        public WyborDwViewModel()
            : base()
        {
            //GetData();
            WybierzCommand = new RelayCommand(GetSelected);
            GetWorkOrderCommand = new RelayCommand(SearchData);
            ObservableCollection<ViewVM> views = new ObservableCollection<ViewVM>
            {
                new ViewVM{ ViewDisplay="ProdukcjaDetale", ViewType = typeof(ProdukcjaDetaleView), ViewModelType = typeof(ProdukcjaDetaleViewModel)},

            };

            Views = views;
            RaisePropertyChanged("Views");
            
        }
        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }
    }
}
