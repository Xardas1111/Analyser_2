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
            //relForm = new RelationshipTableForm(RelationshipsTable.GetTable());
            //relForm.Show();
            //gf = new GramaticForm(RelationshipsTable.GetGramatic());
            //gf.Show();
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
            List<Id> idtable = new List<Id>();
            dataGridView3.Rows.Clear();
            Dictionary<int, Parser2.keeper> states;
            List<Lexem> table = new List<Lexem>();
            File.WriteAllText("Temp.abc", richTextBox1.Text);
            if (Analyser.Parse("Temp.abc", out table, out states , out idtable))
            {
                MessageBox.Show("All is correct", "Result", MessageBoxButtons.OK);
                dataGridView1.Rows.Clear();
                for (int i = 0; i < table.Count; i++)
                {
                    dataGridView1.Rows.Add(table[i].Number, table[i].LineNumber, table[i].LexName, table[i].Code, table[i].IdCode);
                }
                File.Delete("Temp.abc");
                dataGridView2.Rows.Clear();
                foreach (KeyValuePair<int, Parser2.keeper> pair in states)
                {
                    for (int j = 0; j < pair.Value.labels.Count; j++)
                    {
                        dataGridView2.Rows.Add(pair.Key, pair.Value.labels[j], pair.Value.nextstate[j], pair.Value.stack[j]);
                    }
                }
                DijkstraMethod method = new DijkstraMethod(table);
                List<string> poliz = method.CreatePoliz();
                List<PairedValue> labeltable = new List<PairedValue>();
                for (int i = 0; i < poliz.Count; i++)
                {
                    if ((poliz[i][0] == 'm') && (poliz[i][poliz[i].Length - 1] == ':'))
                    {
                        labeltable.Add(new PairedValue(poliz[i].Substring(0, poliz[i].Length - 1), i));
                        dataGridView3.Rows.Add(poliz[i].Substring(0, poliz[i].Length - 1), i);
                    }
                }
                OutputText.Text = "";
                MessageBox.Show(poliz.Aggregate("", (current, t) => current + (" " + t)));
                Interpretator interpretator = new Interpretator(poliz, idtable, labeltable, OutputText);
                interpretator.Interprate();
            }
            
        }
        private string CalculatePoliz(List<string> poliz)
        {
            int a1 = 0, a2 = 0;
            int i = 0;
            while (i < poliz.Count)
            {
                switch (poliz[i])
                {
                    case "+":
                        a1 = int.Parse(poliz[i - 1]);
                        a2 = int.Parse(poliz[i - 2]);
                        a2 += a1;
                        poliz.RemoveAt(i);
                        poliz.RemoveAt(i - 1);
                        poliz.RemoveAt(i - 2);
                        poliz.Insert(i - 2, a2.ToString());
                        i--;
                        break;
                    case "-":
                        a1 = int.Parse(poliz[i - 1]);
                        a2 = int.Parse(poliz[i - 2]);
                        a2 -= a1;
                        poliz.RemoveAt(i);
                        poliz.RemoveAt(i - 1);
                        poliz.RemoveAt(i - 2);
                        poliz.Insert(i - 2, a2.ToString());
                        i--;
                        break;
                    case "*":
                        a1 = int.Parse(poliz[i - 1]);
                        a2 = int.Parse(poliz[i - 2]);
                        a2 *= a1;
                        poliz.RemoveAt(i);
                        poliz.RemoveAt(i - 1);
                        poliz.RemoveAt(i - 2);
                        poliz.Insert(i - 2, a2.ToString());
                        i--;
                        break;
                    case "/":
                        a1 = int.Parse(poliz[i - 1]);
                        a2 = int.Parse(poliz[i - 2]);
                        a2 /= a1;
                        poliz.RemoveAt(i);
                        poliz.RemoveAt(i - 1);
                        poliz.RemoveAt(i - 2);
                        poliz.Insert(i - 2, a2.ToString());
                        i--;
                        break;
                    default:
                        i++;
                        break;
                }
            }
            return poliz[0];
        }
    }
}