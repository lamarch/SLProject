namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class VariableNode
    {
        public enum VariableAccessor
        {
            Name,
            Value,
            Adress,
            Pointer
        }

        public VariableNode(string name, VariableAccessor accessor)
        {
            this.Name = name;
            this.Accessor = accessor;
        }
        public string Name { get; private set; }
        public VariableAccessor Accessor { get; private set; }
    }
}
