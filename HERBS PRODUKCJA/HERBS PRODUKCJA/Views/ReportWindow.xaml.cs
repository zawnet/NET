using FastReport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window, INotifyPropertyChanged
    {
        public DataSet ds;
        public ReportWindow()
        {
            InitializeComponent();
            _report2View = new Report();


            _report2View.Load(@"Raporty\test.frx");
            _report2View.RegisterData(ds, "fzlDataSet1");
            plnPersonForm.DataContext = this;
            
        }
        public ReportWindow(string path, DataSet ds)
        {
            this.ds = ds;
            InitializeComponent();
            _report2View = new Report();


            _report2View.Load(path);
            _report2View.RegisterData(ds, "fzlDataSet1");
            plnPersonForm.DataContext = this;
            //dlc.ExecuteReport();
            //this.Loaded += new RoutedEventHandler(dlReportViewer_ShowReport);
        }

        private Report _report2View;
        public Report Report2View
        {
            get { return _report2View; }
            set
            {
                if (_report2View != value)
                {
                    _report2View = value;
                    OnPropertyChanged("Report2View");

                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void dlReportViewer_ShowReport(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show("Test message ...");
        }

        
    }
}
