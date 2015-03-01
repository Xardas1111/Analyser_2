using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trance_4
{
    public static class RelationshipsTable
    {
        public static List<List<string>> GetTable()
        {
            GeneratorRelationshipsTable table = new GeneratorRelationshipsTable(GetGramarList());
            GramaticToFile(table.GramarList, "Gramatic.txt");
            return table.OutRelationshipsTable;
        }

        public static string GetGramatic()
        {
            string s = "";
            GetGramarList().ForEach(r => s += r + "\n");
            return s;
        }

        public static bool GramaticToFile(List<GrammarLine> grList, string path)
        {
            try
            {
                System.IO.StreamWriter sw = new System.IO.StreamWriter(path);
                grList.ForEach(r => sw.WriteLine(r));
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static List<GrammarLine> GetGramarList()
        {
            List<GrammarLine> gramarList = new List<GrammarLine>();
            gramarList.Add(new GrammarLine("~program","{", "¶", "~operlist2", "}"));
            gramarList.Add(new GrammarLine("~operlist1", "~operlist"));
            gramarList.Add(new GrammarLine("~operlist2", "~operlist"));
            gramarList.Add(new GrammarLine("~operlist", "~oper", "¶"));
            gramarList.Add(new GrammarLine("~operlist", "~oper", "¶", "~operlist1"));
            gramarList.Add(new GrammarLine("~operlist", "~ohol", "¶"));
            gramarList.Add(new GrammarLine("~operlist", "~ohol", "¶", "~operlist1"));
            gramarList.Add(new GrammarLine("~ohol", "~type", "~idlist"));
            gramarList.Add(new GrammarLine("~idlist", "id"));
            gramarList.Add(new GrammarLine("~idlist1", "~idlist"));
            gramarList.Add(new GrammarLine("~idlist", "id",",", "~idlist"));
            gramarList.Add(new GrammarLine("~type", "int"));
            gramarList.Add(new GrammarLine("~type", "real"));
            gramarList.Add(new GrammarLine("~oper", "~prysv"));
            gramarList.Add(new GrammarLine("~oper", "~printt"));
            gramarList.Add(new GrammarLine("~oper", "~scann"));
            gramarList.Add(new GrammarLine("~oper", "~cycle"));
            gramarList.Add(new GrammarLine("~oper", "~if"));
            gramarList.Add(new GrammarLine("~prysv", "id", "=", "~arith_vyr"));
            gramarList.Add(new GrammarLine("~printt", "print", "(", "~arith_vyr1", ")"));
            gramarList.Add(new GrammarLine("~scann", "scan", "(", ")"));
            gramarList.Add(new GrammarLine("~scann", "scan", "(", "~idlist1", ")"));
            gramarList.Add(new GrammarLine("~cycle", "do", "¶", "~operlist2", "while", "~log_vyr"));
            gramarList.Add(new GrammarLine("~operlist2", "~operlist"));
            gramarList.Add(new GrammarLine("~if","if","~log_vyr1", "¶", "{", "¶", "~operlist2", "}"));
            gramarList.Add(new GrammarLine("~log_vyr1", "~log_vyr"));
            gramarList.Add(new GrammarLine("~log_vyr", "~log_dod1", "OR", "~log_vyr"));
            gramarList.Add(new GrammarLine("~log_dod1","~log_dod"));
            gramarList.Add(new GrammarLine("~log_vyr", "~log_dod"));
            gramarList.Add(new GrammarLine("~log_dod", "~log_mnosh1", "AND", "~log_dod"));
            gramarList.Add(new GrammarLine("~log_mnosh1","~log_mnosh"));
            gramarList.Add(new GrammarLine("~log_dod", "~log_mnosh"));
            gramarList.Add(new GrammarLine("~log_mnosh", "~arith_vyr1", "~log_sign", "~arith_vyr"));
            gramarList.Add(new GrammarLine("~log_mnosh", "NOT", "~log_mnosh"));
            gramarList.Add(new GrammarLine("~log_mnosh", "[", "~log_dod1", "]"));
            gramarList.Add(new GrammarLine("~arith_vyr1", "~arith_vyr"));
            gramarList.Add(new GrammarLine("~log_sign", "<"));
            gramarList.Add(new GrammarLine("~log_sign", ">"));
            gramarList.Add(new GrammarLine("~log_sign", "=="));
            gramarList.Add(new GrammarLine("~log_sign", ">="));
            gramarList.Add(new GrammarLine("~log_sign", "<="));
            gramarList.Add(new GrammarLine("~log_sign", "<>"));
            gramarList.Add(new GrammarLine("~arith_vyr", "~dod"));
            gramarList.Add(new GrammarLine("~arith_vyr", "~dod1", "+", "~arith_vyr"));
            gramarList.Add(new GrammarLine("~arith_vyr", "~dod1", "-", "~arith_vyr"));
            gramarList.Add(new GrammarLine("~dod1", "~dod"));
            gramarList.Add(new GrammarLine("~dod", "~mnosh"));
            gramarList.Add(new GrammarLine("~dod", "~mnosh", "*", "~dod"));
            gramarList.Add(new GrammarLine("~dod", "~mnosh", "/", "~dod"));
            gramarList.Add(new GrammarLine("~mnosh", "(", "~arith_vyr1", ")"));
            gramarList.Add(new GrammarLine("~mnosh", "id"));
            gramarList.Add(new GrammarLine("~mnosh", "const"));
            return gramarList;
        }
    }
}