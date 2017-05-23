using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using 建模数模.tools;
using System.Data;
using 数模建模.tools;
using System.Windows.Ink;
using 数模建模.Drawer;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows.Threading;
using System.Threading;
using System.IO;
using System.Windows.Markup;
using System.Xml;
namespace 数模建模
{
    /// <summary>
    /// MainRightTabPage.xaml 的交互逻辑
    /// </summary>
    public partial class MainRightTabPage : UserControl
    {
        List<Point> perfectPoints = new List<Point>();
        DataTable wellCoord = new DataTable();//井
        DataTable wellCoordTrueEnd = new DataTable();//井
        DataTable welspecs = new DataTable();//井口网格坐标
        DataTable canvesGrid = new DataTable();//坐标转换并记录对应网格坐标和层号

        Point wellPoint4allres = new Point();//4单井各层储
        double wellRes4allres = 0;//4单井各层储
        DataTable faultDt = new DataTable();//断层
        List<Point> faultPointsAll = new List<Point>();
        List<String> faultPointsAll_Direct = new List<String>();
        double valBottom = 0;//低级颜色113
        double inVal = 0;//渐变级别
        double valTop = 0;//颜色范围
        double m_d_zoomfactor2 = 0;
        string drawTypeStr = "";
        //  combo_soiltimeStr = "";
        int[] facies = null;
        double[] ntgs = null;//净毛比
        int[] tablesize = { 0, 0, 0 };
        double[] permx;//渗透率 多层
        String allCh = null;//计算全区储量
        double allVol = 0;
        int[] faciesNum = null;
        double[] faciesNumRes = new double[10];// new double[2];
        string tmpVol4EachRes;
        List<String> soiltimeList = new List<string>();//时间簿
        Boolean noDzAlert = true;
        double[] dzs = null;


