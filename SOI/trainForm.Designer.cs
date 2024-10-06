namespace SOI
{
    partial class trainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(trainForm));
            this.buttonTrain = new System.Windows.Forms.Button();
            this.labelV = new System.Windows.Forms.Label();
            this.outputV = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.consoleTextBox = new System.Windows.Forms.TextBox();
            this.outputImgCount = new System.Windows.Forms.Label();
            this.labelImgCount = new System.Windows.Forms.Label();
            this.outputErrorRate = new System.Windows.Forms.Label();
            this.labelErrorRate = new System.Windows.Forms.Label();
            this.trackBarLearningRate = new System.Windows.Forms.TrackBar();
            this.trackBarMutationFactor = new System.Windows.Forms.TrackBar();
            this.outputLearningRate = new System.Windows.Forms.Label();
            this.labelLearningRate = new System.Windows.Forms.Label();
            this.outputMutationFactor = new System.Windows.Forms.Label();
            this.labelMutationFactor = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonReset = new System.Windows.Forms.Button();
            this.outputLearningTime = new System.Windows.Forms.Label();
            this.labelTrainingTime = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.checkBoxRandomLearningRate = new System.Windows.Forms.CheckBox();
            this.checkBoxRandomMutationFactor = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLearningRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMutationFactor)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonTrain
            // 
            this.buttonTrain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonTrain, 2);
            this.buttonTrain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTrain.Location = new System.Drawing.Point(3, 144);
            this.buttonTrain.Name = "buttonTrain";
            this.buttonTrain.Size = new System.Drawing.Size(154, 23);
            this.buttonTrain.TabIndex = 0;
            this.buttonTrain.Text = "Train";
            this.buttonTrain.UseVisualStyleBackColor = true;
            this.buttonTrain.Click += new System.EventHandler(this.buttonTrainClick);
            // 
            // labelV
            // 
            this.labelV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelV.Location = new System.Drawing.Point(3, 115);
            this.labelV.Name = "labelV";
            this.labelV.Size = new System.Drawing.Size(74, 26);
            this.labelV.TabIndex = 24;
            this.labelV.Text = "Version:";
            this.labelV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // outputV
            // 
            this.outputV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputV.Location = new System.Drawing.Point(83, 115);
            this.outputV.Name = "outputV";
            this.outputV.Size = new System.Drawing.Size(74, 26);
            this.outputV.TabIndex = 25;
            this.outputV.Text = "v.0001";
            this.outputV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonStop
            // 
            this.buttonStop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonStop, 2);
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStop.Location = new System.Drawing.Point(163, 144);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(154, 23);
            this.buttonStop.TabIndex = 26;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStopClick);
            // 
            // consoleTextBox
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.consoleTextBox, 8);
            this.consoleTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.consoleTextBox.Location = new System.Drawing.Point(3, 173);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.consoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleTextBox.Size = new System.Drawing.Size(637, 358);
            this.consoleTextBox.TabIndex = 29;
            // 
            // outputImgCount
            // 
            this.outputImgCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputImgCount.Location = new System.Drawing.Point(243, 115);
            this.outputImgCount.Name = "outputImgCount";
            this.outputImgCount.Size = new System.Drawing.Size(74, 26);
            this.outputImgCount.TabIndex = 31;
            this.outputImgCount.Text = "0";
            this.outputImgCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelImgCount
            // 
            this.labelImgCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImgCount.Location = new System.Drawing.Point(163, 115);
            this.labelImgCount.Name = "labelImgCount";
            this.labelImgCount.Size = new System.Drawing.Size(74, 26);
            this.labelImgCount.TabIndex = 30;
            this.labelImgCount.Text = "Images In Model:";
            this.labelImgCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // outputErrorRate
            // 
            this.outputErrorRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputErrorRate.Location = new System.Drawing.Point(403, 115);
            this.outputErrorRate.Name = "outputErrorRate";
            this.outputErrorRate.Size = new System.Drawing.Size(74, 26);
            this.outputErrorRate.TabIndex = 33;
            this.outputErrorRate.Text = "0";
            this.outputErrorRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelErrorRate
            // 
            this.labelErrorRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorRate.Location = new System.Drawing.Point(323, 115);
            this.labelErrorRate.Name = "labelErrorRate";
            this.labelErrorRate.Size = new System.Drawing.Size(74, 26);
            this.labelErrorRate.TabIndex = 32;
            this.labelErrorRate.Text = "Error Rate:";
            this.labelErrorRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // trackBarLearningRate
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.trackBarLearningRate, 6);
            this.trackBarLearningRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLearningRate.LargeChange = 10;
            this.trackBarLearningRate.Location = new System.Drawing.Point(163, 16);
            this.trackBarLearningRate.Maximum = 10000;
            this.trackBarLearningRate.Minimum = 1;
            this.trackBarLearningRate.Name = "trackBarLearningRate";
            this.tableLayoutPanel1.SetRowSpan(this.trackBarLearningRate, 2);
            this.trackBarLearningRate.Size = new System.Drawing.Size(477, 45);
            this.trackBarLearningRate.SmallChange = 5;
            this.trackBarLearningRate.TabIndex = 35;
            this.trackBarLearningRate.TickFrequency = 1000;
            this.trackBarLearningRate.Value = 1;
            this.trackBarLearningRate.ValueChanged += new System.EventHandler(this.learningRateTrackValueChange);
            // 
            // trackBarMutationFactor
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.trackBarMutationFactor, 6);
            this.trackBarMutationFactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarMutationFactor.LargeChange = 10;
            this.trackBarMutationFactor.Location = new System.Drawing.Point(163, 67);
            this.trackBarMutationFactor.Maximum = 10000;
            this.trackBarMutationFactor.Minimum = 1;
            this.trackBarMutationFactor.Name = "trackBarMutationFactor";
            this.tableLayoutPanel1.SetRowSpan(this.trackBarMutationFactor, 2);
            this.trackBarMutationFactor.Size = new System.Drawing.Size(477, 45);
            this.trackBarMutationFactor.SmallChange = 5;
            this.trackBarMutationFactor.TabIndex = 36;
            this.trackBarMutationFactor.TickFrequency = 1000;
            this.trackBarMutationFactor.Value = 1;
            this.trackBarMutationFactor.ValueChanged += new System.EventHandler(this.mutationFactorTrackValueChange);
            // 
            // outputLearningRate
            // 
            this.outputLearningRate.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.outputLearningRate, 2);
            this.outputLearningRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputLearningRate.Location = new System.Drawing.Point(3, 26);
            this.outputLearningRate.Name = "outputLearningRate";
            this.outputLearningRate.Size = new System.Drawing.Size(154, 38);
            this.outputLearningRate.TabIndex = 40;
            this.outputLearningRate.Text = "0";
            this.outputLearningRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLearningRate
            // 
            this.labelLearningRate.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelLearningRate, 2);
            this.labelLearningRate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLearningRate.Location = new System.Drawing.Point(3, 13);
            this.labelLearningRate.Name = "labelLearningRate";
            this.labelLearningRate.Size = new System.Drawing.Size(154, 13);
            this.labelLearningRate.TabIndex = 39;
            this.labelLearningRate.Text = "Learning Rate:";
            this.labelLearningRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // outputMutationFactor
            // 
            this.outputMutationFactor.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.outputMutationFactor, 2);
            this.outputMutationFactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputMutationFactor.Location = new System.Drawing.Point(3, 77);
            this.outputMutationFactor.Name = "outputMutationFactor";
            this.outputMutationFactor.Size = new System.Drawing.Size(154, 38);
            this.outputMutationFactor.TabIndex = 42;
            this.outputMutationFactor.Text = "0";
            this.outputMutationFactor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMutationFactor
            // 
            this.labelMutationFactor.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelMutationFactor, 2);
            this.labelMutationFactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMutationFactor.Location = new System.Drawing.Point(3, 64);
            this.labelMutationFactor.Name = "labelMutationFactor";
            this.labelMutationFactor.Size = new System.Drawing.Size(154, 13);
            this.labelMutationFactor.TabIndex = 41;
            this.labelMutationFactor.Text = "Mutation Factor:";
            this.labelMutationFactor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.buttonReset, 4, 8);
            this.tableLayoutPanel1.Controls.Add(this.outputLearningTime, 7, 7);
            this.tableLayoutPanel1.Controls.Add(this.labelTrainingTime, 6, 7);
            this.tableLayoutPanel1.Controls.Add(this.labelTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.outputMutationFactor, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.consoleTextBox, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.labelMutationFactor, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonTrain, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.outputLearningRate, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonStop, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.labelLearningRate, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelV, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.outputV, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.labelImgCount, 2, 7);
            this.tableLayoutPanel1.Controls.Add(this.trackBarMutationFactor, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.outputImgCount, 3, 7);
            this.tableLayoutPanel1.Controls.Add(this.labelErrorRate, 4, 7);
            this.tableLayoutPanel1.Controls.Add(this.outputErrorRate, 5, 7);
            this.tableLayoutPanel1.Controls.Add(this.trackBarLearningRate, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxRandomLearningRate, 6, 8);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxRandomMutationFactor, 7, 8);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(643, 534);
            this.tableLayoutPanel1.TabIndex = 43;
            // 
            // buttonReset
            // 
            this.buttonReset.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonReset, 2);
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonReset.Location = new System.Drawing.Point(323, 144);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(154, 23);
            this.buttonReset.TabIndex = 45;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonResetClick);
            // 
            // outputLearningTime
            // 
            this.outputLearningTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.outputLearningTime.Location = new System.Drawing.Point(563, 115);
            this.outputLearningTime.Name = "outputLearningTime";
            this.outputLearningTime.Size = new System.Drawing.Size(77, 26);
            this.outputLearningTime.TabIndex = 44;
            this.outputLearningTime.Text = "0";
            this.outputLearningTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTrainingTime
            // 
            this.labelTrainingTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTrainingTime.Location = new System.Drawing.Point(483, 115);
            this.labelTrainingTime.Name = "labelTrainingTime";
            this.labelTrainingTime.Size = new System.Drawing.Size(74, 26);
            this.labelTrainingTime.TabIndex = 43;
            this.labelTrainingTime.Text = "Learinig Time:";
            this.labelTrainingTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTitle
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.labelTitle, 8);
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Location = new System.Drawing.Point(3, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(637, 13);
            this.labelTitle.TabIndex = 25;
            this.labelTitle.Text = "BIAI Project for Silesian University of Technology 2024 by: Klamra Kamil, Legiers" +
    "ki Sebastian , Ulman Artur";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBoxRandomLearningRate
            // 
            this.checkBoxRandomLearningRate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxRandomLearningRate.AutoSize = true;
            this.checkBoxRandomLearningRate.Location = new System.Drawing.Point(483, 144);
            this.checkBoxRandomLearningRate.Name = "checkBoxRandomLearningRate";
            this.checkBoxRandomLearningRate.Size = new System.Drawing.Size(74, 23);
            this.checkBoxRandomLearningRate.TabIndex = 46;
            this.checkBoxRandomLearningRate.Text = "Ranom Learning";
            this.checkBoxRandomLearningRate.UseVisualStyleBackColor = true;
            this.checkBoxRandomLearningRate.Click += new System.EventHandler(this.ClickRandomLearningRate);
            // 
            // checkBoxRandomMutationFactor
            // 
            this.checkBoxRandomMutationFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxRandomMutationFactor.AutoSize = true;
            this.checkBoxRandomMutationFactor.Location = new System.Drawing.Point(563, 144);
            this.checkBoxRandomMutationFactor.Name = "checkBoxRandomMutationFactor";
            this.checkBoxRandomMutationFactor.Size = new System.Drawing.Size(77, 23);
            this.checkBoxRandomMutationFactor.TabIndex = 47;
            this.checkBoxRandomMutationFactor.Text = "Random Mutation";
            this.checkBoxRandomMutationFactor.UseVisualStyleBackColor = true;
            this.checkBoxRandomMutationFactor.Click += new System.EventHandler(this.ClickRandomMutationFactor);
            // 
            // trainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 534);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "trainForm";
            this.Text = "Train the Model | SOI";
            this.Load += new System.EventHandler(this.trainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLearningRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarMutationFactor)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTrain;
        private System.Windows.Forms.Label labelV;
        private System.Windows.Forms.Label outputV;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox consoleTextBox;
        private System.Windows.Forms.Label outputImgCount;
        private System.Windows.Forms.Label labelImgCount;
        private System.Windows.Forms.Label outputErrorRate;
        private System.Windows.Forms.Label labelErrorRate;
        private System.Windows.Forms.TrackBar trackBarLearningRate;
        private System.Windows.Forms.TrackBar trackBarMutationFactor;
        private System.Windows.Forms.Label outputLearningRate;
        private System.Windows.Forms.Label labelLearningRate;
        private System.Windows.Forms.Label outputMutationFactor;
        private System.Windows.Forms.Label labelMutationFactor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label outputLearningTime;
        private System.Windows.Forms.Label labelTrainingTime;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.CheckBox checkBoxRandomLearningRate;
        private System.Windows.Forms.CheckBox checkBoxRandomMutationFactor;
    }
}