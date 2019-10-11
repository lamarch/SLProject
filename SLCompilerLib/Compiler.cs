using System;
using System.Collections.Generic;

namespace SLProject.SLCompilerLib
{
    public class Compiler
    {
        bool debug;
        string code;

        Parser.Parser parser;
        Lexer.Lexer lexer;


        public Compiler(){
            this.lexer = new Lexer.Lexer(def_keywords);
            this.parser = new Parser.Parser(def_keywords);
        }

        public void Compile(string code, bool debug){
            this.code = code;
            this.debug = debug;
            Debug.on = debug;
            Debug.Init();
            Debug.Write("----------START COMPILATION----------\n");
            List<Lexer.Token> tokens = lexer.Lex(code);
            var res = parser.Parse(tokens);
            Console.WriteLine("Result : " + res.ToString());
            Debug.Write("\n----------END COMPILATION----------\n");
            
        }

        public static readonly List<string> def_keywords = new List<string>()
        {
                "function",
                "module",
                "programme",
                "si",
                "fin",
                "importer"
        };
    }
}
