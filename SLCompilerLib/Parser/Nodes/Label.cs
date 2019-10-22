namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class Label
    {
        private InstructionNode instruction;
        private string name;
        public Label(string name, InstructionNode instruction)
        {
            this.name = name;
            this.instruction = instruction;
        }

        public string Name { get; private set; }
        public InstructionNode Instruction { get; private set; }
    }
}
