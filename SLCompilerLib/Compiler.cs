using System;
using System.Collections.Generic;

namespace SLProject.SLCompilerLib
{
    public class Compiler
    {
        private bool debug;
        private string code;
        private Parser.Parser parser;
        private Lexer.Lexer lexer;


        public Compiler ()
        {
            this.lexer = new Lexer.Lexer();
            this.parser = new Parser.Parser();
        }

        public void Compile ( string code, bool debug )
        {
            this.code = code;
            this.debug = debug;
            Debug.on = debug;
            Debug.Write( "----------START COMPILATION----------\n" );

            Debug.Write( "-----START LEXER-----\n" );

            List<Lexer.Token> tokens = this.lexer.Lex(code);

            Debug.Write( "\n-----END LEXER-----\n" );

            Debug.Write( "-----START PARSER-----\n" );

            var res = this.parser.Parse(tokens);

            Debug.Write( "\n-----END PARSER-----\n" );

            Console.WriteLine( "Result : " + res );

            Debug.Write( "\n----------END COMPILATION----------\n" );

        }
    }
}
