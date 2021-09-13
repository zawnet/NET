using GalaSoft.MvvmLight.Command;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class PartieSYMViewModel : CrudVMBase
    {
        public ObservableCollection<PartiaSymVM> PartieSYM { get; set; }
        public RelayCommand DodajPariteCommand { get; set; }
        public RelayCommand EdytujPariteCommand { get; set; }
        public RelayCommand ZapiszPariteCommand { get; set; }
        private PartiaSymVM _selectedPartiaSYM;
        public PartiaSymVM SelectedPartiaSYM
        {
            get { return _selectedPartiaSYM; }
            set
            {
                _selectedPartiaSYM = value;
                RaisePropertyChanged("SelectedPartiaSYM");
            }
        }

        public PartieSYMViewModel() : base()
        {
           DodajPariteCommand = new RelayCommand(DodajPartie);
            ZapiszPariteCommand = new RelayCommand(ZapiszPartie);
            EdytujPariteCommand = new RelayCommand(EdytujPartie);
            WczytajPartie();
        }

        private void ZapiszPartie()
        {
            if(_selectedPartiaSYM.IsNew == true)
            {
                db.PARTIE_SYM.Add(_selectedPartiaSYM.PartiaSYM);
              
            }
            db.SaveChanges();
            _selectedPartiaSYM.IsNew = false;
            
            DialogWindow parent = Application.Current.Windows.OfType<DialogWindow>().First();
            parent.DialogResult = true;
            parent.Close();
        }

        private void WczytajPartie()
        {
            db = new FZLEntities1();
            var query = (from p in db.PARTIE_SYM
                         orderby p.data_przyjecia
                         select p
                         ).ToList();
            PartieSYM = new ObservableCollection<PartiaSymVM>();
            foreach (PARTIE_SYM p in query)
            {
                PartieSYM.Add(new PartiaSymVM { PartiaSYM = p, IsNew = false });
            }
            RaisePropertyChanged("PartieSYM");
        }

        private void EdytujPartie()
        {
            EditPartiaSYMView editControl = new EditPartiaSYMView(this);
            DialogWindow dialog = new DialogWindow(editControl);

            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                //wstawPozycjeSYM(dialog.selTW);
                WczytajPartie();
            }
        }

        private void DodajPartie()
        {
            _selectedPartiaSYM = new PartiaSymVM{ PartiaSYM = new PARTIE_SYM(), IsNew=true };
            _selectedPartiaSYM.PartiaSYM.typ = 1;
            _selectedPartiaSYM.PartiaSYM.data_przyjecia = System.DateTime.Now;
        
            EditPartiaSYMView editControl = new EditPartiaSYMView(this);
            DialogWindow dialog = new DialogWindow(editControl);

            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                //wstawPozycjeSYM(dialog.selTW);
                WczytajPartie();
            }
            
          
        }
    }
}
