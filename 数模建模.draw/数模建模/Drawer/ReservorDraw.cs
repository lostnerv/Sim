using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using 建模数模.tools;
using System.Windows.Shapes;
using System.Windows.Media;
using 数模建模.Delaunay;
using System.Drawing;


namespace 数模建模.Drawer
{
    public static class  ReservorDraw
    {
        static DataTable wellList;
        static DataTable temp_wellList;
        public static DataTable result = new DataTable();
        static string wellSet;
        static double m_d_zoomfactor;
        static double maxY, maxX, minY, minX;
        static double dzoomx, dzoomy;
        //static float dzoomx, dzoomy;
        public static System.Windows.Point point;
        public static List<System.Windows.Point> pointList = new List<System.Windows.Point>();
        static double zoomx = 0;
        static double zoomy = 0;
        //canvas拖放
        static double downZoomx = 0;
        static double downZoomy = 0;
        static double lastMoveX = 0;
        static double lastMoveY = 0;
        static string zoomStat;

        public static DataTable getData(Canvas canves)
        {
            string sql = "select well_desc as jh,geo_offset_east as x,geo_offset_north as y from cd_well_source_a where geo_offset_north is not null and geo_offset_east is not null";
            wellList = GetDataAsDataTable.GetDataReasult(sql);

            wellList = GetDataAsDataTable.LoadDataFromExcel("D:\\wellloc.xls", "Sheet1"); ;

            double maxY = Convert.ToDouble(wellList.Compute("max(x)", ""));
            double maxX = Convert.ToDouble(wellList.Compute("max(y)", ""));

            double minY = Convert.ToDouble(wellList.Compute("min(x)", ""));
            double minX = Convert.ToDouble(wellList.Compute("min(y)", ""));

            dzoomx = canves.ActualWidth / (maxX - minX);
            dzoomy = canves.ActualHeight / (maxY - minY);

            double m_d_zoomfactor = dzoomx > dzoomy ? dzoomx : dzoomy;

            for (int i = 0; i < wellList.Rows.Count; i++)
            {
                double x = Math.Round((Convert.ToDouble(wellList.Rows[i][2]) - minX) * m_d_zoomfactor - 728, 1);
                double y = Math.Round((Convert.ToDouble(wellList.Rows[i][1]) - minY) * m_d_zoomfactor, 1);
                wellList.Rows[i][1] = y;
                wellList.Rows[i][2] = x;
            }
            return wellList;
        }

