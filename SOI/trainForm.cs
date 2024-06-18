using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOI
{
    public partial class trainForm : Form
    {
        Model model;
        bool training = false;  
        public trainForm(Model model)
        {
            InitializeComponent();
            this.model = model;
        }

        private void trainForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonTrainClick(object sender, EventArgs e)
        {
            training = true;
            while (training)
                model.Train();

            outputV.Text = "v." + model.version.ToString().;
        }

        private void buttonStopClick(object sender, EventArgs e)
        {
            training = false;
        }
    }
}
