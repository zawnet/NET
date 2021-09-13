using FeserWard.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.InteliboxDataProviders
{
    public class TWResultsProvider : IIntelliboxResultsProvider
    {
        //you would not do this in a real application because the connection would always be open.
        private FZLEntities1 Data = new FZLEntities1();

        public IEnumerable DoSearch(string searchTerm, int maxResults, object extraInfo)
        {



            //using (var nw = new NorthwindEntities()) {
            //this is a l2e limit for sql server compact
            //for real sql, you can just perform a where without the tolist.
            var l2eIsNotFun = Data.TW.Where(p => p.nazwa.StartsWith(searchTerm));
            return l2eIsNotFun;
            //}
        }
    }
}
