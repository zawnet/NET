﻿using HERBS_PRODUKCJA.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for SpecyfikacjePakowaniaLista.xaml
    /// </summary>
    public partial class SpecyfikacjePakowaniaLista : UserControl
    {
        public SpecyfikacjePakowaniaLista()
        {
            this.DataContext = new WydaniaTwViewModel();
            InitializeComponent();
        }
        public SpecyfikacjePakowaniaLista(WydaniaTwViewModel vm)
        {
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
