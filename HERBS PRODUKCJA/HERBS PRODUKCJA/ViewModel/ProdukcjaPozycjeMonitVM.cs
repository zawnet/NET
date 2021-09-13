using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel
{
    public class ProdukcjaPozycjeMonitVM : VMBase
    {
        public PRODDP_MONIT ProdDPMonit { get; set; }
        public short? lp { get; set; }

        public ProdukcjaPozycjeMonitVM()
        {
            ProdDPMonit = new PRODDP_MONIT();
            
        }

        public void Zapisz(PRODDP_MONIT proddpmonit)
        {
            using (FZLEntities1 db2 = new FZLEntities1())
            {
                this.ProdDPMonit = proddpmonit;
                if (ProdDPMonit.id_opakowania != null)
                {

                   // ProdDPMonit.opakowanie_nazwa = db2.OPAKOWANIA_RODZAJE.Where(x => x.id == ProdDPMonit.id_opakowania).FirstOrDefault().nazwa;
                    //MessageBox.Show(monit.ProdDPMonit.opakowanie_nazwa);
                }
                if (ProdDPMonit.id_opakowania2 != null)
                {

                   // ProdDPMonit.opakowanie_nazwa2 = db2.OPAKOWANIA_RODZAJE.Where(x => x.id == ProdDPMonit.id_opakowania2).FirstOrDefault().nazwa;
                    // MessageBox.Show(monit.ProdDPMonit.opakowanie_nazwa2);
                }
                if (!(ProdDPMonit.id > 0))
                {
                    db2.PRODDP_MONIT.Add(ProdDPMonit);
                }
                db2.SaveChanges();
            }
        }
    }
}
