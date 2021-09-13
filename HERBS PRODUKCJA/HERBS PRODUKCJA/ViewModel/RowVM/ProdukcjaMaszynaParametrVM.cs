using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaMaszynaParametrVM : VMBase
    {
        public PROD_MASZYNY_PARAM MaszynaParam { get; set; }
        public PROD_MASZYNY_PW MaszynaPW { get; set; }
        public List<ProdukcjaMaszynaParametrWartVM> WartosciVM { get; set; }
        public List<PROD_MASZYNY_PARAM_WART> Wartosci { get; set; }
        
       
        public void PobierzWartosciDomyslne()
        {
            WartosciVM = new List<ProdukcjaMaszynaParametrWartVM>();
            ProdukcjaMaszynaParametrWartVM ParamWartVM;
            PROD_MASZYNY_PARAM_WART ParamWart;
            using (FZLEntities1 db = new FZLEntities1())
            {
                var query = (from c in db.PROD_MASZYNY_PARAM_DEF
                             where c.id_param == MaszynaParam.id
                             select c).ToList();
                foreach (PROD_MASZYNY_PARAM_DEF par in query)
                {
                    ParamWartVM = new ProdukcjaMaszynaParametrWartVM();
                    ParamWartVM.IsSelected = false;
                    ParamWart = new PROD_MASZYNY_PARAM_WART();
                    if (Wartosci != null && Wartosci.Count() > 0)
                    { 
                        foreach (PROD_MASZYNY_PARAM_WART w in Wartosci)
                        {
                            if (w.wart == par.wartosc)
                            {
                                ParamWartVM.IsSelected = true;
                                //ParamWartVM.MaszynaPW = MaszynaPW;
                                
                            }
                        }
                    }
                    
                    ParamWart.id_param = par.id_param;
                    ParamWart.PROD_MASZYNY_PARAM = par.PROD_MASZYNY_PARAM;
                    ParamWart.wart = par.wartosc;
                    //MessageBox.Show(ParamWart.wart);
                    
                    ParamWartVM.Wartosc = ParamWart;
                    WartosciVM.Add(ParamWartVM);  
                }
                if (Wartosci != null && Wartosci.Count() > 0)
                {
                    IsSelected = true;
                    foreach (PROD_MASZYNY_PARAM_WART w in Wartosci)
                    {
                        
                        if (w.PROD_MASZYNY_PARAM.parametr_type == "float" || w.PROD_MASZYNY_PARAM.parametr_type == "bool")
                        {
                            
                            ParamWartVM = new ProdukcjaMaszynaParametrWartVM();
                            if (w.wart == "1")
                                ParamWartVM.IsSelected = true;
                            else
                                ParamWartVM.IsSelected = false;
                            ParamWartVM.Wartosc = w;
                            WartosciVM.Add(ParamWartVM);
                        }
                    }
                   
                }
                else
                {
                    IsSelected = false;
                    ParamWartVM = new ProdukcjaMaszynaParametrWartVM();
                    ParamWart = new PROD_MASZYNY_PARAM_WART();
                    ParamWart.PROD_MASZYNY_PARAM = MaszynaParam;
                    ParamWart.id_param = MaszynaParam.id;
                    ParamWartVM.Wartosc = ParamWart;
                    WartosciVM.Add(ParamWartVM);

                }
            }
        }

        public void PobierzWartosci(int id_prod, int id_prod_maszyna_pw)
        {
            FZLEntities1 db = new FZLEntities1();
           
           
                Wartosci = db.PROD_MASZYNY_PARAM_WART.Where(x => x.id_prod == id_prod && (x.id_param == MaszynaParam.id && x.id_prod_maszyny_pw == id_prod_maszyna_pw)).ToList();
                             
            
            if(Wartosci == null)
            {
                IsSelected = false;
                Wartosci = new List<PROD_MASZYNY_PARAM_WART>();
            }
            else
            {
               // IsSelected = true;
            }
        }

        public void ZapiszParametry(PROD_MASZYNY_PW maszynapw)
        {

            using (FZLEntities1 db = new FZLEntities1())
            {
                PROD_MASZYNY_PARAM_WART wartosc;
                foreach (ProdukcjaMaszynaParametrWartVM wart in WartosciVM)
                {
                    if ((wart.Wartosc.PROD_MASZYNY_PARAM.parametr_type == "bool" || wart.Wartosc.PROD_MASZYNY_PARAM.parametr_type == "list") && wart.IsSelected == true)
                    {
                        // MessageBox.Show(wart.Wartosc.wart);
                        if ((wart.Wartosc.id > 0))
                            wartosc = db.PROD_MASZYNY_PARAM_WART.Where(x => x.id == wart.Wartosc.id).FirstOrDefault();
                        else
                            wartosc = new PROD_MASZYNY_PARAM_WART();
                        if (wart.Wartosc.PROD_MASZYNY_PARAM.parametr_type == "bool")
                        {
                            wartosc.wart = "1";
                        }
                        else
                        {
                            wartosc.wart = wart.Wartosc.wart;
                        }

                        wartosc.id_prod = maszynapw.id_prod;
                        wartosc.id_param = MaszynaParam.id;
                        wartosc.id_prod_maszyny_pw = maszynapw.id;
                       
                        if (!(wartosc.id > 0))
                            db.PROD_MASZYNY_PARAM_WART.Add(wartosc);
                        db.SaveChanges();
                       

                    }
                    else if (wart.Wartosc.PROD_MASZYNY_PARAM.parametr_type == "float")
                    {
                        if ((wart.Wartosc.id > 0))
                            wartosc = db.PROD_MASZYNY_PARAM_WART.Where(x => x.id == wart.Wartosc.id).FirstOrDefault();
                        else
                            wartosc = new PROD_MASZYNY_PARAM_WART();

                        wartosc.id_param = wart.Wartosc.id_param;
                        wartosc.id_prod = maszynapw.id_prod;
                        wartosc.id_prod_maszyny_pw = maszynapw.id;
                        wartosc.wart = wart.Wartosc.wart;


                        if (!(wartosc.id > 0))
                            db.PROD_MASZYNY_PARAM_WART.Add(wartosc);
                        db.SaveChanges();
                        

                    }
                }
                UsunWartosci();
                PobierzWartosci(maszynapw.id_prod,maszynapw.id);
            }
            



        }

        public void UsunWartosci()
        {

            using (FZLEntities1 db = new FZLEntities1())
            {
                bool jest;
                if (Wartosci != null && Wartosci.Count() > 0)
                {
                    foreach (PROD_MASZYNY_PARAM_WART wart in Wartosci)
                    {
                        jest = false;
                        foreach (ProdukcjaMaszynaParametrWartVM wartVM in WartosciVM)
                        {
                            if (wart.id == wartVM.Wartosc.id && wart.PROD_MASZYNY_PARAM.parametr_type == "float")
                            {
                                jest = true;
                            }
                            if (wart.id == wartVM.Wartosc.id && ((wart.PROD_MASZYNY_PARAM.parametr_type == "list" || wart.PROD_MASZYNY_PARAM.parametr_type == "bool") && wartVM.IsSelected))
                            {
                                jest = true;
                            }
                        }
                        if (!jest)
                        {
                            var obj = db.PROD_MASZYNY_PARAM_WART.Where(x => x.id == wart.id).FirstOrDefault();
                            if (obj != null)
                            {
                                db.PROD_MASZYNY_PARAM_WART.Remove(obj);
                                db.SaveChanges();
                            }
                        }
                    }
                }
            }
        }

       

    }
}
