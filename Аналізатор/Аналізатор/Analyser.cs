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
    partial class Analyser
    {
        
        static bool IsInitialized(string name, out string type)
        {
            type = "";
            int varinit = 0;
            for (int i = LexemTable.Count - 1; i >= 0; i--)
            {
                int j = i;
                List<Lexem> Sublist = new List<Lexem>();
                while ((j != -1) && (LexemTable[j].LineNumber == LexemTable[i].LineNumber))
                {
                    Sublist.Add(LexemTable[j]);
                    j--;
                }
                bool key = true;
                if (LexemTable[i].LineNumber == NumberOfLines)
                    for (int z = 0; z < Sublist.Count; z++)
                    {
                        if (Sublist[z].LexName == name) key = false;
                        if (((Sublist[z].LexName == "int") || (Sublist[z].LexName == "real")) && (key == true))
                        {
                            type = Sublist[z].LexName;
                            varinit++;
                        }
                    }
                else for (int k = 0; k < Sublist.Count; k++)
                    {
                        if ((Sublist[k].LexName == name))
                        {
                            for (int z = k + 1; z < Sublist.Count; z++)
                            {
                                if (Sublist[z].LexName == name) key = false;
                                if (((Sublist[z].LexName == "int") || (Sublist[z].LexName == "real")) && (key == true))
                                {
                                    type = Sublist[z].LexName;
                                    varinit++;
                                }
                            }
                        }
                    }
                i = ++j;
            }
            if ((varinit == 1))
                return true;
            return false;
        }

        static bool IsSeparator(string sub) 
        {
            for (int i = 0; i < separator.Length; i++)
                if (sub == separator[i]) 
                    return true;
            return false;
        }

        static bool IsLexem(string sub)
        {
            for (int i = 0; i < lexem.Length; i++)
                if (sub == lexem[i]) 
                    return true;
            return false;
        }

        static bool IsTerminal(string sub)
        {
            for (int i = 0; i < terminal.Length; i++)
                if (sub == terminal[i]) 
                    return true;
            return false;
        }

        static bool IsId(string sub) 
        {
            if ((sub[0] >= 65) && (sub[0] <= 90) || (sub[0] >= 97) && (sub[0] <= 122))
                for (int i = 1; i < sub.Length; i++)
                {
                    if (!((sub[i] >= 65) && (sub[i] <= 90)) && !((sub[i] >= 97) && (sub[i] <= 122)) && !((sub[i] >= 48) && (sub[i] <= 57))) 
                        return false;
                }
            else return false;
            return true;
        }

        static bool IsConst(string sub) 
        {
            if (!((sub[0] >= 48) && (sub[0] <= 57)) && (sub[0] != '-')) 
                return false;
            bool point = false;
            if (sub[sub.Length - 1] == '.') 
                return false;
            for (int i = 1; i < sub.Length; i++) 
            {
                if ((sub[i] == '.') && (!point))
                {
                    point = true;
                    continue;
                }
                if (!((sub[i] >= 48) && (sub[i] <= 57)) || (point && (sub[i] == '.')))
                    return false;
            }
            return true;
        }

        static bool IsValid(string sub) 
        {
            for (int i = 0; i < sub.Length; i++) 
            {
                if (!(sub[i] == 46) && !((sub[i] >= 65) && (sub[i] <= 90)) && !((sub[i] >= 97) && (sub[i] <= 122)) && !((sub[i] >= 48) && (sub[i] <= 57)) && !(IsSeparator(Convert.ToString(sub[i])))) 
                    return false;
            }
            return true;
        }
        static bool GetLexem(string name) 
        {
            if (IsLexem(name))
            {
                int c = 0;
                for (int i=0;i<lexem.Length;i++)
                    if(name == lexem[i]) c = i+1;
                LexemTable.Add(new Lexem (++LexemNumber, NumberOfLines, name, c, 0));
                return true;
            }

            if (IsId(name)) 
            {
                string type;
                if (IsInitialized(name, out type))
                {
                    int code = 0 ;
                    bool key = false;
                    for (int i = 0; i < IdTable.Count; i++)
                        if ((name == IdTable[i].IdName))
                        {
                            if (type != IdTable[i].IdType) return false;
                            key = true;
                            code = IdTable[i].IdCode;
                            break;
                        }
                    if (!key)
                    {
                        IdTable.Add(new Id(name, ++IdCodeNumber, type));
                        code = IdCodeNumber;
                    }
                    LexemTable.Add(new Lexem(++LexemNumber, NumberOfLines, name, 46, code));
                }
                else return false;
                return true;
            }

            if (IsConst(name))
            {
                bool key = false;
                int Number = 0;
                for (int i = 0; i < ConstCodeNumber; i++) 
                {
                    if (ConstTable[i].ConstName == name)
                    {
                        key = true;
                        Number = ConstTable[i].ConstCode;
                        break;
                    }
                }
                if (!key)
                {
                    ConstTable.Add(new Const(name, ++ConstCodeNumber));
                    Number = ConstCodeNumber;
                }
                LexemTable.Add(new Lexem(++LexemNumber, NumberOfLines, name, 47, Number));
                return true;
            }
            return false;
        }

        public static bool Parse(string path, out List<Lexem> table, out Dictionary<int,Parser2.keeper> states) 
        {
            LexemNumber = 0;
            NumberOfLines = 0;
            ConstCodeNumber = 0;
            IdCodeNumber = 0;
            string line;
            states = new Dictionary<int, Parser2.keeper>();
            LexemTable = new List<Lexem>();
            IdTable = new List<Id>();
            ConstTable = new List<Const>();
            StreamReader str = new StreamReader(path);
            bool key = true;
            while (!str.EndOfStream && (key == true))
            {
                NumberOfLines++;
                line = str.ReadLine();
                if (!IsValid(line))
                {
                    MessageBox.Show("Error in line " + NumberOfLines, "Error", MessageBoxButtons.OK);
                    key = false;
                    break;
                }
                string subline = "";
                int i = 0;
                for (i = 0; i < line.Length; i++)
                {
                    subline += line[i];
                    if ((subline == " ") || (subline == "\t"))
                    {
                        subline = "";
                        continue;
                    }
                    if ((subline == "-") && (i < line.Length-1) && (line[i + 1] <= 57) && (line[i + 1] >= 46))
                    {
                        if ((LexemTable[LexemNumber - 1].IdCode != 0) || (LexemTable[LexemNumber-1].Code == 25))
                        {
                            GetLexem(subline);
                            subline = "";
                            continue;
                        }
                        continue;
                    }
                    if (IsSeparator(subline))
                    {
                        if (i < line.Length - 1)
                        {
                            if (IsSeparator(subline + line[i + 1]) && (line.Length > 2))
                            {
                                i++;
                                if (!GetLexem(subline + line[i]))
                                {
                                    MessageBox.Show("Error in line " + NumberOfLines, "Error", MessageBoxButtons.OK);
                                    key = false;
                                    break;
                                }
                                subline = "";
                                continue;
                            }
                            else
                            {
                                if (!GetLexem(subline))
                                {
                                    MessageBox.Show("Error in line " + NumberOfLines, "Error", MessageBoxButtons.OK);
                                    key = false;
                                    break;
                                }
                                subline = "";
                                continue;
                            }
                        }
                    }
                    if ((i < line.Length - 1) && (subline != ""))
                    {
                        if ((line.Length > 2) && IsSeparator(Convert.ToString(line[i + 1])))
                        {
                            if (!GetLexem(subline))
                            {
                                MessageBox.Show("Error in line " + NumberOfLines, "Error", MessageBoxButtons.OK);
                                key = false;
                                break;
                            }
                            subline = "";
                            continue;
                        }
                    }
                    if ((i == line.Length - 1) && (subline != ""))
                    {
                        if (!GetLexem(subline))
                        {
                            MessageBox.Show("Error in line " + NumberOfLines, "Error", MessageBoxButtons.OK);
                            key = false;
                            break;
                        }
                        subline = "";
                        continue;
                    }
                }
                if (str.Peek() != -1)
                    LexemTable.Add(new Lexem(++LexemNumber, NumberOfLines, "¶", 27, 0));
            }
            str.Close();
            table = new List<Lexem>();
            if (key)
            {
                table = LexemTable;
                Parser2.Parser parser = new Parser2.Parser(LexemTable);
                try
                {
                    if (parser.check(out states))
                    {
                        return true;
                    }
                }
                catch (ApplicationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
            return false;
        }
    }
}