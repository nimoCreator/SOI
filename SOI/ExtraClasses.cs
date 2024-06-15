using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOI
{

    public struct Point
    {
        public int x, y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public Point(Point p)
        {
            x = p.x;
            y = p.y;
        }
    }

    public struct Points
    {
        public int a;
        public Point c;
        public Point[] points;

        // Constructor to initialize the array
        public Points(int numPoints)
        {
            a = 0;
            c = new Point { x = 0, y = 0 };
            points = new Point[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                points[i] = new Point { x = 0, y = 0 };
            }
        }
    }

    public class ImageMetadata
    {
        public string FileName { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public Point c { get; set; }
        public int a { get; set; }
    }

}
