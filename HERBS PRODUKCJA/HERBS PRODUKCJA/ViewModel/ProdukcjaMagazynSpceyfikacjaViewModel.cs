using GalaSoft.MvvmLight.Command;
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
using System.Windows;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class ProdukcjaMagazynSpceyfikacjaViewModel : CrudVMBase
    {
        private string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        private ProdukcjaMagazynPozycjaVM _selectedProdMZ;
        public ProdukcjaMagazynPozycjaVM SelectedProdMZ
        {
            get { return _selectedProdMZ; }
            set
            {
                _selectedProdMZ = value;
                RaisePropertyChanged("SelectedProdMZ");
            }
        }

        private ProdukcjaMagazynSpecyfikacjaVM _selectedSpecyfikacja;
        public ProdukcjaMagazynSpecyfikacjaVM SelectedSpecyfikacja
        {
            get { return _selectedSpecyfikacja; }
            set
            {
                _selectedSpecyfikacja = value;
                RaisePropertyChanged("SelectedSpecyfikacja");
            }
        }

        private ObservableCollection<ProdukcjaMagazynPozycjaVM> pozycjeMZ;
        public ObservableCollection<ProdukcjaMagazynPozycjaVM> PozycjeMZ
        {
            get
            {
                return pozycjeMZ;
            }
            set
            {
                pozycjeMZ = value;
                RaisePropertyChanged("PozycjeMZ");
            }
        }

        private ObservableCollection<ProdukcjaMagazynSpecyfikacjaVM> _specyfikacje;
        public ObservableCollection<ProdukcjaMagazynSpecyfikacjaVM> Specyfikacje
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

        private List<ProdukcjaMagazynSpecyfikacjaVM> SpecToDel;

        public RelayCommand DodajSpecCommand { get; set; }
        public RelayCommand UsunSpecCommand { get; set; }
        public RelayCommand ZapiszSpecCommand { get; set; }

        public ObservableCollection<OPAKOWANIA_RODZAJE> Opakowania { get; set; }
        public ObservableCollection<MAGAZYNY> Magazyny { get; set; }

        public void PobierzSpcecyfikacjePozycji(PROD_MZ prodmz)
        {
            SpecToDel = new List<ProdukcjaMagazynSpecyfikacjaVM>();
            using (FZLEntities1 db = new FZLEntities1())
            {
                var query = (from s in db.PROD_MZ_SPEC
                             where s.id_prodmz == prodmz.id
                             select s).ToList();

                if(query != null && query.Count() > 0)
                {
                    //pozycjeMZ = new ObservableCollection<ProdukcjaMagazynPozycjaVM>();
                    Specyfikacje = new ObservableCollection<ProdukcjaMagazynSpecyfikacjaVM>();
                    foreach (PROD_MZ_SPEC spec in query)
                    {
                        Specyfikacje.Add(new ProdukcjaMagazynSpecyfikacjaVM { Specyfikacja = spec, IsNew = false });
                    }
                }
                else
                {

                    //pozycjeMZ = new ObservableCollection<ProdukcjaMagazynPozycjaVM>();

                }
            }
        }

        public void wstawSpecyfikacje()
        {
            if (_selectedProdMZ != null)
            {
                if (Specyfikacje == null)
                {
                    Specyfikacje = new ObservableCollection<ProdukcjaMagazynSpecyfikacjaVM>();
                }

                ProdukcjaMagazynSpecyfikacjaVM monitVM = new ProdukcjaMagazynSpecyfikacjaVM();
                monitVM.lp = (short)(PozycjeMZ.IndexOf(_selectedProdMZ));
                monitVM.Specyfikacja.kodtw = _selectedProdMZ.ProdukcjaMZ.kod;

                monitVM.Specyfikacja.data = DateTime.Now;
                monitVM.Specyfikacja.id_prodmz = _selectedProdMZ.ProdukcjaMZ.id;
                monitVM.Specyfikacja.idtw = (int)_selectedProdMZ.ProdukcjaMZ.idtw;
                monitVM.Specyfikacja.godzina = DateTime.Now.ToShortTimeString();
                Specyfikacje.Add(monitVM);
                RaisePropertyChanged("Specyfikacje");
                //this.RaisePropertyChanged("ProdukcjaPozycjeMonitTMP");

            }
        }

        public void usunSpecyfikacje()
        {
            if (SpecToDel == null)
                SpecToDel = new List<ProdukcjaMagazynSpecyfikacjaVM>();
            if(SelectedSpecyfikacja != null)
            {
                SelectedSpecyfikacja.IsDeleted = true;
                SpecToDel.Add(SelectedSpecyfikacja);
                Specyfikacje.Remove(SelectedSpecyfikacja);

               
            }
        }

        protected void PobierzOpakowania()
        {
           // using (FZLEntities1 db = new FZLEntities1())
           // {
                var query = (from c in db.OPAKOWANIA_RODZAJE
                             select c).AsNoTracking().ToList();
                Opakowania = new ObservableCollection<OPAKOWANIA_RODZAJE>();
                foreach (OPAKOWANIA_RODZAJE opak in query)
                {
                    Opakowania.Add(opak);
                }
           // }
            this.RaisePropertyChanged("Opakowania");
        }

        protected void PobierzMgazyny()
        {
            var query = (from c in db.MAGAZYNY
                         select c).ToList();
            Magazyny = new ObservableCollection<MAGAZYNY>();
            foreach (MAGAZYNY mag in query)
            {
                Magazyny.Add(mag);
            }
            this.RaisePropertyChanged("Magazyny");
        }

        private void Zapisz()
        {
         
            foreach(ProdukcjaMagazynSpecyfikacjaVM spec in Specyfikacje)
            {
               
                Save(spec.Specyfikacja);
            }
            foreach (ProdukcjaMagazynSpecyfikacjaVM spec in SpecToDel)
            {
                if(spec.IsDeleted)
                    Delete(spec.Specyfikacja);
            }
            PobierzSpcecyfikacjePozycji(SelectedProdMZ.ProdukcjaMZ);
           
        }

        public void Save(PROD_MZ_SPEC entity)
        {
            if (entity.id == 0)
            {
                db.PROD_MZ_SPEC.Add(entity);
                db.SaveChanges();
            }
            else
            {
                using (var context = new FZLEntities1())
                {
                    context.Entry(entity).State = EntityState.Modified;
                    context.SaveChanges(); //Must be in using block
                }
            }
        }

        private void Delete(PROD_MZ_SPEC entity)
        {
            
            if (entity.id != 0)
            {
                using (var context = new FZLEntities1())
                {
                    context.PROD_MZ_SPEC.Attach(entity);
                    context.PROD_MZ_SPEC.Remove(entity);
                    context.SaveChanges();
                }
                
            }
        }

    


        public ProdukcjaMagazynSpceyfikacjaViewModel() :    base()
        {
            ProdukcjaSpecyfikacjaPakowaniaWindow parent = Application.Current.Windows.OfType<ProdukcjaSpecyfikacjaPakowaniaWindow>().First();
            PobierzOpakowania();
            PobierzMgazyny();
            this.pozycjeMZ = new ObservableCollection<ProdukcjaMagazynPozycjaVM>();
            PozycjeMZ.Add(parent.PozycjaMZ);
            SelectedProdMZ = PozycjeMZ.First();
            DodajSpecCommand = new RelayCommand(wstawSpecyfikacje);
            UsunSpecCommand = new RelayCommand(usunSpecyfikacje);
            ZapiszSpecCommand = new RelayCommand(Zapisz);
            RaisePropertyChanged("SelectedProdMZ");
            //MessageBox.Show(parent.PozycjaMZ.ProdukcjaMZ.kod);
            RaisePropertyChanged("PozycjeMZ");
            PobierzSpcecyfikacjePozycji(SelectedProdMZ.ProdukcjaMZ);
        }

    }
}
