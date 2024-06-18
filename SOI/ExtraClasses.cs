using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Drawing;

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
        public void pointsfromCenter()
        {
            points[0].x = this.c.x - a / 2;
            points[0].y = this.c.y - a / 2;

            points[1].x = this.c.x + a / 2;
            points[1].y = this.c.y - a / 2;

            points[2].x = this.c.x - a / 2;
            points[2].y = this.c.y + a / 2;

            points[3].x = this.c.x + a / 2;
            points[3].y = this.c.y + a / 2;
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

    public class Model
    {
        private readonly MLContext mlContext;
        private ITransformer model;
        private string FilePath = "";
        private string DataFilePath => FilePath + "data.json";
        private string ModelFilePath => FilePath + "model.zip";

        public int version = 1; 

        public Model()
        {
            FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SoI");
            LoadModel();
        }

        public void Train()
        {

        }

        public Points Use(Bitmap image)
        {
            Points result = new Points(4);

                Random rand = new Random();
                int a = Math.Max(1, rand.Next(Math.Min(image.Width, image.Height)));
                int x = rand.Next(image.Width - a);
                int y = rand.Next(image.Height - a);

            result.a = a;
            result.c.x = x + a / 2;
            result.c.y = y + a / 2;
            result.pointsfromCenter();

            return result;
        }

        public void LoadModel()
        {

        }

        private static List<ImageMetadata> LoadImageData(string jsonFilePath)
        {

        }
    }
}
