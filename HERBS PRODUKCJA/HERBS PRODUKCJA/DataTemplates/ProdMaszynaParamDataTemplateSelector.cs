using HERBS_PRODUKCJA.ViewModel.RowVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace HERBS_PRODUKCJA.DataTemplates
{
    public class ProdMaszynaParamDataTemplateSelector : DataTemplateSelector
    {

        public DataTemplate DefaultnDataTemplate { get; set; }
        public DataTemplate BooleanDataTemplate { get; set; }
        public DataTemplate EnumDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ProdukcjaMaszynaParametrWartVM dpi = item as ProdukcjaMaszynaParametrWartVM;
            if (dpi.Wartosc.PROD_MASZYNY_PARAM.parametr_type == "bool")
            {
                return BooleanDataTemplate;
            }
            if (dpi.Wartosc.PROD_MASZYNY_PARAM.parametr_type == "list")
            {
                return EnumDataTemplate;
            }

            return DefaultnDataTemplate;
        }
    }
}
