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
        List<string> keywords;

        public Compiler(List<string> keywords, Parser.ReflectionContext reflectionContext){
            this.lexer = new Lexer.Lexer(keywords);
            this.parser = new Parser.Parser(keywords, reflectionContext);
            this.keywords = keywords;
        }

        public Compiler(List<string> keywords, object context) : this(keywords, new Parser.ReflectionContext(context)){}

        public Compiler(List<string> keywords) : this(keywords, new Parser.DefaultContext()){}

        public void Compile(string code, bool debug){
            this.code = code;
            this.debug = debug;
            Debug.on = debug;
            Debug.Write("----------START COMPILATION----------");
            List<Lexer.Token> tokens = lexer.Lex(code);
            var res = parser.Parse(tokens);
            Console.WriteLine("Result : " + res.Eval());
            Debug.Write("----------END COMPILATION----------");
            
        }
    }
}
