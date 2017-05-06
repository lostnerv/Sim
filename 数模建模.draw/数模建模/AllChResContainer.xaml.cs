using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using 数模建模.Drawer;
using System.Collections.ObjectModel;
using System.Data;
using System.Collections;

namespace 数模建模
{
    public partial class AllChResContainer : Window
    { 
        public AllChResContainer(string title,DataTable dt)
        {
            this.Content = title;
            InitializeComponent();
            this.dataGrid.ItemsSource = dt.DefaultView;
        }
       
    }
}