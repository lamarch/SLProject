using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    class FunctionNode
    {
        List<StatementsNode> stmts;

        public void AddStmt(StatementsNode node)
        {
            stmts.Add(node);
        }

        public FunctionNode()
        {
            stmts = new List<StatementsNode>();
        }
    }
}
