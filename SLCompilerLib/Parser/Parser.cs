using System;
using System.Collections.Generic;
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

        public RootNode Parse(List<Token> tokens)
        {
            index = 0;
            errors = new List<string>();
            this.tokens = tokens;

            ExpectKeyword("debut");
            Expect(TokenType.Identifier);

            StatementSequence();

            ExpectKeyword("fin");

            for (int i = 0; i < errors.Count; i++)
            {
                Console.WriteLine("Error : " + errors[i]);
            }

            return new RootNode("test");
        }  
        
        void StatementSequence()
        {
            if (GetToken().Type == TokenType.Keyword && GetToken().GetStrValue() == "fin")
                return;

            while(GetToken().Type != TokenType.EOF)
            {
                Statement();
                if (GetToken().Type == TokenType.Keyword && GetToken().GetStrValue() == "fin")
                    return;
            }
        }

        void Statement()
        {
            switch (GetToken().Type)
            {
                case TokenType.Keyword:
                    switch (GetToken().GetStrValue())
                    {
                        case "chaine":
                            BuildStringVariable();
                            break;
                        case "si":
                            BuildIfStmt();
                            break;
                        case "fonction":
                            BuildFunction();
                            break;
                    }
                    break;
                case TokenType.Identifier:
                    ParseIdentifierStmt();
                    break;
                default:
                    Advance();
                    break;
            }
        }

        void BuildNumberVariable()
        {

        }

        AssignNode<string> BuildStringVariable()
        {
            string name = Expect(TokenType.Identifier).GetStrValue();
            if (name == null)
                AddError("Assignation : ValueError, cann");
            StringNode value = new StringNode("");
            if(GetToken().Type == TokenType.Equal)
            {
                value = new StringNode(Expect(TokenType.String).GetStrValue());
            }
            return new AssignNode<string>(name, value);
        }

        void BuildFunction()
        {

        }

        void BuildIfStmt()
        {

        }

        void BuildForStmt()
        {

        }

        void BuildWhileStmt()
        {

        }

        void ParseIdentifierStmt()
        {
            while(GetToken().Type != TokenType.RTL) { Advance(); }
        }

        ValueNode<T> ParseExpression<T>()
        {
            
            return null;
        }

        Token Expect(TokenType type)
        {
            if (GetToken().Type != type)
                AddError($"Expected token[\"{type}\"] !");
            Token t = GetToken();
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
    }
}
