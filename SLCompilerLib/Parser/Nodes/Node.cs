using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public abstract class Node<T>
    {
        public abstract T Eval();
    }
}
