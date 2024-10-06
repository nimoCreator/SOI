using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace SOI
{


    public partial class MainForm : Form
    {
        Points points;

        Model model = new Model();

        BackgroundWorker backgroundWorker;
        bool isBusy = false;

        Bitmap inputImage;

        double[] outputWeights;

        bool displayType = true;

        bool outputReady = false;

        public MainForm()
        {
            InitializeComponent();

            updateUI(null, null);

            // Initialize points with an array of 4 elements
            points = new Points(4);

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        { 
            ProcessImage(new Bitmap(e.Argument as Bitmap));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            isBusy = false;
            addConsoleText("Image processing completed.");
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            
        }

        private void buttonTrainClick(object sender, EventArgs e)
        {
            if(isBusy)
            {
                addConsoleText("Processing in progress, please wait for the operation to finish...");
                return;
            }

            Form trainForm = new trainForm(ref model);
            trainForm.ShowDialog();
        }

        private void buttonAddImageClick(object sender, EventArgs e)
        {
            if (isBusy)
            {
                addConsoleText("Processing in progress, please wait for the operation to finish...");
                return;
            }

            AddForm addForm = new AddForm();
            addForm.ShowDialog();

        }

        private void inputImageClick(object sender, EventArgs e)
        {
            if (isBusy)
            {
                addConsoleText("Processing in progress, please wait for the operation to finish...");
                return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                { 
                    isBusy = true;
                    string filePath = openFileDialog.FileName;
                    inputImage = new Bitmap(filePath);
                    inputImageBox.Image = inputImage;

                    outputW.Text = inputImage.Width.ToString() + "px";
                    outputH.Text = inputImage.Height.ToString() + "px";

                    // Process the image
                    if (!backgroundWorker.IsBusy)
                    {
                        backgroundWorker.RunWorkerAsync(inputImage);
                        addConsoleText("Work begins");
                    }
                    else
                    {
                        addConsoleText("Processing in progress, wait to process another image");
                    }
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

        private Points FindSquare(double[] input, int W, int H)
        {
            // Step 1: Convert the input to a 2D array
            double[,] matrix = new double[H, W];
            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    matrix[i, j] = input[i * W + j] - 0.95; // Adjusting values based on a threshold
                }
            }

            // Step 2: Compute the prefix sum for efficient sum calculation
            double[,] prefixSum = new double[H + 1, W + 1];
            for (int i = 1; i <= H; i++)
            {
                for (int j = 1; j <= W; j++)
                {
                    prefixSum[i, j] = matrix[i - 1, j - 1] +
                                      prefixSum[i - 1, j] +
                                      prefixSum[i, j - 1] -
                                      prefixSum[i - 1, j - 1];
                }
            }

            // Step 3: Define the minimum square size (10% of the minimum dimension)
            int minDimension = Math.Min(W, H);
            int minSquareSize = (int)(minDimension * 0.1);

            // Step 4: Find the best square, starting from max possible size and down to minSquareSize
            double maxSum = double.NegativeInfinity;
            CustomPoint bestCenter = new CustomPoint(0, 0);
            int bestSize = 0;

            for (int size = Math.Min(W, H); size >= minSquareSize; size--)  // Start from largest and go down to minSquareSize
            {
                for (int i = size; i <= H; i++)
                {
                    for (int j = size; j <= W; j++)
                    {
                        double currentSum = prefixSum[i, j] -
                                            prefixSum[i - size, j] -
                                            prefixSum[i, j - size] +
                                            prefixSum[i - size, j - size];

                        if (currentSum > maxSum)
                        {
                            maxSum = currentSum;
                            bestCenter = new CustomPoint(j - size / 2, i - size / 2);
                            bestSize = size;
                        }
                    }
                }
            }

            // Step 5: Construct the result with the best found center and size
            Points result = new Points(4);
            result.c = bestCenter;
            result.a = bestSize;
            result.pointsfromCenter();

            return result;
        }

        private Points AI(Bitmap inputImage)
        {
            addConsoleText("AI begins");
            outputWeights = model.Use(inputImage, addConsoleText);
            addConsoleText("AI done, calculating best square...");
            outputReady = true;

            return FindSquare(outputWeights, inputImage.Width, inputImage.Height);
        }
        private void showResult()
        {
            if (!outputReady) return;

            if(displayType)
            {
                Bitmap cropped = new Bitmap(points.a, points.a);
                using (Graphics g = Graphics.FromImage(cropped))
                {
                    g.DrawImage(
                        inputImage, 
                        new Rectangle(0, 0, points.a, points.a), 
                        new Rectangle(points.points[0].x, points.points[0].y, points.a, points.a), 
                        GraphicsUnit.Pixel);
                }

                outputImageBox.Image = cropped;
            }
            else
            {
                Bitmap weightsImage = new Bitmap(inputImage.Width, inputImage.Height);
                for (int x = 0; x < inputImage.Width; x++)
                {
                    for (int y = 0; y < inputImage.Height; y++)
                    {
                        int v = Math.Max(0, Math.Min(255, (int)(outputWeights[y * inputImage.Width + x] * 255)));
                        weightsImage.SetPixel(x, y, Color.FromArgb(v, v, v));
                    }
                }

                outputImageBox.Image = weightsImage;
            }
        }
        private void ProcessImage(Bitmap inputImage)
        {
            points = AI(inputImage);
            using (Graphics g = Graphics.FromImage(inputImage))
            {
                Color semiTransparentBlack = Color.FromArgb(128, 0, 0, 0);
                using (Brush semiTransparentBrush = new SolidBrush(semiTransparentBlack))
                {
                    g.FillRectangle(semiTransparentBrush, new Rectangle(0, 0, inputImage.Width, points.points[0].y)); // Top
                    g.FillRectangle(semiTransparentBrush, new Rectangle(0, points.points[2].y, inputImage.Width, inputImage.Height - points.points[2].y)); // Bottom
                    g.FillRectangle(semiTransparentBrush, new Rectangle(0, points.points[0].y, points.points[0].x, points.points[2].y - points.points[0].y)); // Left
                    g.FillRectangle(semiTransparentBrush, new Rectangle(points.points[1].x, points.points[1].y, inputImage.Width - points.points[1].x, points.points[2].y - points.points[1].y)); // Right
                }

                using (Pen framePen = new Pen(Color.FromArgb(0x00, 0xaa, 0xff), 1))
                {
                    g.DrawRectangle(framePen, new Rectangle(points.points[0].x, points.points[0].y, points.points[1].x - points.points[0].x, points.points[2].y - points.points[0].y));
                }
            }

            inputImageBox.Image = inputImage;

            this.Invoke((MethodInvoker)delegate
            {
                showResult();

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
            });
        }

        private void addConsoleText(string text)
        {
            if (consoleTextBox.InvokeRequired)
            {
                consoleTextBox.Invoke(new Action<string>(addConsoleText), text);
            }
            else
            {
                consoleTextBox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >_ " + text + Environment.NewLine);
            }
        }


        private void buttonTogglePixelImportancyClick(object sender, EventArgs e)
        {

            displayType = !displayType;
            if (displayType)
            {
                buttonShowPixelImportancy.Text = "Show Pixel Importancy";
            }
            else
            {
                buttonShowPixelImportancy.Text = "Show Cropped Image";
            }
            showResult();
        }
        private string secondsToTime(double s)
        {
            string time = "";
            int hours = (int)(s / 3600);
            int minutes = (int)((s - hours * 3600) / 60);
            int seconds = (int)(s - hours * 3600 - minutes * 60);
            time = hours.ToString() + ":" + minutes.ToString("00") + ":" + seconds.ToString("00") + (s - (int)s).ToString("F4").Substring(1);
            return time;
        }
        private void updateUI()
        {
            outputImgCount.Text = model.imgCount().ToString();
            outputTrainingTime.Text = secondsToTime( model.TotalTrainingTime );
            outputV.Text = "v." + model.Version.ToString();
        }

        private void updateUI(object sender, EventArgs e)
        {
            updateUI();
        }

        private void updateUI(object sender, MouseEventArgs e)
        {
            updateUI();
        }
    }
}
