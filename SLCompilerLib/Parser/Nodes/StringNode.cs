using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class StringNode
    {
        public StringNode(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
