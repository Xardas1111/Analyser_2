using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Аналізатор
{
    public class DijkstraMethod
    {
        List<Lexem> lexemtable;
        Dictionary<string, PairedValue> operands;
        int ifcount;
        int iflabelcount;
        int docount;
        int dolabelcount;
        public DijkstraMethod()
        {
            lexemtable = new List<Lexem>();
            operands = new Dictionary<string, PairedValue>();
        }
        public DijkstraMethod(List<Lexem> lexemtable)
        {
            docount = 0;
            ifcount = 0;
            iflabelcount = 0;
            dolabelcount = 1;
            this.lexemtable = lexemtable;
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
            Stack<PairedValue> stack = new Stack<PairedValue>();
            List<string> poliz = new List<string>();
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
                if ((lexemtable[i].Code == 46) || (lexemtable[i].Code == 47))
                {
                    poliz.Add(lexemtable[i].LexName);
                    continue;
                }
                if (stack.Count == 0)
                {
                    stack.Push(operands[lexemtable[i].LexName]);
                    continue;
                }
                if ((lexemtable[i].Code == 22) || (lexemtable[i].Code == 24) || (lexemtable[i].Code == 28))
                {
                    if ((lexemtable[i].Code == 22) && (ifcount != 0))
                    {
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
                        stack.Push(operands[lexemtable[i].LexName]);
                        continue;
                    }
                }
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
                if (lexemtable[i].Code == 25)
                {
                    while (stack.Peek().OperandName != "(")
                    {
                        poliz.Add(stack.Pop().OperandName);
                    }
                    stack.Pop();
                    continue;
                }
                if (lexemtable[i].Code == 29)
                {
                    while (stack.Peek().OperandName != "[")
                    {
                        poliz.Add(stack.Pop().OperandName);
                    }
                    stack.Pop();
                    continue;
                }
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
                if (lexemtable[i].Code == 7) 
                {
                    ifcount++;
                    iflabelcount += 3;
                    stack.Push(operands[lexemtable[i].LexName]);
                    continue;
                }
                if (lexemtable[i].Code == 6)
                {
                    docount++;
                    dolabelcount += 3;
                    stack.Push(operands[lexemtable[i].LexName]);
                    stack.Peek().AdditionalList.Add("m" + dolabelcount);
                    poliz.Add("m" + dolabelcount + ":");
                    continue;
                }
                while ((stack.Count != 0) && (stack.Peek().OperandPriority >= operands[lexemtable[i].LexName].OperandPriority))
                {
                    poliz.Add(stack.Pop().OperandName);
                }
                stack.Push(operands[lexemtable[i].LexName]);
            }
            return poliz;
        }
    }

    public struct PairedValue
    {
        public string OperandName;
        public int OperandPriority;
        public List<string> AdditionalList;
        public PairedValue(string OperandName, int OperandPriority)
        {
            this.OperandName = OperandName;
            this.OperandPriority = OperandPriority;
            AdditionalList = new List<string>();
        }
    }
}