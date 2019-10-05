using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    class BodiesNode
    {
        public List<FunctionNode> FunctionNodes { get; set; }
        public FunctionNode MainFunctionNode { get; set; }


    }
}