        public static DataTable getDataByArea(Canvas canves,DataTable dt)
        {
            DataColumn wellnumColumn = new DataColumn();
            wellnumColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            wellnumColumn.ColumnName = "井号";//该列得名称 
            result.Columns.Add(wellnumColumn);

            DataColumn hzbyColumn = new DataColumn();
            hzbyColumn.DataType = System.Type.GetType("System.String");//该列的数据类型 
            hzbyColumn.ColumnName = "面积";//该列得名称 
            result.Columns.Add(hzbyColumn);

            DataColumn hzbyColumn1 = new DataColumn();
            hzbyColumn1.DataType = System.Type.GetType("System.String");//该列的数据类型 
            hzbyColumn1.ColumnName = "储量";//该列得名称 
            result.Columns.Add(hzbyColumn1);

            foreach (DataRow tempRow in dt.Rows)
            {
                wellSet += "\'" + tempRow[0].ToString().Trim() + "\',";
            }
            wellSet = wellSet.Substring(0, wellSet.Length - 1);

            string sql = "select well_desc as jh,geo_offset_east as x,geo_offset_north as y from cd_well_source_a where geo_offset_north is not null and geo_offset_east is not null and well_desc in (" + wellSet + ") ";
            wellList = GetDataAsDataTable.GetDataReasult(sql);

            

            double maxY = Convert.ToDouble(wellList.Compute("max(x)", ""));
            double maxX = Convert.ToDouble(wellList.Compute("max(y)", ""));

            double minY = Convert.ToDouble(wellList.Compute("min(x)", ""));
            double minX = Convert.ToDouble(wellList.Compute("min(y)", ""));

            //float maxY = float.Parse(wellList.Compute("max(x)", "").ToString());
            //float maxX = float.Parse(wellList.Compute("max(y)", "").ToString());

            //float minY = float.Parse(wellList.Compute("min(x)", "").ToString());
            //float minX = float.Parse(wellList.Compute("min(y)", "").ToString());

            sql = "select well_desc as jh,geo_offset_east as x,geo_offset_north as y from cd_well_source_a where geo_offset_north >" + minX.ToString() + " and geo_offset_east> " + minY.ToString() + " and geo_offset_north < " + maxX.ToString() + " and geo_offset_east<"+maxY.ToString();


            wellList = GetDataAsDataTable.GetDataReasult(sql);

            dzoomx = canves.ActualWidth / (maxX - minX);
            dzoomy = canves.ActualHeight / (maxY - minY);

            double m_d_zoomfactor = dzoomx < dzoomy ? dzoomx : dzoomy;

            for (int i = 0; i < wellList.Rows.Count; i++)
            {
                PointF point1 = new System.Drawing.Point();
                double x = Math.Round((Convert.ToDouble(wellList.Rows[i][2]) - minX) * m_d_zoomfactor, 1);
                double y = Math.Round((Convert.ToDouble(wellList.Rows[i][1]) - minY) * m_d_zoomfactor, 1);
                //wellList.Rows[i][1] = y;
                //wellList.Rows[i][2] = x;

                wellList.Rows[i][1] = y;
                wellList.Rows[i][2] = x;

                point1.X = float.Parse(x.ToString());
                point1.Y = float.Parse(y.ToString());

                points.Add(point1);
            }

            temp_wellList = wellList.Copy();

            return wellList;
        } 

        public static DataTable DrawWellLoc(Canvas canves)
        {
            wellList = GetDataAsDataTable.LoadDataFromExcel("D:\\wellloc.xls", "Sheet1"); ;

            maxY = Convert.ToDouble(wellList.Compute("max(x)", ""));
            maxX = Convert.ToDouble(wellList.Compute("max(y)", ""));

            minY = Convert.ToDouble(wellList.Compute("min(x)", ""));
            minX = Convert.ToDouble(wellList.Compute("min(y)", ""));

            dzoomx =  canves.ActualWidth / (maxX - minX);
            dzoomy = canves.ActualHeight / (maxY - minY);

            m_d_zoomfactor = dzoomx > dzoomy ? dzoomx : dzoomy;

            
            
            for (int i = 0; i < wellList.Rows.Count; i++)
            {
                double x = Math.Round((Convert.ToDouble(wellList.Rows[i][2]) - minX) * m_d_zoomfactor - 728, 1);
                double y = Math.Round((Convert.ToDouble(wellList.Rows[i][1]) - minY) * m_d_zoomfactor, 1);
              
                wellList.Rows[i][1] = y;
                wellList.Rows[i][2] = x;
            }
            return wellList;
        }

        public static void zoom_in(Canvas canves, MouseWheelEventArgs e)
        {
            double ScaleX = 0;
            double ScaleY = 0;
            double dbl_ZoomX = ((ScaleTransform)(((TransformGroup)(((UIElement)(canves)).RenderTransform)).Children[0])).ScaleX;
            double dbl_ZoomY = ((ScaleTransform)(((TransformGroup)(((UIElement)(canves)).RenderTransform)).Children[0])).ScaleY;
            //Boolean canZoomflag = true;

            ((ScaleTransform)(((TransformGroup)(((UIElement)(canves)).RenderTransform)).Children[0])).CenterX = e.GetPosition(canves).X;
            ((ScaleTransform)(((TransformGroup)(((UIElement)(canves)).RenderTransform)).Children[0])).CenterY = e.GetPosition(canves).Y;

            if (e.Delta < 0)
            {
                ScaleX = dbl_ZoomX - 0.1 < 1 ? 0 : dbl_ZoomX - 0.1;
                ScaleY = dbl_ZoomY - 0.1 < 1 ? 0 : dbl_ZoomY - 0.1;
                
            }
            else if (e.Delta > 0)
            {
                ScaleX = dbl_ZoomX + 0.1 > 10.0 ? 10.0 : dbl_ZoomX + 0.1;
                ScaleY = dbl_ZoomY + 0.1 > 10.0 ? 10.0 : dbl_ZoomY + 0.1;
                //System.Console.WriteLine("22222");
            }
            /*if (ScaleX <= 0 || ScaleY <= 0)
            {
                System.Console.WriteLine("false");
                canZoomflag = false;
            }*/
            if (ScaleX > 0 && ScaleY>0)
            {
                zoomx = ScaleX;
                zoomy = ScaleY;
            
                ((ScaleTransform)(((TransformGroup)(((UIElement)(canves)).RenderTransform)).Children[0])).ScaleX = ScaleX;
                ((ScaleTransform)(((TransformGroup)(((UIElement)(canves)).RenderTransform)).Children[0])).ScaleY = ScaleY;
            }
        }

