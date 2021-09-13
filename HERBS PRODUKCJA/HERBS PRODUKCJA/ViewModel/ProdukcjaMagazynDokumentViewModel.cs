using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HERBS_PRODUKCJA.Views;
using HERBS_PRODUKCJA.Support;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using System.Collections.ObjectModel;
using HERBS_PRODUKCJA.Helpers;
using System.ComponentModel;
using HERBS_PRODUKCJA.Messages;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class ProdukcjaMagazynDokumentViewModel : CrudVMBase
    {
        public string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        UZYTKOWNICY user = (UZYTKOWNICY)App.Current.Properties["UserLoged"];
        public List<ProdukcjaMGDwVM> dostawy { get; set; }
        public List<PROD_MGPW> MGPWs { get; set; }
        public List<int> PozycjeToDelete { get; set; }

        private List<String> _typyproddp;
        public List<String> TYPYPRODDP
        {
            get { return _typyproddp; }
            set
            {
                _typyproddp = value;
                RaisePropertyChanged("TYPYPRODDP");
            }
        }


        private ProdukcjaMagazynVM _produkcjaMagazyn;
        public ProdukcjaMagazynVM ProdukcjaMagazyn
        {
            get { return _produkcjaMagazyn; }
            set
            {
                _produkcjaMagazyn = value;
                RaisePropertyChanged("ProdukcjaMagazyn");
            }
        }

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

        private ObservableCollection<ProdMGTyp> _prodmgtypy;
        public ObservableCollection<ProdMGTyp> ProdMGTypy
        {
            get { return _prodmgtypy; }
            set
            {
                _prodmgtypy = value;
                RaisePropertyChanged("ProdMGTypy");
            }
        }

        private ProdMGTyp _sprodmgtyp;
        public ProdMGTyp SProdMgTyp
        {
            get { return _sprodmgtyp; }
            set
            {
                _sprodmgtyp = value;
                // Perform any pre-notification process here.
                if(pozycjeMZ != null)
                    PozycjeMZ.Clear();
                if(ProdukcjaMagazyn != null)
                    ProdukcjaMagazyn.ProdukcjaMG.typ_dk = _sprodmgtyp.Kod;
                if (_sprodmgtyp.Kod == "PZS" || _sprodmgtyp.Kod == "PZO" || _sprodmgtyp.Kod == "PWP" || _sprodmgtyp.Kod == "PWWG")
                {
                    _buttCancel = false;
                    if(_sprodmgtyp.Kod == "PZS" || _sprodmgtyp.Kod == "PZO")
                    {
                        _showKH = true;
                        RaisePropertyChanged("ShowKH");
                        if(_sprodmgtyp.Kod == "PZO")
                        {
                            _showPrice = true;
                            RaisePropertyChanged("ShowPrice");
                        }
                    }
                    if (_sprodmgtyp.Kod == "PWP" || _sprodmgtyp.Kod == "PWWG")
                    {
                        WstawKH(new ProdukcjaKhVM { ProdukcjaKh = db.KH.Where(x => x.kod_firmy == kod_firmy && x.kod == kod_firmy).FirstOrDefault() });
                        
                    }
                    RaisePropertyChanged("ButtCancel");
                    //System.Windows.Forms.MessageBox.Show(_buttCancel.ToString());
                }
                else if (_sprodmgtyp.Kod == "WZWG" || _sprodmgtyp.Kod == "RWS" || _sprodmgtyp.Kod == "RWO")
                {
                    _buttCancel = true;
                    _showKH = true;
                    _showPrice = false;
                    RaisePropertyChanged("ShowPrice");
                    RaisePropertyChanged("ShowKH");
                    RaisePropertyChanged("ButtCancel");
                }
                else
                {
                    _showKH = false;
                    _showPrice = false;
                    RaisePropertyChanged("ShowPrice");
                    RaisePropertyChanged("ShowKH");
                }

                RaisePropertyChanged("SProdMgTyp");
                RaisePropertyChanged("ProdukcjaMagazyn");
            }
        }

        private bool _buttCancel;
        public bool ButtCancel
        {
            get { return _buttCancel; }
            set
            {
                _buttCancel = value;
                RaisePropertyChanged("ButtCancel");
            }
        }

        private bool _showKH;
        public bool ShowKH
        {
            get { return _showKH; }
            set
            {
                _showKH = value;
                RaisePropertyChanged("ShowKH");
            }
        }

        private bool _showPrice;
        public bool ShowPrice
        {
            get { return _showPrice; }
            set
            {
                _showPrice = value;
                RaisePropertyChanged("ShowPrice");
            }
        }






        public RelayCommand WstawTWCommand { get; set; }
        public RelayCommand UsunTWCommand { get; set; }
        public RelayCommand WstawKHCommand { get; set; }
        public RelayCommand WstawDWCommand { get; set; }
        public RelayCommand ZapiszCommand { get; set; }
        public RelayCommand UsunPozycjeCommand { get; set; }
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand { get; set; }
        public RelayCommand<DataGridCellEditEndingEventArgs> MonitCellEditEndingCommand { get; set; }

        private RelayCommand<string> _drukujCommand;
        public RelayCommand<string> DrukujCommand
        {
            get
            {
                return _drukujCommand ?? (_drukujCommand = new RelayCommand<string>(
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
        private RelayCommand<object> _specyfikacjaCommand;
        public RelayCommand<object> SpecyfikacjaCommand
        {
            get
            {
                return _specyfikacjaCommand ?? (_specyfikacjaCommand = new RelayCommand<object>(
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



        public ProdukcjaMagazynDokumentViewModel() 
            :base()
        {

            TYPYPRODDP = Functions.GetTypyPozycjiWG();
            PobierzProdMGTypy();
            RaisePropertyChanged("SProdMgTyp");

            //Messenger.Default.Register<ProdukcjaMagazynVM>(this, prodmgvm => getProdukcjaMGvm(prodmgvm));

          
            WstawTWCommand      = new RelayCommand(PokazTW);
            WstawKHCommand      = new RelayCommand(PokazKH);
            WstawDWCommand      = new RelayCommand(PokazDW);
            ZapiszCommand       = new RelayCommand(Zapisz);
            UsunPozycjeCommand  = new RelayCommand(UsunPozycje);

            _specyfikacjaCommand = new RelayCommand<object>(i => Specyfikacja(i));

            _drukujCommand = new RelayCommand<string>(i => Drukuj(i));
            CellEditEndingCommand = new RelayCommand<DataGridCellEditEndingEventArgs>(args => SprawdzStany());
            // _buttCancel = true;

        }

        private void Specyfikacja(object i)
        {
            try
            {
                ProdukcjaMagazynPozycjaVM poz = (ProdukcjaMagazynPozycjaVM)i; 
                MessageBox.Show(poz.ProdukcjaMZ.kod);
                ProdukcjaSpecyfikacjaPakowaniaWindow widnow = new ProdukcjaSpecyfikacjaPakowaniaWindow(poz);
                widnow.ShowDialog();
               
            }
            catch
            {
                throw new NotImplementedException();
            }
           
        }

        private void Drukuj(object param)
        {
            Raport(param);
        }

        //Funkcja oblicza wagi konkretnych pozycji towarowych
        private void PrzeliczWagi()
        {

        }

        public void getProdukcjaMGvm(ProdukcjaMagazynVM prodmgvm)
        {
            
            MGPWs = new List<PROD_MGPW>();
            PozycjeToDelete = new List<int>();
            if (prodmgvm.ProdukcjaMG.id > 0)
            {
                prodmgvm.ProdukcjaMG = db.PROD_MG.Where(i => i.id == prodmgvm.ProdukcjaMG.id).FirstOrDefault();
            }
           // if (prodmgvm.ProdukcjaMG != null)
            
              //  prodmgvm.IsNew = false;

            if (!prodmgvm.IsNew)
            {
                //db = new FZLEntities1();

                
                ProdukcjaMagazyn = prodmgvm;
                SProdMgTyp = ProdMGTypy.FirstOrDefault(x => x.Kod == ProdukcjaMagazyn.ProdukcjaMG.typ_dk);
                GetPozycjeMZ(prodmgvm.ProdukcjaMG.id);
                RaisePropertyChanged("ProdukcjaMagazyn");
            }
            else
            {
                ProdukcjaMagazyn = prodmgvm;
                SProdMgTyp = ProdMGTypy.FirstOrDefault(x => x.Kod == ProdukcjaMagazyn.ProdukcjaMG.typ_dk);
                RaisePropertyChanged("ProdukcjaMagazyn");
                //SProdMgTyp = ProdMGTypy.FirstOrDefault(x => x.Kod == "PZS");
                //prodmgvm.ProdukcjaMG.typ_dk = SProdMgTyp.Kod;




            }
           // db = new FZLEntities1();
            //ProdukcjaMagazyn = prodmgvm;
            RaisePropertyChanged("ProdukcjaMagazyn");


            RaisePropertyChanged("SProdMgTyp");

            //System.Windows.Forms.MessageBox.Show(prodmgvm.ProdukcjaMG.data.ToString());
        }

        private void GetPozycjeMZ(object MzId)
        {
            //db = new FZLEntities1();

            int mzID = int.Parse(MzId.ToString());
            var query = (from pozycja in db.PROD_MZ
                         where pozycja.id_prodmg == mzID
                         select pozycja).ToList();

            

            pozycjeMZ = new ObservableCollection<ProdukcjaMagazynPozycjaVM>();
           

            foreach (PROD_MZ pozycja in query)
            {
                
                pozycjeMZ.Add( new ProdukcjaMagazynPozycjaVM { ProdukcjaMZ=pozycja, IsNew=false });
                if(SProdMgTyp.Kod == "RWWG" || SProdMgTyp.Kod == "WZWG" || SProdMgTyp.Kod == "RWS" || SProdMgTyp.Kod == "RWO")
                {
                   
                }

               
                var item = db.PROD_MGPW.Where(x => x.id_prodmz == pozycja.id).FirstOrDefault();
                if (item != null)
                {
                    MGPWs.Add(item);
                }
            }
            RaisePropertyChanged("PozycjeMZ");

            //RaisePropertyChanged("ProdukcjaMagazyn");
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
        
        //Pokazuje dostawy 
        public void PokazDW()
        {
           // PopupWindow 
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborProdMGDwWindow dialog = new WyborProdMGDwWindow();
           // if (this.dostawy != null)
               // dialog.selDW = this.dostawy;
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {

                wstawPozycjeDW(dialog.selDW, 1);
            }
        }

        private void wstawPozycjeTW(List<ProdukcjaTwVM> selTW)
        {
            PROD_MZ prod_mz = new PROD_MZ();
            if(PozycjeMZ == null)
            {
                PozycjeMZ = new ObservableCollection<ProdukcjaMagazynPozycjaVM>();
            }
            foreach(ProdukcjaTwVM tw in selTW)
            {
                prod_mz = new PROD_MZ();
                if (ProdukcjaMagazyn.ProdukcjaMG.typ_dk == "PZS")
                    prod_mz.typ_produktu = "SR";
                if (ProdukcjaMagazyn.ProdukcjaMG.typ_dk == "PZO")
                    prod_mz.typ_produktu = "OP";
                prod_mz.kod = tw.ProdukcjaTW.kod;
                prod_mz.opis = tw.ProdukcjaTW.nazwa;
                prod_mz.idtw = tw.ProdukcjaTW.id;
                prod_mz.jm = tw.ProdukcjaTW.jm;
                prod_mz.data = ProdukcjaMagazyn.ProdukcjaMG.data;

                PozycjeMZ.Add(new ProdukcjaMagazynPozycjaVM { ProdukcjaMZ = prod_mz, IsNew=true });
                UstalLp();
            }
            
            RaisePropertyChanged("ProdukcjaMagazyn");
            // throw new NotImplementedException();
        }

        public void PokazKH()
        {
            WyborKhWindow dialog = new WyborKhWindow();
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                WstawKH(dialog.selKH);
            }
        }

        private void WstawKH(ProdukcjaKhVM selKH)
        {
            if (selKH.ProdukcjaKh != null)
            {
                _produkcjaMagazyn.ProdKH = selKH;
                _produkcjaMagazyn.ProdKH.Adres = selKH.getAdres();
                _produkcjaMagazyn.ProdukcjaMG.khid = (int)selKH.ProdukcjaKh.id;
                _produkcjaMagazyn.ProdukcjaMG.khkod = selKH.ProdukcjaKh.kod;
                _produkcjaMagazyn.ProdukcjaMG.khnazwa = selKH.ProdukcjaKh.nazwa;
                _produkcjaMagazyn.ProdukcjaMG.khnip = selKH.ProdukcjaKh.nip;
                _produkcjaMagazyn.ProdukcjaMG.khdom = selKH.ProdukcjaKh.dom;
                _produkcjaMagazyn.ProdukcjaMG.khlokal = selKH.ProdukcjaKh.lokal;
                _produkcjaMagazyn.ProdukcjaMG.khmiasto = selKH.ProdukcjaKh.miejscowosc;
                _produkcjaMagazyn.ProdukcjaMG.khkodpocz = selKH.ProdukcjaKh.kodpocz;
                _produkcjaMagazyn.ProdukcjaMG.khadres = selKH.ProdukcjaKh.ulica;

                RaisePropertyChanged("ProdukcjaMagazyn");
            }
        }

        public void wstawPozycjeDW(List<ProdukcjaMGDwVM> dostawy, byte mode=1)
        {
            
            this.dostawy = dostawy;
            ProdukcjaPozycjeVM poz;
            bool jest = false;
            PROD_MZ prod_mz = new PROD_MZ();
            PROD_MGPW mgpw = new PROD_MGPW();

            if (PozycjeMZ == null)
            {
                PozycjeMZ = new ObservableCollection<ProdukcjaMagazynPozycjaVM>();
            }
            

            foreach (ProdukcjaMGDwVM dostawa in dostawy)
            {
                jest = false;
                prod_mz = new PROD_MZ();
                prod_mz.kod = dostawa.ProdukcjaDW.kodtw;
           
                prod_mz.opis = dostawa.ProdukcjaDW.nazwatw;
                prod_mz.idtw = dostawa.ProdukcjaDW.idtw;
                var tw = db.PROD_HMTW.Where(x =>(x.id_fk == dostawa.ProdukcjaDW.idtw || x.id ==dostawa.ProdukcjaDW.idtw) && x.kod_firmy==kod_firmy).FirstOrDefault();
                prod_mz.jm = tw.jm;
                prod_mz.nr_partii = dostawa.ProdukcjaDW.nr_partii;
                prod_mz.typ_produktu = dostawa.ProdukcjaDW.typ_produktu;
                if (mode == 2)
                {
                    prod_mz.ilosc = dostawa.ProdukcjaDW.iloscdost;
                }
                PozycjeMZ.Add(new ProdukcjaMagazynPozycjaVM { ProdukcjaMZ = prod_mz, IsNew = true });
                UstalLp();
                RezerwujDostawe(dostawa.ProdukcjaDW, prod_mz);
            }            
        }

        public void PobierzProdMGTypy()
        {
            ProdMGTypy = new ObservableCollection<ProdMGTyp>()
            {
                 new ProdMGTyp(){Kod="PZS",  Nazwa="Przyjęcie surowca"}
                ,new ProdMGTyp(){Kod="PZO",  Nazwa="Przyjęcie opakowania"}
                ,new ProdMGTyp(){Kod="PWP",  Nazwa="Przyjęcie po produkcji"}
                ,new ProdMGTyp(){Kod="PWWG",  Nazwa="Przyjęcie wyrobu gotowego"}
                ,new ProdMGTyp(){Kod="RWS",  Nazwa="Rozchód surowca"}
                ,new ProdMGTyp(){Kod="RWO",  Nazwa="Rozchód opakowania"}
                ,new ProdMGTyp(){Kod="WZWG", Nazwa="Wydanie wyrobu gotowego"}
            };
            
            RaisePropertyChanged("SProdMgTyp");
        }

        

        public void RezerwujDostawy()
        {
            
            
        }

        public void RezerwujDostawe(PROD_MGDW dw, PROD_MZ mz)
        {
            PROD_MGPW mgpw;
            if(MGPWs == null)
            {
                MGPWs = new List<PROD_MGPW>();
            }
            //db = new FZLEntities1();
            if (mz.id > 0)
            {
                mgpw = MGPWs.Where(x => x.id_prodmgdw == dw.id && x.id_prodmz == mz.id).FirstOrDefault();
            }
            //w przypadku braku zapisanego powiazania pobeierz na podstawie lp
            else
            {
                mgpw = MGPWs.Where(x => x.id_prodmgdw == dw.id && x.lp == mz.lp).FirstOrDefault();
            }
            //var item = db.PROD_MGPW.FirstOrDefault(i => i.id_prodmz == prodmz.id);
            // var item2 = db.PROD_HMDW.FirstOrDefault((System.Linq.Expressions.Expression<Func<PROD_HMDW, bool>>)(i => i.id == proddp.idproddw));
            var item3 = mz;
            if (mgpw != null)
            {
                mgpw.ilosc = mz.ilosc;
                mgpw.id_prodmz = mz.id;
                mgpw.id_prodmgdw = dw.id;
                mgpw.lp = mz.lp;   
               // item.id_dwfk = proddp.iddw;
            }
            else
            {
                mgpw = new PROD_MGPW();
                mgpw.ilosc = mz.ilosc;
                mgpw.lp = mz.lp;
                mgpw.PROD_MZ = mz;
                mgpw.id_prodmgdw = dw.id;
                MGPWs.Add(mgpw);  
            }

        }

        public void ZapiszPozycje()
        {
            PROD_MGPW pw;
            PROD_MGDW dw;
            if (PozycjeToDelete != null && PozycjeToDelete.Count() > 0)
            {
                //db = new FZLEntities1();
                db.PROD_MZ.RemoveRange(db.PROD_MZ.Where(x => PozycjeToDelete.Contains(x.id)));
                db.SaveChanges();
            }
            //db = new FZLEntities1();
           
                foreach (ProdukcjaMagazynPozycjaVM poz in PozycjeMZ)
                {
                    poz.ProdukcjaMZ.id_prodmg = ProdukcjaMagazyn.ProdukcjaMG.id;
                    poz.ProdukcjaMZ.idkh = this.ProdukcjaMagazyn.ProdukcjaMG.khid;
                    if (!(poz.ProdukcjaMZ.id > 0))
                    {
                        db.PROD_MZ.Add(poz.ProdukcjaMZ);
                    }
                    else
                    {
                        // db = new FZLEntities1();
                        //     var poz_db = db.PROD_MZ.Where(x => x.id == poz.ProdukcjaMZ.id).First();
                        //     poz_db = poz.ProdukcjaMZ;

                    }


                    db.SaveChanges();
                    if (MGPWs != null && MGPWs.Count() > 0)
                    {
                        pw = MGPWs.Where(x => x.id_prodmz == poz.ProdukcjaMZ.id || x.lp == poz.ProdukcjaMZ.lp).First();
                        if (pw != null)
                        {
                            pw.ilosc = poz.ProdukcjaMZ.ilosc;
                            pw.id_prodmz = poz.ProdukcjaMZ.id;
                            pw.lp = poz.ProdukcjaMZ.lp;
                            if (!(pw.id > 0))
                            {
                                db.PROD_MGPW.Add(pw);

                            }
                            db.SaveChanges();

                            dw = db.PROD_MGDW.Where(x => x.id == pw.id_prodmgdw).First();
                            dw.iloscdost = dw.iloscpz - SumujRezerwacjeDostawy(dw.id);
                            dw.stan = dw.iloscpz - SumujRezerwacjeDostawy(dw.id);
                            db.SaveChanges();
                        }

                    }

                }

           
            if (ProdukcjaMagazyn.ProdukcjaMG.typ_dk == "PZS" || ProdukcjaMagazyn.ProdukcjaMG.typ_dk == "PZO" || ProdukcjaMagazyn.ProdukcjaMG.typ_dk == "PWP" || ProdukcjaMagazyn.ProdukcjaMG.typ_dk == "PWWG")
            {
                foreach (ProdukcjaMagazynPozycjaVM poz in PozycjeMZ)
                {
                    ZapiszDostawe(poz);
                }
            }
            //getProdukcjaMGvm(ProdukcjaMagazyn);
            //GetPozycjeMZ(this.ProdukcjaMagazyn.ProdukcjaMG.id);

        }

        public void UsunPozycje()
        {
            if (SelectedProdMZ != null && GetConfirmation("Czy napewno usunąć: " + SelectedProdMZ.ProdukcjaMZ.opis + " w ilości: " + SelectedProdMZ.ProdukcjaMZ.ilosc + SelectedProdMZ.ProdukcjaMZ.jm, SelectedProdMZ.ProdukcjaMZ.opis))
            {
                if (PozycjeToDelete == null)
                    PozycjeToDelete = new List<int>();

                PozycjeToDelete.Add(PozycjeMZ[PozycjeMZ.IndexOf(SelectedProdMZ)].ProdukcjaMZ.id);
                if (SelectedProdMZ.ProdukcjaMZ.PROD_MG.typ_dk == "PZS" || SelectedProdMZ.ProdukcjaMZ.PROD_MG.typ_dk == "PZO" || SelectedProdMZ.ProdukcjaMZ.PROD_MG.typ_dk == "PWWG" || SelectedProdMZ.ProdukcjaMZ.PROD_MG.typ_dk == "PWP" )
                {
                    UsunDostawe(PozycjeMZ[PozycjeMZ.IndexOf(SelectedProdMZ)]);
                }
                if(SelectedProdMZ.ProdukcjaMZ.PROD_MG.typ_dk == "RWS" || SelectedProdMZ.ProdukcjaMZ.PROD_MG.typ_dk == "WZWG" || SelectedProdMZ.ProdukcjaMZ.PROD_MG.typ_dk == "RWO" )
                {
                    UsunRezerwacje(PozycjeMZ[PozycjeMZ.IndexOf(SelectedProdMZ)]);
                }
                PozycjeMZ.RemoveAt(PozycjeMZ.IndexOf(SelectedProdMZ));
                UstalLp();

            }
        }

        private void UsunRezerwacje(ProdukcjaMagazynPozycjaVM produkcjaMagazynPozycjaVM)
        {
            var query = (from r in db.PROD_MGPW
                         where r.id_prodmz == produkcjaMagazynPozycjaVM.ProdukcjaMZ.id
                         select r).ToList();
            PROD_MGDW dw;
            double ilosc_tmp;
            foreach (PROD_MGPW pw in query)
            {
                dw = pw.PROD_MGDW;
                ilosc_tmp = (double)pw.ilosc;
                 

                db.PROD_MGPW.Remove(pw);
                dw.iloscdost = dw.iloscdost + ilosc_tmp;
                dw.stan = dw.iloscdost;

            }
        }

        public void UstalLp()
        {

            foreach (ProdukcjaMagazynPozycjaVM poz in PozycjeMZ)
            {
                poz.ProdukcjaMZ.lp = (short?) (pozycjeMZ.IndexOf(poz) + 1); 

            }
            RaisePropertyChanged("PozycjeMZ");
        }

        public void UsunDostawe(ProdukcjaMagazynPozycjaVM poz)
        {
           // using(db = new FZLEntities1();
            PROD_MGDW dw = (from d in db.PROD_MGDW
                            where d.idpozpz == poz.ProdukcjaMZ.id
                            select d).FirstOrDefault();
            if (dw != null)
            {
                db.PROD_MGDW.Remove(dw);
              //  db.SaveChanges();

            }


        }

        public void ZapiszDostawe(ProdukcjaMagazynPozycjaVM poz)
        {
            db = new FZLEntities1();
            //Znajdz innformacje o odstawie dla danej pozycji
            PROD_MGDW dw = (from d in db.PROD_MGDW
                         where d.idpozpz == poz.ProdukcjaMZ.id
                         select d).FirstOrDefault();
            //dostawa istnieje uaktualnij ją
            if (dw == null || dw.id == null)
            {
                dw = new PROD_MGDW();
            }
                dw.kod_firmy = kod_firmy;
                dw.idpozpz = poz.ProdukcjaMZ.id;
                dw.idkh = poz.ProdukcjaMZ.PROD_MG.khid;
                dw.khkod = poz.ProdukcjaMZ.PROD_MG.khkod;
                dw.khnazwa = poz.ProdukcjaMZ.PROD_MG.khnazwa;
                dw.kodtw = poz.ProdukcjaMZ.kod;
                dw.data = poz.ProdukcjaMZ.PROD_MG.data;
                dw.nazwatw = poz.ProdukcjaMZ.opis;
                dw.idtw = poz.ProdukcjaMZ.idtw;
                dw.ilosc = poz.ProdukcjaMZ.ilosc;
                dw.iloscdost = poz.ProdukcjaMZ.ilosc;
                dw.iloscpz = poz.ProdukcjaMZ.ilosc;
                dw.stan = poz.ProdukcjaMZ.ilosc;
                dw.nr_partii = poz.ProdukcjaMZ.nr_partii;
                dw.kod = poz.ProdukcjaMZ.PROD_MG.kod;
                dw.typdw = poz.ProdukcjaMZ.PROD_MG.typ_dk;
                dw.typ_produktu = poz.ProdukcjaMZ.typ_produktu;

            if (!(dw.id > 0))
            {
                db.PROD_MGDW.Add(dw);
            }
            db.SaveChanges();
                
            
        }

        public double SumujRezerwacjeDostawy(int iddw)
        {
           
            var suma = (from d in db.PROD_MGPW
                        
                        where d.id_prodmgdw == iddw



                        select d.ilosc).Sum();
            if (suma == null)
                return 0;
            else
                return (double)suma;

        }

        private void SprawdzStany()
        {

            if (SelectedProdMZ != null)
            {
                double? ilosc, iloscprod, iloscdost, sum_iloscpws;
                iloscdost = 0;
                ilosc = SelectedProdMZ.ProdukcjaMZ.ilosc;
                PROD_MGPW pw;
                if (MGPWs != null)
                {
                    pw = MGPWs.Where(x => x.lp == SelectedProdMZ.ProdukcjaMZ.lp).FirstOrDefault();
                    if (pw != null && pw.lp > 0)
                    {

                        var lbs = (from g in db.PROD_MGDW where g.id == pw.id_prodmgdw select g).FirstOrDefault();
                        iloscprod = SumujRezerwacjeDostawy(lbs.id);
                        if (SelectedProdMZ.ProdukcjaMZ.id > 0)
                        {
                            using (FZLEntities1 db2 = new FZLEntities1())
                            {
                                var lbs2 = (from p in db2.PROD_MZ where p.id == SelectedProdMZ.ProdukcjaMZ.id select p).FirstOrDefault();
                                if (lbs2.id > 0)
                                {
                                    iloscdost = lbs.iloscpz - (iloscprod - pw.ilosc);
                                    //MessageBox.Show(lbs2.ilosc.ToString());
                                    // iloscprod = iloscprod - lbs2.ilosc;
                                }
                            }
                        }
                        else
                        {
                            iloscdost = lbs.iloscdost;
                        }


                        if (ilosc > iloscdost)
                        {
                            MessageBox.Show("Mksymanlna dostępna ilość dla danej dostawy to: " + iloscdost.ToString());
                            SelectedProdMZ.ProdukcjaMZ.ilosc = iloscdost;
                        }
                    }

                }
            }
            RaisePropertyChanged("PozycjeMZ");
        }

        public bool GetConfirmation(string Message, string Caption)
        {
            return MessageBox.Show(Message,
                                   Caption,
                                   MessageBoxButton.OKCancel,
                                   MessageBoxImage.Question,
                                   MessageBoxResult.Cancel) == MessageBoxResult.OK;
        }

        public int SprawdzPozycje()
        {
            
            if (PozycjeMZ != null && PozycjeMZ.Count > 0)
            {
                foreach (ProdukcjaMagazynPozycjaVM poz in PozycjeMZ)
                {
                    if (poz.ProdukcjaMZ.nr_partii == null || poz.ProdukcjaMZ.nr_partii.Length < 3)
                    {
                        MessageBox.Show("Prodzę podaj poprawny numer partii dla pozycji: " + poz.ProdukcjaMZ.kod);
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }

                return 1;
        }

        public void Zapisz()
        {
            SprawdzStany();
           
            if (SprawdzPozycje() != 0)
            {
                

                if (ProdukcjaMagazyn.IsNew)
                {

                    ProdukcjaMagazyn.ProdukcjaMG.okres = int.Parse(ProdukcjaMagazyn.ProdukcjaMG.data.Value.Year.ToString());
                    ProdukcjaMagazyn.ProdukcjaMG.serial = (int)ProdukcjaMagazyn.pobierzKolejnyNumerSerii(ProdukcjaMagazyn.ProdukcjaMG.okres, ProdukcjaMagazyn.ProdukcjaMG.typ_dk);
                    ProdukcjaMagazyn.ProdukcjaMG.kod = ProdukcjaMagazyn.ProdukcjaMG.typ_dk + "/" + ProdukcjaMagazyn.ProdukcjaMG.serial.ToString() + "/" + ProdukcjaMagazyn.ProdukcjaMG.okres.ToString();

                    ProdukcjaMagazyn.ProdukcjaMG.osoba_mod = user.nazwa;
                    ProdukcjaMagazyn.ProdukcjaMG.data_mod = DateTime.Now;
                    ProdukcjaMagazyn.ProdukcjaMG.osoba = user.nazwa;
                    db.PROD_MG.Add(ProdukcjaMagazyn.ProdukcjaMG);
                }
                else
                {

                    ProdukcjaMagazyn.ProdukcjaMG.osoba_mod = user.nazwa;
                    ProdukcjaMagazyn.ProdukcjaMG.data_mod = DateTime.Now;
                }



                try
                {

                    db.SaveChanges();

                    ZapiszPozycje();


                    System.Windows.Forms.MessageBox.Show("Zapisano dokument");
                    if(ProdukcjaMagazyn.ProdukcjaMG.id > 0)
                    {
                        ProdukcjaMagazyn.IsNew = false;
                    }
                        
                    RaisePropertyChanged("ProdukcjaMagazyn");
                    db = new FZLEntities1();
                    getProdukcjaMGvm(ProdukcjaMagazyn);
                    Messenger.Default.Send<ProdukcjaMagazynVM>(ProdukcjaMagazyn);
                    //ProdukcjaInfo.IsNew = false;

                    // msg.Message = "Database Updated";

                }
                catch (Exception e)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        ErrorMessage = e.Message.ToString();
                    }
                    System.Windows.Forms.MessageBox.Show("Wystąpił błąd: " + e.InnerException.ToString());
                }
            }
            
        }

        public DataSet PrzygotujDaneDoRaportu()
        {
            //FZLDataSetTableAdapters.PRODTableAdapter prodta = new FZLDataSetTableAdapters.PRODTableAdapter();

            var prodmg = db.PROD_MG.Where(i => i.id == this.ProdukcjaMagazyn.ProdukcjaMG.id).ToDataTable();
            var prodmz = db.PROD_MZ.Where(i => i.id_prodmg == this.ProdukcjaMagazyn.ProdukcjaMG.id).ToDataTable();
            var prodmz_spec = db.PROD_MZ_SPEC.Where(i => i.PROD_MZ.id_prodmg == this.ProdukcjaMagazyn.ProdukcjaMG.id).ToDataTable();
            var opakowania_rodzaje = db.OPAKOWANIA_RODZAJE.ToDataTable();
            DataSet ds = new DataSet();

            prodmg.TableName = "PROD_MG";
            ds.Tables.Add(prodmg);

            prodmz.TableName = "PROZ_MZ";
            ds.Tables.Add(prodmz);

            prodmz_spec.TableName = "PROZ_MZ_SPEC";
            ds.Tables.Add(prodmz_spec);

            opakowania_rodzaje.TableName = "OPAKOWANIA_RODZAJE";
            ds.Tables.Add(opakowania_rodzaje); 


            return ds;
        }

        public void Raport(object param)
        {
            string url1 ="";
            if ((string)param == "DK")
            {
                url1 = @"Raporty\MGPZ.frx";
            }
            else if((string)param == "ET")
            {
                url1 = @"Raporty\MGPZ_etykieta.frx";
            }
            else if ((string)param == "DKWZ")
            {
                url1 = @"Raporty\MGWZ.frx";
            }
            else if ((string)param == "SPEC")
            {
                url1 = @"Raporty\MG_SPECYFIKACJA.frx";
            }
            if (url1 != "")
            {
                ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
                dialog.ShowDialog();
            }
           
        }


    }
}
