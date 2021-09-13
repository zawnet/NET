using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Views;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.Messages;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class ProdukcjaMagazynViewModel : CrudVMBase
    {
        public string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        private ObservableCollection<ProdukcjaMagazynVM> _produkcjaMagazyn { get; set; }
        public ObservableCollection<ProdukcjaMagazynVM> ProdukcjaMagazyn { get; set; }
            /*
        {
            get { return _produkcjaMagazyn; }
            set
            {
                _produkcjaMagazyn = value;
                RaisePropertyChanged("ProdukcjaMgazyn");
            }
        }
        */
        public ProdukcjaMagazynVM SelectedProdMG
        {

            get; set;
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

        public RelayCommand NowyDokumentCommand { get; set; }
        public RelayCommand EdytujDokumentCommand { get; set; }
        public RelayCommand OdswiezCommand { get; set; }
        public RelayCommand YearChangedCommand { get; set; }
        public RelayCommand WydaniaCommand { get; set; }
        public List<string> ProdYears { get; set; }
        public int okres { get; set; }


        protected override void GetData()
        {
            using (db = new FZLEntities1())
            {

                _produkcjaMagazyn = new ObservableCollection<ProdukcjaMagazynVM>();
                
                var query = (from c in db.PROD_MG
                             where c.okres == okres
                             where c.kod_firmy == kod_firmy
                             select c).ToList();
                foreach (PROD_MG dkmg in query)
                {
                    _produkcjaMagazyn.Add(new ProdukcjaMagazynVM { IsNew = false, ProdukcjaMG = dkmg });
                }
                ProdukcjaMagazyn = _produkcjaMagazyn;
                RaisePropertyChanged("ProdukcjaMagazyn");
                //MessageBox.Show("Wczytano dane");
            }
        }

        public void getData(ProdukcjaMagazynVM prodmgvm)
        {
            ProdukcjaMagazyn = new ObservableCollection<ProdukcjaMagazynVM>();
            using (FZLEntities1 db = new FZLEntities1())
            {
                var query = (from c in db.PROD_MG
                             where c.kod_firmy == kod_firmy
                             select c).ToList();
                foreach (PROD_MG dkmg in query)
                {
                    ProdukcjaMagazyn.Add(new ProdukcjaMagazynVM { IsNew = false, ProdukcjaMG = dkmg });
                }
                // ProdukcjaMagazyn = _produkcjaMagazyn;

            }
            RaisePropertyChanged("ProdukcjaMgazyn");
            SelectedProdMG = prodmgvm;
            RaisePropertyChanged("SelectedProdMG");

        }

        public void NowyDokument()
        {
            db = new FZLEntities1();
            ProdukcjaMagazynVM prodmg = new ProdukcjaMagazynVM { ProdukcjaMG = new PROD_MG(), IsNew = true };
            ProdukcjaMagazynDokumentViewModel vm = new ProdukcjaMagazynDokumentViewModel();
           // prodmg.ProdukcjaMG = new PROD_MG();
            prodmg.ProdukcjaMG.data = DateTime.Today;
            prodmg.ProdukcjaMG.typ_dk = "PZS";
            prodmg.ProdukcjaMG.kod_firmy = kod_firmy;
            vm.getProdukcjaMGvm(prodmg);
            ProdukcjaMagazynWindow window = new ProdukcjaMagazynWindow(vm);
            window.Show();
           // Messenger.Default.Send(prodmg);
            //MessageBox.Show("Nowy dokument");
        }

        public void EdytujDokument()
        {
            if (SelectedProdMG != null && SelectedProdMG.ProdukcjaMG.id > 0)
            {
                db = new FZLEntities1();
                ProdukcjaMagazynVM prodmg = new ProdukcjaMagazynVM();
                ProdukcjaMagazynDokumentViewModel vm = new ProdukcjaMagazynDokumentViewModel();

                prodmg.IsNew = false;
               
                var mg = db.PROD_MG.Where(x => x.id == SelectedProdMG.ProdukcjaMG.id).FirstOrDefault();
                prodmg.ProdukcjaMG = mg;
                vm.getProdukcjaMGvm(prodmg);
                ProdukcjaMagazynWindow window = new ProdukcjaMagazynWindow(vm);
                window.Show();

                //Messenger.Default.Send(prodmg);
            }    
        }
        public void GetProdYEARS()
        {
            using (db = new FZLEntities1())
            {
                ProdYears = db.PROD_MG.Where(x => x.kod_firmy == kod_firmy).Select(s => s.data.Value.Year.ToString()).Distinct().ToList();
                RaisePropertyChanged("ProdYears");
            }
        }

        private void ChangeYear()
        {
            this.okres = int.Parse(PROD_year);

            GetData();
        }
        private void Wydania()
        {
            SpecyfikacjaWzWindow window = new SpecyfikacjaWzWindow();
            window.ShowDialog();

        }

        public ProdukcjaMagazynViewModel()
            : base()
        {
            _PROD_year = System.DateTime.Now.Year.ToString();
            RaisePropertyChanged("PROD_year");
            this.okres = int.Parse(_PROD_year);
            Messenger.Default.Register<ProdukcjaMagazynVM>(this, prodmgvm => this.RefreshData());
            NowyDokumentCommand         = new RelayCommand(NowyDokument);
            EdytujDokumentCommand       = new RelayCommand(EdytujDokument);
            OdswiezCommand              = new RelayCommand(this.RefreshData);
            YearChangedCommand          = new RelayCommand(ChangeYear);
            WydaniaCommand              = new RelayCommand(Wydania);
            GetProdYEARS();
            GetData();
           
        }

    }
}
