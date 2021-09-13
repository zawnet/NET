using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class ProdukcjaMaszynaParametrMonitVM : VMBase
    {
        public PROD_MASZYNY_MONIT MaszynaMonit { get; set; }
        public String nazwa_maszyny { get; set; }

        public ProdukcjaMaszynaParametrMonitVM()
        {
            
        }

        
        
        public void Zapisz(FZLEntities1 db)
        {
            
                if (MaszynaMonit.id > 0)
                {
                    
                    db.SaveChanges();
                }
                else
                {
                    MaszynaMonit.utworzono = DateTime.Now;
                    db.PROD_MASZYNY_MONIT.Add(MaszynaMonit);
                    db.SaveChanges();
                }
            
        }
    }
}
