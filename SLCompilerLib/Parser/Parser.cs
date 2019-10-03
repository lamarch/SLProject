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
            Debug.Write("-----START PARSER-----\n");

            index = 0;
            errors = new List<string>();
            this.tokens = tokens;

            Node<double> root = ParseExpression();

            if (GetToken().Type != TokenType.EOF)
                ThrowException(ExceptionType.TokenExpected, "EOF expected !");

            for (int i = 0; i < errors.Count; i++)
            {
                Console.WriteLine("Error : " + errors[i]);
            }

            Debug.Write("\n-----END PARSER-----\n");

            return root;
        }

        Node<double> ParseExpression()
        {
            Debug.StartTree("Parse EXPRESSION");

            var expr = ParseAddSub();
            if(GetToken().Type != TokenType.RTL){
                ThrowException(ExceptionType.TokenExpected, "RTL token expected !");
            }
            Debug.EndTree("Parse EXPRESSION");
            return expr;
        }

        Node<double> ParseAddSub(){
            Debug.StartTree("Parse EXPRESSION");

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
                left = new BinaryNode<double>(left, right, op);
            }
            Debug.EndTree("Parse EXPRESSION");
            return left;
        }

        Node<double> ParseMulDiv()
        {
            Debug.StartTree("Parse MUL-DIV");


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
                left = new BinaryNode<double>(left, right, op);
            }
            
            Debug.EndTree("Parse MUL-DIV");

            return left;
        }

        Node<double> ParseUnary()
        {
            Debug.StartTree("Parse UNARY");

            //unary plus
            if(GetToken().Type == TokenType.Plus)
            {
                Advance();
                var n = ParseUnary();

                Debug.EndTree("Parse UNARY");

                return n;
            }

            //unary minus
            if(GetToken().Type == TokenType.Minus)
            {
                Advance();
                var n = new UnaryOpNode<double>(ParseUnary(), (a) => -a);
                Debug.EndTree("Parse UNARY");

                return n;
            }

            var leaf = ParseLeaf();

            Debug.EndTree("Parse UNARY");

            return leaf;
        }

        Node<double> ParseLeaf()
        {
            Debug.StartTree("Parse LEAF");


            //parentethis
            if(GetToken().Type == TokenType.LPar){
                Debug.WriteTree("Parse Parentethis");

                Advance();
                var node = ParseExpression();
                if(GetToken().Type != TokenType.RPar){
                    ThrowException(ExceptionType.TokenExpected, "Close parentethis expected !");
                }
                Advance();

                Debug.EndTree("Parse LEAF");

                return node;
            }


            //number
            if (GetToken().Type == TokenType.Number)
            {
                Debug.WriteTree("Parse Number");

                ValueNode<double> node = new ValueNode<double>(GetToken().GetDoubleValue());
                Advance();

                Debug.EndTree("Parse LEAF");

                return node;
            }


            //variable
            if (GetToken().Type == TokenType.Identifier){
                Debug.WriteTree("Parse Identifier");

                
                ValueNode<double> node = new ValueNode<double>(reflectionContext.ResolveVariable(GetToken().GetStrValue()));
                Debug.Write($"Variable \"{GetToken().GetStrValue()}\" = {node.Eval()}");
                Advance();

                Debug.EndTree("Parse LEAF");

                return node;
            }

            ThrowException(ExceptionType.TokenExpected, "Number is required after an operation !");
            
            Debug.EndTree("Parse LEAF");

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
