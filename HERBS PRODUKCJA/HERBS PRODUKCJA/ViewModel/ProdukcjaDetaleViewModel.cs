using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HERBS_PRODUKCJA.Model;
using HERBS_PRODUKCJA.Support;
using HERBS_PRODUKCJA.Views.RowVM;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using GalaSoft.MvvmLight.Messaging;
using HERBS_PRODUKCJA.Views;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Messages;
using System.Windows;
using HERBS_PRODUKCJA.ViewModel;
//using HERBS_PRODUKCJA.InteliboxDataProviders;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Views;
using HERBS_PRODUKCJA.Helpers;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using FastReport;
using System.IO;
using System.Data;
using System.Windows.Data;
using FastReport.Export.Pdf;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class ProdukcjaDetaleViewModel : CrudVMBase
    {
        UZYTKOWNICY user = (UZYTKOWNICY)App.Current.Properties["UserLoged"];

        public WALUTY SelectedWALUTA { get; set; }
        List<ProdukcjaDwVM>     dostawy;        //dostawy magazyn symfonia
        List<ProdukcjaMGDwVM> dostawyProdMG;        //dostawy magazyn produkcyjny
        List<PROD_PRODMGPW> OpakowaniaPowiazania;
        List<PROD_PRODMGPW> DostawyProdMGPowiazania; //informacje o powiazaniach pozycjiRW z dostawiami magazynu produkcyjnego
        // Example Dictionary again.
        //Dictionary<string, int> d = new Dictionary<string, int>()
        Dictionary<int,ProdukcjaMGDwVM>     dostawyMG;              //słownik kojarzący pozycjeRW z dostawą magazyn produkcyjny
        Dictionary<int,PROD_PRODMGPW>       dostawyMGopakowania;    //słownik kojarzący MOnitPW z dostawa opakowania z magazynu produkcyjnego

        List<ProdukcjaTwVM> towary;
        ProdukcjaMagazynVM ProdukcjaMagazyn = new ProdukcjaMagazynVM();


        private List<String> _waluty;
        public List<String> Waluty
        {
            get { return _waluty; }
            set
            {
                _waluty = value;
                RaisePropertyChanged("Waluty");
            }
        }

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

        private List<ProdukcjaMaszynaVM> _maszyny;
        public List<ProdukcjaMaszynaVM> Maszyny
        {
            get { return _maszyny; }
            set
            {
                _maszyny = value;
                RaisePropertyChanged("Maszyny");
            }
        }
        private ProdukcjaMaszynaVM _maszynasel;
        public ProdukcjaMaszynaVM MaszynaSel
        {
            get { return _maszynasel; }
            set
            {
                _maszynasel = value;
                RaisePropertyChanged("MaszynaSel");
            }
        }
        private List<PROD_LINIE> _linie;
        public List<PROD_LINIE> Linie
        {
            get { return _linie; }
            set
            {
                _linie = value;
                RaisePropertyChanged("Linie");
            }
        }
        private List<PROD_MASZYNY_MONIT> _maszynymonit;
        public List<PROD_MASZYNY_MONIT> MaszynyMonit
        {
            get { return _maszynymonit; }
            set
            {
                _maszynymonit = value;
                RaisePropertyChanged("MaszynyMonit");
            }
        }

        private ProdukcjaPozycjeMonitVM _selectedMONIT;
        public ProdukcjaPozycjeMonitVM SelectedMONIT
        {
            get { return _selectedMONIT; }
            set
            {
                _selectedMONIT = value;
                RaisePropertyChanged("SelectedMONIT");
            }
        }


        private bool isChecked;
        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        private bool _isPlanPrice;
        public bool IsPlanPrice
        {
            get { return _isPlanPrice; }
            set
            {
                _isPlanPrice = value;
                RaisePropertyChanged("IsPlanPrice");
            }
        }
        private Visibility _isShowPlanPrice;
        public Visibility IsShowPlanPrice
        {
            get { return _isShowPlanPrice; }
            set
            {
                _isShowPlanPrice = value;
                RaisePropertyChanged("IsShowPlanPrice");
            }
        }
        private bool isFinish;
        public bool IsFinish
        {
            get { return isFinish; }
            set
            {
                isFinish = value;
                RaisePropertyChanged("IsFinish");
            }
        }
        private bool isPartial;
        public bool IsPartial
        {
            get { return isPartial; }
            set
            {
                isPartial = value;
                RaisePropertyChanged("IsPartial");
            }
        }

        private bool isZero;
        public bool IsZero
        {
            get { return isZero; }
            set
            {
                isZero = value;
                RaisePropertyChanged("IsZero");
            }
        }
        private bool _wgWycenaTryb;
        public bool WgWycenaTryb
        {
            get { return _wgWycenaTryb; }
            set
            {
                _wgWycenaTryb = value;
                RaisePropertyChanged("WgWycenaTryb");
            }
        }

        private float wartoscWG;
        public float WartoscWG
        {
            get { return wartoscWG; }
            set
            {
                wartoscWG = value;
                RaisePropertyChanged("WartoscWG");
            }
        }

        private bool isCanSave;
        public bool IsCanSave
        {
            get { return isCanSave; }
            set
            {
                isCanSave = value;
                RaisePropertyChanged("IsCanSave");
            }
        }

        private bool isCanPrice;
        public bool IsCanPrice
        {
            get { return isCanPrice; }
            set
            {
                isCanPrice = value;
                RaisePropertyChanged("IsCanPrice");
            }
        }

        private bool isCanRW;
        public bool IsCanRW
        {
            get { return isCanRW; }
            set
            {
                isCanRW = value;
                RaisePropertyChanged("IsCanRW");
            }
        }

        private bool _isFinishDate;
        public bool IsFinishDate
        {
            get { return _isFinishDate; }

            set
            {
                if (_isFinishDate == value)
                {
                    return;
                }

                _isFinishDate = value;
                RaisePropertyChanged("IsFinishDate");
            }
        }


        public List<PROD_PRODMGPW> prodmgPW;

        private ProdukcjaRozliczenieFKView _rozliczenieView;
        public ProdukcjaRozliczenieFKView RozliczenieView
        {
            get { return _rozliczenieView; }
            set
            {
                _rozliczenieView = value;
                RaisePropertyChanged("RozliczenieView");
            }
        }


        private ProdukcjaSymulacjaView _symulacjaView;
        public ProdukcjaSymulacjaView SymulacjaView
        {
            get { return _symulacjaView; }
            set
            {
                _symulacjaView = value;
                RaisePropertyChanged("SymulacjaView");
            }
        }


        public void Checkprocess()
        {
            bool canRW;
            canRW = SprawdzPozycjePW();
            if (canRW && IsChecked)
            {

                var result = MessageBox.Show(
                                 "Akceptacja generuje dokumenty magazynowe.\n Jakakolwiek edycja danych nie będzie możliwa.\n Czy jesteś pewien, że chcesz akceptować?",
                                 "Akceptacja", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    ProdukcjaInfo.Produkcja.akceptowano = 1;

                else if (result == MessageBoxResult.Cancel)
                    return; // <- Don't close window
                else if (result == MessageBoxResult.No)
                {
                    ProdukcjaInfo.Produkcja.akceptowano = 0;
                    IsChecked = false;
                }
                //MessageBox.Show(ProdukcjaInfo.Produkcja.akceptowano.ToString());
            }
            else
            {
                ProdukcjaInfo.Produkcja.akceptowano = 0;
                if (!canRW)
                {
                    MessageBox.Show("Nie można akceptować zlecenia ponieważ występuja błędy w cenach!");
                    IsChecked = false;
                }
            }

        }

        public bool SprawdzPozycjePW()
        {
            if (ProdukcjaPozycjePW_FK.Count() == 0)
                return false;
            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjePW_FK)
            {
                if ((poz.PozycjaFK.cena == null || poz.PozycjaFK.cena <= 0))
                {
                    //MessageBox.Show(poz.Pozycja.cena.ToString());
                    return false;

                }
            }
            return true;
        }

        public bool SprawdzPozycjeMG_PW()
        {
            double? ilosc_RW_MG = 0, ilosc_PW_MG = 0;
            

            string Message="";
            string Message2 = "";
            if (ProdukcjaPozycjePW.Count() == 0)
                return false;
            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjePW)
            {
                    if ((poz.Pozycja.ilosc == null || poz.Pozycja.ilosc <= 0))
                        Message += "będnie określona ilość wyrobu gotowego w pozycji:\n" + poz.Pozycja.kodtw + "\n";
                    if((poz.Pozycja.nr_partii == null || poz.Pozycja.nr_partii == ""))
                        Message += "będnie określony numer partii wyrobu gotowego w pozycji:\n" + poz.Pozycja.kodtw + "\n";
                ilosc_PW_MG += poz.Pozycja.ilosc; 
            }
            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjeRW)
            {
                if (poz.Pozycja.PROD_MGDW != null && !poz.Pozycja.PROD_MGDW.kod.Contains("PZO"))
                {

                    ilosc_RW_MG += poz.Pozycja.ilosc_mg;
                }
            }

            if(ilosc_PW_MG > ilosc_RW_MG)
            {
                Message2 += "Uwaga!!! Ilość po produkcji (" + ilosc_PW_MG + ") jest większa od ilości użytych surowców (" + ilosc_RW_MG + ")" + "\n"+
                    "Czy nadal chcesz zakończyć to zlecenie?";
                var result = MessageBox.Show(Message2,"UWAGA" ,MessageBoxButton.YesNo);
                if(result == MessageBoxResult.No)
                {
                    return false;
                }
            }
            if (Message != "" )
            {
                MessageBox.Show(Message);
                return false;
            }
            else
            {
                return true;
            }

            
        }

        public void Finishprocess()
        {

            if (IsFinish && SprawdzPozycjeMG_PW())
            {

                var result = MessageBox.Show(
                                 "Czy chcesz zakończyć to zlecenie?\n"+
                                 "Zakończenie zlecenia wygeneruje oodpowiednie dokumenty magazynowe!\n"+
                                 "(Jakakolwiek edycja danych nie będzie możliwa).\n",
                                 "Akceptacja", MessageBoxButton.YesNo);
                
                if (result == MessageBoxResult.Yes)
                {
                   
                    PobierzZMgazynu();
                    PrzekazNaMgazynWG();
                    //sprawdz czy dokumenty zostały wygenerowane
                    bool is_dkRW, is_dkPW;
                    is_dkRW = false;
                    is_dkPW = false;
                   

                    using (FZLEntities1 db2 = new FZLEntities1())
                    {
                        foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjeRW)
                        {

                            if (db2.PROD_PRODMGPW.Where(x => x.id_dkprodmg > 0 && x.id_proddp == poz.Pozycja.id).ToList().Count() > 0)
                            {
                                is_dkRW = true;
                            }
                        }
                        foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjePW)
                        {
                            if (db2.PROD_PRODMGPW.Where(x => x.id_dkprodmg > 0 && x.id_proddp == poz.Pozycja.id).ToList().Count() > 0)
                            {
                                is_dkPW = true;
                            }
                        }
                    }

                    if (is_dkRW && is_dkPW)
                    {
                        ProdukcjaInfo.Produkcja.zakonczono = 1;
                        IsFinishDate = true;
                        isFinish = true;
                        ProdukcjaInfo.Produkcja.dataprod = DateTime.Now;
                        db.SaveChanges();
                        RaisePropertyChanged("ProdukcjaInfo");

                        var prod_email_list = db.PRODCONF.Where(x => x.kod_firmy == ProdukcjaInfo.Produkcja.kod_firmy && x.nazwa == "prod_email_list").FirstOrDefault();
                        if (prod_email_list != null)
                        {
                            string email_list = prod_email_list.wartosc;
                            string[] emails = email_list.Split(';');
                            string htmlString = @"<html>
	                      <body style=""width:500px; "">
	                      <p>Użytkownik: " + user.nazwisko + " " + user.imie + @" zakończył zlecnienie: <b>" + ProdukcjaInfo.Produkcja.nazwa + @"</b></p>";
                            htmlString += @"<p>" + ProdukcjaInfo.Produkcja.opis + "</p>";
                            

                            htmlString += @"<table border=0><tr><td>";
                            htmlString += @"Użyte surowce do produkcji:<br>";
                            htmlString += @"</td></tr>";
                            htmlString += @"<tr><td>";
                            htmlString += @"<table border=""1""   align=""left"" width=600> <tr>
    <th>PARTIA</th>
    <th>NUMER DOSTAWY</th>
    <th>DOSTAWCA</th>
    <th>TOWAR</th>
    <th>ILOŚC</th>
  </tr>";
                            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjeRW)
                            {
                                htmlString += @"<tr><td>" + poz.Pozycja.nr_partii + @"</td>";
                                htmlString += @"<td>" + poz.Pozycja.kodmgdw + @"</td>";
                                htmlString += @"<td>" + poz.Pozycja.khnazwa + @"</td>";
                                htmlString += @"<td>" + poz.Pozycja.kodtw + @"</td>";
                                htmlString += @"<td>" + poz.Pozycja.ilosc_mg + @"</td></tr>";
                            }

                            htmlString += @"</table></td></tr>";
                            htmlString += @"<tr><td>";
                            htmlString += @"Otrzymano po produkcji:<br>";
                            htmlString += @"</td></tr>";
                            htmlString += @"<tr><td>";
                            htmlString += @"<table border=""1"" align=""left"" width=600> <tr>
    <th>PARTIA</th>
    <th>TYP</th>
    <th>FRAKCJA</th>
    <th>KOD TOWARU</th>
    <th>ILOŚC</th>
  </tr>";
                            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjePW)
                            {
                                htmlString += @"<tr><td>" + poz.Pozycja.nr_partii + @"</td>";
                                htmlString += @"<td>" + poz.Pozycja.typ_produktu + @"</td>";
                                htmlString += @"<td>" + poz.Pozycja.frakcja + @"</td>";
                                htmlString += @"<td>" + poz.Pozycja.kodtw + @"</td>";
                                htmlString += @"<td><b>" + poz.Pozycja.ilosc+"</b>";
                                htmlString += @"<br>OPAKOWANIA:<br>";
                                foreach (PRODDP_MONIT monit in poz.Pozycja.PRODDP_MONIT)
                                {
                                    if (monit.id_opakowania != null && monit.id_opakowania > 0)
                                    {
                                        htmlString += (monit.ilosc_opakowania > 0) ? @monit.ilosc_opakowania + " szt. " + db.OPAKOWANIA_RODZAJE.Find(monit.id_opakowania).nazwa : @db.OPAKOWANIA_RODZAJE.Find(monit.id_opakowania).nazwa;

                                        if (monit.ilewopakowaniu > 0)
                                        {
                                            htmlString += @" po " + monit.ilewopakowaniu + " kg.";
                                        }
                                    }
                                    if(monit.ilosc_opakowania2 != null && monit.ilosc_opakowania2 > 0)
                                    {
                                        htmlString += @"<br>";
                                        htmlString += @monit.ilosc_opakowania2 +" szt. "+ db.OPAKOWANIA_RODZAJE.Find(monit.id_opakowania2).nazwa;
                                    }
                                }
                                htmlString += @"</td></tr>";
                            }

                            htmlString += @"</table></td></tr>";
                            htmlString += @"</table>";
                            htmlString += @"<br><p><b>ZAWNET HERBS PRODUKCJA</b></p>
	                      </body>
	                      </html>
	                     ";
                            try
                            {
                                RaportMG(false, true);
                                System.Net.Mail.Attachment attachment;
                                attachment = new System.Net.Mail.Attachment(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\RaportMG.pdf");

                                foreach (string email in emails)
                                {
                                    if (attachment != null)
                                    {
                                        Functions.SendEmail("Zlecenie: " + ProdukcjaInfo.Produkcja.nazwa + " zostało zakończone", email, htmlString, attachment);
                                    }
                                    else
                                    {
                                        Functions.SendEmail("Zlecenie: " + ProdukcjaInfo.Produkcja.nazwa + " zostało zakończone", email, htmlString);
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                        
                    }
                    else
                    {
                        if (!is_dkRW)
                            MessageBox.Show("Nie można zakończyć tego zlecenia ponieważ brak dokumentu RW do tego zlecenia.");
                        if (!is_dkPW)
                            MessageBox.Show("Nie można zakończyć tego zlecenia ponieważ brak dokumentu PW do tego zlecenia");
                    }
                    

                }

                else if (result == MessageBoxResult.Cancel)
                {
                    //IsFinishDate = false;
                    return; // <- Don't close window
                }
                else if (result == MessageBoxResult.No)
                {
                    ProdukcjaInfo.Produkcja.zakonczono = 0;
                    IsFinish = false;
                    IsFinishDate = false;
                }
                //MessageBox.Show(ProdukcjaInfo.Produkcja.akceptowano.ToString());
            }
            else
            {
                ProdukcjaInfo.Produkcja.zakonczono = 0;
                IsFinish = false;
                IsFinishDate = false;
            }
        }
        /**
         * Obsługa produkcji częściowej
         */
        public void PartialProcess()
        {
            if (IsPartial)
            {
                ProdukcjaInfo.Produkcja.czesciowa = 1;
            }
            else
            {
                ProdukcjaInfo.Produkcja.czesciowa = 0;
            }
            RaisePropertyChanged("ProdukcjaInfo");
            //MessageBox.Show(ProdukcjaInfo.Produkcja.czesciowa.ToString());
        }

        public void ZeroProcess()
        {
            if (IsZero)
            {
                var result = MessageBox.Show(
                                 "Czy chcesz oznaczyć to zlecenie jako zerowe?\n(Zostanie wygenerowany tylko dokument PZ).\n",
                                 "Akceptacja", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    ProdukcjaInfo.Produkcja.zerowa = 1;
                    IsZero = true;

                    RaisePropertyChanged("ProdukcjaInfo");

                }

                else if (result == MessageBoxResult.Cancel)
                {
                    //IsFinishDate = false;
                    return; // <- Don't close window
                }
                else if (result == MessageBoxResult.No)
                {
                    ProdukcjaInfo.Produkcja.zerowa = 0;
                    IsZero = false;

                }
            }
            else
            {
                ProdukcjaInfo.Produkcja.zerowa = 0;
            }
            RaisePropertyChanged("ProdukcjaInfo");
            //MessageBox.Show(ProdukcjaInfo.Produkcja.czesciowa.ToString());
        }

        public RelayCommand WybierzDWCommand { get; set; }
        public RelayCommand WybierzDWMGCommand { get; set; }
        public RelayCommand WybierzDW_FKCommand { get; set; }
        public RelayCommand WybierzDW_SYMCommand { get; set; }
        public RelayCommand WybierzTWCommand { get; set; }
        public RelayCommand WybierzTW_FKCommand { get; set; }
        public RelayCommand RaportCommand { get; set; }
        public RelayCommand RaportMGCommand { get; set; }
        public RelayCommand RaportFKCommand { get; set; }
        public RelayCommand ZlecenieCommand { get; set; }
        public RelayCommand ZlecenieSymulacjaCommand { get; set; }
        public RelayCommand RaportSymulacjaCommand { get; set; }
        public RelayCommand SpecyfiakcjaCommand { get; set; }
        public RelayCommand KodyCommand { get; set; }
        public RelayCommand zapiszCommand { get; set; }
        public RelayCommand CheckCommand { get; set; }
        public RelayCommand FinishCommand { get; set; }
        public RelayCommand PartialCommand { get; set; }
        public RelayCommand ZeroCommand { get; set; }
        public RelayCommand UsunRWCommand { get; set; }
        public RelayCommand UsunRW_FKCommand { get; set; }
        public RelayCommand UsunRW_SYMCommand { get; set; }
        public RelayCommand UsunPWCommand { get; set; }
        public RelayCommand UsunPW_FKCommand { get; set; }
        public RelayCommand DodajLinieCommand { get; set; }
        public RelayCommand UsunLinieCommand { get; set; }
        public RelayCommand DodajMonitCommand { get; set; }
        public RelayCommand GetProddpMonitCommand { get; set; }
        public RelayCommand WycenaTrybCommand { get; set; }
        public RelayCommand KosztyCommand { get; set; }
        public RelayCommand KosztyFKCommand { get; set; }
        public RelayCommand PrzekazNaMagazynWGCommand { get; set; }
        public RelayCommand PobierzZMagazynuCommand { get; set; }
        public RelayCommand WczytajDoFKCommand { get; set; }

        private RelayCommand<ProdukcjaMaszynaVM> _dodajMaszynaMonitCommand;
        /// <summary>
        /// Gets the TestAgainCommand.
        /// </summary>
        public RelayCommand<ProdukcjaMaszynaVM> DodajMaszynaMonitCommand
        {
            get
            {
                return _dodajMaszynaMonitCommand ?? (_dodajMaszynaMonitCommand = new RelayCommand<ProdukcjaMaszynaVM>(
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

        private RelayCommand<ProdukcjaMaszynaParametrMonitVM> _edytujMaszynaMonitCommand;
        public RelayCommand<ProdukcjaMaszynaParametrMonitVM> EdytujMaszynaMonitCommand
        {
            get
            {
                return _edytujMaszynaMonitCommand ?? (_edytujMaszynaMonitCommand = new RelayCommand<ProdukcjaMaszynaParametrMonitVM>(
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

        private RelayCommand<object> _dodajProdMgDwCommand;
        public RelayCommand<object> DodajProdMgDwCommand
        {
            get
            {
                return _dodajProdMgDwCommand ?? (_dodajProdMgDwCommand = new RelayCommand<object>(
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

        private RelayCommand<object> _dodajProdHMDwCommand;
        public RelayCommand<object> DodajProdHMDwCommand
        {
            get
            {
                return _dodajProdHMDwCommand ?? (_dodajProdHMDwCommand = new RelayCommand<object>(
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

        private RelayCommand<object> _dodajProdHMDwFKCommand;
        public RelayCommand<object> DodajProdHMDwFKCommand
        {
            get
            {
                return _dodajProdHMDwFKCommand ?? (_dodajProdHMDwFKCommand = new RelayCommand<object>(
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

        private RelayCommand<object> _usunProdHMDwCommand;
        public RelayCommand<object> UsunProdHMDwCommand
        {
            get
            {
                return _usunProdHMDwCommand ?? (_usunProdHMDwCommand = new RelayCommand<object>(
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

        private RelayCommand<object> _zmienTowarWGCommand;
        public RelayCommand<object> ZmienTowarWGCommand
        {
            get
            {
                return _zmienTowarWGCommand ?? (_zmienTowarWGCommand = new RelayCommand<object>(
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

        private RelayCommand<object> _usunProdHMDwFKCommand;
        public RelayCommand<object> UsunProdHMDwFKCommand
        {
            get
            {
                return _usunProdHMDwFKCommand ?? (_usunProdHMDwFKCommand = new RelayCommand<object>(
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


        private RelayCommand<ProdukcjaMaszynaParametrMonitVM> _usunMaszynaMonitCommand;
        public RelayCommand<ProdukcjaMaszynaParametrMonitVM> UsunMaszynaMonitCommand
        {
            get
            {
                return _usunMaszynaMonitCommand ?? (_usunMaszynaMonitCommand = new RelayCommand<ProdukcjaMaszynaParametrMonitVM>(
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




        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingCommand { get; set; }
        public RelayCommand<DataGridCellEditEndingEventArgs> CellEditEndingFKCommand { get; set; }
        public RelayCommand<DataGridCellEditEndingEventArgs> MonitCellEditEndingCommand { get; set; }
        public RelayCommand<DataGridCellEditEndingEventArgs> WgCellEditEndingCommand { get; set; }
        public RelayCommand<DataGridCellEditEndingEventArgs> WgFKCellEditEndingCommand { get; set; }
        public ObservableCollection<ViewVM> Views { get; set; }
        public ObservableCollection<CommandVM> Commands { get; set; }


        ProdukcjaVM _produkcjaInfo;
        //public ObservableCollection<ProdukcjaPozycjeVM> ProdukcjaPozycjeRW { get; set; }
        //private ObservableCollection<ProdukcjaPozycjeVM> _produkcjaPozycjePW;
        //public ObservableCollection<ProdukcjaPozycjeVM> ProdukcjaPozycjePW { get; set; }
        public ObservableCollection<ProdukcjaPozycjeMonitVM> ProdukcjaPozycjeMonit { get; set; }
        public ObservableCollection<ProdukcjaPozycjeMonitVM> ProdukcjaPozycjeMonitTMP { get; set; }

        public ObservableCollection<ProdukcjaLiniaVM> ProdukcjaLinie { get; set; }
        public ObservableCollection<ProdukcjaLiniaVM> ProdukcjaLinieUsuniete { get; set; }
        public ObservableCollection<OPAKOWANIA_RODZAJE> Opakowania { get; set; }
        public ObservableCollection<PROD_HMTW> Opakowania2 { get; set; }
        public ObservableCollection<MAGAZYNY> Magazyny { get; set; }



        /*
    public ObservableCollection<ProdukcjaPozycjeVM> ProdukcjaPozycjePW {
        get {
            return _produkcjaPozycjePW;
        }
        set {
            _produkcjaPozycjePW = value;
            RaisePropertyChanged("ProdukcjaPozycjePW");
        }
    }
    */
        public ProdukcjaVM ProdukcjaInfo
        {
            get { return _produkcjaInfo; }
            set
            {
                _produkcjaInfo = value;
                RaisePropertyChanged("ProdukcjaInfo");
            }
        }
        private float _wartoscRW;
        public float WartoscRW
        {
            get { return _wartoscRW; }
            set
            {
                _wartoscRW = value;
                RaisePropertyChanged("WartoscRW");
            }
        }
        private float _wartoscRW_FK;
        public float WartoscRW_FK
        {
            get { return _wartoscRW_FK; }
            set
            {
                _wartoscRW_FK = value;
                RaisePropertyChanged("WartoscRW_FK");
            }
        }

        private ProdukcjaPozycjeVM _selectedRW;
        public ProdukcjaPozycjeVM SelectedRW
        {
            get { return _selectedRW; }
            set
            {
                _selectedRW = value;
                RaisePropertyChanged("SelectedRW");
            }
        }

        private ProdukcjaPozycjeVM _selectedRW_FK;
        public ProdukcjaPozycjeVM SelectedRW_FK
        {
            get { return _selectedRW_FK; }
            set
            {
                _selectedRW_FK = value;
                RaisePropertyChanged("SelectedRW_FK");
            }
        }

        private ProdukcjaPozycjeVM _selectedRW_SYM;
        public ProdukcjaPozycjeVM SelectedRW_SYM
        {
            get { return _selectedRW_SYM; }
            set
            {
                _selectedRW_SYM = value;
                RaisePropertyChanged("SelectedRW_SYM");
            }
        }
        private ProdukcjaPozycjeVM _selectedPW;
        public ProdukcjaPozycjeVM SelectedPW
        {
            get { return _selectedPW; }
            set
            {
                _selectedPW = value;
                RaisePropertyChanged("SelectedPW");
            }
        }
        private ProdukcjaPozycjeVM _selectedPW_FK;
        public ProdukcjaPozycjeVM SelectedPW_FK
        {
            get { return _selectedPW_FK; }
            set
            {
                _selectedPW_FK = value;
                RaisePropertyChanged("SelectedPW_FK");
            }
        }
        private PROD_LINIE linieselectedItem;
        public PROD_LINIE LinieSelectedItem
        {
            get
            {
                return linieselectedItem;
            }
            set
            {
                linieselectedItem = value;
                RaisePropertyChanged("LinieSelectedItem");
            }
        }
        private ProdukcjaLiniaVM produkcjalinieselectedItem;
        public ProdukcjaLiniaVM ProdukcjaLinieSelectedItem
        {
            get
            {
                return produkcjalinieselectedItem;
            }
            set
            {
                produkcjalinieselectedItem = value;
                RaisePropertyChanged("ProdukcjaLinieSelectedItem");
            }
        }

        private ProdukcjaMaszynaVM produkcjamaszynaselectedItem;
        public ProdukcjaMaszynaVM ProdukcjaMaszynaSelectedItem
        {
            get
            {
                return produkcjamaszynaselectedItem;
            }
            set
            {
                produkcjamaszynaselectedItem = value;
                RaisePropertyChanged("ProdukcjaMaszynaSelectedItem");
            }
        }

        private ProdukcjaMaszynaParametrVM produkcjamaszynaparametrselectedItem;
        public ProdukcjaMaszynaParametrVM ProdukcjaMaszynaParametrSelectedItem
        {
            get
            {
                return produkcjamaszynaparametrselectedItem;
            }
            set
            {
                produkcjamaszynaparametrselectedItem = value;
                RaisePropertyChanged("ProdukcjaMaszynaParametrSelectedItem");
            }
        }

        private ObservableCollection<ProdukcjaPozycjeVM> _produkcjaPozycjeRW;
        public ObservableCollection<ProdukcjaPozycjeVM> ProdukcjaPozycjeRW
        {
            get { return _produkcjaPozycjeRW; }
            set
            {
                _produkcjaPozycjeRW = value;
                RaisePropertyChanged("ProdukcjaPozycjeRW");

            }
        }


        //Symulacja rozchodów dla produkcji
        private ObservableCollection<ProdukcjaPozycjeVM> _produkcjaPozycjeSYM;
        public ObservableCollection<ProdukcjaPozycjeVM> ProdukcjaPozycjeSYM
        {
            get { return _produkcjaPozycjeSYM; }
            set
            {
                _produkcjaPozycjeSYM = value;
                RaisePropertyChanged("ProdukcjaPozycjeSYM");
            }
        }

        private ObservableCollection<ProdukcjaPozycjeVM> _produkcjaPozycjeRW_FK;
        public ObservableCollection<ProdukcjaPozycjeVM> ProdukcjaPozycjeRW_FK
        {
            get { return _produkcjaPozycjeRW_FK; }
            set
            {
                _produkcjaPozycjeRW_FK = value;
                RaisePropertyChanged("ProdukcjaPozycjeRW_FK");
            }
        }

        private ObservableCollection<ProdukcjaPozycjeVM> _produkcjaPozycjePW;
        public ObservableCollection<ProdukcjaPozycjeVM> ProdukcjaPozycjePW
        {
            get { return _produkcjaPozycjePW; }
            set
            {
                _produkcjaPozycjePW = value;
                RaisePropertyChanged("ProdukcjaPozycjePW");

            }
        }

        private ObservableCollection<ProdukcjaPozycjeVM> _produkcjaPozycjePW_FK;
        public ObservableCollection<ProdukcjaPozycjeVM> ProdukcjaPozycjePW_FK
        {
            get { return _produkcjaPozycjePW_FK; }
            set
            {
                _produkcjaPozycjePW_FK = value;
                RaisePropertyChanged("ProdukcjaPozycjePW_FK");

            }
        }

        protected void GetProdDP_RW()
        {
            _produkcjaPozycjeRW = new ObservableCollection<ProdukcjaPozycjeVM>();
            var pozycje = (from c in db.PRODDP
                           where c.id_prod == ProdukcjaInfo.Produkcja.id
                           where c.rodzaj_dk == "RW"
                           select c).ToList();

            foreach (PRODDP p in pozycje)
            {

                _produkcjaPozycjeRW.Add(new ProdukcjaPozycjeVM { IsNew = false, Pozycja = p });
            }
            if (pozycje == null)
            {
                _produkcjaPozycjeRW.Add(new ProdukcjaPozycjeVM { IsNew = true, Pozycja = new PRODDP() });
            }
            ProdukcjaPozycjeRW = _produkcjaPozycjeRW;
            this.RaisePropertyChanged("ProdukcjaPozycjePW");
        }


        //Pobiera pozycje dla symulacji produkcji
        protected void GetProdDP_SYM()
        {
            _produkcjaPozycjeSYM = new ObservableCollection<ProdukcjaPozycjeVM>();
            var pozycje = (from c in db.PRODDP_SYM
                           where c.id_prod == ProdukcjaInfo.Produkcja.id
                           where c.rodzaj_dk == "RW"
                           select c).ToList();

            foreach (PRODDP_SYM p in pozycje)
            {
                _produkcjaPozycjeSYM.Add(new ProdukcjaPozycjeVM { IsNew = false, PozycjaSYM = p });
            }
            if (pozycje == null)
            {
                _produkcjaPozycjeSYM.Add(new ProdukcjaPozycjeVM { IsNew = true, PozycjaSYM = new PRODDP_SYM() });
            }
            ProdukcjaPozycjeSYM = _produkcjaPozycjeSYM;
            this.RaisePropertyChanged("ProdukcjaPozycjeSYM");
        }

        protected void GetProdDP_RW_FK()
        {
            _produkcjaPozycjeRW_FK = new ObservableCollection<ProdukcjaPozycjeVM>();
            var pozycje = (from c in db.PRODDP_FK
                           where c.id_prod == ProdukcjaInfo.Produkcja.id
                           where c.rodzaj_dk == "RW"
                           select c).ToList();

            foreach (PRODDP_FK p in pozycje)
            {

                _produkcjaPozycjeRW_FK.Add(new ProdukcjaPozycjeVM { IsNew = false, PozycjaFK = p });
            }
            if (pozycje == null)
            {
                _produkcjaPozycjeRW_FK.Add(new ProdukcjaPozycjeVM { IsNew = true, PozycjaFK = new PRODDP_FK() });
            }
            ProdukcjaPozycjeRW_FK = _produkcjaPozycjeRW_FK;
            this.RaisePropertyChanged("ProdukcjaPozycjePW_FK");
        }

        protected void GetProdDP_PW()
        {
            ObservableCollection<ProdukcjaPozycjeVM> _produkcjaPozycjePW = new ObservableCollection<ProdukcjaPozycjeVM>();
            var pozycje = (from c in db.PRODDP
                           where c.id_prod == ProdukcjaInfo.Produkcja.id
                           where c.rodzaj_dk == "PW"
                           select c).ToList();
            short? lp;
            lp = 0;
            foreach (PRODDP p in pozycje)
            {
                if (p.idproddw > 0)
                {
                    lp = lp++;
                  //  if ((p.lp == null || p.lp == 0))
                  //  {

                   //     p.lp = lp;
                   // }
                }
                _produkcjaPozycjePW.Add(new ProdukcjaPozycjeVM { IsNew = false, Pozycja = p });
            }
            ProdukcjaPozycjePW = _produkcjaPozycjePW;
            this.RaisePropertyChanged("ProdukcjaPozycjePW");
        }

        protected void GetProdDP_PW_FK()
        {
            _produkcjaPozycjePW_FK = new ObservableCollection<ProdukcjaPozycjeVM>();
            var pozycje = (from c in db.PRODDP_FK
                           where c.id_prod == ProdukcjaInfo.Produkcja.id
                           where c.rodzaj_dk == "PW"
                           select c).ToList();
            short? lp;
            lp = 0;
            foreach (PRODDP_FK p in pozycje)
            {
               // if (p.idproddw > 0)
               // {
                //    lp = lp++;
                    //  if ((p.lp == null || p.lp == 0))
                    //  {

                    //     p.lp = lp;
                    // }
                //}
                _produkcjaPozycjePW_FK.Add(new ProdukcjaPozycjeVM { IsNew = false, PozycjaFK = p });
            }
            ProdukcjaPozycjePW_FK = _produkcjaPozycjePW_FK; 
            this.RaisePropertyChanged("ProdukcjaPozycjePW_FK");
        }

        protected void WczytajPozycjePW_MONIT()
        {
            //db = new FZLEntities1();
           // using (FZLEntities1 db2 = new FZLEntities1())
            //{
            var query = (from c in db.PRODDP_MONIT
                         where c.PRODDP.id_prod == ProdukcjaInfo.Produkcja.id
                         select c).ToList();
            
            ProdukcjaPozycjeMonit = new ObservableCollection<ProdukcjaPozycjeMonitVM>();
            int lp = 0;
            
            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjePW)
            {
                foreach (PRODDP_MONIT monit in query)
                {
                    if (poz.Pozycja.id == monit.id_proddp)
                    {
                       // lp += 1; 
                        ProdukcjaPozycjeMonitVM monitVM = new ProdukcjaPozycjeMonitVM();
                        monitVM.lp = (short)lp;
                        monitVM.ProdDPMonit = monit;
                        ProdukcjaPozycjeMonit.Add(monitVM);
                       // WczytajOpakowaniaPowoazania(monitVM);
                    }
                }
                lp += 1;
            }
            //}
            //GetProdDP_MONIT();
        }

        //Wczytuje powiazania opakowan z magazynem produkcyjnym
        protected void WczytajOpakowaniaPowoazania(ProdukcjaPozycjeMonitVM monit)
        {
           if(OpakowaniaPowiazania == null)
                OpakowaniaPowiazania = new List<PROD_PRODMGPW>();
            if (dostawyMGopakowania == null)
                dostawyMGopakowania = new Dictionary<int, PROD_PRODMGPW>();
            //Sprawdz czy istnieja juz powiazania opakowan z danym monitem
            bool jest;
            //using (FZLEntities1 db2 = new FZLEntities1())
           //   {
            var opak_pw = (from c in db.PROD_PRODMGPW
                           where c.id_proddpmonit == monit.ProdDPMonit.id
                           
                           select c).ToList();
            if (opak_pw.Count > 0)
            {
                foreach (PROD_PRODMGPW pw in opak_pw)
                {
                        
                    jest = false;
                    foreach (PROD_PRODMGPW pw2 in OpakowaniaPowiazania)
                    {
                        if (pw.id_proddpmonit == pw2.id_proddpmonit && pw.id_prodmgdw == pw2.id_prodmgdw)
                        {
                            jest = true;
                                break;
                        }

                    }

                    if (!jest)
                    {
                        OpakowaniaPowiazania.Add(pw);
                        dostawyMGopakowania.Add((short)monit.lp, pw);
                    }
                    
                    //OpakowaniaPowiazania.Add(pw);
              //  }
            }
            }
        }

        //Dodaje moit wyrobu gotowego do powiazan z magazynem opakowan
        protected void DodajDoOpakowaniaPowiazania(ProdukcjaPozycjeMonitVM monit, PROD_MGDW dw, double ilosc)
        {
            PROD_PRODMGPW opakpw;
            //Sprawdz czy istnieje juz takie powiązanie jezeli tak to uaktualnij ilosc, jezeli nie dodaj nowe
            int index=-1;
            bool jest=false;
            List<int> ids = new List<int>();
            if (OpakowaniaPowiazania != null && OpakowaniaPowiazania.Count() > 0 )
            {
                foreach (PROD_PRODMGPW item in OpakowaniaPowiazania)
                {

                    if (item.id_prodmgdw == dw.id && item.id_proddpmonit == monit.ProdDPMonit.id)
                    {

                        jest = true;

                        index = OpakowaniaPowiazania.IndexOf(item);

                    }

                }
            }
            else
            {
                OpakowaniaPowiazania = new List<PROD_PRODMGPW>();
            }
            if (dostawyMGopakowania == null)
                dostawyMGopakowania = new Dictionary<int, PROD_PRODMGPW>();

            if(!jest)
            
            {
                opakpw = new PROD_PRODMGPW();
             
                opakpw.id_prodmgdw = dw.id;
                opakpw.id_proddpmonit = monit.ProdDPMonit.id;
                opakpw.ilosc = ilosc;
                opakpw.id_proddp = monit.ProdDPMonit.id_proddp;
                OpakowaniaPowiazania.Add(opakpw);
                dostawyMGopakowania.Add((short)monit.lp, opakpw);
            }
            else
            {
                if(index >=0)
                    OpakowaniaPowiazania[index].ilosc = ilosc;
            }
           
        }
        //Wystawia domumenty magazynowe zwiazane z magazynem produkcyjnym
        protected void WystawDokumentyMagazynowe()
        {
            if(DostawyProdMGPowiazania != null && DostawyProdMGPowiazania.Count > 0)
            {
                RezerwujDostawyMG();
                PobierzDostawyProdMgPw();
                ProdukcjaMagazynVM DkMG = new ProdukcjaMagazynVM();
                PROD_MG dkMG; //Dokument magazynowy
                PROD_MZ dkMZ; //Pozycja dokumentu magazynowego
                bool jest = false;
              
                foreach (PROD_PRODMGPW pozpw in DostawyProdMGPowiazania)
                {
                    //Sprawdz czy do danego powiazania został juz wystawiony dokument magazynowy
                    if (pozpw.id_dkprodmg != null && pozpw.id_dkprodmg > 0)
                    {
                        //dokument zostal juz wystawiony
                        jest = true;
                        dkMZ = db.PROD_MZ.Where(x => x.id == pozpw.id_dkprodmg).FirstOrDefault();
                        //DksMG.Add(new ProdukcjaMagazynVM { ProdukcjaMG = dkMZ.PROD_MG, IsNew = false });
                    }
                    else
                    {
                        //Jeżeli nie to generuj nowy dokument magazynowy 
                    }

                }
                if (!jest)
                {
                    DkMG.WystawDokumentRozchodowy(DostawyProdMGPowiazania, "RWS");
                }
                else
                {
                    MessageBox.Show("Dla tego zlecenia zostały już wygenerowane dokumenty magazynowe!\n");
                }
            }
        }
        
        //pobierz z magazynu do produkcji
        protected void PobierzZMgazynu()
        {
            var result = MessageBox.Show(
                                "Operacja pobiera surowce z magazynu produkcyjnego.\n Dokumenty mozna wygenerowac tylko raz dla danego zlecenia.\n Czy wygenerować dokumenty?",
                                "Akceptacja", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                 WystawDokumentyMagazynowe();
            }
        }

        //przekaz na magazyn wyrobow gotowych
        protected void PrzekazNaMgazynWG()
        {

           // if (SprawdzPozycjeMG_PW() == true)
           // {
                int result_code;
                var result = MessageBox.Show(
                                    "Operacja generuje dokumeny przychodowe na magazynie produkcyjnym.\n Dokumenty mozna wygenerowac tylko raz dla danego zlecenia.\n Czy wygenerować dokumenty?",
                                    "Akceptacja", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    result_code = ProdukcjaMagazyn.PrzekazNaMagazynWG(ProdukcjaInfo.Produkcja.id);
                    if (result_code == 0)
                    {
                        MessageBox.Show("Nie można ponownie wygenerować już raz wystawionych dokumentów magazynowych dla tego zlecenia!");
                    }
                    else if (result_code > 0)
                    {
                        MessageBox.Show("Wygenerowano dokument: " + db.PROD_MG.Where(x => x.id == result_code).FirstOrDefault().kod);
                    }

                }
            //}
            //else
           // {
              //  MessageBox.Show("Nie wygenerowano dokumentów magazynowych!");
            //}
        }

        protected void WystawDokumentyMagazynoweOpakowania()
        {
            ProdukcjaMagazynVM DkMG = new ProdukcjaMagazynVM();
            PROD_MG dkMG; //Dokument magazynowy
            PROD_MZ dkMZ; //Pozycja dokumentu magazynowego
            bool jest = false;
            WczytajPozycjePW_MONIT();

            if (OpakowaniaPowiazania != null && OpakowaniaPowiazania.Count() > 0)
            {
                //Sprawdz czy istnieja juz dokumenty magazynowe powiazane z pozycjami dla tego zlecenia
                foreach (PROD_PRODMGPW oppw in OpakowaniaPowiazania)
                {
                    //Sprawdz czy do danego powiazania został juz wystawiony dokument magazynowy
                    if (oppw.id_dkprodmg != null && oppw.id_dkprodmg > 0)
                    {
                        //dokument zostal juz wystawiony
                        jest = true;
                        dkMZ = db.PROD_MZ.Where(x => x.id == oppw.id_dkprodmg).FirstOrDefault();
                        //DksMG.Add(new ProdukcjaMagazynVM { ProdukcjaMG = dkMZ.PROD_MG, IsNew = false });
                    }
                    else
                    {
                        //Jeżeli nie to generuj nowy dokument magazynowy 
                    }

                }
                if (!jest)
                {
                    DkMG.WystawDokumentRozchodowy(OpakowaniaPowiazania, "RWO");
                }
                else
                {
                    MessageBox.Show("Dla tego zlecenia zostały już wygenerowane dokumenty magazynowe!\n");
                }

            }
        }

        protected void GetProdDP_MONIT()
        {

            if (SelectedPW != null && ProdukcjaPozycjeMonit != null)
            {
                ProdukcjaPozycjeMonitTMP = new ObservableCollection<ProdukcjaPozycjeMonitVM>();
                foreach (ProdukcjaPozycjeMonitVM item in ProdukcjaPozycjeMonit)
                {
                    if ((item.lp) == ProdukcjaPozycjePW.IndexOf(SelectedPW))
                    {
                        ProdukcjaPozycjeMonitTMP.Add(item);
                    }
                }
                RaisePropertyChanged("ProdukcjaPozycjeMonitTMP");
                //MessageBox.Show(SelectedPW.Pozycja.kodtw);
            }
            // SumujIlosciPWMONIT();
        }

        protected void SumujIlosciPWMONIT()
        {
            int lp = 0;
            double? suma_pozycji = 0;
            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjePW)
            {
                suma_pozycji = 0;
                if (ProdukcjaPozycjeMonit != null)
                {
                    foreach (ProdukcjaPozycjeMonitVM item in ProdukcjaPozycjeMonit)
                    {

                        if (item.lp == lp)
                        {
                            suma_pozycji = +item.ProdDPMonit.ilosc;
                            // MessageBox.Show(suma_pozycji.ToString());
                        }
                    }
                    //sumuj tylko te które nie maja wpisanej ilosci
                    //  if (!(poz.Pozycja.ilosc > 0 ))
                    if (!(suma_pozycji > 0))
                    {
                        // poz.Pozycja.ilosc = suma_pozycji;
                    }
                }
                lp += 1;
            }
            this.RaisePropertyChanged("ProdukcjaPozycjePW");
        }
        /**
         * Rezerwuje ilości z dostaw opakowania w zleceniu produckyjnym
         * */
        public void RezerwujOpakowania(ProdukcjaPozycjeVM pozpw)
        {
            foreach (ProdukcjaPozycjeMonitVM item in ProdukcjaPozycjeMonit)
            {
                if(item.ProdDPMonit.id_opakowania != null && pozpw.Pozycja.id == item.ProdDPMonit.id_proddp)
                {

                }
                
            }

        }

        protected void PobierzOpakowania()
        {
            var query = (from c in db.OPAKOWANIA_RODZAJE
                         select c).ToList();
            Opakowania = new ObservableCollection<OPAKOWANIA_RODZAJE>();
            foreach (OPAKOWANIA_RODZAJE opak in query)
            {
                Opakowania.Add(opak);
            }
            this.RaisePropertyChanged("Opakowania");
        }

        protected void PobierzOpakowania2()
        {
            if(Opakowania2 == null)
            {
                Opakowania2 = new ObservableCollection<PROD_HMTW>();
            }
            
            if (ProdukcjaPozycjeRW != null && ProdukcjaPozycjeRW.Count > 0)
            {
        
                    foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjeRW)
                    {
                        if(poz.Pozycja.kodmgdw != null && poz.Pozycja.kodmgdw.Contains("PZO"))
                        {
                        //  MessageBox.Show(poz.Pozycja.kodtw);
                        var query = (from c in db.PROD_HMTW
                                    
                                     where c.id == poz.Pozycja.idtw
                                    
                                     select c).ToList();

                        foreach (PROD_HMTW opak in query)
                        {
                            Opakowania2.Add(opak);
                        }
                    }
                                       
                    }
                
            }
           
            this.RaisePropertyChanged("Opakowania2");
        }

        protected void PobierzMgazyny()
        {
            var query = (from c in db.MAGAZYNY
                         where c.aktywny==1
                         select c
                         ).ToList();
            Magazyny = new ObservableCollection<MAGAZYNY>();
            foreach (MAGAZYNY mag in query)
            {
                Magazyny.Add(mag);
            }
            this.RaisePropertyChanged("Magazyny");
        }

        public ProdukcjaDwVM SelectedProdDW { get; set; }
        public ObservableCollection<ProdukcjaDwVM> ProdDWs { get; set; }

        protected void getGetDostawy()
        {
            ObservableCollection<ProdukcjaDwVM> _proddws = new ObservableCollection<ProdukcjaDwVM>();
            var dostawy = (from p in db.PROD_HMDW
                           where p.stan > 0
                           orderby p.opistw
                           select p).ToList();

            foreach (PROD_HMDW prod in dostawy)
            {
                _proddws.Add(new ProdukcjaDwVM { IsNew = false, ProdukcjaDW = prod });
            }
            ProdDWs = _proddws;
            RaisePropertyChanged("ProdDWs");

        }

        public void GetWaluty()
        {
            var wal = (from w in db.WALUTY
                       where w.aktywna == 1
                       select w).ToList();
            if (wal.Count() > 0)
            {
                _waluty = new List<string>();
                foreach (WALUTY w in wal)
                {
                    _waluty.Add(w.kod);
                }
                Waluty = _waluty;
                this.RaisePropertyChanged("Waluty");
            }
        }

        public void PobierzZapisaneLinie()
        {

            ProdukcjaLinie = new ObservableCollection<ProdukcjaLiniaVM>();
            ProdukcjaLiniaVM linia;

            foreach (PROD_LINIE_PW pw in ProdukcjaInfo.Produkcja.PROD_LINIE_PW)
            {
                //MessageBox.Show(pw.nazwa);
                linia = new ProdukcjaLiniaVM();
                linia.IsNew = false;
                linia.LiniaPW = pw;
                linia.getMaszyny();

                ProdukcjaLinie.Add(linia);

            }

            RaisePropertyChanged("ProdukcjaLinie");
        }

        protected override void InsertDW()
        {

        }


        public ProdukcjaDetaleViewModel()
            : base()
        {
            TYPYPRODDP = Functions.GetTypyPozycjiWG();
            ProdukcjaInfo = new ProdukcjaVM();
           
            Messenger.Default.Register<ProdukcjaVM>(this, prodvm => getProdukcjaVM(prodvm));

            bool flag = ProdukcjaInfo == null;
            if (flag)
            {
                ProdukcjaInfo = new ProdukcjaVM();
               
            }
            isFinish = false;
            isCanSave = true;
            _wgWycenaTryb = true;
            ProdukcjaInfo.Produkcja.wycena_typ = 1;
            ProdukcjaInfo.Produkcja.data = (DateTime?)DateTime.Now;
            ProdukcjaInfo.Produkcja.termin_rozpoczecia = (DateTime?)DateTime.Now;
            //RaisePropertyChanged("ProdukcjaInfo");
            _produkcjaPozycjeRW = new ObservableCollection<ProdukcjaPozycjeVM>();
            ProdukcjaPozycjePW = new ObservableCollection<ProdukcjaPozycjeVM>();
            ProdukcjaLinie = new ObservableCollection<ProdukcjaLiniaVM>();
            ProdukcjaLinieUsuniete = new ObservableCollection<ProdukcjaLiniaVM>();
            PobierzLinie();

            RaisePropertyChanged("ProdukcjaLinie");
            // ProdukcjaPozycjePW.Clear();
            // ProdukcjaPozycjeRW.Clear();
            // Views = new ObservableCollection<ViewVM>();
            //  Views.Clear();
            //  Views.Add(new ViewVM { ViewDisplay = "Wybierz dostawe", ViewType = typeof(WyborDwView), ViewModelType = typeof(WyborDwViewModel) });
            //ObservableCollection<ViewVM> views = new ObservableCollection<ViewVM>
            // {
            //      new ViewVM{ ViewDisplay="Wybierz dostawe", ViewType = typeof(WyborDwView), ViewModelType = typeof(WyborDwViewModel)},
            //  };

            // Views = views;
            //RaisePropertyChanged("Views");

            // ObservableCollection<CommandVM> commands = new ObservableCollection<CommandVM>
            //   {
            //       new CommandVM{ CommandDisplay="Wstaw DW", Message=new CommandMessage{ Command = CommandType.InsertDW}}

            //   };

            // Commands = commands;
            // RaisePropertyChanged("Commands");
            zapiszCommand           = new RelayCommand(Zapisz);
            WybierzDWCommand        = new RelayCommand(PokazDW);
            WybierzDWMGCommand      = new RelayCommand(PokazDWMG);
            WybierzDW_FKCommand     = new RelayCommand(PokazDWFK);
            WybierzDW_SYMCommand    = new RelayCommand(WstawSYM);
            WybierzTWCommand        = new RelayCommand(PokazTW);
            WybierzTW_FKCommand     = new RelayCommand(PokazTW_FK);
            RaportCommand           = new RelayCommand(Raport);
            RaportMGCommand         = new RelayCommand(ShowRaportMG);
            RaportFKCommand         = new RelayCommand(RaportFK);
            ZlecenieCommand         = new RelayCommand(Zlecenie);
            ZlecenieSymulacjaCommand = new RelayCommand(ZlecenieSymulacja);
            RaportSymulacjaCommand = new RelayCommand(RaportSymulacja);
            SpecyfiakcjaCommand = new RelayCommand(SpecyfikacjaPakowania);
            KodyCommand = new RelayCommand(Kody);
            CheckCommand = new RelayCommand(Checkprocess);
            FinishCommand = new RelayCommand(Finishprocess);
            PartialCommand = new RelayCommand(PartialProcess);
            ZeroCommand = new RelayCommand(ZeroProcess);
            WycenaTrybCommand = new RelayCommand(WycenaTryb);
            KosztyCommand = new RelayCommand(WyliczKosztyProdukcji);
            KosztyFKCommand = new RelayCommand(WyliczKosztyProdukcjiFK);
            PrzekazNaMagazynWGCommand = new RelayCommand(PrzekazNaMgazynWG);
            PobierzZMagazynuCommand = new RelayCommand(PobierzZMgazynu);
            WczytajDoFKCommand = new RelayCommand(WczytajPozycjeDoFK);
            UsunRWCommand = new RelayCommand(UsunPocycjeRW);
            UsunRW_FKCommand = new RelayCommand(UsunPocycjeRW_FK);
            UsunRW_SYMCommand = new RelayCommand(UsunPocycjeRW_SYM);
            UsunPWCommand = new RelayCommand(UsunPocycjePW);
            UsunPW_FKCommand = new RelayCommand(UsunPocycjePW_FK);
            DodajLinieCommand = new RelayCommand(DodajLinie);
            UsunLinieCommand = new RelayCommand(UsunLinie);
            DodajMonitCommand = new RelayCommand(wstawMonitPW);
            _dodajMaszynaMonitCommand = new RelayCommand<ProdukcjaMaszynaVM>(i => wstawMaszynaMonit(i));
            _edytujMaszynaMonitCommand = new RelayCommand<ProdukcjaMaszynaParametrMonitVM>(i => edytujMaszynaMonit(i));
            _usunMaszynaMonitCommand = new RelayCommand<ProdukcjaMaszynaParametrMonitVM>(i => usunMaszynaMonit(i));
            _dodajProdMgDwCommand = new RelayCommand<object>(i => wstawPozycjeProdMgDW(i));
            _dodajProdHMDwCommand = new RelayCommand<object>(i => wstawPozycjeProdHMDW(i));
            _dodajProdHMDwFKCommand = new RelayCommand<object>(i => wstawPozycjeProdHMDW_FK(i));
            _usunProdHMDwCommand = new RelayCommand<object>(i => usunPozycjeProdHMDW(i));
            _zmienTowarWGCommand = new RelayCommand<object>(i => zmienTowarWG(i));
            _usunProdHMDwFKCommand = new RelayCommand<object>(i => usunPozycjeProdHMDW_FK(i));
            CellEditEndingCommand = new RelayCommand<DataGridCellEditEndingEventArgs>(args => SprawdzStany());
            CellEditEndingFKCommand = new RelayCommand<DataGridCellEditEndingEventArgs>(args => SprawdzStanyFK());
            MonitCellEditEndingCommand = new RelayCommand<DataGridCellEditEndingEventArgs>(args => SprawdzStanyWGMonit());

            WgCellEditEndingCommand = new RelayCommand<DataGridCellEditEndingEventArgs>(args => PrzeliczWartosci());
            WgFKCellEditEndingCommand = new RelayCommand<DataGridCellEditEndingEventArgs>(args => PrzeliczWartosciFK());
            GetProddpMonitCommand = new RelayCommand(GetProdDP_MONIT);
            _rozliczenieView = new ProdukcjaRozliczenieFKView(this);
            RaisePropertyChanged("RozliczenieView");
            _symulacjaView = new ProdukcjaSymulacjaView(this);
            RaisePropertyChanged("SymulacjaView");
            //PrzeliczWartosci();
            //RaisePropertyChanged("ProdukcjaInfo");
            //MessageBox.Show(IsCanSave.ToString());
            //RaisePropertyChanged("IsFinish");
            //RaisePropertyChanged("IsCanSave");
        }

        private void wstawPozycjeProdHMDW(object obj)
        {
            ProdukcjaPozycjeVM x = obj as ProdukcjaPozycjeVM;
            ProdukcjaPozycjeVM poz;
            ProdukcjaDwVM dostawa;
            WyborDwWindow dialog = new WyborDwWindow();
            bool jest;
            // if (this.dostawy != null)
            // dialog.selDW = this.dostawy;
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (x != null)
            {
                if (dialogResult == true && dialog.selDW.Count() > 0)
                {
                    jest = false;
                    dostawa = dialog.selDW.FirstOrDefault();
                    if (_produkcjaPozycjeRW.Count > 0)
                    {
                        foreach (ProdukcjaPozycjeVM ppoz in _produkcjaPozycjeRW)
                        {
                            //Sprawdz czy dostawa jest juz ujenta w danej pozycji
                            if (ppoz.Pozycja.iddw == dostawa.ProdukcjaDW.id_fk)
                            {
                                jest = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    //Jezeli nie ma to dodaj nową pozycje
                    if (!jest)
                    {
                        poz = x;
                        //poz = new ProdukcjaPozycjeVM();
                        poz.Pozycja.kodtw = dostawa.ProdukcjaDW.kodtw;
                        poz.Pozycja.khnazwa = dostawa.ProdukcjaDW.khnazwa;
                        poz.Pozycja.iddw = dostawa.ProdukcjaDW.id_fk;
                        poz.Pozycja.idproddw = dostawa.ProdukcjaDW.id;
                        //poz.Pozycja.opis = dostawa.ProdukcjaDW.kod;
                        poz.Pozycja.nazwatw = dostawa.ProdukcjaDW.opistw;
                        poz.Pozycja.ilosc = dostawa.ProdukcjaDW.stan;
                        poz.Pozycja.cena = dostawa.ProdukcjaDW.cena;
                        poz.Pozycja.idtw = (int)dostawa.ProdukcjaDW.idtw;
                        poz.Pozycja.koddw = dostawa.ProdukcjaDW.kod;
                        //Obsługa wstawiania wigotnosci i czystosci z prob i partii
                        var query = (from f in db.PARTIE_FK
                                     from p in db.PARTIE
                                     where f.iddk_fki == dostawa.ProdukcjaDW.iddkpz
                                     where p.id == f.id_partii
                                     where f.kod_firmy == dostawa.ProdukcjaDW.kod_firmy
                                     select p).FirstOrDefault();
                        if (query != null)
                        {
                            poz.Pozycja.czystosc = query.czystosc;
                            poz.Pozycja.wilgotnosc = query.wilgotnosc;
                        }
                        //obsługa wstawiania informacji o wyrobie gptwym ponownie uzytym do produkcji
                        //MessageBox.Show(dostawa.ProdukcjaDW.iddkpz.ToString());
                        PRODDP surowiec_wg = Functions.PobierzPozycjePrzezDokument(dostawa.ProdukcjaDW.iddkpz);
                        if (surowiec_wg != null)
                        {
                            poz.Pozycja.czystosc = surowiec_wg.czystosc;
                            poz.Pozycja.wilgotnosc = surowiec_wg.wilgotnosc;
                            poz.Pozycja.frakcja = surowiec_wg.frakcja;
                            poz.Pozycja.opis = surowiec_wg.opis;
                            //MessageBox.Show(surowiec_wg.id_hm.ToString());
                        }

                       // poz.Pozycja.lp = (short)(ProdukcjaPozycjeRW.Where(p=>p.Pozycja.idproddw > 0).ToList().Max(i=>i.Pozycja.lp) + 1);
                        // poz.Pozycja.jm = dostawa.ProdukcjaDW.
                       // _produkcjaPozycjeRW.Add(poz);
                    }
                }

                this.RaisePropertyChanged("ProdukcjaPozycjeRW");
            }
        
        }


        private void wstawPozycjeProdHMDW_FK(object obj)
        {
            ProdukcjaPozycjeVM x = obj as ProdukcjaPozycjeVM;
            ProdukcjaPozycjeVM poz;
            ProdukcjaDwVM dostawa;
            WyborDwWindow dialog = new WyborDwWindow();
            bool jest;
            // if (this.dostawy != null)
            // dialog.selDW = this.dostawy;
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (x != null)
            {
                if (dialogResult == true && dialog.selDW.Count() > 0)
                {
                    jest = false;
                    dostawa = dialog.selDW.FirstOrDefault();
                    if (_produkcjaPozycjeRW.Count > 0)
                    {
                        foreach (ProdukcjaPozycjeVM ppoz in _produkcjaPozycjeRW_FK)
                        {
                            //Sprawdz czy dostawa jest juz ujenta w danej pozycji
                            if (ppoz.PozycjaFK.iddw == dostawa.ProdukcjaDW.id_fk)
                            {
                                jest = true;
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                    //Jezeli nie ma to dodaj nową pozycje
                    if (!jest)
                    {
                        poz = x;
                        //poz = new ProdukcjaPozycjeVM();
                        poz.PozycjaFK.kodtw = dostawa.ProdukcjaDW.kodtw;
                        poz.PozycjaFK.khnazwa = dostawa.ProdukcjaDW.khnazwa;
                        poz.PozycjaFK.iddw = dostawa.ProdukcjaDW.id_fk;
                        poz.PozycjaFK.idproddw = dostawa.ProdukcjaDW.id;
                        //poz.Pozycja.opis = dostawa.ProdukcjaDW.kod;
                        poz.PozycjaFK.nazwatw = dostawa.ProdukcjaDW.opistw;
                        poz.PozycjaFK.ilosc = dostawa.ProdukcjaDW.stan;
                        poz.PozycjaFK.cena = dostawa.ProdukcjaDW.cena;
                        poz.PozycjaFK.idtw = (int)dostawa.ProdukcjaDW.idtw;
                        poz.PozycjaFK.koddw = dostawa.ProdukcjaDW.kod;
                        //Obsługa wstawiania wigotnosci i czystosci z prob i partii
                        var query = (from f in db.PARTIE_FK
                                     from p in db.PARTIE
                                     where f.iddk_fki == dostawa.ProdukcjaDW.iddkpz
                                     where p.id == f.id_partii
                                     where f.kod_firmy == dostawa.ProdukcjaDW.kod_firmy
                                     select p).FirstOrDefault();
                        if (query != null)
                        {
                            poz.PozycjaFK.czystosc = query.czystosc;
                            poz.PozycjaFK.wilgotnosc = query.wilgotnosc;
                        }
                        //obsługa wstawiania informacji o wyrobie gptwym ponownie uzytym do produkcji
                        //MessageBox.Show(dostawa.ProdukcjaDW.iddkpz.ToString());
                        PRODDP surowiec_wg = Functions.PobierzPozycjePrzezDokument(dostawa.ProdukcjaDW.iddkpz);
                        if (surowiec_wg != null)
                        {
                            poz.PozycjaFK.czystosc = surowiec_wg.czystosc;
                            poz.PozycjaFK.wilgotnosc = surowiec_wg.wilgotnosc;
                            poz.PozycjaFK.frakcja = surowiec_wg.frakcja;
                            poz.PozycjaFK.opis = surowiec_wg.opis;
                            //MessageBox.Show(surowiec_wg.id_hm.ToString());
                        }

                        // poz.Pozycja.lp = (short)(ProdukcjaPozycjeRW.Where(p=>p.Pozycja.idproddw > 0).ToList().Max(i=>i.Pozycja.lp) + 1);
                        // poz.Pozycja.jm = dostawa.ProdukcjaDW.
                        // _produkcjaPozycjeRW.Add(poz);
                    }
                }

                this.RaisePropertyChanged("ProdukcjaPozycjeRW_FK");
            }

        }

        private void usunPozycjeProdHMDW(object obj)
        {
            try
            {
                if (obj != null)
                {
                    ProdukcjaPozycjeVM x = obj as ProdukcjaPozycjeVM;
                    var result = MessageBox.Show(
                                       "Czy napewno chcesz " + user.imie + " usunąć dostawę: " + x.Pozycja.koddw + "?",
                                       "Akceptacja", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                       
                        var item_HMDW = db.PROD_HMDW.FirstOrDefault(i => i.id == x.Pozycja.idproddw);
                        item_HMDW.iloscdoprod -= x.Pozycja.ilosc;
                        
                        x.Pozycja.ilosc = null;
                        x.Pozycja.iddw = null;
                        x.Pozycja.idproddw = null;
                        x.Pozycja.koddw = null;

                        //usun informacje o powiazaniu dostaty z ta pozycja
                        var item = db.PROD_PW.FirstOrDefault(i => i.id_proddp == x.Pozycja.id);
                        if (item != null)
                        {
                            db.PROD_PW.Remove(item);
                        }

                        RaisePropertyChanged("ProdukcjaPozycjeRW");


                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
           
        }

        private void usunPozycjeProdHMDW_FK(object obj)
        {
            try
            {
                if (obj != null)
                {
                    ProdukcjaPozycjeVM x = obj as ProdukcjaPozycjeVM;
                    var result = MessageBox.Show(
                                       "Czy napewno chcesz " + user.imie + " usunąć dostawę: " + x.Pozycja.koddw + "?",
                                       "Akceptacja", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {

                        var item_HMDW = db.PROD_HMDW.FirstOrDefault(i => i.id == x.PozycjaFK.idproddw);
                        item_HMDW.iloscdoprod -= x.PozycjaFK.ilosc;

                        x.PozycjaFK.ilosc = null;
                        x.PozycjaFK.iddw = null;
                        x.PozycjaFK.idproddw = null;
                        x.PozycjaFK.koddw = null;
                       
                        //usun informacje o powiazaniu dostaty z ta pozycja
                        var item = db.PROD_PW.FirstOrDefault(i => i.id_proddpfk == x.PozycjaFK.id);
                        if (item != null)
                        {
                            db.PROD_PW.Remove(item);
                        }

                        RaisePropertyChanged("ProdukcjaPozycjeRW_FK");


                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

        }

        private void zmienTowarWG(object obj)
        {
            if (obj != null)
            {
                ProdukcjaPozycjeVM poz = (ProdukcjaPozycjeVM)obj;
                 
                //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
                WyborTwWindow dialog = new WyborTwWindow();


            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                    //dialog.selTW.Last();
                    if (dialog.selTW != null && dialog.selTW.Count() > 0)
                    {
                        var towar = dialog.selTW.First();
                        poz.Pozycja.kodtw = towar.ProdukcjaTW.kod;
                        poz.Pozycja.nazwatw = towar.ProdukcjaTW.nazwa;
                        //poz.Pozycja.iddw = towar.ProdukcjaTW.id_fk;
                        //poz.Pozycja.idproddw = towar.ProdukcjaTW.id;
                        //poz.Pozycja.opis = towar.ProdukcjaTW.kod;
                        poz.Pozycja.nazwatw = towar.ProdukcjaTW.nazwa;
                        poz.Pozycja.jm = towar.ProdukcjaTW.jm;
                        //poz.Pozycja.ilosc = towar.ProdukcjaDW.iloscdosp;
                        poz.Pozycja.idtw = (int)towar.ProdukcjaTW.id_fk;
                        poz.Pozycja.kodpaskowy = towar.ProdukcjaTW.kodpaskowy;
                        RaisePropertyChanged("ProdukcjaPozycjePW");
                        // MessageBox.Show(dialog.selTW.First().ProdukcjaTW.kod);

                        //sprawdz czy są wygenerowane dokumenty magazynowe z tego zlecenia,
                        var mgpw = db.PROD_PRODMGPW.Where(x => x.id_proddp == poz.Pozycja.id && x.id_dkprodmg > 0).FirstOrDefault();
                        //jezeli jest to zmien kod towaru na dokumecie i dostawie
                        if (mgpw != null)
                        {
                            //MessageBox.Show(mgpw.PROD_MZ.kod);
                            mgpw.PROD_MZ.idtw = (int)towar.ProdukcjaTW.id;
                            mgpw.PROD_MZ.kod = towar.ProdukcjaTW.kod;
                            if(mgpw.PROD_MZ.PROD_MGDW.First() != null)
                            {
                                mgpw.PROD_MZ.PROD_MGDW.First().kodtw = towar.ProdukcjaTW.kod;
                                mgpw.PROD_MZ.PROD_MGDW.First().idtw = (int)towar.ProdukcjaTW.id;
                            }
                        }
                    }

                //wstawPozycjeTW(dialog.selTW);
            }
        }

        }

        private void wstawPozycjeProdMgDW(object obj)
        {
            ProdukcjaPozycjeVM x = obj as ProdukcjaPozycjeVM;
            ProdukcjaMGDwVM dostawaMG;
            if (dostawyMG == null)
                dostawyMG = new Dictionary<int, ProdukcjaMGDwVM>();
            
               // return false;

            //MessageBox.Show("DUPA!!");
            if (x != null)
            {
                //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
                WyborProdMGDwWindow dialog = new WyborProdMGDwWindow();
                // if (this.dostawy != null)
                // dialog.selDW = this.dostawy;
                Nullable<bool> dialogResult = dialog.ShowDialog();
                if (dialogResult == true && dialog.selDW.Count() > 0)
                {
                    //ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(i)].Pozycja
                    //wstawPozycjeDW(dialog.selDW);
                    dostawaMG = dialog.selDW.First();
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.kodmgdw = dostawaMG.ProdukcjaDW_List.kod+" ("+ dostawaMG.ProdukcjaDW_List.nr_partii+")";
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.kodtw = dostawaMG.ProdukcjaDW_List.kodtw;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.nr_partii = dostawaMG.ProdukcjaDW_List.nr_partii;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.nazwatw = dostawaMG.ProdukcjaDW_List.nazwatw;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.idmgdw = dostawaMG.ProdukcjaDW_List.id;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.khnazwa = dostawaMG.ProdukcjaDW_List.khnazwa;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.ilosc_mg = dostawaMG.ProdukcjaDW_List.iloscdost;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.frakcja = dostawaMG.ProdukcjaDW_List.frakcja;
                    DodajDoPowiazanZMagazynemProd(ProdukcjaPozycjeRW.IndexOf(x), dostawaMG);
                    RaisePropertyChanged("ProdukcjaPozycjeRW");
  
                }
            }
            else
            {
                //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
                WyborProdMGDwWindow dialog = new WyborProdMGDwWindow();
                // if (this.dostawy != null)
                // dialog.selDW = this.dostawy;
                x = new ProdukcjaPozycjeVM();
                ProdukcjaPozycjeRW.Add(x);
                
                Nullable<bool> dialogResult = dialog.ShowDialog();
                if (dialogResult == true && dialog.selDW != null)
                {
                    dostawaMG = dialog.selDW.First();
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.kodmgdw = dostawaMG.ProdukcjaDW_List.kod + " (" + dostawaMG.ProdukcjaDW_List.nr_partii + ")";
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.kodtw = dostawaMG.ProdukcjaDW_List.kodtw;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.nr_partii = dostawaMG.ProdukcjaDW_List.nr_partii;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.nazwatw = dostawaMG.ProdukcjaDW_List.nazwatw;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.idmgdw = dostawaMG.ProdukcjaDW_List.id;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.khnazwa = dostawaMG.ProdukcjaDW_List.khnazwa;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.ilosc_mg = dostawaMG.ProdukcjaDW_List.iloscdost;
                    ProdukcjaPozycjeRW[ProdukcjaPozycjeRW.IndexOf(x)].Pozycja.frakcja = dostawaMG.ProdukcjaDW_List.frakcja;
                    DodajDoPowiazanZMagazynemProd(ProdukcjaPozycjeRW.IndexOf(x), dostawaMG);
                    RaisePropertyChanged("ProdukcjaPozycjeRW");
                }
                else
                {
                    ProdukcjaPozycjeRW.Remove(x);
                    RaisePropertyChanged("ProdukcjaPozycjeRW");
                }

                   
            }
            
        }

        private void WycenaTryb()
        {
            if (!_wgWycenaTryb) // wycena po planowanej cenie
            {
                ProdukcjaInfo.Produkcja.wycena_typ = 0;
                _isPlanPrice = true;

            }
            else
            {
                ProdukcjaInfo.Produkcja.wycena_typ = 1;
                _isPlanPrice = false;
            }
        }

        private void UsunLinie()
        {
            if (ProdukcjaLinieSelectedItem != null)
            {
                ProdukcjaLinieUsuniete.Add(ProdukcjaLinieSelectedItem);
                ProdukcjaLinie.Remove(ProdukcjaLinieSelectedItem);

                RaisePropertyChanged("ProdukcjaLinie");
            }
        }

        private void DodajLinie()
        {
            ProdukcjaLiniaVM linia;
            if (LinieSelectedItem != null)
            {
                linia = new ProdukcjaLiniaVM();
                linia.LiniaPW = new PROD_LINIE_PW();
                linia.LiniaPW.id_lini = LinieSelectedItem.id;
                linia.LiniaPW.id_prod = ProdukcjaInfo.Produkcja.id;
                //linia.LiniaPW.PROD = ProdukcjaInfo.Produkcja;
                //linia.LiniaPW.PROD_LINIE = LinieSelectedItem;
                linia.LiniaPW.nazwa = LinieSelectedItem.nazwa;
                linia.LiniaPW.nazwa_procesu = LinieSelectedItem.nazwa_procesu;
                linia.getMaszyny();

                ProdukcjaLinie.Add(linia);
                RaisePropertyChanged("ProdukcjaLinie");
                //MessageBox.Show(LinieSelectedItem.nazwa.ToString());
            }

        }

        private void PrzeliczWartosci()
        {
            var conf = (from dbo in db.PRODCONF
                        where dbo.kod_firmy == this.ProdukcjaInfo.Produkcja.kod_firmy
                        where dbo.nazwa == "wskaznik"
                        select dbo).First();
            //throw new NotImplementedException();
            double? ilosc, iloscprod, cena, WartoscPU;
            WartoscPU = 0;
            float wskaznik, IloscWG, IloscPW;
            // string val_replece;
            wskaznik = 0;
            var wart_rw = (from p in this.ProdukcjaPozycjeRW
                           select (p.Pozycja.ilosc * p.Pozycja.cena)).Sum();
            WartoscRW = (float)wart_rw;
            var ilosc_wg = (from p in this.ProdukcjaPozycjePW
                            where (p.Pozycja.typ_produktu == "WG" || p.Pozycja.typ_produktu == "PP")
                            select p.Pozycja.ilosc).Sum();
            IloscPW = (float)this.ProdukcjaPozycjePW.Select(x => x.Pozycja.ilosc).Sum();

            IloscWG = (float)ilosc_wg;
            WartoscPU = ProdukcjaPozycjePW.Where(x => x.Pozycja.typ_produktu == "PU").Select(x => x.Pozycja.wartosc).Sum();
            WartoscRW = WartoscRW - (float)WartoscPU;
            if (ProdukcjaInfo.Produkcja.wycena_typ != 1)
            {
                if (SelectedPW != null && SelectedPW.Pozycja.idtw > 0)
                {

                    //ProdukcjaPozycjePW[ProdukcjaPozycjePW.IndexOf(SelectedPW)].Pozycja.wartosc
                    // ilosc = SelectedPW.Pozycja.ilosc;
                    // MessageBox.Show(SelectedPW.Pozycja.waluta);

                    if (SelectedPW.Pozycja.waluta == "PLN")
                    {
                        if (conf != null)
                        {
                            // val_replece = conf.wartosc.Replace(",", ".");
                            // MessageBox.Show(conf.wartosc.ToString());
                            //wskaznik = Convert.ToDouble(conf.wartosc.ToString());
                            wskaznik = float.Parse(conf.wartosc, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        SelectedPW.Pozycja.wskaznik = wskaznik;
                        if (ProdukcjaInfo.Produkcja.zerowa == 1)
                            SelectedPW.Pozycja.cena = SelectedPW.Pozycja.planowana_cena;
                        else
                            SelectedPW.Pozycja.cena = SelectedPW.Pozycja.planowana_cena / (1 + SelectedPW.Pozycja.wskaznik);
                        //SelectedPW.Pozycja.kurs = 1;
                        SelectedPW.Pozycja.wartosc = SelectedPW.Pozycja.ilosc * SelectedPW.Pozycja.cena;

                        //ProdukcjaPozycjePW[ProdukcjaPozycjePW.IndexOf(SelectedPW)].Pozycja.wartosc = ProdukcjaPozycjePW[ProdukcjaPozycjePW.IndexOf(SelectedPW)].Pozycja.ilosc * ProdukcjaPozycjePW[ProdukcjaPozycjePW.IndexOf(SelectedPW)].Pozycja.cena;
                    }
                    else
                    {
                        if (conf != null)
                        {
                            // val_replece = conf.wartosc.Replace(",", ".");
                            //MessageBox.Show(conf.wartosc);
                            // wskaznik = Convert.ToDouble(conf.wartosc.ToString());
                            wskaznik = float.Parse(conf.wartosc, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        SelectedPW.Pozycja.wskaznik = wskaznik;
                        if (ProdukcjaInfo.Produkcja.zerowa == 1)
                        {
                            SelectedPW.Pozycja.cena = SelectedPW.Pozycja.planowana_cena;
                            SelectedPW.Pozycja.wartosc = SelectedPW.Pozycja.ilosc * (SelectedPW.Pozycja.planowana_cena * SelectedPW.Pozycja.kurs);
                        }
                        else
                        {
                            SelectedPW.Pozycja.cena = (SelectedPW.Pozycja.planowana_cena * SelectedPW.Pozycja.kurs) / (1 + SelectedPW.Pozycja.wskaznik);

                            SelectedPW.Pozycja.wartosc = SelectedPW.Pozycja.ilosc * (SelectedPW.Pozycja.planowana_cena * SelectedPW.Pozycja.kurs) / (1 + SelectedPW.Pozycja.wskaznik);
                        }
                    }
                }
            }
            else
            {
                //Funkcja .... ta Pani biegła od Bińkowskeigo :)

                foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjePW)
                {
                    p.Pozycja.wartosc = 0;
                    p.Pozycja.surowiec_koszt = 0;
                    p.Pozycja.cena = 0;
                    RaisePropertyChanged("ProdukcjaPozycjePW");
                    if (p.Pozycja.typ_produktu == "PU")
                    {
                        var udzial_wg = (p.Pozycja.ilosc / IloscWG);
                        p.Pozycja.cena = p.Pozycja.planowana_cena;

                        p.Pozycja.wartosc = p.Pozycja.cena * p.Pozycja.ilosc;
                        if (p.Pozycja.wartosc > 0)
                        {
                            //WartoscRW = (WartoscRW - (float)p.Pozycja.wartosc);
                            p.Pozycja.surowiec_koszt = (WartoscRW - (WartoscRW - (WartoscRW * udzial_wg)));
                            WartoscPU += p.Pozycja.wartosc;
                        }
                    }
                    else
                    {
                        if (p.Pozycja.ilosc != null && p.Pozycja.ilosc > 0 && IloscWG > 0)
                        {

                            var udzial_wg = (p.Pozycja.ilosc / IloscWG);

                            //Odejmi
                            // MessageBox.Show(String.Format("Wycena według kosztów wytowrzenia ! {0}", (WartoscRW * udzial_wg)));
                            p.Pozycja.surowiec_koszt = (WartoscRW - (WartoscRW - (WartoscRW * udzial_wg)));
                            p.Pozycja.cena = (p.Pozycja.surowiec_koszt + (p.Pozycja.produkcja_koszt * p.Pozycja.ilosc)) / p.Pozycja.ilosc;
                            p.Pozycja.wartosc = (p.Pozycja.cena * p.Pozycja.ilosc);

                        }
                    }

                }
                // Policz udzial ilosci danej pozycji i ilosci ogolnej



            }
            //MessageBox.Show(ilosc.ToString());


            //MessageBox.Show("Mksymanlna dostępna ilość dla danej dostawy to: ");

        



        //MessageBox.Show(wart_rw.ToString());

        var ilosc_sr = (from p in this.ProdukcjaPozycjeRW
                        select p.Pozycja.ilosc).Sum();

        var wartosc_wg = (from p in this.ProdukcjaPozycjePW
                          select p.Pozycja.wartosc).Sum();
        wartoscWG = (float)wartosc_wg;
            WartoscWG = wartoscWG;
            ProdukcjaInfo.Produkcja.ilosc_wyrobu_got = IloscPW;
            ProdukcjaInfo.Produkcja.ilosc_surowcow = (float)ilosc_sr;
            ProdukcjaInfo.Produkcja.wart_rw = WartoscRW;
            ProdukcjaInfo.Produkcja.wart_pw = WartoscWG;

            RaisePropertyChanged("ProdukcjaPozycjePW");
            // RaisePropertyChanged("SelectedPW");
            RaisePropertyChanged("ProdukcjaInfo");
            RaisePropertyChanged("SelectedPW");


            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjePW)
            {
                poz.DopasujDoPartii();
            }
    }


        private void PrzeliczWartosciFK()
        {
            if (ProdukcjaPozycjeRW_FK != null && _produkcjaPozycjePW_FK != null)
            {
                var conf = (from dbo in db.PRODCONF
                            where dbo.kod_firmy == this.ProdukcjaInfo.Produkcja.kod_firmy
                            where dbo.nazwa == "wskaznik"
                            select dbo).First();
                //throw new NotImplementedException();
                double? ilosc, iloscprod, cena, WartoscPU;
                WartoscPU = 0;
                float wskaznik, IloscWG, IloscPW;
                // string val_replece;
                wskaznik = 0;
                var wart_rw = (from p in this.ProdukcjaPozycjeRW_FK
                               select (p.PozycjaFK.ilosc * p.PozycjaFK.cena)).Sum();
                WartoscRW = (float)wart_rw;
                var ilosc_wg = (from p in this.ProdukcjaPozycjePW_FK
                                where (p.PozycjaFK.typ_produktu == "WG" || p.PozycjaFK.typ_produktu == "PP")
                                select p.PozycjaFK.ilosc).Sum();
                IloscPW = (float)this.ProdukcjaPozycjePW_FK.Select(x => x.PozycjaFK.ilosc).Sum();

                IloscWG = (float)ilosc_wg;
                WartoscPU = ProdukcjaPozycjePW_FK.Where(x => x.PozycjaFK.typ_produktu == "PU").Select(x => x.PozycjaFK.wartosc).Sum();
                WartoscRW = WartoscRW - (float)WartoscPU;
                WartoscRW_FK = WartoscRW;
                if (ProdukcjaInfo.Produkcja.wycena_typ != 1)
                {
                    if (SelectedPW_FK != null && SelectedPW_FK.PozycjaFK.idtw > 0)
                    {

                        //ProdukcjaPozycjePW[ProdukcjaPozycjePW.IndexOf(SelectedPW)].Pozycja.wartosc
                        // ilosc = SelectedPW.Pozycja.ilosc;
                        // MessageBox.Show(SelectedPW.Pozycja.waluta);

                        if (SelectedPW_FK.PozycjaFK.waluta == "PLN")
                        {
                            if (conf != null)
                            {
                                // val_replece = conf.wartosc.Replace(",", ".");
                                // MessageBox.Show(conf.wartosc.ToString());
                                //wskaznik = Convert.ToDouble(conf.wartosc.ToString());
                                wskaznik = float.Parse(conf.wartosc, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            SelectedPW_FK.PozycjaFK.wskaznik = wskaznik;
                            if (ProdukcjaInfo.Produkcja.zerowa == 1)
                                SelectedPW_FK.PozycjaFK.cena = SelectedPW_FK.PozycjaFK.planowana_cena;
                            else
                                SelectedPW_FK.PozycjaFK.cena = SelectedPW_FK.PozycjaFK.planowana_cena / (1 + SelectedPW_FK.PozycjaFK.wskaznik);
                            //SelectedPW.Pozycja.kurs = 1;
                            SelectedPW_FK.PozycjaFK.wartosc = SelectedPW_FK.PozycjaFK.ilosc * SelectedPW_FK.PozycjaFK.cena;

                            //ProdukcjaPozycjePW[ProdukcjaPozycjePW.IndexOf(SelectedPW)].Pozycja.wartosc = ProdukcjaPozycjePW[ProdukcjaPozycjePW.IndexOf(SelectedPW)].Pozycja.ilosc * ProdukcjaPozycjePW[ProdukcjaPozycjePW.IndexOf(SelectedPW)].Pozycja.cena;
                        }
                        else
                        {
                            if (conf != null)
                            {
                                // val_replece = conf.wartosc.Replace(",", ".");
                                //MessageBox.Show(conf.wartosc);
                                // wskaznik = Convert.ToDouble(conf.wartosc.ToString());
                                wskaznik = float.Parse(conf.wartosc, System.Globalization.CultureInfo.InvariantCulture);
                            }
                            SelectedPW_FK.PozycjaFK.wskaznik = wskaznik;
                            if (ProdukcjaInfo.Produkcja.zerowa == 1)
                            {
                                SelectedPW_FK.PozycjaFK.cena = SelectedPW_FK.PozycjaFK.planowana_cena;
                                SelectedPW_FK.PozycjaFK.wartosc = SelectedPW_FK.PozycjaFK.ilosc * (SelectedPW_FK.PozycjaFK.planowana_cena * SelectedPW_FK.PozycjaFK.kurs);
                            }
                            else
                            {
                                SelectedPW_FK.PozycjaFK.cena = (SelectedPW_FK.PozycjaFK.planowana_cena * SelectedPW_FK.PozycjaFK.kurs) / (1 + SelectedPW_FK.PozycjaFK.wskaznik);

                                SelectedPW_FK.PozycjaFK.wartosc = SelectedPW_FK.PozycjaFK.ilosc * (SelectedPW_FK.PozycjaFK.planowana_cena * SelectedPW_FK.PozycjaFK.kurs) / (1 + SelectedPW_FK.PozycjaFK.wskaznik);
                            }
                        }
                    }
                }
                else
                {
                    //Funkcja .... ta Pani biegła od Bińkowskeigo :)

                    foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjePW_FK)
                    {
                        p.PozycjaFK.wartosc = 0;
                        p.PozycjaFK.surowiec_koszt = 0;
                        p.PozycjaFK.cena = 0;
                        RaisePropertyChanged("ProdukcjaPozycjePW_FK");
                        if (p.PozycjaFK.typ_produktu == "PU")
                        {
                            var udzial_wg = (p.PozycjaFK.ilosc / IloscWG);
                            p.PozycjaFK.cena = p.PozycjaFK.planowana_cena;

                            p.PozycjaFK.wartosc = p.PozycjaFK.cena * p.PozycjaFK.ilosc;
                            if (p.PozycjaFK.wartosc > 0)
                            {
                                //WartoscRW = (WartoscRW - (float)p.Pozycja.wartosc);
                                p.PozycjaFK.surowiec_koszt = (WartoscRW - (WartoscRW - (WartoscRW * udzial_wg)));
                                WartoscPU += p.PozycjaFK.wartosc;
                            }
                        }
                        else
                        {
                            if (p.PozycjaFK.ilosc != null && p.PozycjaFK.ilosc > 0 && IloscWG > 0)
                            {

                                var udzial_wg = (p.PozycjaFK.ilosc / IloscWG);

                                //Odejmi
                                // MessageBox.Show(String.Format("Wycena według kosztów wytowrzenia ! {0}", (WartoscRW * udzial_wg)));
                                p.PozycjaFK.surowiec_koszt = (WartoscRW - (WartoscRW - (WartoscRW * udzial_wg)));
                                p.PozycjaFK.cena = (p.PozycjaFK.surowiec_koszt + (p.PozycjaFK.produkcja_koszt * p.PozycjaFK.ilosc)) / p.PozycjaFK.ilosc;
                                p.PozycjaFK.wartosc = (p.PozycjaFK.cena * p.PozycjaFK.ilosc);

                            }
                        }

                    }
                    // Policz udzial ilosci danej pozycji i ilosci ogolnej



                }
                //MessageBox.Show(ilosc.ToString());


                //MessageBox.Show("Mksymanlna dostępna ilość dla danej dostawy to: ");





                //MessageBox.Show(wart_rw.ToString());

                var ilosc_sr = (from p in this.ProdukcjaPozycjeRW_FK
                                select p.PozycjaFK.ilosc).Sum();

                var wartosc_wg = (from p in this.ProdukcjaPozycjePW_FK
                                  select p.PozycjaFK.wartosc).Sum();
                WartoscWG = (float)wartosc_wg;
                //WartoscWG = wartosc_wg;
                ProdukcjaInfo.Produkcja.ilosc_wyrobu_gotowego_FK = IloscPW;
                //ProdukcjaInfo.Produkcja.ilosc_surowcow_FK = Ilosc;



                ProdukcjaInfo.Produkcja.ilosc_surowcow_FK = (float)ilosc_sr;
                ProdukcjaInfo.Produkcja.wartosc_rw_FK = Math.Round(WartoscRW_FK, 2); //zookręglenie do 2-och miejsc po przecinku
                ProdukcjaInfo.Produkcja.wartosc_pw_FK = Math.Round(WartoscWG, 2);

                RaisePropertyChanged("ProdukcjaPozycjePW_FK");
                // RaisePropertyChanged("SelectedPW");
                RaisePropertyChanged("ProdukcjaInfo");
                RaisePropertyChanged("SelectedPW_FK");


                foreach (ProdukcjaPozycjeVM poz in _produkcjaPozycjePW_FK)
                {
                    poz.DopasujDoPartiiFK();
                }
            }
    }


        private void WyliczKosztyProdukcji()
        {
            double koszt_produkcji, koszt_kg, koszty_obce;
            var ilosc_surowiec = (from p in this.ProdukcjaPozycjeRW
                                  select p.Pozycja.ilosc).Sum();
            var ilosc_wg = (from p in this.ProdukcjaPozycjePW
                            where (p.Pozycja.typ_produktu == "WG" || p.Pozycja.typ_produktu == "PP")
                            select p.Pozycja.ilosc).Sum();
            //Wyliczenie kosztow produkcji------------------------------------------------------------------------------------------------------------------------------------------------------------------------------/
            if ((koszt_produkcji = ProdukcjaInfo.WyliczKosztyProdukcji()) > 0)
            {
                
               
               koszt_kg = Convert.ToDouble(koszt_produkcji / ilosc_wg);
                

                foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjePW)
                {
                    if (p.Pozycja.typ_produktu == "WG" || p.Pozycja.typ_produktu == "PP")
                    {
                        p.Pozycja.produkcja_koszt = koszt_kg;
                    }
                }
                RaisePropertyChanged("ProdukcjaPozycjePW");
                PrzeliczWartosci();

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------/
            }
            if (ProdukcjaInfo.Produkcja.koszty_pozostale != null && ProdukcjaInfo.Produkcja.koszty_pozostale > 0)
            {
                koszt_produkcji = koszt_produkcji + (double)ProdukcjaInfo.Produkcja.koszty_pozostale;
                koszt_kg = Convert.ToDouble(koszt_produkcji / ilosc_wg);


                foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjePW)
                {
                    if (p.Pozycja.typ_produktu == "WG" || p.Pozycja.typ_produktu == "PP")
                    {
                        p.Pozycja.produkcja_koszt = koszt_kg;
                    }
                }
                RaisePropertyChanged("ProdukcjaPozycjePW");
                PrzeliczWartosci();
            }

            koszty_obce = ProdukcjaInfo.WyliczKosztyObce((double)ilosc_surowiec);
            if (koszty_obce > 0)
            {
                koszt_kg = Convert.ToDouble((koszt_produkcji + koszty_obce) / ilosc_wg);
                  foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjePW)
                {
                    if (p.Pozycja.typ_produktu == "WG" || p.Pozycja.typ_produktu == "PP")
                    {
                        p.Pozycja.produkcja_koszt = koszt_kg;
                    }
                }
                RaisePropertyChanged("ProdukcjaPozycjePW");
                PrzeliczWartosci();
            }
           
            MessageBox.Show("Koszty pracy maszyn dla zlecenia to: " + koszt_produkcji.ToString());
        }



        /*
         * Oblicza kost produkcji dla roszliczenia finansowo ksiegowego
         */
        private void WyliczKosztyProdukcjiFK()
        {
            double koszt_produkcji, koszt_kg, koszty_obce;
            var ilosc_surowiec = (from p in this.ProdukcjaPozycjeRW_FK
                                  select p.PozycjaFK.ilosc).Sum();
            var ilosc_wg = (from p in this.ProdukcjaPozycjePW_FK
                            where (p.PozycjaFK.typ_produktu == "WG" || p.PozycjaFK.typ_produktu == "PP")
                            select p.PozycjaFK.ilosc).Sum();
            //Wyliczenie kosztow produkcji------------------------------------------------------------------------------------------------------------------------------------------------------------------------------/
            if ((koszt_produkcji = ProdukcjaInfo.WyliczKosztyProdukcji()) > 0)
            {


                koszt_kg = Convert.ToDouble(koszt_produkcji / ilosc_wg);


                foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjePW_FK)
                {
                    if (p.PozycjaFK.typ_produktu == "WG" || p.PozycjaFK.typ_produktu == "PP")
                    {
                        p.PozycjaFK.produkcja_koszt = koszt_kg;
                    }
                }
                RaisePropertyChanged("ProdukcjaPozycjePW_FK");
                PrzeliczWartosciFK();

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------/
            }
            if (ProdukcjaInfo.Produkcja.koszty_pozostale != null && ProdukcjaInfo.Produkcja.koszty_pozostale > 0)
            {
                koszt_produkcji = koszt_produkcji + (double)ProdukcjaInfo.Produkcja.koszty_pozostale;
                koszt_kg = Convert.ToDouble(koszt_produkcji / ilosc_wg);


                foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjePW_FK)
                {
                    if (p.PozycjaFK.typ_produktu == "WG" || p.PozycjaFK.typ_produktu == "PP")
                    {
                        p.PozycjaFK.produkcja_koszt = koszt_kg;
                    }
                }
                RaisePropertyChanged("ProdukcjaPozycjePW_FK");
                PrzeliczWartosciFK();
            }

            koszty_obce = ProdukcjaInfo.WyliczKosztyObce((double)ilosc_surowiec);
            if (koszty_obce > 0)
            {
                koszt_kg = Convert.ToDouble((koszt_produkcji + koszty_obce) / ilosc_wg);
                foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjePW_FK)
                {
                    if (p.PozycjaFK.typ_produktu == "WG" || p.PozycjaFK.typ_produktu == "PP")
                    {
                        p.PozycjaFK.produkcja_koszt = koszt_kg;
                    }
                }
                RaisePropertyChanged("ProdukcjaPozycjePW_FK");
                PrzeliczWartosciFK();
            }

            MessageBox.Show("Koszty pracy maszyn dla zlecenia to: " + koszt_produkcji.ToString());
        }

        public double SumujRezerwacjeDostawy(int iddw)
        {
            var statusy = new[] { "0", "1" };
            var suma = (from d in db.PROD_PW
                        from d2 in db.PROD
                        where d.id_prod == d2.id
                        where d2.exp_hm != 1
                       // where statusy.Contains(d2.status.ToString())
                        where d.id_proddw == iddw
                        



                        select d.ilosc).Sum();
            if (suma == null)
                return 0;
            else
                return (double)suma;

        }

       
        /*
         * Sumuje ilici jakie są juz zarezerwowane z danej dostawy
         * 
         * */
        public double SumujRezerwacjeDostawyProdMG(int iddw)
        {
            using (FZLEntities1 db2 = new FZLEntities1())

            {
                var typydk = new[] { "RWS", "RWO", "WZWG"};

                var suma = (from d in db2.PROD_PRODMGPW

                            where d.id_prodmgdw == iddw
                            where !(d.id_dkprodmg > 0 ) 
                            //|| (d.id_dkprodmg > 0 && typydk.Any(t => d.PROD_MZ.PROD_MG.typ_dk.Contains(t)) == false) && (d.id_proddp>0 && d.PRODDP.rodzaj_dk == "RW"))
                            select d.ilosc).ToList();
                if (suma.Count() == 0)
                    return 0;
                else
                    return (double)suma.Sum();

               
            }

        }


        private void SprawdzStany()
        {
            double? ilosc, iloscprod, iloscdost;
            ilosc = SelectedRW.Pozycja.ilosc;
            if (SelectedRW.Pozycja.idproddw != null)
            {
                var lbs = (from g in db.PROD_HMDW where g.id == SelectedRW.Pozycja.idproddw select g).FirstOrDefault();
                iloscprod = SumujRezerwacjeDostawy(lbs.id);
                if (SelectedRW.Pozycja.id > 0)
                {
                    using (FZLEntities1 db2 = new FZLEntities1())
                    {

                        var lbs2 = (from p in db2.PRODDP where p.id == SelectedRW.Pozycja.id select p).FirstOrDefault();
                        if (lbs2.id > 0)
                        {
                            //MessageBox.Show(lbs2.ilosc.ToString());
                            iloscprod = iloscprod - lbs2.ilosc;

                            iloscdost = (lbs.iloscdosp - iloscprod) + lbs2.ilosc;

                        }
                    }
                }
                iloscdost = lbs.iloscdosp - iloscprod;


                if (ilosc > iloscdost)
                {
                    MessageBox.Show("Mksymanlna dostępna ilość dla danej dostawy to: " + iloscdost.ToString());
                    SelectedRW.Pozycja.ilosc = iloscdost;
                }
            }
            SprawdzStanyMgProd();

            PrzeliczWartosci();
            // throw new NotImplementedException();
        }


        //Sprawdza stany przy rozliczeniu finansowym produkcji
        private void SprawdzStanyFK()
        {
            double? ilosc, iloscprod, iloscdost;
            ilosc = SelectedRW_FK.PozycjaFK.ilosc;
            if (SelectedRW_FK.PozycjaFK.idproddw != null)
            {
                var lbs = (from g in db.PROD_HMDW where g.id == SelectedRW_FK.PozycjaFK.idproddw select g).FirstOrDefault();
                iloscprod = SumujRezerwacjeDostawy(lbs.id);
                if (SelectedRW_FK.PozycjaFK.id > 0)
                {
                    using (FZLEntities1 db2 = new FZLEntities1())
                    {
                        var lbs2 = (from p in db2.PRODDP_FK where p.id == SelectedRW_FK.PozycjaFK.id select p).FirstOrDefault();
                        if (lbs2.id > 0)
                        {
                            //MessageBox.Show(lbs2.ilosc.ToString());
                            iloscprod = iloscprod - lbs2.ilosc;

                            //iloscdost = (lbs.iloscdosp - iloscprod) + lbs2.ilosc;
                        }
                    }
                }
                iloscdost = lbs.iloscdosp - iloscprod;


                if (ilosc > iloscdost)
                {
                    MessageBox.Show("Mksymanlna dostępna ilość dla danej dostawy to: " + iloscdost.ToString());
                    SelectedRW_FK.PozycjaFK.ilosc = iloscdost;
                }
            }
            //SprawdzStanyMgProd();

            PrzeliczWartosciFK();
            // throw new NotImplementedException();
        }

        public PROD_MZ SprawdzCzyJestDokumentMGPROD(PRODDP dp)
        {
            var query = (from c in db.PROD_PRODMGPW
                       where c.id_proddp == dp.id
                       where c.id_dkprodmg != null
                       select c).FirstOrDefault();
            if (query != null)
                return query.PROD_MZ;
            else
                return null;
        }

        /**Sprawdza dostepne stany w magazynie produkcyjnym 
         * 
         */
        private void SprawdzStanyMgProd()
        {

            double? ilosc, iloscprod, iloscdost;
            PROD_MZ dk_rozliczenie;

            ilosc = SelectedRW.Pozycja.ilosc_mg;
            if (SelectedRW.Pozycja.idmgdw != null)
            {
                dk_rozliczenie = SprawdzCzyJestDokumentMGPROD(SelectedRW.Pozycja);
                if (dk_rozliczenie != null)
                {
                    MessageBox.Show("Modyfikujesz już rozliczoną ilość w dokumencie: "+dk_rozliczenie.PROD_MG.kod);
                   // SelectedRW.Pozycja.ilosc_mg = dk_rozliczenie.ilosc;
                }
                else
                {
                    var lbs = (from g in db.PROD_MGDW where g.id == SelectedRW.Pozycja.idmgdw select g).FirstOrDefault();
                    iloscprod = SumujRezerwacjeDostawyProdMG(lbs.id);
                    if (SelectedRW.Pozycja.id > 0)
                    {
                        using (FZLEntities1 db2 = new FZLEntities1())
                        {
                            var lbs2 = (from p in db2.PRODDP where p.id == SelectedRW.Pozycja.id select p).FirstOrDefault();
                            if (lbs2.id > 0)
                            {
                                //MessageBox.Show(lbs2.ilosc_mg.ToString());
                                if (lbs2.idmgdw == SelectedRW.Pozycja.idmgdw)
                                {
                                    if (lbs2.ilosc_mg > iloscprod)
                                    {
                                        iloscprod = iloscprod + lbs2.ilosc_mg;
                                    }
                                    else
                                    {
                                        iloscprod = iloscprod - lbs2.ilosc_mg;
                                    }
                                    iloscdost = (lbs.iloscdost - iloscprod);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                    iloscdost = lbs.iloscdost;

                    if (ilosc > iloscdost)
                    {
                        MessageBox.Show("Mksymanlna dostępna ilość dla danej dostawy to: " + iloscdost.ToString());
                        SelectedRW.Pozycja.ilosc_mg = iloscdost;
                    }
                }
            }
            //PrzeliczWartosci();
            // throw new NotImplementedException();
        }

        /**Sprawdza dostepne stany w magazynie produkcyjnym 
         * 
         */

        private double SprawdzStanOpakowania(int id_opakowania)
        {
          var  lbs = (from g in db.PROD_MGDW
                   where g.idtw == id_opakowania
                   where g.kod_firmy == ProdukcjaInfo.Produkcja.kod_firmy
                   where g.iloscdost > 0
                   select g).OrderBy(x => x.data).ToList();
            if (lbs.Count > 0)
                return (double)lbs.Select(x => x.iloscdost).Sum();
            return 0;
        }

        private double SprawdzIiloscZarezerwowana(int id_monit, int id_tw)
        {
            using (FZLEntities1 db2 = new FZLEntities1())
            {
                var query = (from q in db2.PROD_PRODMGPW
                             where q.PROD_MGDW.idtw == id_tw
                             where q.id_proddpmonit == id_monit
                             select q).ToList();
                if (query.Count() > 0)
                    return query.Sum(x => x.ilosc);

                return 0;
            }
        }

        private void SprawdzStanyWGMonit()
        {
            /*
            ProdukcjaPozycjeMonitVM monit = SelectedMONIT;
            double? ilosc, iloscprod, iloscdost, iloscrez, iloscprodtmp;
            List<PROD_MGDW> lbs = new List<PROD_MGDW>();
            //  foreach (ProdukcjaPozycjeMonitVM monit in ProdukcjaPozycjeMonit)
            // {



            if (monit.ProdDPMonit.id_opakowania != null && monit.ProdDPMonit.ilosc_opakowania != null)
            {
                //Sprawdz czy istenieja juz rezeracje dla tego monitu powiazane z dokumentem magazynowym
                var opakpw_mg = (from p in db.PROD_PRODMGPW
                                 where p.id_proddpmonit == monit.ProdDPMonit.id
                                 where p.id_dkprodmg != null
                                 select p).ToList();
                if (opakpw_mg != null && opakpw_mg.Count() > 0)
                {
                    MessageBox.Show("Nie można zmienic tej pozycji, ponieważ posada ona powiazane dokumeny magazynowe!");
                    monit.ProdDPMonit.ilosc_opakowania = SprawdzIiloscZarezerwowana(monit.ProdDPMonit.id, (int)monit.ProdDPMonit.id_opakowania);
                }
                else
                {

                    ilosc = monit.ProdDPMonit.ilosc_opakowania;
                    iloscprod = 0;
                    iloscdost = SprawdzStanOpakowania((int)monit.ProdDPMonit.id_opakowania);
                    // MessageBox.Show(iloscdost.ToString());
                    iloscrez = 0;
                    lbs = (from g in db.PROD_MGDW
                           where g.idtw == monit.ProdDPMonit.id_opakowania
                           where g.kod_firmy == ProdukcjaInfo.Produkcja.kod_firmy
                           where g.iloscdost > 0
                           select g).OrderBy(x => x.data).ToList();


                    //Odejmij wszsytkie, które są w innych zleceniach

                    foreach (PROD_MGDW dw in lbs)
                    {



                        iloscprod += SumujRezerwacjeDostawyProdMG(dw.id);

                        //iloscdost += dw.iloscdost;
                       /* foreach (PROD_PRODMGPW pw in OpakowaniaPowiazania)
                        {
                            if (monit.ProdDPMonit.id == pw.id_proddpmonit && pw.id_prodmgdw == dw.id && pw.id > 0)
                            {
                                //iloscrez += pw.ilosc;
                                //pw.ilosc = (double)ilosc;

                            }
                        }
                        */
                        /*
                        foreach (PROD_PRODMGPW pw in OpakowaniaPowiazania)
                        {
                            //var dw_tmp = db.PROD_MGDW.Where(x => x.id == pw.id_prodmgdw).FirstOrDefault();
                            if (pw.id_prodmgdw == dw.id && (pw.id == null || pw.id==0) && monit.ProdDPMonit.id != pw.id_proddpmonit )
                            {
                                iloscrez += pw.ilosc;
                            }
                           
                        }
                        

                    }



                    iloscdost = (iloscdost - iloscprod) + SprawdzIiloscZarezerwowana(monit.ProdDPMonit.id, (int)monit.ProdDPMonit.id_opakowania);

                    //ilosci aktualnie rezewowane


                    if (ilosc > iloscdost && monit != null)
                    {
                        var ilosc_tmp = iloscdost;
                        // iloscdost += SprawdzIiloscZarezerwowana(monit.ProdDPMonit.id, (int)monit.ProdDPMonit.id_opakowania);
                        MessageBox.Show("Mksymanlna dostępna ilość dla danej dostawy to: " + ilosc_tmp.ToString());
                        monit.ProdDPMonit.ilosc_opakowania = ilosc_tmp;
                        SelectedMONIT = monit;
                        RaisePropertyChanged("ProdukcjaPozycjeMonitTMP");

                    }
                    else if (monit != null && (monit.ProdDPMonit.ilosc_opakowania <= iloscdost))
                    {
                        RezerwujOpakowanie(monit);
                    }
                    /*
                     else
                     {
                         //Obsługa FIFO
                         double ilosc_tmp = (double)monit.ProdDPMonit.ilosc_opakowania;
                         double ilosc_potrzebna = (double)monit.ProdDPMonit.ilosc_opakowania; ;
                         foreach (PROD_MGDW dw in lbs)
                         {
                             if (dw.iloscprod == null)
                             {
                                 dw.iloscprod = 0;
                             }
                             if (ilosc_potrzebna > 0)
                             {
                                 if ((dw.iloscdost - dw.iloscprod) >=  ilosc_potrzebna)
                                 {
                                     //ilosc_potrzebna = ilosc_potrzebna - 
                                     DodajDoOpakowaniaPowiazania(monit, dw, ilosc_potrzebna);
                                     break;
                                 }
                                 else
                                 {
                                     ilosc_potrzebna = ilosc_potrzebna - (double)(dw.iloscdost - dw.iloscprod);
                                     DodajDoOpakowaniaPowiazania(monit, dw, (double)(dw.iloscdost - dw.iloscprod));

                                 }
                             }
                         }

                     }
                    
                }
            }
            //}
           
            //PrzeliczWartosci();
            */
            //throw new NotImplementedException();
        }

        protected void UsunRezerwacjeOpakowania(int id_monit, int id_dw)
        {
           // using (FZLEntities1 db2 = new FZLEntities1())
          //  {
                var query = db.PROD_PRODMGPW.Where(x => x.id_proddpmonit == id_monit && x.id_prodmgdw == id_dw).ToList();
                var mgdw = db.PROD_MGDW.Where(x => x.id == id_dw).First();
                List<int> ids = new List<int>();

                foreach (PROD_PRODMGPW pw in query)
                {
                    foreach (PROD_PRODMGPW pw2 in OpakowaniaPowiazania)
                    {
                        if (pw.id_proddpmonit == pw2.id_proddpmonit && pw.id_prodmgdw == pw2.id_prodmgdw)
                            ids.Add(OpakowaniaPowiazania.IndexOf(pw2));
                    }

                    db.PROD_PRODMGPW.Remove(pw);

                    db.SaveChanges();
                }
                for (int i = 0; i < ids.Count; i++)
                {
                    OpakowaniaPowiazania.RemoveAt(ids[i]);
                }
                mgdw.iloscprod = SumujRezerwacjeDostawyProdMG(id_dw);
          //  }
        }

        protected void RezerwujOpakowanie(ProdukcjaPozycjeMonitVM monit)
        {
            List<PROD_MGDW> lbs = new List<PROD_MGDW>();

            List<int> ids_pws = new List<int>(); //lista identyfiaktorow dostaw uzytych do zarezerwowania opakowania
            lbs = (from g in db.PROD_MGDW
                       where g.idtw == monit.ProdDPMonit.id_opakowania
                       where g.iloscdost > 0
                       where g.kod_firmy == ProdukcjaInfo.Produkcja.kod_firmy
                       select g).OrderBy(x => x.data).ToList();
            //Obsługa FIFO
            double ilosc_tmp = (double)monit.ProdDPMonit.ilosc_opakowania;
            double ilosc_potrzebna = (double)monit.ProdDPMonit.ilosc_opakowania;
            double ilosc_dostepna, suma_rezerwacji;
            
            

            foreach (PROD_MGDW dw in lbs)
            {
                if (dw.iloscprod == null)
                {
                    dw.iloscprod = 0;
                }
                

                if (ilosc_potrzebna > 0)
                {
                    suma_rezerwacji = SumujRezerwacjeDostawyProdMG(dw.id)-SprawdzIiloscZarezerwowana(monit.ProdDPMonit.id, (int)monit.ProdDPMonit.id_opakowania);
                    //odejmij od sumy rezerwacji stara rezerwacje

                    if ((dw.iloscdost - suma_rezerwacji) >= ilosc_potrzebna)
                    {
                        //ilosc_potrzebna = ilosc_potrzebna - 
                        DodajDoOpakowaniaPowiazania(monit, dw, ilosc_potrzebna);
                        ids_pws.Add(dw.id);
                        ilosc_potrzebna = 0;
                        break;
                    }
                    else
                    {
                        ilosc_dostepna = (double)(dw.iloscdost - suma_rezerwacji);
                        DodajDoOpakowaniaPowiazania(monit, dw, ilosc_dostepna);
                        ids_pws.Add(dw.id);
                        ilosc_potrzebna = ilosc_potrzebna - ilosc_dostepna;
                    }
                }
                
            }
            using (FZLEntities1 db2 = new FZLEntities1())
            {
                //pobierz rezerwacje zapisane w bazie danych
                var rezerwacje_db = (from r in db2.PROD_PRODMGPW
                                     where r.id_proddpmonit == monit.ProdDPMonit.id
                                     select r).ToList();
                //usun aktualne rezerwacje tego towaru dla tego monitu
                bool jest = false;
                foreach (PROD_PRODMGPW pw in rezerwacje_db)
                {
                    jest = false;
                    foreach (int i in ids_pws)
                    {
                        if (pw.id_prodmgdw == i && pw.id_proddpmonit == monit.ProdDPMonit.id)
                        {

                            // db.PROD_PRODMGPW.Remove(pw);
                            //db.SaveChanges();
                            jest = true;
                        }
                    }
                    if (!jest)//usun nieuzywana rezeracje
                    {
                        UsunRezerwacjeOpakowania(monit.ProdDPMonit.id, (int)pw.id_prodmgdw);
                    }
                }
            }

        }

        public void PokazDW()
        {
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborDwWindow dialog = new WyborDwWindow();
            if (this.dostawy != null)
                dialog.selDW = this.dostawy;
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {

                wstawPozycjeDW(dialog.selDW);
            }

            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

        public void PokazDWMG()
        {
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborProdMGDwWindow dialog = new WyborProdMGDwWindow();
            
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {

                wstawPozycjeDWPRODMG(dialog.selDW);
            }

            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

        public void PokazDWFK()
        {
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborDwWindow dialog = new WyborDwWindow();
            if (this.dostawy != null)
                dialog.selDW = this.dostawy;
            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {

                wstawPozycjeDW_FK(dialog.selDW);
            }

            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

        public void WstawSYM()
        {
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborTwWindow dialog = new WyborTwWindow();

            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                wstawPozycjeSYM(dialog.selTW);
            }
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

        public void PokazTW_FK()
        {
            //MessageBox.Show(string.Format("The Population you double clicked on has this ID - {0}, Name - {1}, and Description {2}",selectedPopulation.id, selectedPopulation.nazwa, selectedPopulation.miejscowosc));
            WyborTwWindow dialog = new WyborTwWindow();

            Nullable<bool> dialogResult = dialog.ShowDialog();
            if (dialogResult == true)
            {
                wstawPozycjeTW_FK(dialog.selTW);
            }
            //System.Windows.MessageBox.Show(WybraneDostawy.Count().ToString());
        }

        public DataSet PrzygotujDaneDoRaportu()
        {
            //FZLDataSetTableAdapters.PRODTableAdapter prodta = new FZLDataSetTableAdapters.PRODTableAdapter();
            var opakowania_rodzaje = db.OPAKOWANIA_RODZAJE.ToList().ToDataTable();
            var kody_gs1 = db.GS1_KODY.Where(i => i.kod_firmy == ProdukcjaInfo.Produkcja.kod_firmy).ToDataTable();
            var prod = db.PROD.Where(i => i.id == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var proddp = db.PRODDP.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var proddp_fk = db.PRODDP_FK.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var proddp_sym = db.PRODDP_SYM.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prodpw = db.PROD_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var proddwids = (from c in db.PROD_PW
                             where c.id_prod == ProdukcjaInfo.Produkcja.id
                             select c.id_proddw).ToArray();
            var prodmgdwids = (from c in db.PRODDP
                             where c.id_prod == ProdukcjaInfo.Produkcja.id
                             select c.idmgdw).ToArray();
            var proddw = (from c in db.PROD_HMDW
                          where proddwids.Contains(c.id)
                          select c).ToDataTable();
            var prod_linie_pw = db.PROD_LINIE_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prod_linie = db.PROD_LINIE.ToDataTable();
            var prod_maszyny = db.PROD_MASZYNY.ToDataTable();
            var prod_maszyny_pw = db.PROD_MASZYNY_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prod_maszyny_param_wart = db.PROD_MASZYNY_PARAM_WART.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prod_maszyny_param = db.PROD_MASZYNY_PARAM.ToDataTable();
            var proddp_monit = db.PRODDP_MONIT.Where(i => i.PRODDP.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prod_maszyny_monit      =   db.PROD_MASZYNY_MONIT.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prod_maszyny_pwids = (from c in db.PROD_MASZYNY_PW
                                      where c.id_prod == ProdukcjaInfo.Produkcja.id
                                      select c.id).ToArray();
            var prod_maszyny_koszty = (from c in db.PROD_MASZYNY_KOSZTY
                                       where prod_maszyny_pwids.Contains(c.id_prod_maszyny_pw)
                                       select c).ToDataTable();

            var prodmgdw = (from c in db.PROD_MGDW
                          where prodmgdwids.Contains(c.id)
                          select c).ToDataTable();

            //db.PROD_MASZYNY_KOSZTY.Where(i => i.)
            var proddk = (from c in db.PRODDK
                          where
                          c.id_prod == ProdukcjaInfo.Produkcja.id
                          
                          select c).ToDataTable();
            var hmids = (from c in db.PRODDK
                         where
                         c.id_prod == ProdukcjaInfo.Produkcja.id
                         select c.id_dk).ToArray();
            foreach (int i in hmids)
            {
                //MessageBox.Show(i.ToString());
            }

            var hmmg = (from c in db.HMMG
                        where hmids.Contains((int)c.hmid)
                        where c.kod_firmy == ProdukcjaInfo.Produkcja.kod_firmy
                        select c).ToDataTable();

            var hmmz = (from c in db.HMMZ
                        where hmids.Contains((int)c.super)
                        where c.kod_firmy == ProdukcjaInfo.Produkcja.kod_firmy
                        select c).ToDataTable();

            var magazyny = (from c in db.MAGAZYNY
                            select c).ToDataTable();

            DataSet ds = new DataSet();
        
            kody_gs1.TableName = "GS1_KODY";
            ds.Tables.Add(kody_gs1);
            prod.TableName = "PROD";
            ds.Tables.Add(prod);
            proddp.TableName = "PRODDP";
            ds.Tables.Add(proddp);
            proddp_fk.TableName = "PRODDP_FK";
            ds.Tables.Add(proddp_fk);
            proddp_sym.TableName = "PRODDP_SYM";
            ds.Tables.Add(proddp_sym);
            proddp_monit.TableName = "PRODDP_MONIT";
            ds.Tables.Add(proddp_monit);
            prodpw.TableName = "PROD_PW";
            ds.Tables.Add(prodpw);
            proddw.TableName = "PROD_HMDW";
            ds.Tables.Add(proddw);
            prodmgdw.TableName = "PROD_MGDW";
            ds.Tables.Add(prodmgdw);
            prod_linie.TableName = "PROD_LINIE";
            ds.Tables.Add(prod_linie);
            prod_linie_pw.TableName = "PROD_LINIE_PW";
            ds.Tables.Add(prod_linie_pw);
            prod_maszyny.TableName = "PROD_MASZYNY";
            ds.Tables.Add(prod_maszyny);
            prod_maszyny_pw.TableName = "PROD_MASZYNY_PW";
            ds.Tables.Add(prod_maszyny_pw);
            prod_maszyny_koszty.TableName = "PROD_MASZYNY_KOSZTY";
            ds.Tables.Add(prod_maszyny_koszty);
            prod_maszyny_param_wart.TableName = "PROD_MASZYNY_PARAM_WART";
            ds.Tables.Add(prod_maszyny_param_wart);
            prod_maszyny_param.TableName = "PROD_MASZYNY_PARAM";
            ds.Tables.Add(prod_maszyny_param);
            prod_maszyny_monit.TableName = "PROD_MASZYNY_MONIT";
            ds.Tables.Add(prod_maszyny_monit);
            proddk.TableName = "PRODDK";
            ds.Tables.Add(proddk);
            hmmg.TableName = "HMMG";
            ds.Tables.Add(hmmg);

            hmmz.TableName = "HMMZ";
            ds.Tables.Add(hmmz);

            opakowania_rodzaje.TableName = "OPAKOWANIA_RODZAJE";
            ds.Tables.Add(opakowania_rodzaje);

            magazyny.TableName = "MAGAZYNY";
            ds.Tables.Add(magazyny);




            return ds;
        }

        public void DrukujEtykieteGS1()
        {
            string url1 = @"Raporty\EtykietaGS1.frx";
            // MessageBox.Show(url1);
            ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
            //ReportWindow dialog = new ReportWindow();
            // report1.RegisterData
            //dialog.Report2View.Report.RegisterData(ds, "fZLDataSet1");
            dialog.ShowDialog();
        }



        public void Raport()
        {
            
            //ds.PROD = Functions.ToDataTable<PROD>(ProdukcjaInfo.Produkcja.);
            //string path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)+ "\\Raporty\\test.frx";
            // string path = (Directory.GetCurrentDirectory() + "\Raporty\test.frx");
            string url1 = @"Raporty\RaportPrzerobu.frx";
           // MessageBox.Show(url1);
            ReportWindow dialog = new ReportWindow(url1,PrzygotujDaneDoRaportu());
            
            //ReportWindow dialog = new ReportWindow();
           // report1.RegisterData
            //dialog.Report2View.Report.RegisterData(ds, "fZLDataSet1");
            dialog.ShowDialog();
        }
        public void ShowRaportMG()
        {
            RaportMG();
        }

        public void RaportMG(bool Show = true, bool ExportToFile = false)
        {

            //ds.PROD = Functions.ToDataTable<PROD>(ProdukcjaInfo.Produkcja.);
            //string path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)+ "\\Raporty\\test.frx";
            // string path = (Directory.GetCurrentDirectory() + "\Raporty\test.frx");
            string url1 = @"Raporty\RaportPrzerobuMG.frx";
            // MessageBox.Show(url1);
            ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
            if (ExportToFile == true)
            {
                
                dialog.Report2View.Prepare();
                PDFExport pdf = new PDFExport();
                dialog.Report2View.Export(pdf, Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+"\\RaportMG.pdf");
            }
            //ReportWindow dialog = new ReportWindow();
            // report1.RegisterData
            //dialog.Report2View.Report.RegisterData(ds, "fZLDataSet1");
            if (Show)
            {
                dialog.ShowDialog();
            }
        }

        public void RaportFK()
        {

            //ds.PROD = Functions.ToDataTable<PROD>(ProdukcjaInfo.Produkcja.);
            //string path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)+ "\\Raporty\\test.frx";
            // string path = (Directory.GetCurrentDirectory() + "\Raporty\test.frx");
            string url1 = @"Raporty\RaportPrzerobuFKv2.frx";
            // MessageBox.Show(url1);
            ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
            //ReportWindow dialog = new ReportWindow();
            // report1.RegisterData
            //dialog.Report2View.Report.RegisterData(ds, "fZLDataSet1");
            dialog.ShowDialog();
        }
        public void Zlecenie()
        { /*
            //FZLDataSetTableAdapters.PRODTableAdapter prodta = new FZLDataSetTableAdapters.PRODTableAdapter();
            var prod = db.PROD.Where(i => i.id == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var proddp = db.PRODDP.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prodpw = db.PROD_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var proddw = db.PROD_HMDW.ToDataTable();
            var prod_linie_pw = db.PROD_LINIE_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prod_linie = db.PROD_LINIE.ToDataTable();
            var prod_maszyny = db.PROD_MASZYNY.ToDataTable();
            var prod_maszyny_pw = db.PROD_MASZYNY_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prod_maszyny_param_wart = db.PROD_MASZYNY_PARAM_WART.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
            var prod_maszyny_param = db.PROD_MASZYNY_PARAM.ToDataTable();
            var prod_maszyny_monit = db.PROD_MASZYNY_MONIT.Where(i=>i.id_prod==ProdukcjaInfo.Produkcja.id).ToDataTable();
            DataSet ds = new DataSet();
            prod.TableName = "PROD";
            ds.Tables.Add(prod);
            proddp.TableName = "PRODDP";
            ds.Tables.Add(proddp);
            prodpw.TableName = "PROD_PW";
            ds.Tables.Add(prodpw);
            proddw.TableName = "PROD_HMDW";
            ds.Tables.Add(proddw);
            prod_linie.TableName = "PROD_LINIE";
            ds.Tables.Add(prod_linie);
            prod_linie_pw.TableName = "PROD_LINIE_PW";
            ds.Tables.Add(prod_linie_pw);
            prod_maszyny.TableName = "PROD_MASZYNY";
            ds.Tables.Add(prod_maszyny);
            prod_maszyny_pw.TableName = "PROD_MASZYNY_PW";
            ds.Tables.Add(prod_maszyny_pw);
            prod_maszyny_param_wart.TableName = "PROD_MASZYNY_PARAM_WART";
            ds.Tables.Add(prod_maszyny_param_wart);
            prod_maszyny_param.TableName = "PROD_MASZYNY_PARAM";
            ds.Tables.Add(prod_maszyny_param);

            prod_maszyny_monit.TableName = "PROD_MASZYNY_MONIT";
            ds.Tables.Add(prod_maszyny_monit);
            */
            //ds.PROD = Functions.ToDataTable<PROD>(ProdukcjaInfo.Produkcja.);
            //string path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)+ "\\Raporty\\test.frx";
            // string path = (Directory.GetCurrentDirectory() + "\Raporty\test.frx");
            string url1 = @"Raporty\ZleceniePrzerobu.frx";
            // MessageBox.Show(url1);
            ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
            //ReportWindow dialog = new ReportWindow();
            // report1.RegisterData
            //dialog.Report2View.Report.RegisterData(ds, "fZLDataSet1");
            dialog.ShowDialog();
        }


        public void ZlecenieSymulacja()
        { 
            string url1 = @"Raporty\ZlecenieSymulacjaPrzerobu.frx";
            // MessageBox.Show(url1);
            ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
            //ReportWindow dialog = new ReportWindow();
            // report1.RegisterData
            //dialog.Report2View.Report.RegisterData(ds, "fZLDataSet1");
            dialog.ShowDialog();
        }

        public void RaportSymulacja()
        {
            string url1 = @"Raporty\RaportSymulacjaPrzerobuMG.frx";
            // MessageBox.Show(url1);
            ReportWindow dialog = new ReportWindow(url1, PrzygotujDaneDoRaportu());
            //ReportWindow dialog = new ReportWindow();
            // report1.RegisterData
            //dialog.Report2View.Report.RegisterData(ds, "fZLDataSet1");
            dialog.ShowDialog();
        }
        public void SpecyfikacjaPakowania()
        {
            if (SelectedPW != null && SelectedPW.Pozycja != null && SelectedPW.Pozycja.id > 0)
            {
                //FZLDataSetTableAdapters.PRODTableAdapter prodta = new FZLDataSetTableAdapters.PRODTableAdapter();
                var prod = db.PROD.Where(i => i.id == ProdukcjaInfo.Produkcja.id).ToDataTable();
                var proddp = db.PRODDP.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
                var prodpw = db.PROD_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
                var proddw = db.PROD_HMDW.ToDataTable();
                var prod_linie_pw = db.PROD_LINIE_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
                var prod_linie = db.PROD_LINIE.ToDataTable();
                var prod_maszyny = db.PROD_MASZYNY.ToDataTable();
                var prod_maszyny_pw = db.PROD_MASZYNY_PW.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
                var prod_maszyny_param_wart = db.PROD_MASZYNY_PARAM_WART.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
                var prod_maszyny_param = db.PROD_MASZYNY_PARAM.ToDataTable();
                var proddp_monit = db.PRODDP_MONIT.Where(i => i.id_proddp == SelectedPW.Pozycja.id).ToDataTable();
                var prod_maszyny_monit = db.PROD_MASZYNY_MONIT.Where(i => i.id_prod == ProdukcjaInfo.Produkcja.id).ToDataTable();
                var magazyny = db.MAGAZYNY.ToDataTable();
               
                
                DataSet ds = new DataSet();
                magazyny.TableName = "MAGAZYNY";
                ds.Tables.Add(magazyny);
                prod.TableName = "PROD";
                ds.Tables.Add(prod);
                proddp.TableName = "PRODDP";
                ds.Tables.Add(proddp);
                proddp_monit.TableName = "PRODDP_MONIT";
                ds.Tables.Add(proddp_monit);
                prodpw.TableName = "PROD_PW";
                ds.Tables.Add(prodpw);
                proddw.TableName = "PROD_HMDW";
                ds.Tables.Add(proddw);
                prod_linie.TableName = "PROD_LINIE";
                ds.Tables.Add(prod_linie);
                prod_linie_pw.TableName = "PROD_LINIE_PW";
                ds.Tables.Add(prod_linie_pw);
                prod_maszyny.TableName = "PROD_MASZYNY";
                ds.Tables.Add(prod_maszyny);
                prod_maszyny_pw.TableName = "PROD_MASZYNY_PW";
                ds.Tables.Add(prod_maszyny_pw);
                prod_maszyny_param_wart.TableName = "PROD_MASZYNY_PARAM_WART";
                ds.Tables.Add(prod_maszyny_param_wart);
                prod_maszyny_param.TableName = "PROD_MASZYNY_PARAM";
                ds.Tables.Add(prod_maszyny_param);
                prod_maszyny_monit.TableName = "PROD_MASZYNY_MONIT";
                ds.Tables.Add(prod_maszyny_monit);
                //ds.PROD = Functions.ToDataTable<PROD>(ProdukcjaInfo.Produkcja.);
                //string path = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)+ "\\Raporty\\test.frx";
                // string path = (Directory.GetCurrentDirectory() + "\Raporty\test.frx");
                string url1 = @"Raporty\SpecyfikacjaPakowania.frx";
                // MessageBox.Show(url1);
                ReportWindow dialog = new ReportWindow(url1, ds);
                //ReportWindow dialog = new ReportWindow();
                // report1.RegisterData
                //dialog.Report2View.Report.RegisterData(ds, "fZLDataSet1");
                dialog.ShowDialog();
            }
        }

        public void Kody()
        {
            if (SelectedPW != null && SelectedPW.Pozycja != null && SelectedPW.Pozycja.id > 0)
            {
                var proddp_monit = db.PRODDP_MONIT.Where(i => i.id_proddp == SelectedPW.Pozycja.id).ToList();
                foreach (PRODDP_MONIT monit in proddp_monit)
                {
                    Functions.GenerujKodSSCC(monit.id);
                }
            }

            DrukujEtykieteGS1();
        }

        public void RezerwujDostawy()
        {
            short lp;
            foreach (ProdukcjaPozycjeVM poz in _produkcjaPozycjeRW)
            {
                lp = (short)(_produkcjaPozycjeRW.IndexOf(poz) + 1);
                if (poz.Pozycja.idproddw != null && poz.Pozycja.idproddw > 0)
                    RezerwujDostawe(poz.Pozycja, lp);
            }
        }

        public void RezerwujDostawyFK()
        {
            if (_produkcjaPozycjeRW_FK != null)
            {
                short lp;
                foreach (ProdukcjaPozycjeVM poz in _produkcjaPozycjeRW_FK)
                {
                    lp = (short)(_produkcjaPozycjeRW_FK.IndexOf(poz) + 1);
                    if (poz.PozycjaFK.idproddw != null && poz.PozycjaFK.idproddw > 0)
                        RezerwujDostaweFK(poz.PozycjaFK, lp);
                }
            }
        }

        //Rezerwuje dostawy z magazynu produkcyjnego
        public void RezerwujDostawyMG()
        {
            if (dostawyMG != null && dostawyMG.Count > 0)
            {

                foreach (var pair in dostawyMG)
                {
                    if (ProdukcjaPozycjeRW.ElementAtOrDefault(pair.Key) != null)
                        RezerwujDostaweProdMG(pair.Value, ProdukcjaPozycjeRW[pair.Key]);

                    // MessageBox.Show(String.Format("{0}, {1}", pair.Key, pair.Value));
                }
            }   
        }

        
        //Usuwa rezerwacje  z magazynu produkcyjnego
        public void UsunRezerwacjeProdMG(List<PRODDP> pozycje)
        {
            bool jest = false;
            List<PROD_PRODMGPW> ListaPw;

            foreach(PRODDP pozRW in pozycje)
            {
                jest = false;
                foreach(ProdukcjaPozycjeVM _pozRW in _produkcjaPozycjeRW)
                {
                    if(pozRW.id == _pozRW.Pozycja.id)
                    {
                        jest = true;
                    }
                }
                if(!jest)
                {
                    ListaPw = db.PROD_PRODMGPW.Where(x => x.id_proddp == pozRW.id).ToList();
                    foreach(PROD_PRODMGPW pw in ListaPw)
                    {
                        var ilosc = pw.ilosc;
                        var proddw = db.PROD_MGDW.Where(x => x.id == pw.id_prodmgdw).FirstOrDefault();
                        proddw.iloscprod -= ilosc;
                        db.PROD_PRODMGPW.Remove(pw);
                        db.SaveChanges();
                    }
                }
            }
        }

       
        //Rezerwuje pojedyńczą dostawe z magazynu Symfonii
        public void RezerwujDostawe(PRODDP proddp, short lp)
        {

            using (FZLEntities1 db = new FZLEntities1())
            {
                var item = db.PROD_PW.FirstOrDefault(i => i.id_proddp == proddp.id);
                var item2 = db.PROD_HMDW.FirstOrDefault((System.Linq.Expressions.Expression<Func<PROD_HMDW, bool>>)(i => i.id == proddp.idproddw));
                var item3 = db.PRODDP.FirstOrDefault(i => i.id == proddp.id);

                // if(item3 != null)
                // item3.lp = lp;


                if (item != null)
                {
                    item.ilosc = proddp.ilosc;
                    item.id_dwfk = proddp.iddw;
                }
                else
                {
                    if (proddp.idproddw > 0)
                    {
                        PROD_PW prodpw = new PROD_PW();
                        prodpw.id_prod = ProdukcjaInfo.Produkcja.id;
                        prodpw.id_proddp = proddp.id;
                        prodpw.id_proddw = (int)proddp.idproddw;
                        prodpw.id_dwfk = proddp.iddw;
                        prodpw.ilosc = proddp.ilosc;
                        prodpw.data = DateTime.Now;
                        db.PROD_PW.Add(prodpw);
                    }
                }

                db.SaveChanges();

                //sumuj ilsci rezerwacji dla danej dostawy we weszystkich zlecenach produkcyjnych
                var lbs = (from g in db.PROD_PW where g.id_proddw == item2.id select g.ilosc).Sum();
                //MessageBox.Show(lbs.ToString());

                item2.iloscdoprod = lbs;
                db.SaveChanges();
            }
        }

        //Rezerwuje pojedyńczą dostawe z magazynu Symfonii
        public void RezerwujDostaweFK(PRODDP_FK proddp, short lp)
        {
            db = new FZLEntities1();
            var item = db.PROD_PW.FirstOrDefault(i => i.id_proddpfk == proddp.id);
            var item2 = db.PROD_HMDW.FirstOrDefault((System.Linq.Expressions.Expression<Func<PROD_HMDW, bool>>)(i => i.id == proddp.idproddw));
            var item3 = db.PRODDP_FK.FirstOrDefault(i => i.id == proddp.id);

            // if(item3 != null)
            // item3.lp = lp;


            if (item != null)
            {
                item.ilosc = proddp.ilosc;
                item.id_dwfk = proddp.iddw;
            }
            else
            {
                if (proddp.idproddw > 0)
                {
                    PROD_PW prodpw = new PROD_PW();
                    prodpw.id_prod = ProdukcjaInfo.Produkcja.id;
                    prodpw.id_proddpfk = proddp.id;
                    prodpw.id_proddw = (int)proddp.idproddw;
                    prodpw.id_dwfk = proddp.iddw;
                    prodpw.ilosc = proddp.ilosc;
                    prodpw.data = DateTime.Now;
                    db.PROD_PW.Add(prodpw);
                }
            }

            db.SaveChanges();

            //sumuj ilsci rezerwacji dla danej dostawy we weszystkich zlecenach produkcyjnych
            var lbs = (from g in db.PROD_PW where g.id_proddw == item2.id select g.ilosc).Sum();
            //MessageBox.Show(lbs.ToString());

            item2.iloscdoprod = lbs;
            db.SaveChanges();
        }

        //Rezerwuje pojedyncza dostawe z magayznu produkcyjnego
        //Ustala dostepny stan towarow na postawie ilosci uzytej w produkcji
        public void  RezerwujDostaweProdMG(ProdukcjaMGDwVM dw, ProdukcjaPozycjeVM pozRW)
        {
            using (FZLEntities1 db2 = new FZLEntities1())
            {
                var itemOld = db2.PRODDP.Where(x => x.id == pozRW.Pozycja.id).FirstOrDefault();

                double RezerwacjeProdSuma;
                //db = new FZLEntities1();
                //Znadz rekord dotyczacy danej dosatawy z magazynu produkcyjnego
                var item1 = (from c in db2.PROD_MGDW
                             where c.id == pozRW.Pozycja.idmgdw

                             select c).FirstOrDefault();

                //Oblicz sume aktualnych rezerwacji dla celów produkcyjnych
                var item2 = (from c in db2.PROD_PRODMGPW
                             where c.id_prodmgdw == pozRW.Pozycja.idmgdw
                             where c.id_dkprodmg == null
                             select c.ilosc).ToList();
                if (item2 != null)
                    RezerwacjeProdSuma = item2.Sum();
                //Sprawdz czy istnieje juz powiazanie dostawy magazynowej z pozycja rw w produkcji
                var item3 = (from c in db2.PROD_PRODMGPW
                                 //Jedna pozycja jedna dostawa
                             where c.id_prodmgdw == pozRW.Pozycja.idmgdw
                             where c.id_proddp == pozRW.Pozycja.id
                             select c).FirstOrDefault();
                //usuń te rezewacje które są inne niż wzkazana na pozycji
                var range = db2.PROD_PRODMGPW.Where(x => x.id_proddp == pozRW.Pozycja.id && x.id_prodmgdw != pozRW.Pozycja.idmgdw).ToList();
                if (range != null && range.Count() > 0)
                {
                    db2.PROD_PRODMGPW.RemoveRange(range);
                }
                //sprawdz informacje o powiazaniu, jeżeli informacje o powiazaniu sa rozne
                //od tych zapisanych w bazeie uaktualnij je
                if (item3 != null && item3.id > 0)
                {
                    PROD_MGDW dw_old = db2.PROD_MGDW.Where(x => x.id == item3.id_prodmgdw).FirstOrDefault();
                    if (dw_old.id != pozRW.Pozycja.idmgdw)
                    {

                        //var itemOld2 = db.PROD_MGDW.Where(x => x.id == item3.First().id_prodmgdw).FirstOrDefault();
                        dw_old.iloscprod -= item3.ilosc;
                        item3.ilosc = (double)pozRW.Pozycja.ilosc_mg;
                        item3.id_dkprodmg = pozRW.Pozycja.idmgdw;


                    }
                    if (pozRW.Pozycja.idmgdw != null && dw_old.id == pozRW.Pozycja.idmgdw)
                    {
                        item3.ilosc = (double)pozRW.Pozycja.ilosc_mg;
                        //item3.id_prodmgdw = (int)pozRW.Pozycja.idmgdw;
                        // db.SaveChanges();
                    }

                }


                //W przypadku braku zapisu powiazania utworz nowy wpis
                else
                {
                    if (pozRW.Pozycja.ilosc_mg != null && pozRW.Pozycja.ilosc_mg > 0)
                    {
                        var obj = new PROD_PRODMGPW();
                        obj.id_prodmgdw = dw.ProdukcjaDW.id;
                        obj.id_proddp = pozRW.Pozycja.id;
                        obj.ilosc = (double)pozRW.Pozycja.ilosc_mg;

                        db2.PROD_PRODMGPW.Add(obj);
                        db2.SaveChanges();
                    }
                }




                //sumuj ilsci rezerwacji dla danej dostawy we weszystkich zlecenach produkcyjnych


                double suma_rezerwacji = 0;
                var lbs = (from g in db2.PROD_PRODMGPW where g.id_prodmgdw == pozRW.Pozycja.idmgdw && g.id_dkprodmg == null select g.ilosc).ToList();
                if (lbs.Count() > 0)
                {

                    suma_rezerwacji = lbs.Sum();

                    //MessageBox.Show(lbs.ToString());
                    item1.iloscprod = suma_rezerwacji;
                    //db.SaveChanges();
                }

                db2.SaveChanges();
            }

        }

        protected void PobierzDostawyProdMgPw()
        {
            if (DostawyProdMGPowiazania == null)
                DostawyProdMGPowiazania = new List<PROD_PRODMGPW>();
           foreach(ProdukcjaPozycjeVM poz in ProdukcjaPozycjeRW)
           {
                var item = db.PROD_PRODMGPW.Where(x => x.id_proddp == poz.Pozycja.id).ToList();
                if (item != null && item.Count > 0)
                {
                    var pw = item.First();
                    //sprawdz czy juz dany obiekt jest w tabeli
                    if (DostawyProdMGPowiazania.Count > 0)
                    {
                        var obj = DostawyProdMGPowiazania.Where(x => x.id_proddp == poz.Pozycja.id).ToList();
                        if (obj != null && obj.Count > 0)
                        {
                            DostawyProdMGPowiazania.RemoveAt(DostawyProdMGPowiazania.IndexOf(obj.First()));
                        }
                    }
                    
                    DostawyProdMGPowiazania.Add(pw);
                }


           }
           
                
        }

        protected void RezerwujDostaweOpakowania(PROD_PRODMGPW pw)
        {
            //db = new FZLEntities1();
            var itemOld = db.PRODDP_MONIT.Where(x => x.id == pw.id_proddpmonit).FirstOrDefault();

            double RezerwacjeProdSuma;
            //db = new FZLEntities1();
            //Znadz rekord dotyczacy danej dosatawy z magazynu produkcyjnego
            var item1 = (from c in db.PROD_MGDW
                         where c.id == pw.id_prodmgdw
                         select c).FirstOrDefault();



            //Oblicz sume aktualnych rezerwacji dla celów produkcyjnych
            var item2 = (from c in db.PROD_PRODMGPW
                         where c.id_prodmgdw == pw.id_prodmgdw
                         select c.ilosc).ToList();
            if (item2 != null)
                RezerwacjeProdSuma = item2.Sum();
            //Sprawdz czy istnieje juz powiazanie dostawy magazynowej z pozycja rw w produkcji
            var item3 = (from c in db.PROD_PRODMGPW
                             //Jedna pozycja jedna dostawa
                             // where c.id_prodmgdw == dw.ProdukcjaDW.id
                         where c.id_proddpmonit == pw.id_proddpmonit
                         where c.id_prodmgdw == pw.id_prodmgdw
                         select c).FirstOrDefault();


            //sprawdz informacje o powiazaniu, jeżeli informacje o powiazaniu sa rozne
            //od tych zapisanych w bazeie uaktualnij je
            if (item3 != null && item3.id > 0)
            {
                PROD_MGDW dw_old = db.PROD_MGDW.Where(x => x.id == item3.id_prodmgdw).FirstOrDefault();
                if (dw_old.id != pw.id_proddpmonit)
                {

                    //var itemOld2 = db.PROD_MGDW.Where(x => x.id == item3.First().id_prodmgdw).FirstOrDefault();
                    dw_old.iloscprod -= item3.ilosc;


                }

                item3.ilosc = (double)pw.ilosc;
                item3.id_prodmgdw = (int)pw.id_prodmgdw;
                db.SaveChanges();
            }
            //W przypadku braku zapisu powiazania utworz nowy wpis
            else
            {
                var obj = new PROD_PRODMGPW();
                obj.id_prodmgdw = pw.id_prodmgdw;
                obj.id_proddp = pw.id_proddp;
                obj.ilosc = (double)pw.ilosc;
                obj.id_proddpmonit = itemOld.id;
                db.PROD_PRODMGPW.Add(obj);
                db.SaveChanges();
            }


            db.SaveChanges();

            //sumuj ilsci rezerwacji dla danej dostawy we weszystkich zlecenach produkcyjnych


            double suma_rezerwacji = 0;
            var lbs = (from g in db.PROD_PRODMGPW where g.id_prodmgdw == pw.id_prodmgdw select g.ilosc).ToList();
            if (lbs.Count() > 0)
            {
                db = new FZLEntities1();
                item1 = (from c in db.PROD_MGDW
                         where c.id == pw.id_prodmgdw
                         select c).FirstOrDefault();
                suma_rezerwacji = lbs.Sum();

                //MessageBox.Show(lbs.ToString());
                item1.iloscprod = suma_rezerwacji;
                db.SaveChanges();
            }
        }


        public void ZapiszPozycjeRW(int idprod)
        {
            
            var _pozycjrwOld = db.PRODDP.Where(x => x.id_prod == idprod && x.rodzaj_dk == "RW").ToList();
            UsunRezerwacjeProdMG(_pozycjrwOld);
            var inserted = (from c in _produkcjaPozycjeRW
                            where c.IsNew
                            select c).ToList();
           
            if (db.ChangeTracker.HasChanges() || inserted.Count > 0)
            {

                foreach (ProdukcjaPozycjeVM c in inserted)
                {   
                    if (c.IsNew)
                    {
                        c.Pozycja.id_prod = ProdukcjaInfo.Produkcja.id;
                        //c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;
                        c.Pozycja.rodzaj_dk = "RW";
                        //c.Pozycja.lp = (short)inserted.IndexOf(c);

                        db.PRODDP.Add(c.Pozycja);

                    }
                }
                try
                {
                    foreach (ProdukcjaPozycjeVM c in inserted)
                    {
                        c.IsNew = false;

                        //c.Pozycja.lp = (short)inserted.IndexOf(c);
                        // c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;
                    }
                    db.SaveChanges();
                    //msg.Message = "Database Updated";
                }
                catch (Exception e)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        if(e.InnerException != null)
                            ErrorMessage = e.InnerException.GetBaseException().ToString();
                    }
                    //MessageBox.Show(e.InnerException.ToString());
                    //msg.Message = "There was a problem updating the database";
                }
            }
            UstalLpDlaRW();
            RaisePropertyChanged("ProdukcjaPozycjeRW");
        }

        public void ZapiszPozycjeRW_FK(int idprod)
        {

            if (_produkcjaPozycjeRW_FK != null)
            {
                var _pozycjrwOld = db.PRODDP_FK.Where(x => x.id_prod == idprod && x.rodzaj_dk == "RW").ToList();

                var inserted = (from c in _produkcjaPozycjeRW_FK
                                where c.IsNew
                                select c).ToList();

                if (db.ChangeTracker.HasChanges() || inserted.Count > 0)
                {
                    foreach (ProdukcjaPozycjeVM c in inserted)
                    {
                        if (c.IsNew)
                        {
                            c.PozycjaFK.id_prod = ProdukcjaInfo.Produkcja.id;
                            //c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;
                            c.PozycjaFK.rodzaj_dk = "RW";
                            //c.Pozycja.lp = (short)inserted.IndexOf(c);
                            db.PRODDP_FK.Add(c.PozycjaFK);
                        }
                    }
                    try
                    {
                        foreach (ProdukcjaPozycjeVM c in inserted)
                        {
                            c.IsNew = false;
                            //c.Pozycja.lp = (short)inserted.IndexOf(c);
                            // c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;
                        }
                        db.SaveChanges();
                        //msg.Message = "Database Updated";
                    }
                    catch (Exception e)
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            if (e.InnerException != null)
                                ErrorMessage = e.InnerException.GetBaseException().ToString();
                        }
                        //MessageBox.Show(e.InnerException.ToString());
                        //msg.Message = "There was a problem updating the database";
                    }
                }
                UstalLpDlaRW();
                RaisePropertyChanged("ProdukcjaPozycjeRW_Fk");
            }
        }

        public void ZapiszPozycjeSYM(int idprod)
        {

            if (_produkcjaPozycjeSYM != null)
            {
                var _pozycjrwOld = db.PRODDP_SYM.Where(x => x.id_prod == idprod && x.rodzaj_dk == "RW").ToList();

                var inserted = (from c in _produkcjaPozycjeSYM
                                where c.IsNew
                                select c).ToList();

                if (db.ChangeTracker.HasChanges() || inserted.Count > 0)
                {
                    foreach (ProdukcjaPozycjeVM c in inserted)
                    {
                        if (c.IsNew)
                        {
                            c.PozycjaSYM.id_prod = ProdukcjaInfo.Produkcja.id;
                            //c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;
                            c.PozycjaSYM.rodzaj_dk = "RW";
                            //c.Pozycja.lp = (short)inserted.IndexOf(c);
                            db.PRODDP_SYM.Add(c.PozycjaSYM);
                        }
                    }
                    try
                    {
                        foreach (ProdukcjaPozycjeVM c in inserted)
                        {
                            c.IsNew = false;
                            //c.Pozycja.lp = (short)inserted.IndexOf(c);
                            // c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;
                        }
                        db.SaveChanges();
                        //msg.Message = "Database Updated";
                    }
                    catch (Exception e)
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            if (e.InnerException != null)
                                ErrorMessage = e.InnerException.GetBaseException().ToString();
                        }
                        //MessageBox.Show(e.InnerException.ToString());
                        //msg.Message = "There was a problem updating the database";
                    }
                }
               // UstalLpDlaRW();
                RaisePropertyChanged("ProdukcjaPozycjeSYM");
            }
        }

        public void ZapiszPozycjePW(int idprod)
        {
            var inserted = (from c in ProdukcjaPozycjePW
                            where c.IsNew
                            select c).ToList();

            if (db.ChangeTracker.HasChanges() || inserted.Count > 0)
            {

                foreach (ProdukcjaPozycjeVM c in inserted)
                {
                    if (c.IsNew)
                    {
                        c.Pozycja.id_prod = ProdukcjaInfo.Produkcja.id;
                        //c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;
                        c.Pozycja.rodzaj_dk = "PW";
                        if (c.Pozycja.nr_partii == null)
                            c.Pozycja.nr_partii = "";
                        db.PRODDP.Add(c.Pozycja);

                    }
                }
                try
                {
                    foreach (ProdukcjaPozycjeVM c in inserted)
                    {
                        c.IsNew = false;
                        if (c.Pozycja.nr_partii == null)
                            c.Pozycja.nr_partii = "";
                       
                        // c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;

                    }
                   
                    db.SaveChanges();
                    //msg.Message = "Database Updated";
                }
                catch (Exception e)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        ErrorMessage = e.InnerException.GetBaseException().ToString();
                    }
                    //MessageBox.Show(e.InnerException.ToString());
                    //msg.Message = "There was a problem updating the database";
                }
            }
            RaisePropertyChanged("ProdukcjaPozycjePW");
        }


        public void ZapiszPozycjePW_FK(int idprod)
        {
            if (ProdukcjaPozycjePW_FK != null)
            {
                var inserted = (from c in ProdukcjaPozycjePW_FK
                                where c.IsNew
                                select c).ToList();

                if (db.ChangeTracker.HasChanges() || inserted.Count > 0)
                {

                    foreach (ProdukcjaPozycjeVM c in inserted)
                    {
                        if (c.IsNew)
                        {
                            c.PozycjaFK.id_prod = ProdukcjaInfo.Produkcja.id;
                            //c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;
                            c.PozycjaFK.rodzaj_dk = "PW";
                            if (c.PozycjaFK.nr_partii == null)
                                c.PozycjaFK.nr_partii = "";
                            db.PRODDP_FK.Add(c.PozycjaFK);

                        }
                    }
                    try
                    {
                        foreach (ProdukcjaPozycjeVM c in inserted)
                        {
                            c.IsNew = false;
                            if (c.PozycjaFK.nr_partii == null)
                                c.PozycjaFK.nr_partii = "";
                            // c.Pozycja.PROD_HMDW.iloscdoprod = c.Pozycja.ilosc;

                        }
                        db.SaveChanges();
                        //msg.Message = "Database Updated";
                    }
                    catch (Exception e)
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            ErrorMessage = e.InnerException.GetBaseException().ToString();
                        }
                        //MessageBox.Show(e.InnerException.ToString());
                        //msg.Message = "There was a problem updating the database";
                    }
                }
                RaisePropertyChanged("ProdukcjaPozycjePW_FK");
            }
        }


        public void Zapisz()
        {
            PrzeliczWartosci();
            PrzeliczWartosciFK();
            if (db.ChangeTracker.HasChanges() || ProdukcjaInfo.IsNew)
            {
                if (ProdukcjaInfo.IsNew)
                {
                    ProdukcjaInfo.Produkcja.okres = int.Parse(ProdukcjaInfo.Produkcja.data.Value.Year.ToString());
                    ProdukcjaInfo.Produkcja.serial = ProdukcjaInfo.pobierzKolejnyNumerSerii((int)ProdukcjaInfo.Produkcja.okres);
                    ProdukcjaInfo.Produkcja.nazwa = "ZP/" + ProdukcjaInfo.Produkcja.serial.ToString() + "/" + ProdukcjaInfo.Produkcja.okres.ToString();
                    ProdukcjaInfo.Produkcja.kod = "ZP/" + ProdukcjaInfo.Produkcja.serial.ToString() + "/" + ProdukcjaInfo.Produkcja.okres.ToString();
                    ProdukcjaInfo.Produkcja.osoba = user.nazwa;
                    db.PROD.Add(ProdukcjaInfo.Produkcja);
                }
                try
                {
                    db.SaveChanges();
                    //ProdukcjaInfo.IsNew = false;

                    // msg.Message = "Database Updated";
                }
                catch (Exception e)
                {
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        if (e.InnerException != null)
                        {
                            ErrorMessage = e.InnerException.GetBaseException().ToString();
                            MessageBox.Show(ErrorMessage);
                        }
                    }
                   
                }

            }
           
            ZapiszPozycjeRW(ProdukcjaInfo.Produkcja.id);
            RezerwujDostawyMG();
            ZapiszPozycjePW(ProdukcjaInfo.Produkcja.id);

            ZapiszPozycjePW_FK(ProdukcjaInfo.Produkcja.id);
            ZapiszPozycjeRW_FK(ProdukcjaInfo.Produkcja.id);
            RezerwujDostawyFK();
            ZapiszPozycjeSYM(ProdukcjaInfo.Produkcja.id);

            SumujIlosciPWMONIT();
            ZapiszMonitPW();
           
            //System.Windows.MessageBox.Show(ProdukcjaInfo.Produkcja.id.ToString());
            ZapiszLinie();
            ZapiszUsunieteLinie();
            RezerwujDostawy();
            
           
            
            if(ProdukcjaInfo.Produkcja.zakonczono == 1)
            {
               WystawDokumentyMagazynoweOpakowania();
             
            }

            //db.SaveChanges();
            getProdukcjaVM(ProdukcjaInfo);
            PobierzZapisaneLinie();
            
            UserMessage msg = new UserMessage();
            msg.Message = "Zapisano";
            Messenger.Default.Send<UserMessage>(msg);
            // RaisePropertyChanged("ProdukcjaInfo");
        }

        private void ZapiszLinie()
        {
            foreach(ProdukcjaLiniaVM pl in ProdukcjaLinie)
            {
                pl.ZapiszLinie(ProdukcjaInfo.Produkcja);
            }
        }

        private void ZapiszUsunieteLinie()
        {
            foreach (ProdukcjaLiniaVM pl in ProdukcjaLinieUsuniete)
            {
                pl.UsunLinie();
            }
            ProdukcjaLinieUsuniete = new ObservableCollection<ProdukcjaLiniaVM>();
        }

        private void ZapiszMonitPW()
        {
            //ProdukcjaPozycjeMonit.Add(monitVM);
            int lp=0;
            db = new FZLEntities1();
            foreach (ProdukcjaPozycjeVM poz in ProdukcjaPozycjePW)
            {
                if (ProdukcjaPozycjeMonit != null)
                {
                    foreach (ProdukcjaPozycjeMonitVM monit in ProdukcjaPozycjeMonit)
                    {
                        if (monit.lp == lp)
                        {
                            monit.ProdDPMonit.id_proddp = poz.Pozycja.id;
                            monit.Zapisz(monit.ProdDPMonit);
                            //poz.PrzypiszNazwyOpakowan();
                        }

                    }
                    lp += 1;
                    ProdukcjaInfo.PrzepiszSpecyfikacjePakowania(poz.Pozycja.id);
                }
            }
            if (OpakowaniaPowiazania != null)
            {
                foreach (PROD_PRODMGPW pw in OpakowaniaPowiazania)
                {
                    if (pw.ilosc == 0)
                        UsunRezerwacjeOpakowania((int)pw.id_proddpmonit, (int)pw.id_prodmgdw);
                }

                foreach (PROD_PRODMGPW pw in OpakowaniaPowiazania)
                {
                    
                    RezerwujDostaweOpakowania(pw);
                }
            }

            WczytajPozycjePW_MONIT();
           
        }

        public void UsunPocycjeRW()
        {
            UserMessage msg = new UserMessage();
            if (SelectedRW != null)
            {
                {
                    if (SelectedRW.Pozycja.id > 0)
                    {
                        //Usun rezerwacje oraz informacje o użytej ilosci do produkcji w danej dostawie
                        foreach (PROD_PW pw in SelectedRW.Pozycja.PROD_PW.ToList())
                        {
                            db.PROD_PW.Remove(pw);
                            var item2 = db.PROD_HMDW.FirstOrDefault((System.Linq.Expressions.Expression<Func<PROD_HMDW, bool>>)(i => i.id == pw.id_proddw));
                            item2.iloscdoprod -= pw.ilosc;
                        }
                        foreach (PROD_PRODMGPW pw in SelectedRW.Pozycja.PROD_PRODMGPW.ToList())
                        {
                            db.PROD_PRODMGPW.Remove(pw);
                            
                            var item2 = db.PROD_MGDW.FirstOrDefault((System.Linq.Expressions.Expression<Func<PROD_MGDW, bool>>)(i => i.id == pw.id_prodmgdw));
                            if (item2 != null)
                            {
                                item2.iloscprod -= pw.ilosc;
                            }
                        }
                        db.PRODDP.Remove(SelectedRW.Pozycja);
                    }
                    _produkcjaPozycjeRW.Remove(SelectedRW);

                    RaisePropertyChanged("ProdukcjaPozycjeRW");
                    msg.Message = "Deleted";
                }
            }
            else
            {
                msg.Message = "No Product selected to delete";
            }
            Messenger.Default.Send<UserMessage>(msg);
        }

        public void UsunPocycjeRW_FK()
        {
            UserMessage msg = new UserMessage();
            if (SelectedRW_FK != null)
            {
                {
                    if (SelectedRW_FK.PozycjaFK.id > 0)
                    {
                        //Usun rezerwacje oraz informacje o użytej ilosci do produkcji w danej dostawie
                        foreach (PROD_PW pw in SelectedRW_FK.PozycjaFK.PROD_PW.ToList())
                        {
                            db.PROD_PW.Remove(pw);
                            var item2 = db.PROD_HMDW.FirstOrDefault((System.Linq.Expressions.Expression<Func<PROD_HMDW, bool>>)(i => i.id == pw.id_proddw));
                            item2.iloscdoprod -= pw.ilosc;
                        }
                        db.PRODDP_FK.Remove(SelectedRW_FK.PozycjaFK);
                    }
                    _produkcjaPozycjeRW_FK.Remove(SelectedRW_FK);

                    RaisePropertyChanged("ProdukcjaPozycjeRW_FK");
                    msg.Message = "Deleted";
                }
            }
            else
            {
                msg.Message = "No Product selected to delete";
            }
            Messenger.Default.Send<UserMessage>(msg);
        }

        public void UsunPocycjeRW_SYM()
        {
            UserMessage msg = new UserMessage();
            if (SelectedRW_SYM != null && SelectedRW_SYM.PozycjaSYM != null)
            {
                {
                   
                        
                    
                   

                    db.PRODDP_SYM.Remove(SelectedRW_SYM.PozycjaSYM);
                    _produkcjaPozycjeSYM.Remove(SelectedRW_SYM);
                    RaisePropertyChanged("ProdukcjaPozycjeSYM");
                    msg.Message = "Deleted";
                }
            }
            else
            {
                msg.Message = "No Product selected to delete";
            }
            Messenger.Default.Send<UserMessage>(msg);
        }

        public void UsunPocycjePW()
        {
            UserMessage msg = new UserMessage();
            if (SelectedPW != null)
            {
                {
                    if (SelectedPW.Pozycja.id > 0)
                        db.PRODDP.Remove(SelectedPW.Pozycja);


                    ProdukcjaPozycjePW.Remove(SelectedPW);
                    RaisePropertyChanged("ProdukcjaPozycjePW");
                    msg.Message = "Deleted";
                }
            }
            else
            {
                msg.Message = "No Product selected to delete";
            }
            Messenger.Default.Send<UserMessage>(msg);
        }

        public void UsunPocycjePW_FK()
        {
            UserMessage msg = new UserMessage();
            if (SelectedPW_FK != null)
            {
                {
                    if (SelectedPW_FK.PozycjaFK.id > 0)
                        db.PRODDP_FK.Remove(SelectedPW_FK.PozycjaFK);
                    ProdukcjaPozycjePW_FK.Remove(SelectedPW_FK);

                    RaisePropertyChanged("ProdukcjaPozycjePW_FK");
                    msg.Message = "Deleted";
                }
            }
            else
            {
                msg.Message = "No Product selected to delete";
            }
            Messenger.Default.Send<UserMessage>(msg);
        }


        public void wstawPozycjeDWPRODMG(List<ProdukcjaMGDwVM> dostawy)
        {
            this.dostawyProdMG = dostawy;
            ProdukcjaPozycjeVM poz;
            bool jest = false;

            foreach (ProdukcjaMGDwVM dostawa in dostawy)
            {
                jest = false;

                if (_produkcjaPozycjeRW.Count > 0)
                {
                    foreach (ProdukcjaPozycjeVM ppoz in _produkcjaPozycjeRW)
                    {
                        //Sprawdz czy dostawa jest juz ujenta w danej pozycji
                        if (ppoz.Pozycja.idmgdw == dostawa.ProdukcjaDW_List.id)
                        {
                            jest = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                //Jezeli nie ma to dodaj nową pozycje
                if (!jest)
                {
                    poz = new ProdukcjaPozycjeVM();
                    poz.Pozycja.kodtw = dostawa.ProdukcjaDW_List.kodtw;
                    poz.Pozycja.kodmgdw = dostawa.ProdukcjaDW_List.kod + " (" + dostawa.ProdukcjaDW.nr_partii + ")";
                    poz.Pozycja.nr_partii = dostawa.ProdukcjaDW_List.nr_partii;
                    poz.Pozycja.khnazwa = dostawa.ProdukcjaDW_List.khnazwa;
                    poz.Pozycja.idmgdw = dostawa.ProdukcjaDW_List.id;
                    //poz.Pozycja.idproddw = dostawa.ProdukcjaDW.id;
                    //poz.Pozycja.opis = dostawa.ProdukcjaDW.kod;
                    poz.Pozycja.nazwatw = dostawa.ProdukcjaDW_List.nazwatw;
                    poz.Pozycja.ilosc_mg = dostawa.ProdukcjaDW_List.iloscdost;
                   // poz.Pozycja.cena = dostawa.ProdukcjaDW.cena;
                    poz.Pozycja.idtw = (int)dostawa.ProdukcjaDW_List.idtw;
                    poz.Pozycja.frakcja = dostawa.ProdukcjaDW_List.frakcja;

                    if(dostawa.ProdukcjaDW.cena > 0 )
                    {
                        poz.Pozycja.cena = dostawa.ProdukcjaDW.cena;
                    }

                    //Obsługa wstawiania wigotnosci i czystosci z prob i partii
                    var query = (
                                 from p in db.PARTIE
                                 where p.nr_partii.Contains(dostawa.ProdukcjaDW.nr_partii)
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        poz.Pozycja.czystosc = query.czystosc;
                        poz.Pozycja.wilgotnosc = query.wilgotnosc;
                    }
                    //obsługa wstawiania informacji o wyrobie gptwym ponownie uzytym do produkcji
                    //MessageBox.Show(dostawa.ProdukcjaDW.iddkpz.ToString());
                    PRODDP surowiec_wg = Functions.PobierzZlecenieDlaDostawy(dostawa.ProdukcjaDW_List.id, db);
                    if (surowiec_wg != null)
                    {
                        poz.Pozycja.czystosc = surowiec_wg.czystosc;
                        poz.Pozycja.wilgotnosc = surowiec_wg.wilgotnosc;
                        poz.Pozycja.frakcja = surowiec_wg.frakcja;
                        poz.Pozycja.opis = surowiec_wg.opis;
                        //MessageBox.Show(surowiec_wg.id_hm.ToString());
                    }
                    //poz.Pozycja.lp = (short)(ProdukcjaPozycjeRW.Where(p => p.Pozycja.idproddw > 0).ToList().Max(i => i.Pozycja.lp) + 1;
                    // poz.Pozycja.jm = dostawa.ProdukcjaDW.
                    _produkcjaPozycjeRW.Add(poz);
                }
            }

            this.RaisePropertyChanged("ProdukcjaPozycjeRW");
        }

        public void wstawPozycjeDW(List<ProdukcjaDwVM> dostawy)
        {
            this.dostawy = dostawy;
            ProdukcjaPozycjeVM poz;
            bool jest = false;

            foreach (ProdukcjaDwVM dostawa in dostawy)
            {
                jest = false;

                if (_produkcjaPozycjeRW.Count > 0)
                {
                    foreach (ProdukcjaPozycjeVM ppoz in _produkcjaPozycjeRW)
                    {
                        //Sprawdz czy dostawa jest juz ujenta w danej pozycji
                        if (ppoz.Pozycja.iddw == dostawa.ProdukcjaDW.id_fk)
                        {
                            jest = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                //Jezeli nie ma to dodaj nową pozycje
                if (!jest)
                {
                    poz = new ProdukcjaPozycjeVM();
                    poz.Pozycja.kodtw = dostawa.ProdukcjaDW.kodtw;
                    poz.Pozycja.khnazwa = dostawa.ProdukcjaDW.khnazwa;
                    poz.Pozycja.iddw = dostawa.ProdukcjaDW.id_fk;
                    poz.Pozycja.idproddw = dostawa.ProdukcjaDW.id;
                    //poz.Pozycja.opis = dostawa.ProdukcjaDW.kod;
                    poz.Pozycja.nazwatw = dostawa.ProdukcjaDW.opistw;
                    poz.Pozycja.ilosc = dostawa.ProdukcjaDW.stan;
                    poz.Pozycja.cena = dostawa.ProdukcjaDW.cena;
                    poz.Pozycja.idtw = (int)dostawa.ProdukcjaDW.idtw;
                    poz.Pozycja.koddw = dostawa.ProdukcjaDW.kod;
                    //Obsługa wstawiania wigotnosci i czystosci z prob i partii
                    var query = (from f in db.PARTIE_FK
                                 from p in db.PARTIE
                                 where f.iddk_fki == dostawa.ProdukcjaDW.iddkpz
                                 where p.id == f.id_partii
                                 where f.kod_firmy == dostawa.ProdukcjaDW.kod_firmy
                                 select p).FirstOrDefault();
                    if(query != null)
                    {
                        poz.Pozycja.czystosc = query.czystosc;
                        poz.Pozycja.wilgotnosc = query.wilgotnosc;
                    }
                    //obsługa wstawiania informacji o wyrobie gptwym ponownie uzytym do produkcji
                    //MessageBox.Show(dostawa.ProdukcjaDW.iddkpz.ToString());
                    PRODDP surowiec_wg = Functions.PobierzPozycjePrzezDokument(dostawa.ProdukcjaDW.iddkpz);
                    if(surowiec_wg != null)
                    {
                        poz.Pozycja.czystosc = surowiec_wg.czystosc;
                        poz.Pozycja.wilgotnosc = surowiec_wg.wilgotnosc;
                        poz.Pozycja.frakcja = surowiec_wg.frakcja;
                        poz.Pozycja.opis = surowiec_wg.opis;
                        //MessageBox.Show(surowiec_wg.id_hm.ToString());
                    }
                    //poz.Pozycja.lp = (short)(ProdukcjaPozycjeRW.Where(p => p.Pozycja.idproddw > 0).ToList().Max(i => i.Pozycja.lp) + 1;
                    // poz.Pozycja.jm = dostawa.ProdukcjaDW.
                    _produkcjaPozycjeRW.Add(poz);
                }
            }

            this.RaisePropertyChanged("ProdukcjaPozycjeRW");
        }

        public void wstawPozycjeDW_FK(List<ProdukcjaDwVM> dostawy)
        {
            this.dostawy = dostawy;
            ProdukcjaPozycjeVM poz;
            bool jest = false;
            if(_produkcjaPozycjeRW_FK ==  null)
            {
                _produkcjaPozycjeRW_FK = new ObservableCollection<ProdukcjaPozycjeVM>();
            }
            foreach (ProdukcjaDwVM dostawa in dostawy)
            {
                jest = false;

                if (_produkcjaPozycjeRW_FK.Count > 0)
                {
                    foreach (ProdukcjaPozycjeVM ppoz in _produkcjaPozycjeRW_FK)
                    {
                        //Sprawdz czy dostawa jest juz ujenta w danej pozycji
                        if (ppoz.PozycjaFK.iddw == dostawa.ProdukcjaDW.id_fk  )
                        {
                            jest = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                //Jezeli nie ma to dodaj nową pozycje
                //marazie pozwalamy na zmiane
                if (true)
                {
                    poz = new ProdukcjaPozycjeVM();
                    poz.PozycjaFK.kodtw = dostawa.ProdukcjaDW.kodtw;
                    poz.PozycjaFK.khnazwa = dostawa.ProdukcjaDW.khnazwa;
                    poz.PozycjaFK.iddw = dostawa.ProdukcjaDW.id_fk;
                    poz.PozycjaFK.idproddw = dostawa.ProdukcjaDW.id;
                    //poz.Pozycja.opis = dostawa.ProdukcjaDW.kod;
                    poz.PozycjaFK.nazwatw = dostawa.ProdukcjaDW.opistw;
                    poz.PozycjaFK.ilosc = dostawa.ProdukcjaDW.stan;
                    poz.PozycjaFK.cena = dostawa.ProdukcjaDW.cena;
                    poz.PozycjaFK.idtw = (int)dostawa.ProdukcjaDW.idtw;
                    poz.PozycjaFK.koddw = dostawa.ProdukcjaDW.kod;
                    //Obsługa wstawiania wigotnosci i czystosci z prob i partii
                    var query = (from f in db.PARTIE_FK
                                 from p in db.PARTIE
                                 where f.iddk_fki == dostawa.ProdukcjaDW.iddkpz
                                 where p.id == f.id_partii
                                 where f.kod_firmy == dostawa.ProdukcjaDW.kod_firmy
                                 select p).FirstOrDefault();
                    if (query != null)
                    {
                        poz.PozycjaFK.czystosc = query.czystosc;
                        poz.PozycjaFK.wilgotnosc = query.wilgotnosc;
                    }
                    //obsługa wstawiania informacji o wyrobie gptwym ponownie uzytym do produkcji
                    //MessageBox.Show(dostawa.ProdukcjaDW.iddkpz.ToString());
                    PRODDP surowiec_wg = Functions.PobierzPozycjePrzezDokument(dostawa.ProdukcjaDW.iddkpz);
                    if (surowiec_wg != null)
                    {
                        poz.PozycjaFK.czystosc = surowiec_wg.czystosc;
                        poz.PozycjaFK.wilgotnosc = surowiec_wg.wilgotnosc;
                        poz.PozycjaFK.frakcja = surowiec_wg.frakcja;
                        poz.PozycjaFK.opis = surowiec_wg.opis;
                        //MessageBox.Show(surowiec_wg.id_hm.ToString());
                    }
                    //poz.Pozycja.lp = (short)(ProdukcjaPozycjeRW.Where(p => p.Pozycja.idproddw > 0).ToList().Max(i => i.Pozycja.lp) + 1;
                    // poz.Pozycja.jm = dostawa.ProdukcjaDW.
                    _produkcjaPozycjeRW_FK.Add(poz);
                }
            }

            this.RaisePropertyChanged("ProdukcjaPozycjeRW_FK");
        }

        public void wstawPozycjeMG_DW(List<ProdukcjaMGDwVM> dostawy)
        {
           
            
        }


        protected void UstalLpDlaRW()
        {
            foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjeRW)
            {
                p.Pozycja.lp = 0;
            }

            foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjeRW)
            {
                if(p.Pozycja.iddw > 0)
                {
                    p.Pozycja.lp = ProdukcjaPozycjeRW.Where(x => x.Pozycja.iddw > 0).ToList().Max(i => i.Pozycja.lp) + 1;
                }
                
            }

            if (ProdukcjaPozycjeRW_FK != null)
            {
                foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjeRW_FK)
                {
                    p.PozycjaFK.lp = 0;
                }


                foreach (ProdukcjaPozycjeVM p in ProdukcjaPozycjeRW_FK)
                {
                    if (p.PozycjaFK.iddw > 0)
                    {
                        p.PozycjaFK.lp = ProdukcjaPozycjeRW_FK.Where(x => x.PozycjaFK.iddw > 0).ToList().Max(i => i.PozycjaFK.lp) + 1;
                    }

                }
                this.RaisePropertyChanged("ProdukcjaPozycjeRW_FK");
            }

        }

        public void wstawMonitPW()
        {
            if (SelectedPW != null)
            {
                if (ProdukcjaPozycjeMonit == null)
                {
                    ProdukcjaPozycjeMonit = new ObservableCollection<ProdukcjaPozycjeMonitVM>();
                }

                ProdukcjaPozycjeMonitVM monitVM = new ProdukcjaPozycjeMonitVM();
                monitVM.lp = (short)(ProdukcjaPozycjePW.IndexOf(SelectedPW));
                monitVM.ProdDPMonit.kodtw = SelectedPW.Pozycja.kodtw;
                monitVM.ProdDPMonit.data = DateTime.Now;
                monitVM.ProdDPMonit.godzina = DateTime.Now.ToShortTimeString();
                ProdukcjaPozycjeMonit.Add(monitVM);
                GetProdDP_MONIT();
                //this.RaisePropertyChanged("ProdukcjaPozycjeMonitTMP");
            }
        }

        public void wstawMaszynaMonit(ProdukcjaMaszynaVM maszyna)
        {
            ProdukcjaMaszynaParamMonitWindow dialog = new ProdukcjaMaszynaParamMonitWindow(maszyna);
            Messenger.Default.Send<ProdukcjaMaszynaVM>(maszyna);
            Nullable<bool> dialogResult = dialog.ShowDialog();
            
            if (dialogResult == true)
            {
                _maszyny = ProdukcjaInfo.getMaszyny();
                //maszyna.PobierzMaszynaMonit();

                MaszynaSel = Maszyny.Where(x => x.MaszynaPW.id == maszyna.MaszynaPW.id).FirstOrDefault();
                RaisePropertyChanged("Maszyny");
                RaisePropertyChanged("MaszynaSel");
               
            }
           // MessageBox.Show(maszyna.MaszynaPW.nazwa_maszyny);
        }

        public void edytujMaszynaMonit(ProdukcjaMaszynaParametrMonitVM monit)
        {

            ProdukcjaMaszynaParamMonitWindow dialog = new ProdukcjaMaszynaParamMonitWindow(monit);
            Messenger.Default.Send<ProdukcjaMaszynaParametrMonitVM>(monit);
            Nullable<bool> dialogResult = dialog.ShowDialog();

            if (dialogResult == true)
            {
                _maszyny = ProdukcjaInfo.getMaszyny();
                //maszyna.PobierzMaszynaMonit();

                MaszynaSel = Maszyny.Where(x => x.MaszynaPW.id == monit.MaszynaMonit.id_prod_maszyny_pw).FirstOrDefault();
                RaisePropertyChanged("Maszyny");
                RaisePropertyChanged("MaszynaSel");

            }
            // MessageBox.Show(maszyna.MaszynaPW.nazwa_maszyny);
        }

        public void usunMaszynaMonit(ProdukcjaMaszynaParametrMonitVM monit)
        {
            if (monit != null && GetConfirmation("Czy napewno usunąć? ", monit.MaszynaMonit.param_nazwa))
            {
                var item = db.PROD_MASZYNY_MONIT.Where(i => i.id == monit.MaszynaMonit.id).First();
                db.PROD_MASZYNY_MONIT.Remove(item);
                db.SaveChanges();
                db = new FZLEntities1();
                _maszyny = ProdukcjaInfo.getMaszyny();
                //maszyna.PobierzMaszynaMonit();

                MaszynaSel = Maszyny.Where(x => x.MaszynaPW.id == monit.MaszynaMonit.id_prod_maszyny_pw).FirstOrDefault();
                RaisePropertyChanged("Maszyny");
                RaisePropertyChanged("MaszynaSel");
            }
                //ProdukcjaMaszynaParamMonitWindow dialog = new ProdukcjaMaszynaParamMonitWindow(monit);
                //Messenger.Default.Send<ProdukcjaMaszynaParametrMonitVM>(monit);
                //Nullable<bool> dialogResult = dialog.ShowDialog();

          
            
            // MessageBox.Show(maszyna.MaszynaPW.nazwa_maszyny);
        }
    public bool GetConfirmation(string Message, string Caption)
    {
        return MessageBox.Show(Message,
                               Caption,
                               MessageBoxButton.OKCancel,
                               MessageBoxImage.Question,
                               MessageBoxResult.Cancel) == MessageBoxResult.OK;
    }

    public void wstawPozycjeTW(List<ProdukcjaTwVM> towary)
        {

            this.towary = towary;
            ProdukcjaPozycjeVM poz;
            bool jest = false;

            foreach (ProdukcjaTwVM towar in towary)
            {
                jest = false;
                /*
                if (ProdukcjaPozycjeRW.Count > 0)
                {

                    foreach (ProdukcjaPozycjeVM ppoz in ProdukcjaPozycjePW)
                    {

                        if (ppoz.Pozycja.idtw == towar.ProdukcjaTW.id_fk)
                        {
                            jest = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                */
                if (!jest)
                {
                    poz = new ProdukcjaPozycjeVM();
                    poz.Pozycja.typ_produktu = "WG";
                    //poz.Pozycja.lp = 
                    poz.Pozycja.kodtw = towar.ProdukcjaTW.kod;
                    poz.Pozycja.nazwatw = towar.ProdukcjaTW.nazwa;
                    //poz.Pozycja.iddw = towar.ProdukcjaTW.id_fk;
                    //poz.Pozycja.idproddw = towar.ProdukcjaTW.id;
                    //poz.Pozycja.opis = towar.ProdukcjaTW.kod;
                    poz.Pozycja.nazwatw = towar.ProdukcjaTW.nazwa;
                    poz.Pozycja.jm = towar.ProdukcjaTW.jm;
                    //poz.Pozycja.ilosc = towar.ProdukcjaDW.iloscdosp;
                    poz.Pozycja.idtw = (int)towar.ProdukcjaTW.id_fk;
                    poz.Pozycja.kodpaskowy = towar.ProdukcjaTW.kodpaskowy;
                    //poz.Pozycja.koddw = dostawa.ProdukcjaDW.kod;
                    // poz.Pozycja.jm = dostawa.ProdukcjaDW.
                    ProdukcjaPozycjePW.Add(poz);
                }
            }

            this.RaisePropertyChanged("ProdukcjaPozycjePW");
        }

        

        public void wstawPozycjeSYM(List<ProdukcjaTwVM> towary)
        {

            this.towary = towary;
            ProdukcjaPozycjeVM poz;
            bool jest = false;
            if (ProdukcjaPozycjeSYM == null)
            {
                _produkcjaPozycjeSYM = new ObservableCollection<ProdukcjaPozycjeVM>();
            }

            foreach (ProdukcjaTwVM towar in towary)
            {
                jest = false;
                /*
                if (ProdukcjaPozycjeRW.Count > 0)
                {

                    foreach (ProdukcjaPozycjeVM ppoz in ProdukcjaPozycjePW)
                    {

                        if (ppoz.Pozycja.idtw == towar.ProdukcjaTW.id_fk)
                        {
                            jest = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                */
                if (!jest)
                {
                    poz = new ProdukcjaPozycjeVM();
                    //poz.PozycjaSYM.typ_produktu = "WG";
                    //poz.Pozycja.lp = 
                    poz.PozycjaSYM.kodtw = towar.ProdukcjaTW.kod;
                    poz.PozycjaSYM.nazwatw = towar.ProdukcjaTW.nazwa;
                    //poz.Pozycja.iddw = towar.ProdukcjaTW.id_fk;
                    //poz.Pozycja.idproddw = towar.ProdukcjaTW.id;
                    //poz.Pozycja.opis = towar.ProdukcjaTW.kod;
                    poz.PozycjaSYM.nazwatw = towar.ProdukcjaTW.nazwa;
                    poz.PozycjaSYM.jm = towar.ProdukcjaTW.jm;
                    //poz.Pozycja.ilosc = towar.ProdukcjaDW.iloscdosp;
                    poz.PozycjaSYM.idtw = (int)towar.ProdukcjaTW.id_fk;
                    poz.PozycjaSYM.kodpaskowy = towar.ProdukcjaTW.kodpaskowy;
                    //poz.Pozycja.koddw = dostawa.ProdukcjaDW.kod;
                    // poz.Pozycja.jm = dostawa.ProdukcjaDW.
                    ProdukcjaPozycjeSYM.Add(poz);
                }
            }

            this.RaisePropertyChanged("ProdukcjaPozycjeSYM");
        }

        public void wstawPozycjeTW_FK(List<ProdukcjaTwVM> towary)
        {

            this.towary = towary;
            ProdukcjaPozycjeVM poz;
            bool jest = false;
            if(ProdukcjaPozycjePW_FK == null)
            {
                ProdukcjaPozycjePW_FK = new ObservableCollection<ProdukcjaPozycjeVM>();
            }
            foreach (ProdukcjaTwVM towar in towary)
            {
                jest = false;
                /*
                if (ProdukcjaPozycjeRW.Count > 0)
                {

                    foreach (ProdukcjaPozycjeVM ppoz in ProdukcjaPozycjePW)
                    {

                        if (ppoz.Pozycja.idtw == towar.ProdukcjaTW.id_fk)
                        {
                            jest = true;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                */
                if (!jest)
                {
                    poz = new ProdukcjaPozycjeVM();
                    poz.PozycjaFK.typ_produktu = "WG";
                    //poz.Pozycja.lp = 
                    poz.PozycjaFK.kodtw = towar.ProdukcjaTW.kod;
                    poz.PozycjaFK.nazwatw = towar.ProdukcjaTW.nazwa;
                    //poz.Pozycja.iddw = towar.ProdukcjaTW.id_fk;
                    //poz.Pozycja.idproddw = towar.ProdukcjaTW.id;
                    //poz.Pozycja.opis = towar.ProdukcjaTW.kod;
                    poz.PozycjaFK.nazwatw = towar.ProdukcjaTW.nazwa;
                    poz.PozycjaFK.jm = towar.ProdukcjaTW.jm;
                    //poz.Pozycja.ilosc = towar.ProdukcjaDW.iloscdosp;
                    poz.PozycjaFK.idtw = (int)towar.ProdukcjaTW.id_fk;
                    poz.PozycjaFK.kodpaskowy = towar.ProdukcjaTW.kodpaskowy;
                    //poz.Pozycja.koddw = dostawa.ProdukcjaDW.kod;
                    // poz.Pozycja.jm = dostawa.ProdukcjaDW.
                    ProdukcjaPozycjePW_FK.Add(poz);
                }
            }

            this.RaisePropertyChanged("ProdukcjaPozycjePW_FK");
        }

        public void WczytajPozycjeDoFK()
        {
            ProdukcjaPozycjeVM poz;
            if (ProdukcjaPozycjePW.Count > 0)
            {
                
                ProdukcjaPozycjePW_FK = new ObservableCollection<ProdukcjaPozycjeVM>();
                foreach (ProdukcjaPozycjeVM pozPWMG in ProdukcjaPozycjePW)
                {
                    poz = new ProdukcjaPozycjeVM();
                    poz.PozycjaFK.typ_produktu = "WG";
                    //poz.Pozycja.lp = 
                    poz.PozycjaFK.kodtw = pozPWMG.Pozycja.kodtw;
                    poz.PozycjaFK.nazwatw = pozPWMG.Pozycja.nazwatw;
                    poz.PozycjaFK.ilosc = pozPWMG.Pozycja.ilosc;
                    poz.PozycjaFK.id_partii = pozPWMG.Pozycja.id_partii;
                    poz.PozycjaFK.nr_partii = pozPWMG.Pozycja.nr_partii;
                    poz.PozycjaFK.frakcja = pozPWMG.Pozycja.frakcja;
                    poz.PozycjaFK.idtw = pozPWMG.Pozycja.idtw;
                    poz.PozycjaFK.czystosc = pozPWMG.Pozycja.czystosc;
                    poz.PozycjaFK.wilgotnosc = pozPWMG.Pozycja.wilgotnosc;

                    poz.PozycjaFK.jm = pozPWMG.Pozycja.jm;
                   
                    ProdukcjaPozycjePW_FK.Add(poz);
                }
                this.RaisePropertyChanged("ProdukcjaPozycjePW_FK");
            }
            
        } 


        public void getProdukcjaVM(ProdukcjaVM prodvm)
        {

            ProdukcjaInfo = prodvm;
            WgWycenaTryb = true;
            ProdukcjaInfo.Produkcja.wycena_typ = 1;



            if (prodvm.Produkcja.id > 0)
            {
                db = new FZLEntities1();
                ProdukcjaInfo.Produkcja = db.PROD.FirstOrDefault(i => i.id == prodvm.Produkcja.id);
                ProdukcjaInfo.IsNew = false;

                if (ProdukcjaInfo.Produkcja.akceptowano == 1)
                    IsChecked = true;
                else
                    IsChecked = false;

                if (ProdukcjaInfo.Produkcja.wycena_typ == 1)
                {
                    WgWycenaTryb = true;
                    IsShowPlanPrice = Visibility.Collapsed;
                }
                else
                {
                    WgWycenaTryb = false;
                    IsShowPlanPrice = Visibility.Visible;
                }
                if (ProdukcjaInfo.Produkcja.zakonczono == 1 && user.admin != 1 && LoginViewViewModel.sprUprawnienie("produkcja_cansave") != true)
                {
                    IsFinish = true;
                    IsCanSave = false;
                    IsFinishDate = false;
                }

                else  
                {
                    if (ProdukcjaInfo.Produkcja.zakonczono != 1)
                        IsFinish = false;
                    else
                        IsFinish = true;
                    IsCanSave = true;
                }

                if(ProdukcjaInfo.Produkcja.czesciowa != 1)
                {
                    IsPartial = false;
                }
                else
                {
                    IsPartial = true;
                    if(ProdukcjaInfo.DokumentyRW().Count()> 0)
                    {
                        IsCanRW = false;
                    }
                }

                if (ProdukcjaInfo.Produkcja.zerowa != 1)
                {
                    IsZero = false;
                }
                else
                {
                    IsZero = true;
                }

                

                RaisePropertyChanged("ProdukcjaInfo");
            }
            else
            {
                ProdukcjaInfo = new ProdukcjaVM();
                ProdukcjaInfo.Produkcja.data = (DateTime?)DateTime.Now;
                ProdukcjaInfo.Produkcja.zakonczono = 0;
                ProdukcjaInfo.Produkcja.akceptowano = 0;
                ProdukcjaInfo.Produkcja.wycena_typ = 1;
                ProdukcjaInfo.Produkcja.data = (DateTime?)DateTime.Now;
                ProdukcjaInfo.Produkcja.datadk = (DateTime?)DateTime.Now;
                _wgWycenaTryb = true;
                IsShowPlanPrice = Visibility.Collapsed;
                IsFinish = false;
                isCanSave = true;
                
                //RaisePropertyChanged("IsPlanPrice");
                WgWycenaTryb = true;

                //RaisePropertyChanged("IsFinish");
                //

               RaisePropertyChanged("IsCanSave");
                RaisePropertyChanged("ProdukcjaInfo");
            }
            isCanPrice = LoginViewViewModel.sprUprawnienie("produkcja_canprice");
            IsPlanPrice = false;
           // if (OpakowaniaPowiazania == null)
            //    OpakowaniaPowiazania = new List<PROD_PRODMGPW>();
            
            
            this.PobierzMgazyny();
            this.GetWaluty();
            this.GetProdDP_RW();
            this.GetProdDP_RW_FK();
            this.GetProdDP_SYM();
            this.PobierzOpakowania();
            this.PobierzOpakowania2();
            this.PobierzPowiazaniaZmagazynemProdukcyjnym();
            this.PobierzDostawyProdMgPw();
            this.GetProdDP_PW();
            this.GetProdDP_PW_FK();
            this.GetProdDP_SYM();
            this.WczytajPozycjePW_MONIT();
      
            this.PobierzZapisaneLinie();
            Maszyny = ProdukcjaInfo.getMaszyny();
            foreach(ProdukcjaMaszynaVM m in Maszyny)
            {
                m.PobierzMaszynaMonit();
            }
            RaisePropertyChanged("Maszyny");

            var wart_rw = (from p in this.ProdukcjaPozycjeRW
                           select (p.Pozycja.ilosc * p.Pozycja.cena)).Sum();
            //MessageBox.Show(wart_rw.ToString());
            WartoscRW = (float)wart_rw;
            RaisePropertyChanged("WartoscRW");
        }

        public void UstalIDPartii(String nr_partii)
        {
            
            Match match = Regex.Match(nr_partii, @"F([A-Za-z0-9\-]+)$",
                RegexOptions.IgnoreCase);

            // Here we check the Match instance.
            if (match.Success)
            {
                // Finally, we get the Group value and display it.
                string key = match.Groups[1].Value;
                Console.WriteLine(key);
            }
        }
        /**
         * Pobiera Linie Produkcyjne
         */
        public void PobierzLinie()
        {
            Linie = db.PROD_LINIE.Where(x => x.kod_firmy == ProdukcjaInfo.Produkcja.kod_firmy).ToList();
            RaisePropertyChanged("Linie");
        }


        //Tworzy powiązanie dostawy z magazynu produkcyjnego z pozycja RW
        public void DodajDoPowiazanZMagazynemProd(int lp,ProdukcjaMGDwVM dwmg)
        {
            ProdukcjaMGDwVM val;
            if (dostawyMG.TryGetValue(lp, out val) ) 
                dostawyMG[lp] = dwmg;
            else
                dostawyMG.Add(lp, dwmg);
           
        }

        /**
         * Pobiera informacje o pozycjach RW ujętych w magazynie produkcyjnym
         * 
         */
        public void PobierzPowiazaniaZmagazynemProdukcyjnym()
        {
            dostawyMG = new Dictionary<int, ProdukcjaMGDwVM>();
            if (DostawyProdMGPowiazania == null)
            {
                DostawyProdMGPowiazania = new List<PROD_PRODMGPW>();
            }
            //bool jest=false;
            if (ProdukcjaPozycjeRW.Count > 0)
            {
                foreach(ProdukcjaPozycjeVM prw in ProdukcjaPozycjeRW)
                {
                    if (prw.Pozycja.idmgdw != null)
                    {
                        if (dostawyMG == null)
                            dostawyMG = new Dictionary<int, ProdukcjaMGDwVM>();
                        //sprawdz czy

                        var obj = db.PROD_MGDW.Where(i => i.id == prw.Pozycja.idmgdw).First();

                        // dostawyMG.Clear();
                        // jest = true;
                        DodajDoPowiazanZMagazynemProd(ProdukcjaPozycjeRW.IndexOf(prw), new ProdukcjaMGDwVM { ProdukcjaDW = obj, IsNew = false });
                    }
                }
            }
        }
    }
    interface IMessageBoxManager
    {
        MessageBoxResult ShowMessageBox(string text, string title,
                                        MessageBoxButton buttons);
    }
    /**
     * Anty remiugiusz zeby nie mogł zmineic częściowego przyjęcia
     */ 
    public class CellEnabled : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if((value != null && ((int)value > 0)) && LoginViewViewModel.sprUprawnienie("produkcja_cansave") != true)
                    return false;
                
                else
                    return true;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    public class YesNoToBooleanConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            byte inValue = System.Convert.ToByte(value);
            string outValue = (inValue == 1) ? "Yes" : "No";
            return outValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            switch (value.ToString().Trim().ToLower())
            {
                case "yes":
                    return 1;
                case "no":
                    return 0;
                default:
                    return 0;
            }
        }
    }

}
