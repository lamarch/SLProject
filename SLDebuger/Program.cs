using System;
using System.Collections.Generic;
using SLProject;
using SLProject.SLCompilerLib.Lexer;
using SLProject.SLCompilerLib.Parser;

namespace SLProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> keywords = new List<string>()
            {
                "debut",
                "chaine",
                "nombre",
                "fonction",
                "si",
                "alors",
                "fin"
            };

            SLCompilerLib.Lexer.Lexer lexer = new SLCompilerLib.Lexer.Lexer(keywords);
            SLCompilerLib.Parser.Parser parser = new SLCompilerLib.Parser.Parser(keywords);
            parser.Parse( lexer.Lex(" debut programme\n" +
                "bonjour\n" +
                "fin\n") );



            Console.Read();

        }
    }
}
