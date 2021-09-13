using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaMaszynaVM : VMBase
    {
        public PROD_MASZYNY_PW MaszynaPW { get; set; }
        public PROD_MASZYNY Maszyna { get; set; }

        public List<ProdukcjaMaszynaParametrVM> Parametry { get; set; }
        public List<ProdukcjaMaszynaParametrMonitVM> Monits { get; set; }

        public ProdukcjaMaszynaVM()
        {
            //MaszynaPW.PROD_MASZYNY_MONIT.ToList();
             //MaszynaPW.nazwa_maszyny
        }
        //Definjuje domyślne wartosci dla parametrow Maszyn
        public void getDefaultsParamWart(PROD_MASZYNY_PARAM maszyna_param)
        {
           // List<PROD_ma>
        }

        public void GetParametry()
        {
            Parametry = new List<ProdukcjaMaszynaParametrVM>();
            
                foreach (PROD_MASZYNY_PARAM lm in Maszyna.PROD_MASZYNY_PARAM)
                {

                    ProdukcjaMaszynaParametrVM MaszPar = new ProdukcjaMaszynaParametrVM();
                  //  MaszPar.IsSelected = true;
                    //PROD_MASZYNY_PW mpw = new PROD_MASZYNY_PW();

                    MaszPar.MaszynaParam = lm;

                //mpw.id_maszyny = Masz.Maszyna.id;
                //mpw.id_prod = LiniaPW.id_prod;
                // mpw.id_prod_linie = LiniaPW.id_lini;
                // mpw.nazwa_maszyny = Masz.Maszyna.nazwa;
                // MaszynyPW.Add(mpw);
                // System.Windows.Forms.MessageBox.Show(lm.PROD_MASZYNY.nazwa);

                if (MaszynaPW != null && MaszynaPW.id > 0)
                {
                    MaszPar.MaszynaPW = MaszynaPW;
                    MaszPar.PobierzWartosci(MaszynaPW.id_prod, MaszynaPW.id);
                }

                    MaszPar.PobierzWartosciDomyslne();
                  
                    Parametry.Add(MaszPar);
                }
            
        }

        public void UsunMaszyne()
        {

            using (FZLEntities1 db = new FZLEntities1())
            {
                var obj = db.PROD_MASZYNY_PW.Where(x => x.id == MaszynaPW.id).FirstOrDefault();
                if (obj != null)
                {
                    db.PROD_MASZYNY_PW.Remove(obj);
                    db.SaveChanges();
                }
            }
        }

        public void UsunZapisaneParametry(int id_maszyna_pw)
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                foreach (PROD_MASZYNY_PARAM lm in Maszyna.PROD_MASZYNY_PARAM)
                {
                    var obj = db.PROD_MASZYNY_PARAM_WART.Where(x => x.id_prod == MaszynaPW.id_prod && (x.id_param == lm.id && x.id_prod_maszyny_pw == id_maszyna_pw)).ToList();
                    if (obj != null)
                    {
                        foreach (PROD_MASZYNY_PARAM_WART wart in obj)
                        {
                            db.PROD_MASZYNY_PARAM_WART.Remove(wart);
                            db.SaveChanges();
                        }

                    }
                }
            }
        }

        public void PobierzMaszynaMonit()
        {
            Monits = new List<ProdukcjaMaszynaParametrMonitVM>();
            using (FZLEntities1 db = new FZLEntities1())
            {
                var items= ((from m in db.PROD_MASZYNY_MONIT
                         where m.id_prod_maszyny_pw == MaszynaPW.id
                         select m).ToList());
                foreach(PROD_MASZYNY_MONIT m in items)
                {
                    Monits.Add(new ProdukcjaMaszynaParametrMonitVM { MaszynaMonit = m });
                }


                

            }
            
            //return null;
        }
       
        public void WyliczWartoscPracy()
        {

            string startTime = "7:00 AM";
            string endTime = "2:00 PM";
            if (Monits != null && Monits.Count() > 0)
            {
                foreach (ProdukcjaMaszynaParametrMonitVM m in Monits)
                {
                    if (m.MaszynaMonit.param_nazwa == "Czas pracy" && m.MaszynaMonit.rozpoczecie_data != null && m.MaszynaMonit.zakonczenie_data != null)
                    {
                        startTime = m.MaszynaMonit.rozpoczecie_data.ToString() + m.MaszynaMonit.rozpoczecie_godzina;
                        endTime = m.MaszynaMonit.zakonczenie_data.ToString() + m.MaszynaMonit.zakonczenie_godzina;
                        TimeSpan duration = DateTime.Parse(endTime).Subtract(DateTime.Parse(startTime));
                        //System.Windows.MessageBox.Show(duration.ToString());

                    }
                }
            }
            /*
            DateTime start, stop;
            foreach 
            if (Wartosc.PROD_MASZYNY_PARAM.kod == "start")
            {
                start = DateTime.ParseExact(Wartosc.pr, "yyyy-MM-dd HH:mm:ss,fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
            }
            */
        }




    }
}