        public static void zoom_out(Canvas canves)
        {
            canves.Children.Clear();

            m_d_zoomfactor = m_d_zoomfactor / 2;

            for (int i = 0; i < wellList.Rows.Count; i++)
            {
                double x = Math.Round((Convert.ToDouble(wellList.Rows[i][2])) * (1 - m_d_zoomfactor), 1);
                double y = Math.Round((Convert.ToDouble(wellList.Rows[i][1])) * (1 - m_d_zoomfactor), 1);

                wellList.Rows[i][1] = y;
                wellList.Rows[i][2] = x;
            }
        }

        public static void canvas_zoom_down(Canvas leftCanvas, MouseButtonEventArgs e)
        {
            downZoomx = e.GetPosition(leftCanvas).X;
            downZoomy = e.GetPosition(leftCanvas).Y;
            lastMoveX = downZoomx;
            lastMoveY = downZoomy;
            if (e.RightButton == MouseButtonState.Pressed)//缩小
            {
                zoomStat = "缩小";
            }
            else if (e.LeftButton == MouseButtonState.Pressed)//放大
            {
                zoomStat = "放大";
            }
        }
        public static void canvas_zoom_up(Canvas leftCanvas, MouseButtonEventArgs e)
        {
            if (e.GetPosition(leftCanvas).X == downZoomx
                && e.GetPosition(leftCanvas).Y == downZoomy)
            {
                double ScaleX = 0;
                double ScaleY = 0;
                double dbl_ZoomX = ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).ScaleX;
                double dbl_ZoomY = ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).ScaleY;
                //Boolean canZoomflag = true;
                ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).CenterX = e.GetPosition(leftCanvas).X;
                ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).CenterY = e.GetPosition(leftCanvas).Y;

                if ("缩小".Equals(zoomStat))//缩小
                {
                    ScaleX = dbl_ZoomX - 0.1 < 1 ? 0 : dbl_ZoomX - 0.1;
                    ScaleY = dbl_ZoomY - 0.1 < 1 ? 0 : dbl_ZoomY - 0.1;
                }
                else if ("放大".Equals(zoomStat))//
                {
                    ScaleX = dbl_ZoomX + 0.1 > 10.0 ? 10.0 : dbl_ZoomX + 0.1;
                    ScaleY = dbl_ZoomY + 0.1 > 10.0 ? 10.0 : dbl_ZoomY + 0.1;
                }
                if (ScaleX > 0 && ScaleY > 0)
                {
                    ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).ScaleX = ScaleX;
                    ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).ScaleY = ScaleY;
                }
            }
        }
        public static void canvas_move(Canvas leftCanvas, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //Console.WriteLine(e.GetPosition(leftCanvas).X + ":" + e.GetPosition(leftCanvas).Y);
                double addX = e.GetPosition(leftCanvas).X - lastMoveX;
                double addY = e.GetPosition(leftCanvas).Y - lastMoveY;
                double oldCenterX = ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).CenterX;
                double oldCenterY = ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).CenterY;
                /* Console.WriteLine((2 * oldX - e.GetPosition(leftCanvas).X) + ":" + (2 * oldY - e.GetPosition(leftCanvas).Y));
                 Console.WriteLine("！！！" );*/
                ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).CenterX = oldCenterX - addX;
                ((ScaleTransform)(((TransformGroup)(((UIElement)(leftCanvas)).RenderTransform)).Children[0])).CenterY = oldCenterY - addY;
                lastMoveX = e.GetPosition(leftCanvas).X;
                lastMoveY = e.GetPosition(leftCanvas).Y;
            }
        }

        public static void move(Canvas canves,System.Windows.Point p)
        {
            canves.Children.Clear();

            double dx = p.X * m_d_zoomfactor;
            double dy = p.Y * m_d_zoomfactor;
            
            for (int i = 0; i < wellList.Rows.Count; i++)
            {
                double x = Math.Round((Convert.ToDouble(wellList.Rows[i][2]) + dx) , 1);
                double y = Math.Round((Convert.ToDouble(wellList.Rows[i][1]) + dy), 1);

                wellList.Rows[i][1] = y;
                wellList.Rows[i][2] = x;
            }
        }

        public static void DrawPoint(Canvas canves)
        {
    
            foreach (DataRow pointRow in wellList.Rows)
            {
                Shape circle = new Ellipse() { Fill = System.Windows.Media.Brushes.Red, Height = 3, Width = 3 };
               
                Canvas.SetLeft(circle, Convert.ToDouble(pointRow[2]));
                Canvas.SetTop(circle, Convert.ToDouble(pointRow[1]));
                canves.Children.Add(circle);
                TextBlock block = new TextBlock();
                block.Text = pointRow[0].ToString();
                block.FontSize = 5;
                Canvas.SetLeft(block, Convert.ToDouble(pointRow[2]));
                Canvas.SetTop(block, Convert.ToDouble(pointRow[1]));
                canves.Children.Add(block);
            }

            DrawTrangle(canves);
        }

        static void DrawingLines(Canvas canves, List<TriEdge> edges)
        {
            foreach (TriEdge e in edges)
            {
                
                Line line = new Line();
                line.Stroke = System.Windows.Media.Brushes.Blue;
                line.X1 = e.startPoint.point.X;
                line.X2 = e.endPoint.point.X;
                line.Y1 = e.startPoint.point.Y;
                line.Y2 = e.endPoint.point.Y;
                line.StrokeThickness = 1;
                line.HorizontalAlignment = HorizontalAlignment.Left;
                line.VerticalAlignment = VerticalAlignment.Center;
                canves.Children.Add(line);
            }
        }

        public static void DrawLine(Canvas canves, MouseButtonEventArgs e)
        {
            System.Windows.Point point = new System.Windows.Point();

            point.X = e.GetPosition(canves).X;
            point.Y = e.GetPosition(canves).Y;

            if (pointList.Count > 0)
            {
                Line line = new Line();
                line.Stroke = System.Windows.Media.Brushes.Red;
                line.X1 = pointList[pointList.Count - 1].X;
                line.X2 = e.GetPosition(canves).X;
                line.Y1 = pointList[pointList.Count - 1].Y;
                line.Y2 = e.GetPosition(canves).Y;
                line.HorizontalAlignment = HorizontalAlignment.Left;
                line.VerticalAlignment = VerticalAlignment.Center;
                canves.Children.Add(line);
            }
            pointList.Add(point);
        }
        public static void DrawLine(Canvas canves, MouseButtonEventArgs e, SolidColorBrush scb)
        {
            if ( null==scb)
            {
                scb = System.Windows.Media.Brushes.Black;// Red;
            }
            System.Windows.Point point = new System.Windows.Point();

            point.X = e.GetPosition(canves).X;
            point.Y = e.GetPosition(canves).Y;

            if (pointList.Count > 0)
            {
                Line line = new Line();
                line.Stroke = scb;
                line.X1 = pointList[pointList.Count - 1].X;
                line.X2 = e.GetPosition(canves).X;
                line.Y1 = pointList[pointList.Count - 1].Y;
                line.Y2 = e.GetPosition(canves).Y;
                line.HorizontalAlignment = HorizontalAlignment.Left;
                line.VerticalAlignment = VerticalAlignment.Center;
                canves.Children.Add(line);
            }
            pointList.Add(point);
        }

        public static void EreaseLine(Canvas canves, MouseButtonEventArgs e)
        {
            foreach (System.Windows.Point point in pointList)
            {
                HitTestResult result = VisualTreeHelper.HitTest(canves, point);
                
                if (result != null)
                {
                    canves.Children.Remove(result.VisualHit as Line);
                }
            }
             pointList.Clear();
        }


        public static void test(Canvas canves)
        {
            Visual childVisual = (Visual)VisualTreeHelper.GetChild(canves, 3);

            TextBlock te = (TextBlock)childVisual;
            double x = Canvas.GetLeft(te);
            double y = Canvas.GetTop(te);
            System.Console.WriteLine(x);
            System.Console.WriteLine(y);
            System.Console.WriteLine(te.Text);
            //System.Console.WriteLine(childVisual.GetValue(Text));
            //childVisual.GetValue(X);
        }


        public static void printPointInPy()
        {
            foreach (DataRow row in wellList.Rows)
            {
                System.Windows.Point pt = new System.Windows.Point(Convert.ToDouble(row[2].ToString()), Convert.ToDouble(row[1].ToString()) + zoomy);

                if (isInRegion(pt, pointList))
                {
                    System.Console.WriteLine(row[0].ToString());
                }
            }
        }

        //判断点在线的一边
        public static int isLeft(System.Windows.Point P0, System.Windows.Point P1, System.Windows.Point P2)
        {
            int abc = (int)((P1.X - P0.X) * (P2.Y - P0.Y) - (P2.X - P0.X) * (P1.Y - P0.Y));
            return abc;
        } 

        //判断点pnt是否在region内主程序
        public static bool isInRegion(System.Windows.Point pnt, List<System.Windows.Point> region)
        {
            int wn = 0, j = 0; //wn 计数器 j第二个点指针
            for (int i = 0; i < region.Count; i++)
            {
                //开始循环
                if (i == region.Count - 1)
                {
                    j = 0;//如果 循环到最后一点 第二个指针指向第一点
                }
                else
                {
                    j = j + 1; //如果不是 ，则找下一点
                }

                if (region[i].Y <= pnt.Y) // 如果多边形的点 小于等于 选定点的 Y 坐标
                {
                    if (region[j].Y > pnt.Y) // 如果多边形的下一点 大于于 选定点的 Y 坐标
                    {
                        if (isLeft(region[i], region[j], pnt) > 0)
                        {
                            wn++;
                        }
                    }
                }
                else
                {
                    if (region[j].Y <= pnt.Y)
                    {
                        if (isLeft(region[i], region[j], pnt) < 0)
                        {
                            wn--;
                        }
                    }
                }
            }
            if (wn == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static List<TriPoint> triPoint = new List<TriPoint>();
        static List<TriEdge> triEdgeTemp = new List<TriEdge>();
        static List<Triangle> listTriangle = new List<Triangle>();
        static List<Triangle> cal_listTriangle = new List<Triangle>();
        static List<PointF> circumPoints = new List<PointF>();
        static List<PointF> points = new List<PointF>();

        private static void FirstTriangle()
        {
            for (int i = 0; i < points.Count; i++)
            {
                TriPoint p = new TriPoint();
                p.point = points[i];
                p.ID = i + 1;
                triPoint.Add(p);
            }
            int index = 0;
            double length = double.MaxValue;
            foreach (TriPoint p1 in triPoint)
            {
                double temp = TriEdge.LengthSquare(triPoint[0], p1);
                if (temp != 0 && temp < length)
                {
                    index = p1.ID;
                    length = temp;
                }
            }
            TriPoint point1, point2, point3;
            point1 = triPoint[0];
            point3 = triPoint[index - 1];
            if (point1 != null && point3 != null)
            {
                TriEdge edge = new TriEdge(point1, point3);
                point2 = TriEdge.GetBestPoint(edge, triPoint);
                if (point2 != null)
                {
                    TriEdge triEdge1 = new TriEdge(point1, point2);
                    TriEdge triEdge2 = new TriEdge(point2, point3);
                    TriEdge triEdge3 = new TriEdge(point3, point1);
                    Triangle angle = new Triangle(point1, point2, point3);
                    angle.edge1 = triEdge1;
                    angle.edge2 = triEdge2;
                    angle.edge3 = triEdge3;
                    triEdge1.leftTriangle = angle;
                    triEdge2.leftTriangle = angle;
                    triEdge3.leftTriangle = angle;
                    triEdgeTemp.Add(triEdge1);
                    triEdgeTemp.Add(triEdge2);
                    triEdgeTemp.Add(triEdge3);
                    listTriangle.Add(angle);
                }
                else
                {
                    edge = new TriEdge(point3, point1);
                    point2 = TriEdge.GetBestPoint(edge, triPoint);
                    TriEdge triEdge1 = new TriEdge(point3, point2);
                    TriEdge triEdge2 = new TriEdge(point2, point1);
                    TriEdge triEdge3 = new TriEdge(point1, point3);
                    Triangle angle = new Triangle(point3, point2, point1);
                    angle.edge1 = triEdge1;
                    angle.edge2 = triEdge2;
                    angle.edge3 = triEdge3;
                    triEdge1.leftTriangle = angle;
                    triEdge2.leftTriangle = angle;
                    triEdge3.leftTriangle = angle;
                    triEdgeTemp.Add(triEdge1);
                    triEdgeTemp.Add(triEdge2);
                    triEdgeTemp.Add(triEdge3);
                    listTriangle.Add(angle);
                }
            }
        }

        private static void BuildDelaunay()
        {
            while (triEdgeTemp.Count != 0)
            {
                TriEdge edge = triEdgeTemp[0];
                TriPoint point2 = new TriPoint();
                point2 = TriEdge.GetBestPoint(edge, triPoint);
                if (point2 != null)
                {
                    Triangle triangle = new Triangle(edge.startPoint, point2, edge.endPoint);
                    TriEdge edge1 = new TriEdge(edge.startPoint, point2);
                    TriEdge edge2 = new TriEdge(point2, edge.endPoint);
                    TriEdge edge3 = new TriEdge(edge.endPoint, edge.startPoint);
                    edge1.leftTriangle = triangle;
                    edge2.leftTriangle = triangle;
                    edge3.leftTriangle = triangle;
                    triangle.edge1 = edge1;
                    triangle.edge2 = edge2;
                    triangle.edge3 = edge3;
                    edge3.rightTriangle = edge.leftTriangle;
                    edge.rightTriangle = edge3.leftTriangle;
                    triEdgeTemp.Remove(edge);
                    listTriangle.Add(triangle);
                    TriEdge edgeTemp = new TriEdge();
                    edgeTemp.startPoint = edge1.endPoint;
                    edgeTemp.endPoint = edge1.startPoint;
                    TriEdge sameEdge = TriEdge.FindSameEdge(triEdgeTemp, edgeTemp);
                    if (sameEdge == null)
                    {
                        triEdgeTemp.Add(edge1);
                    }
                    else
                    {
                        triEdgeTemp.Remove(sameEdge);
                    }
                    edgeTemp.startPoint = edge2.endPoint;
                    edgeTemp.endPoint = edge2.startPoint;
                    sameEdge = TriEdge.FindSameEdge(triEdgeTemp, edgeTemp);
                    if (sameEdge == null)
                    {
                        triEdgeTemp.Add(edge2);
                    }
                    else
                    {
                        triEdgeTemp.Remove(sameEdge);
                    }
                }
                else
                {
                    triEdgeTemp.Remove(edge);
                }
            }
        }

        public static void DrawTrangle(Canvas canves)
        {
            FirstTriangle();
            BuildDelaunay();
            List<TriEdge> edges = new List<TriEdge>();
            edges = Triangle.GetEdges(listTriangle);
            TriEdge.GetUniqueEdges(edges);
            DrawingLines(canves,edges);
            //listTriangle.Clear();
            triPoint.Clear();
        }

        public static double Cal_Well_Area()
        {
            foreach(DataRow row in temp_wellList.Rows)
            {
                foreach (Triangle trangle in listTriangle)
                {
                    float a = trangle.point1.point.X;
                    
                    float b = trangle.point1.point.Y;

                    float c = trangle.point2.point.X;
                    float d = trangle.point2.point.Y;

                    float e = trangle.point3.point.X;
                    float f = trangle.point3.point.Y;

                    if ((float.Parse(row[2].ToString()) == a || float.Parse(row[2].ToString()) == c || float.Parse(row[2].ToString()) == e) && (float.Parse(row[1].ToString()) == b || float.Parse(row[1].ToString()) == d || float.Parse(row[1].ToString()) == f))
                    {
                        trangle.trangle_name = row[0].ToString();
                        cal_listTriangle.Add(trangle);
                    }
                }
            }
            return 0;
        }

        public static void Cal_Well_Square(string wellnum)
        {
            double temp_area = 0, area = 0;
            double capacity = 0;
            DataTable hha = new DataTable();

            string sql = "select jh,yxhd,kxd,hqbhd from daa074 where jh = '"+wellnum+"'";
        
            hha = GetDataAsDataTable.GetDataReasult(sql);
            foreach (Triangle trangle in cal_listTriangle)
            {
                
                if (wellnum == trangle.trangle_name)
                {
                    double a = trangle.point1.point.X;
                    double b = trangle.point1.point.Y;

                    double c = trangle.point2.point.X;
                    double d = trangle.point2.point.Y;

                    double e = trangle.point3.point.X;
                    double f = trangle.point3.point.Y;

                    temp_area = Math.Abs((a * d + b * c + c * f - a * f - b * c - d * e) / 2);

                    area += temp_area;
                }
            }

            foreach(DataRow row in hha.Rows)
            {
                double bhd = Convert.ToDouble(row[3].ToString());
                double kxd = Convert.ToDouble(row[2].ToString());
                double yxhd = Convert.ToDouble(row[1].ToString());
                double temp_cap = Cal_Capacity( 1, 1, 1, 1, 10, 10);
                capacity += temp_cap;
            }

            DataRow row1 = result.NewRow();
            row1[0] = wellnum;
            row1[1] = area;
            row1[2] = capacity;

            result.Rows.Add(row1);
            result.AcceptChanges();
        }

        public static double Cal_Area()
        {
            double temp_area = 0, area = 0;

            foreach (Triangle trangle in listTriangle)
            {
                double a = trangle.point1.point.X;
                double b = trangle.point1.point.Y;

                double c = trangle.point2.point.X;
                double d = trangle.point2.point.Y;

                double e = trangle.point3.point.X;
                double f = trangle.point3.point.Y;

                temp_area = Math.Abs((a * d + b * c + c * f - a * f - b * c - d * e) / 2);

                area += temp_area;
            }
            return area;
        }

        public static double Cal_Capacity(double S,double h,double fai,double s0,double ro,double B)
        {
            //double A,采用容积法计算储量，具体公式如下：  N=0.01AohφSoiρoa/Boi 
            // 式中：N——石油地质储量，104t  
            // Ao——含油面积，km2 
            // h——有效厚度，m 
            // φ——有效孔隙度，% 
            // Soi——含油饱和度，%  
            // ρoa——地面原油密度，g/cm3  t/m3
            // Boi——体积系数 
            double N = 0;
            N = 100 * S * h * fai * s0 * (ro / B);// 100 是%%=0.01*0.01，0.01是100*100 （%*100）
            return N;
        }

        public static double Cal_Phase(double A, double S, double h, double fai, double s0, double ro, double B)
        {
            double N = 0;
            N = 100 * A * S * h * fai * s0 * (ro / B);
            return N;
        }
    }
}
