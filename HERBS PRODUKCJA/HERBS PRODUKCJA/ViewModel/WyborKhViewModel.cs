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
    public class WyborKhViewModel : CrudVMBase
    {
        private string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        public ObservableCollection<ProdukcjaKhVM> ProdKHs { get; set; }
        public ProdukcjaKhVM SelectedKH { get; set; }
        public List<ProdukcjaKhVM> WybraneKHs { get; set; }
        public RelayCommand WybierzCommand { get; set; }
        private bool _AllSelected;
        public bool AllSelected
        {
            get { return _AllSelected; }
            set
            {
                _AllSelected = value;
                ProdKHs.ToList().ForEach(x => x.IsSelected = value);
                RaisePropertyChanged("AllSelected");
            }
        }

        public WyborKhViewModel() 
             : base()
        {
            WybierzCommand = new RelayCommand(GetSelected);
            GetWorkOrderCommand = new RelayCommand(SearchData);
        }
        
        /**
         * Pobiera dane z tabeli KH  według kodu firmy
         */
        protected override async void GetData()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                ObservableCollection<ProdukcjaKhVM> _prodkhs = new ObservableCollection<ProdukcjaKhVM>();
                var  khs = await(from p in db.KH
                           where p.kod_firmy == kod_firmy
                           orderby p.kod
                           select p).ToListAsync();

                foreach (KH kh in khs)
                {
                    _prodkhs.Add(new ProdukcjaKhVM { IsNew = false, ProdukcjaKh = kh });
                }
                ProdKHs = _prodkhs;

                RaisePropertyChanged("ProdKHs");
            }

        }

        //bound to the work order name textbox
        private string _khName;
        public string KHName
        {
            get { return _khName; }
            set
            {
                _khName = value;
                RaisePropertyChanged("KHName");
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
            string name = (KHName ?? "").ToString();
            return !string.IsNullOrWhiteSpace(name);
        }

        private void GetWorkOrder(object parameter)
        {
            string name = KHName;

        }

        protected void SearchData()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {

                string search = KHName;
                //System.Windows.Forms.MessageBox.Show(search);
                ObservableCollection<ProdukcjaKhVM> _prodkhs = new ObservableCollection<ProdukcjaKhVM>();

                var khs = db.KH.Where(t =>
            (t.kod.ToUpper().Contains(search.ToUpper()) ||
            t.nazwa.ToUpper().Contains(search.ToUpper())) &&
            t.kod_firmy == kod_firmy).ToList();

                foreach (KH kh in khs)
                {
                    _prodkhs.Add(new ProdukcjaKhVM { IsNew = false, ProdukcjaKh = kh });
                }
                ProdKHs = _prodkhs;
                // System.Windows.Forms.MessageBox.Show(search+" znaleziono:"+ProdKHs.Count.ToString());
                RaisePropertyChanged("ProdKHs");
            }

        }

        public void GetSelected()
        {
            if (this.WybraneKHs != null && this.WybraneKHs.Count > 0)
            {

            }
            else
            {
                this.WybraneKHs = new List<ProdukcjaKhVM>();
            }
            foreach (ProdukcjaKhVM obj in ProdKHs)
            {

                if (obj.IsSelected)
                {
                    WybraneKHs.Add(obj);
                }
            }
        
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborKhWindow parent = Application.Current.Windows.OfType<WyborKhWindow>().First();
            parent.selKHs = WybraneKHs;
            parent.selKH = SelectedKH;
            parent.DialogResult = true;
            parent.Close();
            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

    }
}
