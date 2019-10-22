using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SLProject.SLCompilerLib.Lexer;
using SLProject.SLCompilerLib.Parser.Nodes;

namespace SLProject.SLCompilerLib.Parser
{
    public class Parser
    {
        private List<Token> tokens;
        private int index;

        public Parser ()
        {
        }

        public ProgramNode Parse ( List<Token> tokens )
        {

            this.index = 0;
            this.tokens = tokens;
            List<Label> labels;
            List<InstructionNode> instructions;

            instructions = this.ParseInstructions( out labels );

            if ( this.GetToken().Type != TokenType.EOF ) {
                ThrowException( ExceptionType.TokenExpected, "EOF expected !", "PARSER:sys" );
            }

            return new ProgramNode( labels, instructions );
        }

        private List<InstructionNode> ParseInstructions ( out List<Label> labels )
        {
            uint instr_nb = 0;
            List<InstructionNode> nodes = new List<InstructionNode>();
            labels = new List<Label>();
            string label_name = "";
            string function = "";
            List<ValueNode> args = new List<ValueNode>();
            InstructionNode instruction;

            Debug.StartBranch( "Parse INSTRUCTIONS" );

            while ( this.GetToken( 0 ).Type != TokenType.EOF ) {

                if ( this.GetToken().Type == TokenType.RTL ) {
                    this.Advance();
                    continue;
                }

                args.Clear();
                label_name = "";
                function = "";

                Debug.StartBranch( "Parse INSTRUCTION " + ++instr_nb );


                //Parse label
                if ( this.GetToken( 1 ).Type == TokenType.Colon ) {

                    if ( this.GetToken( 0 ).Type == TokenType.Identifier ) {
                        label_name = this.GetToken( 0 ).Value as string;
                        this.Advance( 2 );
                        Debug.WriteBranch( "Label found : " + label_name );


                    } else {
                        ThrowException( ExceptionType.ValueError, "Label identifier expect !", "PARSER:user" );
                    }

                }

                //Get function
                if ( this.GetToken().Type != TokenType.Identifier ) {
                    ThrowException( ExceptionType.TokenExpected, $"Function identifier expected (actual token : {this.GetToken()} )  !", "PARSER:user" );
                }

                function = (string) this.GetToken( 0 ).Value;
                Debug.WriteBranch( "function found : " + function );
                this.Advance();

                Debug.StartBranch( "Parse ARGUMENTS" );

                //Get args if there are
                while ( this.GetToken( 0 ).Type != TokenType.RTL ) {


                    if ( this.GetToken().Type == TokenType.Number ) {
                        //number
                        args.Add( new ValueNode( ValueNode.ValueType.number, this.GetToken().Value ) );
                        Debug.WriteBranch( "Number found : " + (double) this.GetToken().Value );

                        this.Advance();





                    } else if ( this.GetToken().Type == TokenType.String ) {
                        //string
                        args.Add( new ValueNode( ValueNode.ValueType.@string, this.GetToken().Value ) );
                        Debug.WriteBranch( "String found : " + (string) this.GetToken().Value );

                        this.Advance();




                    } else if ( this.GetToken( 0 ).Type == TokenType.Ampersand ) {

                        //get variable adress

                        if ( this.GetToken( 1 ).Type == TokenType.Identifier ) {
                            args.Add( new ValueNode( ValueNode.ValueType.var,
                                                new VariableNode( (string) this.GetToken().Value, VariableNode.VariableAccessor.Adress ) ) );

                            Debug.WriteBranch( "Variable (adress) found : " + (string) this.GetToken( 1 ).Value );

                            this.Advance( 2 );

                        } else {
                            ThrowException( ExceptionType.TokenUnexcpected, "Ampersand token cannot be alone !", "PARSER:user_syntax" );
                        }




                    } else if ( this.GetToken( 0 ).Type == TokenType.Star ) {

                        //get value pointed by this variable

                        if ( this.GetToken( 1 ).Type == TokenType.Identifier ) {
                            args.Add( new ValueNode( ValueNode.ValueType.var,
                                                new VariableNode( (string) this.GetToken().Value, VariableNode.VariableAccessor.Pointer ) ) );

                            Debug.WriteBranch( "Variable (pointer) found : " + (string) this.GetToken( 1 ).Value );

                            this.Advance( 2 );

                        } else {
                            ThrowException( ExceptionType.TokenUnexcpected, "Star token cannot be alone !", "PARSER:user_syntax" );
                        }





                    } else if ( this.GetToken( 0 ).Type == TokenType.Dollar ) {

                        //get variable value

                        if ( this.GetToken( 1 ).Type == TokenType.Identifier ) {
                            args.Add( new ValueNode( ValueNode.ValueType.var,
                                                new VariableNode( (string) this.GetToken().Value, VariableNode.VariableAccessor.Value ) ) );

                            Debug.WriteBranch( "Variable (value) found : " + (string) this.GetToken( 1 ).Value );

                            this.Advance( 2 );

                        } else {
                            ThrowException( ExceptionType.TokenUnexcpected, "Dollar token cannot be alone !", "PARSER:user_syntax" );
                        }





                    } else {
                        //Unknown or unallowed token found for argument
                        ThrowException( ExceptionType.TokenUnexcpected, $"Unexpected token ( actual token : {this.GetToken()} )", "PARSER:user_syntax" );
                    }


                }
                Debug.EndBranch(); //arguments

                instruction = new InstructionNode( function, args );



                //Create the label
                if ( label_name != "" ) {
                    labels.Add( new Label( label_name, instruction ) );
                }


                nodes.Add( instruction );

                Debug.EndBranch(); //instruction
            }

            Debug.EndBranch(); //instructions
            return nodes;
        }

