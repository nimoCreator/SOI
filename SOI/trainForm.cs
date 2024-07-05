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

        int epochCount = 0;
        double LearningRate = 0.1;
        double MutationFactor = 0.1;

        public trainForm(ref Model model)
        {
            InitializeComponent();
            this.model = model;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);

            outputV.Text = "v." + model.version.ToString();

            outputImgCount.Text = model.imgCount().ToString();

            /*            outputErrorRate.Text = model.ErrorRate().ToString();*/


            epochCountTrackValueChange(null, null);
            learningRateTrackValueChange(null, null);
            mutationFactorTrackValueChange(null, null);
        }
        private void trainForm_Load(object sender, EventArgs e)
        {

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
                addConsoleText("Training started");
            }
            else
            {
                addConsoleText("Training already in progress!");
            }

            if (outputV.InvokeRequired)
            {
                outputV.Invoke(new Action(() => outputV.Text = "v." + model.version.ToString()));
                outputErrorRate.Invoke(new Action(() => outputErrorRate.Text = model.errorRate.ToString()));
                outputErrorRateChange.Invoke(new Action(() => outputErrorRateChange.Text = (model.previousErrorRate - model.errorRate).ToString()));
            }
            else
            {
                outputV.Text = "v." + model.version.ToString();
                outputErrorRate.Text = model.errorRate.ToString();
                outputErrorRateChange.Text = (model.previousErrorRate - model.errorRate).ToString();
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

            do
            {
                model.Train(epochCount, MutationFactor, LearningRate, addConsoleText);

                this.Invoke((MethodInvoker)delegate
                {
                    outputV.Text = "v." + model.version.ToString();
                    outputErrorRate.Text = model.errorRate.ToString();
                    outputErrorRateChange.Text = (model.previousErrorRate - model.errorRate).ToString();

                    addConsoleText("New model version: v." + model.version.ToString());
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
                outputV.Text = "v." + model.version.ToString();
                outputErrorRate.Text = model.errorRate.ToString();
                outputErrorRateChange.Text = (model.previousErrorRate - model.errorRate).ToString();

                //play sound

                System.Media.SystemSounds.Exclamation.Play();

                addConsoleText("Training stopped");
            });
        }

        private void epochCountTrackValueChange(object sender, EventArgs e)
        {
            epochCount = epochCountTrackBar.Value;
            outputEpochs.Text = epochCount.ToString();
        }

        private void learningRateTrackValueChange(object sender, EventArgs e)
        {
            LearningRate = trackBarLearningRate.Value / 10000.0;
            outputLearningRate.Text = LearningRate.ToString();
        }

        private void mutationFactorTrackValueChange(object sender, EventArgs e)
        {
            MutationFactor = trackBarMutationFactor.Value / 10000.0;
            outputMutationFactor.Text = MutationFactor.ToString();
        }
    }
}