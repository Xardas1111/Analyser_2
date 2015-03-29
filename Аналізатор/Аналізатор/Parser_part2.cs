using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Parser2
{
    public class keeper
    {
        public List<int> labels { get; set; }
        public List<int> nextstate { get; set; }
        public int additionalstack { get; set; }
        public List<int> stack { get; set; }
        public object notequals { get; set; }

        public keeper(List<int> labels, List<int> state, List<int> stack, int additionalstack, object notequals)
        {
            this.notequals = notequals;
            this.labels = labels;
            this.stack = stack;
            this.nextstate = state;
            this.additionalstack = additionalstack;
        }
    }
    public class Parser
    {
        List<Аналізатор.Lexem> Lexems;
        Аналізатор.Lexem CurrentLexem;
        Dictionary<int, keeper> States = new Dictionary<int, keeper>();
        int numberOfState;
        Stack<int> stack = new Stack<int>();
        int count;
        public Parser(List<Аналізатор.Lexem> Lexems)
        {
            this.Lexems = new List<Аналізатор.Lexem>(Lexems);
            count = 0;
            numberOfState = 1;
            States.Add(1, new keeper(new List<int> { 22 }, new List<int> { 2 }, new List<int> { 0 }, 0, "Opening bracket expected"));
            States.Add(2, new keeper(new List<int> { 27 }, new List<int> { 3 }, new List<int> { 0 }, 0, "new line expected"));
            States.Add(3, new keeper(new List<int> { 23 }, new List<int> { 0 }, new List<int> { 0 }, 4, 5));
            States.Add(4, new keeper(new List<int> { 27 }, new List<int> { 3 }, new List<int> { 0 }, 0, "new line expected"));
            States.Add(5, new keeper(new List<int> { 1, 2, 46, 3, 4, 7, 6 }, new List<int> { 26, 26, 6, 9, 11, 31, 22 }, new List<int> { 0, 0, 0, 0, 0, 17, 0 }, 0, "operand expected"));
            States.Add(6, new keeper(new List<int> { 8 }, new List<int> { 28 }, new List<int> { 8 }, 0, "'=' expected"));
            States.Add(8, new keeper(new List<int> { 0 }, new List<int> { 0 }, new List<int> { 0 }, 0, 0));
            States.Add(9, new keeper(new List<int> { 24 }, new List<int> { 28 }, new List<int> { 10 }, 0, "opening bracket missing"));
            States.Add(10, new keeper(new List<int> { 25 }, new List<int> { 0 }, new List<int> { 0 }, 0, "closing bracket missing"));
            States.Add(11, new keeper(new List<int> { 24 }, new List<int> { 12 }, new List<int> { 0 }, 0, "opening bracket missing"));
            States.Add(12, new keeper(new List<int> { 46, 25 }, new List<int> { 13, 0 }, new List<int> { 0, 0 }, 0, "id or closing bracket expected"));
            States.Add(13, new keeper(new List<int> { 25, 17 }, new List<int> { 0, 14 }, new List<int> { 0, 0 }, 0, "',' or closing bracket expected"));
            States.Add(14, new keeper(new List<int> { 46 }, new List<int> { 13 }, new List<int> { 0 }, 0, "id expected"));
            //States.Add(16, new keeper(new List<int> { 27 }, new List<int> { 17 }, new List<int> { 0 }, 0, "new line expected"));
            States.Add(17, new keeper(new List<int> { 22 }, new List<int> { 19 }, new List<int> { 0 }, 0, "opening bracket expected"));
            //States.Add(18, new keeper(new List<int> { 27 }, new List<int> { 19 }, new List<int> { 0 }, 0, "new line expected"));
            States.Add(19, new keeper(new List<int> { 23 }, new List<int> { 0 }, new List<int> { 0 }, 20, 5));
            States.Add(20, new keeper(new List<int> { 27 }, new List<int> { 19 }, new List<int> { 0 }, 0, "new line expected"));
            //States.Add(21, new keeper(new List<int> { 27 }, new List<int> { 22 }, new List<int> { 0 }, 0, "new line expected"));
            States.Add(22, new keeper(new List<int> { 5 }, new List<int> { 31 }, new List<int> { 25 }, 23, 5));
            States.Add(23, new keeper(new List<int> { 27 }, new List<int> { 22 }, new List<int> { 0 }, 0, "new line expected"));
            States.Add(25, new keeper(new List<int> { 0 }, new List<int> { 0 }, new List<int> { 0 }, 0, 0));
            States.Add(26, new keeper(new List<int> { 46 }, new List<int> { 27 }, new List<int> { 0 }, 0, "id expected"));
            States.Add(27, new keeper(new List<int> { 17 }, new List<int> { 26 }, new List<int> { 0 }, 0, 0));
            States.Add(28, new keeper(new List<int> { 24, 46, 47 }, new List<int> { 28, 30, 30 }, new List<int> { 29, 0, 0 }, 0, "need an arithmetic expression"));
            States.Add(29, new keeper(new List<int> { 25 }, new List<int> { 30 }, new List<int> { 0 }, 0, "closing bracket expected"));
            States.Add(30, new keeper(new List<int> { 18, 19, 20, 21 }, new List<int> { 28, 28, 28, 28 }, new List<int> { 0, 0, 0, 0 }, 0, 0));
            States.Add(31, new keeper(new List<int> { 26, 28 }, new List<int> { 31, 31 }, new List<int> { 0, 34 }, 32, 28));
            States.Add(32, new keeper(new List<int> { 9, 10, 11, 12, 13, 14 }, new List<int> { 28, 28, 28, 28, 28, 28 }, new List<int> { 33, 33, 33, 33, 33, 33 }, 0, "logical sign missing"));
            States.Add(33, new keeper(new List<int> { 15, 16 }, new List<int> { 31, 31 }, new List<int> { 0,0 }, 0, 0));
            States.Add(34, new keeper(new List<int> { 29 }, new List<int> { 33 }, new List<int> { 0 }, 0, "closing bracket expected"));
        }
        Аналізатор.Lexem GetNextLexem()
        {
            if (count >= Lexems.Count)
            {
                return null;
            }
            else
            {
                CurrentLexem = Lexems[count++];
                return CurrentLexem;
            }
        }
        public bool check(out Dictionary<int, keeper> states)
        {
            states = States;
            while (true)
            {
                bool found = false;
                Аналізатор.Lexem Lexem = GetNextLexem();
                if (Lexem == null)
                    throw new ApplicationException("{ expected in line:" + Lexems[count - 1].LineNumber);
                for (int i = 0; i < States[numberOfState].labels.Count; i++)
                    if (Lexem.Code == States[numberOfState].labels[i])
                    {
                        if (States[numberOfState].nextstate[i] == 0)
                        {
                            if (stack.Count > 0)
                            {
                                numberOfState = stack.Pop();
                                found = true;
                                break;
                            }
                            else
                            {
                                if (Lexems.Count - count > 0)
                                {
                                    throw new ApplicationException("After the last scope cannot be any text Line:" + Lexem.LineNumber);
                                }
                                return true;
                            }
                        }
                        else
                        {
                            if (States[numberOfState].stack[i] != 0)
                            {
                                stack.Push(States[numberOfState].stack[i]);
                                numberOfState = States[numberOfState].nextstate[i];
                                found = true;
                                break;
                            }
                            else
                            {
                                numberOfState = States[numberOfState].nextstate[i];
                                found = true;
                                break;

                            }
                        }
                    }

                if (!found)
                {
                    if (States[numberOfState].notequals is int)
                    {
                        if ((int)States[numberOfState].notequals == 0)
                        {
                            numberOfState = stack.Pop();
                            count--;
                            found = true;
                        }
                        else
                        {
                            stack.Push(States[numberOfState].additionalstack);
                            numberOfState = (int)States[numberOfState].notequals;
                            count--;
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    throw new ApplicationException(States[numberOfState].notequals as string + " at line " + Lexem.LineNumber);
                }
            }
        }
    }
}