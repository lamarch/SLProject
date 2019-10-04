using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SLProject.SLCompilerLib.Lexer
{
    public class Lexer
    {
        string code;
        int length;
        int index;
        List<string> keywords;


        public Lexer(List<string> keywords)
        {
            this.keywords = keywords;
        }

        public List<Token> Lex(string code)
        {
            Debug.Write("-----START LEXER-----\n");

            List<Token> ret = new List<Token>();
            this.code = code;
            this.length = code.Length;
            index = 0;
            Token token = new Token(TokenType.Null);


            while(token.Type != TokenType.EOF)
            {
                token = GetNextToken();
                Debug.Write("New TOKEN found : " + token);
                ret.Add(token);
            }
            Debug.Write("\n-----END LEXER-----\n");

            return ret;
        }

        Token GetNextToken()
        {
            Token ret = null;

            //remove spaces
            while (IsSpace(PeekChar())) { Advance(); }

            if (IsEOF())
                return new Token(TokenType.EOF);

            if (IsRTL(PeekChar()))
            {
                Advance();
                return new Token(TokenType.RTL);
            }

            if (ScanIdentifierOrKeyword(ref ret))
                return ret;

            if (ScanString(ref ret))
                return ret;

            if (ScanNumber(ref ret))
                return ret;

            if (ScanSign(ref ret))
                return ret;


            //error
            ret = new Token(TokenType.Error, $"Unknown value [\"{PeekChar()}\"] !");
            Advance();
            return ret;
        }

        bool ScanString(ref Token token)
        {
            int startPos = index;
            int len = 0;

            if (PeekChar() != '\"')
                return false;

            while (true)
            {
                len++;
                if (Advance() == '\"')
                {
                    len++;
                    Advance();
                    break;
                }
                
            }
            token = new Token(TokenType.String, code.Substring(startPos, len).Trim('"'));
            return true;
        }

        bool ScanNumber(ref Token token)
        {
            int startPos = index;
            bool hasPoint = false;
            int len = 0;

            if (!char.IsDigit(PeekChar()))
                return false;

            while (true)
            {
                if (char.IsDigit(PeekChar()))
                {
                    len++;
                }else if(PeekChar() == '.' && !hasPoint)
                {
                    hasPoint = true;
                    len++;
                }
                else
                {
                    break;
                }
                Advance();
            }
            string extracted = code.Substring(startPos, len);
            double res = 0;

            if(double.TryParse(extracted, System.Globalization.NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out res))
            {
                token = new Token(TokenType.Number, res);
                return true;
            }
            else
            {
                return false;
            }
        }

        bool ScanIdentifierOrKeyword(ref Token token)
        {
            bool stop = false;
            int startPos = index;
            int len = 0;

            while (!stop)
            {
                switch (PeekChar())
                {
                    case '_':
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                        len++;
                        Advance();
                        break;
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        if (len == 0)
                        {
                            goto default;
                        }
                        else
                        {
                            len++;
                            Advance();
                            break;
                        }
                    default:
                        stop = true;
                        break;
                        
                }//switch
            }//while
            if (len == 0)
                return false;
            else
            {
                string extracted = code.Substring(startPos, len);
                Token new_tok;
                if (keywords.Contains(extracted))
                    new_tok = new Token(TokenType.Keyword, extracted);
                else
                    new_tok = new Token(TokenType.Identifier, extracted);

                token = new_tok;

                return true;
            }

        }

        bool ScanSign(ref Token token)
        {
            char c = PeekChar();
            bool valid = true;
            TokenType type = TokenType.Null;

            switch (c)
            {
                case '=':
                    if(PeekChar(1) == '=')
                    {
                        type = TokenType.Equal;
                        Advance(2);
                    }
                    else
                    {
                        type = TokenType.EqualEqual;
                        Advance(1);
                    }
                    break;

                case '<':
                    if(PeekChar(1) == '=')
                    {
                        type = TokenType.LessEqual;
                        Advance(2);

                    }
                    else if(PeekChar(1) == '<')
                    {
                        type = TokenType.LessLess;
                        Advance(2);

                    }
                    else
                    {
                        type = TokenType.Less;
                        Advance(1);
                    }
                    break;

                case '>':
                    if (PeekChar(1) == '=')
                    {
                        type = TokenType.GreatherEqual;
                        Advance(2);

                    }
                    else if (PeekChar(1) == '<')
                    {
                        type = TokenType.GreatherGreather;
                        Advance(2);

                    }
                    else
                    {
                        type = TokenType.Greather;
                        Advance(1);
                    }
                    break;

                case '+':
                    type = TokenType.Plus;
                    Advance(1);
                    break;

                case '-':
                    type = TokenType.Minus;
                    Advance(1);
                    break;

                case '*':
                    type = TokenType.Star;
                    Advance(1);
                    break;

                case '/':
                    type = TokenType.Slash;
                    Advance(1);
                    break;

                case '%':
                    type = TokenType.Percent;
                    Advance(1);
                    break;

                case '(':
                    type = TokenType.LPar;
                    Advance(1);
                    break;

                case ')':
                    type = TokenType.RPar;
                    Advance(1);
                    break;

                case '[':
                    type = TokenType.LHook;
                    Advance(1);
                    break;

                case ']':
                    type = TokenType.RHook;
                    Advance(1);
                    break;

                case '{':
                    type = TokenType.LBra;
                    Advance(1);
                    break;

                case '}':
                    type = TokenType.RBra;
                    Advance(1);
                    break;

                case '.':
                    type = TokenType.Point;
                    Advance(1);
                    break;

                case ',':
                    type = TokenType.Comma;
                    Advance(1);
                    break;

                case ':':
                    if(PeekChar(1) == ':')
                    {
                        type = TokenType.ColonColon;
                        Advance(2);
                    }
                    else
                    {
                        type = TokenType.Colon;
                        Advance(1);
                    }
                    break;

                default:
                    valid = false;
                    break;

            }

            if (valid)
                token = new Token(type);

            return valid;
            
        }

        bool IsRTL(char c) => c == '\n';

        bool IsSpace(char c) => c == ' ' || c == '\t';

        char Advance(int step = 1)
        {
            index += step;
            return PeekChar();
        }

        char PeekChar(int offset = 0)
        {
            if (IsEOF(offset))
                return '\0';
            else
                return code[index + offset];
        }

        bool IsEOF(int offset = 0) => index + offset >= length;
    }
}
