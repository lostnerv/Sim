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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Controls.Ribbon;
using System.Data;
using 建模数模.tools;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Threading;
using 建模数模;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;
using 数模建模.tools;

using 数模建模.Drawer;

namespace 数模建模
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public MainWindow()
        {    
            InitializeComponent();
        }

        private void bt_wellLoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                剩余油数据源配置 dlg = new 剩余油数据源配置();
                dlg.Show();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            } 
        }

        private void click_showResByLayer(object sender, RoutedEventArgs e)
        {
            try 
            {
                ResByLayer win = new ResByLayer();
                win.Show();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void bt_compare_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                CompareContainer draw = new CompareContainer("对比");
                draw.Show();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void bt_convertFacies_click(object sender, RoutedEventArgs e)
        {
            XmlHelper helper = new XmlHelper();
            string faciesPath = helper.GetXMLDocument("FACIES"); // "E:\\1.txt";
            string outPath = "D:";
            string fgridpath = helper.GetXMLDocument("FGRID");   //"E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\F10-27RIGHT_E100.FGRID";  
              
            数模建模.SIMB.ConvertFacies convertFacies = new 数模建模.SIMB.ConvertFacies();
            int[] tablesize= convertFacies.readFGRID(fgridpath);
            foreach (int k in tablesize)
            {
                Console.WriteLine(""+k);
            }
            convertFacies.convertFacies(faciesPath, outPath);
        }
    }
}
