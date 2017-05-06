using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 数模建模.Delaunay
{
    class TriEdge
    {
        public TriPoint startPoint, endPoint;
        public Triangle leftTriangle, rightTriangle;
       // private double length;
        public double Length
        {
            get
            {
                return Math.Sqrt(EdgeLengthSquare());
            }
        }
        public TriEdge(TriPoint StartPoint, TriPoint EndPoint)
        {
            this.startPoint = StartPoint;
            this.endPoint = EndPoint;
        }

        public TriEdge()
        {

        }

        public bool RightOfLine(TriPoint p3)
        {
            double temp = (p3.point.X - startPoint.point.X) * (endPoint.point.Y - startPoint.point.Y) - (p3.point.Y - startPoint.point.Y) * (endPoint.point.X - startPoint.point.X);
            if (temp > 0)
                return true;
            else
                return false;
        }

        public double EdgeLengthSquare()
        {
            double x1 = startPoint.point.X;
            double y1 = startPoint.point.Y;
            double x2 = endPoint.point.X;
            double y2 = endPoint.point.Y;
            double lengtSquare = (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
            return lengtSquare;
        }

        public static double LengthSquare(TriPoint point1, TriPoint point2) //(x1-x2)*(x1-x2)*(y1-y2)*(y1-y2)
        {
            double x1 = point1.point.X;
            double y1 = point1.point.Y;
            double x2 = point2.point.X;
            double y2 = point2.point.Y;
            return (x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2);
        }

        public double CosValue(TriPoint point3)   //cosx=(a*a+b*b-c*c)/2*a*b
        {
            double a = LengthSquare(startPoint, point3);
            double b = LengthSquare(endPoint, point3);
            double c = LengthSquare(startPoint, endPoint);
            double cos = (a + b - c) / (2 * Math.Sqrt(a) * Math.Sqrt(b));
            return cos;
        }


        public static int GetPointID(TriEdge edge, List<TriPoint> triPoint)
        {
            double temp = 1;
            int tempID = 0;
            foreach (TriPoint p in triPoint)
            {
                if (edge.RightOfLine(p))
                {
                    if (edge.CosValue(p) < temp)
                    {
                        tempID = p.ID;
                        temp = edge.CosValue(p);
                    }
                }
            }
            return tempID;
        }

        public static TriEdge FindSameEdge(List<TriEdge> listEdge, TriEdge edge)
        {
            foreach (TriEdge e in listEdge)
            {
                if (e.startPoint == edge.startPoint && e.endPoint == edge.endPoint)
                    return e;
            }
            return null;
        }

        public static TriPoint GetBestPoint(TriEdge edge, List<TriPoint> triPoint)
        {
            if (GetPointID(edge, triPoint) == 0)
                return null;
            return triPoint[GetPointID(edge, triPoint) - 1];
        }

        public static TriEdge FindOppsiteEdge(List<TriEdge> listEdge, TriEdge edge)
        {
            foreach (TriEdge e in listEdge)
            {
                if (e.startPoint == edge.endPoint && e.endPoint == edge.startPoint)
                    return e;
            }
            return null;
        }

        public static void GetUniqueEdges(List<TriEdge> edges)
        {
            List<TriEdge> edgesTemp = new List<TriEdge>();
            foreach (TriEdge edge in edges)
            {
                TriEdge edgeTemp = FindOppsiteEdge(edges, edge);
                if (edgeTemp != null && FindOppsiteEdge(edgesTemp, edgeTemp) == null)
                {
                    edgesTemp.Add(edgeTemp);
                }
            }
            foreach (TriEdge e in edgesTemp)
            {
                edges.Remove(e);
            }
        }
    }
}
