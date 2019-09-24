using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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

        public Parser(List<string> keywords)
        {
            this.keywords = keywords;
        }

        public Node<double> Parse(List<Token> tokens)
        {
            index = 0;
            errors = new List<string>();
            this.tokens = tokens;

            Node<double> root = ParseAddSub();

            if (GetToken().Type != TokenType.EOF)
                throw new SyntaxErrorException("EOF token expected !");

            for (int i = 0; i < errors.Count; i++)
            {
                Console.WriteLine("Error : " + errors[i]);
            }

            return root;
        }

        Node<double> ParseAddSub()
        {
            var left = ParseUnary();
            
            Func<double, double, double> op = null;
            while (true)
            {
                op = null;
                if(GetToken().Type == TokenType.Plus)
                {
                    op = (a, b) => a + b;
                }else if(GetToken().Type == TokenType.Minus)
                {
                    op = (a, b) => a - b;
                }

                if (op == null)
                    return left;

                Advance();
                var right = ParseUnary();
                left = new BinaryNode<double>(left, right, op);
            }
        }

        Node<double> ParseMulDiv()
        {
            var left = ParseUnary();
            Func<double, double, double> op = null;
            
            while(true){
                if(GetToken(1).Type == TokenType.Star){
                    op = (a, b) => a * b;
                    Advance();
                }
                else if(GetToken(1).Type == TokenType.Slash){
                    op = (a, b) => a / b;
                    Advance();
                }else{
                    return left;
                }
                var right = ParseUnary();
                left = new BinaryNode<double>(left, right, op);
            }
        }

        Node<double> ParseUnary()
        {
            if(GetToken().Type == TokenType.Plus)
            {
                Advance();
                return ParseUnary();
            }

            if(GetToken().Type == TokenType.Minus)
            {
                Advance();
                return new UnaryOpNode<double>(ParseUnary(), (a) => -a);
            }

            return ParseNumber();
        }

        Node<double> ParseNumber()
        {
            if (GetToken().Type == TokenType.Number)
            {
                ValueNode<double> node = new ValueNode<double>(GetToken().GetDoubleValue());
                Advance();
                return node;
            }
            throw new SyntaxErrorException("Unexpected token : " + GetToken());
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

        #endregion
    }
}
