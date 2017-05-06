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
using 数模建模.SIMB;
using 数模建模.Drawer;

namespace 数模建模
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        public static DataTable wellList;
        public static string wellSet;
        public DataTable result;
        BackgroundWorker worker = new BackgroundWorker();
        BackgroundWorker simb = new BackgroundWorker();

      

        bool flag_wellhead = false;
        bool flag_welltop = false;
        bool flag_wellsand = false;
        bool flag_wellskew = false;
        bool flag_wellprop = false;
        bool flag_wellpropa = false;
        bool flag_wellpropb = false;
        bool flag_wellcruve = false;
        bool flag_wellpaller = false;
        
        public MainWindow()
        {
            
            InitializeComponent();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            simb.DoWork += new DoWorkEventHandler(simb_DoWork);

        }

        private void simb_DoWork(object sender, DoWorkEventArgs e)
        {
            using (FileStream fsRead = new FileStream(@"D:\新建文件夹 (15)\数模后拟合率计算\f10-27right\F10-27RIGHT_E100.SMSPEC", FileMode.Open))
            {
                int fsLen = (int)fsRead.Length;

                byte[] heByte = new byte[fsLen];
                int r = fsRead.Read(heByte, 0, heByte.Length);
                
                string myStr = System.Text.Encoding.ASCII.GetString(heByte);
                RWFile file = new RWFile();
                file.Append("c:\\123.txt",myStr);
                //Console.WriteLine(myStr);
                //Console.ReadKey();
            } 
        }


        private void OracleConn_Click(object sender, RoutedEventArgs e)
        {         
            //simb.RunWorkerAsync();

            //office模块数据准备。 例子
            // 数模建模.SIMB.OfficePre sns = new 数模建模.SIMB.OfficePre();
            // sns.getGridData("E:\\Documents\\项目开发\\MyWork\\sns\\1\\建模导出文件格式\\grid.txt");
            // sns.writeGgoResult("E:\\Documents\\项目开发\\MyWork\\sns");
            // sns.writeActnumResult("E:\\Documents\\项目开发\\MyWork\\sns");
            // sns.writePermResult("E:\\Documents\\项目开发\\MyWork\\sns");
            // sns.writePoroResult("E:\\Documents\\项目开发\\MyWork\\sns");
            // sns.writeNtgResult("E:\\Documents\\项目开发\\MyWork\\sns");
            // sns.writeCopyResult("E:\\Documents\\项目开发\\MyWork\\sns\\1\\建模导出文件格式\\fault.txt", "E:\\Documents\\项目开发\\MyWork\\sns\\GOTH.INC");
            // sns.writeCopyResult("E:\\Documents\\项目开发\\MyWork\\sns\\1\\建模导出文件格式\\well connection.txt", "E:\\Documents\\项目开发\\MyWork\\sns\\jg.trj");
            // sns.writeEqlnumResult("E:\\Documents\\项目开发\\MyWork\\sns");

            /*
            sns.getZ25sumData("E:\\Documents\\项目开发\\MyWork\\sns\\1\\建模导出文件格式\\Z5 sum.txt");
            foreach (DataRow row in sns.z25sumDT.Rows)
            {
                string data = row["SUM"].ToString();
                System.Console.WriteLine(data);
            }
            System.Console.WriteLine("-------------");
            sns.addZ25sumData("aaa77777");
            sns.delZ25sumData("TCPUTS");
            sns.updateZ25sumData("STEPTYPE","Jony"); ;

            System.Console.WriteLine("-------------");
            foreach (DataRow row in sns.z25sumDT.Rows)
            {
                string data = row["SUM"].ToString();
                System.Console.WriteLine(data);
            }
            System.Console.WriteLine("-------------");

            */
            //　　提取并生成油井、水井井史文件history.vol前 数据准备
            DataTable dt=  new DataTable(); 
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column.ColumnName = "tmp";
            dt.Columns.Add(column);
            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column2.ColumnName = "yt";
            dt.Columns.Add(column2);
            DataColumn column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column3.ColumnName = "cw";
            dt.Columns.Add(column3);

            DataRow row = dt.NewRow();
            row[0] = "8B541-1";
            row[1] = "肇州";
            row[2] = "P";
            dt.Rows.Add(row);
            DataRow row2 = dt.NewRow();
            row2[0] = "8F154-100";
            row2[1] = "肇州";
            row2[2] = "P";
            dt.Rows.Add(row2);
            DataRow row3 = dt.NewRow();
            row3[0] = "8X50-67";
            row3[1] = "肇州";
            row3[2] = "P";
            dt.Rows.Add(row3);
            
            数模建模.SIMB.SchPre sns = new 数模建模.SIMB.SchPre(dt);
            sns.writeCs("E:\\Documents\\项目开发\\MyWork\\8");
            /*
            // 虚拟井号
            sns.saveorUpdateWellXML("8B541-1", "well0123");
            sns.saveorUpdateWellXML("8F154-100", "well911");
            //　　提取并生成油井、水井井史文件history.vol
            sns.writeHistory("E:\\Documents\\项目开发\\MyWork\\8");
            sns.writeSk("E:\\Documents\\项目开发\\MyWork\\8");//,String unknowNum="0.1397")
            sns.writeWellnet("E:\\Documents\\项目开发\\MyWork\\8");
            sns.writeLimit("E:\\Documents\\项目开发\\MyWork\\8","350");
             * */

            ////////////拟合///////////
            //手写的datatable
            
            /*DataTable dt = new DataTable();

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column.ColumnName = "tmp";
            dt.Columns.Add(column);
            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column2.ColumnName = "yt";
            dt.Columns.Add(column2);
            DataColumn column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.String");//该列的数据类型 
            column3.ColumnName = "cw";
            dt.Columns.Add(column3);

            DataRow row = dt.NewRow();
            row[0] = "2011-10-1";
            row[1] = "0.36452517";
            row[2] = "0.37713677";
            dt.Rows.Add(row);
            DataRow row2 = dt.NewRow();
            row2[0] = "2011-10-1";
            row2[1] = "0";
            row2[2] = "0";
            dt.Rows.Add(row2);
            DataRow row3 = dt.NewRow();
            row3[0] = "2011-10-5";
            row3[1] = "0.36223012";
            row3[2] = "0.37713677";

            dt.Rows.Add(row3);*/
            ////////////拟合///////////
            
           // 数模建模.SIMB.Fitting sns = new 数模建模.SIMB.Fitting(dt,1);
           // sns.importExcelToDataSet("E:\\Documents\\项目开发\\MyWork\\8\\需求\\拟合率.xls");
            /* System.Console.WriteLine("-------------");
             System.Console.WriteLine(sns.getNgNum());
             System.Console.WriteLine(sns.getNgRate());
             DataTable dtaa = sns.getTable();
             foreach (DataRow rowaa in dtaa.Rows)
             {
                 System.Console.WriteLine(rowaa[0].ToString());
                 System.Console.WriteLine(rowaa[1].ToString());
                 System.Console.WriteLine(rowaa[2].ToString());
                 System.Console.WriteLine(rowaa[3].ToString());
             }
             System.Console.WriteLine("-------------");
            
             */

           /////test.FGRID+test.PRT
           /* 数模建模.SIMB.FgridPrt sns = new 数模建模.SIMB.FgridPrt();
            sns.readFGRID("E:\\Documents\\项目开发\\MyWork\\8\\需求\\test.FGRID");
            DataTable dtprt = sns.readPRT("E:\\Documents\\项目开发\\MyWork\\8\\需求\\test.PRT");
            for (int i = 0; i < 112; i++)
                System.Console.WriteLine(dtprt.Rows[43][i].ToString());*/

            /////沉积相带图
            /*数模建模.SIMB.FgridPline sns = new 数模建模.SIMB.FgridPline();
            DataTable dtprt = sns.readPRT("E:\\Documents\\项目开发\\MyWork\\8\\需求\\沉积相带.txt");
            foreach (DataRow rowaa in dtprt.Rows)
            {
                System.Console.WriteLine(rowaa[0].ToString());
                System.Console.WriteLine(rowaa[1].ToString());
                System.Console.WriteLine("-------------");
            }*/


            /*饱和度新图
            数模建模.SIMB.FgridNew sns = new 数模建模.SIMB.FgridNew();
            sns.readFile("E:\\Documents\\项目开发\\MyWork\\8\\需求\\test.FGRID");*/
            /////////以上为我的测试/////////

        }

        private void Daa01Reader_Click(object sender, RoutedEventArgs e)
        {
            wellSet = "";
            foreach (DataRow tempRow in Data_Result.result_temp_welllist.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);

            string sql = "select * from daa01 where jh in (" + wellSet + ")";
            result = GetDataAsDataTable.GetDataReasult(sql);
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
        }

        private void ExcelConn_Click(object sender, RoutedEventArgs e)
        {
            String resultPath = null;
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                resultPath = openFileDialog.FileName;
            }

            DataTable dt = GetDataAsDataTable.LoadDataFromExcel(resultPath);

            //由于Excel文件中没有列名，默认将数据第一行的数据值作为列名。
            //这里将数据复制下来，为Datatable添加列名后，将复制数据追加到DataTable末端。
            if (dt != null)
            {
                string welltemp = dt.Columns[0].ColumnName;
                dt.Columns[0].ColumnName = "wellnum";
                DataRow dr = dt.NewRow();
                dr["wellnum"] = welltemp;
                dt.Rows.Add(dr);
                //wellList.Clear();
                wellList = dt;

                foreach (DataRow tempRow in wellList.Rows)
                {
                    wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
                }
                wellSet = wellSet.Substring(0, wellSet.Length - 1);

                this.LeftMain.wellList.ItemsSource = dt.DefaultView;
                this.LeftMain.wellList.SelectedValuePath = "wellnum";
                this.LeftMain.wellList.DisplayMemberPath = "wellnum";
            }
        }

        private void SimB_output_Click(object sender, RoutedEventArgs e)
        {
            flag_wellhead = Convert.ToBoolean(this.ck_wellhead.IsChecked);
            flag_welltop = Convert.ToBoolean(this.ck_welltop.IsChecked);
            flag_wellprop = Convert.ToBoolean(this.ck_wellprop.IsChecked);
            flag_wellskew = Convert.ToBoolean(this.ck_wellskew.IsChecked);
            flag_wellsand = Convert.ToBoolean(this.ck_wellsand.IsChecked);
            flag_wellpropa = Convert.ToBoolean(this.ck_wellpropA.IsChecked);
            flag_wellpropb = Convert.ToBoolean(this.ck_wellpropB.IsChecked);
            flag_wellcruve = Convert.ToBoolean(this.ck_wellcruve.IsChecked);
            flag_wellpaller = Convert.ToBoolean(this.ck_paller.IsChecked);

            worker.RunWorkerAsync();
        }

        private void Daa074Reader_Click(object sender, RoutedEventArgs e)
        {
            wellSet = "";
            foreach (DataRow tempRow in Data_Result.result_temp_welllist.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);

            string sql = "select * from daa074 where jh in (" + wellSet + ")";
            result = GetDataAsDataTable.GetDataReasult(sql);
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
        }

        private void Daa071Reader_Click(object sender, RoutedEventArgs e)
        {
            wellSet = "";
            foreach (DataRow tempRow in Data_Result.result_temp_welllist.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);

            string sql = "select * from daa071 where jh in (" + wellSet + ")";
            result = GetDataAsDataTable.GetDataReasult(sql);
            DataTable haha = new DataTable();
            haha = result.Clone();
            foreach (DataRow tempRow in Data_Result.result_temp_welllist.Rows)
            {
                string jh = tempRow[0].ToString().Trim();
                DataRow[] matches = result.Select(" jh= '" + jh + "'", "xcxh asc");
                foreach (DataRow r1 in matches)
                {
                    DataRow row = haha.NewRow();
                    row[0] = r1[0];
                    row[1] = r1[1];
                    row[2] = r1[2];
                    row[3] = r1[3];
                    row[4] = r1[4];
                    row[5] = r1[5];
                    haha.Rows.Add(row);
                }
            }
            Data_Result.result_temp_zhongzhuan = haha;
            this.RightMain.dataGrid1.ItemsSource = haha.DefaultView;
        }

        private void Daa054Reader_Click(object sender, RoutedEventArgs e)
        {
            wellSet = "";
            foreach (DataRow tempRow in Data_Result.result_temp_welllist.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);

            string sql = "select * from daa054 where jh in (" + wellSet + ")";
            result = GetDataAsDataTable.GetDataReasult(sql);
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            XmlHelper OutputPath = new XmlHelper();
            string Opath = OutputPath.GetXMLDocument("simboutput");

            if (flag_wellhead)
            {
                Wellhead wellhead = new Wellhead(wellList);
                wellhead.getData();
                XmlHelper helper = new XmlHelper();
                string path = helper.GetXMLDocument("wellhead");
                wellhead.getDataFromLocal(path);

                //直接输出
                result = wellhead.WriteResultData();
                Data_Result.result_temp_zhongzhuan = result;
                wellhead.WriteResult(Opath + "\\wellhead.txt");
            }
            if (flag_welltop)
            {
                Welltop welltop = new Welltop(wellList);
                welltop.getData();
                welltop.WriteResult(Opath + "\\welltop.txt");
            }
            if (flag_wellprop)
            {
                Wellprop wellprop = new Wellprop(Data_Result.result_temp_welllist);
                wellprop.getData();
                //result = wellprop.WriteResultData();
                wellprop.WriteResult(Opath + "\\岩性");
            }
            if (flag_wellpropa)
            {
                WellpropA wellpropa = new WellpropA(wellList);
                result = wellpropa.getData();
                wellpropa.WriteResult(Opath + "\\井属性数据库输出\\");
            }
            if (flag_wellsand)
            {
                WellSand wellsand = new WellSand(wellList);
                wellsand.getData();
                wellsand.WriteResult(Opath + "\\折算砂岩\\");
            }
            if (flag_wellpropb)
            {
                //WellpropB propb = new WellpropB();
                //propb.getData();
            }
            if (flag_wellpaller)
            {
                XmlHelper helper = new XmlHelper();
                string path = helper.GetXMLDocument("paller");
                Paller paller = new Paller();
                paller.getData(path);
                System.Console.WriteLine(Opath + "\\paller.txt");
                paller.WriteResult(Opath + "\\paller.txt");
                
            }
            if (flag_wellskew)
            {
                XmlHelper helper = new XmlHelper();
                string path = helper.GetXMLDocument("wellskew");
                DirectoryInfo folder = new DirectoryInfo(path);

                //if (!Directory.Exists(Opath + "\\井斜\\"))
                //{
                //    Directory.CreateDirectory(path);
                //}

                foreach (FileInfo file in folder.GetFiles())
                {
                    //Wellskew well = new Wellskew(wellList);
                    
                    //string temp_name = "8"+file.Name.ToString().ToUpper();
                    //DataRow[] dataRows = wellList.Select("wellnum"+"='"+temp_name+"'");
                    //if (dataRows.Length >= 1)
                    //{
                    //    well.getDate(file.FullName, "DEV");
                    //    well.WriteResult("D:\\result\\" + temp_name);
                    //}
                    //System.Console.WriteLine(temp_name);

                    //well.WriteResult1("D:\\result\\" + temp_name);
                }
            }
            if (flag_wellcruve)
            {
                //Wellcruve curve = new Wellcruve();
                //XmlHelper helper = new XmlHelper();
                //string path = helper.GetXMLDocument("wellcruve");
                //DirectoryInfo folder = new DirectoryInfo(path);
                //foreach (FileInfo file in folder.GetFiles("*.txt"))
                //{
                //    Wellskew well = new Wellskew();
                //    well.getDate(file.FullName, "DEP");
                //    well.WriteResult(Opath + "\\wellcruve\\" + file.Name);
                //}
            }
            //string path;
            //System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            //fbd.ShowDialog();
            //if (fbd.SelectedPath != string.Empty)
            //    path = fbd.SelectedPath;
            
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            System.Windows.MessageBox.Show("正在处理", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("处理完成", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information);
        }

        private void bt_wellhead_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper OutputPath = new XmlHelper();
            string Opath = OutputPath.GetXMLDocument("simboutput");
            Wellhead wellhead = new Wellhead(Data_Result.result_temp_welllist);
            result = wellhead.getData();
            wellhead.WriteResult_test(Opath + "\\wellhead.txt");
            //XmlHelper helper = new XmlHelper();
            //string path = helper.GetXMLDocument("wellhead");

            //result = wellhead.getDataFromLocal(path);

            ////result = wellhead.getData();
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;

            
        }

        private void bt_welltop_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper OutputPath = new XmlHelper();
            string Opath = OutputPath.GetXMLDocument("simboutput");

            Welltop welltop = new Welltop(Data_Result.result_temp_welllist);
            result = welltop.getData();
            Data_Result.result_temp_zhongzhuan = result;
            welltop.WriteResult(Opath + "\\welltop.txt");
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
            //welltop.WriteResult(resultPath);
        }

        private void bt_wellprop_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper OutputPath = new XmlHelper();
            string Opath = OutputPath.GetXMLDocument("simboutput");

            Wellprop wellprop = new Wellprop(Data_Result.result_temp_welllist);
            result = wellprop.getData();
            Data_Result.result_temp_zhongzhuan = result;

            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
            wellprop.WriteResultData();
            //wellprop.WriteResult(Opath + "\\岩性");
            
        }

        private void bt_wellpropA_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper OutputPath = new XmlHelper();
            string Opath = OutputPath.GetXMLDocument("simboutput");

            WellpropA wellpropa = new WellpropA(Data_Result.result_temp_welllist);
            result = wellpropa.getData();
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
            wellpropa.WriteResult(Opath + "\\井属性数据库输出\\");
        }

        private void bt_wellskew_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper OutputPath = new XmlHelper();
            string Opath = OutputPath.GetXMLDocument("simboutput");

            Wellskew well = new Wellskew(Data_Result.result_temp_welllist);
            Data_Result.result_temp_zhongzhuan = result;
            result = well.getDateFromDatabase();
            well.WriteResult3(Opath + "\\井斜\\");
        }

        private void bt_wellpropB_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper OutputPath = new XmlHelper();
            string Opath = OutputPath.GetXMLDocument("simboutput");

            WellpropA wellpropa = new WellpropA(Data_Result.result_temp_welllist);
            result = wellpropa.getData();
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
            //WellpropB propb = new WellpropB();
            //propb.getData();
        }

        private void bt_wellsand_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper OutputPath = new XmlHelper();
            string Opath = OutputPath.GetXMLDocument("simboutput");

            XmlHelper helper = new XmlHelper();
            string path = helper.GetXMLDocument("sandvalue");
            WellSand sand = new WellSand(Data_Result.result_temp_welllist);
            result = sand.getData(path);
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
        }

        private void bt_wellcruve_Click(object sender, RoutedEventArgs e)
        {
            Wellcruve well = new Wellcruve();
            well.getData("D:\\新建文件夹 (15)\\8f158-122.txt");
            well.WriteResult("D:\\result\\test.txt");
        }

        private void bt_paller_Click(object sender, RoutedEventArgs e)
        {
            XmlHelper helper = new XmlHelper();
            string path = helper.GetXMLDocument("paller");
            Paller paller = new Paller();
            result = paller.getData(path);
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
            paller.WriteResult("C:\\paller");
        }

        private void bt_summary_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DataCheck_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void DataCheck_Click_1(object sender, RoutedEventArgs e)
        {
            DataCheck data = new DataCheck(Data_Result.result_temp_welllist);
            result = data.CheckData();
            Data_Result.result_temp_zhongzhuan = result;
            this.RightMain.dataGrid1.ItemsSource = result.DefaultView;
        }

        private void bt_his_Click(object sender, RoutedEventArgs e)
        {
            SchPre sch = new SchPre(Data_Result.result_temp_welllist);
            sch.writeHistory("D:\\test", "永乐", "P");
            this.RightMain.dataGrid1.ItemsSource = sch.resultData.DefaultView;
        }

        private void bt_sk_Click(object sender, RoutedEventArgs e)
        {
            SchPre sch = new SchPre(Data_Result.result_temp_welllist);
            sch.writeSk("D:\\test");
        }

        private void bt_cs_Click(object sender, RoutedEventArgs e)
        {
            SchPre sch = new SchPre(Data_Result.result_temp_welllist);
            sch.writeCs("D:\\test");
        }

        private void bt_net_Click(object sender, RoutedEventArgs e)
        {
            SchPre sch = new SchPre(Data_Result.result_temp_welllist);
            sch.writeWellnet("D:\\test");
        }

        private void bt_limit_Click(object sender, RoutedEventArgs e)
        {
            SchPre sch = new SchPre(Data_Result.result_temp_welllist);
            sch.writeLimit("D:\\test","45");
        }

        private void bt_grid_Click(object sender, RoutedEventArgs e)
        {
            OfficePre office = new OfficePre();
            //office.getGridData();
            office.writeGgoResult("D:\\test");
            office.writeActnumResult("D:\\test");
            office.writePermResult("D:\\test");
            office.writePoroResult("D:\\test");
            office.writeNtgResult("D:\\test");
            office.writeEqlnumResult("D:\\test");
            //office.writeCopyResult("D:\\test");
        }

        private void bt_wellLoc_Click(object sender, RoutedEventArgs e)
        {
            剩余油数据源配置 dlg = new 剩余油数据源配置();
            dlg.Show();
            //Data_Result.result_temp_zhongzhuan = ReservorDraw.getDataByArea(this.RightMain.canves1,Data_Result.result_temp_welllist);
            //this.RightMain.dataGrid1.ItemsSource = Data_Result.result_temp_zhongzhuan.DefaultView;
            //ReservorDraw.DrawPoint(this.RightMain.canves1);
            //ReservorDraw.Cal_Well_Area();
            //foreach (DataRow row in Data_Result.result_temp_welllist.Rows)
            //{
         
            //    ReservorDraw.Cal_Well_Square(row[0].ToString());
            //}

            //foreach (DataRow row in ReservorDraw.result.Rows)
            //{
            //    //System.Console.WriteLine(row[0].ToString() + "@" + row[1].ToString() + "@" + row[2].ToString());
            //}
            
        }

        private void bt_wellLocZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ReservorDraw.test(this.RightMain.canves1);
            ReservorDraw.printPointInPy();
        }

        private void bt_move_Click(object sender, RoutedEventArgs e)
        {
            int haha = VisualTreeHelper.GetChildrenCount(this.RightMain.canves1);
            System.Console.WriteLine(haha);
            //Point pp = new Point();
            //pp.X = -1000;
            //pp.Y = 100;
            //ReservorDraw.move(this.RightMain.canves1, pp);
            ////draw.zoom_in(this.RightMain.canves1);
            //ReservorDraw.DrawPoint(this.RightMain.canves1);     
        }
        /*
        private void bt_prt_Click(object sender, RoutedEventArgs e)
        {
            
            // 用prt画饱和度
            //Canvas canves = this.RightMain.canvesprt;
            //数模建模.SIMB.FgridPrt sns = new 数模建模.SIMB.FgridPrt();
            //int[] tablesize = sns.readFGRID("E:\\Documents\\项目开发\\MyWork\\8\\需求\\test.FGRID");
            //DataTable dtprt = sns.readPRT("E:\\Documents\\项目开发\\MyWork\\8\\需求\\test.PRT");
            //int picsize = 3;// 方块大小
            ///*double maxPRT = 0;
            //double minPRT = 999;
            //for (int i = 0; i < tablesize[0]; i++)
            //{
            //    double tmpMax = Convert.ToDouble(dtprt.Compute("max(v" + (i + 1) + ")", ""));
            //    double tmpMin=999;
            //    try//过滤0后 如果没有数据 报错
            //    {
            //        tmpMin = Convert.ToDouble(dtprt.Compute("min(v" + (i + 1) + ")", "v" + (i + 1) + ">0"));
            //    }
            //    catch
            //    { 
                
            //    }
            //   if (maxPRT < tmpMax)
            //   {
            //       maxPRT = tmpMax;
            //   }
            //   if (minPRT > tmpMin)
            //   {
            //       minPRT = tmpMin;
            //   }
            //}
            //System.Console.WriteLine("x," + maxPRT + "," + minPRT);*/
            //double valBottom = 113;
            //double inVal = 1;
            //double valTop = inVal * 4 + valBottom;
            //for (int i = 0; i < tablesize[1]; i++)
            //{
            //    for (int j = 0; j < tablesize[0]; j++)//先画第一列
            //    {
            //       // System.Console.WriteLine(dtprt.Rows[i][j]);
            //        double val=Convert.ToDouble( dtprt.Rows[i][j]);
            //        if(val>0)
            //        {
            //            //string[] valArr = val.Split(new char[2] { '.', '*' });
            //            //int val1=int.Parse(valArr[0]);
            //            //int val2 =int.Parse(valArr[1]);
            //            // val = val.Replace(".", "");
            //            //val = val.Replace("*", "");
            //            int val0 = Convert.ToInt32(100 * val);
            //            Rectangle rect = new Rectangle();
            //            rect.Width = picsize;
            //            rect.Height = picsize;
            //            System.Windows.Media.Color myColor = new System.Windows.Media.Color();
            //            myColor.A = 255;
            //            if (val <= valBottom)
            //            {
            //                myColor.R = 0;
            //                myColor.G = 0;
            //                myColor.B = 255;
            //            }
            //            else if (val >=
            //                valTop)
            //            {
            //                myColor.R = 255;
            //                myColor.G = 0;
            //                myColor.B = 0;
            //            }
            //            else
            //            {
            //                double colorFlag = val - valBottom;
            //                if (colorFlag <= inVal)
            //                {
            //                    myColor.R = 0;
            //                    myColor.G = (byte)(255 / inVal * colorFlag);
            //                    myColor.B = 255;
            //                }
            //                else if (colorFlag <= inVal*2)
            //                {
            //                    myColor.R = 0;
            //                    myColor.G = 255;
            //                    myColor.B = (byte)(255 - 255 / inVal * (colorFlag - inVal));
            //                }
            //                else if (colorFlag <= inVal*3)
            //                {
            //                    myColor.R = (byte)(255 / inVal * (colorFlag - inVal*2));
            //                    myColor.G = 255;
            //                    myColor.B = 0;
            //                }
            //                else if (colorFlag <= inVal*4)
            //                {
            //                    myColor.R = 255;
            //                    myColor.G = (byte)(255 - 255 / inVal * (colorFlag - inVal * 3));
            //                    myColor.B = 0;
            //                }
            //            }
            //            /*
            //            myColor.R = (byte)((val0 / 100000 * 6 + val0 % 100000 / 10000 * 5 + val0 % 10000 / 1000 * 4
            //                + val0 % 1000 / 100 * 3 + val0 % 100 / 10 * 2 + val0 % 10 * 1) % 255);
            //            myColor.G = (byte)((val0 / 100000 * 6 + val0 % 100000 / 10000 * 5 + val0 % 10000 / 1000 * 4
            //                + val0 % 1000 / 100 * 3 + val0 % 100 / 10 * 2 + val0 % 10 * 1) % 255);
            //            myColor.B = (byte)((val0 / 100000 * 6 + val0 % 100000 / 10000 * 5 + val0 % 10000 / 1000 * 4
            //                + val0 % 1000 / 100 * 3 + val0 % 100 / 10 * 2 + val0 % 10 * 1) % 255);*/
            //            System.Windows.Media.SolidColorBrush myBrush = new System.Windows.Media.SolidColorBrush(myColor);
            //            rect.Fill = myBrush;
            //            Canvas.SetLeft(rect, i * picsize);
            //            Canvas.SetTop(rect, j * picsize);
            //            canves.Children.Add(rect);
            //        }
            //    }
            //}
            
        /*
            // 沉积相带
            Canvas canves2 = this.RightMain.canvespoly;

            canves2.Children.Clear();
            数模建模.SIMB.DFDPoly poly = new 数模建模.SIMB.DFDPoly();
            // 解析文件
            DataTable dtpoly = poly.readPRT("E:\\Documents\\项目开发\\MyWork\\8\\需求\\八厂相图_P13.dfd");
            int listCount = 0;
            double dzoomx, dzoomy;
            bool isSand=false;//砂体
            String sandLengthStr="20";
            double sandLengthD=0.0;
            try {
                sandLengthD = Convert.ToDouble(sandLengthStr);
            }
            catch {
                System.Console.WriteLine("断层距离格式错误");
            }
            // 计算缩放比例
            double maxX = Convert.ToDouble(dtpoly.Compute("max(x)", ""));
            double maxY = Convert.ToDouble(dtpoly.Compute("max(y)", ""));
            double minX = Convert.ToDouble(dtpoly.Compute("min(x)", ""));
            double minY = Convert.ToDouble(dtpoly.Compute("min(y)", ""));
            dzoomx = canves2.ActualWidth / (maxX - minX);
            dzoomy = canves2.ActualHeight / (maxY - minY);

            double m_d_zoomfactor = dzoomx > dzoomy ? dzoomx : dzoomy;            
            // System.Console.WriteLine("x," + maxX + "," + minX);
            // System.Console.WriteLine("y" + maxY + "," + minY);
            foreach (int startPointNum in poly.clist)
            {
                Boolean isBorder = true;
                listCount++;
                int endPointNum = 0;
                if (listCount < poly.clist.Count)//第listCount个图形
                {
                    endPointNum = (int)poly.clist[listCount];//起点是1 记录下个图形的开始行
                }
                else
                {
                    //maxc = dtpoly.Rows.Count;
                    break;
                }
                Polygon myPolygon = new Polygon();
                Polyline polyline = new Polyline();
                Polygon sandPolygon = new Polygon();
                PointCollection myPointCollection = new PointCollection();
                PointCollection sandPointCollection = new PointCollection();
                if ("0" == (string) poly.colorList[listCount - 1])//color是0的只有黑边
                {
                    isBorder = true;
                    polyline.Stroke = System.Windows.Media.Brushes.Black;
                    polyline.StrokeThickness = 0.4;
                }
                else
                {
                    isBorder = false;
                    System.Windows.Media.Color myColor2 = new System.Windows.Media.Color();
                   // Random ra = new Random();
                   //  ra = new Random();
                   //  ra = new Random();
                    String colorStr = (string)poly.colorList[listCount - 1];
                    colorStr = colorStr.Substring(1);
                    if("65280".Equals(colorStr))
                    {
                        isSand=true;
                    }
                    else{
                        isSand=false;
                    }
                    long colorl = long.Parse(colorStr);
                    long colorlB = colorl / 65536;
                    long colorlG = (colorl - colorlB * 65536) / 256;
                    long colorlR = (colorl - colorlB * 65536 - colorlG * 256);
                    myColor2.A = 255;
                    myColor2.R = (byte)colorlR;
                    myColor2.G = (byte)colorlG;
                    myColor2.B = (byte)colorlB;
                                   
                    System.Windows.Media.SolidColorBrush myBrush2 = new System.Windows.Media.SolidColorBrush(myColor2);
                    myPolygon.Fill = myBrush2;
                    myPolygon.Stroke = System.Windows.Media.Brushes.Black;
                    myPolygon.StrokeThickness = 0.05;
                }
               // System.Console.WriteLine(a+":");
               // System.Console.WriteLine(dtpoly.Rows[a][0] + "," + dtpoly.Rows[a][1]);
                
                for (int i = startPointNum; i < endPointNum; i++)
                {
                    double x = Math.Round((Convert.ToDouble(dtpoly.Rows[i][0]) - minX) * m_d_zoomfactor, 1);//- 728
                    //canvas 不是标准坐标轴
                    double y = Math.Round((maxY-Convert.ToDouble(dtpoly.Rows[i][1]) ) * m_d_zoomfactor, 1);
                    //System.Console.WriteLine(x + "x" + y);
                    //dtpoly.Rows[i][1] = y;
                    //dtpoly.Rows[i][2] = x;
                    // System.Console.WriteLine(x+","+y);
                    Point Point = new System.Windows.Point(x, y);
                    myPointCollection.Add(Point);                   
                }
                List<Point> pointL=new List<Point>();
                foreach (Point greenPoint in myPointCollection)
                {
                    pointL.Add(new Point(greenPoint.X, greenPoint.Y));
                }
                for (int i = startPointNum; i < endPointNum; i++)
                {
                    //System.Console.WriteLine(Convert.ToDouble(dtpoly.Rows[i][0]));
                    //System.Console.WriteLine(minX);
                    //System.Console.WriteLine(m_d_zoomfactor);
                    double x = Math.Round((Convert.ToDouble(dtpoly.Rows[i][0]) - minX) * m_d_zoomfactor, 1);
                    double y = Math.Round((maxY - Convert.ToDouble(dtpoly.Rows[i][1])) * m_d_zoomfactor, 1);
                   // System.Console.WriteLine(x + "x" + y);
                    if (isSand && sandLengthD > 0)
                    {
                      
                        double x2, y2, x3, y3;
                        if (i + 1 < endPointNum)
                        {
                            x2 = Math.Round((Convert.ToDouble(dtpoly.Rows[i + 1][0]) - minX) * m_d_zoomfactor, 1);
                            y2 = Math.Round((maxY - Convert.ToDouble(dtpoly.Rows[i + 1][1])) * m_d_zoomfactor, 1);
                        }
                        else
                        {
                            x2 = Math.Round((Convert.ToDouble(dtpoly.Rows[startPointNum][0]) - minX) * m_d_zoomfactor, 1);
                            y2 = Math.Round((maxY - Convert.ToDouble(dtpoly.Rows[startPointNum][1])) * m_d_zoomfactor, 1);
                        }
                       // System.Console.WriteLine(x2 + "x" + y2);
                        double fx1, fx2;
                        fx1 = Math.Sqrt(Math.Pow(sandLengthD, 2) / (1 + Math.Pow((x - x2) / (y - y2), 2)));
                        fx2 = -fx1;
                        x3 = fx1 + (x + x2) / 2;
                        y3 = (y + y2) / 2 - fx1 * (x - x2) / (y - y2);
                        if (!ReservorDraw.isInRegion(new Point(x3, y3), ReservorDraw.pointList))
                        {
                            x3 = fx2 + (x + x2) / 2;
                            y3 = (y + y2) / 2 - fx2 * (x - x2) / (y - y2);
                        }

                        if (x3 > 0 && y3 > 0)
                        {
                            x3 = Math.Round(x3, 1);
                            y3 = Math.Round(y3, 1);
                            sandPointCollection.Add(new System.Windows.Point(x3, y3));
                        }
                         //System.Console.WriteLine(x3 + "x" + y3);
                        
                    }
                }
                sandPolygon.Points = sandPointCollection;
                sandPolygon.Stroke = System.Windows.Media.Brushes.Black;
                sandPolygon.StrokeThickness = 0.1;

                if (isBorder)
                {
                    polyline.Points = myPointCollection;
                    canves2.Children.Add(polyline);
                }
                else
                {
                    myPolygon.Points = myPointCollection;
                    canves2.Children.Add(myPolygon);
                }
                //画得乱七八糟
               // canves2.Children.Add(sandPolygon);                
            }
        }*/

        private void click_showResByLayer(object sender, RoutedEventArgs e)
        {
            ResByLayer win = new ResByLayer();
            win.Show();
        }

        private void bt_compare_Click(object sender, RoutedEventArgs e)
        {
            CompareContainer draw = new CompareContainer("对比");
            draw.Show();
        }
        private void bt_convertFacies_click(object sender, RoutedEventArgs e)
        {
            string faciesPath = "E:\\1.txt";
            string outPath = "D:";
            string fgridpath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\F10-27RIGHT_E100.FGRID";           
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
