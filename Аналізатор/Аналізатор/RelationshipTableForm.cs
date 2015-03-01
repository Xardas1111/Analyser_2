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
    public partial class RelationshipTableForm : Form
    {
        List<List<string>> outRelationshipTable = new List<List<string>>();
        public RelationshipTableForm(List<List<string>> _outRelationshipTable)
        {
            this.outRelationshipTable = new List<List<string>>(_outRelationshipTable);
            InitializeComponent();
        }

        private void RelationshipTableForm_Load(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Add("1\\2","1\\2");
            foreach (string s in outRelationshipTable[0])
            {
                dataGridView1.Columns.Add(s, s);
            }
            for (int i = 1; i < outRelationshipTable.Count; i++)
            {
                dataGridView1.Rows.Add();
                for (int j = 0; j < outRelationshipTable[i].Count; j++)
                    dataGridView1.Rows[i - 1].Cells[j].Value = outRelationshipTable[i][j];
            }
        }
    }
}
