using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Аналізатор
{
    public partial class VariableEntering : Form
    {
        public string Value;
        public VariableEntering()
        {
            InitializeComponent();
            Value = "";
        }
        private void VariableEntering_Load(object sender, EventArgs e)
        {

        }
        private void EnteringName_Click(object sender, EventArgs e)
        {
            Value = VariableValue.Text;
            this.Hide();
        }
    }
}