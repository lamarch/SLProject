using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class NumberNode
    {
        protected double value;
        public NumberNode(double value)
        {
            this.value = value;

        }

        public virtual double Eval(){
            return value;
        }
    }
}
