using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HERBS_PRODUKCJA.ViewModel.RowVM;

namespace HERBS_PRODUKCJA
{

    public class ProdukcjaVM : VMBase
    {
        public PROD Produkcja { get; set; }
        public ObservableCollection<PRODDP> ProdDP { get; set; }
        UZYTKOWNICY user = (UZYTKOWNICY)App.Current.Properties["UserLoged"];

        public ProdukcjaVM()
        {
            Produkcja = new PROD();
            Produkcja.kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
            //Produkcja.kod_firmy = "FZL_sp";
            Produkcja.typ = 1;
            Produkcja.osobne_pw = 1;
        }

        public ProdukcjaVM(int idprod)
        {
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                this.Produkcja = ctx.PROD.Where(x => x.id == idprod).FirstOrDefault();
            }
        }
        public string noGroup
        {
            get
            {
                return "Total";
            }
        }

        public int? pobierzKolejnyNumerSerii(int okres)
        {
            int? serianr;
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                    
                   var query =  (from u in ctx.PROD
                                    where u.kod_firmy == Produkcja.kod_firmy
                                    where u.okres == okres
                                    where u.typ == 1
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
        public List<PRODDP> PozycjeRW()
        {
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                return (from c in ctx.PRODDP
                        where c.id_prod == Produkcja.id
                        where c.rodzaj_dk == "RW"
                        select c).ToList();

            }
        }

        public List<PRODDP> PozycjePW()
        {
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                return ctx.PRODDP.Where(i => (i.id_prod == this.Produkcja.id) && (i.rodzaj_dk == "PW")).ToList();
            }
        }
        public List<PRODDK> DokumentyRW()
        {
            return Produkcja.PRODDK.Where(i => i.typ_dk == "RW").ToList();
        }
        public List<PRODDK> DokumentyPW()
        {
            return Produkcja.PRODDK.Where(i => i.typ_dk == "PW").ToList();
        }
        public List<PROD_PW> ProdPW()
        {
            return Produkcja.PROD_PW.ToList();
        }

        public void ZlaczDo(PROD prod)
        {
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                Produkcja = ctx.PROD.Where(i => i.id == Produkcja.id).FirstOrDefault();
            }

            PrzeniesRW(prod);
            UsunPowiazania(Produkcja.id);
            ZapiszPowiazania(prod);
            PrzeniesPW(prod);

