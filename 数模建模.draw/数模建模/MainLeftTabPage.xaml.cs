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
using System.Data;
using System.ComponentModel;
using 建模数模.tools;
using 数模建模.tools;

namespace 数模建模
{
    /// <summary>
    /// WellListTabPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainLeftTabPage : UserControl
    {
        BackgroundWorker worker = new BackgroundWorker();

        public static DataTable welllistData;

        public MainLeftTabPage()
        {
            InitializeComponent();
        }

        private void wellList_Initialized(object sender, EventArgs e)
        {
            XmlHelper helper = new XmlHelper();
            string path = helper.GetXMLDocument("wellnum");

            DataTable dt = GetDataAsDataTable.LoadDataFromExcel(path);

            ////由于Excel文件中没有列名，默认将数据第一行的数据值作为列名。
            ////这里将数据复制下来，为Datatable添加列名后，将复制数据追加到DataTable末端。
            if (dt != null)
            {
                string welltemp = dt.Columns[0].ColumnName;
                dt.Columns[0].ColumnName = "wellnum";
                DataRow dr = dt.NewRow();
                dr["wellnum"] = welltemp;
                dt.Rows.Add(dr);
                //wellList.Clear();
                //wellList = dt;
                Data_Result.result_temp_welllist = dt;
                this.wellList.ItemsSource = dt.DefaultView;
                this.wellList.SelectedValuePath = "wellnum";
                this.wellList.DisplayMemberPath = "wellnum";
            }

        }

        private void WellBlock_Initialized(object sender, EventArgs e)
        {
            
        }
    }
}