        public MainRightTabPage()
        {
            InitializeComponent();

            reservers_init();
            initCanvesGrid();
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
            //净毛比
            DataColumn columnNTG = new DataColumn();
            columnNTG.DataType = System.Type.GetType("System.Double");
            columnNTG.ColumnName = "NTG";
            canvesGrid.Columns.Add(columnNTG);

            //可修改ksbEditDT
            DataColumn columnEXcount = new DataColumn();
            columnEXcount.DataType = System.Type.GetType("System.Int32");
            columnEXcount.ColumnName = "xcount";
            ksbEditDT.Columns.Add(columnEXcount);
            DataColumn columnEYcount = new DataColumn();
            columnEYcount.DataType = System.Type.GetType("System.Int32");
            columnEYcount.ColumnName = "ycount";
            ksbEditDT.Columns.Add(columnEYcount);
            DataColumn columnKSB = new DataColumn();
            columnKSB.DataType = System.Type.GetType("System.String");
            columnKSB.ColumnName = "ksb";
            ksbEditDT.Columns.Add(columnKSB);
            DataColumn columnKVal = new DataColumn();
            columnKVal.DataType = System.Type.GetType("System.Double");
            columnKVal.ColumnName = "val";
            ksbEditDT.Columns.Add(columnKVal);

            this.drawtype.Items.Add("相图");
            this.drawtype.Items.Add("丰度");
            this.drawtype.Items.Add("孔隙度");
            this.drawtype.Items.Add("渗透率");
            this.drawtype.Items.Add("饱和度");
            this.drawtype.SelectedIndex = 0;
        }



        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "txt files(*.txt)|*.txt|xls files(*.xls)|*.xls|All files(*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                DataGridToExcel.OutDataToExcel2(Data_Result.result_temp_zhongzhuan, saveFileDialog.FileName);
            }
        }

        double lastMoveX = 0;
        double lastMoveY = 0;
        private void canvas_move(object sender, MouseEventArgs e)
        {
            //2016-11-6 10:08:52
            switch (switchClickMode)
            {
                case "染色":
                    //ReservorDraw.DrawLine(canvesprt, e, scb);
                    if (e.LeftButton == MouseButtonState.Pressed)
                    {
                        Canvas leftCanvas = (Canvas)sender;
                        Point pClick = new Point(e.GetPosition(leftCanvas).X, e.GetPosition(leftCanvas).Y);
                        double addX = Math.Abs(e.GetPosition(leftCanvas).X - lastMoveX);
                        double addY = Math.Abs(e.GetPosition(leftCanvas).Y - lastMoveY);
                        //画井点用的
                        List<Ellipse> wellPoints = new List<Ellipse>();
                        List<TextBlock> wellNames = new List<TextBlock>();
                        // 每移动15m 开始着色
                        if (addX > 15 * m_d_zoomfactor2 || addY > 15 * m_d_zoomfactor2)
                        {
                            foreach (DataRow row3 in canvesGrid.Rows)
                            {
                                //画井点
                                drawWells(row3, wellPoints, wellNames);
                                double soil = (double)row3["barsa"];
                                Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                Point point1 = new Point((double)row3["x1"], (double)row3["y1"]);
                                Point point2 = new Point((double)row3["x2"], (double)row3["y2"]);
                                Point point3 = new Point((double)row3["x3"], (double)row3["y3"]);
                                List<Point> pointL = new List<Point>();
                                pointL.Add(point0);
                                pointL.Add(point1);
                                pointL.Add(point3);
                                pointL.Add(point2);
                                if (ReservorDraw.isInRegion(pClick, pointL) && soil > 0)
                                {
                                    Polygon myPolygon2 = new Polygon();
                                    PointCollection myPointCollection2 = new PointCollection();
                                    myPointCollection2.Add(point0);
                                    myPointCollection2.Add(point1);
                                    myPointCollection2.Add(point3);
                                    myPointCollection2.Add(point2);
                                    //myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                    myPolygon2.StrokeThickness = 0.00;
                                    SolidColorBrush myBrush = scb;
                                    myPolygon2.Fill = myBrush;
                                    myPolygon2.Points = myPointCollection2;
                                    this.canvesprt.Children.Add(myPolygon2);
                                    break;
                                }

                            }
                            lastMoveX = e.GetPosition(leftCanvas).X;
                            lastMoveY = e.GetPosition(leftCanvas).Y;
                            //画井点
                            for (int i = 0; i < wellPoints.Count; i++)
                            {
                                if (wellPoints[i] != null)
                                {
                                    this.canvesprt.Children.Add(wellPoints[i]);
                                    this.canvesprt.Children.Add(wellNames[i]);
                                }
                            }
                        }

                    }
                    break;
                case "移动":
                    Canvas canv = (Canvas)sender;
                    ReservorDraw.canvas_move(canv, e);
                    break;
                default:
                    break;
            }
        }
        private void canvas_down(object sender, MouseButtonEventArgs e)
        {
            Canvas canv = (Canvas)sender;
            ReservorDraw.canvas_zoom_down(canv, e);
        }
        //Boolean switchksbFlag = false;//环线还是查看孔渗饱
        String switchClickMode = "移动";
        DataTable ksbEditDT = new DataTable();
        double wellR = 0;//单井井距，单位m，算井控范围
        private void canves1_MouseLeftButtonUp_prt(object sender, MouseButtonEventArgs e)
        {
            //Line line = new Line();
            //line.Stroke = System.Windows.Media.Brushes.Red;
            //line.X1 = 1;
            //line.X2 = e.GetPosition(this.canves1).X;
            //line.Y1 = 1;
            //line.Y2 = e.GetPosition(this.canves1).Y;
            //line.HorizontalAlignment = HorizontalAlignment.Left;
            //line.VerticalAlignment = VerticalAlignment.Center;
            //canves1.Children.Add(line);
            //Point point = new Point();
            //point.X = e.GetPosition(this.canves1).X - ReservorDraw.point.X;
            //point.Y = e.GetPosition(this.canves1).Y - ReservorDraw.point.X;
            //ReservorDraw.move(canves1,point);
            //ReservorDraw.DrawPoint(canves1);
            canvesprt.Focus();
            switch (switchClickMode)
            {
                case "圈选":
                    ReservorDraw.DrawLine(canvesprt, e, scb);
                    break;
                case "孔渗饱":
                    Point pClick = new Point(e.GetPosition(this.canvesprt).X, e.GetPosition(this.canvesprt).Y);
                    foreach (DataRow canvesRow in canvesGrid.Rows)
                    {
                        List<Point> pointL = new List<Point>();
                        pointL.Add(new Point((double)canvesRow["x0"], (double)canvesRow["y0"]));
                        pointL.Add(new Point((double)canvesRow["x1"], (double)canvesRow["y1"]));
                        pointL.Add(new Point((double)canvesRow["x3"], (double)canvesRow["y3"]));
                        pointL.Add(new Point((double)canvesRow["x2"], (double)canvesRow["y2"]));

                        if (ReservorDraw.isInRegion(pClick, pointL))
                        {
                            ksbEditDT.Clear();
                            DataRow editDTRowbarsa = ksbEditDT.NewRow();//准备点击事件孔渗饱
                            editDTRowbarsa["xcount"] = canvesRow["xcount"];
                            editDTRowbarsa["ycount"] = canvesRow["ycount"];
                            editDTRowbarsa["ksb"] = "饱和度";
                            editDTRowbarsa["val"] = canvesRow["barsa"];
                            ksbEditDT.Rows.Add(editDTRowbarsa);

                            DataRow editDTRowpermx = ksbEditDT.NewRow();//准备点击事件孔渗饱
                            editDTRowpermx["xcount"] = canvesRow["xcount"];
                            editDTRowpermx["ycount"] = canvesRow["ycount"];
                            editDTRowpermx["ksb"] = "渗透率";
                            editDTRowpermx["val"] = canvesRow["permx"];
                            ksbEditDT.Rows.Add(editDTRowpermx);

                            DataRow editDTRowporo = ksbEditDT.NewRow();//准备点击事件孔渗饱
                            editDTRowporo["xcount"] = canvesRow["xcount"];
                            editDTRowporo["ycount"] = canvesRow["ycount"];
                            editDTRowporo["ksb"] = "孔隙度";
                            editDTRowporo["val"] = canvesRow["poro"];
                            ksbEditDT.Rows.Add(editDTRowporo);
                            this.dataGridKSB.ItemsSource = ksbEditDT.DefaultView;
                            break;
                        }
                    }
                    break;
                case "单井井控范围":
                    List<Point> wellCtrlPoint = new List<Point>();
                    if (wellPoint4allres != null)//单井各层储量
                    {
                        pClick = wellPoint4allres;
                    }
                    else
                    {
                        pClick = new Point(e.GetPosition(this.canvesprt).X, e.GetPosition(this.canvesprt).Y);
                    }
                    Console.WriteLine("单井井控范围" + pClick.X + ":" + pClick.Y);
                    //bool hasy1=false,hasy2=false,hasx1=false,hasx2=false;
                    bool hasA = false, hasB = false, hasC = false, hasD = false;
                    bool hasX = false, hasY = false, hasNX = false, hasNY = false;
                    double drawedWellR = wellR;
                    bool cuthasA = false, cuthasB = false, cuthasC = false, cuthasD = false;
                    bool cuthasX = false, cuthasY = false, cuthasNX = false, cuthasNY = false;
                    bool hasProd = false;
                    //取出范围内点
                    foreach (DataRow canvesRow in canvesGrid.Rows)
                    {
                        if (canvesRow["stat"].ToString().Length > 0)
                        {
                            string stat = canvesRow["stat"].ToString();
                            Point point0 = new Point();

                            point0 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);

                            double wellLength = pointl(pClick, point0);
                            if ("OIL".Equals(stat) && wellLength < 100 * m_d_zoomfactor2)//百米内油井视为同井
                            {
                                hasProd = true;
                                Console.WriteLine("本油井找到");
                                pClick = point0;
                            }
                            if ("WATER".Equals(stat) && wellLength <= wellR * 2)//井距是wellr200 井范围增大4倍数
                            {
                                //System.Console.WriteLine(stat);
                                wellCtrlPoint.Add(point0);
                                Console.WriteLine("邻水井找到");
                            }
                            else if ("OIL".Equals(stat) && wellLength <= wellR * 2 * 1.5
                                && wellLength >= 100 * m_d_zoomfactor2)//百米内油井视为同井
                            {
                                //Console.WriteLine("邻油井找到");
                                //System.Console.WriteLine(stat);
                                point0 = new Point((point0.X + pClick.X) / 2,
                                    (point0.Y + pClick.Y) / 2);
                                wellCtrlPoint.Add(point0);
                                System.Console.WriteLine("l:" + wellLength);
                                System.Console.WriteLine("r:" + wellR);
                                // if (wellLength <= wellR * 2)
                                //{
                                drawedWellR = wellLength / 2;
                                if (point0.Y > pClick.Y + 32 * m_d_zoomfactor2
                                       && Math.Abs(point0.Y - pClick.Y) >= Math.Abs(point0.X - pClick.X)
                                       && !hasY
                                       )
                                {
                                    hasY = true;
                                    cuthasY = true;
                                }
                                else if (point0.Y + 32 * m_d_zoomfactor2 < pClick.Y
                                     && Math.Abs(point0.Y - pClick.Y) >= Math.Abs(point0.X - pClick.X)
                                    && !hasNY
                                    )
                                {
                                    hasNY = true; cuthasNY = true;
                                }
                                if (point0.X + 32 * m_d_zoomfactor2 < pClick.X
                                     && Math.Abs(point0.Y - pClick.Y) <= Math.Abs(point0.X - pClick.X)
                                     && !hasNX)
                                {
                                    hasNX = true; cuthasNX = true;
                                }
                                else if (point0.X > pClick.X + 32 * m_d_zoomfactor2
                                      && Math.Abs(point0.Y - pClick.Y) <= Math.Abs(point0.X - pClick.X)
                                     && !hasX)
                                {
                                    hasX = true; cuthasX = true;
                                }
                                if (point0.X + 32 * m_d_zoomfactor2 < pClick.X && point0.Y > pClick.Y + 32 * m_d_zoomfactor2
                                     && !hasA)
                                {
                                    hasA = true; cuthasA = true;
                                }
                                else if (point0.X > pClick.X + 32 * m_d_zoomfactor2 && point0.Y > pClick.Y + 32 * m_d_zoomfactor2
                                     && !hasB)
                                {
                                    hasB = true; cuthasB = true;
                                }
                                else if (point0.X + 32 * m_d_zoomfactor2 < pClick.X && point0.Y + 32 * m_d_zoomfactor2 < pClick.Y
                                     && !hasC)
                                {
                                    hasC = true; cuthasC = true;
                                }
                                else if (point0.X > pClick.X + 32 * m_d_zoomfactor2 && point0.Y + 32 * m_d_zoomfactor2 < pClick.Y
                                     && !hasD)
                                {
                                    hasD = true; cuthasD = true;
                                }
                                // }
                            }

                        }
                    }
                    //判断边界点补全
                    foreach (Point tmpPoint in wellCtrlPoint)
                    {
                        if (tmpPoint.Y > pClick.Y + 32 * m_d_zoomfactor2
                            && Math.Abs(tmpPoint.Y - pClick.Y) >= Math.Abs(tmpPoint.X - pClick.X)
                            && !hasY
                            )
                        {
                            hasY = true;
                        }
                        else if (tmpPoint.Y + 32 * m_d_zoomfactor2 < pClick.Y
                             && Math.Abs(tmpPoint.Y - pClick.Y) >= Math.Abs(tmpPoint.X - pClick.X)
                            && !hasNY
                            )
                        {
                            hasNY = true;
                        }
                        if (tmpPoint.X + 32 * m_d_zoomfactor2 < pClick.X
                             && Math.Abs(tmpPoint.Y - pClick.Y) <= Math.Abs(tmpPoint.X - pClick.X)
                             && !hasNX)
                        {
                            hasNX = true;
                        }
                        else if (tmpPoint.X > pClick.X + 32 * m_d_zoomfactor2
                              && Math.Abs(tmpPoint.Y - pClick.Y) <= Math.Abs(tmpPoint.X - pClick.X)
                             && !hasX)
                        {
                            hasX = true;
                        }
                        if (tmpPoint.X + 32 * m_d_zoomfactor2 < pClick.X && tmpPoint.Y > pClick.Y + 32 * m_d_zoomfactor2
                             && !hasA)
                        {
                            hasA = true;
                        }
                        else if (tmpPoint.X > pClick.X + 32 * m_d_zoomfactor2 && tmpPoint.Y > pClick.Y + 32 * m_d_zoomfactor2
                             && !hasB)
                        {
                            hasB = true;
                        }
                        else if (tmpPoint.X + 32 * m_d_zoomfactor2 < pClick.X && tmpPoint.Y + 32 * m_d_zoomfactor2 < pClick.Y
                             && !hasC)
                        {
                            hasC = true;
                        }
                        else if (tmpPoint.X > pClick.X + 32 * m_d_zoomfactor2 && tmpPoint.Y + 32 * m_d_zoomfactor2 < pClick.Y
                             && !hasD)
                        {
                            hasD = true;
                        }
                    }

                    if (!hasA && !(hasNX && hasY))
                    {
                        if (!cuthasNX && !cuthasY)
                        {
                            wellCtrlPoint.Add(new Point(pClick.X - wellR, pClick.Y + wellR));
                        }
                        else
                        {
                            wellCtrlPoint.Add(new Point(pClick.X - drawedWellR, pClick.Y + drawedWellR));
                        }
                    }
                    if (!hasB && !(hasX && hasY))
                    {
                        if (!cuthasX && !cuthasY)
                        {
                            wellCtrlPoint.Add(new Point(pClick.X + wellR, pClick.Y + wellR));
                        }
                        else
                        {
                            wellCtrlPoint.Add(new Point(pClick.X + drawedWellR, pClick.Y + drawedWellR));
                        }
                    }

                    if (!hasC && !(hasNX && hasNY))
                    {
                        if (!cuthasNX && !cuthasNY)
                        {
                            wellCtrlPoint.Add(new Point(pClick.X - wellR, pClick.Y - wellR));
                        }
                        else
                        {
                            wellCtrlPoint.Add(new Point(pClick.X + drawedWellR, pClick.Y + drawedWellR));
                        }
                    }
                    if (!hasD && !(hasX && hasNY))
                    {
                        if (!cuthasX && !cuthasNY)
                        {
                            wellCtrlPoint.Add(new Point(pClick.X + wellR, pClick.Y - wellR));
                        }
                        else
                        {
                            wellCtrlPoint.Add(new Point(pClick.X + drawedWellR, pClick.Y + drawedWellR));
                        }
                    }
                    Point[] wellCtrlPoints = wellCtrlPoint.Distinct().ToArray();
                    wellCtrlPoint.Clear();
                    foreach (Point onePoint in wellCtrlPoints)
                    {
                        wellCtrlPoint.Add(onePoint);
                    }
                    Console.WriteLine("Count：" + wellCtrlPoint.Count);
                    //排序tan
                    List<Point> newOrderPoint = sortWellCtrl(wellCtrlPoint, pClick);
                    //断层切割
                    List<Point> remainPoint = new List<Point>();
                    List<Point> faultInCtrl = new List<Point>();//在范围内的断层点
                    foreach (Point oneFault in faultPointsAll)
                    {
                        if (ReservorDraw.isInRegion(oneFault, newOrderPoint))
                        {
                            faultInCtrl.Add(oneFault);
                        }
                    }
                    Point[] fPoints = faultInCtrl.Distinct().ToArray();
                    faultInCtrl.Clear();
                    foreach (Point onefPoint in fPoints)
                    {
                        faultInCtrl.Add(onefPoint);
                    }
                    if (faultInCtrl.Count > 1)
                    {
                        // int isCenterLeft = ReservorDraw.isLeft(faultInCtrl[0], faultInCtrl[faultInCtrl.Count-1], pClick);
                        /*if (pClick.X < faultInCtrl[0].X && pClick.X < faultInCtrl[faultInCtrl.Count - 1].X)//在断层左
                        {
                            System.Console.WriteLine("l");
                            foreach (Point temp in newOrderPoint)
                            {
                                if (temp.X < faultInCtrl[0].X && temp.X < faultInCtrl[faultInCtrl.Count - 1].X)
                                {
                                    remainPoint.Add(temp);
                                }
                            }
                        }
                        else if (pClick.X > faultInCtrl[0].X && pClick.X > faultInCtrl[faultInCtrl.Count - 1].X)//在右
                        {
                            System.Console.WriteLine("r");
                            foreach (Point temp in newOrderPoint)
                            {
                                if (temp.X > faultInCtrl[0].X && temp.X > faultInCtrl[faultInCtrl.Count - 1].X)
                                {
                                    remainPoint.Add(temp);
                                }
                            }
                        }
                        else if (pClick.Y < faultInCtrl[0].Y && pClick.Y < faultInCtrl[faultInCtrl.Count - 1].Y)//在下（实际，上）
                        {
                            System.Console.WriteLine("hahah" );
                            foreach (Point temp in newOrderPoint)
                            {
                                if (temp.Y < faultInCtrl[0].Y && temp.Y < faultInCtrl[faultInCtrl.Count - 1].Y)
                                {
                                    remainPoint.Add(temp);
                                }
                            }
                        }
                        else if (pClick.Y > faultInCtrl[0].Y && pClick.Y > faultInCtrl[faultInCtrl.Count - 1].Y)
                        {
                            System.Console.WriteLine("d");
                            foreach (Point temp in newOrderPoint)
                            {
                                if (temp.Y > faultInCtrl[0].Y && temp.Y > faultInCtrl[faultInCtrl.Count - 1].Y)
                                {
                                    remainPoint.Add(temp);
                                }
                            }
                        }*/
                        int pLeft = ReservorDraw.isLeft(faultInCtrl[0], faultInCtrl[faultInCtrl.Count - 1], pClick);
                        foreach (Point temp in newOrderPoint)
                        {
                            if (pLeft * ReservorDraw.isLeft(faultInCtrl[0], faultInCtrl[faultInCtrl.Count - 1], temp) > 0)
                            {
                                remainPoint.Add(temp);
                            }
                        }
                        foreach (Point temp in faultInCtrl)
                        {
                            remainPoint.Add(temp);
                            newOrderPoint = sortWellCtrl(remainPoint, pClick);
                        }
                    }
                    else
                    {
                        // remainPoint = newOrderPoint;
                    }

                    //划线
                    PointCollection pointCollection = new PointCollection();
                    foreach (Point point in newOrderPoint)//newOrderPoint remainPoint
                    {
                        pointCollection.Add(point);
                    }
                    //faultPoints.Clear();
                    if (hasProd)//百米内没有油井不画
                    {
                        if (allCh == null)
                        {
                            Polygon polygon = new Polygon();
                            polygon.Stroke = System.Windows.Media.Brushes.Black;
                            polygon.StrokeThickness = 1;
                            polygon.Points = pointCollection;
                            this.canvesprt.Children.Add(polygon);
                        }
                        //计算储量
                        cal_res(newOrderPoint);
                    }
                    break;
                default:
                    break;

            }
            // switchClickMode = "移动";
        }

        private List<Point> sortWellCtrl(List<Point> wellCtrlPoint, Point pClick)
        {
            //排序tan
            List<Point> newOrderPoint = new List<Point>();
            List<Point> sort1Point = new List<Point>();
            List<Point> sort2Point = new List<Point>();
            foreach (Point point in wellCtrlPoint)
            {
                if (point.X > pClick.X)
                {
                    sort1Point.Add(point);
                }
            }
            //冒泡
            int i, j;
            for (i = 0; i < sort1Point.Count; i++)
            {
                for (j = 0; i + j < sort1Point.Count - 1; j++)
                {
                    if ((pClick.Y - sort1Point[j].Y) / (pClick.X - sort1Point[j].X)
                        > (pClick.Y - sort1Point[j + 1].Y) / (pClick.X - sort1Point[j + 1].X))
                    {
                        Point temp = sort1Point[j];
                        sort1Point[j] = sort1Point[j + 1];
                        sort1Point[j + 1] = temp;
                    }
                }
            }
            foreach (Point point in sort1Point)
            {
                //  System.Console.WriteLine("1---");
                // System.Console.WriteLine(point.Y);
                newOrderPoint.Add(point);
            }
            foreach (Point point in wellCtrlPoint)
            {
                if (point.X == pClick.X && point.Y > pClick.Y)
                {
                    newOrderPoint.Add(point);
                    break;
                }
            }
            foreach (Point point in wellCtrlPoint)
            {
                if (point.X < pClick.X)
                {
                    sort2Point.Add(point);
                }
            }
            //冒泡左侧顺时针 k由大到小
            // foreach (Point point in sort2Point)
            // {
            //  System.Console.WriteLine("2---");
            // System.Console.WriteLine(point.Y);
            // }
            i = 0; j = 0;
            for (i = 0; i < sort2Point.Count; i++)
            {
                for (j = 0; i + j < sort2Point.Count - 1; j++)
                {
                    double k1 = (pClick.Y - sort2Point[j].Y) / (pClick.X - sort2Point[j].X);
                    double k2 = (pClick.Y - sort2Point[j + 1].Y) / (pClick.X - sort2Point[j + 1].X);
                    //System.Console.WriteLine(k1+"...."+ k2);
                    if (k1 > k2)
                    {
                        // System.Console.WriteLine("换掉");
                        Point temp = sort2Point[j];
                        sort2Point[j] = sort2Point[j + 1];
                        sort2Point[j + 1] = temp;
                    }

                }
            }
            foreach (Point point in sort2Point)
            {
                // System.Console.WriteLine("new2---");
                // System.Console.WriteLine(point.Y);
                newOrderPoint.Add(point);
            }
            foreach (Point point in wellCtrlPoint)
            {
                if (point.X == pClick.X && point.Y < pClick.Y)
                {
                    newOrderPoint.Add(point);
                    break;
                }
            }
            return newOrderPoint;
        }
        private void dataGridKSB_RowEditEnding(object sender, EventArgs e)
        {
            Boolean breakFlag = false;//修改点找到
            foreach (DataRow canvesRow in canvesGrid.Rows)
            {
                foreach (DataRow canvesEditRow in ksbEditDT.Rows)
                {
                    if (canvesEditRow["xcount"].Equals(canvesRow["xcount"]) && canvesEditRow["ycount"].Equals(canvesRow["ycount"]))
                    {
                        String ksb = (string)canvesEditRow["ksb"];
                        switch (ksb)//替换原dataTable
                        {
                            case "孔隙度":
                                canvesRow["poro"] = canvesEditRow["val"];
                                break;
                            case "渗透率":
                                canvesRow["permx"] = canvesEditRow["val"];
                                break;
                            case "饱和度":
                                canvesRow["barsa"] = canvesEditRow["val"];
                                break;
                            default:
                                break;
                        }

                        Polygon editedPoly = new Polygon();
                        PointCollection editedPoints = new PointCollection();
                        editedPoints.Add(new Point((double)canvesRow["x0"], (double)canvesRow["y0"]));
                        editedPoints.Add(new Point((double)canvesRow["x1"], (double)canvesRow["y1"]));
                        editedPoints.Add(new Point((double)canvesRow["x3"], (double)canvesRow["y3"]));
                        editedPoints.Add(new Point((double)canvesRow["x2"], (double)canvesRow["y2"]));
                        Color myColor = new Color();
                        switch (drawTypeStr)
                        {
                            case "孔隙度":
                                myColor = barsaColor((double)canvesRow["poro"]);
                                break;
                            case "渗透率":
                                myColor = barsaColor((double)canvesRow["permx"]);
                                break;
                            case "饱和度":
                                myColor = barsaColor((double)canvesRow["barsa"]);
                                break;
                            default:
                                break;
                        }
                        SolidColorBrush myBrush = new SolidColorBrush(myColor);
                        editedPoly.Fill = myBrush;
                        editedPoly.Points = editedPoints;
                        this.canvesprt.Children.Add(editedPoly);
                        breakFlag = true;
                    }
                }
                if (breakFlag)
                {
                    // System.Console.WriteLine((double)canvesRow["poro"]);
                    // System.Console.WriteLine((double)canvesRow["barsa"]);
                    // System.Console.WriteLine((double)canvesRow["permx"]);
                    break;
                }
            }
        }


        /**
         * 储量计算
         * */
        private void cal_res(object sender, RoutedEventArgs e)
        {
            double h = 0;// 
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            //System.Console.WriteLine("选中的list:"+ReservorDraw.pointList.Count());
            try
            {
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
                // var points = ReservorDraw.pointList;
                // s = Math.Abs(points.Take(points.Count - 1)
                //   .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                //   .Sum() / 2 / 1000000);//据说计算面积
                foreach (DataRow canvesRow in canvesGrid.Rows)
                {
                    Point p1 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                    Point p2 = new Point((double)canvesRow["x1"], (double)canvesRow["y1"]);
                    Point p3 = new Point((double)canvesRow["x2"], (double)canvesRow["y2"]);
                    Point p4 = new Point((double)canvesRow["x3"], (double)canvesRow["y3"]);
                    Point rawp1 = new Point((double)canvesRow["rawx0"], (double)canvesRow["rawy0"]);
                    Point rawp2 = new Point((double)canvesRow["rawx1"], (double)canvesRow["rawy1"]);
                    Point rawp3 = new Point((double)canvesRow["rawx2"], (double)canvesRow["rawy2"]);
                    Point rawp4 = new Point((double)canvesRow["rawx3"], (double)canvesRow["rawy3"]);
                    if (ReservorDraw.isInRegion(p1, ReservorDraw.pointList)
                        || ReservorDraw.isInRegion(p2, ReservorDraw.pointList)
                        || ReservorDraw.isInRegion(p3, ReservorDraw.pointList)
                        || ReservorDraw.isInRegion(p4, ReservorDraw.pointList))
                    {
                        List<Point> points = new List<Point>();
                        points.Add(rawp1);
                        points.Add(rawp2);
                        points.Add(rawp4);
                        points.Add(rawp3);
                        points.Add(rawp1);
                        s = Math.Abs(points.Take(points.Count - 1)
                          .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                          .Sum() / 2 / 1000000);//据说计算面积
                        // System.Console.WriteLine("cal..." + resSum);
                        h = (double)canvesRow["dz"];
                        // System.Console.WriteLine("cal..." + h);
                        // System.Console.WriteLine("cal..." + s);
                        if (h > 0)
                            resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)canvesRow["poro"], (double)canvesRow["barsa"], ro, b0);
                    }
                }
                res_result.Text = resSum.ToString("0.0000") + "万吨";
            }
            catch
            {
                System.Console.WriteLine("格式错误");
            }
        }
        /**
         *  范围储量计算
         */
        private void cal_res(List<Point> pointList)
        {
            double h = 0;
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            try
            {
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
                foreach (DataRow canvesRow in canvesGrid.Rows)
                {
                    Point p1 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                    Point p2 = new Point((double)canvesRow["x1"], (double)canvesRow["y1"]);
                    Point p3 = new Point((double)canvesRow["x2"], (double)canvesRow["y2"]);
                    Point p4 = new Point((double)canvesRow["x3"], (double)canvesRow["y3"]);
                    Point rawp1 = new Point((double)canvesRow["rawx0"], (double)canvesRow["rawy0"]);
                    Point rawp2 = new Point((double)canvesRow["rawx1"], (double)canvesRow["rawy1"]);
                    Point rawp3 = new Point((double)canvesRow["rawx2"], (double)canvesRow["rawy2"]);
                    Point rawp4 = new Point((double)canvesRow["rawx3"], (double)canvesRow["rawy3"]);
                    if (ReservorDraw.isInRegion(p1, pointList)
                        || ReservorDraw.isInRegion(p2, pointList)
                        || ReservorDraw.isInRegion(p3, pointList)
                        || ReservorDraw.isInRegion(p4, pointList))
                    {
                        List<Point> points = new List<Point>();
                        points.Add(rawp1);
                        points.Add(rawp2);
                        points.Add(rawp4);
                        points.Add(rawp3);
                        points.Add(rawp1);
                        s = Math.Abs(points.Take(points.Count - 1)
                          .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                          .Sum() / 2 / 1000000);//据说计算面积
                        h = (double)canvesRow["dz"];
                        if (h > 0)
                            resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)canvesRow["poro"], (double)canvesRow["barsa"], ro, b0);
                    }
                }
                res_result.Text = resSum.ToString("0.0000") + "万吨";
                wellRes4allres = resSum;
            }
            catch
            {
                System.Console.WriteLine("格式错误");
            }
        }
        private void canves1_MouseRightButtonUp_prt(object sender, MouseButtonEventArgs e)
        {
            ReservorDraw.EreaseLine(canvesprt, e);
        }
        SolidColorBrush scb = System.Windows.Media.Brushes.Black;//Red;
        private void prtchangeColor1(object sender, RoutedEventArgs e)
        {

            //switch (colorType)
            //{
            //    case 1:
            scb = System.Windows.Media.Brushes.RoyalBlue;
            //        break;
            //    default:
            //        scb = System.Windows.Media.Brushes.Red;
            //        break;
            // }
            switchClickMode = "染色";
        }
        private void prtchangeColor2(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.Violet;
            switchClickMode = "染色";
        }
        private void prtchangeColor3(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.YellowGreen;
            switchClickMode = "染色";
        }
        private void prtchangeColor4(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.SlateBlue;
            switchClickMode = "染色";
        }
        private void prtchangeColor5(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.PaleGoldenrod;
            switchClickMode = "染色";
        }
        private void prtchangeColor6(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.Yellow;
            switchClickMode = "染色";
        }
        private void prtchangeColor7(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.Tomato;
            switchClickMode = "染色";
        }
        private void prtchangeColor8(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.Sienna;
            switchClickMode = "染色";
        }
        private void prtchangeColor9(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.Silver;
            switchClickMode = "染色";
        }
        private void prtchangeColor10(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.Black;
            switchClickMode = "染色";
        }
        private void prtchangeColor11(object sender, RoutedEventArgs e)
        {
            scb = System.Windows.Media.Brushes.SpringGreen;
            switchClickMode = "染色";
        }
        private void switch2move(object sender, RoutedEventArgs e)
        {
            switchClickMode = "移动";
        }
        private void switch2select(object sender, RoutedEventArgs e)
        {
            switchClickMode = "圈选";
        }
        //孔渗饱查看切换
        private void switchKSB(object sender, RoutedEventArgs e)
        {
            switchClickMode = "孔渗饱";
        }
        private void canves1_MouseWheel_poly(object sender, MouseWheelEventArgs e)
        {
            // ReservorDraw.zoom_in(canvespoly, e);
        }

        /**
         * 可采储量预测
         * 2016-6-26 11:11:48
         */
        DataTable data;
        private void findReservers(object sender, EventArgs e)
        {
            if ("区块".Equals(ComboBoxType.Text))
            {
                reservers_qk(sender, e);
            }
            else
            {
                reservers_jh(sender, e);
            }
        }
        private void reservers_jh(object sender, EventArgs e)
        {
            //textReservers.Text = "正在计算...";
            数模建模.SIMB.Reserve sns = new 数模建模.SIMB.Reserve();
            data = sns.djncyl(this.textBoxJH.Text);//比如8B102
            refresh_reservers();
            this.dataGridReservers.ItemsSource = data.DefaultView;
        }

        private void reservers_qk(object sender, EventArgs e)
        {
            textReservers.Text = "正在计算...";
            数模建模.SIMB.Reserve sns = new 数模建模.SIMB.Reserve();
            data = sns.qkljcyl(this.ComboBoxQk.Text);//比如 SFT4K
            refresh_reservers();
            this.dataGridReservers.ItemsSource = data.DefaultView;
        }

        private void dataGridReservers_RowEditEnding(object sender, EventArgs e)
        {
            /* Ncyl_data ncyl_data = new Ncyl_data();   //我自己的数据表实例类  
             ncyl_data = e.Row.Item as Ncyl_data;        //获取该行的记录  */
            refresh_reservers();
            //this.dataGridReservers.ItemsSource = data.DefaultView;
        }

        private void reservers_init()
        {

            ObservableCollection<Qkdy_ent> imgs = new ObservableCollection<Qkdy_ent>();
            try//2016-11-2 18:47:17 可以无网启动
            {
                数模建模.SIMB.Reserve sns = new 数模建模.SIMB.Reserve();
                DataTable tmpdata = sns.qkdy();
                foreach (DataRow drow in tmpdata.Rows)
                {
                    Qkdy_ent qkdy = new Qkdy_ent();
                    qkdy.Qkdy = drow["qkdy"].ToString();
                    imgs.Add(qkdy);
                }
            }
            catch
            {
                Console.WriteLine("网络连接中断");
            }
            //进行绑定
            ComboBoxQk.DisplayMemberPath = "Qkdy";//控件显示的列名
            ComboBoxQk.SelectedValuePath = "Qkdy";//控件值的列名
            ComboBoxQk.ItemsSource = imgs;
        }
        class Qkdy_ent   //声明类
        {
            string qkdy;
            public string Qkdy
            {
                get { return qkdy; }
                set { qkdy = value; }
            }
        }
        private void reserversType_changed(object sender, EventArgs e)
        {
            if ("单井".Equals(ComboBoxType.Text))//上次显示的值？
            {
                this.textBoxJH.Visibility = Visibility.Hidden;
                this.ComboBoxQk.Visibility = Visibility.Visible;
            }
            else if ("区块".Equals(ComboBoxType.Text))
            {
                this.textBoxJH.Visibility = Visibility.Visible;
                this.ComboBoxQk.Visibility = Visibility.Hidden;
            }
        }

        private void refresh_reservers()
        {
            Dictionary<int, double> d = new Dictionary<int, double>(); ;
            foreach (DataRow drow in data.Rows)
            {
                d.Add(Convert.ToInt32(drow["nf"].ToString()), Convert.ToDouble(drow["ncyl"].ToString()));
            }
            ((LineSeries)mcChart.Series[0]).ItemsSource = d;
            数模建模.SIMB.Reserve sns = new 数模建模.SIMB.Reserve();
            double reserve = sns.qkReserve(data);
            if (reserve < 0)
            {
                textReservers.Text = "未处于自然递减阶段，预测无效。";
            }
            else
                textReservers.Text = "预测剩余可采储量：" + reserve + "万吨";
        }

        /**
         * 饱和度和断层
         * 2016-7-2 11:33:06
         */

        private void findDc(object sender, RoutedEventArgs e)
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            double timed = ts.TotalSeconds;
            //string fgridpath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\F10-27RIGHT_E100.FGRID";
            //string prtpath = "E:\\wf.txt";
            //string schIncPath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\f10-27right_sch.inc";//含井号
            //string gothIncPath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\f10-27right_goth.inc";//断层吗
            ////string regIncPath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\f10-27right_reg.inc";
            //string faciesPath = "E:\\1.txt";//相图吧

            XmlHelper helper = new XmlHelper();
            string fgridpath = helper.GetXMLDocument("FGRID");
            string prtpath = helper.GetXMLDocument("PRTINC");
            string schIncPath = helper.GetXMLDocument("SCH");
            string gothIncPath = helper.GetXMLDocument("GOTH");
            string faciesPath = helper.GetXMLDocument("FACIES");
            // 2017年5月8日 19:30:22 新文件
            //  净毛比、孔隙度、渗透率关键字分别为NTG、PORO、PERMX
            string gproPath = helper.GetXMLDocument("GPRO");
            string finitPath = helper.GetXMLDocument("FINIT");
            string ch = this.ComboBoxCH.Text;
            drawTypeStr = this.drawtype.Text;
            if (allCh != null)
            {
                ch = allCh;
                drawTypeStr = "相图";
            }

            string combo_soiltimeStr = this.combo_soiltime.Text;
            //initCanvesGrid();二次实例化报错
            double d = 0;
            double a = 0;
            double b = 0;
            allVol = 0;
            double ro = 0;
            double b0 = 0;
            int onefacies = 0;

            canvesGrid.Clear();
            perfectPoints.Clear();//注采完善区域
            if (ch != null && !"".Equals(ch))//选中层号之后才开始
            {
                Canvas canvesptr = this.canvesprt;
                canvesptr.Children.Clear();
                ReservorDraw.pointList.Clear();
                数模建模.SIMB.FgridNew fgridNew = new 数模建模.SIMB.FgridNew();
                数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
                try
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
                }
                //调用主线程UI的的代码  
                //    new Thread(o =>
                //  { 
                System.Console.WriteLine("开始解析文件:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
                // 解析文件
                tablesize = fgridPrt.readFGRID(fgridpath);
                DataTable dtfgridNew = fgridNew.readFile(fgridpath, ch);
                DataTable dtprt = fgridPrt.readPRT(prtpath, ch, combo_soiltimeStr);// 现饱和度 旧版读取 孔隙度、渗透率
                faultDt = fgridPrt.readGothInc(gothIncPath, ch);//断层
                fgridPrt.readSchInc(schIncPath, ch);
                double[] poro;// = fgridPrt.poro;//孔隙度 旧版 现在为空值
                //permx = fgridPrt.permx;//渗透率 旧版 现在为空值
                DataTable dzDt = fgridPrt.dzDt;//厚度 旧版 不用了
                welspecs = fgridPrt.welspecs;//井口网格坐标
                ntgs = fgridPrt.readNTG(gproPath);// 净毛比 2017年5月8日 20:43:53
                poro = fgridPrt.poro;// 孔隙度 2017年5月8日 20:44:06
                permx = fgridPrt.permx;// 渗透率 2017年5月8日 20:44:08.
                dzs = fgridPrt.readDz(finitPath);
                //
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
                if (0 == list.Count)
                {
                    MessageBox.Show("Prt文件没有数据");
                    Environment.Exit(0);
                }
                list.Sort();
                double minpermx = Convert.ToDouble(list[0]);
                double maxpermx = Convert.ToDouble(list[list.Count - 1]);
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
                double minporo = Convert.ToDouble(list[0]);
                double maxporo = Convert.ToDouble(list[list.Count - 1]);

                // 计算缩放比例
                double dzoomx2, dzoomy2;
                double maxX2 = Convert.ToDouble(dtfgridNew.Compute("max(x)", ""));
                double maxY2 = Convert.ToDouble(dtfgridNew.Compute("max(y)", ""));
                double minX2 = Convert.ToDouble(dtfgridNew.Compute("min(x)", ""));
                double minY2 = Convert.ToDouble(dtfgridNew.Compute("min(y)", ""));
                //System.Console.WriteLine("-------" + maxX2);
                dzoomx2 = canvesptr.ActualWidth / (maxX2 - minX2);
                dzoomy2 = canvesptr.ActualHeight / (maxY2 - minY2);
                m_d_zoomfactor2 = dzoomx2 < dzoomy2 ? dzoomx2 : dzoomy2;//改成取小的了 为了展示全图  2016-11-2 19:24:08
                wellR = 200;
                wellR = wellR * m_d_zoomfactor2;
                int prtXCount = 0;
                int prtYCount = 0;
                //double  dcwidth = 64;//同断层 假设相邻点32m
                //dcwidth=dcwidth*m_d_zoomfactor2;
                //System.Console.WriteLine("m_d_zoomfactor2:"+m_d_zoomfactor2);
                d = d * m_d_zoomfactor2;
                //准备画井点用的
                wellCoord = fgridPrt.welspecsFakerWellCoord;//wellCoord;伪装2016-12-28 18:18:22
                wellCoordTrueEnd = fgridPrt.wellCoord;//原wellCoord
                DataTable wellStat = fgridPrt.wellStat;
                Ellipse[] wellpoints = new Ellipse[wellCoord.Rows.Count];
                Ellipse[] wellborders = new Ellipse[wellCoord.Rows.Count];
                TextBlock[] TextNames = new TextBlock[wellCoord.Rows.Count];
                double[] pointX = new double[wellCoord.Rows.Count];
                double[] pointY = new double[wellCoord.Rows.Count];
                int jhCount = 0;
                facies = null;

                if ("相图".Equals(drawTypeStr))
                {
                    facies = fgridPrt.readFacies(faciesPath); // 新版变成旧版了 2017年5月23日 14:29:04变成新版
                    // 2017年5月23日 14:29:20变成旧版
                    //facies = fgridPrt.readRegInc(faciesPath); // 原旧版 重新启用 2017年5月8日 21:02:39 
                }
                //ntgs=fgridPrt.readNTG(gproPath);// 净毛比 2017年5月8日 20:43:53
                // poro = fgridPrt.poro;// 孔隙度 2017年5月8日 20:44:06
                //permx = fgridPrt.permx;// 渗透率 2017年5月8日 20:44:08
                System.Console.WriteLine("底图绘制:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
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
                    //myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
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

                    //if (pointl(new Point(x0, y0), new Point(156.164925685062, 285.026937384162)) < 1)
                    //{
                    //    System.Console.WriteLine(prtYCount + "x" + prtXCount);
                    //}

                    //上prt
                    int hadC = (Convert.ToInt32(ch) - 1) * tablesize[0] * tablesize[1]; //跳过前几层
                    int hady = prtYCount * tablesize[0];//跳过前几行
                    double val = Convert.ToDouble(dtprt.Rows[prtYCount][prtXCount]);//饱和度
                    double dzval = 3;
                    dzval = dzs[hadC + hady + prtXCount];
                    noDzAlert = false;
                    /*if (DBNull.Value != dzDt.Rows[prtYCount][prtXCount])// 2017年5月10日 14:29:41缺失关键字
                    {
                        dzval = Convert.ToDouble(dzDt.Rows[prtYCount][prtXCount]);
                        noDzAlert = false;
                    }
                    if (noDzAlert)
                    {
                        Console.WriteLine("++++++++没有DZ++++++++");
                        MessageBox.Show("PRT文件没有DZ，默认置3");
                        noDzAlert = false;//不用再提醒啦 分层分相都会多次调用
                    }*/
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
                        double s = Math.Abs(points.Take(points.Count - 1)
                          .Select((p, si) => (points[si + 1].X - p.X) * (points[si + 1].Y + p.Y))
                          .Sum() / 2 / 1000000);//km2
                        allVol = allVol + 100 * s * dzval * poro[hadC + hady + prtXCount] * val * ro / b0;
                    }
                    if ("相图".Equals(drawTypeStr))
                    {
                        onefacies = facies[hadC + hady + prtXCount];
                        if (onefacies > 0 && onefacies < 11 && null == allCh)
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
                                    // System.Console.WriteLine(valBottom + "a" + valTop);
                                    Color myColor = barsaColor(val);
                                    SolidColorBrush myBrush = new SolidColorBrush(myColor);
                                    myPolygon2.Fill = myBrush;
                                }
                                break;
                            case "渗透率":
                                /* ArrayList list = new ArrayList(permx); 
                                 list.Sort();
                                 double minpermx = Convert.ToDouble(list[0]);
                                 double maxpermx = Convert.ToDouble(list[list.Count - 1]); */
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
                                    //System.Console.WriteLine(fd);
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
                    if (null == allCh)
                    {
                        foreach (DataRow faultRow in faultDt.Rows)
                        {
                            if ((prtXCount + 1 + "").Equals(faultRow["i"].ToString())
                             && (prtYCount + 1 + "").Equals(faultRow["j"].ToString()))
                            {
                                SolidColorBrush myBrush = Brushes.Gray;
                                myPolygon2.Fill = myBrush;
                            }
                        }
                        //相图孔渗饱和断层
                        myPolygon2.Points = myPointCollection2;
                        canvesptr.Children.Add(myPolygon2);//带颜色的小四边形
                    }
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
                    canvesGridRow["NTG"] = ntgs[hadC + hady + prtXCount];
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
                }

                //System.Console.WriteLine("allVol:" + allVol);
                if (allVol > 0)
                {
                    this.ch_res_all.Text = "本层储量" + allVol.ToString("0.0000") + "万吨";
                }
                for (int i = 0; i < wellpoints.Length; i++)
                {
                    if (wellpoints[i] != null && null == allCh)
                    {
                        canvesptr.Children.Add(wellpoints[i]);
                        //canvesptr.Children.Add(wellborders[i]);
                        canvesptr.Children.Add(TextNames[i]);
                    }
                }

                //断层 他项权证
                //List<PointCollection> faultPointCollections=new  List<PointCollection>();                
                List<Point> faultPoints = new List<Point>();
                faultPointsAll.Clear();
                faultPointsAll_Direct.Clear();
                foreach (DataRow faultRow in faultDt.Rows)
                {
                    int k = (int)faultRow["k"];
                    string direct = faultRow["direct"].ToString();
                    faultPointsAll_Direct.Add(direct);
                    if (k > 0)
                    {
                        int i = (int)faultRow["i"];
                        int j = (int)faultRow["j"];
                        foreach (DataRow canvesRow in canvesGrid.Rows)
                        {
                            if (i - 1 == (int)canvesRow["xcount"] && j - 1 == (int)canvesRow["ycount"])
                            {
                                //System.Console.WriteLine((double)canvesRow["x3"]+"f"+ (double)canvesRow["y3"]);
                                faultPoints.Add(new Point((double)canvesRow["x3"], (double)canvesRow["y3"]));
                                faultPointsAll.Add(new Point((double)canvesRow["x3"], (double)canvesRow["y3"]));
                                break;
                            }
                        }
                    }
                    else//断层结束点-1（自定义）
                    {
                        faultPointsAll.Add(new Point(-1, -1));
                        PointCollection faultPointCollection = new PointCollection();
                        //faultPointCollection.Clear();//就全没有了
                        foreach (Point point in faultPoints)
                        {
                            faultPointCollection.Add(point);
                        }
                        faultPoints.Clear();
                        //System.Console.WriteLine("draw it!" + faultPointCollection.Count);
                        /* Polyline faultPolyline = new Polyline();
                         faultPolyline.Stroke = System.Windows.Media.Brushes.Brown;
                         faultPolyline.StrokeThickness = 1;
                         faultPolyline.Points = faultPointCollection;
                         canvesptr.Children.Add(faultPolyline);*/
                    }
                }
                //调用主线程UI的的代码
                //}));  
                //   }) { IsBackground = true }.Start();
                //最后我们来个图例
                if (null == allCh)
                {
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
                System.Console.WriteLine("end:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
            }
            else
            {
                fgrid_ch(fgridpath);
                数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
                soiltimeList = fgridPrt.readSoilTime(prtpath);
                foreach (string soiltime in soiltimeList)
                {
                    this.combo_soiltime.Items.Add(soiltime);
                }
                this.combo_soiltime.SelectedIndex = 0;
            }
        }

        //找出极值点
        private List<Point[]> topPointAt1(Point[] points, Point[] pointsRight, double widthd)
        {
            for (int i = 0; i <= points.Length - 1; i++)//(Point tmpp in points)
            {
                int topCount = 0;
                foreach (Point otherP in points)
                {
                    if (pointl(points[i], otherP) < widthd)//找出只有一个相邻点
                    {
                        topCount++;
                    }
                }
                if (1 == topCount)
                {
                    Point temp;
                    temp = points[0];
                    points[0] = points[i];
                    points[i] = temp;
                    //对岸
                    temp = pointsRight[0];
                    pointsRight[0] = pointsRight[i];
                    pointsRight[i] = temp;
                    break;
                }
            }
            return sortPoint(points, pointsRight, widthd);
        }
        //邻位排序
        private List<Point[]> sortPoint(Point[] points, Point[] pointsRight, double widthd)
        {
            double minDistance = 1000;
            int minj = 0;
            for (int i = 0; i <= points.Length - 1; i++)
            {
                for (int j = i + 1; j <= points.Length - 1; j++)
                {
                    double templ = pointl(points[i], points[j]);
                    if (templ < minDistance)
                    {
                        minDistance = templ;
                        minj = j;
                    }

                    //Point temp;
                    // temp = points[i];
                    // points[i ] = points[j];
                    // points[j] = temp;
                    //对岸 
                    //  temp = pointsRight[i + 1];
                    //  pointsRight[i + 1] = pointsRight[j];
                    // pointsRight[j] = temp;
                    // }
                }
                // System.Console.WriteLine(pointl(points[i], points[minj]));
                if (i != points.Length - 1)
                {
                    if (pointl(points[i], points[minj]) < widthd)//该点没有相邻点。。好多为啥？
                    {
                        Point temp;
                        temp = points[i + 1];
                        points[i + 1] = points[minj];
                        points[minj] = temp;
                        // 对岸 
                        temp = pointsRight[i + 1];
                        pointsRight[i + 1] = pointsRight[minj];
                        pointsRight[minj] = temp;
                    }
                    else//
                    {
                        System.Console.WriteLine(pointl(points[i], points[minj]) + ">" + widthd);
                    }

                }
            }
            List<Point[]> ll = new List<Point[]>();
            ll.Add(points);
            ll.Add(pointsRight);
            return ll;
        }
        //点位排序
        private Point[] sortAngle(Point[] p, Point minP)
        {
            for (int i = 0; i <= p.Length - 1; i++)
            {
                for (int j = 0; j <= p.Length - 2; j++)
                {
                    //if (p[j + 1] < p[j])
                    //if (p[j] == p[0])
                    //{
                    //    break;
                    //}
                    if (Angle(minP, p[j + 1]) < Angle(minP, p[j]))
                    {
                        Point temp;
                        temp = p[j];
                        p[j] = p[j + 1];
                        p[j + 1] = temp;
                    }
                    else if (Angle(minP, p[j + 1]) == Angle(minP, p[j]))
                    {

                    }
                }
            }
            return p;
        }
        //判断角度
        private double Angle(Point p0, Point p1)
        {
            double si;
            double l = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2));
            double l2 = Math.Abs(p1.X - p0.X);
            if (p1.Y < p0.Y)
            {
                si = Math.Acos(l2 / l);
            }
            else
            {
                si = Math.Acos(-l2 / l);
            }
            return si;
        }
        //判断距离
        private double pointl(Point p0, Point p1)
        {
            double l = Math.Sqrt(Math.Pow(p1.X - p0.X, 2) + Math.Pow(p1.Y - p0.Y, 2));
            return l;
        }
        //层号下拉菜单
        int maxCh = 0;
        private void fgrid_ch(String filepath)
        {
            ObservableCollection<Fgrid_ch> imgs = new ObservableCollection<Fgrid_ch>();
            数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
            // 解析文件
            int[] tablesize = fgridPrt.readFGRID(filepath);
            maxCh = tablesize[2];
            for (int i = 1; i <= tablesize[2]; i++)
            {
                Fgrid_ch fgrid_ch = new Fgrid_ch();
                fgrid_ch.Ch = "" + i;
                imgs.Add(fgrid_ch);
            }
            //进行绑定
            ComboBoxCH.DisplayMemberPath = "Ch";//控件显示的列名
            ComboBoxCH.SelectedValuePath = "Ch";//控件值的列名
            ComboBoxCH.ItemsSource = imgs;
            this.ComboBoxCH.SelectedIndex = 0;
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



        private void cal_fitting(object sender, RoutedEventArgs e)
        {
            String filepath = this.fitting_file.filePath.Text;// "E:\\Documents\\项目开发\\MyWork\\8\\需求\\拟合率.xls";
            double headErrNum = 10;
            double midErrNum = 5;
            double endErrNum = 2;
            try
            {
                headErrNum = Convert.ToDouble(this.headErr.Text);
                midErrNum = Convert.ToDouble(this.midErr.Text);
                endErrNum = Convert.ToDouble(this.endErr.Text);
            }
            catch
            {
                System.Console.WriteLine("格式错误");
            }
            //String standard = "10";// this.fitting_standard.Text;
            /*try
            {*/
            数模建模.SIMB.Fitting sns = new 数模建模.SIMB.Fitting(filepath, headErrNum, "前期");
            //sns.importExcelToDataSet();
            this.fitting_num.Text = "" + sns.getNgNum();
            this.fitting_rate.Text = sns.getNgRate() + "%";
            DataTable fittingdt = sns.dt;
            //画曲线
            /*Dictionary<string, double> d = new Dictionary<string, double>();
            int countx = 0;
            foreach (DataRow drow in fittingdt.Rows)
            {
                countx++;
                string[] dateStrAll = drow["DATE"].ToString().Split(' ');
                d.Add(dateStrAll[0], Convert.ToDouble(drow["val"].ToString()));
            }
            ((LineSeries)fittingChart.Series[0]).ItemsSource = d;*/
            /* }
             catch
             {
                 System.Console.WriteLine("ERROR:cal_fitting");
             }*/
            /* DataTable dtaa = sns.getTable();
             foreach (DataRow rowaa in dtaa.Rows)
             {
                 System.Console.WriteLine(rowaa[0].ToString());
                 System.Console.WriteLine(rowaa[1].ToString());
                 System.Console.WriteLine(rowaa[2].ToString());
                 System.Console.WriteLine(rowaa[3].ToString());
             }
             System.Console.WriteLine("-------------");*/
            //中期
            数模建模.SIMB.Fitting sns_mid = new 数模建模.SIMB.Fitting(filepath, midErrNum, "中期");
            this.fitting_num_mid.Text = "" + sns_mid.getNgNum();
            this.fitting_rate_mid.Text = sns_mid.getNgRate() + "%";
            DataTable fittingdt_mid = sns_mid.dt;
            //画曲线
            /*Dictionary<string, double> d_mid = new Dictionary<string, double>();
            foreach (DataRow drow in fittingdt_mid.Rows)
            {
                string[] dateStrAll = drow["DATE"].ToString().Split(' ');
                d_mid.Add(dateStrAll[0], Convert.ToDouble(drow["val"].ToString()));
            }
            ((LineSeries)fittingChart_mid.Series[0]).ItemsSource = d_mid;*/
            //后期
            数模建模.SIMB.Fitting sns_last = new 数模建模.SIMB.Fitting(filepath, endErrNum, "后期");
            this.fitting_num_last.Text = "" + sns_last.getNgNum();
            this.fitting_rate_last.Text = sns_last.getNgRate() + "%";
            DataTable fittingdt_last = sns_last.dt;

            // 合并Datatable
            DataTable newDataTable = fittingdt.Copy();
            //添加DataTable2的数据
            foreach (DataRow dr in fittingdt_mid.Rows)
            {
                newDataTable.ImportRow(dr);
            }
            foreach (DataRow dr in fittingdt_last.Rows)
            {
                newDataTable.ImportRow(dr);
            }
            //画曲线
            Dictionary<string, double> d_last = new Dictionary<string, double>();
            foreach (DataRow drow in newDataTable.Rows)
            {
                string[] dateStrAll = drow["DATE"].ToString().Split(' ');
                d_last.Add(dateStrAll[0], Convert.ToDouble(drow["val"].ToString()));
            }
            ((LineSeries)fittingChart_last.Series[0]).ItemsSource = d_last;
        }

        private void window_keyDown(object sender, KeyEventArgs e)
        {
            //事件处理
            System.Console.WriteLine("window_keyDown:");
        }
        private void selectWell(object sender, RoutedEventArgs e)
        {
            switchClickMode = "单井井控范围";
        }
        /// <summary>
        /// 井控范围内的断层边部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawFaultBorder(object sender, RoutedEventArgs e)
        {
            drawWellCtrlFlag = false;
            drawWellCtrl(sender, e);
            drawWellCtrlFlag = true;
            List<Point> otherSidePoint = new List<Point>();
            List<Point> oneSidePoint = faultPointsAll;
            String textdcRange = this.textdcRange.Text;
            double textdcRangeD = 0.0;
            double h = 0;// 
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            //画井点用的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            //System.Console.WriteLine("选中的list:"+ReservorDraw.pointList.Count());

            try
            {
                if (textdcRange != null && !"".Equals(textdcRange))
                {
                    textdcRangeD = Convert.ToDouble(this.textdcRange.Text) * m_d_zoomfactor2;
                }
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch
            {
                System.Console.WriteLine("textdcRangeD格式错误");
            }
            // System.Console.WriteLine("textdcRangeD:" + textdcRangeD);
            if (b0 > 0)
            {

                foreach (DataRow canvesRow in canvesGrid.Rows)
                {
                    foreach (Point faultPoint3 in oneSidePoint)
                    {
                        double x3 = (double)canvesRow["x3"];
                        double y3 = (double)canvesRow["y3"];
                        if (x3 <= faultPoint3.X + textdcRangeD
                            && x3 >= faultPoint3.X - textdcRangeD
                            && y3 <= faultPoint3.Y + textdcRangeD
                            && y3 >= faultPoint3.Y - textdcRangeD
                            //&& !"fault".Equals(canvesRow["checkpoint"].ToString())
                            )
                        {
                            //canvesRow["checkpoint"] = "fault";
                            double oil = (double)canvesRow["barsa"];//饱和度
                            if (oil > 0)
                            {
                                bool inWellCtrlFlag = false;
                                foreach (List<Point> oneWellCtrl in wellCtrlList)
                                {
                                    Point point0 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                                    if (ReservorDraw.isInRegion(point0, oneWellCtrl))
                                    {
                                        inWellCtrlFlag = true;
                                        break;
                                    }
                                }
                                if (inWellCtrlFlag)
                                {
                                    Polygon myPolygon2 = new Polygon();
                                    PointCollection myPointCollection2 = new PointCollection();
                                    //myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                    myPolygon2.StrokeThickness = 0.00;//00000000000000
                                    Point p0 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                                    Point p1 = new Point((double)canvesRow["x1"], (double)canvesRow["y1"]);
                                    Point p2 = new Point((double)canvesRow["x2"], (double)canvesRow["y2"]);
                                    Point p3 = new Point((double)canvesRow["x3"], (double)canvesRow["y3"]);
                                    Point rawp1 = new Point((double)canvesRow["rawx0"], (double)canvesRow["rawy0"]);
                                    Point rawp2 = new Point((double)canvesRow["rawx1"], (double)canvesRow["rawy1"]);
                                    Point rawp3 = new Point((double)canvesRow["rawx2"], (double)canvesRow["rawy2"]);
                                    Point rawp4 = new Point((double)canvesRow["rawx3"], (double)canvesRow["rawy3"]);
                                    myPointCollection2.Add(p0);
                                    myPointCollection2.Add(p1);
                                    myPointCollection2.Add(p3);
                                    myPointCollection2.Add(p2);
                                    myPolygon2.Points = myPointCollection2;
                                    myPolygon2.Fill = Brushes.Gray;
                                    if (null == allCh)
                                    {
                                        this.canvesprt.Children.Add(myPolygon2);
                                    }

                                    List<Point> points = new List<Point>();
                                    points.Add(rawp1);
                                    points.Add(rawp2);
                                    points.Add(rawp4);
                                    points.Add(rawp3);
                                    points.Add(rawp1);
                                    s = Math.Abs(points.Take(points.Count - 1)
                                     .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                     .Sum() / 2 / 1000000);//据说计算面积
                                    h = (double)canvesRow["dz"];
                                    if (h > 0)
                                    {
                                        resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)canvesRow["poro"], (double)canvesRow["barsa"], ro, b0);
                                    }
                                }
                            }
                            break;
                        }
                    }
                    //画井点
                    if (null == allCh)
                    {
                        drawWells(canvesRow, wellPoints, wellNames);
                    }
                }
            }

            if (resSum > 0)
            {
                this.ch_res_all.Text = "断层储量" + resSum.ToString("0.0000") + "万吨";
                tmpVol4EachRes = resSum.ToString("0.0000");
            }
            else
                tmpVol4EachRes = "0";
            //画井点
            if (null == allCh)
            {
                for (int i = 0; i < wellPoints.Count; i++)
                {
                    if (wellPoints[i] != null)
                    {
                        this.canvesprt.Children.Add(wellPoints[i]);
                        this.canvesprt.Children.Add(wellNames[i]);
                    }
                }
            }
            /*
           for( int i =0 ;i<oneSidePoint.Count;i++)
           {
               Point oneFaultBorder=oneSidePoint[i];
               if (oneFaultBorder.X > 0)
               {
                   if ("Y".Equals(faultPointsAll_Direct[i]))
                   {
                       oneSidePoint[i] = new Point(oneFaultBorder.X, oneFaultBorder.Y + textdcRangeD);
                       otherSidePoint.Add( new Point(oneFaultBorder.X, oneFaultBorder.Y - textdcRangeD));
                   }
                   else
                   {
                       oneSidePoint[i] = new Point(oneFaultBorder.X + textdcRangeD, oneFaultBorder.Y);
                       otherSidePoint.Add( new Point(oneFaultBorder.X - textdcRangeD, oneFaultBorder.Y));
                   }
               }
               else
               {
                   otherSidePoint.Add( new Point(-1, -1));
               }
           }
           for (int i = 0; i < oneSidePoint.Count; i++) //
           {
               Polygon myPolygon = new Polygon();
               PointCollection myPointCollection = new PointCollection();
               myPolygon.Stroke = System.Windows.Media.Brushes.Black;
               myPolygon.StrokeThickness = 1;
               int k = 0;
               for (int j = i; j < oneSidePoint.Count; j++)
               {
                   if (oneSidePoint[j].X < 0)
                       break;
                   myPointCollection.Add(new Point(oneSidePoint[j].X, oneSidePoint[j].Y));
                   k++;
               }
               //怎么倒着排过来
               List<Point> tempOtherSide = new List<Point>();
               for (int j = i; j < otherSidePoint.Count; j++)
               {
                   if (otherSidePoint[j].X < 0)
                       break;
                   tempOtherSide.Add(new Point(otherSidePoint[j].X, otherSidePoint[j].Y));
                  
               }
               for (int j = tempOtherSide.Count-1; j>=0; j--)
               {
                   if (tempOtherSide[j].X < 0)
                       break;
                   myPointCollection.Add(new Point(tempOtherSide[j].X, tempOtherSide[j].Y));
               }
               myPolygon.Points = myPointCollection;
               this.canvesprt.Children.Add(myPolygon);//带颜色的小四边形
               i = i + k;
           }*/
        }

        /// <summary>
        /// 砂体边部
        /// 不含井控范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        bool drawWellCtrlFlag = true;
        private void drawSandBorder(object sender, RoutedEventArgs e)
        {
            drawWellCtrlFlag = false;
            drawWellCtrl(sender, e);//找井控范围，但是不画画
            drawWellCtrlFlag = true;
            List<Point> sandBorderPoint = new List<Point>();
            double textSandD = 0;
            double h = 0;// 
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            try
            {
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch
            {
                System.Console.WriteLine("res_density格式错误");
            }
            try
            {
                if (textSand != null && !"".Equals(textSand))
                {
                    textSandD = Convert.ToDouble(this.textSand.Text) * m_d_zoomfactor2;
                }
            }
            catch
            {
                System.Console.WriteLine("textSandD格式错误");
            }
            //找边界点
            //tablesize x ,y, z
            for (int i = 0; i < canvesGrid.Rows.Count; i++)
            {
                DataRow canvesRow = canvesGrid.Rows[i];
                string thisFacies = canvesRow["facies"].ToString();
                Point thisPoint = new Point((double)canvesRow["x3"], (double)canvesRow["y3"]);
                int prtXCount = (int)canvesRow["xcount"];
                int prtYCount = (int)canvesRow["ycount"];
                // if (thisFacies.Length > 0) Console.WriteLine("thisFacies"+thisFacies);
                if (thisFacies.Length > 0 && !"0".Equals(thisFacies))//未考虑断层影响（坐标差距空白点）
                {
                    //int colNum = (int)canvesRow["xcount"];
                    //int rowNum = (int)canvesRow["ycount"];// 从零开始
                    //if ())
                    //{
                    bool isSandBorder = false;
                    if (i > tablesize[0] - 1 //不是第一行
                         && !thisFacies.Equals(canvesGrid.Rows[i - tablesize[0]]["facies"].ToString()))//上一行的值不相等
                    {
                        isSandBorder = true;
                    }
                    else if (i + tablesize[0] < canvesGrid.Rows.Count //不是最后
                        && !thisFacies.Equals(canvesGrid.Rows[i + tablesize[0]]["facies"].ToString()))//下
                    {
                        isSandBorder = true;
                    }
                    else if (i % tablesize[0] > 0  // 不是左边界i > 0 &&
                        && !thisFacies.Equals(canvesGrid.Rows[i - 1]["facies"].ToString()))// 左侧点比较
                    {
                        isSandBorder = true;
                    }
                    else if (i + 1 < canvesGrid.Rows.Count && (i + 1) % tablesize[0] > 0 // 不是右边界 && i + 1 > canvesGrid.Rows.Count
                        && !thisFacies.Equals(canvesGrid.Rows[i + 1]["facies"].ToString()))//比右
                    {
                        isSandBorder = true;
                    }
                    else if (0 == i % tablesize[0] || 0 == (i + 1) % tablesize[0] || i < tablesize[0] || i > tablesize[0] * (tablesize[1] - 1))
                    {
                        isSandBorder = true;
                    }
                    foreach (DataRow faultRow in faultDt.Rows)
                    {
                        if ((prtXCount + 1 + "").Equals(faultRow["i"].ToString())
                         && (prtYCount + 1 + "").Equals(faultRow["j"].ToString()))
                        {
                            isSandBorder = false;
                        }
                    }
                    if (isSandBorder)
                    { sandBorderPoint.Add(thisPoint); }
                    //}
                }
            }
            Console.WriteLine(sandBorderPoint.Count);
            foreach (DataRow canvesRow in canvesGrid.Rows)
            {
                string tmpStr = canvesRow["facies"].ToString();
                // Console.WriteLine(tmpStr);
                int onefacies = Convert.ToInt32(tmpStr);
                if (onefacies > 0)
                {
                    double x3 = (double)canvesRow["x3"];
                    double y3 = (double)canvesRow["y3"];
                    foreach (Point oneSandBorder in sandBorderPoint)
                    {
                        if (x3 <= oneSandBorder.X + textSandD
                        && x3 >= oneSandBorder.X - textSandD
                        && y3 <= oneSandBorder.Y + textSandD
                        && y3 >= oneSandBorder.Y - textSandD)
                        {
                            bool inWellCtrlFlag = false;
                            foreach (List<Point> oneWellCtrl in wellCtrlList)
                            {
                                Point point0 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                                if (ReservorDraw.isInRegion(point0, oneWellCtrl))
                                {
                                    inWellCtrlFlag = true;
                                    break;
                                }
                            }

                            if (!inWellCtrlFlag)//非井控
                            {
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;//00000000000000
                                Point p0 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                                Point p1 = new Point((double)canvesRow["x1"], (double)canvesRow["y1"]);
                                Point p2 = new Point((double)canvesRow["x2"], (double)canvesRow["y2"]);
                                Point p3 = new Point((double)canvesRow["x3"], (double)canvesRow["y3"]);
                                Point rawp1 = new Point((double)canvesRow["rawx0"], (double)canvesRow["rawy0"]);
                                Point rawp2 = new Point((double)canvesRow["rawx1"], (double)canvesRow["rawy1"]);
                                Point rawp3 = new Point((double)canvesRow["rawx2"], (double)canvesRow["rawy2"]);
                                Point rawp4 = new Point((double)canvesRow["rawx3"], (double)canvesRow["rawy3"]);
                                myPointCollection2.Add(p0);
                                myPointCollection2.Add(p1);
                                myPointCollection2.Add(p3);
                                myPointCollection2.Add(p2);
                                myPolygon2.Points = myPointCollection2;
                                myPolygon2.Fill = Brushes.SpringGreen;// DarkOrange;
                                if (null == allCh)
                                {
                                    this.canvesprt.Children.Add(myPolygon2);
                                }
                                List<Point> points = new List<Point>();
                                points.Add(rawp1);
                                points.Add(rawp2);
                                points.Add(rawp4);
                                points.Add(rawp3);
                                points.Add(rawp1);
                                s = Math.Abs(points.Take(points.Count - 1)
                                    .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                    .Sum() / 2 / 1000000);//据说计算面积
                                h = (double)canvesRow["dz"];
                                if (h > 0)
                                {
                                    resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)canvesRow["poro"], (double)canvesRow["barsa"], ro, b0);
                                }
                            }
                            break;//该点已画完了 找下一个点
                        }
                    }
                }
            }
            if (resSum > 0)
            {
                this.ch_res_all.Text = "砂体边部储量" + resSum.ToString("0.0000") + "万吨";
                tmpVol4EachRes = resSum.ToString("0.0000");
            }
            else
            {
                tmpVol4EachRes = "0";
            }
        }

        private void expImg(object sender, RoutedEventArgs e)
        {
            for (int cc = 0; cc < canvasList.Count; cc++)
            {
                int Height = (int)canvasList[cc].ActualHeight;
                int Width = (int)canvasList[cc].ActualWidth;
                RenderTargetBitmap bmp = new RenderTargetBitmap(Width, Height, 96, 96, PixelFormats.Pbgra32);
                bmp.Render(canvasList[cc]);
                DateTime dt = DateTime.Now;
                string filetime = dt.ToString();
                string file = @"c:\img_" + cc + filetime.Replace(":", "").Replace("/", "") + ".jpg";
                string Extension = System.IO.Path.GetExtension(file).ToLower();
                BitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                using (Stream stm = File.Create(file))
                {
                    encoder.Save(stm);
                }
            }
        }
        List<Canvas> canvasList = new List<Canvas>();
        private void newWin(object sender, RoutedEventArgs e)
        {
            DrawContainer draw = new DrawContainer("沉á积y相à带?图?");

            //  draw.canvesprt1 = this.canvesprt;

            var childrenList = this.canvesprt.Children.Cast<UIElement>().ToArray();
            draw.canvesprt1.Children.Clear();
            foreach (var c in childrenList)
            {
                this.canvesprt.Children.Remove(c);
                draw.canvesprt1.Children.Add(c);
            }
            /*
            draw.canvesprt1.Children.Clear();
            foreach (UIElement child in this.canvesprt.Children)
            {
                var xaml = System.Windows.Markup.XamlWriter.Save(child);
                var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                draw.canvesprt1.Children.Add(deepCopy);
            }
             *              * */
            draw.Show();
            canvasList.Add(draw.canvesprt1);

            //canves2.Children.Clear();
            /*
             foreach (var item in this.canvesprt.Children)
               {
                   draw.canvesprt1.Children.Add((UIElement)item);
               }
              */
            // canves2.Children.Clear();
        }
        /*
        public static T Clone<T>(T source)
        {
            var objXaml = XamlWriter.Save(source);
            var stringReader = new StringReader(objXaml);
            var xmlReader = XmlReader.Create(stringReader);
            var t = (T)XamlReader.Load(xmlReader);
            return t;
        }*/
        //注采完善自动判断 2016-8-27
        private void findInjProdPerfect(object sender, RoutedEventArgs e)
        {
            double a = 0;
            double b = 0;
            try
            {
                String textProja = this.textProja.Text;
                if (textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                String textInjb = this.textInjb.Text;
                if (textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
            }
            catch
            {
                System.Console.WriteLine("ab数字格式错误");
            }
            List<Point> perfectProd = new List<Point>();
            List<Point> perfectInj = new List<Point>();
            foreach (DataRow row1 in canvesGrid.Rows)
            {
                Ellipse wellborder = new Ellipse();
                wellborder.Stroke = System.Windows.Media.Brushes.Black;
                wellborder.StrokeThickness = 0.2;
                if ("OIL".Equals(row1["stat"]))
                {
                    wellborder.Width = 2 * a;
                    wellborder.Height = 2 * a;
                    foreach (DataRow row2 in canvesGrid.Rows)
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            if (pointD <= a || pointD <= b)
                            {
                                perfectProd.Add(pordp);
                                perfectInj.Add(injp);
                            }
                        }
                    }
                }
                else if ("WATER".Equals(row1["stat"]))
                {
                    wellborder.Width = 2 * b;
                    wellborder.Height = 2 * b;
                }
                Canvas.SetLeft(wellborder, (double)row1["x0"] - wellborder.Width / 2 + 3 / 2);// + wellpoint.Width / 2后是在多边形中心画
                Canvas.SetTop(wellborder, (double)row1["y0"] - wellborder.Height / 2 + 3 / 2);
                this.canvesprt.Children.Add(wellborder);
            }
            /*for (int i = 0; i < wellborders.Length; i++)
            {
                if (wellborders[i] != null)
                {
                    //canvesptr.Children.Add(wellpoints[i]);
                    this.canvesprt.Children.Add(wellborders[i]);
                    //canvesptr.Children.Add(TextNames[i]);
                }
            }*/
            double h = 0;
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            try
            {
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
                foreach (DataRow canvesRow in canvesGrid.Rows)
                {
                    Point p1 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                    Point p2 = new Point((double)canvesRow["x1"], (double)canvesRow["y1"]);
                    Point p3 = new Point((double)canvesRow["x2"], (double)canvesRow["y2"]);
                    Point p4 = new Point((double)canvesRow["x3"], (double)canvesRow["y3"]);
                    Point rawp1 = new Point((double)canvesRow["rawx0"], (double)canvesRow["rawy0"]);
                    Point rawp2 = new Point((double)canvesRow["rawx1"], (double)canvesRow["rawy1"]);
                    Point rawp3 = new Point((double)canvesRow["rawx2"], (double)canvesRow["rawy2"]);
                    Point rawp4 = new Point((double)canvesRow["rawx3"], (double)canvesRow["rawy3"]);
                    bool pointIn = false;
                    foreach (Point wellPoint in perfectProd)
                    {
                        if (pointl(p1, wellPoint) <= a
                            || pointl(p2, wellPoint) <= a
                            || pointl(p3, wellPoint) <= a
                            || pointl(p4, wellPoint) <= a)
                        {
                            pointIn = true;
                            break;
                        }
                    }
                    if (!pointIn)
                    {
                        foreach (Point wellPoint in perfectInj)
                        {
                            if (pointl(p1, wellPoint) <= b
                                || pointl(p2, wellPoint) <= b
                                || pointl(p3, wellPoint) <= b
                                || pointl(p4, wellPoint) <= b)
                            {
                                pointIn = true;
                                break;
                            }
                        }
                    }
                    if (pointIn)
                    {
                        List<Point> points = new List<Point>();
                        points.Add(rawp1);
                        points.Add(rawp2);
                        points.Add(rawp4);
                        points.Add(rawp3);
                        points.Add(rawp1);
                        s = Math.Abs(points.Take(points.Count - 1)
                          .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                          .Sum() / 2 / 1000000);//据说计算面积
                        h = (double)canvesRow["dz"];
                        if (h > 0)
                            resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)canvesRow["poro"], (double)canvesRow["barsa"], ro, b0);
                    }
                }
                res_result.Text = resSum.ToString("0.0000") + "万吨";
            }
            catch
            {
                System.Console.WriteLine("格式错误");
            }
        }
        ///井控范围
        ///
        List<List<Point>> wellCtrlList = new List<List<Point>>();//存储排过序的井控范围点
        private void drawWellCtrl(object sender, RoutedEventArgs e)
        {
            double a = 0;
            double b = 0;
            double maxWelldistance = 0;

            //画井点的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();

            try
            {
                String textProja = this.textProja.Text;
                if (textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                String textInjb = this.textInjb.Text;
                if (textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
                String textMaxDistance = "10000";//文学定义10公里 是粗是细 this.maxWellDistance.Text;因为凸包不算了
                if (textMaxDistance != null && !"".Equals(textMaxDistance))
                {
                    maxWelldistance = Convert.ToDouble(textMaxDistance) * m_d_zoomfactor2;
                }
            }
            catch
            {
                System.Console.WriteLine("ab数字格式错误");
            }
            if (a > 0 && b > 0)
            {
                //取出所有井号信息
                List<JhCluster> allJh = new List<JhCluster>();
                foreach (DataRow row1 in canvesGrid.Rows)
                {
                    if ("OIL".Equals(row1["stat"]) || "WATER".Equals(row1["stat"]))
                    {
                        JhCluster jhCluster = new JhCluster();
                        jhCluster.pjh = (string)row1["jh"];
                        jhCluster.px = (double)row1["x0"];
                        jhCluster.py = (double)row1["y0"];
                        jhCluster.pstatus = (string)row1["stat"];
                        allJh.Add(jhCluster);
                    }
                }
                //依据井距maxWelldistance分聚类
                List<List<JhCluster>> convexCluster = new List<List<JhCluster>>();
                while (allJh.Count > 0)
                {
                    //榨干这个类
                    List<JhCluster> tmpjhlist1 = new List<JhCluster>();
                    tmpjhlist1.Add(allJh[0]);
                    allJh.Remove(allJh[0]);
                    int compareNum = 0;
                    //聚类递归算法
                    clusterWellCtrl(compareNum, allJh, tmpjhlist1, maxWelldistance);
                    convexCluster.Add(tmpjhlist1);
                }
                //队不同聚类 算凹包
                foreach (List<JhCluster> jhClusterList in convexCluster)
                {
                    List<Point> nodes = new List<Point>();//凸包点集
                    //油水井四点扩展
                    foreach (JhCluster jhCluster in jhClusterList)
                    {
                        if ("OIL".Equals(jhCluster.pstatus))
                        {
                            nodes.Add(new Point(jhCluster.px, jhCluster.py + a));
                            nodes.Add(new Point(jhCluster.px - a, jhCluster.py));
                            nodes.Add(new Point(jhCluster.px, jhCluster.py - a));
                            nodes.Add(new Point(jhCluster.px + a, jhCluster.py));
                        }
                        else if ("WATER".Equals(jhCluster.pstatus))
                        {
                            nodes.Add(new Point(jhCluster.px, jhCluster.py + b));
                            nodes.Add(new Point(jhCluster.px - b, jhCluster.py));
                            nodes.Add(new Point(jhCluster.px, jhCluster.py - b));
                            nodes.Add(new Point(jhCluster.px + b, jhCluster.py));
                        }
                    }
                    //Console.WriteLine(nodes.Count);
                    /*foreach (Point kk in nodes)
                    {
                        Console.WriteLine(kk.X + ":" + kk.Y);
                    }*/
                    //算凸包
                    ConvexAogrithm ca = new ConvexAogrithm(nodes);
                    Point p;
                    ca.GetNodesByAngle(out p);
                    Stack<Point> p_nodes = ca.SortedNodes;
                    //Console.WriteLine("凸包结束");
                    PointCollection pointCollection = new PointCollection();
                    foreach (Point point in p_nodes)//newOrderPoint remainPoint
                    {
                        pointCollection.Add(point);
                    }
                    //圈选范围
                    List<Point> pointList = new List<Point>();
                    foreach (Point point in p_nodes)//newOrderPoint remainPoint
                    {
                        pointList.Add(point);
                    }
                    wellCtrlList.Add(pointList);
                    if (drawWellCtrlFlag)
                    {
                        foreach (DataRow row1 in canvesGrid.Rows)
                        {
                            double soil = (double)row1["barsa"];
                            Point point0 = new Point((double)row1["x0"], (double)row1["y0"]);
                            //井控范围
                            if (soil > 0 && ReservorDraw.isInRegion(point0, pointList))
                            {
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                myPointCollection2.Add(new System.Windows.Point((double)row1["x0"], (double)row1["y0"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row1["x1"], (double)row1["y1"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row1["x3"], (double)row1["y3"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row1["x2"], (double)row1["y2"]));
                                //myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;
                                SolidColorBrush myBrush = new SolidColorBrush(Colors.RoyalBlue);
                                myPolygon2.Fill = myBrush;
                                myPolygon2.Points = myPointCollection2;
                                this.canvesprt.Children.Add(myPolygon2);
                            }
                        }
                    }
                }

                /* Canvas.SetLeft(wellborder, (double)row1["x0"] - wellborder.Width / 2 + 3 / 2);// + wellpoint.Width / 2后是在多边形中心画
                Canvas.SetTop(wellborder, (double)row1["y0"] - wellborder.Height / 2 + 3 / 2);
                this.canvesprt.Children.Add(wellborder);*/

                /*Polygon polygon = new Polygon();
                polygon.Stroke = System.Windows.Media.Brushes.Black;
                polygon.StrokeThickness = 1;
                polygon.Points = pointCollection;
                this.canvesprt.Children.Add(polygon);*/

                foreach (DataRow row1 in canvesGrid.Rows)
                {
                    //画井点
                    int ellipseWidth = 3;
                    string jh = row1["jh"].ToString();
                    if (jh != null && !"".Equals(jh))
                    {
                        Ellipse wellpoint = new Ellipse();
                        //Ellipse wellborder = new Ellipse();
                        wellpoint.Width = ellipseWidth;
                        wellpoint.Height = ellipseWidth;
                        // wellborder.Stroke = System.Windows.Media.Brushes.Black;
                        // wellborder.StrokeThickness = 0.2;
                        wellpoint.Stroke = System.Windows.Media.Brushes.Black;
                        wellpoint.StrokeThickness = 0.1;
                        //井别
                        string stat = row1["stat"].ToString();
                        switch (stat)
                        {
                            case "OIL":
                                wellpoint.Fill = System.Windows.Media.Brushes.Red;
                                //wellborder.Width = a;
                                //wellborder.Height = a;
                                break;
                            default:
                                wellpoint.Fill = System.Windows.Media.Brushes.Green;
                                // wellborder.Width = b;
                                // wellborder.Height = b;
                                break;
                        }

                        Canvas.SetLeft(wellpoint, (double)row1["x0"]);//- wellpoint.Width / 2后是在x0点画
                        Canvas.SetTop(wellpoint, (double)row1["y0"]);
                        //this.canvesprt.Children.Add(wellpoint);
                        if (null == allCh)
                        {
                            wellPoints.Add(wellpoint);
                            // Canvas.SetLeft(wellborder, (double)row1["x0"] - wellborder.Width / 2 + wellpoint.Width / 2);// + wellpoint.Width / 2后是在多边形中心画
                            // Canvas.SetTop(wellborder, (double)row1["y0"] - wellborder.Height / 2 + wellpoint.Height / 2);

                            // wellBorders.Add(wellborder);

                            TextBlock t1 = new TextBlock();
                            t1.Text = jh;
                            t1.FontSize = 6;//井号字体
                            Canvas.SetLeft(t1, (double)row1["x0"]);
                            Canvas.SetTop(t1, (double)row1["y0"]);
                            // this.canvesprt.Children.Add(t1);
                            wellNames.Add(t1);
                        }
                    }

                }
                for (int i = 0; i < wellPoints.Count; i++)
                {
                    if (wellPoints[i] != null)
                    {
                        this.canvesprt.Children.Add(wellPoints[i]);
                        //canvesptr.Children.Add(wellborders[i]);
                        this.canvesprt.Children.Add(wellNames[i]);
                    }
                }
            }
        }

        class JhCluster   //声明类
        {
            string jh;
            double x;
            double y;
            string status;
            public string pjh
            {
                get { return jh; }
                set { jh = value; }
            }
            public double px
            {
                get { return x; }
                set { x = value; }
            }
            public double py
            {
                get { return y; }
                set { y = value; }
            }
            public string pstatus
            {
                get { return status; }
                set { status = value; }
            }
        }
        //聚类递归
        private void clusterWellCtrl(int compareNum, List<JhCluster> allJh, List<JhCluster> tmpjhlist1, double maxWelldistance)
        {
            List<JhCluster> allJhTmp = allJh;
            for (int clusterCount = 0; clusterCount < allJhTmp.Count; clusterCount++)//Collection was modified; enumeration operation may not execute.
            {
                if (pointl(new Point(tmpjhlist1[compareNum].px, tmpjhlist1[compareNum].py), new Point(allJhTmp[clusterCount].px, allJhTmp[clusterCount].py)) < maxWelldistance)
                {
                    //同一个聚类
                    tmpjhlist1.Add(allJhTmp[clusterCount]);
                    allJh.Remove(allJhTmp[clusterCount]);
                }
            }
            compareNum++;
            if (compareNum < tmpjhlist1.Count)//剩余未遍历的list
            {
                clusterWellCtrl(compareNum, allJh, tmpjhlist1, maxWelldistance);
            }
        }

        private void drawOilWithoutInj(object sender, RoutedEventArgs e)
        {
            double wlength = 450 * m_d_zoomfactor2;
            double h = 0;
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            double a = 0;
            double b = 0;
            List<Point> perfectPoints = new List<Point>();
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
            }
            catch
            {
                System.Console.WriteLine("数字格式错误");
            }
            try
            {
                if (this.withOutIP != null && !"".Equals(withOutIP))
                {
                    wlength = Convert.ToDouble(this.withOutIP.Text) * m_d_zoomfactor2;
                }
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch
            {
                System.Console.WriteLine("数字格式错误");
            }
            //画井点
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();

            //不在注采完善区
            bool isInPerfect = false;
            foreach (DataRow rowPerfect in canvesGrid.Rows)
            {
                if ("OIL".Equals(rowPerfect["stat"]))
                {
                    bool hasPerfectInj = false;
                    Point pordPerfectp = new Point((double)rowPerfect["x0"], (double)rowPerfect["y0"]);
                    foreach (DataRow row2 in canvesGrid.Rows)
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointPerfectD = pointl(pordPerfectp, injp);
                            if (pointPerfectD <= a || pointPerfectD <= b)
                            {
                                hasPerfectInj = true;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    if (soil > 0 && pointI0D <= b)
                                    {
                                        perfectPoints.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                    }
                                }
                                /*double point02inj = pointl(point0, injp);
                                double point02Prod = pointl(point0, pordPerfectp);
                                if (point02Prod <= a || point02inj <= b)
                                {
                                    isInPerfect = true;
                                    break;
                                }*/
                            }
                        }
                    }
                    if (hasPerfectInj)//画油井圆
                    {
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordPerfectp, point0);
                            if (soil > 0 && pointD <= a)
                            {
                                perfectPoints.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                            }
                        }
                    }
                }
            }
            //
            foreach (DataRow row1 in canvesGrid.Rows)
            {
                if ("OIL".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    foreach (DataRow row2 in canvesGrid.Rows)
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            if (pointD <= wlength)
                            {
                                hasInj = true;
                            }
                        }
                    }
                    if (!hasInj)
                    {
                        /* Ellipse wellborder = new Ellipse();
                         wellborder.Stroke = System.Windows.Media.Brushes.Black;
                         wellborder.StrokeThickness = 0.2;
                         wellborder.Width = 2 * wlength;
                         wellborder.Height = 2 * wlength;
                         Canvas.SetLeft(wellborder, (double)row1["x0"] - wellborder.Width / 2 + 3 / 2);// + wellpoint.Width / 2后是在多边形中心画
                         Canvas.SetTop(wellborder, (double)row1["y0"] - wellborder.Height / 2 + 3 / 2);
                         this.canvesprt.Children.Add(wellborder);*/
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            if (soil > 0 && pointD <= wlength)
                            {
                                //不在注采完善区
                                foreach (Point onePerfect in perfectPoints)
                                {
                                    if (onePerfect == point0)
                                    {
                                        isInPerfect = true;
                                        break;
                                    }
                                }
                                //
                                if (!isInPerfect)
                                {
                                    Polygon myPolygon2 = new Polygon();
                                    PointCollection myPointCollection2 = new PointCollection();
                                    myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                    myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                    myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                    myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                    //myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                    myPolygon2.StrokeThickness = 0.00;
                                    SolidColorBrush myBrush = new SolidColorBrush(Colors.Sienna);//Tomato
                                    myPolygon2.Fill = myBrush;
                                    myPolygon2.Points = myPointCollection2;
                                    if (null == allCh)
                                    {
                                        this.canvesprt.Children.Add(myPolygon2);
                                    }
                                    //储量
                                    Point rawp1 = new Point((double)row3["rawx0"], (double)row3["rawy0"]);
                                    Point rawp2 = new Point((double)row3["rawx1"], (double)row3["rawy1"]);
                                    Point rawp3 = new Point((double)row3["rawx2"], (double)row3["rawy2"]);
                                    Point rawp4 = new Point((double)row3["rawx3"], (double)row3["rawy3"]);
                                    List<Point> points = new List<Point>();
                                    points.Add(rawp1);
                                    points.Add(rawp2);
                                    points.Add(rawp4);
                                    points.Add(rawp3);
                                    points.Add(rawp1);
                                    s = Math.Abs(points.Take(points.Count - 1)
                                       .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                       .Sum() / 2 / 1000000);//据说计算面积
                                    h = (double)row3["dz"];
                                    if (h > 0)
                                    {
                                        resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)row3["poro"], (double)row3["barsa"], ro, b0);
                                    }
                                }
                            }
                            isInPerfect = false;
                        }
                    }

                }
                /*else if ("WATER".Equals(row1["stat"]))
                {
                    wellborder.Width = 2 * wlength;
                    wellborder.Height = 2 * wlength;
                }*/
                //画井点
                if (null == allCh)
                {
                    drawWells(row1, wellPoints, wellNames);
                }
            }
            if (resSum > 0)
            {
                this.ch_res_all.Text = "有采无注储量" + resSum.ToString("0.0000") + "万吨";
                tmpVol4EachRes = resSum.ToString("0.0000");
            }
            else tmpVol4EachRes = "0";
            //画井点
            if (null == allCh)
            {
                for (int i = 0; i < wellPoints.Count; i++)
                {
                    if (wellPoints[i] != null)
                    {
                        this.canvesprt.Children.Add(wellPoints[i]);
                        this.canvesprt.Children.Add(wellNames[i]);
                    }
                }
            }
        }

        private void drawInjWithoutOil(object sender, RoutedEventArgs e)
        {
            double wlength = 450 * m_d_zoomfactor2;
            double h = 0;
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            double a = 0;
            double b = 0;
            List<Point> perfectPoints = new List<Point>();
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
            }
            catch
            {
                System.Console.WriteLine("数字格式错误");
            }
            try
            {
                if (this.withOutIP != null && !"".Equals(withOutIP))
                {
                    wlength = Convert.ToDouble(this.withOutIP.Text) * m_d_zoomfactor2;
                }
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch
            {
                System.Console.WriteLine("数字格式错误");
            }
            //不在注采完善区
            bool isInPerfect = false;
            foreach (DataRow rowPerfect in canvesGrid.Rows)
            {
                if ("OIL".Equals(rowPerfect["stat"]))
                {
                    bool hasPerfectInj = false;
                    Point pordPerfectp = new Point((double)rowPerfect["x0"], (double)rowPerfect["y0"]);
                    foreach (DataRow row2 in canvesGrid.Rows)
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointPerfectD = pointl(pordPerfectp, injp);
                            if (pointPerfectD <= a || pointPerfectD <= b)
                            {
                                hasPerfectInj = true;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    if (soil > 0 && pointI0D <= b)
                                    {
                                        perfectPoints.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                    }
                                }
                                /*double point02inj = pointl(point0, injp);
                                double point02Prod = pointl(point0, pordPerfectp);
                                if (point02Prod <= a || point02inj <= b)
                                {
                                    isInPerfect = true;
                                    break;
                                }*/
                            }
                        }
                    }
                    if (hasPerfectInj)//画油井圆
                    {
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordPerfectp, point0);
                            if (soil > 0 && pointD <= a)
                            {
                                perfectPoints.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                            }
                        }
                    }
                }
            }
            //
            //画井点
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();

            foreach (DataRow row1 in canvesGrid.Rows)
            {
                if ("WATER".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    foreach (DataRow row2 in canvesGrid.Rows)
                    {
                        if ("OIL".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            if (pointD <= wlength)
                            {
                                hasInj = true;
                            }
                        }
                    }
                    if (!hasInj)
                    {
                        /* Ellipse wellborder = new Ellipse();
                         wellborder.Stroke = System.Windows.Media.Brushes.Black;
                         wellborder.StrokeThickness = 0.2;
                         wellborder.Width = 2 * wlength;
                         wellborder.Height = 2 * wlength;
                         Canvas.SetLeft(wellborder, (double)row1["x0"] - wellborder.Width / 2 + 3 / 2);// + wellpoint.Width / 2后是在多边形中心画
                         Canvas.SetTop(wellborder, (double)row1["y0"] - wellborder.Height / 2 + 3 / 2);
                         this.canvesprt.Children.Add(wellborder);*/
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            if (soil > 0 && pointD <= wlength)
                            {
                                //不在注采完善区
                                foreach (Point onePerfect in perfectPoints)
                                {
                                    if (onePerfect == point0)
                                    {
                                        isInPerfect = true;
                                        break;
                                    }
                                }
                                //
                                if (!isInPerfect)
                                {
                                    Polygon myPolygon2 = new Polygon();
                                    PointCollection myPointCollection2 = new PointCollection();
                                    myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                    myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                    myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                    myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                    // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                    myPolygon2.StrokeThickness = 0.00;
                                    SolidColorBrush myBrush = new SolidColorBrush(Colors.Tomato);//Sienna
                                    myPolygon2.Fill = myBrush;
                                    myPolygon2.Points = myPointCollection2;
                                    if (null == allCh)
                                    {
                                        this.canvesprt.Children.Add(myPolygon2);
                                    }

                                    //算储量
                                    Point rawp1 = new Point((double)row3["rawx0"], (double)row3["rawy0"]);
                                    Point rawp2 = new Point((double)row3["rawx1"], (double)row3["rawy1"]);
                                    Point rawp3 = new Point((double)row3["rawx2"], (double)row3["rawy2"]);
                                    Point rawp4 = new Point((double)row3["rawx3"], (double)row3["rawy3"]);
                                    List<Point> points = new List<Point>();
                                    points.Add(rawp1);
                                    points.Add(rawp2);
                                    points.Add(rawp4);
                                    points.Add(rawp3);
                                    points.Add(rawp1);
                                    s = Math.Abs(points.Take(points.Count - 1)
                                        .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                        .Sum() / 2 / 1000000);//据说计算面积
                                    h = (double)row3["dz"];
                                    if (h > 0)
                                    {
                                        resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)row3["poro"], (double)row3["barsa"], ro, b0);
                                    }
                                }
                            }
                            isInPerfect = false;
                        }
                    }

                }
                /*else if ("WATER".Equals(row1["stat"]))
                {
                    wellborder.Width = 2 * wlength;
                    wellborder.Height = 2 * wlength;
                }*/
                //画井点
                if (null == allCh)
                {
                    drawWells(row1, wellPoints, wellNames);
                }
            }
            if (resSum > 0)
            {
                this.ch_res_all.Text = "有注无采储量" + resSum.ToString("0.0000") + "万吨";
                tmpVol4EachRes = resSum.ToString("0.0000");
            }
            else tmpVol4EachRes = "0";
            //画井点
            if (null == allCh)
            {
                for (int i = 0; i < wellPoints.Count; i++)
                {
                    if (wellPoints[i] != null)
                    {
                        this.canvesprt.Children.Add(wellPoints[i]);
                        this.canvesprt.Children.Add(wellNames[i]);
                    }
                }
            }
        }
        //注采完善
        private void drawInjProdPerfect(object sender, RoutedEventArgs e)
        {
            double a = 0;
            double b = 0;
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
            }
            catch
            {
                System.Console.WriteLine("数字格式错误");
            }
            //画井点
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            foreach (DataRow row1 in canvesGrid.Rows)
            {
                if ("OIL".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    foreach (DataRow row2 in canvesGrid.Rows)
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            if (pointD <= a || pointD <= b)
                            {
                                hasInj = true;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    if (soil > 0 && pointI0D <= b)
                                    {
                                        Polygon myPolygon2 = new Polygon();
                                        PointCollection myPointCollection2 = new PointCollection();
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                        // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                        myPolygon2.StrokeThickness = 0.00;
                                        SolidColorBrush myBrush = new SolidColorBrush(Colors.Violet);//.PaleGoldenrod);
                                        myPolygon2.Fill = myBrush;
                                        myPolygon2.Points = myPointCollection2;
                                        this.canvesprt.Children.Add(myPolygon2);
                                    }
                                }
                            }
                        }
                    }
                    if (hasInj)//画油井圆
                    {
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            if (soil > 0 && pointD <= a)
                            {
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;
                                SolidColorBrush myBrush = new SolidColorBrush(Colors.Violet);//.PaleGoldenrod);
                                myPolygon2.Fill = myBrush;
                                myPolygon2.Points = myPointCollection2;
                                this.canvesprt.Children.Add(myPolygon2);
                            }
                        }
                    }
                }

                //画井点
                drawWells(row1, wellPoints, wellNames);
            }
            //画井点

            for (int i = 0; i < wellPoints.Count; i++)
            {
                if (wellPoints[i] != null)
                {
                    this.canvesprt.Children.Add(wellPoints[i]);
                    this.canvesprt.Children.Add(wellNames[i]);
                }
            }
        }
        /// <summary>
        /// 主力油层
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawMainOil(object sender, RoutedEventArgs e)
        {
            //地层均质系数
            double obFactorD = 0;
            double notObFactorD = 0;
            double a = 0;
            double b = 0;
            //bool isInPerfect = false;
            //画井点用的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            if (perfectPoints == null || perfectPoints.Count == 0)
            {
                //getPerfectPoints();
            }
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
            }
            catch { Console.WriteLine("格式错误"); }
            try
            {
                obFactorD = Convert.ToDouble(this.obFactor.Text);
                notObFactorD = Convert.ToDouble(this.notObFactor.Text);
            }
            catch { Console.WriteLine("格式错误"); }

            //double maxpermx = (double)canvesGrid.Compute("max(permx)", "");
            //Console.WriteLine(maxpermx);
            foreach (DataRow row1 in canvesGrid.Rows)
            {
                if ("OIL".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    double dz = Convert.ToDouble(row1["dz"]);
                    double permx = Convert.ToDouble(row1["permx"]);
                    String jh = row1["jh"].ToString();
                    String ch = row1["ch"].ToString();
                    DataRow[] jhRows = wellCoordTrueEnd.Select("jh = '" + jh + "'");
                    List<string> chList = new List<string>();
                    List<double> dicengXishu = new List<double>();
                    double yxhd = -1;
                    double maxDicengXishu = -1;
                    double avgDicengXishu = -1;
                    double sumDicengXishu = 0;
                    foreach (DataRow wellRow in jhRows)
                    {
                        if (!chList.Contains(wellRow["z"].ToString()))
                        {
                            chList.Add(wellRow["z"].ToString());
                        }
                        dicengXishu.Add(Convert.ToDouble(wellRow["地层系数"]));
                        if (ch.Equals(wellRow["z"].ToString()))
                        {
                            yxhd = Convert.ToDouble(wellRow["地层系数"]) / permx;// 计算有效厚度 
                            // Console.WriteLine(dz + "=" + yxhd + "=" + permx + "=" + Convert.ToDouble(wellRow["地层系数"]));
                        }

                    }

                    int chCount = chList.Count;
                    foreach (double onedicengxishu in dicengXishu)
                    {
                        if (onedicengxishu > maxDicengXishu)
                        {
                            maxDicengXishu = onedicengxishu;
                        }
                        sumDicengXishu += onedicengxishu;
                    }
                    avgDicengXishu = sumDicengXishu / dicengXishu.Count;
                    foreach (DataRow row2 in canvesGrid.Rows)// 扫描水井 
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            if ((pointD <= a || pointD <= b) // 注采完善
                            && (
                            yxhd >= 1//射开有效厚度大于1m的层;
                                     //|| (chCount <= 4 && avgDicengXishu / maxDicengXishu < obFactorD)
                                     // || (chCount >= 5 && avgDicengXishu / maxDicengXishu < notObFactorD)
                            ))
                            {
                                hasInj = true;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    if (soil > 0 && pointI0D <= b)
                                    {
                                        Polygon myPolygon2 = new Polygon();
                                        PointCollection myPointCollection2 = new PointCollection();
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                        // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                        myPolygon2.StrokeThickness = 0.00;
                                        SolidColorBrush myBrush = new SolidColorBrush(Colors.OliveDrab);//.PaleGoldenrod);
                                        myPolygon2.Fill = myBrush;
                                        myPolygon2.Points = myPointCollection2;
                                        this.canvesprt.Children.Add(myPolygon2);
                                    }
                                }
                            }
                        }
                    }
                    if (hasInj)//画油井圆
                    {
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            if (soil > 0 && pointD <= a)
                            {
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;
                                SolidColorBrush myBrush = new SolidColorBrush(Colors.OliveDrab);//.PaleGoldenrod);
                                myPolygon2.Fill = myBrush;
                                myPolygon2.Points = myPointCollection2;
                                this.canvesprt.Children.Add(myPolygon2);
                            }
                        }
                    }
                }
                //画井点
                drawWells(row1, wellPoints, wellNames);
            }
            //画井点
            for (int i = 0; i < wellPoints.Count; i++)
            {
                if (wellPoints[i] != null)
                {
                    this.canvesprt.Children.Add(wellPoints[i]);
                    this.canvesprt.Children.Add(wellNames[i]);
                }
            }
        }
        /// <summary>
        /// 注采完善明显
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawObvious(object sender, RoutedEventArgs e)
        {
            //地层均质系数
            double obFactorD = 0;
            double notObFactorD = 0;
            double a = 0;
            double b = 0;
            //bool isInPerfect = false;
            //画井点用的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            if (perfectPoints == null || perfectPoints.Count == 0)
            {
                //getPerfectPoints();
            }
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
            }
            catch { Console.WriteLine("格式错误"); }
            try
            {
                obFactorD = Convert.ToDouble(this.obFactor.Text);
                notObFactorD = Convert.ToDouble(this.notObFactor.Text);
            }
            catch { Console.WriteLine("格式错误"); }

            //double maxpermx = (double)canvesGrid.Compute("max(permx)", "");
            //Console.WriteLine(maxpermx);
            foreach (DataRow row1 in canvesGrid.Rows)
            {
                if ("OIL".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    double dz = Convert.ToDouble(row1["dz"]);
                    double permx = Convert.ToDouble(row1["permx"]);
                    String jh = row1["jh"].ToString();
                    String ch = row1["ch"].ToString();
                    DataRow[] jhRows = wellCoordTrueEnd.Select("jh = '" + jh + "'");
                    List<string> chList = new List<string>();
                    List<double> dicengXishu = new List<double>();
                    double yxhd = -1;
                    double maxDicengXishu = -1;
                    double avgDicengXishu = -1;
                    double sumDicengXishu = 0;
                    foreach (DataRow wellRow in jhRows)
                    {
                        if (!chList.Contains(wellRow["z"].ToString()))
                        {
                            chList.Add(wellRow["z"].ToString());
                        }
                        dicengXishu.Add(Convert.ToDouble(wellRow["地层系数"]));
                        if (ch.Equals(wellRow["z"].ToString()))
                        {
                            yxhd = Convert.ToDouble(wellRow["地层系数"]) / permx;// 计算有效厚度 
                            Console.WriteLine(dz + "=" + yxhd + "=" + permx + "=" + Convert.ToDouble(wellRow["地层系数"]));
                        }

                    }

                    int chCount = chList.Count;
                    foreach (double onedicengxishu in dicengXishu)
                    {
                        if (onedicengxishu > maxDicengXishu)
                        {
                            maxDicengXishu = onedicengxishu;
                        }
                        sumDicengXishu += onedicengxishu;
                    }
                    avgDicengXishu = sumDicengXishu / dicengXishu.Count;
                    foreach (DataRow row2 in canvesGrid.Rows)// 扫描水井 
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            if ((pointD <= a || pointD <= b) // 注采完善
                            && (
                            yxhd >= 1//射开有效厚度大于1m的层;
                            || (chCount <= 4 && avgDicengXishu / maxDicengXishu < obFactorD)
                            || (chCount >= 5 && avgDicengXishu / maxDicengXishu < notObFactorD)
                            ))
                            {
                                hasInj = true;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    if (soil > 0 && pointI0D <= b)
                                    {
                                        Polygon myPolygon2 = new Polygon();
                                        PointCollection myPointCollection2 = new PointCollection();
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                        // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                        myPolygon2.StrokeThickness = 0.00;
                                        SolidColorBrush myBrush = new SolidColorBrush(Colors.Sienna);//.PaleGoldenrod);
                                        myPolygon2.Fill = myBrush;
                                        myPolygon2.Points = myPointCollection2;
                                        this.canvesprt.Children.Add(myPolygon2);
                                    }
                                }
                            }
                        }
                    }
                    if (hasInj)//画油井圆
                    {
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            if (soil > 0 && pointD <= a)
                            {
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;
                                SolidColorBrush myBrush = new SolidColorBrush(Colors.Sienna);//.PaleGoldenrod);
                                myPolygon2.Fill = myBrush;
                                myPolygon2.Points = myPointCollection2;
                                this.canvesprt.Children.Add(myPolygon2);
                            }
                        }
                    }
                }
                //画井点
                drawWells(row1, wellPoints, wellNames);
            }
            //画井点
            for (int i = 0; i < wellPoints.Count; i++)
            {
                if (wellPoints[i] != null)
                {
                    this.canvesprt.Children.Add(wellPoints[i]);
                    this.canvesprt.Children.Add(wellNames[i]);
                }
            }
        }

        private void drawWells(DataRow row1, List<Ellipse> wellPoints, List<TextBlock> wellNames)
        {
            //画井点
            int ellipseWidth = 3;
            string jh = row1["jh"].ToString();
            if (jh != null && !"".Equals(jh))
            {
                Ellipse wellpoint = new Ellipse();
                wellpoint.Width = ellipseWidth;
                wellpoint.Height = ellipseWidth;
                wellpoint.Stroke = System.Windows.Media.Brushes.Black;
                wellpoint.StrokeThickness = 0.1;
                //井别
                string stat = row1["stat"].ToString();
                switch (stat)
                {
                    case "OIL":
                        wellpoint.Fill = System.Windows.Media.Brushes.Red;
                        break;
                    default:
                        wellpoint.Fill = System.Windows.Media.Brushes.Green;
                        break;
                }

                Canvas.SetLeft(wellpoint, (double)row1["x0"]);
                Canvas.SetTop(wellpoint, (double)row1["y0"]);
                wellPoints.Add(wellpoint);

                TextBlock t1 = new TextBlock();
                t1.Text = jh;
                t1.FontSize = 6;//井号字体
                Canvas.SetLeft(t1, (double)row1["x0"]);
                Canvas.SetTop(t1, (double)row1["y0"]);
                wellNames.Add(t1);
            }
        }

        private void drawUnobvious(object sender, RoutedEventArgs e)
        {
            //地层均质系数
            //double obFactorD = 0;
            double notObFactorD = 0;
            try
            {
                // obFactorD = Convert.ToDouble(this.obFactor.Text);
                notObFactorD = Convert.ToDouble(this.notObFactor.Text);
            }
            catch { }
            double maxpermx = (double)canvesGrid.Compute("max(permx)", "");
            Console.WriteLine(maxpermx);
            foreach (DataRow row3 in canvesGrid.Rows)
            {
                double dz = (double)row3["dz"];
                double permx = (double)row3["permx"];
                double soil = (double)row3["barsa"];
                if (dz > 5 && permx / maxpermx < notObFactorD && soil > 0)
                {
                    Polygon myPolygon2 = new Polygon();
                    PointCollection myPointCollection2 = new PointCollection();
                    myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                    myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                    myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                    myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                    //  myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                    myPolygon2.StrokeThickness = 0.00;
                    SolidColorBrush myBrush = new SolidColorBrush(Colors.Olive);
                    myPolygon2.Fill = myBrush;
                    myPolygon2.Points = myPointCollection2;
                    this.canvesprt.Children.Add(myPolygon2);
                }

            }

        }

        private void drawNonMainOil(object sender, RoutedEventArgs e)
        {
            //地层均质系数
            double obFactorD = 0;
            double notObFactorD = 0;
            double a = 0;
            double b = 0;
            //bool isInPerfect = false;
            //画井点用的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            if (perfectPoints == null || perfectPoints.Count == 0)
            {
                //getPerfectPoints();
            }
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
            }
            catch { Console.WriteLine("格式错误"); }
            try
            {
                obFactorD = Convert.ToDouble(this.obFactor.Text);
                notObFactorD = Convert.ToDouble(this.notObFactor.Text);
            }
            catch { Console.WriteLine("格式错误"); }

            //double maxpermx = (double)canvesGrid.Compute("max(permx)", "");
            //Console.WriteLine(maxpermx);
            foreach (DataRow row1 in canvesGrid.Rows)
            {
                if ("OIL".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    double dz = Convert.ToDouble(row1["dz"]);
                    double permx = Convert.ToDouble(row1["permx"]);
                    String jh = row1["jh"].ToString();
                    String ch = row1["ch"].ToString();
                    DataRow[] jhRows = wellCoordTrueEnd.Select("jh = '" + jh + "'");
                    List<string> chList = new List<string>();
                    List<double> dicengXishu = new List<double>();
                    double yxhd = -1;
                    double maxDicengXishu = -1;
                    double avgDicengXishu = -1;
                    double sumDicengXishu = 0;
                    foreach (DataRow wellRow in jhRows)
                    {
                        if (!chList.Contains(wellRow["z"].ToString()))
                        {
                            chList.Add(wellRow["z"].ToString());
                        }
                        dicengXishu.Add(Convert.ToDouble(wellRow["地层系数"]));
                        if (ch.Equals(wellRow["z"].ToString()))
                        {
                            yxhd = Convert.ToDouble(wellRow["地层系数"]) / permx;// 计算有效厚度 
                            // Console.WriteLine(dz + "=" + yxhd + "=" + permx + "=" + Convert.ToDouble(wellRow["地层系数"]));
                        }

                    }

                    int chCount = chList.Count;
                    foreach (double onedicengxishu in dicengXishu)
                    {
                        if (onedicengxishu > maxDicengXishu)
                        {
                            maxDicengXishu = onedicengxishu;
                        }
                        sumDicengXishu += onedicengxishu;
                    }
                    avgDicengXishu = sumDicengXishu / dicengXishu.Count;
                    foreach (DataRow row2 in canvesGrid.Rows)// 扫描水井 
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            if ((pointD <= a || pointD <= b) // 注采完善
                            && (
                            yxhd < 1//射开有效厚度大于1m的层;
                                    //|| (chCount <= 4 && avgDicengXishu / maxDicengXishu < obFactorD)
                                    // || (chCount >= 5 && avgDicengXishu / maxDicengXishu < notObFactorD)
                            ))
                            {
                                hasInj = true;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    if (soil > 0 && pointI0D <= b)
                                    {
                                        Polygon myPolygon2 = new Polygon();
                                        PointCollection myPointCollection2 = new PointCollection();
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                        // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                        myPolygon2.StrokeThickness = 0.00;
                                        SolidColorBrush myBrush = new SolidColorBrush(Colors.Orange);//.PaleGoldenrod);
                                        myPolygon2.Fill = myBrush;
                                        myPolygon2.Points = myPointCollection2;
                                        this.canvesprt.Children.Add(myPolygon2);
                                    }
                                }
                            }
                        }
                    }
                    if (hasInj)//画油井圆
                    {
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            if (soil > 0 && pointD <= a)
                            {
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;
                                SolidColorBrush myBrush = new SolidColorBrush(Colors.Orange);//.PaleGoldenrod);
                                myPolygon2.Fill = myBrush;
                                myPolygon2.Points = myPointCollection2;
                                this.canvesprt.Children.Add(myPolygon2);
                            }
                        }
                    }
                }
                //画井点
                drawWells(row1, wellPoints, wellNames);
            }
            //画井点
            for (int i = 0; i < wellPoints.Count; i++)
            {
                if (wellPoints[i] != null)
                {
                    this.canvesprt.Children.Add(wellPoints[i]);
                    this.canvesprt.Children.Add(wellNames[i]);
                }
            }
        }

        /// <summary>
        /// 层内干扰
        /// 注采完善中的
        ///     不明显 和 主力 并且同一相带
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void drawInLayer(object sender, RoutedEventArgs e)
        {
            //地层均质系数
            double obFactorD = 0;
            double notObFactorD = 0;
            double a = 0;
            double b = 0;
            //bool isInPerfect = false;
            //画井点用的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            double h = 0;
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            List<Point> hasColorPoints = new List<Point>();
            if (perfectPoints == null || perfectPoints.Count == 0)
            {
                //getPerfectPoints();
            }
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch { Console.WriteLine("格式错误"); }
            try
            {
                obFactorD = Convert.ToDouble(this.obFactor.Text);
                notObFactorD = Convert.ToDouble(this.notObFactor.Text);
            }
            catch { Console.WriteLine("格式错误"); }

            //double maxpermx = (double)canvesGrid.Compute("max(permx)", "");
            //Console.WriteLine(maxpermx);
            foreach (DataRow row1 in canvesGrid.Rows)//寻找油井
            {
                if ("OIL".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    string faciesProd = row1["facies"].ToString();
                    double dz = Convert.ToDouble(row1["dz"]);
                    double permx = Convert.ToDouble(row1["permx"]);
                    String jh = row1["jh"].ToString();
                    String ch = row1["ch"].ToString();
                    DataRow[] jhRows = wellCoordTrueEnd.Select("jh = '" + jh + "'");
                    List<string> chList = new List<string>();
                    List<double> dicengXishu = new List<double>();
                    double yxhd = -1;
                    double maxDicengXishu = -1;
                    double avgDicengXishu = -1;
                    double sumDicengXishu = 0;
                    foreach (DataRow wellRow in jhRows)
                    {
                        if (!chList.Contains(wellRow["z"].ToString()))
                        {
                            chList.Add(wellRow["z"].ToString());
                        }
                        dicengXishu.Add(Convert.ToDouble(wellRow["地层系数"]));
                        if (ch.Equals(wellRow["z"].ToString()))
                        {
                            yxhd = Convert.ToDouble(wellRow["地层系数"]) / permx;// 计算有效厚度 
                            // Console.WriteLine(dz + "=" + yxhd + "=" + permx + "=" + Convert.ToDouble(wellRow["地层系数"]));
                        }

                    }

                    int chCount = chList.Count;
                    foreach (double onedicengxishu in dicengXishu)
                    {
                        if (onedicengxishu > maxDicengXishu)
                        {
                            maxDicengXishu = onedicengxishu;
                        }
                        sumDicengXishu += onedicengxishu;
                    }
                    avgDicengXishu = sumDicengXishu / dicengXishu.Count;
                    foreach (DataRow row2 in canvesGrid.Rows)// 扫描水井 
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            string faciesInj2 = row2["facies"].ToString();
                            if ((pointD <= a || pointD <= b) // 注采完善
                            && (yxhd >= 1//射开有效厚度大于1m的层;
                                         //|| (chCount <= 4 && avgDicengXishu / maxDicengXishu < obFactorD)
                                         // || (chCount >= 5 && avgDicengXishu / maxDicengXishu < notObFactorD)
                            ))
                            {
                                hasInj = true;
                                //&& faciesInj2.Equals(faciesProd)//同相带
                                List<Polygon> polygonList = new List<Polygon>();
                                bool isSameFacies = true;
                                double tmpSumRes = 0;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    string faciesInjS = row3["facies"].ToString();
                                    if (soil > 0 && pointI0D <= b && !faciesInjS.Equals("0")
                                        //   && faciesInjS.Equals(faciesProd)//同相带
                                        )
                                    {
                                        if (!faciesInjS.Equals(faciesInj2))//不同相带
                                        {
                                            isSameFacies = false;
                                        }
                                        Polygon myPolygon2 = new Polygon();
                                        PointCollection myPointCollection2 = new PointCollection();
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                        // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                        myPolygon2.StrokeThickness = 0.00;
                                        SolidColorBrush myBrush = new SolidColorBrush(Colors.PaleGoldenrod);//.PaleGoldenrod);
                                        myPolygon2.Fill = myBrush;
                                        myPolygon2.Points = myPointCollection2;
                                        //this.canvesprt.Children.Add(myPolygon2);
                                        polygonList.Add(myPolygon2);
                                        //储量
                                        Point rawp1 = new Point((double)row3["rawx0"], (double)row3["rawy0"]);
                                        Point rawp2 = new Point((double)row3["rawx1"], (double)row3["rawy1"]);
                                        Point rawp3 = new Point((double)row3["rawx2"], (double)row3["rawy2"]);
                                        Point rawp4 = new Point((double)row3["rawx3"], (double)row3["rawy3"]);
                                        List<Point> points = new List<Point>();
                                        hasColorPoints.Add(rawp1);
                                        points.Add(rawp1);
                                        points.Add(rawp2);
                                        points.Add(rawp4);
                                        points.Add(rawp3);
                                        points.Add(rawp1);
                                        s = Math.Abs(points.Take(points.Count - 1)
                                           .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                           .Sum() / 2 / 1000000);//据说计算面积
                                        h = (double)row3["dz"];
                                        if (h > 0)
                                        {
                                            tmpSumRes = tmpSumRes + ReservorDraw.Cal_Capacity(s, h, (double)row3["poro"], (double)row3["barsa"], ro, b0);
                                        }
                                    }
                                }
                                if (isSameFacies)
                                {
                                    foreach (Polygon myPolygon in polygonList)
                                    {
                                        if (null == allCh)
                                        {
                                            this.canvesprt.Children.Add(myPolygon);
                                        }
                                    }
                                    resSum = tmpSumRes + resSum;
                                }
                            }
                        }
                    }
                    if (hasInj)//画油井圆
                    {
                        bool isSameFacies = true;
                        List<Polygon> polygonList = new List<Polygon>();
                        double tmpSumRes = 0;
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            string faciesProdS = row3["facies"].ToString();
                            if (soil > 0 && pointD <= a && !faciesProdS.Equals("0")
                                //&& faciesProdS.Equals(faciesProd)//同相带
                                )
                            {
                                if (!faciesProdS.Equals(faciesProd))
                                {
                                    isSameFacies = false;
                                }
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;
                                SolidColorBrush myBrush = new SolidColorBrush(Colors.PaleGoldenrod);//.PaleGoldenrod);
                                myPolygon2.Fill = myBrush;
                                myPolygon2.Points = myPointCollection2;
                                //this.canvesprt.Children.Add(myPolygon2);
                                polygonList.Add(myPolygon2);
                                //储量
                                Point rawp1 = new Point((double)row3["rawx0"], (double)row3["rawy0"]);
                                Point rawp2 = new Point((double)row3["rawx1"], (double)row3["rawy1"]);
                                Point rawp3 = new Point((double)row3["rawx2"], (double)row3["rawy2"]);
                                Point rawp4 = new Point((double)row3["rawx3"], (double)row3["rawy3"]);
                                if (!hasColorPoints.Contains(rawp1))
                                {
                                    List<Point> points = new List<Point>();
                                    points.Add(rawp1);
                                    points.Add(rawp2);
                                    points.Add(rawp4);
                                    points.Add(rawp3);
                                    points.Add(rawp1);
                                    s = Math.Abs(points.Take(points.Count - 1)
                                       .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                       .Sum() / 2 / 1000000);//据说计算面积
                                    h = (double)row3["dz"];
                                    if (h > 0)
                                    {
                                        tmpSumRes = tmpSumRes + ReservorDraw.Cal_Capacity(s, h, (double)row3["poro"], (double)row3["barsa"], ro, b0);
                                    }
                                }
                            }
                        }
                        if (isSameFacies)
                        {
                            foreach (Polygon myPolygon in polygonList)
                            {
                                if (null == allCh)
                                {
                                    this.canvesprt.Children.Add(myPolygon);
                                }
                            }
                            resSum = tmpSumRes + resSum;
                        }
                    }
                }
                //画井点
                if (null == allCh)
                {
                    drawWells(row1, wellPoints, wellNames);
                }
            }
            if (resSum > 0)
            {
                this.ch_res_all.Text = "层内干扰储量" + resSum.ToString("0.0000") + "万吨";
                tmpVol4EachRes = resSum.ToString("0.0000");
            }
            else tmpVol4EachRes = "0";
            //画井点
            for (int i = 0; i < wellPoints.Count; i++)
            {
                if (wellPoints[i] != null)
                {
                    this.canvesprt.Children.Add(wellPoints[i]);
                    this.canvesprt.Children.Add(wellNames[i]);
                }
            }
        }

        private void getPerfectPoints()
        {
            double a = 0;
            double b = 0;
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
            }
            catch
            {
                System.Console.WriteLine("数字格式错误");
            }
            //在注采完善区
            foreach (DataRow rowPerfect in canvesGrid.Rows)
            {
                if ("OIL".Equals(rowPerfect["stat"]))
                {
                    bool hasPerfectInj = false;
                    Point pordPerfectp = new Point((double)rowPerfect["x0"], (double)rowPerfect["y0"]);
                    foreach (DataRow row2 in canvesGrid.Rows)
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointPerfectD = pointl(pordPerfectp, injp);
                            if (pointPerfectD <= a || pointPerfectD <= b)
                            {
                                hasPerfectInj = true;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    if (soil > 0 && pointI0D <= b)
                                    {
                                        perfectPoints.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                    }
                                }
                            }
                        }
                    }
                    if (hasPerfectInj)//画油井圆
                    {
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordPerfectp, point0);
                            if (soil > 0 && pointD <= a)
                            {
                                perfectPoints.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                            }
                        }
                    }
                }
            }
            //
        }
        /// <summary>
        /// 平面干扰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawPlane(object sender, RoutedEventArgs e)
        {
            //地层均质系数
            double obFactorD = 0;
            double notObFactorD = 0;
            double a = 0;
            double b = 0;
            //bool isInPerfect = false;
            //画井点用的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            double h = 0;
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            List<Point> hasColorPoints = new List<Point>();
            if (perfectPoints == null || perfectPoints.Count == 0)
            {
                //getPerfectPoints();
            }
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch { Console.WriteLine("格式错误"); }
            try
            {
                obFactorD = Convert.ToDouble(this.obFactor.Text);
                notObFactorD = Convert.ToDouble(this.notObFactor.Text);
            }
            catch { Console.WriteLine("格式错误"); }

            //double maxpermx = (double)canvesGrid.Compute("max(permx)", "");
            //Console.WriteLine(maxpermx);
            foreach (DataRow row1 in canvesGrid.Rows)//寻找油井
            {
                if ("OIL".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    string faciesProd = row1["facies"].ToString();
                    double dz = Convert.ToDouble(row1["dz"]);
                    double permx = Convert.ToDouble(row1["permx"]);
                    String jh = row1["jh"].ToString();
                    String ch = row1["ch"].ToString();
                    DataRow[] jhRows = wellCoordTrueEnd.Select("jh = '" + jh + "'");
                    List<string> chList = new List<string>();
                    List<double> dicengXishu = new List<double>();
                    double yxhd = -1;
                    double maxDicengXishu = -1;
                    double avgDicengXishu = -1;
                    double sumDicengXishu = 0;
                    foreach (DataRow wellRow in jhRows)
                    {
                        if (!chList.Contains(wellRow["z"].ToString()))
                        {
                            chList.Add(wellRow["z"].ToString());
                        }
                        dicengXishu.Add(Convert.ToDouble(wellRow["地层系数"]));
                        if (ch.Equals(wellRow["z"].ToString()))
                        {
                            yxhd = Convert.ToDouble(wellRow["地层系数"]) / permx;// 计算有效厚度 
                            // Console.WriteLine(dz + "=" + yxhd + "=" + permx + "=" + Convert.ToDouble(wellRow["地层系数"]));
                        }

                    }

                    int chCount = chList.Count;
                    foreach (double onedicengxishu in dicengXishu)
                    {
                        if (onedicengxishu > maxDicengXishu)
                        {
                            maxDicengXishu = onedicengxishu;
                        }
                        sumDicengXishu += onedicengxishu;
                    }
                    avgDicengXishu = sumDicengXishu / dicengXishu.Count;
                    foreach (DataRow row2 in canvesGrid.Rows)// 扫描水井 
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            string faciesInj2 = row2["facies"].ToString();
                            if ((pointD <= a || pointD <= b) // 注采完善
                            && (yxhd >= 1//射开有效厚度大于1m的层;
                                         //|| (chCount <= 4 && avgDicengXishu / maxDicengXishu < obFactorD)
                                         // || (chCount >= 5 && avgDicengXishu / maxDicengXishu < notObFactorD)
                            ))
                            {
                                hasInj = true;
                                //&& faciesInj2.Equals(faciesProd)//同相带
                                List<Polygon> polygonList = new List<Polygon>();
                                bool isSameFacies = true;
                                double tmpSumRes = 0;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    string faciesInjS = row3["facies"].ToString();
                                    if (soil > 0 && pointI0D <= b && !faciesInjS.Equals("0")
                                        //   && faciesInjS.Equals(faciesProd)//同相带
                                        )
                                    {
                                        if (!faciesInjS.Equals(faciesInj2))//不同相带
                                        {
                                            isSameFacies = false;
                                        }
                                        Polygon myPolygon2 = new Polygon();
                                        PointCollection myPointCollection2 = new PointCollection();
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                        // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                        myPolygon2.StrokeThickness = 0.00;
                                        SolidColorBrush myBrush = new SolidColorBrush(Colors.YellowGreen);//.PaleGoldenrod);
                                        myPolygon2.Fill = myBrush;
                                        myPolygon2.Points = myPointCollection2;
                                        //this.canvesprt.Children.Add(myPolygon2);
                                        polygonList.Add(myPolygon2);
                                        //储量
                                        Point rawp1 = new Point((double)row3["rawx0"], (double)row3["rawy0"]);
                                        Point rawp2 = new Point((double)row3["rawx1"], (double)row3["rawy1"]);
                                        Point rawp3 = new Point((double)row3["rawx2"], (double)row3["rawy2"]);
                                        Point rawp4 = new Point((double)row3["rawx3"], (double)row3["rawy3"]);
                                        List<Point> points = new List<Point>();
                                        hasColorPoints.Add(rawp1);
                                        points.Add(rawp1);
                                        points.Add(rawp2);
                                        points.Add(rawp4);
                                        points.Add(rawp3);
                                        points.Add(rawp1);
                                        s = Math.Abs(points.Take(points.Count - 1)
                                           .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                           .Sum() / 2 / 1000000);//据说计算面积
                                        h = (double)row3["dz"];
                                        if (h > 0)
                                        {
                                            tmpSumRes = tmpSumRes + ReservorDraw.Cal_Capacity(s, h, (double)row3["poro"], (double)row3["barsa"], ro, b0);
                                        }
                                    }
                                }
                                if (!isSameFacies)
                                {
                                    foreach (Polygon myPolygon in polygonList)
                                    {
                                        if (null == allCh)
                                        {
                                            this.canvesprt.Children.Add(myPolygon);
                                        }
                                    }
                                    resSum = tmpSumRes + resSum;
                                }
                            }
                        }
                    }
                    if (hasInj)//画油井圆
                    {
                        bool isSameFacies = true;
                        List<Polygon> polygonList = new List<Polygon>();
                        double tmpSumRes = 0;
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            string faciesProdS = row3["facies"].ToString();
                            if (soil > 0 && pointD <= a && !faciesProdS.Equals("0")
                                //&& faciesProdS.Equals(faciesProd)//同相带
                                )
                            {
                                if (!faciesProdS.Equals(faciesProd))
                                {
                                    isSameFacies = false;
                                }
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;
                                SolidColorBrush myBrush = new SolidColorBrush(Colors.YellowGreen);//.PaleGoldenrod);
                                myPolygon2.Fill = myBrush;
                                myPolygon2.Points = myPointCollection2;
                                //this.canvesprt.Children.Add(myPolygon2);
                                polygonList.Add(myPolygon2);
                                //储量
                                Point rawp1 = new Point((double)row3["rawx0"], (double)row3["rawy0"]);
                                Point rawp2 = new Point((double)row3["rawx1"], (double)row3["rawy1"]);
                                Point rawp3 = new Point((double)row3["rawx2"], (double)row3["rawy2"]);
                                Point rawp4 = new Point((double)row3["rawx3"], (double)row3["rawy3"]);
                                if (!hasColorPoints.Contains(rawp1))
                                {
                                    List<Point> points = new List<Point>();
                                    points.Add(rawp1);
                                    points.Add(rawp2);
                                    points.Add(rawp4);
                                    points.Add(rawp3);
                                    points.Add(rawp1);
                                    s = Math.Abs(points.Take(points.Count - 1)
                                       .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                       .Sum() / 2 / 1000000);//据说计算面积
                                    h = (double)row3["dz"];
                                    if (h > 0)
                                    {
                                        tmpSumRes = tmpSumRes + ReservorDraw.Cal_Capacity(s, h, (double)row3["poro"], (double)row3["barsa"], ro, b0);
                                    }
                                }
                            }
                        }
                        if (!isSameFacies)
                        {
                            foreach (Polygon myPolygon in polygonList)
                            {
                                if (null == allCh)
                                {
                                    this.canvesprt.Children.Add(myPolygon);
                                }
                            }
                            resSum = tmpSumRes + resSum;
                        }
                    }
                }
                //画井点
                if (null == allCh)
                {
                    drawWells(row1, wellPoints, wellNames);
                }
            }
            // Console.WriteLine("平面干扰储量" + resSum.ToString("0.0000") + "万吨");
            if (resSum > 0)
            {
                this.ch_res_all.Text = "平面干扰储量" + resSum.ToString("0.0000") + "万吨";

                tmpVol4EachRes = resSum.ToString("0.0000");
            }
            else tmpVol4EachRes = "0";
            //画井点
            for (int i = 0; i < wellPoints.Count; i++)
            {
                if (wellPoints[i] != null)
                {
                    this.canvesprt.Children.Add(wellPoints[i]);
                    this.canvesprt.Children.Add(wellNames[i]);
                }
            }
        }
        /// <summary>
        /// 层间干扰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawInterLayer(object sender, RoutedEventArgs e)
        {
            //地层均质系数
            double obFactorD = 0;
            double notObFactorD = 0;
            double a = 0;
            double b = 0;
            //bool isInPerfect = false;
            //画井点用的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            double h = 0;
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            List<Point> hasColorPoints = new List<Point>();
            if (perfectPoints == null || perfectPoints.Count == 0)
            {
                //getPerfectPoints();
            }
            try
            {
                if (this.textProja != null && !"".Equals(textProja))
                {
                    a = Convert.ToDouble(this.textProja.Text) * m_d_zoomfactor2;
                }
                if (this.textInjb != null && !"".Equals(textInjb))
                {
                    b = Convert.ToDouble(this.textInjb.Text) * m_d_zoomfactor2;
                }
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch { Console.WriteLine("格式错误"); }
            try
            {
                obFactorD = Convert.ToDouble(this.obFactor.Text);
                notObFactorD = Convert.ToDouble(this.notObFactor.Text);
            }
            catch { Console.WriteLine("格式错误"); }

            //double maxpermx = (double)canvesGrid.Compute("max(permx)", "");
            //Console.WriteLine(maxpermx);
            foreach (DataRow row1 in canvesGrid.Rows)
            {
                if ("OIL".Equals(row1["stat"]))
                {
                    bool hasInj = false;
                    Point pordp = new Point((double)row1["x0"], (double)row1["y0"]);
                    double dz = Convert.ToDouble(row1["dz"]);
                    double permx = Convert.ToDouble(row1["permx"]);
                    String jh = row1["jh"].ToString();
                    String ch = row1["ch"].ToString();
                    DataRow[] jhRows = wellCoordTrueEnd.Select("jh = '" + jh + "'");
                    List<string> chList = new List<string>();
                    List<double> dicengXishu = new List<double>();
                    double yxhd = -1;
                    double maxDicengXishu = -1;
                    double avgDicengXishu = -1;
                    double sumDicengXishu = 0;
                    foreach (DataRow wellRow in jhRows)
                    {
                        if (!chList.Contains(wellRow["z"].ToString()))
                        {
                            chList.Add(wellRow["z"].ToString());
                        }
                        dicengXishu.Add(Convert.ToDouble(wellRow["地层系数"]));
                        if (ch.Equals(wellRow["z"].ToString()))
                        {
                            yxhd = Convert.ToDouble(wellRow["地层系数"]) / permx;// 计算有效厚度 
                            // Console.WriteLine(dz + "=" + yxhd + "=" + permx + "=" + Convert.ToDouble(wellRow["地层系数"]));
                        }

                    }

                    int chCount = chList.Count;
                    foreach (double onedicengxishu in dicengXishu)
                    {
                        if (onedicengxishu > maxDicengXishu)
                        {
                            maxDicengXishu = onedicengxishu;
                        }
                        sumDicengXishu += onedicengxishu;
                    }
                    avgDicengXishu = sumDicengXishu / dicengXishu.Count;
                    foreach (DataRow row2 in canvesGrid.Rows)// 扫描水井 
                    {
                        if ("WATER".Equals(row2["stat"]))
                        {
                            Point injp = new Point((double)row2["x0"], (double)row2["y0"]);
                            double pointD = pointl(pordp, injp);
                            if ((pointD <= a || pointD <= b) // 注采完善
                            && (
                            yxhd < 1//射开有效厚度大于1m的层;
                                    //|| (chCount <= 4 && avgDicengXishu / maxDicengXishu < obFactorD)
                                    // || (chCount >= 5 && avgDicengXishu / maxDicengXishu < notObFactorD)
                            ))
                            {
                                hasInj = true;
                                foreach (DataRow row3 in canvesGrid.Rows)//画水井圆
                                {
                                    double soil = (double)row3["barsa"];
                                    Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                                    double pointI0D = pointl(injp, point0);
                                    if (soil > 0 && pointI0D <= b)
                                    {
                                        Polygon myPolygon2 = new Polygon();
                                        PointCollection myPointCollection2 = new PointCollection();
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                        myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                        // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                        myPolygon2.StrokeThickness = 0.00;
                                        SolidColorBrush myBrush = new SolidColorBrush(Colors.SlateBlue);//.PaleGoldenrod);
                                        myPolygon2.Fill = myBrush;
                                        myPolygon2.Points = myPointCollection2;
                                        if (null == allCh)
                                        {
                                            this.canvesprt.Children.Add(myPolygon2);
                                        }
                                        //储量
                                        Point rawp1 = new Point((double)row3["rawx0"], (double)row3["rawy0"]);
                                        Point rawp2 = new Point((double)row3["rawx1"], (double)row3["rawy1"]);
                                        Point rawp3 = new Point((double)row3["rawx2"], (double)row3["rawy2"]);
                                        Point rawp4 = new Point((double)row3["rawx3"], (double)row3["rawy3"]);
                                        List<Point> points = new List<Point>();
                                        hasColorPoints.Add(rawp1);
                                        points.Add(rawp1);
                                        points.Add(rawp2);
                                        points.Add(rawp4);
                                        points.Add(rawp3);
                                        points.Add(rawp1);
                                        s = Math.Abs(points.Take(points.Count - 1)
                                           .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                           .Sum() / 2 / 1000000);//据说计算面积
                                        h = (double)row3["dz"];
                                        if (h > 0)
                                        {
                                            resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)row3["poro"], (double)row3["barsa"], ro, b0);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (hasInj)//画油井圆
                    {
                        foreach (DataRow row3 in canvesGrid.Rows)
                        {
                            double soil = (double)row3["barsa"];
                            Point point0 = new Point((double)row3["x0"], (double)row3["y0"]);
                            double pointD = pointl(pordp, point0);
                            if (soil > 0 && pointD <= a)
                            {
                                Polygon myPolygon2 = new Polygon();
                                PointCollection myPointCollection2 = new PointCollection();
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x0"], (double)row3["y0"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x1"], (double)row3["y1"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x3"], (double)row3["y3"]));
                                myPointCollection2.Add(new System.Windows.Point((double)row3["x2"], (double)row3["y2"]));
                                // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                myPolygon2.StrokeThickness = 0.00;
                                SolidColorBrush myBrush = new SolidColorBrush(Colors.SlateBlue);//.PaleGoldenrod);
                                myPolygon2.Fill = myBrush;
                                myPolygon2.Points = myPointCollection2;
                                if (null == allCh)
                                {
                                    this.canvesprt.Children.Add(myPolygon2);
                                }
                                //储量
                                Point rawp1 = new Point((double)row3["rawx0"], (double)row3["rawy0"]);
                                Point rawp2 = new Point((double)row3["rawx1"], (double)row3["rawy1"]);
                                Point rawp3 = new Point((double)row3["rawx2"], (double)row3["rawy2"]);
                                Point rawp4 = new Point((double)row3["rawx3"], (double)row3["rawy3"]);
                                if (!hasColorPoints.Contains(rawp1))
                                {
                                    List<Point> points = new List<Point>();
                                    points.Add(rawp1);
                                    points.Add(rawp2);
                                    points.Add(rawp4);
                                    points.Add(rawp3);
                                    points.Add(rawp1);
                                    s = Math.Abs(points.Take(points.Count - 1)
                                       .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                       .Sum() / 2 / 1000000);//据说计算面积
                                    h = (double)row3["dz"];
                                    if (h > 0)
                                    {
                                        resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)row3["poro"], (double)row3["barsa"], ro, b0);
                                    }
                                }
                            }
                        }
                    }
                }
                //画井点
                if (null == allCh)
                {
                    drawWells(row1, wellPoints, wellNames);
                }
            }
            if (resSum > 0)
            {
                this.ch_res_all.Text = "层间干扰储量" + resSum.ToString("0.0000") + "万吨";
                tmpVol4EachRes = resSum.ToString("0.0000");
            }
            else tmpVol4EachRes = "0";
            //画井点
            for (int i = 0; i < wellPoints.Count; i++)
            {
                if (wellPoints[i] != null)
                {
                    this.canvesprt.Children.Add(wellPoints[i]);
                    this.canvesprt.Children.Add(wellNames[i]);
                }
            }
        }
        /// <summary>
        /// 非井控断层边部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawFaultBorderOutCtrl(object sender, RoutedEventArgs e)
        {
            drawWellCtrlFlag = false;
            drawWellCtrl(sender, e);//寻找井控范围
            drawWellCtrlFlag = true;
            List<Point> otherSidePoint = new List<Point>();
            List<Point> oneSidePoint = faultPointsAll;
            String textdcRange = this.textdcRange.Text;
            double textdcRangeD = 0.0;
            double h = 0;// 
            double s = 0;
            double ro = 0;
            double b0 = 0;
            double resSum = 0;
            //画井点用的
            List<Ellipse> wellPoints = new List<Ellipse>();
            List<TextBlock> wellNames = new List<TextBlock>();
            //System.Console.WriteLine("选中的list:"+ReservorDraw.pointList.Count());
            try
            {
                if (textdcRangeOutCtrl != null && !"".Equals(textdcRangeOutCtrl.Text))
                {
                    textdcRangeD = Convert.ToDouble(this.textdcRangeOutCtrl.Text) * m_d_zoomfactor2;
                }
                ro = Convert.ToDouble(this.res_density.Text);
                b0 = Convert.ToDouble(this.res_vol.Text);
            }
            catch
            {
                System.Console.WriteLine("textdcRangeD格式错误");
            }
            // System.Console.WriteLine("textdcRangeD:" + textdcRangeD);
            if (b0 > 0)
            {

                foreach (DataRow canvesRow in canvesGrid.Rows)
                {
                    foreach (Point faultPoint3 in oneSidePoint)
                    {
                        double x3 = (double)canvesRow["x3"];
                        double y3 = (double)canvesRow["y3"];
                        if (x3 <= faultPoint3.X + textdcRangeD
                            && x3 >= faultPoint3.X - textdcRangeD
                            && y3 <= faultPoint3.Y + textdcRangeD
                            && y3 >= faultPoint3.Y - textdcRangeD
                            //&& !"fault".Equals(canvesRow["checkpoint"].ToString())
                            )
                        {
                            //canvesRow["checkpoint"] = "fault";
                            double oil = (double)canvesRow["barsa"];//饱和度
                            if (oil > 0)
                            {
                                bool inWellCtrlFlag = false;
                                foreach (List<Point> oneWellCtrl in wellCtrlList)
                                {
                                    Point point0 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                                    if (ReservorDraw.isInRegion(point0, oneWellCtrl))
                                    {
                                        inWellCtrlFlag = true;
                                        break;
                                    }
                                }
                                if (!inWellCtrlFlag)
                                {
                                    Polygon myPolygon2 = new Polygon();
                                    PointCollection myPointCollection2 = new PointCollection();
                                    // myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                                    myPolygon2.StrokeThickness = 0.00;//00000000000000
                                    Point p0 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                                    Point p1 = new Point((double)canvesRow["x1"], (double)canvesRow["y1"]);
                                    Point p2 = new Point((double)canvesRow["x2"], (double)canvesRow["y2"]);
                                    Point p3 = new Point((double)canvesRow["x3"], (double)canvesRow["y3"]);
                                    Point rawp1 = new Point((double)canvesRow["rawx0"], (double)canvesRow["rawy0"]);
                                    Point rawp2 = new Point((double)canvesRow["rawx1"], (double)canvesRow["rawy1"]);
                                    Point rawp3 = new Point((double)canvesRow["rawx2"], (double)canvesRow["rawy2"]);
                                    Point rawp4 = new Point((double)canvesRow["rawx3"], (double)canvesRow["rawy3"]);
                                    myPointCollection2.Add(p0);
                                    myPointCollection2.Add(p1);
                                    myPointCollection2.Add(p3);
                                    myPointCollection2.Add(p2);
                                    myPolygon2.Points = myPointCollection2;
                                    myPolygon2.Fill = Brushes.Gray;
                                    if (null == allCh)
                                    { this.canvesprt.Children.Add(myPolygon2); }

                                    List<Point> points = new List<Point>();
                                    points.Add(rawp1);
                                    points.Add(rawp2);
                                    points.Add(rawp4);
                                    points.Add(rawp3);
                                    points.Add(rawp1);
                                    s = Math.Abs(points.Take(points.Count - 1)
                                     .Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y))
                                     .Sum() / 2 / 1000000);//据说计算面积
                                    h = (double)canvesRow["dz"];
                                    if (h > 0)
                                    {
                                        resSum = resSum + ReservorDraw.Cal_Capacity(s, h, (double)canvesRow["poro"], (double)canvesRow["barsa"], ro, b0);
                                    }
                                }
                            }
                            break;
                        }
                    }

                }
            }
            if (resSum > 0)
            {
                this.ch_res_all.Text = "断层储量" + resSum.ToString("0.0000") + "万吨";
                tmpVol4EachRes = resSum.ToString("0.0000");
            }
            else tmpVol4EachRes = "0";

        }

        private void newWinAllRes(object sender, RoutedEventArgs e)
        {
            String comboCh = ComboBoxCH.Text;
            DataTable allResDt = new DataTable();

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "层号";
            allResDt.Columns.Add(column);

            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");
            column2.ColumnName = "储量";
            allResDt.Columns.Add(column2);

            if (null == comboCh || "".Equals(comboCh))
            {
                findDc(sender, e);//读取文件
                Console.WriteLine("读取文件");
            }
            for (int i = 1; i <= maxCh; i++)
            {
                allCh = "" + i;
                findDc(sender, e);//
                DataRow row = allResDt.NewRow();
                row["层号"] = allCh;
                row["储量"] = allVol;
                allResDt.Rows.Add(row);
            }

            AllChResContainer draw = new AllChResContainer("储量", allResDt);
            draw.Show();
            allCh = null;
        }

        private void newWinEachRes(object sender, RoutedEventArgs e)
        {
            String comboCh = ComboBoxCH.Text;
            String oldDrawTypeStr = this.ComboBoxCH.Text;
            int oldSoil = combo_soiltime.SelectedIndex;
            if (null == comboCh || "".Equals(comboCh))
            {
                Console.WriteLine("newWinEachRes读取文件");
                findDc(sender, e);//读取文件
                Console.WriteLine("读取文件");
            }
            else
            {
                string headTime = soiltimeList[0];
                string endTime = soiltimeList[soiltimeList.Count() - 1];
                DataTable allResDt = new DataTable();

                DataColumn column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "剩余油类型";
                allResDt.Columns.Add(column);

                DataColumn column1 = new DataColumn();
                column1.DataType = System.Type.GetType("System.String");
                column1.ColumnName = headTime;
                allResDt.Columns.Add(column1);

                DataColumn column2 = new DataColumn();
                column2.DataType = System.Type.GetType("System.String");
                column2.ColumnName = endTime;
                allResDt.Columns.Add(column2);

                comboCh = ComboBoxCH.Text;
                allCh = comboCh;
                combo_soiltime.SelectedIndex = 0;
                findDc(sender, e);//
                //砂体
                DataRow rowSand = allResDt.NewRow();
                drawSandBorder(sender, e);
                rowSand["剩余油类型"] = "砂体边部";
                rowSand[headTime] = tmpVol4EachRes;
                allResDt.Rows.Add(rowSand);
                //
                DataRow rowFaultBorder = allResDt.NewRow();
                drawFaultBorder(sender, e);
                rowFaultBorder["剩余油类型"] = "断层(井控)";
                rowFaultBorder[headTime] = tmpVol4EachRes;
                allResDt.Rows.Add(rowFaultBorder);
                //
                DataRow rowFaultBorderOutCtr = allResDt.NewRow();
                drawFaultBorderOutCtrl(sender, e);
                rowFaultBorderOutCtr["剩余油类型"] = "断层(非井控)";
                rowFaultBorderOutCtr[headTime] = tmpVol4EachRes;
                allResDt.Rows.Add(rowFaultBorderOutCtr);
                //
                DataRow rowInjWithoutOil = allResDt.NewRow();
                drawInjWithoutOil(sender, e);
                rowInjWithoutOil["剩余油类型"] = "有注无采";
                rowInjWithoutOil[headTime] = tmpVol4EachRes;
                allResDt.Rows.Add(rowInjWithoutOil);
                //
                DataRow rowOilWithoutInj = allResDt.NewRow();
                drawOilWithoutInj(sender, e);
                rowOilWithoutInj["剩余油类型"] = "有采无注";
                rowOilWithoutInj[headTime] = tmpVol4EachRes;
                allResDt.Rows.Add(rowOilWithoutInj);
                //
                DataRow rowPlane = allResDt.NewRow();
                drawPlane(sender, e);
                rowPlane["剩余油类型"] = "平面干扰";
                rowPlane[headTime] = tmpVol4EachRes;
                allResDt.Rows.Add(rowPlane);
                //
                DataRow rowInterLayer = allResDt.NewRow();
                drawInterLayer(sender, e);
                rowInterLayer["剩余油类型"] = "层间干扰";
                rowInterLayer[headTime] = tmpVol4EachRes;
                allResDt.Rows.Add(rowInterLayer);
                //
                DataRow rowInLayer = allResDt.NewRow();
                drawInLayer(sender, e);
                rowInLayer["剩余油类型"] = "层内干扰";
                rowInLayer[headTime] = tmpVol4EachRes;
                allResDt.Rows.Add(rowInLayer);
                //最近的时间
                combo_soiltime.SelectedIndex = soiltimeList.Count() - 1;
                findDc(sender, e);
                //
                drawSandBorder(sender, e);
                DataRow rowSand2 = allResDt.Rows[0];
                rowSand2.BeginEdit();
                rowSand2[endTime] = tmpVol4EachRes; ;
                rowSand2.EndEdit();
                //
                drawFaultBorder(sender, e);
                DataRow rowFaultBorder2 = allResDt.Rows[1];
                rowFaultBorder2.BeginEdit();
                rowFaultBorder2[endTime] = tmpVol4EachRes; ;
                rowFaultBorder2.EndEdit();
                //
                drawFaultBorderOutCtrl(sender, e);
                DataRow rowFaultBorderOutCtr2 = allResDt.Rows[2];
                rowFaultBorderOutCtr2.BeginEdit();
                rowFaultBorderOutCtr2[endTime] = tmpVol4EachRes; ;
                rowFaultBorderOutCtr2.EndEdit();
                //
                drawInjWithoutOil(sender, e);
                DataRow rowInjWithoutOil2 = allResDt.Rows[3];
                rowInjWithoutOil2.BeginEdit();
                rowInjWithoutOil2[endTime] = tmpVol4EachRes; ;
                rowInjWithoutOil2.EndEdit();
                //
                drawOilWithoutInj(sender, e);
                DataRow rowOilWithoutInj2 = allResDt.Rows[4];
                rowOilWithoutInj2.BeginEdit();
                rowOilWithoutInj2[endTime] = tmpVol4EachRes; ;
                rowOilWithoutInj2.EndEdit();
                //
                drawPlane(sender, e);
                DataRow rowPlane2 = allResDt.Rows[5];
                rowPlane2.BeginEdit();
                rowPlane2[endTime] = tmpVol4EachRes; ;
                rowPlane2.EndEdit();
                //
                drawInterLayer(sender, e);
                DataRow rowInterLayer2 = allResDt.Rows[6];
                rowInterLayer2.BeginEdit();
                rowInterLayer2[endTime] = tmpVol4EachRes; ;
                rowInterLayer2.EndEdit();
                //
                drawInLayer(sender, e);
                DataRow rowInLayer2 = allResDt.Rows[7];
                rowInLayer2.BeginEdit();
                rowInLayer2[endTime] = tmpVol4EachRes; ;
                rowInLayer2.EndEdit();

                AllChResContainer draw = new AllChResContainer("储量", allResDt);
                draw.Show();

            }
            allCh = null;
            ComboBoxCH.Text = comboCh;
            this.ComboBoxCH.Text = oldDrawTypeStr;
            combo_soiltime.SelectedIndex = oldSoil;
            findDc(sender, e);
        }

        /**
         * FACIES分区计算一个储量
         * 2016-12-27 17:57:52
         * 可优化
         */
        private void calFaciesRes()
        {
            for (int i = 0; i < 10; i++)
            {
                faciesNumRes[i] = 0;
            }
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            double timed = ts.TotalSeconds;
            //string fgridpath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\F10-27RIGHT_E100.FGRID";
            //string prtpath = "E:\\wf.txt";
            //string schIncPath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\f10-27right_sch.inc";//含井号
            //string gothIncPath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\f10-27right_goth.inc";//断层吗
            ////string regIncPath = "E:\\Documents\\项目开发\\MyWork\\8\\需求\\f10-27right\\f10-27right_reg.inc";
            //string faciesPath = "E:\\1.txt";//相图吧
            //
            XmlHelper helper = new XmlHelper();
            string fgridpath = helper.GetXMLDocument("FGRID");
            string prtpath = helper.GetXMLDocument("PRTINC");
            string schIncPath = helper.GetXMLDocument("SCH");
            string gothIncPath = helper.GetXMLDocument("GOTH");
            string faciesPath = helper.GetXMLDocument("FACIES");
            string gproPath = helper.GetXMLDocument("GPRO");
            string finitPath = helper.GetXMLDocument("FINIT");
            string ch = this.ComboBoxCH.Text;
            drawTypeStr = this.drawtype.Text;
            if (allCh != null)
            {
                ch = allCh;
                drawTypeStr = "相图";
            }

            string combo_soiltimeStr = this.combo_soiltime.Text;
            //initCanvesGrid();二次实例化报错
            double d = 0;
            double a = 0;
            double b = 0;
            allVol = 0;
            double ro = 0;
            double b0 = 0;
            int onefacies = 0;

            //canvesGrid.Clear();
            //perfectPoints.Clear();//注采完善区域


            facies = null;
            if ("相图".Equals(drawTypeStr))
            {
                数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
                tablesize = fgridPrt.readFGRID(fgridpath);
                facies = fgridPrt.readFacies(faciesPath); // 新版变成旧版了 2017年5月23日 14:30:01再变新版
                // 2017年5月23日 14:30:17 尼玛变成旧版了
                //facies = fgridPrt.readRegInc(faciesPath); // 原旧版 重新启用 2017年5月8日 21:02:39 
                //Console.WriteLine(facies.Length);
                faciesNum = facies.Distinct().ToArray();
                /*foreach(int kkm in kk)
                   Console.WriteLine(kkm);*/
            }

            if (ch != null && !"".Equals(ch))//选中层号之后才开始
            {
                // canvesptr = this.canvesprt;
                //  canvesptr.Children.Clear();
                //   ReservorDraw.pointList.Clear();
                数模建模.SIMB.FgridNew fgridNew = new 数模建模.SIMB.FgridNew();

                try
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
                }
                //调用主线程UI的的代码  
                //    new Thread(o =>
                //  { 
                System.Console.WriteLine("开始解析文件:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
                // 解析文件
                数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
                tablesize = fgridPrt.readFGRID(fgridpath);
                DataTable dtfgridNew = fgridNew.readFile(fgridpath, ch);
                DataTable dtprt = fgridPrt.readPRT(prtpath, ch, combo_soiltimeStr);
                faultDt = fgridPrt.readGothInc(gothIncPath, ch);//断层
                fgridPrt.readSchInc(schIncPath, ch);
                double[] poro;//= fgridPrt.poro;//孔隙度
                //permx = fgridPrt.permx;//渗透率
                DataTable dzDt = fgridPrt.dzDt;//厚度 
                facies = fgridPrt.readFacies(faciesPath); // 新版变成旧版了 2017年5月23日 14:31:11 变新
                // 、、facies = 2017年5月23日 14:31:46 变旧
               // 、、facies = fgridPrt.readRegInc(faciesPath); // 原旧版 重新启用 2017年5月8日 21:02:39
                ntgs = fgridPrt.readNTG(gproPath);// 净毛比 2017年5月8日 20:43:53
                poro = fgridPrt.poro;// 孔隙度 2017年5月8日 20:44:06
                permx = fgridPrt.permx;// 渗透率 2017年5月8日 20:44:08
                dzs = fgridPrt.readDz(finitPath); // 新DZ 2017年5月23日 14:24:35
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
                double minpermx = Convert.ToDouble(list[0]);
                double maxpermx = Convert.ToDouble(list[list.Count - 1]);
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
                double minporo = Convert.ToDouble(list[0]);
                double maxporo = Convert.ToDouble(list[list.Count - 1]);

                // 计算缩放比例
                /* double dzoomx2, dzoomy2;
                 double maxX2 = Convert.ToDouble(dtfgridNew.Compute("max(x)", ""));
                 double maxY2 = Convert.ToDouble(dtfgridNew.Compute("max(y)", ""));
                 double minX2 = Convert.ToDouble(dtfgridNew.Compute("min(x)", ""));
                 double minY2 = Convert.ToDouble(dtfgridNew.Compute("min(y)", ""));
                 //System.Console.WriteLine("-------" + maxX2);
                 Canvas canvesptr = this.canvesprt;
                 dzoomx2 = canvesptr.ActualWidth / (maxX2 - minX2);
                 dzoomy2 = canvesptr.ActualHeight / (maxY2 - minY2);
                 m_d_zoomfactor2 = dzoomx2 < dzoomy2 ? dzoomx2 : dzoomy2;//改成取小的了 为了展示全图  2016-11-2 19:24:08
                 wellR = 200;
                 wellR = wellR * m_d_zoomfactor2;*/
                int prtXCount = 0;
                int prtYCount = 0;
                // d = d * m_d_zoomfactor2;
                //准备画井点用的
                /* wellCoord = fgridPrt.welspecsFakerWellCoord;//wellCoord;伪装2016-12-28 18:18:22
                 wellCoordTrueEnd = fgridPrt.wellCoord;//原wellCoord*/
                DataTable wellStat = fgridPrt.wellStat;
                /* Ellipse[] wellpoints = new Ellipse[wellCoord.Rows.Count];
                 Ellipse[] wellborders = new Ellipse[wellCoord.Rows.Count];
                 TextBlock[] TextNames = new TextBlock[wellCoord.Rows.Count];*/
                double[] pointX = new double[wellCoord.Rows.Count];
                double[] pointY = new double[wellCoord.Rows.Count];
                int jhCount = 0;
                Boolean noDz = false;
                //facies = null;
                /*if ("相图".Equals(drawTypeStr))
                {
                    facies = fgridPrt.readFacies(faciesPath);
                }*/
                System.Console.WriteLine("底图绘制xunhuan:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
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
                    //myPolygon2.Stroke = System.Windows.Media.Brushes.Black;
                    myPolygon2.StrokeThickness = 0.00;//00000000000000

                    double rawx0 = Convert.ToDouble(dtfgridNew.Rows[i + 0][0]);
                    double rawy0 = Convert.ToDouble(dtfgridNew.Rows[i + 0][1]);
                    double rawx1 = Convert.ToDouble(dtfgridNew.Rows[i + 1][0]);
                    double rawy1 = Convert.ToDouble(dtfgridNew.Rows[i + 1][1]);
                    double rawx3 = Convert.ToDouble(dtfgridNew.Rows[i + 3][0]);
                    double rawy3 = Convert.ToDouble(dtfgridNew.Rows[i + 3][1]);
                    double rawx2 = Convert.ToDouble(dtfgridNew.Rows[i + 2][0]);
                    double rawy2 = Convert.ToDouble(dtfgridNew.Rows[i + 2][1]);
                    /*  //四边形调序 否则化成8字形 canvas y轴坐标是反的
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
                      myPointCollection2.Add(new System.Windows.Point(x2, y2));*/

                    //if (pointl(new Point(x0, y0), new Point(156.164925685062, 285.026937384162)) < 1)
                    //{
                    //    System.Console.WriteLine(prtYCount + "x" + prtXCount);
                    //}

                    //上prt
                    int hadC = (Convert.ToInt32(ch) - 1) * tablesize[0] * tablesize[1]; //跳过前几层
                    int hady = prtYCount * tablesize[0];//跳过前几行
                    double val = Convert.ToDouble(dtprt.Rows[prtYCount][prtXCount]);
                    double dzval = 3;
                    dzval = dzs[hadC + hady + prtXCount];
                    /*
                    if (DBNull.Value != dzDt.Rows[prtYCount][prtXCount])// 2017年5月10日 14:29:41缺失关键字
                    {
                        dzval = Convert.ToDouble(dzDt.Rows[prtYCount][prtXCount]);
                    }
                    else
                    {
                        noDz = true;
                    }
                    */
                    //canvesGrid.Rows.Add(canvesGridRow);跳转至1005行

                    if ("相图".Equals(drawTypeStr))
                    {
                        onefacies = facies[hadC + hady + prtXCount];
                        if (onefacies > 0 && onefacies < 11 && null == allCh)
                        {
                            valBottom = 1;//低级颜色113
                            valTop = 10;//顶级颜色
                            inVal = (valTop - valBottom) / 4;//渐变级别//1 1.8   
                            Color myColor = barsaColor(facies[hadC + hady + prtXCount]);
                            SolidColorBrush myBrush = new SolidColorBrush(myColor);
                            myPolygon2.Fill = myBrush;
                        }
                    }
                    // for (int onefaciesi = 1; onefaciesi <= 10; onefaciesi++)
                    //foreach(int onefaciesi in faciesNum)
                    //0不要
                    for (int onefaciesi = 1; onefaciesi < faciesNum.Length; onefaciesi++)
                    {
                        if (onefacies == faciesNum[onefaciesi] && onefacies > 0 && onefacies < 11)
                        {
                            //全局储量
                            if (val > 0 && poro[hadC + hady + prtXCount] > 0 && b0 > 0)
                            {
                                List<Point> points = new List<Point>();
                                points.Add(new Point(rawx0, rawy0));
                                points.Add(new Point(rawx1, rawy1));
                                points.Add(new Point(rawx3, rawy3));
                                points.Add(new Point(rawx2, rawy2));
                                points.Add(new Point(rawx0, rawy0));//注意
                                double s = Math.Abs(points.Take(points.Count - 1)
                                  .Select((p, si) => (points[si + 1].X - p.X) * (points[si + 1].Y + p.Y))
                                  .Sum() / 2 / 1000000);//km2
                                faciesNumRes[onefaciesi - 1] = faciesNumRes[onefaciesi - 1] + 100 * s * dzval * poro[hadC + hady + prtXCount] * val * ro / b0;
                            }
                        }
                    }
                    // canvesGrid.Rows.Add(canvesGridRow);
                    //行号结算
                    prtXCount++;
                    if (prtXCount == tablesize[0])//第一行数据已写完
                    {
                        prtXCount = 0;
                        prtYCount++;
                    }
                }
                //System.Console.WriteLine("allVol:" + allVol);

                //断层 他项权证
                //List<PointCollection> faultPointCollections=new  List<PointCollection>();                

                //调用主线程UI的的代码
                //}));  
                //   }) { IsBackground = true }.Start();
                //最后我们来个图例
                System.Console.WriteLine("end:" + ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds - timed));
            }
            else
            {
                fgrid_ch(fgridpath);
                数模建模.SIMB.FgridPrt fgridPrt = new 数模建模.SIMB.FgridPrt();
                soiltimeList = fgridPrt.readSoilTime(prtpath);
                foreach (string soiltime in soiltimeList)
                {
                    this.combo_soiltime.Items.Add(soiltime);
                }
                this.combo_soiltime.SelectedIndex = 0;
            }
        }
        /**
         * Facies分区计算储量 
         * 2016-12-27 17:59:51
         */
        private void newWin4CalFaciesRes(object sender, RoutedEventArgs e)
        {
            String comboCh = ComboBoxCH.Text;
            DataTable allResDt = new DataTable();

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "层号";
            allResDt.Columns.Add(column);


            /*   if (null == comboCh || "".Equals(comboCh))
               {*/
            calFaciesRes();//读取文件
            Console.WriteLine("读取文件");
            /*   }*/
            //0不要
            for (int i = 1; i < faciesNum.Length; i++)
            {
                DataColumn column2 = new DataColumn();
                column2.DataType = System.Type.GetType("System.String");
                column2.ColumnName = "Facies" + faciesNum[i];
                allResDt.Columns.Add(column2);
            }
            for (int i = 1; i <= maxCh; i++)
            {
                allCh = "" + i;
                calFaciesRes();//
                DataRow row = allResDt.NewRow();
                row["层号"] = allCh;
                for (int faciesNumi = 1; faciesNumi < faciesNum.Length; faciesNumi++)
                {
                    row["Facies" + faciesNum[faciesNumi]] = faciesNumRes[faciesNumi - 1].ToString("0.0000");
                }
                allResDt.Rows.Add(row);
                /* //Facies=10
                 DataRow row10 = allResDt.NewRow();
                 row10["层号"] = allCh;
                 row10["Facies"] = "10";
                 row10["储量"] = faciesNumRes[1].ToString("0.0000");
                 allResDt.Rows.Add(row10);*/
            }

            AllChResContainer draw = new AllChResContainer("储量", allResDt);
            draw.Show();
            allCh = null;
        }
        /**
         * 单井各层储量
         */
        private void newWinEachWell(object sender, RoutedEventArgs e)
        {
            String comboCh = ComboBoxCH.Text;
            DataTable allResDt = new DataTable();
            DataTable canvesGridTmp = new DataTable();//4单井各层储量
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "井号";
            allResDt.Columns.Add(column);

            if (null == comboCh || "".Equals(comboCh))
            {
                Console.WriteLine("读取文件");
                findDc(null, null);//读取文件();//读取文件
            }

            for (int i = 1; i <= maxCh; i++)
            {
                DataColumn column2 = new DataColumn();
                column2.DataType = System.Type.GetType("System.String");
                column2.ColumnName = "层号" + i;
                allResDt.Columns.Add(column2);
            }

            for (int i = 1; i <= maxCh; i++)//分层tablesize[2]=0 (int i = 1; i <= maxCh; i++)
            {
                allCh = "" + i;
                findDc(null, null);//dododododo
                if (1 == i)
                {
                    Console.WriteLine("canvesGrid" + canvesGrid.Rows.Count);
                    canvesGridTmp = canvesGrid.Clone();
                    foreach (DataRow bigGridRow in canvesGrid.Rows)
                    {
                        if (bigGridRow["jh"] != null && !"".Equals(bigGridRow["jh"].ToString())
                            && "OIL".Equals(bigGridRow["stat"].ToString())
                            )
                        {
                            DataRow row = allResDt.NewRow();
                            row["井号"] = bigGridRow["jh"];
                            allResDt.Rows.Add(row);
                            canvesGridTmp.Rows.Add(bigGridRow.ItemArray);
                        }
                        else
                        {
                            // bigGridRow.Delete();
                        }
                    }
                    Console.WriteLine("canvesGridTmp" + canvesGridTmp.Rows.Count);
                }

                foreach (DataRow bigGridRow in canvesGridTmp.Rows)//分井
                {
                    String jh = bigGridRow["jh"].ToString();
                    String x0 = bigGridRow["x0"].ToString();
                    String y0 = bigGridRow["y0"].ToString();
                    wellPoint4allres = new Point(Convert.ToDouble(x0), Convert.ToDouble(y0));
                    switchClickMode = "单井井控范围";
                    canves1_MouseLeftButtonUp_prt(null, null);
                    DataRow dRow = allResDt.Select("井号='" + jh + "'")[0];
                    dRow.BeginEdit();
                    dRow["层号" + i] = wellRes4allres.ToString("0.0000");
                    dRow.EndEdit();
                    wellRes4allres = 0;
                }
            }
            AllChResContainer draw = new AllChResContainer("储量", allResDt);
            draw.Show();
            //全局变量还原
            wellPoint4allres = new Point();
            allCh = null;
        }

        private void newKsbEachC(object sender, RoutedEventArgs e)
        {
            String comboCh = ComboBoxCH.Text;
            DataTable allResDt = new DataTable();

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "层号";
            allResDt.Columns.Add(column);

            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");
            column2.ColumnName = "孔隙度";
            allResDt.Columns.Add(column2);

            DataColumn column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.String");
            column3.ColumnName = "渗透率";
            allResDt.Columns.Add(column3);

            DataColumn column4 = new DataColumn();
            column4.DataType = System.Type.GetType("System.String");
            column4.ColumnName = "饱和度";
            allResDt.Columns.Add(column4);

            if (null == comboCh || "".Equals(comboCh))
            {
                findDc(sender, e);//读取文件
                Console.WriteLine("读取文件");
            }
            for (int i = 1; i <= maxCh; i++)
            {
                allCh = "" + i;
                findDc(sender, e);//dodoodo
                double avgCalSoil = 0;
                double sumPoro = 0;
                double sumPermx = 0;
                double sumCalSoil = 0;
                int poroNum = 0;
                int permxNum = 0;
                foreach (DataRow bigRow in canvesGrid.Rows)
                {
                    double tmpPoro = Convert.ToDouble(bigRow["poro"]);
                    double tmpPermx = Convert.ToDouble(bigRow["permx"]);
                    double tmpSoil = Convert.ToDouble(bigRow["barsa"]);//饱和度
                    double tmpDz = Convert.ToDouble(bigRow["dz"]);//饱和度
                    double tmpNTG = Convert.ToDouble(bigRow["NTG"]);//饱和度
                    if (tmpPoro > 0)
                    {
                        sumPoro += tmpPoro;
                        poroNum++;
                    }
                    if (tmpPermx > 0)
                    {
                        sumPermx += tmpPermx;
                        permxNum++;
                    }
                    if (tmpSoil > 0)
                    {
                        sumCalSoil += tmpSoil * tmpPoro * tmpDz * tmpNTG;
                        avgCalSoil += tmpPoro * tmpDz * tmpNTG;
                    }
                }
                DataRow row = allResDt.NewRow();
                row["层号"] = allCh;
                row["孔隙度"] = (sumPoro / poroNum).ToString("0.0000");
                row["渗透率"] = (sumPermx / permxNum).ToString("0.0000");
                row["饱和度"] = (sumCalSoil / avgCalSoil).ToString("0.0000");
                allResDt.Rows.Add(row);
            }
            AllChResContainer draw = new AllChResContainer("储量", allResDt);
            draw.Title = "孔渗饱";
            draw.Show();
            allCh = null;
        }

        private void canves1_MouseWheel_prt(object sender, MouseWheelEventArgs e)
        {
            ReservorDraw.zoom_in(canvesprt, e);
        }
        /**
         * 圈选孔渗饱
         * 2017-1-9 20:43:34
         */
        private void newWinAvgKsb(object sender, RoutedEventArgs e)
        {
            DataTable allResDt = new DataTable();

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "层号";
            allResDt.Columns.Add(column);

            DataColumn column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.String");
            column2.ColumnName = "孔隙度";
            allResDt.Columns.Add(column2);

            DataColumn column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.String");
            column3.ColumnName = "渗透率";
            allResDt.Columns.Add(column3);

            DataColumn column4 = new DataColumn();
            column4.DataType = System.Type.GetType("System.String");
            column4.ColumnName = "饱和度";
            allResDt.Columns.Add(column4);
            double avgCalSoil = 0;
            double sumPoro = 0;
            double sumPermx = 0;
            double sumCalSoil = 0;
            int poroNum = 0;
            int permxNum = 0;
            foreach (DataRow canvesRow in canvesGrid.Rows)
            {
                Point p1 = new Point((double)canvesRow["x0"], (double)canvesRow["y0"]);
                Point p2 = new Point((double)canvesRow["x1"], (double)canvesRow["y1"]);
                Point p3 = new Point((double)canvesRow["x2"], (double)canvesRow["y2"]);
                Point p4 = new Point((double)canvesRow["x3"], (double)canvesRow["y3"]);
                Point rawp1 = new Point((double)canvesRow["rawx0"], (double)canvesRow["rawy0"]);
                Point rawp2 = new Point((double)canvesRow["rawx1"], (double)canvesRow["rawy1"]);
                Point rawp3 = new Point((double)canvesRow["rawx2"], (double)canvesRow["rawy2"]);
                Point rawp4 = new Point((double)canvesRow["rawx3"], (double)canvesRow["rawy3"]);
                if (ReservorDraw.isInRegion(p1, ReservorDraw.pointList)
                    || ReservorDraw.isInRegion(p2, ReservorDraw.pointList)
                    || ReservorDraw.isInRegion(p3, ReservorDraw.pointList)
                    || ReservorDraw.isInRegion(p4, ReservorDraw.pointList))
                {
                    double tmpPoro = Convert.ToDouble(canvesRow["poro"]);
                    double tmpPermx = Convert.ToDouble(canvesRow["permx"]);
                    double tmpSoil = Convert.ToDouble(canvesRow["barsa"]);//饱和度
                    double tmpDz = Convert.ToDouble(canvesRow["dz"]);//饱和度
                    double tmpNTG = Convert.ToDouble(canvesRow["NTG"]);//饱和度
                    if (tmpPoro > 0)
                    {
                        sumPoro += tmpPoro;
                        poroNum++;
                    }
                    if (tmpPermx > 0)
                    {
                        sumPermx += tmpPermx;
                        permxNum++;
                    }
                    if (tmpSoil > 0)
                    {
                        sumCalSoil += tmpSoil * tmpPoro * tmpDz * tmpNTG;
                        avgCalSoil += tmpPoro * tmpDz * tmpNTG;
                    }
                }

            }
            DataRow row = allResDt.NewRow();
            row["层号"] = allCh;
            row["孔隙度"] = (sumPoro / poroNum).ToString("0.0000");
            row["渗透率"] = (sumPermx / permxNum).ToString("0.0000");
            row["饱和度"] = (sumCalSoil / avgCalSoil).ToString("0.0000");
            allResDt.Rows.Add(row);

            AllChResContainer draw = new AllChResContainer("孔渗饱", allResDt);
            draw.Title = "孔渗饱";
            draw.Show();
            allCh = null;
        }
    }
}
