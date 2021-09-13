using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace HERBS_PRODUKCJA.Helpers
{
    public static class Functions
    {
       
        public static DataTable ToDataTable<T>(this IEnumerable<T> entityList) where T : class
        {
            var properties = typeof(T).GetProperties();
            var table = new DataTable();

            foreach (var property in properties)
            {
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                table.Columns.Add(property.Name, type);
            }
            foreach (var entity in entityList)
            {
                table.Rows.Add(properties.Select(p => p.GetValue(entity, null)).ToArray());
            }
            return table;
        }



        /*
         * Wstawia 
         */
        public static void NaprawProdMaszynaParamWart()
        {

            using (FZLEntities1 db = new FZLEntities1())
            {
                var PRODLINIE = (from c in db.PROD_LINIE_PW
                                  select c).ToList();

                var PRODMASZYNYPW = (from c in db.PROD_MASZYNY_PW
                                     select c).ToList();
                foreach (PROD_MASZYNY_PW mpw in PRODMASZYNYPW)
                {
                    var prodparamwart = (from c in db.PROD_MASZYNY_PARAM_WART
                                         where c.id_prod == mpw.id_prod
                                         select c).ToList();
                    foreach (PROD_MASZYNY_PARAM_WART wart in prodparamwart)
                    {
                        if(wart.PROD_MASZYNY_PARAM.id_maszyny == mpw.id_maszyny && wart.id_prod_maszyny_pw == null)
                        {
                            wart.id_prod_maszyny_pw = mpw.id;
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        //Generator kodu sscc dla konkretnej zapokowanej pozycji wyrobu gotowego
        public static void GenerujKodSSCC(int id_prod_dp_monit)
        {
            string kod_firmy = System.Windows.Application.Current.Properties["kod_firmy"].ToString();
            int? serial;
            serial = 0;
            string[] sa; 
            GS1_KODY kodgs1;
            PRODDP_MONIT monit;
            PROD_HMTW towar;
            PRODDP proddp;
            GS1_CONFIG gsconfig;
            using (FZLEntities1 db = new FZLEntities1())
            {
                gsconfig = db.GS1_CONFIG.Where(x => x.kod_firmy == kod_firmy).FirstOrDefault();
                monit = db.PRODDP_MONIT.Where(i => i.id == id_prod_dp_monit).FirstOrDefault();
                //sprawdz czy juz insteje wpis dotyczacy sscc
                var kody = (from c in db.GS1_KODY
                            where c.id_prod_dp_monit == id_prod_dp_monit
                            select c).ToList();
                
                //Jeżeli nie ma wpisu dotyczącego danego opakowania (monitu z produkcji) 
                //Generuj nowy kod sscc dla danego monitu
                if (kody.Count == 0)
                {
                    
                    serial = (from c in db.GS1_KODY
                              where c.kod_firmy == kod_firmy
                              select c.seria_nr).Max();
                    if (serial == null)
                        serial = 1;
                    else
                        serial = serial + 1;

                    proddp = db.PRODDP.Where(x => x.id == monit.id_proddp).FirstOrDefault();
                    towar = db.PROD_HMTW.Where(x => x.id_fk == proddp.idtw && x.kod_firmy == kod_firmy).FirstOrDefault();
                    kodgs1 = new GS1_KODY();
                    kodgs1.id_prod_dp_monit = monit.id;
                    kodgs1.ilosc_opakowania = monit.ilosc_opakowania;
                    kodgs1.nazwa_opakowania = monit.opakowanie_nazwa;
                    kodgs1.kod_firmy = kod_firmy;
                    kodgs1.kod_tw = towar.kodpaskowy;
                    kodgs1.nazwa_tw = towar.nazwa;
                    kodgs1.nr_getin = towar.numer_gtin;
                    kodgs1.termin = monit.termin_przydatnosci;
                    kodgs1.waga_netto = monit.ilosc;
                    kodgs1.waga_opakowania = monit.waga_opakowania;
                    
                    kodgs1.seria_nr = serial;
                    if (monit.oznaczenie != null || monit.oznaczenie != "")
                        kodgs1.nr_partii = monit.oznaczenie;
                    else
                        kodgs1.nr_partii = monit.PRODDP.nr_partii;
                    kodgs1.sscc = gsconfig.cyfra__rozszerzajaca.ToString() + gsconfig.nr_jednostki + String.Format("{0:000000}", serial);
                    kodgs1.cyfra_kontrolna = GetGTINCheckDigitUsingQuery(kodgs1.sscc).ToString();
                    kodgs1.sscc_pelny = kodgs1.sscc + kodgs1.cyfra_kontrolna.ToString();
                    if(kodgs1.termin != null)
                    {
                        //kodgs1.termin_kod = string.Format("The date/time is: {0:YYMMdd}", monit.termin_przydatnosci);


                    }
                    
                    db.GS1_KODY.Add(kodgs1);
                    db.SaveChanges();
                     
                    System.Windows.Forms.MessageBox.Show(kodgs1.sscc);
                }
                else
                {
                    //db = new FZLEntities1();
                    proddp = db.PRODDP.Where(x => x.id == monit.id_proddp).FirstOrDefault();
                    towar = db.PROD_HMTW.Where(x => x.id_fk == proddp.idtw && x.kod_firmy == kod_firmy).FirstOrDefault();
                    kodgs1 = (from c in db.GS1_KODY
                              where c.id_prod_dp_monit == id_prod_dp_monit
                              select c).FirstOrDefault();
                   // kodgs1.id_prod_dp_monit = monit.id;
                    kodgs1.ilosc_opakowania = monit.ilosc_opakowania;
                    kodgs1.nazwa_opakowania = monit.opakowanie_nazwa;
                    kodgs1.kod_firmy = kod_firmy;
                    kodgs1.kod_tw = towar.kodpaskowy;
                    kodgs1.nazwa_tw = towar.nazwa;
                    kodgs1.nr_getin = towar.numer_gtin;
                    kodgs1.termin = monit.termin_przydatnosci;
                    kodgs1.waga_netto = monit.ilosc;
                    kodgs1.waga_opakowania = monit.waga_opakowania;

                    //kodgs1.seria_nr = serial;
                    if (monit.oznaczenie != null || monit.oznaczenie != "")
                        kodgs1.nr_partii = monit.oznaczenie;
                    else
                        kodgs1.nr_partii = monit.PRODDP.nr_partii;
                    //kodgs1.sscc = gsconfig.cyfra__rozszerzajaca.ToString() + gsconfig.nr_jednostki + String.Format("{0:000000}", serial);
                   // kodgs1.cyfra_kontrolna = GetGTINCheckDigitUsingQuery(kodgs1.sscc).ToString();
                   // kodgs1.sscc_pelny = kodgs1.sscc + kodgs1.cyfra_kontrolna.ToString();
                    if (kodgs1.termin != null)
                    {
                        kodgs1.termin_kod = string.Format("The date/time is: {0:YYMMdd}", monit.termin_przydatnosci);


                    }

                    
                   // db.SaveChanges();
                }
                
            }
        }

        public static int GetGTINCheckDigitUsingQuery(string code)
        {
            var reversed = code.Reverse().ToArray();
            var sum =
               (from i in Enumerable.Range(0, reversed.Count())
                let digit = (int)char.GetNumericValue(reversed[i])
                select digit * (i % 2 == 0 ? 3 : 1)).Sum();
            return (10 - sum % 10) % 10;
        }

        public static PRODDP PobierzPozycjePrzezDokument(int? idprdhmdw)
        {
            PRODDP proddp;
            using (FZLEntities1 db = new FZLEntities1())
            {
                proddp = (from c in db.PRODDP
                          from p in db.PARTIE_FK
                          where p.iddk_fki == idprdhmdw
                          where c.id_hm == p.idpoz_fki
                          select c).FirstOrDefault();
            }
                return proddp;
        }
    

        public static PRODDP PobierzZlecenieDlaDostawy(int mgdw_id, FZLEntities1 db)
        {
            PRODDP proddp;
           // using (FZLEntities1 db = new FZLEntities1())
        //    {
                proddp = (from c in db.PROD_PRODMGPW
                          from p in db.PRODDP
                          where c.id_prodmgdw == mgdw_id
                          where c.id_proddp == p.id
                          
                          select p).FirstOrDefault();
          //  }
            return proddp;
        }

        public static String getMotherBoardID()
        {
            String serial = "";
            try
            {
                ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
                foreach (ManagementObject getserial in MOS.Get())
                {
                   serial =  getserial["SerialNumber"].ToString();
                }
                return serial;
            }
            catch (Exception)
            {
                return serial;
            }
        }

        public static String getCpuID()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                if (cpuInfo == "")
                {
                    //Get only the first CPU's ID
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }

        public static List<string> GetTypyPozycjiWG()
        {
            List<string> TYPY_PRODDP = new List<string>();
            TYPY_PRODDP.Add("SR");
            TYPY_PRODDP.Add("WG");
            TYPY_PRODDP.Add("PU");
            TYPY_PRODDP.Add("PP");
            TYPY_PRODDP.Add("OD");
            TYPY_PRODDP.Add("OP");
            TYPY_PRODDP.Add("NS");
            return TYPY_PRODDP;
        }


        public static void SendEmail(string subject, string to, string content, System.Net.Mail.Attachment attachment = null)
        {
            var smtpServerName = ConfigurationManager.AppSettings["SmtpServer"];
            var port = ConfigurationManager.AppSettings["Port"];
            var senderEmailId = ConfigurationManager.AppSettings["SenderEmailId"];
            var senderPassword = ConfigurationManager.AppSettings["SenderPassword"];

            var smptClient = new SmtpClient(smtpServerName, Convert.ToInt32(port))
            {
                Credentials = new NetworkCredential(senderEmailId, senderPassword),
                EnableSsl = true
            };







            MailAddress addressFrom = new MailAddress(senderEmailId, "HERBS PRODUKCJA");
            MailAddress addressTo = new MailAddress(to);
         
            MailMessage message = new MailMessage(addressFrom, addressTo);
           // message.From = addressFrom;
            foreach (var address in to.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
               // message.To.Add(new MailAddress(address));
            }
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = content;
            if(attachment != null)
            {
                message.Attachments.Add(attachment);
            }


           
            smptClient.Send(message);
        }

        
    }
}
