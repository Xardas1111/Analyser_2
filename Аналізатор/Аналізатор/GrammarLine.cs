using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trance_4
{
    public class GrammarLine
    {
        private string leftToken;

        public string LeftToken
        {
            get
            {
                return leftToken;
            }
        }

        private List<string> rightTokens;

        public GrammarLine(string _leftTokens, params string[] rightTokens)
        {
            this.leftToken = _leftTokens;
            this.rightTokens = new List<string>(rightTokens);
        }

        public static bool IsTerminal(string token)
        {
            if (token[0] == '~')
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Tuple<string, string>> GetEqualsPair()
        {
            if (rightTokens.Count < 2)
                return null;
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            for (int i = 0; i < rightTokens.Count - 1; i++)
            {
                list.Add(Tuple.Create<string, string>(rightTokens[i], rightTokens[i + 1]));
            }
            return list;
        }


        public List<string> GetAllTokens()
        {
            List<string> temp = new List<string>();
            temp.Add(leftToken);
            temp.AddRange(rightTokens);
            return temp;
        }

        public string First()
        {
            return rightTokens[0];
        }

        public string Last()
        {
            return rightTokens[rightTokens.Count - 1];
        }


        public override string ToString()
        {
            string s = ((leftToken[0] == '~') ? ("< " + leftToken.Substring(1) + " >") : leftToken) + " ::= ";
            rightTokens.ForEach(r => s += ((r[0] == '~') ? ("< " + r.Substring(1) + " > ") : r + " "));
            return s;
        }

    }
}
