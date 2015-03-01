using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Trance_4
{
    public partial class GramaticForm : Form
    {
        string gramatic;
        public GramaticForm(string gramatic)
        {
            this.gramatic = gramatic;
            InitializeComponent();
        }

        private void GramaticForm_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = gramatic;
        }

    }
}
