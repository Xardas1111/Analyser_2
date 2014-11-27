using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Аналізатор
{
    public partial class Parser
    {
        int ifcounter = 0;
        int docounter = 0;
        List<Lexem> lexemlist;
        static int lexemcount;
        public Parser(List<Lexem> lexemlist) 
        {
            this.lexemlist = lexemlist;
            lexemcount = 0;
        }

        void MoveToNextLexem() 
        {
            lexemcount++;
        }

        Lexem GetNextLexem() 
        {
            if (lexemcount < lexemlist.Count-1)
                return lexemlist[lexemcount++];
            return new Lexem() { LineNumber = lexemlist[lexemlist.Count - 1].LineNumber };
        }

        Lexem GetLexem() 
        {
            if (lexemcount >= lexemlist.Count)
                return new Lexem(){LineNumber = lexemlist[lexemlist.Count-1].LineNumber};
            return lexemlist[lexemcount];
        }

        public bool IsIdList()
        {
            if (GetLexem().Code == 46)
            {
                MoveToNextLexem();
                if (GetLexem().Code == 17)
                {
                    MoveToNextLexem();
                    if (IsIdList())
                    {
                        return true;
                    }
                    else
                    {
                        throw new ApplicationException("Wrong syntax in line " + GetLexem().LineNumber);
                    }
                }
                else
                {
                    return true;
                }
            }
            else 
            {
                return false;
            }
        }

        public bool IsDefined() 
        {
            if (GetLexem().Code == 1 || GetLexem().Code == 2)
            {
                MoveToNextLexem();
                if (IsIdList())
                {
                    return true;
                }
                else
                {
                    throw new ApplicationException("Need variables in line " + GetLexem().LineNumber);
                }
            }
            else 
            {
                return false;
            }
        }

        public bool IsArithmetic() 
        {
            if (IsTerm())
            {
                if (GetLexem().Code == 18 || GetLexem().Code == 19)
                {
                    MoveToNextLexem();
                    if (IsArithmetic())
                    {
                        return true;
                    }
                    else
                    {
                        throw new ApplicationException("Need an arithmetic equation in line " + GetLexem().LineNumber);
                    }
                }
                else
                {
                    return true;
                }
            }
            else 
            {
                return false;
            }
        }

        public bool IsMultiplier() 
        {
            if (GetLexem().Code == 24)
            {
                MoveToNextLexem();
                if (IsArithmetic())
                {
                    if (GetLexem().Code == 25)
                    {
                        MoveToNextLexem();
                        return true;
                    }
                    else
                    {
                        throw new ApplicationException("Need a closing bracket in line " + GetLexem().LineNumber);
                    }
                }
                else 
                {
                    throw new ApplicationException("Need an arithmetic equation in line " + GetLexem().LineNumber);
                }
            }
            else 
            {
                if (GetLexem().Code == 46 || GetLexem().Code == 47)
                {
                    MoveToNextLexem();
                    return true;
                }
                else
                {
                    return false;
                }
            }          
        }

        public bool IsTerm() 
        {
            if (IsMultiplier())
            {
                if (GetLexem().Code == 20 || GetLexem().Code == 21)
                {
                    MoveToNextLexem();
                    if (IsTerm())
                    {
                        return true;
                    }
                    else
                    {
                        throw new ApplicationException("Need a term in line " + GetLexem().LineNumber);
                    }
                }
                else
                {
                    return true;
                }
            }
            else 
            {
                return false;
            }
        }

        public bool IsAssign() 
        {
            if (GetLexem().Code == 46)
            {
                MoveToNextLexem();
                if (GetLexem().Code == 8)
                {
                    MoveToNextLexem();
                    if (IsArithmetic())
                    {
                        return true;
                    }
                    else
                    {
                        throw new ApplicationException("Need an arithmetic equation in line " + GetLexem().LineNumber);
                    }
                }
                else
                {
                    throw new ApplicationException("Need an operator \'=\' line " + GetLexem().LineNumber);
                }
            }
            else 
            {
                return false;
            }
        }
    }
}