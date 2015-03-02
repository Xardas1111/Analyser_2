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
        RelationshipTableForm relForm;
        GramaticForm gf;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            relForm = new RelationshipTableForm(RelationshipsTable.GetTable());
            relForm.Show();
            gf = new GramaticForm(RelationshipsTable.GetGramatic());
            gf.Show();
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
            dataGridView3.Rows.Clear();
           
            List<Lexem> table = new List<Lexem>();
            File.WriteAllText("Temp.abc", richTextBox1.Text);
            if (Analyser.Parse("Temp.abc", out table,dataGridView3,relForm.dataGridView1,RelationshipsTable.GetGramarList()))
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
        }
    }
}