using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class NumberNode
    {
        private double value;
        public NumberNode(double value)
        {
            this.value = value;

        }

        public 

        public double Value { get => value; set => this.value = value; }
    }
}
