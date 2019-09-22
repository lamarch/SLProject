using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class StringNode : ValueNode<string>
    {
        public StringNode(string value) : base(value)
        {
        }
    }
}
