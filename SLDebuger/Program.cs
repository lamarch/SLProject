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
            List<string> keywords = new List<string>();

            SLCompilerLib.Lexer.Lexer lexer = new SLCompilerLib.Lexer.Lexer(keywords);
            SLCompilerLib.Parser.Parser parser = new SLCompilerLib.Parser.Parser(keywords, new SLCompilerLib.Parser.ReflectionContext(new SLCompilerLib.Parser.DefaultContext()));
            string rep;
            while(true){
                Console.WriteLine("Votre calcul :");
                rep = Console.ReadLine();
                try{
                    Console.WriteLine("Calcul en cours...");
                    Console.WriteLine("Resultat : " + parser.Parse( lexer.Lex(rep)).Eval());
                }catch(Exception e){
                    Console.WriteLine("Une erreur est survenue :\n\t"+e.Message);
                }
                Console.WriteLine();
            }
        }
    }
}
