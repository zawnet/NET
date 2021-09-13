using HERBS_PRODUKCJA.ViewModel;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HERBS_PRODUKCJA.Helpers
{

    public class DataGridColumnVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ProdukcjaDetaleViewModel viewModel = (ProdukcjaDetaleViewModel)value;
            ProdukcjaVM prod = (ProdukcjaVM) value;
           
            return (prod.Produkcja.wycena_typ != 1) ? Visibility.Visible : Visibility.Collapsed;
               
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class BoolToBrushConverter : IValueConverter
    {
        public Brush KolorDlaFałszu { get; set; } = Brushes.Black;
        public Brush KolorDlaPrawdy { get; set; } = Brushes.Gray;
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            bool bvalue = (bool)value;
            return !bvalue ? KolorDlaFałszu : KolorDlaPrawdy;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
   
    public class BoolToTextDecorationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            bool bvalue = (bool)value;
            return bvalue ? TextDecorations.Strikethrough : null;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    

}
