using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    class AssignNode<T> : ValueNode<T>
    {
        string name;

        public AssignNode(string name, ValueNode<T> value) : base(value.Eval())
        {
            this.name = name;
        }
    }
}
