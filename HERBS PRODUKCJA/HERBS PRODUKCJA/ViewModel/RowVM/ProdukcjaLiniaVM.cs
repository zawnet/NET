using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaLiniaVM : VMBase
    {
        public PROD_LINIE_PW LiniaPW { get; set; }
        public List<PROD_MASZYNY_PW> MaszynyPW { get; set; }
        // public List<PROD_MASZYNY> Maszyny { get; set; }
        public List<ProdukcjaMaszynaVM> Maszyny { get; set; }

        public ProdukcjaLiniaVM()
        {


        }
        public void getMaszyny()
        {
            GetMaszynyPW();
            using (FZLEntities1 db = new FZLEntities1())
            {
                Maszyny = new List<ProdukcjaMaszynaVM>();
                PROD_MASZYNY_PW mpw;
                foreach (PROD_LINIE_MASZYNY lm in db.PROD_LINIE_MASZYNY.Where(x => x.id_lini == LiniaPW.id_lini).ToList())
                {
                    if (lm.PROD_MASZYNY.aktywna == 1)
                    {
                        ProdukcjaMaszynaVM Masz = new ProdukcjaMaszynaVM();
                        Masz.IsSelected = false;
                        mpw = new PROD_MASZYNY_PW();
                        foreach (PROD_MASZYNY_PW maszyna_pw in MaszynyPW)
                        {
                            if (maszyna_pw.id_maszyny == lm.id_maszyny)
                            {
                                Masz.IsSelected = true;

                                mpw = maszyna_pw;
                                Masz.MaszynaPW = maszyna_pw;
                                //  Masz.Maszyna = maszyna_pw.PROD_MASZYNY;
                               // Masz.Maszyna = db.PROD_MASZYNY.Where(x => x.id == maszyna_pw.id).FirstOrDefault();
                                //Masz.GetParametry();

                            }

                        }

                        Masz.Maszyna = lm.PROD_MASZYNY;


                        //mpw.id_maszyny = Masz.Maszyna.id;
                        //mpw.id_prod = LiniaPW.id_prod;
                        // mpw.id_prod_linie = LiniaPW.id_lini;
                        // mpw.nazwa_maszyny = Masz.Maszyna.nazwa;
                        // MaszynyPW.Add(mpw);
                        // System.Windows.Forms.MessageBox.Show(lm.PROD_MASZYNY.nazwa);
                        
                        Masz.GetParametry();

                        Maszyny.Add(Masz);
                        //Masz.WyliczWartoscPracy();
                    }
                }
            }

            }

        public void GetMaszynyPW()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                MaszynyPW = (from m in db.PROD_MASZYNY_PW
                             where m.id_prod == LiniaPW.id_prod
                             where m.id_prod_linie == LiniaPW.id
                             select m).ToList();
            }
        }

        /**
         * Zapisuje informacjie o liniach produkcyjnych do bazy danyc
         * 
         **/
        public void ZapiszLinie(PROD prod)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                PROD_MASZYNY_PW MaszynaPW;

                if (prod.id > 0)
                {
                    if (!(LiniaPW.id > 0))
                    {
                        LiniaPW.id_prod = prod.id;
                        db.PROD_LINIE_PW.Add(LiniaPW);
                        db.SaveChanges();

                    }
                    foreach (ProdukcjaMaszynaVM m in Maszyny)
                    {
                        if (m.IsSelected)
                        {


                            MaszynaPW = new PROD_MASZYNY_PW();
                            
                            foreach (PROD_MASZYNY_PW maszyna_pw in MaszynyPW)
                            {
                                if (m.MaszynaPW != null && maszyna_pw.id == m.MaszynaPW.id)
                                {

                                    MaszynaPW = maszyna_pw;
                                }

                            }


                            MaszynaPW.id_prod = prod.id;
                            MaszynaPW.id_maszyny = m.Maszyna.id;
                            MaszynaPW.nazwa_maszyny = m.Maszyna.nazwa;
                            MaszynaPW.id_prod_linie = LiniaPW.id;
                            if (!(MaszynaPW.id > 0))
                                db.PROD_MASZYNY_PW.Add(MaszynaPW);
                            db.SaveChanges();
                            m.MaszynaPW = MaszynaPW;
                            ZapiszParametry(m);
                            //MessageBox.Show(m.Maszyna.nazwa);
                        }
                    }
                    UsunOdznaczoneMaszyny();
                    //getMaszyny();
                    
                }
            }
        }

        public void ZapiszParametry(ProdukcjaMaszynaVM maszyna)
        {
           
            //foreach (ProdukcjaMaszynaVM m in Maszyny)
          //  {

                if (maszyna.IsSelected)
                {
                   
                    foreach (ProdukcjaMaszynaParametrVM p in maszyna.Parametry)
                    {
                        if (p.IsSelected)
                        {
                            p.ZapiszParametry(maszyna.MaszynaPW);
                            p.PobierzWartosci(maszyna.MaszynaPW.id_prod, maszyna.MaszynaPW.id);
                        }
                    }

                }
          //  }
        }

        public void UsunOdznaczoneMaszyny()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                foreach (ProdukcjaMaszynaVM m in Maszyny)
                {

                    if (!m.IsSelected && m.MaszynaPW != null)
                    {
                       
                        m.UsunZapisaneParametry(m.MaszynaPW.id);
                        m.UsunMaszyne();
                    }
                }
            }
        }

        public void WczytajParametry(PROD prod)
        {
            foreach (ProdukcjaMaszynaVM m in Maszyny)
            {

                

                    foreach (ProdukcjaMaszynaParametrVM p in m.Parametry)
                    {
                        p.PobierzWartosci(prod.id, m.MaszynaPW.id);
                    }

                
            }
        }

        public void UsunMaszyny()
        {
            using(FZLEntities1 db = new FZLEntities1())
            {
                var obj = db.PROD_MASZYNY_PW.Where(x => x.id_prod_linie == LiniaPW.id).ToList();

                foreach(PROD_MASZYNY_PW masz in obj)
                {
                    UsunParametryMaszyn(masz);
                    db.PROD_MASZYNY_PW.Remove(masz);
                    db.SaveChanges();
                }

            }
        }

        public void UsunParametryMaszyn(PROD_MASZYNY_PW masz_pw)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                var obj = db.PROD_MASZYNY_PARAM_WART.Where(x => x.id_prod == masz_pw.id_prod && x.id_prod_maszyny_pw == masz_pw.id).ToList();
                foreach(PROD_MASZYNY_PARAM_WART wart in obj)
                {
                    db.PROD_MASZYNY_PARAM_WART.Remove(wart);
                    db.SaveChanges();
                }
            }
        }

        public void UsunLinie()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                var obj = db.PROD_LINIE_PW.Where(x => x.id == LiniaPW.id).FirstOrDefault();
                if (obj.id > 0)
                {
                    UsunMaszyny();
                    db.PROD_LINIE_PW.Remove(obj);
                    db.SaveChanges();
                }
               
            }
        }

    }

}       
