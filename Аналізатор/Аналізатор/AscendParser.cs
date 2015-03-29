using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Trance_4;

namespace Аналізатор
{
    public class AscendParser
    {
        private int count;
        private List<string> Polizlist;
        private List<Lexem> lexemtable;
        private DataGridView relationshiptable;
        private DataGridView outtable;
        private List<GrammarLine> grammarlist;
        private List<string> stack;
        private bool isread = false;

        public AscendParser()
        {
        }

        public AscendParser(List<Lexem> lexemtable, DataGridView relationshiptable, DataGridView outtable, List<GrammarLine> grammarlist)
        {
            this.relationshiptable = relationshiptable;
            this.lexemtable = lexemtable;
            this.outtable = outtable;
            this.grammarlist = grammarlist;
            this.count = 0;
            this.Polizlist = new List<string>();
        }

        private Lexem TryNextLexem()
        {
            if (this.count < this.lexemtable.Count)
                return this.lexemtable[this.count];
            return new Lexem()
            {
                IdCode = -1,
                LexName = ""
            };
        }

        private void NextLexem()
        {
            ++this.count;
        }

        public bool Parse()
        {
            this.stack = new List<string>();
            this.stack.Add("");
            while (true)
            {
                string lexem2 = this.TryNextLexem().Code != 46 ? (this.TryNextLexem().Code != 47 ? this.TryNextLexem().LexName : "const") : "id";
                string relation = this.GetRelation(this.stack[this.stack.Count - 1], lexem2);
                this.FillOutTable(relation);
                if (relation == "<." || relation == ".=")
                {
                    this.stack.Add(lexem2);
                    this.NextLexem();
                }
                else
                {
                    int num = 0;
                    List<string> list = new List<string>();
                    while (this.GetRelation(this.stack[this.stack.Count - 1 - num - 1], this.stack[this.stack.Count - 1 - num]) != "<.")
                        ++num;
                    List<string> range = this.stack.GetRange(this.stack.Count - 1 - num, num + 1);
                    if (!(this.GetGrammar(range) == "~program"))
                    {
                        this.stack.RemoveRange(this.stack.Count - 1 - num, num + 1);
                        this.stack.Add(this.GetGrammar(range));
                        isread = false ;
                    }
                    else
                        break;
                }
            }
            this.outtable.Rows.Add((object)"~program", (object)".>");
            return true;
        }

        private string GetRelation(string lexem1, string lexem2)
        {
            
            if (lexem1 == "")
                return "<.";
            if (lexem2 == "")
                return ".>";
            int index = 0;
            while (index < this.relationshiptable.RowCount && !(this.relationshiptable[0, index].Value.ToString() == lexem1))
                ++index;
            if (this.relationshiptable[lexem2, index] == null)
                throw new ApplicationException("Wrong operator: " + (object)this.lexemtable[this.count].LineNumber);
            return this.relationshiptable[lexem2, index].Value.ToString();
        }

        private void FillOutTable(string relation)
        {
            string str1 = "";
            for (int index = 0; index < this.stack.Count; ++index)
                str1 = str1 + " " + this.stack[index];
            string str2 = "";
            for (int index = this.count; index < this.lexemtable.Count; ++index)
                str2 = str2 + " " + this.lexemtable[index].LexName;
            string str3 = "";
            for (int i = 0; i < Polizlist.Count; i++) 
            {
                str3 += Polizlist[i] + " ";
            }
                this.outtable.Rows.Add((object)str1, (object)relation, (object)str2, (object)str3);
        }

        private string GetGrammar(List<string> temp)
        {
            string str = "";
            for (int index = 0; index < this.grammarlist.Count; ++index)
            {
                if (Enumerable.SequenceEqual<string>((IEnumerable<string>)temp, (IEnumerable<string>)this.grammarlist[index].RightToken))
                {
                    if (!isread)
                    {
                        switch (index)
                        {
                            case 41: Polizlist.Add("+"); break;
                            case 42: Polizlist.Add("-"); break;
                            case 45: Polizlist.Add("*"); break;
                            case 46: Polizlist.Add("/"); break;
                            case 49: Polizlist.Add(lexemtable[count - 1].LexName); break;
                        }
                        
                    }
                    str = this.grammarlist[index].LeftToken;
                    break;
                }
            }
            isread = true;
            if (str == "")
                throw new ApplicationException("Wrong operator: " + (object)this.lexemtable[this.count].LineNumber);
            return str;
        }

        public List<string> GetPoliz() 
        {
            return Polizlist;
        }
    }
}