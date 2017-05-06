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
    /// <summary>
    /// DrawContainer.xaml 的交互逻辑
    /// </summary>
   
    public partial class CompareContainer : Window
    {
        string fgridpath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\F10-27RIGHT_E100.FGRID";
        string prtpath = "E:\\wf.txt";
        string schIncPath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\f10-27right_sch.inc";
        string gothIncPath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\f10-27right_goth.inc";
        string faciesPath = "E:\\1.txt";
        String drawTypeLeftStr, drawTypeRightStr,ch;
        DataTable canvesGrid= new DataTable();//坐标转换并记录对应网格坐标和层号
        DataTable faultDt = new DataTable();//断层
        DataTable dtfgridNew, dtprt, dzDt, wellCoord, wellStat;
        List<Point> faultPointsAll = new List<Point>();
        List<String> faultPointsAll_Direct = new List<String>();
        double valBottom = 0;//低级颜色113
        double inVal = 0;//渐变级别
        double valTop = 0;//颜色范围
        double m_d_zoomfactor2 = 0;
        int[] facies = null;
        int[] tablesize={0,0,0};
        double wellR = 0;//单井井距，单位m，算井控范围
        double maxX2 ,maxY2, minX2,minY2  ;
        double[] poro, permx;
        double minpermx, maxpermx,minporo,maxporo;
        Ellipse[] wellpoints, wellborders;
        TextBlock[] TextNames;
        //
        double downZoomx = 0;
        double downZoomy = 0;
        double lastMoveX = 0;
        double lastMoveY = 0;
        string zoomStat;
        
        //
        public CompareContainer(string title)
        {
            this.Content = title;
            InitializeComponent();
            readCombo();
            initCanvesGrid();
        }

        private void readCombo()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            double timed = ts.TotalSeconds;
            string ch = this.comboCH.Text;
            string comboTimeStr = this.comboTime.Text;
            fgrid_ch(fgridpath);
            //时间
            数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
            List<String> soiltimeList = fgridPrt.readSoilTime(prtpath);
            foreach (string soiltime in soiltimeList)
            {
                this.comboTime.Items.Add(soiltime);
            }
            this.comboTime.SelectedIndex = 0;   
            //
            this.drawtypeLeft.Items.Add("饱和度");
            this.drawtypeLeft.Items.Add("渗透率");
            this.drawtypeLeft.Items.Add("孔隙度");
            this.drawtypeLeft.Items.Add("相图");
            //this.drawtypeLeft.Items.Add("丰度");
            this.drawtypeLeft.SelectedIndex = 0;   
            //
            this.drawtypeRight.Items.Add("饱和度");
            this.drawtypeRight.Items.Add("渗透率");
            this.drawtypeRight.Items.Add("孔隙度");
            this.drawtypeRight.Items.Add("相图");
            //this.drawtypeRight.Items.Add("丰度");
            this.drawtypeRight.SelectedIndex = 0;  
            //
            System.Console.WriteLine("end:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
        }

        private void fgrid_ch(String filepath)
        {
            ObservableCollection<Fgrid_ch> imgs = new ObservableCollection<Fgrid_ch>();
            数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
            // 解析文件
            int[] tablesize = fgridPrt.readFGRID(filepath);
            for (int i = 1; i <= tablesize[2]; i++)
            {
                Fgrid_ch fgrid_ch = new Fgrid_ch();
                fgrid_ch.Ch = "" + i;
                imgs.Add(fgrid_ch);
            }
            //进行绑定
            comboCH.DisplayMemberPath = "Ch";//控件显示的列名
            comboCH.SelectedValuePath = "Ch";//控件值的列名
            comboCH.ItemsSource = imgs;
            this.comboCH.SelectedIndex = 0;
        }
        class Fgrid_ch   //声明类
        {
            string ch;
            public string Ch
            {
                get { return ch; }
                set { ch = value; }
            }
        }

        private void click_draw(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            double timed = ts.TotalSeconds;
            //
            ch = this.comboCH.Text;
            drawTypeLeftStr = this.drawtypeLeft.Text;
            drawTypeRightStr = this.drawtypeRight.Text;
            string combo_soiltimeStr = this.comboTime.Text;
            //initCanvesGrid();二次实例化报错
            double d = 0;

            canvesGrid.Clear();
            if (ch != null && !"".Equals(ch))//选中层号之后才开始
            {
                //Canvas leftCanvas = this.leftCanvas;
                leftCanvas.Children.Clear();
                rightCanvas.Children.Clear();
                ReservorDraw.pointList.Clear();
                数模建模.SIMB.FgridNew fgridNew = new 数模建模.SIMB.FgridNew();
                数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
                /*try
                {
                    ro = Convert.ToDouble(this.res_density.Text);
                    b0 = Convert.ToDouble(this.res_vol.Text);
                }
                catch
                {
                    System.Console.WriteLine("density格式错误");
                }
                 try
                {
                    String textdcRange = this.textdcRange.Text;
                    if (textdcRange != null && !"".Equals(textdcRange))
                    {
                        d = Convert.ToDouble(this.textdcRange.Text);
                    }
                    String textProja = this.textProja.Text;
                    if (textProja != null && !"".Equals(textProja))
                    {
                        a = Convert.ToDouble(this.textProja.Text);
                    }
                    String textInjb = this.textInjb.Text;
                    if (textInjb != null && !"".Equals(textInjb))
                    {
                        b = Convert.ToDouble(this.textInjb.Text);
                    }
                }
                catch
                {
                    System.Console.WriteLine("dcRange数字格式错误");
                }*/
                //调用主线程UI的的代码  
                //    new Thread(o =>
                //  { 
                System.Console.WriteLine("开始解析文件:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
                // 解析文件
                tablesize = fgridPrt.readFGRID(fgridpath);
                dtfgridNew = fgridNew.readFile(fgridpath, ch);
                dtprt = fgridPrt.readPRT(prtpath, ch, combo_soiltimeStr);
                faultDt = fgridPrt.readGothInc(gothIncPath, ch);//断层
                fgridPrt.readSchInc(schIncPath, ch);
                poro = fgridPrt.poro;//孔隙度
                permx = fgridPrt.permx;//渗透率
                dzDt = fgridPrt.dzDt;//厚度
                System.Console.WriteLine("开始求极值:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
                //求极值
                //渗透率
                ArrayList list = new ArrayList();
                foreach (double tempd in permx)
                {
                    if (tempd > 0)
                    {
                        list.Add(tempd);
                    }
                }
                list.Sort();
                minpermx = Convert.ToDouble(list[0]);
                maxpermx = Convert.ToDouble(list[list.Count - 1]);
                //孔隙度极值
                list.Clear();
                //list = new ArrayList(poro);
                foreach (double tempd in poro)
                {
                    if (tempd > 0)
                    {
                        // System.Console.WriteLine(list.Count+":"+tempd);
                        list.Add(tempd);
                    }
                }
                list.Sort();
                minporo = Convert.ToDouble(list[0]);
                maxporo = Convert.ToDouble(list[list.Count - 1]);

                // 计算缩放比例
                double dzoomx2, dzoomy2;
                maxX2 = Convert.ToDouble(dtfgridNew.Compute("max(x)", ""));
                maxY2 = Convert.ToDouble(dtfgridNew.Compute("max(y)", ""));
                minX2 = Convert.ToDouble(dtfgridNew.Compute("min(x)", ""));
                minY2 = Convert.ToDouble(dtfgridNew.Compute("min(y)", ""));
                //System.Console.WriteLine("-------" + maxX2);
                dzoomx2 = leftCanvas.ActualWidth / (maxX2 - minX2);
                dzoomy2 = leftCanvas.ActualHeight / (maxY2 - minY2);
                m_d_zoomfactor2 = dzoomx2 > dzoomy2 ? dzoomx2 : dzoomy2;
                wellR = 200;
                wellR = wellR * m_d_zoomfactor2;
                d = d * m_d_zoomfactor2;
                //准备画井点用的
                wellCoord = fgridPrt.wellCoord;
                wellStat = fgridPrt.wellStat;
                wellpoints = new Ellipse[wellCoord.Rows.Count];
                wellborders = new Ellipse[wellCoord.Rows.Count];
                TextNames = new TextBlock[wellCoord.Rows.Count];
                double[] pointX = new double[wellCoord.Rows.Count];
                double[] pointY = new double[wellCoord.Rows.Count];
                //
                drawCanvas(drawTypeLeftStr, fgridPrt, leftCanvas);
                drawCanvas(drawTypeRightStr,fgridPrt,rightCanvas);
                System.Console.WriteLine("end:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));             
            }
            else
            {
                fgrid_ch(fgridpath);
                数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
                List<String> soiltimeList=fgridPrt.readSoilTime(prtpath);
                foreach (string soiltime in soiltimeList)
                {
                    this.comboTime.Items.Add(soiltime);
                }
                this.comboTime.SelectedIndex = 0;   
            } 
        }
        /**
        * 饱和度显色
        * */
        private Color barsaColor(double val)
        {
            Color myColor = new Color();
            myColor.A = 255;
            if (val <= valBottom)
            {
                myColor.R = 0;
                myColor.G = 0;
                myColor.B = 255;
            }
            else if (val >= valTop)
            {
                myColor.R = 255;
                myColor.G = 0;
                myColor.B = 0;
            }
            else
            {
                double colorFlag = val - valBottom;
                if (colorFlag <= inVal)//蓝绿档
                {
                    myColor.R = 0;
                    myColor.G = (byte)(255 / inVal * colorFlag);
                    myColor.B = 255;
                }
                else if (colorFlag <= inVal * 2)
                {
                    myColor.R = 0;
                    myColor.G = 255;
                    myColor.B = (byte)(255 - 255 / inVal * (colorFlag - inVal));
                }
                else if (colorFlag <= inVal * 3)//绿红档
                {
                    myColor.R = (byte)(255 / inVal * (colorFlag - inVal * 2));
                    myColor.G = 255;
                    myColor.B = 0;
                }
                else if (colorFlag <= inVal * 4)
                {
                    myColor.R = 255;
                    myColor.G = (byte)(255 - 255 / inVal * (colorFlag - inVal * 3));
                    myColor.B = 0;
                }
            }
            return myColor;
        }
        private void drawCanvas(String drawTypeStr, 数模建模.SIMB.FgridPrt fgridPrt, Canvas canvesptr)
        {
            int prtXCount = 0;
            int prtYCount = 0;
            int onefacies = 0;
            int jhCount = 0;
            double a = 0;
            double b = 0;
            double allVol = 0;
            double ro = 0;
            double b0 = 0;
            facies = null;
            if ("相图".Equals(drawTypeStr))
            {
                facies = fgridPrt.readFacies(faciesPath);
            }
            //   this.Dispatcher.Invoke(DispatcherPriority.Normal,
            //new Action(() =>
            //{
            //调用主线程UI的的代码   
            //无尽华尔兹
            for (int i = 0; i < dtfgridNew.Rows.Count - 3; i = i + 4)
            {
                //if (i >100) break;
                Polygon myPolygon2 = new Polygon();
                PointCollection myPointCollection2 = new PointCollection();
                myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                myPolygon2.StrokeThickness = 0.00;//00000000000000

                double rawx0 = Convert.ToDouble(dtfgridNew.Rows[i + 0][0]);
                double rawy0 = Convert.ToDouble(dtfgridNew.Rows[i + 0][1]);
                double rawx1 = Convert.ToDouble(dtfgridNew.Rows[i + 1][0]);
                double rawy1 = Convert.ToDouble(dtfgridNew.Rows[i + 1][1]);
                double rawx3 = Convert.ToDouble(dtfgridNew.Rows[i + 3][0]);
                double rawy3 = Convert.ToDouble(dtfgridNew.Rows[i + 3][1]);
                double rawx2 = Convert.ToDouble(dtfgridNew.Rows[i + 2][0]);
                double rawy2 = Convert.ToDouble(dtfgridNew.Rows[i + 2][1]);
                //四边形调序 否则化成8字形 canvas y轴坐标是反的
                double x0 = Math.Round((rawx0 - minX2) * m_d_zoomfactor2, 1);//- 728
                double y0 = Math.Round((maxY2 - rawy0) * m_d_zoomfactor2, 1);
                //System.Console.WriteLine("="+x0+"."+y0);
                myPointCollection2.Add(new System.Windows.Point(x0, y0));
                double x1 = Math.Round((rawx1 - minX2) * m_d_zoomfactor2, 1);
                double y1 = Math.Round((maxY2 - rawy1) * m_d_zoomfactor2, 1);
                myPointCollection2.Add(new System.Windows.Point(x1, y1));
                double x3 = Math.Round((rawx3 - minX2) * m_d_zoomfactor2, 1);
                double y3 = Math.Round((maxY2 - rawy3) * m_d_zoomfactor2, 1);
                myPointCollection2.Add(new System.Windows.Point(x3, y3));
                double x2 = Math.Round((rawx2 - minX2) * m_d_zoomfactor2, 1);
                double y2 = Math.Round((maxY2 - rawy2) * m_d_zoomfactor2, 1);
                myPointCollection2.Add(new System.Windows.Point(x2, y2));

                //上prt
                int hadC = (Convert.ToInt32(ch) - 1) * tablesize[0] * tablesize[1]; //跳过前几层
                int hady = prtYCount * tablesize[0];//跳过前几行
                double val = Convert.ToDouble(dtprt.Rows[prtYCount][prtXCount]);
                double dzval = Convert.ToDouble(dzDt.Rows[prtYCount][prtXCount]);

                //canvesGrid.Rows.Add(canvesGridRow);跳转至1005行
                //全局储量
                if (val > 0 && poro[hadC + hady + prtXCount] > 0 && b0 > 0)
                {
                    List<Point> points = new List<Point>();
                    points.Add(new Point(rawx0, rawy0));
                    points.Add(new Point(rawx1, rawy1));
                    points.Add(new Point(rawx3, rawy3));
                    points.Add(new Point(rawx2, rawy2));
                    points.Add(new Point(rawx0, rawy0));//注意
                   /* double s = Math.Abs(points.Take(points.Count - 1)
                      .Select((p, si) => (points[si + 1].X - p.X) * (points[si + 1].Y + p.Y))
                      .Sum() / 2 / 1000000);//km2
                    allVol = allVol + 100 * s * dzval * poro[hadC + hady + prtXCount] * val * ro / b0;*/
                }
                if ("相图".Equals(drawTypeStr))
                {
                    onefacies = facies[hadC + hady + prtXCount];
                    if (onefacies > 0 && onefacies < 11)
                    {
                        valBottom = 1;//低级颜色113
                        valTop = 10;//顶级颜色
                        inVal = (valTop - valBottom) / 4;//渐变级别//1 1.8   
                        Color myColor = barsaColor(facies[hadC + hady + prtXCount]);
                        SolidColorBrush myBrush = new SolidColorBrush(myColor);
                        myPolygon2.Fill = myBrush;
                    }
                }
                else if (val > 0 || permx[hadC + hady + prtXCount] > 0 || poro[hadC + hady + prtXCount] > 0)
                {
                    //调色
                    //int val0 = Convert.ToInt32(100 * val);
                    switch (drawTypeStr)
                    {
                        case "饱和度":
                            if (val > 0)
                            {
                                valBottom = fgridPrt.minv;//低级颜色113
                                valTop = fgridPrt.maxv;//顶级颜色
                                inVal = (valTop - valBottom) / 4;//渐变级别//1 1.8   
                                Color myColor = barsaColor(val);
                                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                                myPolygon2.Fill = myBrush;
                            }
                            break;
                        case "渗透率":
                            if (permx[hadC + hady + prtXCount] > 0)
                            {
                                valBottom = minpermx;//低级颜色113
                                valTop = maxpermx;//顶级颜色
                                inVal = (valTop - valBottom) / 4;//渐变级别//1 1.8   
                                Color myColor = barsaColor(permx[hadC + hady + prtXCount]);
                                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                                myPolygon2.Fill = myBrush;
                            }
                            break;
                        case "孔隙度":
                            if (poro[hadC + hady + prtXCount] > 0)
                            {
                                valBottom = minporo;//低级颜色113
                                valTop = maxporo;//顶级颜色
                                inVal = (valTop - valBottom) / 4;//渐变级别//1 1.8     
                                Color myColor = barsaColor(poro[hadC + hady + prtXCount]);
                                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                                myPolygon2.Fill = myBrush;
                            }
                            break;
                        case "丰度":
                            if (poro[hadC + hady + prtXCount] > 0 && b0 > 0)
                            {
                                double fd = 100 * dzval * poro[hadC + hady + prtXCount] * val * ro / b0;
                                valBottom = 0;//  50/ 10000;//低级颜色113
                                valTop = 100;// 300 / 10000;//顶级颜色
                                inVal = (valTop - valBottom) / 4;//渐变级别//1 1.8                                       
                                Color myColor = barsaColor(fd);
                                SolidColorBrush myBrush = new SolidColorBrush(myColor);
                                myPolygon2.Fill = myBrush;
                            }
                            break;
                        default:
                            break;
                    }
                }
                //2016-8-11 断层绘制
                foreach (DataRow faultRow in faultDt.Rows)
                {
                    if ((prtXCount + 1 + "").Equals(faultRow["i"].ToString())
                     && (prtYCount + 1 + "").Equals(faultRow["j"].ToString()))
                    {
                        SolidColorBrush myBrush = Brushes.Gray;
                        myPolygon2.Fill = myBrush;
                    }
                }
                myPolygon2.Points = myPointCollection2;
                canvesptr.Children.Add(myPolygon2);//带颜色的小四边形

                //准备点击事件孔渗饱等
                DataRow canvesGridRow = canvesGrid.NewRow();
                canvesGridRow["ch"] = ch;
                canvesGridRow["dz"] = dzval;
                canvesGridRow["barsa"] = val;
                canvesGridRow["x0"] = x0;
                canvesGridRow["y0"] = y0;
                canvesGridRow["x1"] = x1;
                canvesGridRow["y1"] = y1;
                canvesGridRow["x2"] = x2;
                canvesGridRow["y2"] = y2;
                canvesGridRow["x3"] = x3;
                canvesGridRow["y3"] = y3;
                canvesGridRow["xcount"] = prtXCount;
                canvesGridRow["ycount"] = prtYCount;
                canvesGridRow["permx"] = permx[hadC + hady + prtXCount];
                canvesGridRow["poro"] = poro[hadC + hady + prtXCount];
                canvesGridRow["facies"] = onefacies;
                canvesGridRow["rawx0"] = rawx0;
                canvesGridRow["rawy0"] = rawy0;
                canvesGridRow["rawx1"] = rawx1;
                canvesGridRow["rawy1"] = rawy1;
                canvesGridRow["rawx2"] = rawx2;
                canvesGridRow["rawy2"] = rawy2;
                canvesGridRow["rawx3"] = rawx3;
                canvesGridRow["rawy3"] = rawy3;
                //画井点
                int ellipseWidth = 3;
                foreach (DataRow row in wellCoord.Rows)
                {
                    if (ch.Equals(row["z"].ToString()) && prtXCount.Equals(row["x"]) && prtYCount.Equals(row["y"]))
                    {
                        Ellipse wellpoint = new Ellipse();
                        Ellipse wellborder = new Ellipse();
                        string jh = row["jh"].ToString();
                        wellpoint.Width = ellipseWidth;
                        wellpoint.Height = ellipseWidth;
                        wellborder.Stroke = System.Windows.Media.Brushes.Black;
                        wellborder.StrokeThickness = 0.2;
                        wellpoint.Stroke = System.Windows.Media.Brushes.Black;
                        wellpoint.StrokeThickness = 0.1;
                        foreach (DataRow rowStat in wellStat.Rows)
                        {
                            if (jh.Equals(rowStat["jh"]))
                            {
                                string stat = rowStat["stat"].ToString();
                                switch (stat)
                                {
                                    case "OIL":
                                        wellpoint.Fill = System.Windows.Media.Brushes.Red;
                                        wellborder.Width = a;
                                        wellborder.Height = a;
                                        break;
                                    default:
                                        wellpoint.Fill = System.Windows.Media.Brushes.Green;
                                        wellborder.Width = b;
                                        wellborder.Height = b;
                                        break;
                                }
                                canvesGridRow["stat"] = stat;
                                canvesGridRow["jh"] = jh;
                            }
                        }
                        Canvas.SetLeft(wellpoint, x0);//- wellpoint.Width / 2后是在x0点画
                        Canvas.SetTop(wellpoint, y0);
                        //canvesptr.Children.Add(wellpoint);
                        wellpoints[jhCount] = wellpoint;
                        Canvas.SetLeft(wellborder, x0 - wellborder.Width / 2 + wellpoint.Width / 2);// + wellpoint.Width / 2后是在多边形中心画
                        Canvas.SetTop(wellborder, y0 - wellborder.Height / 2 + wellpoint.Height / 2);
                        // canvesptr.Children.Add(wellborder);
                        wellborders[jhCount] = wellborder;
                        TextBlock t1 = new TextBlock();
                        t1.Text = jh;
                        t1.FontSize = 6;//井号字体
                        Canvas.SetLeft(t1, x0);
                        Canvas.SetTop(t1, y0);
                        TextNames[jhCount] = t1;
                        //canvesptr.Children.Add(t1);
                        jhCount++;
                    }
                }
                canvesGrid.Rows.Add(canvesGridRow);
                //行号结算
                prtXCount++;
                if (prtXCount == tablesize[0])//第一行数据已写完
                {
                    prtXCount = 0;
                    prtYCount++;
                }
            }//大循环结束

            //System.Console.WriteLine("allVol:" + allVol);
            /*if (allVol > 0)
            {
                this.ch_res_all.Text = "本层储量" + allVol.ToString("0.0000") + "万吨";
            }*/
            for (int i = 0; i < wellpoints.Length; i++)
            {
                if (wellpoints[i] != null)
                {
                    canvesptr.Children.Add(wellpoints[i]);
                    //canvesptr.Children.Add(wellborders[i]);
                    canvesptr.Children.Add(TextNames[i]);
                }
            }


            //调用主线程UI的的代码
            //}));  
            //   }) { IsBackground = true }.Start();
            //最后我们来个图例
            int legendMaxCount = 12;
            for (int legendCount = 0; legendCount <= legendMaxCount; legendCount++)
            {
                Rectangle rect = new Rectangle();
                rect.Width = 10;
                rect.Height = rect.Width;
                double nowval = valBottom + legendCount * (valTop - valBottom) / legendMaxCount;
                Color legendColor = barsaColor(nowval);
                SolidColorBrush myBrush = new SolidColorBrush(legendColor);
                rect.Fill = myBrush;
                Canvas.SetLeft(rect, legendCount * rect.Width);
                Canvas.SetTop(rect, 0);
                canvesptr.Children.Add(rect);
                if (0 == legendCount
                    || legendMaxCount == legendCount
                    || 0 == legendCount % 4)
                {
                    TextBlock t1 = new TextBlock();
                    t1.Text = nowval.ToString("0.00");
                    t1.FontSize = 10;//井号字体
                    Canvas.SetLeft(t1, rect.Width * legendCount);
                    Canvas.SetTop(t1, 10);
                    canvesptr.Children.Add(t1);
                }
            }
        }
        private void initCanvesGrid()
        {
            DataColumn columnCh = new DataColumn();
            columnCh.DataType = System.Type.GetType("System.Int32");
            columnCh.ColumnName = "ch";
            canvesGrid.Columns.Add(columnCh);

            DataColumn columnPoro = new DataColumn();
            columnPoro.DataType = System.Type.GetType("System.Double");
            columnPoro.ColumnName = "poro";//孔隙度
            canvesGrid.Columns.Add(columnPoro);

            DataColumn columnPermx = new DataColumn();
            columnPermx.DataType = System.Type.GetType("System.Double");
            columnPermx.ColumnName = "permx";//渗透率
            canvesGrid.Columns.Add(columnPermx);

            DataColumn columnBarsa = new DataColumn();
            columnBarsa.DataType = System.Type.GetType("System.Double");
            columnBarsa.ColumnName = "barsa";//soil饱和度
            canvesGrid.Columns.Add(columnBarsa);

            DataColumn columnX0 = new DataColumn();
            columnX0.DataType = System.Type.GetType("System.Double");
            columnX0.ColumnName = "x0";
            canvesGrid.Columns.Add(columnX0);
            DataColumn columnY0 = new DataColumn();
            columnY0.DataType = System.Type.GetType("System.Double");
            columnY0.ColumnName = "y0";
            canvesGrid.Columns.Add(columnY0);

            DataColumn columnX1 = new DataColumn();
            columnX1.DataType = System.Type.GetType("System.Double");
            columnX1.ColumnName = "x1";
            canvesGrid.Columns.Add(columnX1);
            DataColumn columnY1 = new DataColumn();
            columnY1.DataType = System.Type.GetType("System.Double");
            columnY1.ColumnName = "y1";
            canvesGrid.Columns.Add(columnY1);

            DataColumn columnX2 = new DataColumn();
            columnX2.DataType = System.Type.GetType("System.Double");
            columnX2.ColumnName = "x2";
            canvesGrid.Columns.Add(columnX2);
            DataColumn columnY2 = new DataColumn();
            columnY2.DataType = System.Type.GetType("System.Double");
            columnY2.ColumnName = "y2";
            canvesGrid.Columns.Add(columnY2);

            DataColumn columnX3 = new DataColumn();
            columnX3.DataType = System.Type.GetType("System.Double");
            columnX3.ColumnName = "x3";
            canvesGrid.Columns.Add(columnX3);
            DataColumn columnY3 = new DataColumn();
            columnY3.DataType = System.Type.GetType("System.Double");
            columnY3.ColumnName = "y3";
            canvesGrid.Columns.Add(columnY3);

            DataColumn columnXcount = new DataColumn();
            columnXcount.DataType = System.Type.GetType("System.Int32");
            columnXcount.ColumnName = "xcount";
            canvesGrid.Columns.Add(columnXcount);
            DataColumn columnYcount = new DataColumn();
            columnYcount.DataType = System.Type.GetType("System.Int32");
            columnYcount.ColumnName = "ycount";
            canvesGrid.Columns.Add(columnYcount);

            DataColumn columnDz = new DataColumn();
            columnDz.DataType = System.Type.GetType("System.Double");
            columnDz.ColumnName = "dz";//厚度
            canvesGrid.Columns.Add(columnDz);

            DataColumn columnStat = new DataColumn();
            columnStat.DataType = System.Type.GetType("System.String");
            columnStat.ColumnName = "stat";
            canvesGrid.Columns.Add(columnStat);//井别

            DataColumn columnjh = new DataColumn();
            columnjh.DataType = System.Type.GetType("System.String");
            columnjh.ColumnName = "jh";
            canvesGrid.Columns.Add(columnjh);//井别

            /*  DataColumn columnCp = new DataColumn();
              columnCp.DataType = System.Type.GetType("System.String");
              columnCp.ColumnName = "checkpoint";
              canvesGrid.Columns.Add(columnCp);//断层去重*/

            DataColumn columnFacies = new DataColumn();
            columnFacies.DataType = System.Type.GetType("System.Int32");
            columnFacies.ColumnName = "facies";
            canvesGrid.Columns.Add(columnFacies);//相图

            DataColumn columnrawX0 = new DataColumn();
            columnrawX0.DataType = System.Type.GetType("System.Double");
            columnrawX0.ColumnName = "rawx0";
            canvesGrid.Columns.Add(columnrawX0);

            DataColumn columnrawY0 = new DataColumn();
            columnrawY0.DataType = System.Type.GetType("System.Double");
            columnrawY0.ColumnName = "rawy0";
            canvesGrid.Columns.Add(columnrawY0);

            DataColumn columnrawX1 = new DataColumn();
            columnrawX1.DataType = System.Type.GetType("System.Double");
            columnrawX1.ColumnName = "rawx1";
            canvesGrid.Columns.Add(columnrawX1);
            DataColumn columnrawY1 = new DataColumn();
            columnrawY1.DataType = System.Type.GetType("System.Double");
            columnrawY1.ColumnName = "rawy1";
            canvesGrid.Columns.Add(columnrawY1);

            DataColumn columnrawX2 = new DataColumn();
            columnrawX2.DataType = System.Type.GetType("System.Double");
            columnrawX2.ColumnName = "rawx2";
            canvesGrid.Columns.Add(columnrawX2);
            DataColumn columnrawY2 = new DataColumn();
            columnrawY2.DataType = System.Type.GetType("System.Double");
            columnrawY2.ColumnName = "rawy2";
            canvesGrid.Columns.Add(columnrawY2);

            DataColumn columnrawX3 = new DataColumn();
            columnrawX3.DataType = System.Type.GetType("System.Double");
            columnrawX3.ColumnName = "rawx3";
            canvesGrid.Columns.Add(columnrawX3);
            DataColumn columnrawY3 = new DataColumn();
            columnrawY3.DataType = System.Type.GetType("System.Double");
            columnrawY3.ColumnName = "rawy3";
            canvesGrid.Columns.Add(columnrawY3);           
        }
        private void canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Canvas canv = (Canvas)sender;
            ReservorDraw.zoom_in(canv, e);
        }

        private void canvas_down(object sender, MouseButtonEventArgs e)
        {
            Canvas canv = (Canvas)sender;
            ReservorDraw.canvas_zoom_down(canv, e);
        }
        private void canvas_up(object sender, MouseButtonEventArgs e)
        {
            Canvas canv = (Canvas)sender;
            ReservorDraw.canvas_zoom_up(canv, e);
        }
        private void canvas_move(object sender, MouseEventArgs e)
        {
            Canvas canv = (Canvas)sender;
            ReservorDraw.canvas_move(canv, e);
        }
    }
}