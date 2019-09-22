using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class NumberNode : ValueNode<double>
    {
        public NumberNode(double value) : base(value)
        {
        }
    }
}
