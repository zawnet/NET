using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HERBS_PRODUKCJA.ViewModel.RowVM;
using HERBS_PRODUKCJA.Support;
using System.Collections.ObjectModel;
using HERBS_PRODUKCJA.Helpers;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaMagazynVM : VMBase
    {
        public PROD_MG ProdukcjaMG { get; set; }
        public ProdukcjaKhVM ProdKH {get;set;}
        public ObservableCollection<PROD_MZ> PozycjeMZ { get; set; }
        public string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        private List<String> _typyproddp;
        



        UZYTKOWNICY user = (UZYTKOWNICY)App.Current.Properties["UserLoged"];

        public ProdukcjaMagazynVM()
        {
            
            this.ProdukcjaMG = new PROD_MG();
            this.PozycjeMZ = new ObservableCollection<PROD_MZ>();


            this.ProdukcjaMG.kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
        }

        public int? pobierzKolejnyNumerSerii(int okres, string typ)
        {
            int? serianr;
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                var query = (from u in ctx.PROD_MG
                                    where u.kod_firmy == ProdukcjaMG.kod_firmy
                                    where u.okres == okres
                                    where u.typ_dk == typ
                                    select u.serial).ToList();
                
                        

                if (query.Count > 0)
                {
                    serianr = query.Max() + 1;
                }
                else
                {
                    serianr = 1;
                }
            }
            return serianr;
        }

        public int WystawDokumentPrzychodowy(List<PROD_PRODMGPW> pozycje, string typdk)
        {
            KH kh;
            //string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
            using (FZLEntities1 db = new FZLEntities1())
            {
                PROD_MGPW mgpw;
                PROD_MGDW mgdw;
                PROD_MG prodmg = new PROD_MG();
                PROD_MZ prodmz;
                prodmg.typ_dk = typdk;
                kh = (from k in db.KH
                      where k.kod_firmy == kod_firmy
                      where k.zew_id == 1
                      select k).First();

                if (kh == null)
                    return 0;

                //Kontrahent
                prodmg.data = System.DateTime.Now;
                prodmg.khid = (int)kh.id;
                prodmg.khnazwa = kh.nazwa;
                prodmg.khkod = kh.kod;
                prodmg.khnazwa = kh.nazwa;
                prodmg.khnip = kh.nip;
                prodmg.khdom = kh.dom;
                prodmg.khlokal = kh.lokal;
                prodmg.khmiasto = kh.miejscowosc;
                prodmg.khkodpocz = kh.kodpocz;
                prodmg.khadres = kh.ulica;
                prodmg.kod_firmy = kod_firmy;

                if (!(prodmg.id > 0))
                {
                    prodmg.okres = int.Parse(prodmg.data.Value.Year.ToString());
                    prodmg.serial = (int)pobierzKolejnyNumerSerii(prodmg.okres, prodmg.typ_dk);
                    prodmg.kod = prodmg.typ_dk + "/" + prodmg.serial.ToString() + "/" + prodmg.okres.ToString();
                    prodmg.osoba_mod = user.nazwa;
                    prodmg.data_mod = DateTime.Now;
                    prodmg.osoba = user.nazwa;

                    db.PROD_MG.Add(prodmg);
                    db.SaveChanges();
                }

                //Wstaw pozycje dokumentu
                foreach (PROD_PRODMGPW mzpw in pozycje)
                {
                    var twdw = db.PROD_MGDW.Where(x => x.id == mzpw.id_prodmgdw).First();
                    prodmz = new PROD_MZ();
                    prodmz.id_prodmg = prodmg.id;
                    prodmz.idtw = twdw.idtw;

                    prodmz.kod = twdw.kodtw;
                    prodmz.opis = twdw.nazwatw;
                    prodmz.ilosc = mzpw.ilosc;
                    prodmz.nr_partii = twdw.PROD_MZ.nr_partii;

                    db.PROD_MZ.Add(prodmz);
                    db.SaveChanges();
                    var obj = db.PROD_PRODMGPW.Where(x => x.id == mzpw.id).ToList();
                    if (obj != null && obj.Count() > 0)
                        obj.First().id_dkprodmg = prodmz.id;


                    
                }

            }
                return 0;

        }



        public int WystawDokumentPrzychodowy(List<PRODDP> pozycje, string typdk)
        {
            KH kh;
            //string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
            PROD zlec ;
            PRODDP dp;
            PROD_MGPW mgpw;
            PROD_MGDW mgdw = new PROD_MGDW();
            PROD_MG prodmg = new PROD_MG();
            PROD_MZ prodmz;

            PROD_PRODMGPW prodmgpw;

            dp = pozycje.ElementAt(0);   
            
            using (FZLEntities1 db = new FZLEntities1())
            {
                zlec = (from pr in db.PROD
                        where pr.id == dp.id_prod
                        select pr).First(); 
                    

                prodmg.typ_dk = typdk;
                kh = (from k in db.KH
                      where k.kod_firmy == kod_firmy
                      where k.zew_id == 1
                      select k).First();

                if (kh == null)
                    return 0;
                
                //Kontrahent
                prodmg.data = System.DateTime.Now;
                prodmg.khid = (int)kh.id;
                prodmg.khnazwa = kh.nazwa;
                prodmg.khkod = kh.kod;
                prodmg.khnazwa = kh.nazwa;
                prodmg.khnip = kh.nip;
                prodmg.khdom = kh.dom;
                prodmg.khlokal = kh.lokal;
                prodmg.khmiasto = kh.miejscowosc;
                prodmg.khkodpocz = kh.kodpocz;
                prodmg.khadres = kh.ulica;
                prodmg.kod_firmy = kod_firmy;
                prodmg.opis = zlec.nazwa;

                if (!(prodmg.id > 0))
                {
                    prodmg.okres = int.Parse(prodmg.data.Value.Year.ToString());
                    prodmg.serial = (int)pobierzKolejnyNumerSerii(prodmg.okres, prodmg.typ_dk);
                    prodmg.kod = prodmg.typ_dk + "/" + prodmg.serial.ToString() + "/" + prodmg.okres.ToString();
                    prodmg.osoba_mod = user.nazwa;
                    prodmg.data_mod = DateTime.Now;
                    prodmg.osoba = user.nazwa;
                    
                    db.PROD_MG.Add(prodmg);
                    db.SaveChanges();
                }

                //Wstaw pozycje dokumentu
                foreach (PRODDP poz in pozycje)
                {
                   
                    prodmz = new PROD_MZ();
                    prodmz.id_prodmg = prodmg.id;
                    prodmz.idtw = poz.idtw;
                    prodmz.typ_produktu = poz.typ_produktu;
                    prodmz.kod = poz.kodtw;
                    prodmz.opis = poz.nazwatw;
                    prodmz.ilosc = poz.ilosc;
                    prodmz.nr_partii = poz.nr_partii;

                    db.PROD_MZ.Add(prodmz);
                    db.SaveChanges();
                    mgdw = ZapiszDostawe(prodmz);
                    //zapisz powizanie ze zleceniem produkcyjnym
                    prodmgpw = new PROD_PRODMGPW();
                    prodmgpw.id_proddp = poz.id;
                    prodmgpw.id_prodmgdw = mgdw.id;
                    prodmgpw.id_dkprodmg = prodmz.id;
                    prodmgpw.ilosc = (double)prodmz.ilosc;
                    
                    db.PROD_PRODMGPW.Add(prodmgpw);
                    db.SaveChanges();
                   

                   
                }
                return prodmg.id;
            }
            return 0;

        }

        /**
         * Funkcja realizuje obsługę wystawiania dokumentów rozchodowych w magazynie produkcyjnym
         * na podstawie rozchodu ze zlecenia produkcyjnego
         */
        public int WystawDokumentRozchodowy(List<PROD_PRODMGPW> pozycje, string typdk)
        {
            KH kh;
            PROD zlec;
            PRODDP dp;
            PROD_PRODMGPW prod_pow;
            //string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();

            prod_pow = pozycje.ElementAt(0);
            dp = prod_pow.PRODDP;
            
            using (FZLEntities1 db = new FZLEntities1())
            {
                zlec = (from pr in db.PROD
                        where pr.id == dp.id_prod
                        select pr).First();
                PROD_MGPW mgpw;
                PROD_MGDW mgdw;
                PROD_MG prodmg = new PROD_MG();
                PROD_MZ prodmz;
                prodmg.typ_dk = typdk;
                kh = (from k in db.KH
                      where k.kod_firmy == kod_firmy
                      where k.zew_id == 1
                      select k).First();

                if (kh == null)
                    return 0;
                //db.KH.Where(k => k.kod_firmy == System.Windows.Application.Current.Properties["kod_firmy"].ToString() && k.zew_id == 1).FirstOrDefault();
                //Kontrahent
                prodmg.data = System.DateTime.Now;
                prodmg.khid = (int)kh.id;
                prodmg.khnazwa = kh.nazwa;
                prodmg.khkod = kh.kod;
                prodmg.khnazwa = kh.nazwa;
                prodmg.khnip = kh.nip;
                prodmg.khdom = kh.dom;
                prodmg.khlokal = kh.lokal;
                prodmg.khmiasto = kh.miejscowosc;
                prodmg.khkodpocz = kh.kodpocz;
                prodmg.khadres = kh.ulica;
                prodmg.kod_firmy = kod_firmy;
                prodmg.opis = zlec.nazwa;

                if(!(prodmg.id > 0))
                {
                    prodmg.okres = int.Parse(prodmg.data.Value.Year.ToString());
                    prodmg.serial = (int)pobierzKolejnyNumerSerii(prodmg.okres, prodmg.typ_dk);
                    prodmg.kod = prodmg.typ_dk + "/" + prodmg.serial.ToString() + "/" + prodmg.okres.ToString();
                    prodmg.osoba_mod = user.nazwa;
                    prodmg.data_mod = DateTime.Now;
                    prodmg.osoba = user.nazwa;
                   
                    db.PROD_MG.Add(prodmg);
                    db.SaveChanges();
                }

                //Wstaw pozycje dokumentu
                foreach(PROD_PRODMGPW mzpw in pozycje)
                {
                    var twdw = db.PROD_MGDW.Where(x => x.id == mzpw.id_prodmgdw).First();
                    prodmz = new PROD_MZ();   
                    prodmz.id_prodmg = prodmg.id;
                    prodmz.idtw = twdw.idtw;
                    prodmz.nr_partii = twdw.nr_partii;
                    prodmz.kod = twdw.kodtw;
                   
                    prodmz.opis = twdw.nazwatw;
                    prodmz.ilosc = mzpw.ilosc;
                    prodmz.nr_partii = twdw.PROD_MZ.nr_partii;
                    prodmz.typ_produktu = mzpw.PRODDP.typ_produktu;
                    db.PROD_MZ.Add(prodmz);
                    db.SaveChanges();
                    var obj = db.PROD_PRODMGPW.Where(x => x.id == mzpw.id).ToList();
                    if(obj != null && obj.Count() > 0)
                        obj.First().id_dkprodmg = prodmz.id;
                   

                    //utowrz powiazanie
                    mgpw = new PROD_MGPW();
                    mgpw.id_prodmgdw = (int)mzpw.id_prodmgdw;
                    mgpw.id_prodmz = prodmz.id;
                    mgpw.ilosc = mzpw.ilosc;
                    //mgpw.
                    db.PROD_MGPW.Add(mgpw);
                    db.SaveChanges();
                }

                //Uzggodnij statny dostaw dostawy
                foreach (PROD_PRODMGPW mz in pozycje)
                {
                    mgdw = db.PROD_MGDW.Where(x => x.id == mz.PROD_MGDW.id).First();
                    mgdw.stan = mgdw.iloscpz - SumujRezerwacjeDostawy(mgdw.id);
                    mgdw.iloscdost = mgdw.stan;
                    //mgdw.iloscprod += mz.ilosc;

                    db.SaveChanges();
                }
                return prodmg.id;

            }
            
               
            return 0;
        }
        public double SumujRezerwacjeDostawy(int iddw)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {

                var suma = (from d in db.PROD_MGPW

                            where d.id_prodmgdw == iddw



                            select d.ilosc).Sum();
                if (suma == null)
                    return 0;
                else
                    return (double)suma;
            }
        }
        //Funkcja realizuje przkeazanie wyrobow ktotych z produkcji na magazyn produkcyjny
        //Funkcja zwraca identyfikator dokumentu w przypadku poprawnego wygenerowania
        // 0 - jezeli dokment jest juz wystwawiony

        public int PrzekazNaMagazynWG(int id_prod)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                var query = (from c in db.PRODDP
                             where c.id_prod == id_prod
                             where c.rodzaj_dk == "PW"
                             select c).ToList();
                bool jest = false;

                if(query.Count() > 0)
                {
                   foreach(PRODDP dp in query)
                    {
                        //sprawdz czy dokument został juz wystawiony
                        var query2 = (from p in db.PROD_PRODMGPW
                                      where p.id_proddp == dp.id
                                      where p.id_dkprodmg > 0
                                      select p).ToList();
                        if(query2.Count() > 0)
                        {
                            jest = true;
                            return 0;
                            //juz istnieje wystawiony dokument magazynowy dla tego zlecenia
                        }
                        else
                        {
                           
                        }
                    }
                   if(!jest)
                    {
                        //nie ma dokumentu magazynowego zatem mozna wygenerowac nowe dokumenty
                        return WystawDokumentPrzychodowy(query, "PWWG");
                    }
                } 
           }
            return 0;
        }



        //Zapisuje dostawe w tabeli dostaw towarów PROD_MGDW
        public PROD_MGDW ZapiszDostawe(PROD_MZ poz)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {

                //Znajdz innformacje o odstawie dla danej pozycji
                PROD_MGDW dw = (from d in db.PROD_MGDW
                                where d.idpozpz == poz.id
                                select d).FirstOrDefault();
                //dostawa istnieje uaktualnij ją
                if (dw == null || dw.id == null)
                {
                    dw = new PROD_MGDW();
                }
                dw.kod_firmy = kod_firmy;
                dw.idpozpz = poz.id;
                dw.idkh = poz.PROD_MG.khid;
                dw.khkod = poz.PROD_MG.khkod;
                dw.khnazwa = poz.PROD_MG.khnazwa;
                dw.kodtw = poz.kod;
                dw.data = poz.PROD_MG.data;
                dw.nazwatw = poz.opis;
                dw.idtw = poz.idtw;
                dw.ilosc = poz.ilosc;
                dw.iloscdost = poz.ilosc;
                dw.iloscpz = poz.ilosc;
                dw.stan = poz.ilosc;
                dw.nr_partii = poz.nr_partii;
                dw.kod = poz.PROD_MG.kod;
                dw.typdw = poz.PROD_MG.typ_dk;
                dw.typ_produktu = poz.typ_produktu;

                if (!(dw.id > 0))
                {
                    db.PROD_MGDW.Add(dw);
                }
                db.SaveChanges();
                return dw;

            }
        }

    }
}
