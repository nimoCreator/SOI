using System;
using System.Drawing;
using System.Windows.Forms;

namespace SOI
{


    public partial class MainForm : Form
    {
        Points points;

        Model model = new Model();

        public MainForm()
        {
            InitializeComponent();


            // Initialize points with an array of 4 elements
            points = new Points(4);
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
        }

        private void buttonTrainClick(object sender, EventArgs e)
        {
            Form trainForm = new trainForm(model);
        }

        private void buttonAddImageClick(object sender, EventArgs e)
        {
            // open add form

            AddForm addForm = new AddForm();
            addForm.ShowDialog();

        }

        private void inputImageClick(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    Bitmap inputImage = new Bitmap(filePath);
                    inputImageBox.Image = inputImage;

                    outputW.Text = inputImage.Width.ToString() + "px";
                    outputH.Text = inputImage.Height.ToString() + "px";

                    // Process the image
                    ProcessImage(inputImage);
                }
            }
        }

        private void outputImageClick(object sender, EventArgs e)
        {
            if (outputImageBox.Image != null)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PNG Image|*.png";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;
                        outputImageBox.Image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }

        private Points AI(Bitmap inputImage)
        {
            return model.Use(inputImage);
        }

        private void ProcessImage(Bitmap inputImage)
        {
            points = AI(inputImage);

            Bitmap outputImage = new Bitmap(points.a, points.a);
            using (Graphics g = Graphics.FromImage(outputImage))
            {
                g.DrawImage(inputImage, new Rectangle(0, 0, points.a, points.a), new Rectangle(points.points[0].x, points.points[0].y, points.a, points.a), GraphicsUnit.Pixel);
            }

            outputImageBox.Image = outputImage;

            using (Graphics g = Graphics.FromImage(inputImage))
            {
                Color semiTransparentBlack = Color.FromArgb(128, 0, 0, 0);
                using (Brush semiTransparentBrush = new SolidBrush(semiTransparentBlack))
                {
                    g.FillRectangle(semiTransparentBrush, new Rectangle(0,                  0,                  inputImage.Width,       points.points[0].y));
                    g.FillRectangle(semiTransparentBrush, new Rectangle(0,                  points.points[2].y, inputImage.Width,       inputImage.Height - points.points[3].y));
                    g.FillRectangle(semiTransparentBrush, new Rectangle(0,                  points.points[0].y, points.points[0].x,     points.a));
                    g.FillRectangle(semiTransparentBrush, new Rectangle(points.points[1].x, points.points[1].y, inputImage.Width - points.points[1].x,       points.a));
                }
            }

            outputXC.Text = points.c.x.ToString() + "px";
            outputYC.Text = points.c.y.ToString() + "px";
            outputA.Text  = points.a.ToString() + "px";
            outputX1.Text = points.points[0].x.ToString() + "px";
            outputY1.Text = points.points[0].y.ToString() + "px";
            outputX2.Text = points.points[1].x.ToString() + "px";
            outputY2.Text = points.points[1].y.ToString() + "px";
            outputX3.Text = points.points[2].x.ToString() + "px";
            outputY3.Text = points.points[2].y.ToString() + "px";
            outputX4.Text = points.points[3].x.ToString() + "px";
            outputY4.Text = points.points[3].y.ToString() + "px";
        }
    }
}
