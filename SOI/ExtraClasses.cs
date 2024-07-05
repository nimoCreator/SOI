using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOI
{
    public static class Constants
    {
        public const int FrameSize = 16; // <===================================================== TUTAJ
        public const int LayerCount = 1; // <===================================================== TUTAJ
    }

    public struct CustomPoint
    {
        public int x, y;

        public CustomPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public CustomPoint(CustomPoint p)
        {
            x = p.x;
            y = p.y;
        }
    }

    public struct Points
    {
        public int a;
        public CustomPoint c;
        public CustomPoint[] points;

        public Points(int numPoints)
        {
            a = 0;
            c = new CustomPoint(0, 0);
            points = new CustomPoint[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                points[i] = new CustomPoint(0, 0);
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
        public CustomPoint c { get; set; }
        public int a { get; set; }
    }

    public class Neuron
    {
        public double[] Weights { get; set; }

        public Neuron(int inputSize)
        {
            Weights = new double[inputSize];
            InitializeWeights();
        }

        private void InitializeWeights()
        {
            Random rand = new Random();
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = rand.NextDouble();
            }
        }

        public double Activate(double[] inputs)
        {
            if (inputs.Length != Weights.Length)
                throw new ArgumentException("Input size must match weight size");

            double sum = 0.0;
            for (int i = 0; i < inputs.Length; i++)
            {
                sum += inputs[i] * Weights[i];
            }

            return sum / inputs.Length;
        }

        public double Activate(double[] inputs, Action<string> addConsoleText)
        {
            if (inputs.Length != Weights.Length)
            {
                addConsoleText($"Input size must match weight size");
                throw new ArgumentException("Input size must match weight size");
            }

            double sum = 0.0;
            for (int i = 0; i < inputs.Length; i++)
            {
                /*                addConsoleText($"weight {i} / {inputs.Length}");*/
                sum += inputs[i] * Weights[i];
            }

            return sum / inputs.Length;
        }
    }

    public class Layer
    {
        public Neuron[] Neurons { get; set; }

        public Layer(int numNeurons, int inputSize)
        {
            Neurons = new Neuron[numNeurons];
            for (int i = 0; i < numNeurons; i++)
            {
                Neurons[i] = new Neuron(inputSize);
            }
        }

        public double[] Activate(double[] inputs, Action<string> addConsoleText)
        {
            //addConsoleText($"Layer size: {Neurons.Length}, input size: {Neurons[0].Weights.Length}");
            double[] output = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                //addConsoleText($"Neuron {i} / {Neurons.Length}");
                output[i] = Neurons[i].Activate(inputs, addConsoleText);
            }
            return output;
        }

        public double[] Activate(double[] inputs)
        {
            double[] output = new double[Neurons.Length];
            for (int i = 0; i < Neurons.Length; i++)
            {
                output[i] = Neurons[i].Activate(inputs);
            }
            return output;
        }
    }

    public class Kolorek
    {
        public int R, G, B, A;

        public Kolorek(int R, int G, int B, int A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public Kolorek(Kolorek k)
        {
            R = k.R;
            G = k.G;
            B = k.B;
            A = k.A;
        }

        public Kolorek()
        {
            R = 0;
            G = 0;
            B = 0;
            A = 0;
        }

        public void set(int R, int G, int B, int A)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = A;
        }

        public void set(Kolorek k)
        {
            R = k.R;
            G = k.G;
            B = k.B;
            A = k.A;
        }

        public void set(int R, int G, int B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
            this.A = 255;
        }

    }

    public class Model
    {
        private string FilePath = "";
        private string DataFilePath => Path.Combine(FilePath, "data.json");
        private string ModelFilePath => Path.Combine(FilePath, "model.json");

        public int version = 1;

        public Layer[] Layers { get; set; }
        public List<ImageMetadata> ImageData { get; set; } = new List<ImageMetadata>();

        public double previousErrorRate = 1.0;
        public double errorRate = 1.0;

        public double LearningRate = 0.15;
        public double MutationRate = 0.15;

        public Model()
        {
            FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SoI");
            LoadModel();
            LoadImageData();
        }

        public void LoadModel()
        {
            if (File.Exists(ModelFilePath))
            {
                string json = File.ReadAllText(ModelFilePath);
                Layers = JsonConvert.DeserializeObject<Layer[]>(json);
            }
            else
            {
                Layers = new Layer[Constants.LayerCount];
                Layers[0] = new Layer(Constants.FrameSize, Constants.FrameSize * 4);
                for (int i = 1; i < Constants.LayerCount; i++)
                {
                    Layers[i] = new Layer(Constants.FrameSize, Constants.FrameSize);
                }

                SaveModel();
            }
        }

        public void LoadImageData()
        {
            if (File.Exists(DataFilePath))
            {
                string json = File.ReadAllText(DataFilePath);
                ImageData = JsonConvert.DeserializeObject<List<ImageMetadata>>(json);
            }
        }

        public void SaveModel()
        {
            string json = JsonConvert.SerializeObject(Layers);
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            File.WriteAllText(ModelFilePath, json);
        }

        public void Train(int epochCount, double mutationFactor, double learningFactor, Action<string> addConsoleText)
        {
            LoadImageData();

            LearningRate = learningFactor;
            MutationRate = mutationFactor;

            for (int g = 0; g < Layers.Length; g++)
            {
                addConsoleText($"Layer {g}, Size {Layers[g].Neurons.Length}, Input Size {Layers[g].Neurons[g].Weights.Length}");
            }

            int i = 1;
            int total = ImageData.Count;
            foreach (var imageData in ImageData)
            {
                addConsoleText($"START Image: {i++} / {total} : {imageData.FileName}");

                double[] inputimage = PreprocessImage(LoadImage(imageData.FileName));
                double[] expectedoutput = CalculateExpectedOutputs(imageData, addConsoleText);

                Evolve(
                    inputimage,
                    expectedoutput, 
                    epochCount, 
                    addConsoleText);

                //addConsoleText($"END Image");
            }

            SaveModel();
            version++;
        }

        public double[] PreprocessImage(Bitmap image, Action<string> addConsoleText)
        {
            double[] inputs = new double[image.Width * image.Height * 4];
            for (int i = 0; i < image.Width * image.Height; i += 4)
            {
                //addConsoleText($"Pixel {i} / {image.Width * image.Height}");

                inputs[i] = image.GetPixel(i % image.Width, i / image.Width).R;
                inputs[i + 1] = image.GetPixel(i % image.Width, i / image.Width).G;
                inputs[i + 2] = image.GetPixel(i % image.Width, i / image.Width).B;
                inputs[i + 3] = image.GetPixel(i % image.Width, i / image.Width).A;
            }
            addConsoleText($"Image preprocessed");
            return inputs;
        }
        public double[] PreprocessImage(Bitmap image)
        {
            double[] inputs = new double[image.Width * image.Height * 4];
            for (int i = 0; i < image.Width * image.Height; i += 4)
            {
                //addConsoleText($"Pixel {i} / {image.Width * image.Height}");

                inputs[i] = image.GetPixel(i % image.Width, i / image.Width).R;
                inputs[i + 1] = image.GetPixel(i % image.Width, i / image.Width).G;
                inputs[i + 2] = image.GetPixel(i % image.Width, i / image.Width).B;
                inputs[i + 3] = image.GetPixel(i % image.Width, i / image.Width).A;
            }
            return inputs;
        }

        private Bitmap LoadImage(string fileName)
        {
            try
            {
                return new Bitmap(Path.Combine(FilePath, fileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Error loading image '{fileName}': {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private double[] CalculateExpectedOutputs(ImageMetadata imageData, Action<string> addConsoleText)
        {
            double[] output = new double[imageData.H * imageData.W];
            for (int i = 0; i < imageData.H; i++)
            {
                for (int j = 0; j < imageData.W; j++)
                {
                    output[i * imageData.W + j] = 0;
                }
            }
            for (int i = imageData.c.x - imageData.a / 2; i < imageData.c.x + imageData.a / 2; i++)
            {
                for (int j = imageData.c.y - imageData.a / 2; j < imageData.c.y + imageData.a / 2; j++)
                {
                    output[j * imageData.W + i] = 1;
                }
            }
            // save the expected output to desktop
            /*
            {
                Bitmap outputImage = new Bitmap(imageData.W, imageData.H);
                for (int i = 0; i < imageData.H; i++)
                {
                    for (int j = 0; j < imageData.W; j++)
                    {
                        int v = Math.Max(0, Math.Min(255, (int)(output[i * imageData.W + j] * 255)));
                        outputImage.SetPixel(j, i, Color.FromArgb(v, v, v));
                    }
                }
                outputImage.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"/SOI_expected_outputs/", imageData.FileName + "_expected.png"));
            }
            */

            return output;
        }


        private void Evolve(double[] image, double[] expectedOutput, int epochCount, Action<string> addConsoleText)
        {
            Random rand = new Random();

            double[] output = CalculateImage(image);

            double currentErrorRate = CalculateErrorRate(output, expectedOutput);

            for (int epoch = 0; epoch < epochCount; epoch++)
            {
                //addConsoleText($"START epoch {epoch} / {epochCount}");
                for (int i = 0; i < Layers.Length; i++)
                {
                    Parallel.ForEach(Layers[i].Neurons, neuron =>
                    {
                        for (int k = 0; k < neuron.Weights.Length; k++)
                        {
                            if (rand.NextDouble() < MutationRate)
                            {
                                neuron.Weights[k] += (rand.NextDouble() * 2 - 1) * LearningRate;
                            }
                        }
                    });

                }

                double[] newOutput = CalculateImage(image, addConsoleText);
                double newErrorRate = CalculateErrorRate(newOutput, expectedOutput);

                if (newErrorRate < currentErrorRate)
                {
                    addConsoleText($"IMPROVEMENT!!! {errorRate} <= new: {newErrorRate} | improvement: -{currentErrorRate - newErrorRate}");

                    upDateErrorRate(newErrorRate);
                    SaveModel();
                }
                else
                {
                    addConsoleText($"No improvement");
                    LoadModel();
                }
            }
        }

        private double CalculateErrorRate(double[] outputs, double[] expectedOutputs)
        {
            double sum = 0.0;
            for (int i = 0; i < outputs.Length; i++)
            {
                sum += Math.Abs(outputs[i] - expectedOutputs[i]);
            }
            return sum / outputs.Length;
        }
        public void upDateErrorRate(double newErrorRate)
        {
            previousErrorRate = errorRate;
            errorRate = newErrorRate;
            return;
        }

        public double[] Use(Bitmap inputImage, Action<string> addConsoleText)
        {

            return CalculateImage(
                PreprocessImage(inputImage, addConsoleText), 
                addConsoleText);
        }

        private double[] CalculateImage(double[] input) // input is 4 times longer than the image
        {
            //addConsoleText($"CalculateImage()");
            double[] output = new double[input.Length / 4];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = 0;
            }
            //addConsoleText($"Calculate Image: output prepared");
            int windowCount = input.Length / Constants.FrameSize * 2 - 1;
            int currentWindow = 1;
            for (int i = 0; i <= input.Length - Constants.FrameSize; i += Constants.FrameSize / 2)
            {
                //addConsoleText($"START Window {currentWindow++} / {windowCount} \t Pixels from {i} to {i + Constants.FrameSize}");
                double[] window = new double[Constants.FrameSize * 4];
                for (int j = 0; j < Constants.FrameSize; j++)
                {
                    //addConsoleText($"Pixels {i+j*4} - {i+j*4+3}");
                    int baseIndex = i + j * 4;
                    if (baseIndex + 3 < input.Length)
                    {
                        window[j * 4] = input[baseIndex];
                        window[j * 4 + 1] = input[baseIndex + 1];
                        window[j * 4 + 2] = input[baseIndex + 2];
                        window[j * 4 + 3] = input[baseIndex + 3];
                    }
                }
                //addConsoleText($"START Neurons");
                double[] part = ActivateWindow(window);
                //addConsoleText($"END Neurons");
                for (int j = 0; j < Constants.FrameSize; j++)
                {
                    int outIndex = i + j;
                    if (outIndex < output.Length)
                    {
                        output[outIndex] += part[j];
                    }
                }
                //addConsoleText($"END Window");
            }
            // stabilize the output
            for (int i = Constants.FrameSize / 2; i < output.Length - Constants.FrameSize / 2; i++)
            {
                output[i] /= 2;
            }

            return output;
        }

        private double[] CalculateImage(double[] input, Action<string> addConsoleText) // input is 4 times longer than the image
        {
            //addConsoleText($"CalculateImage()");
            double[] output = new double[input.Length / 4];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = 0;
            }
            //addConsoleText($"Calculate Image: output prepared");
            int windowCount = input.Length / Constants.FrameSize * 2 - 1;
            int currentWindow = 1;
            for (int i = 0; i <= input.Length - Constants.FrameSize; i += Constants.FrameSize / 2)
            {
                //addConsoleText($"START Window {currentWindow++} / {windowCount} \t Pixels from {i} to {i + Constants.FrameSize}");
                double[] window = new double[Constants.FrameSize * 4];
                for (int j = 0; j < Constants.FrameSize; j++)
                {
                    //addConsoleText($"Pixels {i+j*4} - {i+j*4+3}");
                    int baseIndex = i + j * 4;
                    if (baseIndex + 3 < input.Length)
                    {
                        window[j * 4] = input[baseIndex];
                        window[j * 4 + 1] = input[baseIndex + 1];
                        window[j * 4 + 2] = input[baseIndex + 2];
                        window[j * 4 + 3] = input[baseIndex + 3];
                    }
                }
                //addConsoleText($"START Neurons");
                double[] part = ActivateWindow(window, addConsoleText);
                //addConsoleText($"END Neurons");
                for (int j = 0; j < Constants.FrameSize; j++)
                {
                    int outIndex = i + j;
                    if (outIndex < output.Length)
                    {
                        output[outIndex] += part[j];
                    }
                }
                //addConsoleText($"END Window");
            }
            // stabilize the output
            for (int i = Constants.FrameSize / 2; i < output.Length - Constants.FrameSize / 2; i++)
            {
                output[i] /= 2;
            }

            return output;
        }

        private double[] ActivateWindow(double[] window)
        {
            double[] output = Layers[0].Activate(window);
            for (int i = 1; i < Layers.Length; i++)
            {
                output = Layers[i].Activate(output);
            }
            return output;
        }

        private double[] ActivateWindow(double[] window, Action<string> addConsoleText)
        {
            //double[][] outputs = new double[Constants.LayerCount][];
            //addConsoleText($"Layer 0, inputting {window.Length} into {Layers[0].Neurons[0].Weights.Length}");
            double[] output = Layers[0].Activate(window);
            //outputs[0] = output;
            //addConsoleText($"Layer 0, output {output.Length}");
            for (int i = 1; i < Layers.Length; i++)
            {
                //addConsoleText($"Layer {i}, inputting {output.Length} into {Layers[i].Neurons[0].Weights.Length}");
                output = Layers[i].Activate(output);
                //outputs[i] = output;
                //addConsoleText($"Layer {i}, output {output.Length}");
            }
            //outputs = outputs;
            return output;
        }

        public int imgCount()
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
    }
}