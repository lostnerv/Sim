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

namespace 数模建模
{
    /// <summary>
    /// DrawContainer.xaml 的交互逻辑
    /// </summary>
    public partial class DrawContainer : Window
    {
        public DrawContainer(string title)
        {
            this.Content = title;
            InitializeComponent();
        }

        private void canves1_MouseWheel_prt(object sender, MouseWheelEventArgs e)
        {
            ReservorDraw.zoom_in(canvesprt1, e);
        }

       
    }
}
