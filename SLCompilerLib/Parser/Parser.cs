using System;
using System.Collections.Generic;
using SLProject.SLCompilerLib.Lexer;
using SLProject.SLCompilerLib.Parser.Nodes;

namespace SLProject.SLCompilerLib.Parser
{
    public class Parser
    {
        List<Token> tokens;
        List<string> errors;
        List<string> keywords;
        int index;

        public Parser(List<string> keywords){
            this.keywords = keywords;
        }

        public NumberNode Parse(List<Token> tokens)
        {
            Debug.Write("-----START PARSER-----\n");

            index = 0;
            errors = new List<string>();
            this.tokens = tokens;

            ParseImport();
            NumberNode root = ParseExpression();

            if (GetToken().Type != TokenType.EOF)
                ThrowException(ExceptionType.TokenExpected, "EOF expected !", "PARSER_parse_32");

            for (int i = 0; i < errors.Count; i++)
            {
                Console.WriteLine("Error : " + errors[i]);
            }

            Debug.Write("\n-----END PARSER-----\n");

            return root;
        }

        List<ImportNode> ParseImport(){
            Debug.StartBranch("Parse IMPORTS");
            List<ImportNode> imports = new List<ImportNode>();
            Accessor accessor = null;
            while(IsKeyword("importer")){
                Advance();
                if (!IsIdent())
                {
                    ThrowException(ExceptionType.TokenExpected, "Identifier expected after \"importer\" keyword !", "PARSER_parseimport_49");
                }
                accessor = GetAccessor();
                Debug.WriteBranch("IMPORT found (\""+ accessor +"\") !");

                imports.Add(new ImportNode(ImportNode.ImportType.Undefined, accessor));
            }
            Debug.EndBranch();
            return imports;
        }

        void ParseBody(){

        }

        void ParseModule(){

        }

        void ParseFunction(){

        }

        void ParseMain(){

        }

        NumberNode ParseExpression()
        {
            Debug.StartBranch("Parse EXPRESSION");

            var expr = ParseAddSub();
            Debug.EndBranch();
            return expr;
        }

        NumberNode ParseAddSub(){
            Debug.StartBranch("Parse EXPRESSION");

            var left = ParseMulDiv();
            
            Func<double, double, double> op = null;
            while (true)
            {
                op = null;
                //addition
                if(GetToken().Type == TokenType.Plus)
                {
                    op = (a, b) => a + b;
                }
                //substraction
                else if(GetToken().Type == TokenType.Minus)
                {
                    op = (a, b) => a - b;
                }
                //RTL
                else if(GetToken().Type == TokenType.RTL){
                    Advance();
                    continue;
                }
                //Error
                else{
                    break;
                }

                Advance();
                var right = ParseMulDiv();
                left = new ExpressionNode(op, left, right);
            }
            Debug.EndBranch();
            return left;
        }

        NumberNode ParseMulDiv()
        {
            Debug.StartBranch("Parse MUL-DIV");


            var left = ParseUnary();
            Func<double, double, double> op = null;

            while(true){
                //multiplication
                if(GetToken().Type == TokenType.Star){
                    op = (a, b) => a * b;
                }
                //division
                else if(GetToken().Type == TokenType.Slash){
                    op = (a, b) => a / b;
                }
                //RTL
                else if(GetToken().Type == TokenType.RTL){
                    Advance();
                    continue;
                }
                //Error
                else{
                    break;
                }

                Advance();

                var right = ParseUnary();
                left = new ExpressionNode(op, left, right);
            }
            
            Debug.EndBranch();

            return left;
        }

        NumberNode ParseUnary()
        {
            Debug.StartBranch("Parse UNARY");

            //unary plus
            if(GetToken().Type == TokenType.Plus)
            {
                Advance();
                var n = ParseUnary();

                Debug.EndBranch();

                return n;
            }

            //unary minus
            if(GetToken().Type == TokenType.Minus)
            {
                Advance();
                var n = new ExpressionNode(a => -a, ParseUnary());
                Debug.EndBranch();

                return n;
            }

            var leaf = ParseLeaf();

            Debug.EndBranch();

            return leaf;
        }

        NumberNode ParseLeaf()
        {
            Debug.StartBranch("Parse LEAF");


            //parentethis
            if(GetToken().Type == TokenType.LPar){
                Debug.WriteBranch("Parse Parentethis");

                Advance();
                var node = ParseExpression();
                if(GetToken().Type != TokenType.RPar){
                    ThrowException(ExceptionType.TokenExpected, "Close parentethis expected !", "PARSER_parseleaf_172");
                }
                Advance();

                Debug.EndBranch();

                return node;
            }


            //number
            if (GetToken().Type == TokenType.Number)
            {
                Debug.WriteBranch("Parse Number");

                NumberNode node = new NumberNode(GetToken().GetDoubleValue());
                Advance();

                Debug.EndBranch();

                return node;
            }

            ThrowException(ExceptionType.TokenExpected, "Number is required !", "PARSER_parseleaf_210");
            
            Debug.EndBranch();

            return null;
        }


        #region moves

        public static void ThrowException(ExceptionType type, string message, string code){
            if(type == ExceptionType.TokenExpected){
                throw new Exception("Token Expected Exception ["+code+"] : " + message);
            }else if(type == ExceptionType.TokenUnexcpected){
                throw new Exception("Token Unexpected Exception ["+code+"] : " + message);
            }else if(type == ExceptionType.ValueError){
                throw new Exception("Value Error Exception ["+code+"] : " + message);
            }else if(type == ExceptionType.UnknownIdentifier){
                throw new Exception("Unknown Identifier Error Exception ["+code+"] : " + message);
            }else{
                throw new Exception("Unknown Exception ["+code+"] : " + message);
            }
        }

        bool IsKeyword(string name){
            return GetToken().Type == TokenType.Keyword && GetToken().GetStrValue() == name;
        }

        bool IsIdent(){
            return GetToken().Type == TokenType.Identifier;
        }

        Accessor GetAccessor()
        {
            List<string> idents = new List<string>();
            bool point = false;

            while(GetToken().Type == TokenType.Identifier)
            {
                point = false;
                idents.Add(GetToken().GetStrValue());
                Advance();

                if(GetToken().Type == TokenType.Point)
                {
                    point = true;
                    Advance();
                }
            }
            if (point)
                ThrowException(ExceptionType.TokenUnexcpected, "Accessors cannot finish by a point !", "PARSER_getaccessor_287");
            return new Accessor(idents);
        }

        Token Advance(int step = 1)
        {
            index += step;
            return GetToken();
        }

        Token GetToken(int offset = 0)
        {
            if (index + offset >= tokens.Count)
                return new Token(TokenType.EOF);
            else
                return tokens[index + offset];
        }

        public enum ExceptionType{
            Unknown,
            TokenExpected,
            TokenUnexcpected,
            ValueError,
            UnknownIdentifier
        }

        #endregion
    }
}
