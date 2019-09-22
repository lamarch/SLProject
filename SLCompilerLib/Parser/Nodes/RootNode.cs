using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class RootNode : ValueNode<string>
    {
        public RootNode(string name) : base(name)
        {
        }

        public override string Eval()
        {
            return "SL PROGRAM " + base.value;
        }
    }
}
