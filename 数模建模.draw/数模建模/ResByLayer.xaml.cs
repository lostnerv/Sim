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
using System.Data;
using System.Collections;

namespace 数模建模
{
    /// <summary>
    /// ResByLayer.xaml 的交互逻辑
    /// </summary>
    public partial class ResByLayer : Window
    {
        public ResByLayer()
        {
            InitializeComponent();
        }
      
        private void time_analyze(object sender, RoutedEventArgs e)
        {
            String prtpath = this.prt_file.filePath.Text; 
            数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
            List<String> soiltimeList = fgridPrt.readSoilTime(prtpath);
            foreach (string soiltime in soiltimeList)
            {
                this.combo_soiltime.Items.Add(soiltime);
            }
            this.combo_soiltime.SelectedIndex = 0;   
        }

        private void cal_res(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            double ro = 0;
            double b0 = 0;
            double allVol = 0;
            double timed = ts.TotalSeconds;
           // int ch = 0;
            string prtpath = this.prt_file.filePath.Text;
            string fgridpath = fgrid_file.filePath.Text;
            string combo_soiltimeStr = this.combo_soiltime.Text;
            int[] tablesize = { 0, 0, 0 };
            DataTable resByLayerDT = new DataTable();            
           
            //
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "ch";
            resByLayerDT.Columns.Add(column);

            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.Double");
            column2.ColumnName = "val";
            resByLayerDT.Columns.Add(column2);

            数模建模.SIMB.FgridPrt prodF = new 数模建模.SIMB.FgridPrt();
            tablesize = prodF.readFGRID(fgridpath);
           
            try
            {
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch
            {
                System.Console.WriteLine("density格式错误");
            }

            for (int ch = 1; ch <= tablesize[2]; ch++)
            {
                System.Console.WriteLine(ch + "/" + tablesize[2] + " 开始解析文件:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
                double oneChVol = 0;
                数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
                数模建模.SIMB.FgridNew fgridNew = new 数模建模.SIMB.FgridNew();
                // 解析文件
                fgridPrt.x = tablesize[0];
                fgridPrt.y = tablesize[1];
                fgridPrt.z = tablesize[2];
                DataTable dtfgridNew = fgridNew.readFile(fgridpath, ""+ch);
                DataTable dtprt = fgridPrt.readPRT(prtpath, "" + ch, combo_soiltimeStr);
                double[] poro = fgridPrt.poro;//孔隙度
                double[] permx = fgridPrt.permx;//渗透率
                DataTable dzDt = fgridPrt.dzDt;//厚度
              
                //System.Console.WriteLine("解析结束:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
                //求极值
                // 计算缩放比例
                int prtXCount = 0;
                int prtYCount = 0;
                //double  dcwidth = 64;//同断层 假设相邻点32m
                //准备画井点用的

                //System.Console.WriteLine("底图绘制:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
             
                //无尽华尔兹
                for (int i = 0; i < dtfgridNew.Rows.Count - 3; i = i + 4)
                {
                    //四边形调序 否则化成8字形 canvas y轴坐标是反的
                    double x0 = Convert.ToDouble(dtfgridNew.Rows[i + 0][0]);
                    double y0 = Convert.ToDouble(dtfgridNew.Rows[i + 0][1]);
                    double x1 = Convert.ToDouble(dtfgridNew.Rows[i + 1][0]);
                    double y1 = Convert.ToDouble(dtfgridNew.Rows[i + 1][1]);
                    double x3 = Convert.ToDouble(dtfgridNew.Rows[i + 3][0]);
                    double y3 = Convert.ToDouble(dtfgridNew.Rows[i + 3][1]);
                    double x2 = Convert.ToDouble(dtfgridNew.Rows[i + 2][0]);
                    double y2 = Convert.ToDouble(dtfgridNew.Rows[i + 2][1]);

                    //上prt
                    int hadC = (Convert.ToInt32(ch) - 1) * tablesize[0] * tablesize[1]; //跳过前几层
                    int hady = prtYCount * tablesize[0];//跳过前几行
                    double val = Convert.ToDouble(dtprt.Rows[prtYCount][prtXCount]);
                    double dzval = Convert.ToDouble(dzDt.Rows[prtYCount][prtXCount]);

                    //canvesGrid.Rows.Add(canvesGridRow);跳转至1005行
                    //全局储量
                    if (val > 0 && poro[hadC + hady + prtXCount] > 0 && b0 > 0 && dzval>0)
                    {
                        List<Point> ListPoint = new List<Point>();
                        ListPoint.Add(new Point(x0, y0));
                        ListPoint.Add(new Point(x1, y1));
                        ListPoint.Add(new Point(x3, y3));
                        ListPoint.Add(new Point(x2, y2));
                        var points = ListPoint;
                        points.Add(points[0]);
                        double s = Math.Abs(points.Take(points.Count - 1)
                          .Select((p, si) => (points[si + 1].X - p.X) * (points[si + 1].Y + p.Y))
                          .Sum() / 2) / 1000/1000;//km2
                        double onePointVol = 100 * s * dzval * poro[hadC + hady + prtXCount] * val * ro / b0;
                       // System.Console.WriteLine("s:" + s);
                     /*   if ( 5102843.5==y2)
                        {
                            System.Console.WriteLine("s:" + s);
                            System.Console.WriteLine("0:" + x0+","+y0);
                            System.Console.WriteLine("1:" + x1 + "," + y1);
                            System.Console.WriteLine("3:" + x3 + "," + y3);
                            System.Console.WriteLine("2:" + x2 + "," + y2);

                        }*/
                        //System.Console.WriteLine("dzval:" + dzval);
                       // System.Console.WriteLine("poro:" + poro[hadC + hady + prtXCount]);
                        //System.Console.WriteLine("soil:" + val);
                        //System.Console.WriteLine("onePointVol:" + onePointVol);
                        oneChVol = oneChVol + onePointVol;
                        allVol = allVol + onePointVol;
                       // System.Console.WriteLine("oneChVol:" + oneChVol);
                       // System.Console.WriteLine("allVol:" + allVol);
                    }

                    //行号结算
                    prtXCount++;
                    if (prtXCount == tablesize[0])//第一行数据已写完
                    {
                        prtXCount = 0;
                        prtYCount++;
                    }
                }
                //System.Console.WriteLine("allVol:" + allVol);
                // System.Console.WriteLine("oneChVol:" + oneChVol);
                //if (allVol > 0)
                //{
                //this.ch_res_all.Text = "本层储量" + allVol.ToString("0.0000") + "万吨";
                DataRow canvesGridRow = resByLayerDT.NewRow();
                canvesGridRow["ch"] = ch;
                canvesGridRow["val"] = oneChVol.ToString("0.0000");
                resByLayerDT.Rows.Add(canvesGridRow);  
            }
            System.Console.WriteLine("end:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
            DataRow canvesGridRow2 = resByLayerDT.NewRow();
            canvesGridRow2["ch"] = "合计";
            canvesGridRow2["val"] = allVol.ToString("0.0000");
            resByLayerDT.Rows.Add(canvesGridRow2);  
            this.dataGridReservers.ItemsSource = resByLayerDT.DefaultView;
        }
    }
}
