using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    class StatementsNode
    {
        public enum StatementType
        {
            funcCallStmt,
            ifStmt,
            forStmt,
            whileStmt
        }
    }
}
