using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class ValueNode<T> : Node<T>
    {
        protected T value;

        public ValueNode(T value)
        {
            this.value = value;
        }

        public override T Eval()
        {
            return value;
        }
    }
}
