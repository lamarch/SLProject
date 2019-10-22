using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Lexer
{
    public enum TokenType
    {
        Error,
        EOF,
        Null,
        RTL,
        
        Number,
        String,

        Equal,
        EqualEqual,
        Greather,
        //GreatherGreather,
        GreatherEqual,
        Less,
        //LessLess,
        LessEqual,
        Ampersand,
        //AmpersandAmpersand,
        //Bar,
        //BarBar,
        //Exclamation,
        ExclamationEqual,
        //Tilde,
        Dollar,

        Plus,
        //PlusPlus,
        Minus,
        //MinusMinus,
        Star,
        //StarStar,
        Percent,
        Slash,
        AntiSlash,

        //DQuote,
        //SQuote,
        LPar,
        RPar,
        //LBra,
        //RBra,
        //LHook,
        //RHook,

        //Point,
        //Comma,
        Colon,
        //ColonColon,
        //SemiColon,

        //Keyword,
        Identifier,
    }
}
