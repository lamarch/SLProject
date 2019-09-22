using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class UnaryOpNode<T> : Node<T>
    {
        Node<T> child;
        Func<T, T> op;

        public UnaryOpNode(Node<T> child, Func<T, T> op)
        {
            this.child = child;
            this.op = op;
        }

        public override T Eval()
        {
            return op(child.Eval());
        }
    }
}
