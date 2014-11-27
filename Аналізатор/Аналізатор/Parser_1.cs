using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Аналізатор
{
    public partial class Parser
    {
        public bool IsLogExpression() 
        {
            if (IsLogTerm())
            {
                if (GetLexem().Code == 16)
                {
                    MoveToNextLexem();
                    if (IsLogExpression())
                    {
                        return true;
                    }
                    else
                    {
                        throw new ApplicationException("Need a logical expression in line " + GetLexem().LineNumber);
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

        public bool IsLogTerm() 
        {
            if (IsLogMultiplier())
            {
                if (GetLexem().Code == 15)
                {
                    MoveToNextLexem();
                    if (IsLogTerm())
                    {
                        return true;
                    }
                    else
                    {
                        throw new ApplicationException("Need a logical term in line " + GetLexem().LineNumber);
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

        public bool IsLogMultiplier() 
        {
            if (IsArithmetic())
            {
                if ((GetLexem().Code >= 9) && (GetLexem().Code <= 14))
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
                    throw new ApplicationException("Need a logical operator in line " + GetLexem().LineNumber);
                }
            }
            else 
            {
                if (GetLexem().Code == 26)
                {
                    MoveToNextLexem();
                    if (IsLogMultiplier())
                    {
                        return true;
                    }
                    else
                    {
                        throw new ApplicationException("Need a logical multiplier in line " + GetLexem().LineNumber);
                    }
                }
                else 
                {
                    if (GetLexem().Code == 28)
                    {
                        MoveToNextLexem();
                        if (IsLogTerm())
                        {
                            if (GetLexem().Code == 29)
                            {
                                MoveToNextLexem();
                                return true;
                            }
                            else
                            {
                                throw new ApplicationException("Need a logical closing bracket in line " + GetLexem().LineNumber);
                            }
                        }
                        else
                        {
                            throw new ApplicationException("Need a logical term in line " + GetLexem().LineNumber);
                        }
                    }
                    else 
                    {
                        return false;
                    }
                }
            }
        }
    }
}