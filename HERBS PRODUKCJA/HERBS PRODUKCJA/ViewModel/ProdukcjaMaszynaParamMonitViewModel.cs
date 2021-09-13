using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.ViewModel;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Views;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class ProdukcjaMaszynaParamMonitViewModel : CrudVMBase
    {
        UZYTKOWNICY user;
        private ProdukcjaMaszynaParametrMonitVM _maszynamonit;
        public ProdukcjaMaszynaParametrMonitVM MaszynaMonit
        {
            get { return _maszynamonit; }
            set
            {
                _maszynamonit = value;
                RaisePropertyChanged("MaszynaMonit");
            }
        }
        private List<PROD_MASZYNY_PARAM> _maszynaParametry;
        public List<PROD_MASZYNY_PARAM> MaszynaParametry
        {
            get { return _maszynaParametry; }
            set
            {
                _maszynaParametry = value;
                RaisePropertyChanged("MaszynaParametry");
            }
        }

        private PROD_MASZYNY_PARAM _selectedParam;
        public PROD_MASZYNY_PARAM SelectedParam
        {
            get { return _selectedParam; }
            set
            {
                _selectedParam = value;
                RaisePropertyChanged("SelectedParam");
            }
        }

        public RelayCommand zapiszCommand { get; set; }
        public RelayCommand anulujCommand { get; set; }

        public ProdukcjaMaszynaParamMonitViewModel()
            : base()
        {
            user = (UZYTKOWNICY)App.Current.Properties["UserLoged"];
            
            Messenger.Default.Register<ProdukcjaMaszynaVM>(this, prodvm => WczytajMaszyne(prodvm));
            Messenger.Default.Register<ProdukcjaMaszynaParametrMonitVM>(this, prodvm => WczytajMonit(prodvm));
           

            zapiszCommand = new RelayCommand(Zapisz);
            anulujCommand = new RelayCommand(Anuluj);
            // ProdukcjaMaszynaParamMonitWindow parent = Application.Current.Windows.OfType<ProdukcjaMaszynaParamMonitWindow>().First();
            //_maszynamonit.MaszynaMonit.id_prod_maszyny_pw = parent.Maszyna.MaszynaPW.id;
            // _maszynamonit.MaszynaMonit.PROD_MASZYNY_PW = parent.Maszyna.MaszynaPW;
        }

        public void WczytajMaszyne(ProdukcjaMaszynaVM m)
        {
            _maszynamonit = new ProdukcjaMaszynaParametrMonitVM();
            //MessageBox.Show(m.MaszynaPW.nazwa_maszyny);
            if (m.MaszynaPW.id != null && m.MaszynaPW.id > 0)
            {
               // using (FZLEntities1 db = new FZLEntities1())
              //  {
                    _maszynamonit.MaszynaMonit = new PROD_MASZYNY_MONIT();
                    _maszynamonit.nazwa_maszyny = db.PROD_MASZYNY.Where(x => x.id == m.MaszynaPW.id_maszyny).FirstOrDefault().nazwa;
                    _maszynamonit.MaszynaMonit.id_prod = m.MaszynaPW.id_prod;
                    _maszynamonit.MaszynaMonit.id_prod_maszyny_pw = m.MaszynaPW.id;
                    _maszynamonit.MaszynaMonit.data = DateTime.Now;
                    _maszynamonit.MaszynaMonit.czas = DateTime.Now.ToShortTimeString();
                    _maszynamonit.MaszynaMonit.uzytkownik = user.nazwa;
                    _maszynamonit.MaszynaMonit.ilosc_pracownikow = db.PROD_MASZYNY.Where(x => x.id == m.MaszynaPW.id_maszyny).FirstOrDefault().ilosc_osob;
                    RaisePropertyChanged("MaszynaMonit");
                    WczytajParametry(m);
              //  }
                //MessageBox.Show(m.MaszynaPW.nazwa_maszyny);
            }
            RaisePropertyChanged("MaszynaMonit");
        }



        public void WczytajMonit(ProdukcjaMaszynaParametrMonitVM monit)
        {
            _maszynamonit = new ProdukcjaMaszynaParametrMonitVM();
            // using (FZLEntities1 db = new FZLEntities1())
            // {
            //db = new FZLEntities1();
            //MaszynaMonit = new ProdukcjaMaszynaParametrMonitVM { IsNew=false, MaszynaMonit = monit.MaszynaMonit};
            _maszynamonit.MaszynaMonit = db.PROD_MASZYNY_MONIT.Where(x=>x.id == monit.MaszynaMonit.id).First();
            if (!(_maszynamonit.MaszynaMonit.ilosc_pracownikow > 0))
            {
                _maszynamonit.MaszynaMonit.ilosc_pracownikow = db.PROD_MASZYNY.Where(x => x.id == _maszynamonit.MaszynaMonit.PROD_MASZYNY_PW.id_maszyny).FirstOrDefault().ilosc_osob;
            }
            ProdukcjaMaszynaVM m = new ProdukcjaMaszynaVM { IsNew = false, MaszynaPW = db.PROD_MASZYNY_PW.Where(x=>x.id== MaszynaMonit.MaszynaMonit.id_prod_maszyny_pw).FirstOrDefault() };
                
               
                WczytajParametry(m);
            RaisePropertyChanged("MaszynaMonit");
            //}
        }

        public void WczytajParametry(ProdukcjaMaszynaVM m)
        {
           // using (FZLEntities1 db = new FZLEntities1())
          //  {
                _maszynaParametry = (from p in db.PROD_MASZYNY_PARAM
                                     where p.id_maszyny == m.MaszynaPW.id_maszyny
                                     where p.czy_monit == 1
                                    where p.aktywny == 1
                                     select p).ToList();
                RaisePropertyChanged("MaszynaParametry");
         //   }
        }

        public void Zapisz()
        {
            List<PROD_MASZYNY_PW> maszyny_zl_pw = new List<PROD_MASZYNY_PW>();
            List<int> maszyny_zl = new List<int>();
            PROD_MASZYNY_MONIT monit_zl;
            PROD_MASZYNY_PARAM maszyna_param;
            
            int id_maszyny = db.PROD_MASZYNY_PW.Where(x => x.id == MaszynaMonit.MaszynaMonit.id_prod_maszyny_pw).First().PROD_MASZYNY.id;
            if (MaszynaMonit.MaszynaMonit.id_param > 0)
            {
                maszyna_param = (from p in MaszynaParametry
                                  where p.id == MaszynaMonit.MaszynaMonit.id_param
                                  select p).FirstOrDefault();
                MaszynaMonit.MaszynaMonit.param_nazwa = maszyna_param.nazwa;

                //MessageBox.Show(MaszynaMonit.MaszynaMonit.id_param.ToString());
                // MaszynaMonit.MaszynaMonit.czas = TimeSpan.Parse(MaszynaMonit.MaszynaMonit.czas.ToString());

                // var obj = db.PROD_MASZYNY_MONIT.Where(x => x.id == MaszynaMonit.MaszynaMonit.id).FirstOrDefault();
                maszyny_zl = (from m in db.PROD_MASZYNY_ZL
                                  where m.id_maszyna_gl == id_maszyny
                                  where m.czy_monit_razem == 1
                                  select m.id_maszyna_dl).ToList();
              
                    if (MaszynaMonit.MaszynaMonit.id > 0)
                    {

                    db.SaveChanges();
                   
                    RaisePropertyChanged("MaszynaMonit");
                    MessageBox.Show("Zapisano parametr");

                }
                    else
                    {
                        _maszynamonit.MaszynaMonit.utworzono = DateTime.Now;
                        db.PROD_MASZYNY_MONIT.Add(_maszynamonit.MaszynaMonit);
                         db.SaveChanges();
                    if (maszyny_zl != null && maszyny_zl.Count > 0)
                    {
                        //Jeżeli istnieje maszyna połączona to znajdz ja w przypiętych do linii
                        maszyny_zl_pw = (from mpw in db.PROD_MASZYNY_PW
                                             // where mpw.id_prod_linie == MaszynaMonit.MaszynaMonit.id_prod_maszyny_pw
                                         where maszyny_zl.Contains(mpw.id_maszyny)
                                         where mpw.id_prod_linie == MaszynaMonit.MaszynaMonit.PROD_MASZYNY_PW.id_prod_linie

                                         select mpw).ToList();
                        if ((maszyny_zl_pw != null && maszyny_zl_pw.Count > 0) && (maszyna_param.kod == "czas_pracy") && GetConfirmation("Istnieją maszyny powiązane z tą maszyną. Czy wstawić te same informację do pozostałych maszyn? ", MaszynaMonit.nazwa_maszyny))
                        {
                            foreach(PROD_MASZYNY_PW mpwzl in maszyny_zl_pw)
                            {
                                var maszyna_zl_param = db.PROD_MASZYNY_PARAM.Where(x => x.id_maszyny == mpwzl.id_maszyny && x.kod=="czas_pracy").FirstOrDefault();
                                if(maszyna_zl_param != null && maszyna_zl_param.id > 0)
                                {
                                    monit_zl = new PROD_MASZYNY_MONIT();
                                    monit_zl.utworzono = _maszynamonit.MaszynaMonit.utworzono;
                                    monit_zl.uzytkownik = _maszynamonit.MaszynaMonit.uzytkownik;
                                    monit_zl.data = _maszynamonit.MaszynaMonit.data;
                                    monit_zl.id_param = maszyna_zl_param.id;
                                    monit_zl.id_prod = _maszynamonit.MaszynaMonit.id_prod;
                                    monit_zl.param_nazwa = maszyna_zl_param.nazwa;
                                    monit_zl.id_prod_maszyny_pw = mpwzl.id;
                                    //ilość osób domyślna z maszyny
                                    monit_zl.ilosc_pracownikow = mpwzl.PROD_MASZYNY.ilosc_osob;
                                    monit_zl.rozpoczecie_data = _maszynamonit.MaszynaMonit.rozpoczecie_data;
                                    monit_zl.rozpoczecie_godzina = _maszynamonit.MaszynaMonit.rozpoczecie_godzina;
                                    monit_zl.zakonczenie_data = _maszynamonit.MaszynaMonit.zakonczenie_data;
                                    monit_zl.zakonczenie_godzina = _maszynamonit.MaszynaMonit.zakonczenie_godzina;

                                    db.PROD_MASZYNY_MONIT.Add(monit_zl);
                                    db.SaveChanges();
                                }
                            }
                            
                           

                        }
                    }
                    
                    RaisePropertyChanged("MaszynaMonit");
                    MessageBox.Show("Zapisano parametr");

                }
                    //  MaszynaMonit.Zapisz(db);
                   
                    
                
                
                ProdukcjaMaszynaParamMonitWindow parent = Application.Current.Windows.OfType<ProdukcjaMaszynaParamMonitWindow>().First();
                parent.DialogResult = true;
                parent.Close();
            }
            else
            {
                MessageBox.Show("Proszę wybrać rodzaj parametru!");
            }


        }
        public bool GetConfirmation(string Message, string Caption)
        {
            return MessageBox.Show(Message,
                                   Caption,
                                   MessageBoxButton.OKCancel,
                                   MessageBoxImage.Question,
                                   MessageBoxResult.Cancel) == MessageBoxResult.OK;
        }


        public void Anuluj()
        {
            ProdukcjaMaszynaParamMonitWindow parent = Application.Current.Windows.OfType<ProdukcjaMaszynaParamMonitWindow>().First();
            parent.DialogResult = false;
            parent.Close();
        }

    }
}