            WstawDoZlaczen(prod);
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                Produkcja = ctx.PROD.Where(i => i.id == Produkcja.id).FirstOrDefault();
                Produkcja.status = 4;
                ctx.SaveChanges();
            }

        }
        /**
         * 
         * Przenosi pozycje RW ze starego zlednia do nowego zlecenia
         * 
         */
        public void PrzeniesRW(PROD prod)
        {
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                PRODDP proddp;
                foreach (PRODDP poz in PozycjeRW())
                {
                    proddp = new PRODDP();
                    proddp = poz;
                    proddp.id = 0;
                    proddp.id_prod = prod.id;
                    ctx.PRODDP.Add(proddp);
                    ctx.SaveChanges();
                    //PrzeniesPowiazanie(poz, proddp);
                }

            }
        }
        /**
         * Przenosi pozycje PW ze starego zlednia do nowego zlecenia
         * 
         */
        public void PrzeniesPW(PROD prod)
        {

            PRODDP proddp;
            foreach (PRODDP poz in this.PozycjePW())
            {
                using (FZLEntities1 ctx = new FZLEntities1())
                {
                    proddp = poz;
                    proddp.id = 0;
                    proddp.id_prod = prod.id;
                    ctx.PRODDP.Add(proddp);
                    ctx.SaveChanges();
                }

            }

        }
        public void PrzeniesParam(PROD prod)
        {

        }
        /**
         * Przenosi powiązanie pozycji starego zlecenia do powiazania z pozycją w nowym zleceniu
         * 
         */
        public void ZapiszPowiazania(PROD prod)
        {
            var pozycje = new List<PRODDP>();
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                pozycje = ctx.PRODDP.Where(x => x.id_prod == prod.id && x.rodzaj_dk=="RW").ToList();
                
            }
            foreach(PRODDP poz in pozycje)
            {
                if(poz.iddw > 0)
                    RezerwujDostawe(poz);
            }
        }


        /**
         * Usuwa powiązania ze starego zlecnia
         * 
         */
        public void UsunPowiazania(int id_prod)
        {
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                ctx.PROD_PW.RemoveRange(ctx.PROD_PW.Where(x => x.id_prod == id_prod));
                ctx.SaveChanges();
            }
        }
        /**
         * Wstawia informacje o polaczeni zlecenia z innym zleceniem (prod2)
         * 
         */
        public void WstawDoZlaczen(PROD prod)
        {
            using (FZLEntities1 ctx = new FZLEntities1())
            {
                PROD_ZL prodzl = new PROD_ZL();
                prodzl.id1 = Produkcja.id;
                prodzl.id2 = prod.id;
                prodzl.opis = "";
                prodzl.osoba = user.nazwa;
                prodzl.data = DateTime.Now.Date;

                ctx.PROD_ZL.Add(prodzl);
                ctx.SaveChanges();
            }

        }

        public void RezerwujDostawe(PRODDP proddp)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {

                var item = db.PROD_PW.FirstOrDefault(i => i.id_proddp == proddp.id);
                var item2 = db.PROD_HMDW.FirstOrDefault((System.Linq.Expressions.Expression<Func<PROD_HMDW, bool>>)(i => i.id == proddp.idproddw));
                var item3 = db.PRODDP.FirstOrDefault(i => i.id == proddp.id);
                //item3.lp = lp;


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
                        prodpw.id_prod = proddp.id_prod;
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

        public List<ProdukcjaMaszynaVM> getMaszyny()
        {
            List<ProdukcjaMaszynaVM> maszyny = new List<ProdukcjaMaszynaVM>();
           
            using (FZLEntities1 db = new FZLEntities1())
            {
               
                var query = (from c in db.PROD_MASZYNY_PW
                             where c.id_prod == Produkcja.id
                             select c).ToList();
                maszyny.Clear();
                foreach (PROD_MASZYNY_PW m in query)
                {
                    ProdukcjaMaszynaVM Masz = new ProdukcjaMaszynaVM();
                    Masz.MaszynaPW = m;
                    Masz.PobierzMaszynaMonit();
                    maszyny.Add(Masz); 
                }
                
            }
            return maszyny;
        }
        /*
         *Wylicza na podstawie manszruty i monitorowania czasu pracy koszty produkcji
         * adekawatnie dla wpisanej ilosci wyrobu gotowego 
         */
        public double WyliczKosztyProdukcji()
        {
            string startTime = "0:00 AM";
            string endTime = "0:00 PM";
            double? koszt_pracy, koszt_osoba;
            double koszt_dla_zlecenia = 0.00;
            PROD_MASZYNY_KOSZTY maszyna_koszt;
            using (FZLEntities1 db = new FZLEntities1())
            {
                var maszyny_monits = (from c in db.PROD_MASZYNY_MONIT
                                      where c.id_prod == this.Produkcja.id
                                      select c).ToList();
                var maszyny_pw = (from c in db.PROD_MASZYNY_PW
                                  where c.id_prod == this.Produkcja.id
                                  select c).ToList();

                foreach(PROD_MASZYNY_MONIT m in maszyny_monits)
                {
                    if (m.param_nazwa == "Czas pracy" && m.rozpoczecie_data != null && m.zakonczenie_data != null)
                    {
                        startTime = m.rozpoczecie_data.Value.ToShortDateString() + " " +m.rozpoczecie_godzina;
                        endTime = m.zakonczenie_data.Value.ToShortDateString() + " " +m.zakonczenie_godzina;
                        if (DateTime.Parse(endTime) > DateTime.Parse(startTime) )
                        {
                            koszt_pracy = 0;
                            TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
                            if (m.PROD_MASZYNY_PW.PROD_MASZYNY.mocKW > 0 && m.PROD_MASZYNY_PW.PROD_MASZYNY.cena_czas > 0)
                            {
                                koszt_pracy = (m.PROD_MASZYNY_PW.PROD_MASZYNY.mocKW * m.PROD_MASZYNY_PW.PROD_MASZYNY.cena_czas) * duration.TotalHours;
                            }
                            if (m.ilosc_pracownikow > 0)
                            {

                                koszt_osoba = 0; 
                                if(m.PROD_MASZYNY_PW.PROD_MASZYNY.udzial_osoba > 0)
                                {
                                    koszt_osoba = ((m.PROD_MASZYNY_PW.PROD_MASZYNY.koszt_osoba * Convert.ToDouble(m.PROD_MASZYNY_PW.PROD_MASZYNY.udzial_osoba)) * m.ilosc_pracownikow) * duration.TotalHours;
                                }
                                else
                                {
                                    koszt_osoba = m.PROD_MASZYNY_PW.PROD_MASZYNY.koszt_osoba * m.ilosc_pracownikow * duration.TotalHours;
                                }
                                koszt_pracy += koszt_osoba;
                            }
                            //sprawdz czy istnieja juz wczesniejsze koszty pracy
                            maszyna_koszt = db.PROD_MASZYNY_KOSZTY.Where(x => x.id_prod_maszyny_monit == m.id).FirstOrDefault();
                            if(maszyna_koszt == null)
                                maszyna_koszt = new PROD_MASZYNY_KOSZTY();
                            maszyna_koszt.id_prod_maszyny_pw = (int)m.id_prod_maszyny_pw;
                            maszyna_koszt.id_prod_maszyny_monit = m.id;
                            maszyna_koszt.czas_pracy = duration.TotalHours.ToString();
                            maszyna_koszt.wartosc_czas = Math.Round((double)koszt_pracy, 2);
                            koszt_dla_zlecenia += (double)maszyna_koszt.wartosc_czas;
                            maszyna_koszt.cena_czas = m.PROD_MASZYNY_PW.PROD_MASZYNY.cena_czas;
                            if (!(maszyna_koszt.id > 0))
                                db.PROD_MASZYNY_KOSZTY.Add(maszyna_koszt);
                            db.SaveChanges();
                           // System.Windows.MessageBox.Show(koszt_pracy.ToString());
                        }

                    }
                    
                   
                }
               
                                    
            }
            return koszt_dla_zlecenia;
        }

        public double WyliczKosztyObce(double ilosc_kg)
        {
            double koszty_obce = 0;

            using (FZLEntities1 db = new FZLEntities1())
            {
                var maszyny_pw = (from c in db.PROD_MASZYNY_PW
                                  where c.id_prod == this.Produkcja.id
                                  select c).ToList();
                foreach(PROD_MASZYNY_PW pw in maszyny_pw)
                {
                    if(pw.PROD_MASZYNY.cena_kg != null && pw.PROD_MASZYNY.cena_kg > 0)
                    {
                        koszty_obce += ((double)pw.PROD_MASZYNY.cena_kg * ilosc_kg);
                    }
                   
                }
                return koszty_obce;
            }

            return 0;
        }



        /**
         * Pobiera informacje o pozycjach RW ujętych w magazynie produkcyjnym
         * 
         */
        public List<PROD_PRODMGPW> PobierzPowiazaniaZmagazynemProdukcyjnym()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                
            }
            return new List<PROD_PRODMGPW>();
        }

        /*
         * Sprawdza czy pozycja zostala juz dodana do PRODDP_FK
         * */
        public bool CzyJestwPRODDP_FK(int iddw, int id_prod)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                var query = db.PRODDP_FK.Where(x => x.iddw == iddw && x.id_prod == id_prod).First();
               
                if (query != null) return true;
            }

                return false;
        }

        /*
         * Przepisuje pozycje z dostawami z Symfonii do tabeli PRODDP_FK
         * */
        public void PrzepiszPozycjeDoFK()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                var queryRW = (from dp in db.PRODDP
                               where dp.id_prod == this.Produkcja.id
                               where dp.iddw > 0
                               where dp.rodzaj_dk == "RW"
                               select dp).ToList();

                foreach (PRODDP dp in queryRW)
                {
                    if(!CzyJestwPRODDP_FK((int)dp.iddw, dp.id_prod))
                    {

                    }
                }

            }
        }

        /*
         *  
         */
         public void WstawSpecyfikacjeDoZakonczonegoZlecena(int id_prod)
        {

            if(this.Produkcja.zakonczono == 1)
            {
                using (FZLEntities1 db = new FZLEntities1())
                {
                    var query = db.PRODDP.Where(x => x.id_prod == id_prod).ToList();
                    foreach (PRODDP poz in query)
                    {
                        PrzepiszSpecyfikacjePakowania(poz.id);
                    }
                }
                   
               
            }
        }

        /* 
         * Przepisuje informacje o sspecyfikacji pakowania dla danej poozycji z zlecenia prod
         */
        public void PrzepiszSpecyfikacjePakowania(int id_proddp)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {

                var query = (from m in db.PRODDP_MONIT
                             where m.id_proddp == id_proddp
                             select m).ToList();

                var query2 = (from mgpw in db.PROD_PRODMGPW
                              where mgpw.id_proddp == id_proddp
                              where mgpw.id_dkprodmg != null
                              select mgpw
                              ).ToList();

                //Znajdz pozycje w magazynie produkcyjnym, ktora zostala wygneerowana z pozycji zlecenia
                if (query2 != null && query2.Count() > 0)
                {
                    PROD_PRODMGPW powmg = query2.First();


                    foreach (PRODDP_MONIT monit in query)
                    {
                        //sprawdz czy do tej pozycji zostala juz uzupelniona specyfikacja
                        var query3 = (from spec in db.PROD_MZ_SPEC
                                      where spec.id_prod_monit == monit.id
                                      select spec).FirstOrDefault();
                        if (query3 == null)
                        {
                            PROD_MZ_SPEC spec = new PROD_MZ_SPEC();
                            spec.id_prod_monit = monit.id;
                            spec.id_prodmz = powmg.PROD_MZ.id;
                            spec.data = monit.data;
                            spec.godzina = monit.godzina;
                            spec.kodtw = monit.kodtw;
                            spec.ilosc = monit.ilosc;
                            if (monit.id_opakowania != null)
                            {
                                if (db.OPAKOWANIA_RODZAJE.Where(x => x.id == monit.id_opakowania).Count() > 0)
                                {
                                    spec.id_opakowania = monit.id_opakowania;
                                }
                            }
                            spec.opakowanie_nazwa = monit.opakowanie_nazwa;
                            spec.opakowanie_nazwa2 = monit.opakowanie_nazwa2;
                            spec.ilosc_opakowania = monit.ilosc_opakowania;
                            spec.waga_opakowania = monit.waga_opakowania;
                            spec.id_miejsca_skladowania = monit.id_miejsca_skladowania;
                            // spec.id_opakowania_rodzaje2 = monit.id_opakowania_rodzaje2;
                            if (monit.id_opakowania2 != null)
                            {
                                if (db.OPAKOWANIA_RODZAJE.Where(x => x.id == monit.id_opakowania2).Count() > 0)
                                {
                                    spec.id_opakowania2 = monit.id_opakowania2;
                                }
                                
                            }
                            spec.ilosc_opakowania2 = monit.ilosc_opakowania2;
                            spec.ilewopakowaniu = monit.ilewopakowaniu;
                            //spec.OPAKOWANIA_RODZAJE1.
                            
                            db.PROD_MZ_SPEC.Add(spec);
                            db.SaveChanges();

                        }
                        //System.Windows.MessageBox.Show(pw.PROD_MZ.PROD_MG.kod);
                    }
                }
           }
        }


        }
}

