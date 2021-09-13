using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaPozycjeVM : VMBase
    {
        public PRODDP Pozycja { get; set; }
        public PRODDP_FK PozycjaFK { get; set; }
        public PRODDP_SYM PozycjaSYM { get; set; }
        public PROD Produkcja { get; set; }
        public bool IsChecked { get; set; }
        public ProdukcjaPozycjeVM()
        {
            Pozycja = new PRODDP();
            PozycjaFK = new PRODDP_FK();
            PozycjaSYM = new PRODDP_SYM();
            Pozycja.waluta = "PLN";
            Pozycja.data_kursu = DateTime.Today;
            Pozycja.nr_partii = "";
           
        }

        public void GetProd()
        {
            using (FZLEntities1 dbx = new FZLEntities1())
            {
                this.Produkcja = dbx.PROD.Where(i => i.id == Pozycja.id_prod).FirstOrDefault();
            }
        }

        public void PrzypiszNazwyOpakowan()
        {
            OPAKOWANIA_RODZAJE opakowanie_rodzaj;
            MAGAZYNY magazyn;
            using (FZLEntities1 db = new FZLEntities1())
            {
                var dpmonits = (from c in db.PRODDP_MONIT
                                where c.id_proddp == this.Pozycja.id
                                select c).ToList();

                foreach (PRODDP_MONIT monit in dpmonits)
                {
                    if (monit.id_opakowania != null && monit.id_opakowania > 0)
                    {
                        opakowanie_rodzaj = db.OPAKOWANIA_RODZAJE.Where(x => x.id == monit.id_opakowania).FirstOrDefault();
                        monit.opakowanie_nazwa = opakowanie_rodzaj.nazwa;
                        db.SaveChanges();
                    }
                    if (monit.id_opakowania2 != null && monit.id_opakowania2 > 0)
                    {
                        opakowanie_rodzaj = db.OPAKOWANIA_RODZAJE.Where(x => x.id == monit.id_opakowania2).FirstOrDefault();
                        monit.opakowanie_nazwa2 = opakowanie_rodzaj.nazwa;
                        db.SaveChanges();
                    }
                    if (monit.id_miejsca_skladowania != null && monit.id_miejsca_skladowania > 0 )
                    {
                        magazyn = db.MAGAZYNY.Where(x => x.id == monit.id_miejsca_skladowania).FirstOrDefault();
                        monit.miejsce_skladowania = magazyn.nazwa;
                        db.SaveChanges();
                    }

                }
            }
        }

        public void DopasujDoPartii()
        {
            //MessageBox.Show("Test");
            string pattern = "(^F\\.?) | (^F/.?)";
            string[] names = { Pozycja.nr_partii};
            
            Match result = Regex.Match(Pozycja.nr_partii, pattern);
            if (result.Success)
            {
                // Indeed, the expression "([a-zA-Z]+) (\d+)" matches the date string

                // To get the indices of the match, you can read the Match object's
                // Index and Length values.
                // This will print [0, 7], since it matches at the beginning and end of the 
                // string
               MessageBox.Show(String.Format("Match at index [{0}, {1})",
                    result.Index,
                    result.Index + result.Length));

                // To get the fully matched text, you can read the Match object's Value
                // This will print "June 24"
                Console.WriteLine("Match: {0}", result.Value);

                // If you want to iterate over each of the matches, you can call the 
                // Match object's NextMatch() method which will return the next Match
                // object.
                // This will print out each of the matches sequentially.
                while (result.Success)
                {
                    Console.WriteLine("Match: {0}", result.Value);
                    result = result.NextMatch();
                }
            }

                if (Pozycja.nr_partii != "" && Pozycja.nr_partii != null)
            {
                using (FZLEntities1 dbx = new FZLEntities1())
                {
                    var partia = (from p in dbx.PARTIE
                                  where Pozycja.nr_partii.Contains(p.nr_partii)
                                  select p).FirstOrDefault();

                    if(partia != null)
                    {
                        Pozycja.id_partii = partia.id;
                       //MessageBox.Show(partia.id.ToString());
                    }
                    else
                    {
                        Pozycja.id_partii = null;
                    }
                    
                }
                    
            }
        }


        public void DopasujDoPartiiFK()
        {
            //MessageBox.Show("Test");
            string pattern = "(^F\\.?) | (^F/.?)";
            string[] names = { PozycjaFK.nr_partii };
            if (PozycjaFK.nr_partii != null && PozycjaFK.nr_partii != "")
            {
                Match result = Regex.Match(PozycjaFK.nr_partii, pattern);
                if (result.Success)
                {
                    // Indeed, the expression "([a-zA-Z]+) (\d+)" matches the date string

                    // To get the indices of the match, you can read the Match object's
                    // Index and Length values.
                    // This will print [0, 7], since it matches at the beginning and end of the 
                    // string
                    MessageBox.Show(String.Format("Match at index [{0}, {1})",
                         result.Index,
                         result.Index + result.Length));

                    // To get the fully matched text, you can read the Match object's Value
                    // This will print "June 24"
                    Console.WriteLine("Match: {0}", result.Value);

                    // If you want to iterate over each of the matches, you can call the 
                    // Match object's NextMatch() method which will return the next Match
                    // object.
                    // This will print out each of the matches sequentially.
                    while (result.Success)
                    {
                        Console.WriteLine("Match: {0}", result.Value);
                        result = result.NextMatch();
                    }
                }
            }

            if (PozycjaFK.nr_partii != "" && PozycjaFK.nr_partii != null)
            {
                using (FZLEntities1 dbx = new FZLEntities1())
                {
                    var partia = (from p in dbx.PARTIE
                                  where PozycjaFK.nr_partii.Contains(p.nr_partii)
                                  select p).FirstOrDefault();

                    if (partia != null)
                    {
                        PozycjaFK.id_partii = partia.id;
                        //MessageBox.Show(partia.id.ToString());
                    }
                    else
                    {
                        PozycjaFK.id_partii = null;
                    }

                }

            }
        }

        /**
         * sprawdza czy dana pozycja produkcjna ma powiazanie z magazynem produkcyjnym
         * 
         * */
        public PROD_PRODMGPW sprawdzPowiazanieMagazynProd()
        {
            using (FZLEntities1 db = new FZLEntities1())
            {
                if (Pozycja.id > 0)
                {
                    var query = (from p in db.PROD_PRODMGPW
                                 where p.id_proddp == this.Pozycja.id
                                 select p).FirstOrDefault();
                    if (query != null && query.id > 0)
                    {
                        return query;
                    }
                }
            }
                return null;
        }

        

    }
}
