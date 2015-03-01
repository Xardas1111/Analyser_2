using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trance_4
{
    public class GeneratorRelationshipsTable
    {
        private List<GrammarLine> gramarList = new List<GrammarLine>();

        public List<GrammarLine> GramarList
        {
            get { return gramarList; }
        }

        private List<List<string>> outRelationshipsTable = new List<List<string>>();

        public List<List<string>> OutRelationshipsTable
        {
            get { return outRelationshipsTable; }
        }

        bool isError = true;

        public bool IsError
        {
            get { return isError; }
        }

        private List<Tuple<string, string>> AllTuplePair = new List<Tuple<string, string>>();

        private List<string> allTokens = new List<string>();

        public GeneratorRelationshipsTable(List<GrammarLine> _gramarList)
        {
            this.gramarList = new List<GrammarLine>(_gramarList);
            if (Generate())
                isError = false;
            else
                isError = true;
        }

        public bool Generate()
        {
            AllTokens();
            IniMatrix();
            GetAllTuplePair();
            LeftTopMatrix();
            if (!FindEquals())
                return false;
            if (!GreaterLower())
                return false;
            return true;
            
        }


        private void IniMatrix()
        {
            for (int i = 0; i <= allTokens.Count; i++)
            {
                List<string> temp = new List<string>();
                for (int j = 0; j <= allTokens.Count; j++)
                {
                    temp.Add("");
                }
                outRelationshipsTable.Add(temp);
            }
        }

        private void AllTokens()
        {
            List<string> tokensList = new List<string>();
            foreach (GrammarLine g in gramarList)
            {
                tokensList.AddRange(g.GetAllTokens());
            }
            allTokens = tokensList.Distinct().ToList();
        }

        public void LeftTopMatrix()
        {
            outRelationshipsTable[0][0] = "1\\2";
            int i = 1;
            foreach (string s in allTokens)
            {
                outRelationshipsTable[i][0] = s;
                outRelationshipsTable[0][i] = s;
                i++;
            }
        }

        private void GetAllTuplePair()
        {
            foreach (GrammarLine g in gramarList)
            {
                var temp = g.GetEqualsPair();
                if (temp != null)
                    AllTuplePair.AddRange(temp);
            }
        }

        private bool FindEquals()
        {
            for (int i = 1; i < outRelationshipsTable.Count; i++)
            {
                for (int j = 1; j < outRelationshipsTable[i].Count; j++)
                {
                    if (AllTuplePair.Contains(Tuple.Create<string, string>(outRelationshipsTable[i][0], outRelationshipsTable[0][j])))
                    {
                        if (outRelationshipsTable[i][j] == "")
                            outRelationshipsTable[i][j] = ".=";
                        else
                            return false;
                    }
                }
            }
            return true;
        }

        private bool GreaterLower()
        {
            for (int i = 1; i < outRelationshipsTable.Count; i++)
            {
                for (int j = 1; j < outRelationshipsTable[i].Count; j++)
                {
                    if (outRelationshipsTable[i][j] == ".=")
                    {
                        if (!GrammarLine.IsTerminal(outRelationshipsTable[i][0]) || !GrammarLine.IsTerminal(outRelationshipsTable[0][j]))
                        {
                            List<string> lastPlus = new List<string>(GetLastPlus(outRelationshipsTable[i][0], null));
                            List<string> firstPlus = new List<string>(GetFirstPlus(outRelationshipsTable[0][j], null));
                            foreach (var a in lastPlus)
                            {
                                foreach (var f in firstPlus)
                                {
                                    int k = outRelationshipsTable.FindIndex(r => r[0] == a);
                                    int kf = outRelationshipsTable.FindIndex(r => r[0] == f);
                                    if (outRelationshipsTable[k][kf] == "" || outRelationshipsTable[k][kf] == ".>")
                                        outRelationshipsTable[k][kf] = ".>";

                                }
                            }
                        }
                        if (!GrammarLine.IsTerminal(outRelationshipsTable[i][0]))
                        {
                            List<string> lastPlus = new List<string>(GetLastPlus(outRelationshipsTable[i][0], null));
                            foreach (var a in lastPlus)
                            {
                                int k = outRelationshipsTable.FindIndex(r => r[0] == a);
                                if (outRelationshipsTable[k][j] == "" || outRelationshipsTable[k][j] == ".>")
                                    outRelationshipsTable[k][j] = ".>";
                                else
                                    outRelationshipsTable[k][j] += " Err " + ".>";
                            }
                        }
                        if (!GrammarLine.IsTerminal(outRelationshipsTable[0][j]))
                        {
                            List<string> firstPlus = new List<string>(GetFirstPlus(outRelationshipsTable[0][j], null));
                            foreach (var a in firstPlus)
                            {
                                int k = outRelationshipsTable[0].FindIndex(r => r == a);
                                if (outRelationshipsTable[i][k] == "" || outRelationshipsTable[i][k] == "<.")
                                    outRelationshipsTable[i][k] = "<.";
                                else
                                { outRelationshipsTable[i][k] += " Err " + "<."; }

                                
                            }
                        }

                    }
                }
            }
            return true;
        }

        private IEnumerable<string> GetLastPlus(string p, List<string> list)
        {
            if (list == null)
                list = new List<string>();
            var grList = gramarList.FindAll(r => r.LeftToken == p);
            foreach (var gramarLine in grList)
            {
                string tempLast = gramarLine.Last();
                if (list.Contains(tempLast))
                    continue;
                list.Add(tempLast);
                if (!GrammarLine.IsTerminal(tempLast))
                {
                    GetLastPlus(tempLast, list);
                }
            }
            return list;
        }

        private List<string> GetFirstPlus(string p, List<string> list)
        {
            if (list == null)
                list = new List<string>();
            var grList = gramarList.FindAll(r => r.LeftToken == p);
            foreach (var gramarLine in grList)
            {
                string tempFirst = gramarLine.First();
                if (list.Contains(tempFirst))
                    continue;
                list.Add(tempFirst);
                if (!GrammarLine.IsTerminal(tempFirst))
                {
                    GetFirstPlus(tempFirst, list);
                }
            }
            return list;
        }
    }
}
