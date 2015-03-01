using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Trance_4;

namespace Аналізатор
{
    public partial class Form1 : Form
    {
        string path;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FileDialog a = new OpenFileDialog();
            if (a.ShowDialog() == DialogResult.OK)
            {
                path = a.FileName;
                textBox1.Text = path;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (File.Exists(@path))
            {
                richTextBox1.Text = File.ReadAllText(@path);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog a = new SaveFileDialog();
            if (a.ShowDialog() == DialogResult.OK)
            {
                path = a.FileName;
                File.WriteAllText(path, richTextBox1.Text);
                textBox1.Text = path;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (path != "")
            {
                File.WriteAllText(path, richTextBox1.Text);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Dictionary<int, Parser2.keeper> states;
            List<Lexem> table = new List<Lexem>();
            File.WriteAllText("Temp.abc", richTextBox1.Text);
            if (Analyser.Parse("Temp.abc", out table, out states))
            {
                MessageBox.Show("All is correct", "Result", MessageBoxButtons.OK);
            }
            dataGridView1.Rows.Clear();
            for (int i = 0; i < table.Count; i++)
            {
                dataGridView1.Rows.Add(table[i].Number, table[i].LineNumber, table[i].LexName, table[i].Code, table[i].IdCode);
            }
            File.Delete("Temp.abc");
            dataGridView2.Rows.Clear();
            for (int i = 1; i <= 34; i++)
            {
                if (states.ContainsKey(i))
                    for (int j = 0; j < states[i].labels.Count; j++)
                    {
                        dataGridView2.Rows.Add(i, states[i].labels[j].ToString() == "0" ? "" : states[i].labels[j].ToString(), states[i].nextstate[j], states[i].stack[j]);
                    }
            }
            RelationshipTableForm relForm = new RelationshipTableForm(RelationshipsTable.GetTable());
            relForm.Show();
            MessageBox.Show(relForm.dataGridView1[0, 0].Value.ToString());
            GramaticForm gf = new GramaticForm(RelationshipsTable.GetGramatic());
            gf.Show();
        }
    }
}