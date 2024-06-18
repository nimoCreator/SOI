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
            this.buttonTrain = new System.Windows.Forms.Button();
            this.labelV = new System.Windows.Forms.Label();
            this.outputV = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonTrain
            // 
            this.buttonTrain.Location = new System.Drawing.Point(228, 230);
            this.buttonTrain.Name = "buttonTrain";
            this.buttonTrain.Size = new System.Drawing.Size(75, 23);
            this.buttonTrain.TabIndex = 0;
            this.buttonTrain.Text = "Train";
            this.buttonTrain.UseVisualStyleBackColor = true;
            this.buttonTrain.Click += new System.EventHandler(this.buttonTrainClick);
            // 
            // labelV
            // 
            this.labelV.Location = new System.Drawing.Point(191, 138);
            this.labelV.Name = "labelV";
            this.labelV.Size = new System.Drawing.Size(112, 26);
            this.labelV.TabIndex = 24;
            this.labelV.Text = "Version:";
            this.labelV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // outputV
            // 
            this.outputV.Location = new System.Drawing.Point(309, 138);
            this.outputV.Name = "outputV";
            this.outputV.Size = new System.Drawing.Size(101, 25);
            this.outputV.TabIndex = 25;
            this.outputV.Text = "v.0001";
            this.outputV.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(335, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "Stop";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonStopClick);
            // 
            // trainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelV);
            this.Controls.Add(this.outputV);
            this.Controls.Add(this.buttonTrain);
            this.Name = "trainForm";
            this.Text = "trainForm";
            this.Load += new System.EventHandler(this.trainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonTrain;
        private System.Windows.Forms.Label labelV;
        private System.Windows.Forms.Label outputV;
        private System.Windows.Forms.Button button1;
    }
}