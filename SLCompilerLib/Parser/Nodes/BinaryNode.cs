using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class BinaryNode<T> : Node<T>
    {
        Node<T> child_1;
        Node<T> child_2;
        Func<T, T, T> op;

        public BinaryNode(Node<T> child_1, Node<T> child_2, Func<T, T, T> op)
        {
            this.child_1 = child_1;
            this.child_2 = child_2;
            this.op = op;
        }
        public override T Eval()
        {
            return op(child_1.Eval(), child_2.Eval());
        }
    }
}
