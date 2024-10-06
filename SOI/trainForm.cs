using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOI
{
    public partial class trainForm : Form
    {
        Model model;
        bool training = false;
        BackgroundWorker backgroundWorker;

        double LearningRate = 0.1;
        double MutationFactor = 0.1;

        bool randomMutationFactor = false;
        bool randomLearningRate = false;


        public trainForm(ref Model model)
        {
            InitializeComponent();
            this.model = model;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);

            outputV.Text = "v." + model.Version.ToString();
            double change = model.PreviousAvgErrorRate - model.AvgErrorRate;
            outputErrorRate.Text = model.AvgErrorRate.ToString("F8") + " (-" + change.ToString("F8") + ")";
            outputLearningTime.Text = secondsToTime(model.TotalTrainingTime);

            outputImgCount.Text = model.imgCount().ToString();

            /*            outputErrorRate.Text = model.ErrorRate().ToString();*/

            learningRateTrackValueChange(null, null);
            mutationFactorTrackValueChange(null, null);
        }
        private void trainForm_Load(object sender, EventArgs e)
        {

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

        private void buttonTrainClick(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy)
            {
                training = true;
                backgroundWorker.RunWorkerAsync();
            }
            else
            {
                addConsoleText("Training already in progress!");
            }

            if (outputV.InvokeRequired)
            {
                outputV.Invoke(new Action(() => outputV.Text = "v." + model.Version.ToString()));
                outputErrorRate.Invoke(new Action(() =>
                    outputErrorRate.Text = model.AvgErrorRate.ToString("F8") + " (" + (model.PreviousAvgErrorRate - model.AvgErrorRate).ToString("F8") + ")"
                    ));
                outputLearningTime.Invoke(new Action(() => outputLearningTime.Text = secondsToTime(model.TotalTrainingTime)));
            }
            else
            {
                outputV.Text = "v." + model.Version.ToString();
                double change = model.PreviousAvgErrorRate - model.AvgErrorRate;
                outputErrorRate.Text = model.AvgErrorRate.ToString("F8") + " (-" + change.ToString("F8") + ")";
                outputLearningTime.Text = secondsToTime(model.TotalTrainingTime);
            }
        }

        private void buttonStopClick(object sender, EventArgs e)
        {
            addConsoleText("Training will stop once the epoch is over!");
            training = false;
            backgroundWorker.CancelAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            addConsoleText($"Training started with ErrorRate: {model.AvgErrorRate}");

            do
            {
                model.Train(MutationFactor, LearningRate, addConsoleText);

                this.Invoke((MethodInvoker)delegate
                {
                    outputV.Text = "v." + model.Version.ToString();
                    double change = model.PreviousAvgErrorRate - model.AvgErrorRate;
                    outputErrorRate.Text = model.AvgErrorRate.ToString("F8") + " (-" + change.ToString("F8") + ")";
                    outputLearningTime.Text = secondsToTime(model.TotalTrainingTime);

                    if(randomMutationFactor)
                    {
                        randomiseMutationFactor();
                    }
                    if (randomLearningRate)
                    {
                        randomiseLearningRate();
                    }
                });

                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    training = false;
                    break;
                }
            } while (training);

            this.Invoke((MethodInvoker)delegate
            {
                outputV.Text = "v." + model.Version.ToString();
                double change = model.PreviousAvgErrorRate - model.AvgErrorRate;
                outputErrorRate.Text = model.AvgErrorRate.ToString("F8") + " (-" + change.ToString("F8") + ")";
                outputLearningTime.Text = secondsToTime(model.TotalTrainingTime);

                //play sound

                System.Media.SystemSounds.Exclamation.Play();

                addConsoleText("Training stopped");
            });
        }

        private void learningRateTrackValueChange(object sender, EventArgs e)
        {
            changeLearningRate(trackBarLearningRate.Value);
        }

        private void mutationFactorTrackValueChange(object sender, EventArgs e)
        {
            changeMutationFactor(trackBarMutationFactor.Value);
        }

        private void buttonResetClick(object sender, EventArgs e)
        {
            model.reset();

            outputV.Text = "v." + model.Version.ToString();
            double change = model.PreviousAvgErrorRate - model.AvgErrorRate;
            outputErrorRate.Text = model.AvgErrorRate.ToString("F8") + " (-" + change.ToString("F8") + ")";
            outputLearningTime.Text = secondsToTime(model.TotalTrainingTime);
        }

        private void changeMutationFactor(int mf)
        {
            MutationFactor = trackBarMutationFactor.Value / 10000.0;
            outputMutationFactor.Text = MutationFactor.ToString();
        }
        private void changeLearningRate(int lr)
        {
            LearningRate = trackBarLearningRate.Value / 10000.0;
            outputLearningRate.Text = LearningRate.ToString();
        }

        private void randomiseMutationFactor()
        {
            trackBarMutationFactor.Value = (int)(CryptoRandom.GetMoreRandomDouble() * 10000);
        }

        private void randomiseLearningRate()
        {
            trackBarLearningRate.Value = (int)(CryptoRandom.GetMoreRandomDouble() * 10000);
        }

        private void ClickRandomMutationFactor(object sender, EventArgs e)
        {
            randomMutationFactor = !randomMutationFactor;
            if(randomMutationFactor)
            {
                randomiseMutationFactor();
            }
        }

        private void ClickRandomLearningRate(object sender, EventArgs e)
        {
            randomLearningRate = !randomLearningRate;
            if (randomLearningRate)
            {
                randomiseLearningRate();
            }
        }
    }
}