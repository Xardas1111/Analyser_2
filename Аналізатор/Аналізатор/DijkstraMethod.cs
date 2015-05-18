using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Аналізатор
{
    //
    //              ***************************************
    //                    Реалізація метода Дійкстри     
    //              ***************************************
    //
    public class DijkstraMethod
    {
        List<Lexem> lexemtable;//таблиця лексем
        Dictionary<string, PairedValue> operands;//словник операторів та їх приоритетів
        int ifcount;//лічильник входження в умовний оператор
        int iflabelcount;//умовного оператора
        int docount;//лічильник входження в цикл
        int dolabelcount;//лічильник міток для цикла
        //конструктор за замовчуванням
        public DijkstraMethod()
        {
            lexemtable = new List<Lexem>();
            operands = new Dictionary<string, PairedValue>();
        }
        //конструктор ініціалізації полів класу
        public DijkstraMethod(List<Lexem> lexemtable)
        {
            //Ініціалізація лічильників
            docount = 0;
            ifcount = 0;
            iflabelcount = 0;
            dolabelcount = 1;
            // Передача лексем в об'єкт
            this.lexemtable = lexemtable;
            /* Додавання операторів та їх приоритетів в словник */
            operands = new Dictionary<string, PairedValue>();
            operands.Add("{", new PairedValue("{", 0));
            operands.Add("[", new PairedValue("[", 0));
            operands.Add("(", new PairedValue("(", 0));
            operands.Add("do", new PairedValue("do", 0));
            operands.Add("if", new PairedValue("if", 0));
            operands.Add("]", new PairedValue("]", 1));
            operands.Add("}", new PairedValue("}", 1));
            operands.Add(")", new PairedValue(")", 1));
            operands.Add("while", new PairedValue("while", 1));
            operands.Add("print", new PairedValue("print", 2));
            operands.Add("=", new PairedValue("=", 2));
            operands.Add("OR", new PairedValue("OR", 3));
            operands.Add("AND", new PairedValue("AND", 4));
            operands.Add("NOT", new PairedValue("NOT", 5));
            operands.Add(">", new PairedValue(">", 6));
            operands.Add("<", new PairedValue("<", 6));
            operands.Add(">=", new PairedValue(">=", 6));
            operands.Add("<=", new PairedValue("<=", 6));
            operands.Add("<>", new PairedValue("<>", 6));
            operands.Add("==", new PairedValue("==", 6));
            operands.Add("+", new PairedValue("+", 7));
            operands.Add("-", new PairedValue("-", 7));
            operands.Add("*", new PairedValue("*", 8));
            operands.Add("/", new PairedValue("/", 8));
        }
        public List<string> CreatePoliz()
        {
            //Створення стеку, необхідного для роботи
            Stack<PairedValue> stack = new Stack<PairedValue>();
            //Створення списку для полізу
            List<string> poliz = new List<string>();
            //Ігнорування ініціалізації змінних
            for (int i = 0; i < lexemtable.Count; i++)
            {
                if (lexemtable[i].Code < 3)
                {
                    int j = 0;
                    while (lexemtable[i + j].Code != 27)
                    {
                        j++;
                    }
                    i = i + j;
                    continue;
                }
                //Додавання в поліз змінних і констант
                if ((lexemtable[i].Code == 46) || (lexemtable[i].Code == 47))
                {
                    poliz.Add(lexemtable[i].LexName);
                    continue;
                }
                //Перевірка, чи стек пустий і додавання в стек оператора
                if (stack.Count == 0)
                {
                    stack.Push(operands[lexemtable[i].LexName]);
                    continue;
                }
                //Додавання в стек відкриваючих дужок
                if ((lexemtable[i].Code == 22) || (lexemtable[i].Code == 24) || (lexemtable[i].Code == 28))
                {
                    //Перевірка на те, чи поточна дужка в умовному операторі
                    if ((lexemtable[i].Code == 22) && (ifcount != 0))
                    {
                        //Виштовхування всього до if і генерація в поліз відповідних оператрів
                        while (stack.Peek().OperandName != "if")
                        {
                            poliz.Add(stack.Pop().OperandName);
                        }
                        poliz.Add("m" + iflabelcount);
                        poliz.Add("UPL");
                        stack.Peek().AdditionalList.Add("m" + iflabelcount);
                        continue;
                    }
                    else
                    {
                        //Додавання в стек оператора
                        stack.Push(operands[lexemtable[i].LexName]);
                        continue;
                    }
                }
                //Перевірка на ¶ і виконання відповідних дій
                if (lexemtable[i].Code == 27)
                {
                    while ((stack.Peek().OperandName != "{") && (stack.Peek().OperandName != "if") && (stack.Peek().OperandName != "do"))
                    {
                        if (stack.Peek().OperandName == "while")
                        {
                            stack.Pop();
                            poliz.Add("m" + (Convert.ToInt32(stack.Peek().AdditionalList[docount - 1].Substring(1, stack.Peek().AdditionalList[docount - 1].Length - 1)) + 1));
                            poliz.Add("UPL");
                            poliz.Add(stack.Peek().AdditionalList[docount - 1]);
                            poliz.Add("BP");
                            poliz.Add("m" + (Convert.ToInt32(stack.Peek().AdditionalList[docount - 1].Substring(1, stack.Peek().AdditionalList[docount - 1].Length - 1)) + 1) + ":");
                            if (docount == 1) 
                            {
                                stack.Peek().AdditionalList.Clear();
                            }
                            stack.Pop();
                            docount--;
                            break;
                        }
                        else
                        {
                            poliz.Add(stack.Pop().OperandName);
                        }
                    }
                    continue;
                }
                //Виконання дій, якщо лексема - закриваюча дужка }
                if (lexemtable[i].Code == 23)
                {
                    if (ifcount != 0)
                    {
                        while (stack.Peek().OperandName != "if")
                        {
                            poliz.Add(stack.Pop().OperandName);
                        }
                        poliz.Add(stack.Peek().AdditionalList[ifcount-1] + ":");
                        if (ifcount == 1)
                        {
                            stack.Peek().AdditionalList.Clear();
                        }
                        stack.Pop();
                        ifcount--;
                        
                        continue;
                    }
                    else
                    {
                        while (stack.Peek().OperandName != "{")
                        {
                            poliz.Add(stack.Pop().OperandName);
                        }
                        stack.Pop();
                        continue;
                    }
                }
                //Виконання дій, якщо лексема - закриваюча дужка )
                if (lexemtable[i].Code == 25)
                {
                    while (stack.Peek().OperandName != "(")
                    {
                        poliz.Add(stack.Pop().OperandName);
                    }
                    stack.Pop();
                    continue;
                }
                //Виконання дій, якщо лексема - закриваюча дужка ]
                if (lexemtable[i].Code == 29)
                {
                    while (stack.Peek().OperandName != "[")
                    {
                        poliz.Add(stack.Pop().OperandName);
                    }
                    stack.Pop();
                    continue;
                }
                // scan
                if (lexemtable[i].Code == 4) 
                {
                    i += 2;
                    while (lexemtable[i].Code != 27) 
                    {
                        if (lexemtable[i].Code == 46)
                        {
                            poliz.Add(lexemtable[i].LexName);
                            i++;
                        }
                        else 
                        {
                            poliz.Add("scan");
                            i++;
                        }
                    }
                    continue;
                }
                // if
                if (lexemtable[i].Code == 7) 
                {
                    ifcount++;
                    iflabelcount += 3;
                    stack.Push(operands[lexemtable[i].LexName]);
                    continue;
                }
                // do
                if (lexemtable[i].Code == 6)
                {
                    docount++;
                    dolabelcount += 3;
                    stack.Push(operands[lexemtable[i].LexName]);
                    stack.Peek().AdditionalList.Add("m" + dolabelcount);
                    poliz.Add("m" + dolabelcount + ":");
                    continue;
                }
                //Порівнюємо приоритет поточного оператора і прирівнюємо його до оператора на вершині стеку
                while ((stack.Count != 0) && (stack.Peek().OperandPriority >= operands[lexemtable[i].LexName].OperandPriority))
                {
                    poliz.Add(stack.Pop().OperandName);
                }
                stack.Push(operands[lexemtable[i].LexName]);
            }
            //Повертаємо поліз
            return poliz;
        }
    }
    //
    //              ***************************************
    //                 Робоча структура для роботи метода
    //              ***************************************
    //
    public struct PairedValue
    {
        public string OperandName;//Назва оператора
        public int OperandPriority;//Приоритет оператора
        public List<string> AdditionalList;//Додаткові поля(прикріплені мітки)
        //Конструктор
        public PairedValue(string OperandName, int OperandPriority)
        {
            this.OperandName = OperandName;
            this.OperandPriority = OperandPriority;
            AdditionalList = new List<string>();
        }
    }
}