using GalaSoft.MvvmLight.Command;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class WyborTwViewModel : CrudVMBase
    {
        private string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        public ProdukcjaTwVM SelectedProdTW { get; set; }
        public List<ProdukcjaTwVM> WybraneTowary { get; set; }
        public ObservableCollection<ProdukcjaTwVM> ProdTWs { get; set; }
        public RelayCommand WybierzCommand { get; set; }
        private bool _AllSelected;
        public bool AllSelected
        {
            get { return _AllSelected; }
            set
            {
                _AllSelected = value;
                ProdTWs.ToList().ForEach(x => x.IsSelected = value);
                RaisePropertyChanged("AllSelected");
            }
        }

        
        //bound to the work order name textbox
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
            get;set;
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
        protected async override void GetData()
        {

            ObservableCollection<ProdukcjaTwVM> _prodtws = new ObservableCollection<ProdukcjaTwVM>();
            var towary = await (from p in db.PROD_HMTW
                                 where p.kod_firmy==kod_firmy
                                 where p.rodzaj != 65880
                                 where p.rodzaj != 66491
                                orderby p.kod
                                 select p).ToListAsync();

            foreach (PROD_HMTW tw in towary)
            {
                _prodtws.Add(new ProdukcjaTwVM { IsNew = false, ProdukcjaTW = tw });
            }
            ProdTWs = _prodtws;
          
            RaisePropertyChanged("ProdTWs");

        }

        protected void SearchData()
        {
            string search = TWName;
            ObservableCollection<ProdukcjaTwVM> _prodtws = new ObservableCollection<ProdukcjaTwVM>();
            
            var towary = db.PROD_HMTW.Where(t =>
        (t.kod.ToUpper().Contains(search.ToUpper()) ||
        t.nazwa.ToUpper().Contains(search.ToUpper())) &&
        t.kod_firmy == kod_firmy && t.rodzaj != 66491 && t.rodzaj != 65880).ToList();
             
            foreach (PROD_HMTW tw in towary)
            {
                _prodtws.Add(new ProdukcjaTwVM { IsNew = false, ProdukcjaTW = tw });
            }
            ProdTWs = _prodtws;
            //MessageBox.Show(search+" znaleziono:"+ProdTWs.Count.ToString());
            RaisePropertyChanged("ProdTWs");

        }
        public void GetSelected()
        {
            if (this.WybraneTowary != null && this.WybraneTowary.Count > 0)
            {

            }
            else
            {
                this.WybraneTowary = new List<ProdukcjaTwVM>();
            }
            foreach (ProdukcjaTwVM obj in ProdTWs)
                if (obj.IsSelected)
                {
                    WybraneTowary.Add(obj);
                }

            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborTwWindow parent = Application.Current.Windows.OfType<WyborTwWindow>().First();
            parent.selTW = WybraneTowary;
            parent.DialogResult = true;
            parent.Close();
            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

       

        public WyborTwViewModel()
            : base()
        {
            //GetData();
            WybierzCommand = new RelayCommand(GetSelected);
            GetWorkOrderCommand = new RelayCommand(SearchData);
            

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
