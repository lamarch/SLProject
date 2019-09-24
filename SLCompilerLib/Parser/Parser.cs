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
        IReflectionContext reflectionContext;

        public Parser(List<string> keywords, IReflectionContext rctx){
            this.reflectionContext = rctx;
            this.keywords = keywords;
        }

        public Node<double> Parse(List<Token> tokens)
        {
            Debug.Write("-----START PARSER-----");

            index = 0;
            errors = new List<string>();
            this.tokens = tokens;

            Node<double> root = ParseAddSub();

            if (GetToken().Type != TokenType.EOF)
                ThrowException(ExceptionType.TokenExpected, "EOF expected !");

            for (int i = 0; i < errors.Count; i++)
            {
                Console.WriteLine("Error : " + errors[i]);
            }

            Debug.Write("-----END PARSER-----");

            return root;
        }

        Node<double> ParseAddSub()
        {
            Debug.Write("Parse ADD-SUB");
            var left = ParseMulDiv();
            
            Func<double, double, double> op = null;
            while (true)
            {
                op = null;
                //addition
                if(GetToken().Type == TokenType.Plus)
                {
                    op = (a, b) => a + b;
                //substraction
                }else if(GetToken().Type == TokenType.Minus)
                {
                    op = (a, b) => a - b;
                }

                if (op == null)
                    return left;

                Advance();
                var right = ParseMulDiv();
                left = new BinaryNode<double>(left, right, op);
            }
        }

        Node<double> ParseMulDiv()
        {
            Debug.Write("Parse MUL-DIV");

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
                }else{
                    return left;
                }

                Advance();

                var right = ParseUnary();
                left = new BinaryNode<double>(left, right, op);
            }
        }

        Node<double> ParseUnary()
        {
            Debug.Write("Parse UNARY");

            //unary plus
            if(GetToken().Type == TokenType.Plus)
            {
                Advance();
                return ParseUnary();
            }

            //unary minus
            if(GetToken().Type == TokenType.Minus)
            {
                Advance();
                return new UnaryOpNode<double>(ParseUnary(), (a) => -a);
            }

            return ParseNumber();
        }

        Node<double> ParseNumber()
        {
            Debug.Write("Parse NUMBER");
            //parentethis
            if(GetToken().Type == TokenType.LPar){
                Debug.Write("Parse PARENTETHIS");

                Advance();
                var node = ParseAddSub();
                if(GetToken().Type != TokenType.RPar){
                    ThrowException(ExceptionType.TokenExpected, "Close parentethis expected !");
                }
                Advance();
                return node;
            }
            //number
            if (GetToken().Type == TokenType.Number)
            {
                ValueNode<double> node = new ValueNode<double>(GetToken().GetDoubleValue());
                Advance();
                return node;
            }
            //variable
            if (GetToken().Type == TokenType.Identifier){
                Debug.Write("Parse IDENTIFIER");

                ValueNode<double> node = new ValueNode<double>(reflectionContext.ResolveVariable(GetToken().GetStrValue()));
                Debug.Write($"Variable \"{GetToken().GetStrValue()}\" = {node.Eval()}");
                Advance();

                return node;
            }
            ThrowException(ExceptionType.TokenExpected, "Number is required after an operation !");
            return null;
        }


        #region moves

        Token Expect(TokenType type)
        {
            Token t = GetToken();
            if (t.Type != type)
                AddError($"Expected token[\"{type}\"] !");
            Advance();
            return t;
        }

        Token ExpectKeyword(string value)
        {
            if(GetToken().Type != TokenType.Keyword || (string)GetToken().Value != value)
                AddError($"Expected keyword[\"{value}\"] !");
            Token t = GetToken();
            Advance();
            return t;
        }

        void ThrowException(ExceptionType type, string message){
            if(type == ExceptionType.TokenExpected){
                throw new Exception("Token Expected Exception : " + message);
            }else if(type == ExceptionType.TokenUnexcpected){
                throw new Exception("Token Unexpected Exception : " + message);
            }else if(type == ExceptionType.ValueError){
                throw new Exception("Value Error Expected : " + message);
            }else{
                throw new Exception("Unknown Exception : " + message);
            }
        }

        void AddError(string message)
        {
            errors.Add(message);
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

        enum ExceptionType{
            TokenExpected,
            TokenUnexcpected,
            ValueError,
            Unknown
        }

        #endregion
    }
}
