using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Аналізатор
{
    public class Lexem
    {
        public int Number;
        public int LineNumber;
        public string LexName;
        public int Code;
        public int IdCode;
        public Lexem() 
        { 
            
        }
        public Lexem(int Number, int LineNumber, string LexName, int Code, int IdCode)
        {
            this.Number = Number;
            this.LineNumber = LineNumber;
            this.LexName = LexName;
            this.Code = Code;
            this.IdCode = IdCode;
        }
    }

    struct Id
    {
        public string IdName;
        public int IdCode;
        public string IdType;
        public Id(string IdName, int IdCode, string IdType)
        {
            this.IdName = IdName;
            this.IdCode = IdCode;
            this.IdType = IdType;
        }
    }

    struct Const
    {
        public string ConstName;
        public int ConstCode;
        public Const(string ConstName, int ConstCode)
        {
            this.ConstName = ConstName;
            this.ConstCode = ConstCode;
        }
    }
    partial class Analyser
    {
        static int LexemNumber = 0;
        static int NumberOfLines = 0;
        static int ConstCodeNumber = 0;
        static int IdCodeNumber = 0;
        static string[] lexem = { "int", "real", "print", "scan", "while", "do", "if", "=", "<>", "<=", ">=", "<"
        , ">", "==", "AND", "OR", ",", "+", "-", "*", "/", "{", "}", "(", ")", "NOT", "¶", "[", "]" };
        static string[] terminal = { "int", "real", "print", "scan", "while", "do", "if", "moveto", "AND", "OR", "NOT" };
        static string[] separator = { "=", "<>", "<=", ">=", "<", ">", "==", ","
                                        , "+", "-", "*", "/", "{", "}", "(", ")", " ", "\t", "[", "]" };

        static List<Lexem> LexemTable = new List<Lexem>();
        static List<Id> IdTable = new List<Id>();
        static List<Const> ConstTable = new List<Const>();
    }
}