        private ValueNode ParseExpression ()
        {
            Debug.StartBranch( "Parse EXPRESSION" );

            if ( this.GetToken().Type == TokenType.String ) {
                Debug.WriteBranch( "STRING value found !" );
                Debug.EndBranch();
                return new ValueNode( ValueNode.ValueType.@string, new StringNode( this.GetToken().GetStrValue() ) );
            } else {
                Debug.WriteBranch( "NUMBER value found !" );
                NumberNode numberNode = this.ParseAddSub();
                Debug.EndBranch();
                return new ValueNode( ValueNode.ValueType.number, numberNode );
            }
        }

        private NumberNode ParseAddSub ()
        {
            Debug.StartBranch( "Parse ADD-SUB" );

            var left = this.ParseMulDiv();

            Func<double, double, double> op = null;
            while ( true ) {
                op = null;
                //addition
                if ( this.GetToken().Type == TokenType.Plus ) {
                    op = ( a, b ) => a + b;
                }
                //substraction
                else if ( this.GetToken().Type == TokenType.Minus ) {
                    op = ( a, b ) => a - b;
                }
                //RTL
                else if ( this.GetToken().Type == TokenType.RTL ) {
                    this.Advance();
                    continue;
                }
                //Error
                else {
                    break;
                }

                this.Advance();
                var right = this.ParseMulDiv();
                left = new ExpressionNode( op, left, right );
            }
            Debug.EndBranch();
            return left;
        }

        private NumberNode ParseMulDiv ()
        {
            Debug.StartBranch( "Parse MUL-DIV" );


            var left = this.ParseUnary();
            Func<double, double, double> op = null;

            while ( true ) {
                //multiplication
                if ( this.GetToken().Type == TokenType.Star ) {
                    op = ( a, b ) => a * b;
                }
                //division
                else if ( this.GetToken().Type == TokenType.Slash ) {
                    op = ( a, b ) => a / b;
                }
                //RTL
                else if ( this.GetToken().Type == TokenType.RTL ) {
                    this.Advance();
                    continue;
                }
                //Error
                else {
                    break;
                }

                this.Advance();

                var right = this.ParseUnary();
                left = new ExpressionNode( op, left, right );
            }

            Debug.EndBranch();

            return left;
        }

        private NumberNode ParseUnary ()
        {
            Debug.StartBranch( "Parse UNARY" );

            //unary plus
            if ( this.GetToken().Type == TokenType.Plus ) {
                this.Advance();
                var n = this.ParseUnary();

                Debug.EndBranch();

                return n;
            }

            //unary minus
            if ( this.GetToken().Type == TokenType.Minus ) {
                this.Advance();
                var n = new ExpressionNode(a => -a, this.ParseUnary());
                Debug.EndBranch();

                return n;
            }

            var leaf = this.ParseLeaf();

            Debug.EndBranch();

            return leaf;
        }

        private NumberNode ParseLeaf ()
        {

            Debug.StartBranch( "Parse LEAF" );


            //parentethis
            if ( this.GetToken().Type == TokenType.LPar ) {
                Debug.WriteBranch( "Parse Parentethis" );

                this.Advance();
                var node = this.ParseAddSub();
                if ( this.GetToken().Type != TokenType.RPar ) {
                    ThrowException( ExceptionType.TokenExpected, "Close parentethis expected !", "PARSER:user" );
                }
                this.Advance();

                Debug.EndBranch();

                return node;
            }


            //number
            if ( this.GetToken().Type == TokenType.Number ) {
                Debug.WriteBranch( "Parse Number" );

                NumberNode node = new NumberNode(this.GetToken().GetDoubleValue());
                this.Advance();

                Debug.EndBranch();

                return node;
            }

            Debug.EndBranch();

            return null;
        }


        #region moves

        public static void ThrowException ( ExceptionType type, string message, string code, [CallerLineNumber]int line = -1, [CallerMemberName]string caller = "NO_CALLER_CAUGHT", [CallerFilePath]string file = "NO_FILE" )
        {
            string exception_msg = "ERROR-" + code + " @ file:\"" + file + "\" caller:" + caller + " line:" + line + "\n";
            if ( type == ExceptionType.TokenExpected ) {
                exception_msg += "Token Expected Exception : " + message;
            } else if ( type == ExceptionType.TokenUnexcpected ) {
                exception_msg += "Token Unexpected Exception : " + message;
            } else if ( type == ExceptionType.ValueError ) {
                exception_msg += "Value Error Exception : " + message;
            } else if ( type == ExceptionType.UnknownIdentifier ) {
                exception_msg += "Unknown Identifier Error Exception : " + message;
            } else {
                exception_msg += "Unknown Exception : " + message;
            }
            throw new Exception( exception_msg );
        }

        //private bool IsKeyword ( string name ) => this.GetToken().Type == TokenType.Keyword && this.GetToken().GetStrValue() == name;

        //private bool IsIdent () => this.GetToken().Type == TokenType.Identifier;

        private Token Advance ( int step = 1 )
        {
            this.index += step;
            return this.GetToken();
        }

        private Token GetToken ( int offset = 0 )
        {
            if ( this.index + offset >= this.tokens.Count ) {
                return new Token( TokenType.EOF );
            } else {
                return this.tokens[this.index + offset];
            }
        }

        public enum ExceptionType
        {
            Unknown,
            TokenExpected,
            TokenUnexcpected,
            ValueError,
            UnknownIdentifier
        }

        #endregion
    }
}
