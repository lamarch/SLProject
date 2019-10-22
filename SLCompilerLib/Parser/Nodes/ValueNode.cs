namespace SLProject.SLCompilerLib.Parser.Nodes
{
    public class ValueNode
    {
        public enum ValueType
        {
            number, @string, var
        }

        private ValueType valueType;
        private object value;

        public ValueNode(ValueType valueType, object value)
        {
            this.valueType = valueType;
            this.value = value;
        }

        public object GetValue { get; private set; }

        public NumberNode GetNumber { get => (NumberNode)value; }
        public StringNode GetString { get => (StringNode)value; }
        public VariableNode GetVariable { get => (VariableNode)value; }
    }
}
