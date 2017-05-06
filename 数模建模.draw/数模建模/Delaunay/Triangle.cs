using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace 数模建模.Delaunay
{
    class Triangle
    {
        public TriPoint point1, point2, point3;
        public TriEdge edge1, edge2, edge3;
        public string trangle_name;
        //private TriPoint circumCenter;
        public TriPoint CircumCenter
        {
            get
            {
                TriPoint point = new TriPoint();
                float x1 = point1.point.X;
                float y1 = point1.point.Y;
                float x2 = point2.point.X;
                float y2 = point2.point.Y;
                float x3 = point3.point.X;
                float y3 = point3.point.Y;
                point.point.X = ((y2 - y1) * (y3 * y3 - y1 * y1 + x3 * x3 - x1 * x1) - (y3 - y1) * (y2 * y2 - y1 * y1 + x2 * x2 - x1 * x1)) / (2 * (x3 - x1) * (y2 - y1) - 2 * ((x2 - x1) * (y3 - y1)));
                point.point.Y = ((x2 - x1) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1) - (x3 - x1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1)) / (2 * (y3 - y1) * (x2 - x1) - 2 * ((y2 - y1) * (x3 - x1)));
                return point;
            }
            set
            {

            }
        }
        public Triangle()
        {

        }

        public Triangle(TriPoint p1, TriPoint p2, TriPoint p3)
        {
            this.point1 = p1;
            this.point2 = p2;
            this.point3 = p3;
        }

        public PointF CalCenter(Triangle triangle)
        {
            PointF circumPoint = new Point();
            float x1 = triangle.point1.point.X;
            float y1 = triangle.point1.point.Y;
            float x2 = triangle.point2.point.X;
            float y2 = triangle.point2.point.Y;
            float x3 = triangle.point3.point.X;
            float y3 = triangle.point3.point.Y;
            circumPoint.X = ((y2 - y1) * (y3 * y3 - y1 * y1 + x3 * x3 - x1 * x1) - (y3 - y1) * (y2 * y2 - y1 * y1 + x2 * x2 - x1 * x1)) / (2 * (x3 - x1) * (y2 - y1) - 2 * ((x2 - x1) * (y3 - y1)));
            circumPoint.Y = ((x2 - x1) * (x3 * x3 - x1 * x1 + y3 * y3 - y1 * y1) - (x3 - x1) * (x2 * x2 - x1 * x1 + y2 * y2 - y1 * y1)) / (2 * (y3 - y1) * (x2 - x1) - 2 * ((y2 - y1) * (x3 - x1)));
            return circumPoint;
        }

        public List<TriPoint> points
        {
            get
            {
                List<TriPoint> points = new List<TriPoint>();
                points.Add(point1);
                points.Add(point2);
                points.Add(point3);
                return points;
            }
        }

        public bool ContainPoint(TriPoint p)
        {
            if (point1 == p || point2 == p || point3 == p)
                return true;
            else
                return false;
        }

        public static TriEdge GetEdge(List<TriEdge> edges, Triangle angle)
        {
            foreach (TriEdge e in edges)
            {
                if (angle.edge1 == e)
                    return angle.edge1;
                if (angle.edge2 == e)
                    return angle.edge2;
                if (angle.edge3 == e)
                    return angle.edge3;
            }
            return null;
        }

        public static List<TriEdge> GetEdges(List<Triangle> triangles)
        {
            List<TriEdge> edges = new List<TriEdge>();
            foreach (Triangle angle in triangles)
            {
                edges.Add(angle.edge1);
                edges.Add(angle.edge2);
                edges.Add(angle.edge3);
            }
            if (edges.Count == 0)
                return null;
            return edges;
        }
    }
}
