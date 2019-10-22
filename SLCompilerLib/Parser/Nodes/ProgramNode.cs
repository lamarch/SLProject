using System;
using System.Collections.Generic;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class ProgramNode
    {
        List<Label> labels;
        List<InstructionNode> instructions;

        public ProgramNode ( List<Label> labels, List<InstructionNode> instructions )
        {
            this.Labels = labels ?? throw new ArgumentNullException( nameof( labels ) );
            this.Instructions = instructions ?? throw new ArgumentNullException( nameof( instructions ) );
        }

        public List<Label> Labels {
            get => this.labels;
            private set => this.labels = value;
        }
        public List<InstructionNode> Instructions {
            get => this.instructions;
            private set => this.instructions = value;
        }
    }
}
