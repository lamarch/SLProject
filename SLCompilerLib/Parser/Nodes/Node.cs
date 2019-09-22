using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public abstract class Node<T> : BaseNode
    {
        public abstract new T Eval();
    }
}
