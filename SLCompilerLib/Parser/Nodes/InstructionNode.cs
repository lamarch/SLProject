using System;
using System.Collections.Generic;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class InstructionNode
    {
        private string function;
        private List<ValueNode> args;

        public InstructionNode(String function, List<ValueNode> args)
        {
            this.function = function ?? throw new ArgumentNullException(nameof(function));
            this.args = args ?? throw new ArgumentNullException(nameof(args));
        }
    }
}
