using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Security.Cryptography;
using SOI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SOI
{
    public class CryptoRandom
    {
        static public double GetMoreRandomDouble()
        {
            var bytes = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            ulong value = BitConverter.ToUInt64(bytes, 0);
            return (double)value / ulong.MaxValue;
        }
    }

    public static class Constants
    {
        public const int FrameSize = 16; // <===================================================== TUTAJ
                                    // f

        public static readonly int[] LayerSizes = { 768, 640, 512, 384, 256 }; // <===================================================== TUTAJ
                                                // 3 * f^3, 2.5*f^2, 2*f^2, 1.5*f^2, f^2
                                                
        public const int RGBA = 3;
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

        public double ErrorRate { get; set; }
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
            for (int i = 0; i < Weights.Length; i++)
            {
                Weights[i] = CryptoRandom.GetMoreRandomDouble();
                // Weights[i] = 0;
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

        public int inputSize;

        public Layer(int numNeurons, int inputSize)
        {
            Neurons = new Neuron[numNeurons];
            for (int i = 0; i < numNeurons; i++)
            {
                Neurons[i] = new Neuron(inputSize);
            }
            this.inputSize = inputSize;
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

    public class ModelData
    {
        public Layer[] Layers { get; set; }
        public int Version { get; set; }
        public double TotalTrainingTime { get; set; }
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
        private string FilePath;
        private string DataFilePath => Path.Combine(FilePath, "data.json");
        private string ModelFilePath => Path.Combine(FilePath, "model.json");
        private string CSVFilePath => Path.Combine(FilePath, "errorRates.csv");

        public int Version { get; private set; } = 1;
        public double TotalTrainingTime { get; private set; } = 0.0;
        public TimeSpan TrainingTime { get; set; } = TimeSpan.Zero;

        public Layer[] Layers { get; private set; }
        public List<ImageMetadata> ImageData { get; private set; } = new List<ImageMetadata>();

        public double PreviousAvgErrorRate { get; private set; } = 1.0;
        public double AvgErrorRate { get; private set; } = 1.0;

        public double LearningRate { get; set; } = 0.15;
        public double MutationRate { get; set; } = 0.15;

        public Model()
        {
            FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SoI");
            LoadModel();
            LoadImageData();
            CalcAvgErrorRate(null);
        }
        public void LoadModel()
        {
            try
            {
                if (File.Exists(ModelFilePath))
                {
                    string json = File.ReadAllText(ModelFilePath);
                    var modelData = JsonConvert.DeserializeObject<ModelData>(json);

                    Layers = modelData.Layers ?? throw new InvalidOperationException("Layers cannot be null.");
                    if(Layers.Length != Constants.LayerSizes.Length)
                    {
                        throw new InvalidOperationException("Invalid layer count.");
                    }
                    if(Layers[0].Neurons.Length != Constants.LayerSizes[0] || Layers[0].inputSize != Constants.LayerSizes[0])
                    {
                        throw new InvalidOperationException("Invalid input size.");
                    }
                    for(int i = 1; i < Layers.Length; i++)
                    {
                        if(Layers[i].Neurons.Length != Constants.LayerSizes[i] || Layers[i].inputSize != Constants.LayerSizes[i-1])
                        {
                            throw new InvalidOperationException("Invalid layer size.");
                        }
                    }

                    Version = modelData.Version;
                    TotalTrainingTime = modelData.TotalTrainingTime;
                }
                else
                {
                    throw new InvalidOperationException("Model file not found.");
                }
            }
            catch
            {
                InitializeLayers();
                SaveModel();
            }
        }
        private void InitializeLayers()
        {
            Layers = new Layer[Constants.LayerSizes.Length];
            Layers[0] = new Layer(Constants.LayerSizes[0], Constants.LayerSizes[0]);
            for (int i = 1; i < Constants.LayerSizes.Length; i++)
            {
                Layers[i] = new Layer(Constants.LayerSizes[i], Constants.LayerSizes[i-1]);
            }
        }

        public void LoadImageData()
        {
            if (File.Exists(DataFilePath))
            {
                try
                {
                    string json = File.ReadAllText(DataFilePath);
                    ImageData = JsonConvert.DeserializeObject<List<ImageMetadata>>(json);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SaveModel()
        {
            var modelData = new ModelData
            {
                Version = Version,
                TotalTrainingTime = TotalTrainingTime,
                Layers = Layers,
            };

            string json = JsonConvert.SerializeObject(modelData, Formatting.Indented);
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
            }
            File.WriteAllText(ModelFilePath, json);
        }

        public void Train( double mutationFactor, double learningFactor, Action<string> addConsoleText)
        {

/*            for(int i = 0; i < 10; i++)
            {
                addConsoleText(CryptoRandom.GetMoreRandomDouble().ToString());
            }
*/
            LoadImageData();

            LearningRate = learningFactor;
            MutationRate = mutationFactor;

/*            for (int g = 0; g < Layers.Length; g++)
            {
                addConsoleText($"Layer {g + 1}, Size {Layers[g].Neurons.Length}, Input Size {Layers[g].Neurons[0].Weights.Length}");
            }*/

            var startTime = DateTime.Now;
            //addConsoleText($"Starting ErrorRate: {AvgErrorRate}");

            // etap 1 evolve

            CalcAvgErrorRate(addConsoleText);


            foreach (var layer in Layers)
            {
                Parallel.ForEach(layer.Neurons, neuron =>
                {
                    for (int k = 0; k < neuron.Weights.Length; k++)
                    {
                        if (CryptoRandom.GetMoreRandomDouble() < MutationRate)
                        {
                            // neuron.Weights[k] += (rand.NextDouble() * 2 - 1) * LearningRate;
                            neuron.Weights[k] += (CryptoRandom.GetMoreRandomDouble()*2 - 1) * LearningRate;
                        }
                    }
                });
            }

            // 3 jak jest po evolucji

            double newAvgErrorRate = 0.0;

            foreach (var imageData in ImageData)
            {

                double[] inputImage = PreprocessImage(LoadImage(imageData.FileName));
                double[] expectedOutput = CalculateExpectedOutputs(imageData, addConsoleText);

                double[] output = CalculateImage(inputImage, imageData.W, imageData.H);

                imageData.ErrorRate = CalculateErrorRate(output, expectedOutput);

                newAvgErrorRate += imageData.ErrorRate;
            }

            newAvgErrorRate /= ImageData.Count;

            SaveErrorRatesToCSV(newAvgErrorRate);

            if (newAvgErrorRate < AvgErrorRate)
            {
                addConsoleText($"IMPROVEMENT!!! {newAvgErrorRate} (-{(AvgErrorRate - newAvgErrorRate)})");

                SaveImageMetadata(); // Save updated metadata
                CalcAvgErrorRate(addConsoleText);
                SaveModel();

                Version++;
                addConsoleText("New model version: v." + Version.ToString());
            }
            else
            {
                addConsoleText($"No improvement... ({newAvgErrorRate})");
                LoadModel();
                LoadImageData();
            }

            var endTime = DateTime.Now;
            TotalTrainingTime += (endTime - startTime).TotalSeconds;
            SaveModel();
        }

        public double[] PreprocessImage(Bitmap image, Action<string> addConsoleText = null)
        {
            double[] inputs = new double[image.Width * image.Height * 4];
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    int index = (i * image.Height + j) * Constants.RGBA;
                    inputs[index] = pixel.R / 255.0;
                    inputs[index + 1] = pixel.G / 255.0;
                    inputs[index + 2] = pixel.B / 255.0;
                    // inputs[index + 3] = pixel.A; // JEZELI RGBA == 4 ODKOMENTOWAC
                }
            }
            // if (addConsoleText != null) addConsoleText("Image preprocessed");
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
                outputImage.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/SOI_expected_outputs/", imageData.FileName + "_expected.png"));
            }


            return output;
        }

        private void CalcAvgErrorRate(Action<string> addConsoleText)
        {
            /*if(addConsoleText != null)
                addConsoleText($"Before {AvgErrorRate} ( old: {PreviousAvgErrorRate} )");
*/
            double sum = 0.0;
            foreach (var imageData in ImageData)
            {
                sum += imageData.ErrorRate;
            }
            PreviousAvgErrorRate = (double)AvgErrorRate;
            AvgErrorRate = sum / ImageData.Count;

           /* if (addConsoleText != null)
                addConsoleText($"Now {AvgErrorRate} ( old: {PreviousAvgErrorRate} )");*/
        }

        public void SaveImageMetadata()
        {
            string json = JsonConvert.SerializeObject(ImageData, Formatting.Indented);
            File.WriteAllText(DataFilePath, json); // Save to data.json
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
        public double[] Use(Bitmap inputImage, Action<string> addConsoleText)
        {
            return CalculateImage(
                PreprocessImage(inputImage, addConsoleText),
                inputImage.Width,
                inputImage.Height,
                addConsoleText);
        }


        /*        {
                    //addConsoleText($"CalculateImage()");
                    double[] output = new double[input.Length / 4];
                    for (int i = 0; i<output.Length; i++)
                    {
                        output[i] = 0;
                    }
                    addConsoleText($"Calculate Image: output prepared");
                    int windowCount = input.Length / Constants.FrameSize * 2 - 1;
                    int currentWindow = 1;
                    for (int i = 0; i <= input.Length - Constants.FrameSize; i += Constants.FrameSize / 2)
                    {
                        //addConsoleText($"START Window {currentWindow++} / {windowCount} \t Pixels from {i} to {i + Constants.FrameSize}");
                        double[] window = new double[Constants.FrameSize * 4];
                        for (int j = 0; j<Constants.FrameSize; j++)
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
                }*/

        private double[] CalculateImage(double[] input, int Width, int Height, Action<string> addConsoleText = null)
        {
            double[] output = new double[Width * Height];
            double[] weight = new double[output.Length];  

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = 0;
                weight[i] = 0;
            }

            for (int ix = 0; ix <= Width - Constants.FrameSize; ix += Constants.FrameSize / 2)
            {
                for (int yi = 0; yi <= Height - Constants.FrameSize; yi += Constants.FrameSize / 2)
                {
                    double[] window = new double[Constants.FrameSize * Constants.FrameSize * Constants.RGBA];

                    for (int x = 0; x < Constants.FrameSize; x++)
                    {
                        for (int y = 0; y < Constants.FrameSize; y++)
                        {
                            int baseIndex = ((ix + x) * Height + (yi + y)) * 3;
                            if (baseIndex + 2 < input.Length)
                            {
                                window[(x * Constants.FrameSize + y) * 3] = input[baseIndex];
                                window[(x * Constants.FrameSize + y) * 3 + 1] = input[baseIndex + 1];
                                window[(x * Constants.FrameSize + y) * 3 + 2] = input[baseIndex + 2];
                                // window[(x * Constants.FrameSize + y) * 4 + 3] = input[baseIndex + 3]; // <= if RGBA == 4 uncomment
                            }
                        }
                    }

                    double[] part = ActivateWindow(window);

                    for (int x = 0; x < Constants.FrameSize; x++)
                    {
                        for (int y = 0; y < Constants.FrameSize; y++)
                        {
                            int outIndex = (ix + x) * Height + (yi + y);
                            if (outIndex < output.Length)
                            {
                                output[outIndex] += part[x * Constants.FrameSize + y];
                                weight[outIndex] += 1;
                            }
                        }
                    }
                }
            }

            /*

            122222221
            244444442
            244444442
            244444442
            122222221

             */

            for (int i = 0; i < output.Length; i++)
            {
                if (weight[i] > 0)  // Avoid division by zero
                {
                    output[i] /= weight[i];
                }
            }

            return output;
        }




        private double[] ActivateWindow(double[] window, Action<string> addConsoleText = null)
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

        public void reset()
        {
            PreviousAvgErrorRate = 1.0;
            AvgErrorRate = 0.0;

            foreach (var imageData in ImageData)
            {

                double[] inputImage = PreprocessImage(LoadImage(imageData.FileName));
                double[] expectedOutput = CalculateExpectedOutputs(imageData, null);

                double[] output = CalculateImage(inputImage, imageData.W, imageData.H);

                imageData.ErrorRate = CalculateErrorRate(output, expectedOutput);

                AvgErrorRate += imageData.ErrorRate;
            }

            AvgErrorRate /= ImageData.Count;

            SaveImageMetadata();
            SaveModel();
        }
        public void SaveErrorRatesToCSV(double tryErrorRate)
        {
            try
            {
                var csvLines = new List<string>();
                if (!File.Exists(CSVFilePath))
                {
                    csvLines.Add("Timestamp, Best Error Rate, Try Error Rate");
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                if (AvgErrorRate < tryErrorRate)
                    csvLines.Add($"{timestamp},{AvgErrorRate},{tryErrorRate}");
                else
                    csvLines.Add($"{timestamp},{tryErrorRate},{tryErrorRate}");

/*                foreach (var imageData in ImageData)
                {
                    csvLines.Add($"{timestamp},{AvgErrorRate},{imageData.FileName},{imageData.ErrorRate}");
                }*/

                File.AppendAllLines(CSVFilePath, csvLines);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving error rates to CSV: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}