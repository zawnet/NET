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
using System.Windows.Shapes;

namespace HERBS_PRODUKCJA.Views
{
    /// <summary>
    /// Interaction logic for ProdukcjaMagazynWindow.xaml
    /// </summary>
    public partial class ProdukcjaMagazynWindow : Window
    {
        public ProdukcjaMagazynDokumentViewModel vm { get; set; }

        public ProdukcjaMagazynWindow()
        {
            InitializeComponent();
        }

        public ProdukcjaMagazynWindow(ProdukcjaMagazynDokumentViewModel obj)
        {
            this.vm = obj;
            InitializeComponent();
        }


    }
}