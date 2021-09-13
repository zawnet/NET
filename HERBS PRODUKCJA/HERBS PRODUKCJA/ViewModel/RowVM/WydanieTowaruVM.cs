using HERBS_PRODUKCJA.Support;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HERBS_PRODUKCJA.ViewModel.RowVM
{
    public class WydanieTowaruVM : VMBase
    {
        public WZ_SPECYFIKACJE WZSPEC { get; set; }
        public ObservableCollection<WZ_SPECYFIKACJE_TOWARY> WZSPEC_TOWARY {get;set;}
        public ObservableCollection<WZ_SPECYFIKACJE_OPAKOWANIA> WZSPEC_OPAKOWANIA { get; set; }

        public string NabwcaAdres { get; set; }
        public string OdbiorcaAdres { get; set; }

        public WydanieTowaruVM()
        {

        }

    }
}
