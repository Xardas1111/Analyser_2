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
    //
    //         ***************************************
    //               Реалізація виконання поліз   
    //         ***************************************
    //
    public class Interpretator
    {
        TextBox textbox;//Текстове поле для виведення
        List<string> poliz;//Поліз для виконання
        List<Id> idtable;//Таблиця ідентифікаторів
        List<PairedValue> labeltable;//Таблиця міток
        VariableEntering varmenu;//Вікно для введення змінних
        //Конструктор за замовчуванням
        public Interpretator()
        {
            poliz = new List<string>();
            idtable = new List<Id>();
            labeltable = new List<PairedValue>();
        }
        //Конструктор ініціалізації полів
        public Interpretator(List<string> poliz, List<Id> idtable, List<PairedValue> labeltable, TextBox textbox)
        {
            this.labeltable = labeltable;
            this.poliz = poliz;
            this.idtable = idtable;
            this.textbox = textbox;
        }
        //
        //        ***************************************
        //        Інтерпретування поліза і його виконання  
        //        ***************************************
        //
        public void Interprate()
        {
            Stack<string> stack = new Stack<string>();//Робочий стек
            string a1 = "", a2 = "";//Робочі змінні для обчислень
            for (int i = 0; i < poliz.Count; i++)
            {
                switch (poliz[i])
                {
                    case "+"://Додавання
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(GetValue(a1) + GetValue(a2), new CultureInfo("en-Us"));
                        stack.Push(a1);
                        break;
                    case "-"://Віднімання
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(GetValue(a1) - GetValue(a2), new CultureInfo("en-Us"));
                        stack.Push(a1);
                        break;
                    case "*"://Множення
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(GetValue(a1) * GetValue(a2), new CultureInfo("en-Us"));
                        stack.Push(a1);
                        break;
                    case "/"://Ділення
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
                    case "AND"://Логічне І
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(Convert.ToBoolean(a2) && Convert.ToBoolean(a1));
                        stack.Push(a1);
                        break;
                    case "OR"://Логічне АБО
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        a1 = Convert.ToString(Convert.ToBoolean(a2) || Convert.ToBoolean(a1));
                        stack.Push(a1);
                        break;
                    case "NOT"://Логічне НІ
                        a1 = stack.Pop();
                        a1 = Convert.ToString(!Convert.ToBoolean(a1));
                        stack.Push(a1);
                        break;
                    case "="://Присвоєння значення змінній
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
                    case "print"://Виведення на екран арифметичного виразу
                        a1 = stack.Pop();
                        textbox.Text += Convert.ToString(GetValue(a1), new CultureInfo("en-US")) + Environment.NewLine;
                        break;
                    case ">"://Знак більше
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) > GetValue(a2)).ToString());
                        break;
                    case ">="://Знак більше-дорівнює
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) >= GetValue(a2)).ToString());
                        break;
                    case "<="://Знак меньше-дорівнює
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) <= GetValue(a2)).ToString());
                        break;
                    case "<"://Знак меньше
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) < GetValue(a2)).ToString());
                        break;
                    case "=="://Знак дорівнює
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) == GetValue(a2)).ToString());
                        break;
                    case "<>"://Знак недовірнює
                        a2 = stack.Pop();
                        a1 = stack.Pop();
                        stack.Push((GetValue(a1) != GetValue(a2)).ToString());
                        break;
                    case "UPL"://Перехід на мітку по похибці
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
                    case "BP"://Безумовний перехід на мітку
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
                    case "scan"://Введення значення змінної
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
        //
        //       ***********************************************
        //       Метод повернення значення(змінної чи константи)  
        //       ***********************************************
        //
        double GetValue(string a)
        {
            string Value = "";//Повертання значення
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