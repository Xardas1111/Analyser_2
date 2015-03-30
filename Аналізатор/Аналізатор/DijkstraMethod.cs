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
        Dictionary<string, Operand> operands;
        int ifcount;
        int labelcount;
        public DijkstraMethod()
        {
            lexemtable = new List<Lexem>();
            operands = new Dictionary<string, Operand>();
        }
        public DijkstraMethod(List<Lexem> lexemtable)
        {
            ifcount = 0;
            labelcount = 0;
            this.lexemtable = lexemtable;
            operands = new Dictionary<string, Operand>();
            operands.Add("{", new Operand("{", 0));
            operands.Add("[", new Operand("[", 0));
            operands.Add("(", new Operand("(", 0));
            operands.Add("do", new Operand("do", 0));
            operands.Add("if", new Operand("if", 0));
            operands.Add("]", new Operand("]", 1));
            operands.Add("}", new Operand("}", 1));
            operands.Add(")", new Operand(")", 1));
            operands.Add("while", new Operand("while", 1));
            operands.Add("print", new Operand("print", 2));
            operands.Add("=", new Operand("=", 2));
            operands.Add("OR", new Operand("OR", 3));
            operands.Add("AND", new Operand("AND", 4));
            operands.Add("NOT", new Operand("NOT", 5));
            operands.Add(">", new Operand(">", 6));
            operands.Add("<", new Operand("<", 6));
            operands.Add(">=", new Operand(">=", 6));
            operands.Add("<=", new Operand("<=", 6));
            operands.Add("<>", new Operand("<>", 6));
            operands.Add("==", new Operand("==", 6));
            operands.Add("+", new Operand("+", 7));
            operands.Add("-", new Operand("-", 7));
            operands.Add("*", new Operand("*", 8));
            operands.Add("/", new Operand("/", 8));
        }
        public List<string> CreatePoliz()
        {
            Stack<Operand> stack = new Stack<Operand>();
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
                        poliz.Add("m" + labelcount);
                        poliz.Add("UPL");
                        stack.Peek().AdditionalList.Add("m" + labelcount);
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
                        poliz.Add(stack.Pop().OperandName);
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
                    labelcount++;
                    stack.Push(operands[lexemtable[i].LexName]);
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

    public struct Operand
    {
        public string OperandName;
        public int OperandPriority;
        public List<string> AdditionalList;
        public Operand(string OperandName, int OperandPriority)
        {
            this.OperandName = OperandName;
            this.OperandPriority = OperandPriority;
            AdditionalList = new List<string>();
        }
    }
}