using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

        public ProgramNode Parse(List<Token> tokens)
        {
            Debug.Write("-----START PARSER-----\n");

            index = 0;
            errors = new List<string>();
            this.tokens = tokens;

            ParseImport();
            ParseBodies();

            if (GetToken().Type != TokenType.EOF)
                ThrowException(ExceptionType.TokenExpected, "EOF expected !", "PARSER:sys");

            for (int i = 0; i < errors.Count; i++)
            {
                Console.WriteLine("Error : " + errors[i]);
            }

            Debug.Write("\n-----END PARSER-----\n");

            return new ProgramNode();
        }

        List<ImportNode> ParseImport(){
            Debug.StartBranch("Parse IMPORTS");
            List<ImportNode> imports = new List<ImportNode>();
            Accessor accessor = null;

            while(IsKeyword("importer")){
                Advance();
                if (!IsIdent())
                {
                    ThrowException(ExceptionType.TokenExpected, "Identifier expected after \"importer\" keyword !", "PARSER:user");
                }
                accessor = GetAccessor();
                Debug.WriteBranch("IMPORT found (\""+ accessor +"\") !");

                imports.Add(new ImportNode(ImportNode.ImportType.Undefined, accessor));
            }
            Debug.EndBranch();
            return imports;
        }

        /// <summary>
        /// Parse all except imports
        /// </summary>
        /// <returns></returns>
        BodiesNode ParseBodies(){
            Debug.StartBranch("Parse BODIES");
            BodiesNode bodies = new BodiesNode();
            FunctionNode main = new FunctionNode();
            while(GetToken().Type != TokenType.EOF)
            {
                if(IsKeyword("fonction"))
                {
                    Debug.WriteBranch("Function found !");

                    bodies.FunctionNodes.Add(ParseFunction());
                }else if(GetToken().Type == TokenType.RTL)
                {
                    Advance();
                }
                else
                {
                    Debug.WriteBranch("Statement found !");
                    main.AddStmt(ParseStmt());
                }
            }
            Debug.EndBranch();
            return new BodiesNode();
        }

        /// <summary>
        /// Take all stmts and put them in a function
        /// </summary>
        /// <returns></returns>
        FunctionNode ParseFunction()
        {
            Debug.StartBranch("Parse FUNCTION");
            if (!IsKeyword("fonction"))
            {
                ThrowException(ExceptionType.TokenExpected, "\"fonction\" keywords expected !", "PARSER:sys");
            }
            Debug.EndBranch();
            return null;
        }

        /// <summary>
        /// Take all lines(one or more) and make a stmt(statement)
        /// </summary>
        /// <returns></returns>
        StatementsNode ParseStmt()
        {
            Debug.StartBranch("Parse STATEMENT");

            StatementsNode node = null;
            List<Token> stmt_toks = new List<Token>();
            if(GetToken().Type == TokenType.Keyword)
            {
                if (IsKeyword("fonction"))
                {
                    ThrowException(ExceptionType.TokenUnexcpected, "ParseStmt cannot parse function !", "PARSER:sys");
                }
            }
            else
            {
                ThrowException(ExceptionType.TokenUnexcpected, "Statement cannot start with an other token that \"keyword\" : " + GetToken().Type, "PARSER:user");
            }
            Debug.EndBranch();

            return node;
        }

        ValueNode ParseExpression()
        {
            Debug.StartBranch("Parse EXPRESSION");

            if(GetToken().Type == TokenType.String)
            {
                Debug.WriteBranch("STRING value found !");
                Debug.EndBranch();         
                return new ValueNode(ValueNode.ValueType.@string, new StringNode(GetToken().GetStrValue()));
            }
            else
            {
                NumberNode expr = ParseAddSub(out bool isNum);
                if (isNum)
                {
                    Debug.WriteBranch("NUMBER value found !");
                    Debug.EndBranch();
                    return new ValueNode(ValueNode.ValueType.number, expr);
                }
                else
                {
                    Debug.WriteBranch("UNKNOWN value found !");
                    ThrowException(ExceptionType.ValueError, "Unknowkn value found !", "PARSER:undef");
                }
            }
            return null;
        }

        NumberNode ParseAddSub(out bool isNum){
            Debug.StartBranch("Parse ADD-SUB");

            var left = ParseMulDiv(out isNum);
            
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
                var right = ParseMulDiv(out _);
                left = new ExpressionNode(op, left, right);
            }
            Debug.EndBranch();
            return left;
        }

        NumberNode ParseMulDiv(out bool isNum)
        {
            Debug.StartBranch("Parse MUL-DIV");


            var left = ParseUnary(out isNum);
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

                var right = ParseUnary(out _);
                left = new ExpressionNode(op, left, right);
            }
            
            Debug.EndBranch();

            return left;
        }

        NumberNode ParseUnary(out bool isNum)
        {
            Debug.StartBranch("Parse UNARY");

            //unary plus
            if(GetToken().Type == TokenType.Plus)
            {
                Advance();
                var n = ParseUnary(out isNum);

                Debug.EndBranch();

                return n;
            }

            //unary minus
            if(GetToken().Type == TokenType.Minus)
            {
                Advance();
                var n = new ExpressionNode(a => -a, ParseUnary(out isNum));
                Debug.EndBranch();

                return n;
            }

            var leaf = ParseLeaf(out isNum);

            Debug.EndBranch();

            return leaf;
        }

        NumberNode ParseLeaf(out bool isNum)
        {
            isNum = true;

            Debug.StartBranch("Parse LEAF");


            //parentethis
            if(GetToken().Type == TokenType.LPar){
                Debug.WriteBranch("Parse Parentethis");

                Advance();
                var node = ParseAddSub(out isNum);
                if(GetToken().Type != TokenType.RPar){
                    ThrowException(ExceptionType.TokenExpected, "Close parentethis expected !", "PARSER:user");
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

            isNum = false;
            
            Debug.EndBranch();

            return null;
        }


        #region moves

        public static void ThrowException(ExceptionType type, string message, string code, [CallerLineNumber]int line = -1, [CallerMemberName]string caller = "NO_CALLER_CAUGHT", [CallerFilePath]string file = "NO_FILE"){
            string exception_msg = "ERROR-" + code + " @ file:\"" + file + "\" caller:" + caller + " line:" + line + "\n";
            if(type == ExceptionType.TokenExpected){
                exception_msg += "Token Expected Exception : " + message;
            }else if(type == ExceptionType.TokenUnexcpected){
                exception_msg += "Token Unexpected Exception : " + message;
            }else if(type == ExceptionType.ValueError){
                exception_msg += "Value Error Exception : " + message;
            }else if(type == ExceptionType.UnknownIdentifier){
                exception_msg += "Unknown Identifier Error Exception : " + message;
            }else{
                exception_msg += "Unknown Exception : " + message;
            }
            throw new Exception(exception_msg);
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
                ThrowException(ExceptionType.TokenUnexcpected, "Accessors cannot finish by a point !", "PARSER:user");
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
