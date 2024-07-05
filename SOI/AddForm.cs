using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
// NUGET -> >_Install-Package Newtonsoft.Json


namespace SOI
{
    public partial class AddForm : Form
    {
        private int imagesCount = 0;

        private bool isDrawing = false;

        private CustomPoint startPoint;
        private CustomPoint endPoint;

        private Points points;

        private Bitmap originalImage;
        private Bitmap tempImage;

        public AddForm()
        {
            InitializeComponent();
            points = new Points(4);
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            if (inputImageBox.Image != null)
                originalImage = new Bitmap(inputImageBox.Image);

            imagesCount = jsonLength();
            resetPoints();
            updatePointUI();
        }

        private void clickInputImage(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    originalImage = new Bitmap(filePath);
                    inputImageBox.Image = originalImage;
                    tempImage = (Bitmap)originalImage.Clone();

                    OpenTheImage(originalImage);
                }
            }
        }

        private void OpenTheImage(Bitmap input)
        {
            resetPoints();
            updatePointUI();

            outputH.Text = input.Height.ToString() + "px";
            outputW.Text = input.Width.ToString() + "px";
        }

        private CustomPoint setPoint(int x, int y)
        {
            if (inputImageBox.Image == null) return new CustomPoint(0, 0);

            float imageAspect = (float)originalImage.Width / originalImage.Height;
            float boxAspect = (float)inputImageBox.ClientSize.Width / inputImageBox.ClientSize.Height;

            int displayedWidth, displayedHeight;
            if (imageAspect > boxAspect)
            {
                displayedWidth = inputImageBox.ClientSize.Width;
                displayedHeight = (int)(inputImageBox.ClientSize.Width / imageAspect);
            }
            else
            {
                displayedWidth = (int)(inputImageBox.ClientSize.Height * imageAspect);
                displayedHeight = inputImageBox.ClientSize.Height;
            }

            int offsetX = (inputImageBox.ClientSize.Width - displayedWidth) / 2;
            int offsetY = (inputImageBox.ClientSize.Height - displayedHeight) / 2;

            int translatedX = (int)((x - offsetX) * (float)originalImage.Width / displayedWidth);
            int translatedY = (int)((y - offsetY) * (float)originalImage.Height / displayedHeight);



            return new CustomPoint(
                Math.Max(0, Math.Min(translatedX, originalImage.Width)), 
                Math.Max(0, Math.Min(translatedY, originalImage.Height)));
        }

        private void inputImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                startPoint = setPoint(e.X, e.Y);
            }
        }

        private void inputImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (isDrawing)
                {
                    endPoint = setPoint(e.X, e.Y);
                    drawRectangle();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void inputImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                isDrawing = false;
                endPoint = setPoint(e.X, e.Y);

                int x1 = startPoint.x, x2 = endPoint.x, y1 = startPoint.y, y2 = endPoint.y;

                if (x1 > x2)
                {
                    startPoint.x = x2;
                    endPoint.x = x1;
                }

                if (y1 > y2)
                {
                    startPoint.y = y2;
                    endPoint.y = y1;
                }

                int width = Math.Abs(startPoint.x - endPoint.x), height = Math.Abs(startPoint.y - endPoint.y);

                startPoint.x += Math.Max((width - height) / 2, 0);
                endPoint.x -= Math.Max((width - height) / 2, 0);
                startPoint.y += Math.Max((height - width) / 2, 0);
                endPoint.y -= Math.Max((height - width) / 2, 0);

                drawZone();
                updatePointUI();
            }
        }

        private void resetPoints()
        {
            startPoint = new CustomPoint(0, 0);
            endPoint = new CustomPoint(originalImage.Width, originalImage.Height);
            updatePointUI();
        }

        private void updatePointUI()
        {
            points.a = Math.Min(Math.Abs(startPoint.x - endPoint.x), Math.Abs(startPoint.y - endPoint.y));
            points.c.x = (startPoint.x + endPoint.x) / 2;
            points.c.y = (startPoint.y + endPoint.y) / 2;

            points.points[0].x = points.c.x - points.a / 2;
            points.points[0].y = points.c.y - points.a / 2;

            points.points[1].x = points.c.x + points.a / 2;
            points.points[1].y = points.c.y - points.a / 2;

            points.points[2].x = points.c.x - points.a / 2;
            points.points[2].y = points.c.y + points.a / 2;

            points.points[3].x = points.c.x + points.a / 2;
            points.points[3].y = points.c.y + points.a / 2;

            outputXC.Text = points.c.x.ToString() + "px";
            outputYC.Text = points.c.y.ToString() + "px";
            outputA.Text = points.a.ToString() + "px";
            outputX1.Text = points.points[0].x.ToString() + "px";
            outputY1.Text = points.points[0].y.ToString() + "px";
            outputX2.Text = points.points[1].x.ToString() + "px";
            outputY2.Text = points.points[1].y.ToString() + "px";
            outputX3.Text = points.points[2].x.ToString() + "px";
            outputY3.Text = points.points[2].y.ToString() + "px";
            outputX4.Text = points.points[3].x.ToString() + "px";
            outputY4.Text = points.points[3].y.ToString() + "px";

            outputImgCount.Text = imagesCount.ToString();
        }

        private int jsonLength()
        {
            string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SoI");
            string metadataFileName = "data.json";
            string metadataPath = Path.Combine(directoryPath, metadataFileName);

            if (File.Exists(metadataPath))
            {
                string json = File.ReadAllText(metadataPath);
                List<ImageMetadata> metadataList = JsonConvert.DeserializeObject<List<ImageMetadata>>(json);

                return metadataList.Count;
            }

            return 0;
        }

        private void drawRectangle()
        {
            // Dispose of the previous tempImage to free memory
            tempImage?.Dispose();

            // Create a new clone of the original image for drawing
            tempImage = new Bitmap(originalImage.Width, originalImage.Height);

            using (Graphics g = Graphics.FromImage(tempImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                g.DrawImage(originalImage, 0, 0);
                try
                {
                    g.DrawRectangle(
                        Pens.Blue,
                        Math.Min(startPoint.x, endPoint.x),
                        Math.Min(startPoint.y, endPoint.y),
                        Math.Abs(endPoint.x - startPoint.x),
                        Math.Abs(endPoint.y - startPoint.y)
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error drawing rectangle: " + ex.Message);
                }
            }

            inputImageBox.Image = tempImage;
        }

        private void drawZone()
        {
            // Dispose of the previous tempImage to free memory
            tempImage?.Dispose();

            // Create a new clone of the original image for drawing
            tempImage = new Bitmap(originalImage.Width, originalImage.Height);

            using (Graphics g = Graphics.FromImage(tempImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                g.DrawImage(originalImage, 0, 0);
                try
                {
                    int left = Math.Min(startPoint.x, endPoint.x);
                    int top = Math.Min(startPoint.y, endPoint.y);
                    int bottom = Math.Max(startPoint.y, endPoint.y);
                    int right = Math.Max(startPoint.x, endPoint.x);

                    Color semiTransparentBlack = Color.FromArgb(128, 0, 0, 0);
                    using (Brush semiTransparentBrush = new SolidBrush(semiTransparentBlack))
                    {
                        g.FillRectangle(semiTransparentBrush, new Rectangle(0, 0, tempImage.Width, top));
                        g.FillRectangle(semiTransparentBrush, new Rectangle(0, bottom, tempImage.Width, tempImage.Height - bottom));
                        g.FillRectangle(semiTransparentBrush, new Rectangle(0, top, left, bottom - top));
                        g.FillRectangle(semiTransparentBrush, new Rectangle(right, top, tempImage.Width - right, bottom - top));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error drawing zone: " + ex.Message);
                }
            }

            inputImageBox.Image = tempImage;
        }

        private string GenerateRandomNumberBeforeDate()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 10000); // Example range for the random number

            string prefix = $"{randomNumber}_{DateTime.Now:yyyyMMdd_HHmmss}";

            return prefix;
        }


        private void addImageClick(object sender, EventArgs e)
        {
            try
            {

                if (originalImage != null)
                {
                    string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SoI");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    string imageFileName = GenerateRandomNumberBeforeDate() + ".png";
                    string metadataFileName = "data.json";

                    string imagePath = Path.Combine(directoryPath, imageFileName);

                    // Determine which image to save based on the current isImageFromApi flag
                    tempImage?.Dispose();

                    // Create a new clone of the original image for drawing
                    tempImage = new Bitmap(originalImage.Width, originalImage.Height);

                    using (Graphics g = Graphics.FromImage(tempImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

                        g.DrawImage(originalImage, 0, 0);
                    }

                    inputImageBox.Image = tempImage;

                    // Save the image
                    tempImage.Save(imagePath, ImageFormat.Png);

                    // Save metadata
                    ImageMetadata metadata = new ImageMetadata
                    {
                        FileName = imageFileName,
                        W = originalImage.Width,
                        H = originalImage.Height,
                        c = new CustomPoint(points.c),
                        a = points.a
                    };

                    string metadataPath = Path.Combine(directoryPath, metadataFileName);
                    List<ImageMetadata> metadataList;

                    if (File.Exists(metadataPath))
                    {
                        string json = File.ReadAllText(metadataPath);

                        metadataList = JsonConvert.DeserializeObject<List<ImageMetadata>>(json);

                        metadataList.Add(metadata);
                    }
                    else
                    {
                        metadataList = new List<ImageMetadata> { metadata };
                    }

                    string updatedJson = JsonConvert.SerializeObject(metadataList, Formatting.Indented);
                    File.WriteAllText(metadataPath, updatedJson);

                    imagesCount++;
                    resetPoints();

                    MessageBox.Show($"[i][E001] Image and metadata saved successfully at:\n{imagePath}\n{metadataPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("[!][E003] Please select an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"[!][E004] An error occurred while saving the image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonAPIClick(object sender, EventArgs e)
        {
            try
            {
                Random random = new Random();
                int size = random.Next(200, 1800); // Reasonable size for images

                List<string> imageApis = new List<string>
                {
                    "https://picsum.photos/" + size,
                    "https://loremflickr.com/" + size + "/" + size
                };

                using (WebClient webClient = new WebClient())
                {
                    byte[] imageBytes = webClient.DownloadData(imageApis[random.Next(imageApis.Count)]);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        // Load the image from API into originalImageFromApi
                        originalImage = new Bitmap(ms); 
                        inputImageBox.Image = (Bitmap)originalImage.Clone();
                        tempImage = (Bitmap)originalImage.Clone();
                        

                        OpenTheImage(originalImage);
                    }
                }
            }
            catch (WebException webEx)
            {
                MessageBox.Show("Failed to load image from API: " + webEx.Message, "Web Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        /*
        Bitmap inputImage = (Bitmap)inputImageBox.Image;

        int width = Math.Abs(startPoint.x - endPoint.x), height = Math.Abs(startPoint.y - endPoint.y);

        Bitmap outputImage = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(outputImage))
        {
            g.DrawImage(inputImage,
                new Rectangle(0, 0, width, height),
                new Rectangle(
                    startPoint.x,
                    startPoint.y,
                    width,
                    height
                    ),
                GraphicsUnit.Pixel);
        }

        inputImageBox.Image = outputImage;
        */
    }
}
