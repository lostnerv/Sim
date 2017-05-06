using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace 数模建模.Delaunay
{
    class TriPoint
    {
        public PointF point;
        public int ID;
        public TriPoint()
        {
            point = new Point();
        }

        public static int GetYMinPoint(List<TriPoint> points)
        {
            int id = 0;
            double yMin = points[0].point.Y;
            for (int i = 0; i < points.Count; i++)
            {
                if (yMin > points[i].point.Y)
                {
                    id = i;
                    yMin = points[i].point.Y;
                }
            }
            return id;
        }

        public static void GetAssortedPoints(List<TriPoint> points)
        {
            List<TriPoint> pointsTemp = new List<TriPoint>();
            int index = GetYMinPoint(points);
            TriPoint point = points[index];
            points.Remove(point);
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count - 1; j++)
                {
                    if (GetCos(point, points[i]) > GetCos(point, points[j]))
                    {
                        TriPoint p = points[i];
                        points[i] = points[j];
                        points[j] = p;
                    }
                }
            }
            points.Insert(0, point);
        }

        public static double GetCos(TriPoint p1, TriPoint p2)
        {
            double length = Math.Sqrt(TriEdge.LengthSquare(p1, p2));
            return (p2.point.X - p1.point.X) / length;
        }

        public static bool PointsEqual(PointF point1, PointF point2)
        {
            if (point1.X == point2.X && point1.Y == point2.Y)
                return true;
            return
                false;
        }
    }
}
