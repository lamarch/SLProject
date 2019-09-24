using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Lexer
{
    public class Token
    {
        public object Value { get; set; }

        public TokenType Type;

        public Token(TokenType type, object value = null)
        {
            this.Type = type;
            this.Value = value;
        }

        public string GetStrValue() => (string)Value;
        public double GetDoubleValue() => (double)Value;
        
        public override string ToString()
        {
            string ret = "TOKEN : " + Type + "; Value = ";

            if (Value == null)
                ret += "null";
            else
                ret += Value;

            return ret;
        }
    }
}
