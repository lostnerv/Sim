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
using 数模建模.tools;

namespace 数模建模
{
    /// <summary>
    /// 剩余油数据源配置.xaml 的交互逻辑
    /// </summary>
    public partial class 剩余油数据源配置 : Window
    {
        public 剩余油数据源配置()
        {
            InitializeComponent();
            XmlHelper helper = new XmlHelper();
            this.fgrid.filePath.Text = helper.GetXMLDocument("FGRID");
            this.prtinc.filePath.Text = helper.GetXMLDocument("PRTINC");
            this.schinc.filePath.Text = helper.GetXMLDocument("SCH");
            this.gothinc.filePath.Text = helper.GetXMLDocument("GOTH");
            this.faciesinc.filePath.Text = helper.GetXMLDocument("FACIES");
            this.gproinc.filePath.Text = helper.GetXMLDocument("GPRO");
            this.partinc.filePath.Text = helper.GetXMLDocument("partinc");
            //this.finitinc.filePath.Text = helper.GetXMLDocument("FINIT");
        }

        private void simaf_save_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper helper = new XmlHelper();

            if (helper.GetXMLDocument("FGRID") == null)
            {
                helper.AddXMLDocument("FGRID", this.fgrid.filePath.Text);
            }
            else
            {
                helper.EditXMLDocument("FGRID", this.fgrid.filePath.Text);
            }

            if (helper.GetXMLDocument("PRTINC") == null)
            {
                helper.AddXMLDocument("PRTINC", this.prtinc.filePath.Text);
            }
            else
            {
                helper.EditXMLDocument("PRTINC", this.prtinc.filePath.Text);
            }

            if (helper.GetXMLDocument("SCH") == null)
            {
                helper.AddXMLDocument("SCH", this.schinc.filePath.Text);
            }
            else
            {
                helper.EditXMLDocument("SCH", this.schinc.filePath.Text);
            }

            if (helper.GetXMLDocument("GOTH") == null)
            {
                helper.AddXMLDocument("GOTH", this.gothinc.filePath.Text);
            }
            else
            {
                helper.EditXMLDocument("GOTH", this.gothinc.filePath.Text);
            }

            if (helper.GetXMLDocument("FACIES") == null)
            {
                helper.AddXMLDocument("FACIES", this.faciesinc.filePath.Text);
            }
            else
            {
                helper.EditXMLDocument("FACIES", this.faciesinc.filePath.Text);
            }
            // 2017年5月8日 19:32:56 新文件
            if (helper.GetXMLDocument("GPRO") == null)
            {
                helper.AddXMLDocument("GPRO", this.gproinc.filePath.Text);
            }
            else
            {
                helper.EditXMLDocument("GPRO", this.gproinc.filePath.Text);
            }
            // 2017年5月23日 10:16:59 dz
           /* if (helper.GetXMLDocument("FINIT") == null)
            {
                helper.AddXMLDocument("FINIT", this.finitinc.filePath.Text);
            }
            else
            {
                helper.EditXMLDocument("FINIT", this.finitinc.filePath.Text);
            }*/
            // 2017年7月25日 09:47:40
            if (helper.GetXMLDocument("partinc") == null)
            {
                helper.AddXMLDocument("partinc", this.partinc.filePath.Text);
            }
            else
            {
                helper.EditXMLDocument("partinc", this.partinc.filePath.Text);
            }
            MessageBox.Show("保存成功");
        }
    }
}
