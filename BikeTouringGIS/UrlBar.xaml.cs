﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace BikeTouringGIS
{
    /// <summary>
    /// Interaction logic for UrlBar.xaml
    /// </summary>
    public partial class UrlBar : UserControl
    {
        public UrlBar()
        {
            InitializeComponent();
        }

        private void OpenWebsite(object sender,RequestNavigateEventArgs e)
        {
            var hyperlink = (Hyperlink)e.Source;
            Process.Start(hyperlink.NavigateUri.AbsoluteUri);
        }
    }
}
