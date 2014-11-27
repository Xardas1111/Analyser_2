using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Аналізатор
{
    public partial class Parser
    {
        public bool IsIf() 
        {
            if (GetLexem().Code == 7)
            {
                ifcounter++;
                MoveToNextLexem();
                if (IsLogExpression())
                {
                    if (GetLexem().Code == 27)
                    {
                        MoveToNextLexem();
                        if (GetLexem().Code == 22)
                        {
                            MoveToNextLexem();
                            if (GetLexem().Code == 27)
                            {
                                MoveToNextLexem();
                                if (IsOpList())
                                {
                                    if (GetLexem().Code == 23)
                                    {
                                        ifcounter--;
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
                                    throw new ApplicationException("Need an operation list in line " + GetLexem().LineNumber);
                                }
                            }
                            else
                            {
                                throw new ApplicationException("Need to start from a new line in line " + GetLexem().LineNumber);
                            }
                        }
                        else
                        {
                            throw new ApplicationException("Need an opening bracket in line " + GetLexem().LineNumber);
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Need to start from a new line in line " + GetLexem().LineNumber);
                    }
                }
                else
                {
                    throw new ApplicationException("Need a logical expression in line " + GetLexem().LineNumber);
                }
            }
            else 
            {
                return false;
            }
        }

        public bool IsWhile() 
        {
            if (GetLexem().Code == 6)
            {
                docounter++;
                MoveToNextLexem();
                if (GetLexem().Code == 27)
                {
                    MoveToNextLexem();
                    if (IsOpList())
                    {
                        if (GetLexem().Code == 5)
                        {
                            MoveToNextLexem();
                            if (IsLogExpression())
                            {
                                docounter--;
                                return true;
                            }
                            else
                            {
                                throw new ApplicationException("Need a logical expression in line " + GetLexem().LineNumber);
                            }
                        }
                        else
                        {
                            throw new ApplicationException("\'while\' operand is needed in line " + GetLexem().LineNumber);
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Needs an operation list in line " + GetLexem().LineNumber);
                    }
                }
                else
                {
                    throw new ApplicationException("Needs to start from a new line in line " + GetLexem().LineNumber);
                }
            }
            else 
            {
                return false;
            }
        }

        public bool IsPrint()
        {
            if (GetLexem().Code == 3)
            {
                MoveToNextLexem();
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
                            throw new ApplicationException("Needs a closing bracket in line " + GetLexem().LineNumber);
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Needs an arithmetic expression in line " + GetLexem().LineNumber);
                    }
                }
                else
                {
                    throw new ApplicationException("Needs an opening bracket in line " + GetLexem().LineNumber);
                }
            }
            else 
            {
                return false;
            }
        }

        public bool IsScan() 
        {
            if (GetLexem().Code == 4)
            {
                MoveToNextLexem();
                if (GetLexem().Code == 24)
                {
                    MoveToNextLexem();
                    if (IsIdList())
                    {
                        if (GetLexem().Code == 25)
                        {
                            MoveToNextLexem();
                            return true;
                        }
                        else
                        {
                            throw new ApplicationException("Needs an closing bracket in line " + GetLexem().LineNumber);
                        }
                    }
                    else
                    {
                        if (GetLexem().Code == 25)
                        {
                            MoveToNextLexem();
                            return true;
                        }
                        else
                        {
                            throw new ApplicationException("Needs correct syntax in line " + GetLexem().LineNumber);
                        }
                    }
                }
                else
                {
                    throw new ApplicationException("Needs an opening bracket in line " + GetLexem().LineNumber);
                }
            }
            else 
            {
                return false;
            }
        }
    }
}