using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace Аналізатор
{
    public class Interpretator
    {
        TextBox textbox;
        List<string> poliz;
        List<Id> idtable;
        List<PairedValue> labeltable;
        VariableEntering varmenu;
        public Interpretator()
        {
            poliz = new List<string>();
            idtable = new List<Id>();
            labeltable = new List<PairedValue>();
        }
        public Interpretator(List<string> poliz, List<Id> idtable, List<PairedValue> labeltable, TextBox textbox)
        {
            this.labeltable = labeltable;
            this.poliz = poliz;
            this.idtable = idtable;
            this.textbox = textbox;
        }
        public void Interprate()
        {
            Stack<string> stack = new Stack<string>();
            string a1 = "", a2 = "";
            for (int i = 0; i < poliz.Count; i++)
            {
                switch (poliz[i])
                {
                    case "+":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(GetValue(a1) + GetValue(a2), new CultureInfo("en-Us"));
                        stack.Push(a1);
                        break;
                    case "-":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(GetValue(a1) - GetValue(a2), new CultureInfo("en-Us"));
                        stack.Push(a1);
                        break;
                    case "*":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(GetValue(a1) * GetValue(a2), new CultureInfo("en-Us"));
                        stack.Push(a1);
                        break;
                    case "/":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        if (GetValue(a2) == 0)
                        {
                            MessageBox.Show("DEV/NULL", "ERROR!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        a1 = Convert.ToString(GetValue(a1) / GetValue(a2), new CultureInfo("en-Us"));
                        stack.Push(a1);
                        break;
                    case "AND":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(Convert.ToBoolean(a2) && Convert.ToBoolean(a1));
                        stack.Push(a1);
                        break;
                    case "OR":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(Convert.ToBoolean(a2) || Convert.ToBoolean(a1));
                        stack.Push(a1);
                        break;
                    case "NOT":
                        a1 = stack.Pop();
                        a1 = Convert.ToString(!Convert.ToBoolean(a1));
                        stack.Push(a1);
                        break;
                    case "=":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        for (int j = 0; j < idtable.Count; j++)
                        {
                            if (idtable[j].IdName == a1)
                            {
                                string value = "";
                                if (idtable[j].IdType == "int")
                                {
                                    value = Convert.ToInt32(GetValue(a2)).ToString();
                                }
                                else
                                {
                                    value = Convert.ToString(GetValue(a2), new CultureInfo("en-Us"));
                                }
                                idtable[j] = new Id(idtable[j].IdName, idtable[j].IdCode, idtable[j].IdType) { Value = value };
                                break;
                            }
                        }
                        break;
                    case "print":
                        a1 = stack.Pop();
                        textbox.Text += Convert.ToString(GetValue(a1), new CultureInfo("en-US")) + Environment.NewLine;
                        break;
                    case ">":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) > GetValue(a2)).ToString());
                        break;
                    case ">=":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) >= GetValue(a2)).ToString());
                        break;
                    case "<=":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) <= GetValue(a2)).ToString());
                        break;
                    case "<":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) < GetValue(a2)).ToString());
                        break;
                    case "==":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) == GetValue(a2)).ToString());
                        break;
                    case "<>":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) != GetValue(a2)).ToString());
                        break;
                    case "UPL":
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        if (!Convert.ToBoolean(a1))
                        {
                            for (int j = 0; j < labeltable.Count; j++)
                            {
                                if (a2 == labeltable[j].OperandName)
                                {
                                    i = labeltable[j].OperandPriority;
                                    break;
                                }
                            }
                        }
                        break;
                    case "BP":
                        a1 = stack.Pop();
                        for (int j = 0; j < labeltable.Count; j++)
                        {
                            if (a1 == labeltable[j].OperandName)
                            {
                                i = labeltable[j].OperandPriority;
                                break;
                            }
                        }
                        break;
                    case "scan":
                        if (stack.Count != 0)
                        {
                            a1 = stack.Pop();
                            varmenu = new VariableEntering();
                            for (int j = 0; j < idtable.Count; j++)
                            {
                                if (a1 == idtable[j].IdName)
                                {
                                    varmenu.SetName = a1;
                                    break;
                                }
                            }
                        mark:
                            varmenu.ShowDialog();
                            for (int j = 0; j < idtable.Count; j++)
                            {
                                if (idtable[j].IdName == a1)
                                {
                                    string value = "";
                                    if (idtable[j].IdType == "int")
                                    {
                                        int test;
                                        if (!int.TryParse(varmenu.Value, NumberStyles.Any, new CultureInfo("en-US"), out test))
                                        {
                                            goto mark;
                                        }
                                        value = Convert.ToInt32(GetValue(varmenu.Value)).ToString();
                                    }
                                    else
                                    {
                                        double test;
                                        if (!double.TryParse(varmenu.Value, NumberStyles.Any, new CultureInfo("en-US"), out test))
                                        {
                                            goto mark;
                                        }
                                        value = Convert.ToString(GetValue(varmenu.Value), new CultureInfo("en-Us"));
                                    }
                                    idtable[j] = new Id(idtable[j].IdName, idtable[j].IdCode, idtable[j].IdType) { Value = value };
                                    break;
                                }
                            }
                            varmenu.Close();
                        }
                        break;
                    default:
                        stack.Push(poliz[i]);
                        break;
                }
            }
        }
        double GetValue(string a)
        {
            string Value = "";
            if (!char.IsDigit(a[0]) && a[0] != '-')
            {
                for (int i = 0; i < idtable.Count; i++)
                {
                    if (a == idtable[i].IdName)
                    {
                        Value = idtable[i].Value;
                        break;
                    }
                }
            }
            else
            {
                Value = a;
            }
            return Convert.ToDouble(Value, new CultureInfo("en-US"));
        }
    }
}