using System;
using System.Collections.Generic;
using System.Text;

namespace SLProject.SLCompilerLib.Parser.Nodes
{
    class ValueNode
    {
        public enum ValueType
        {
            number, @string
        }

        ValueType valueType;
        object value;

        public ValueNode(ValueType valueType, object value)
        {
            this.valueType = valueType;
            this.value = value;
        }
    }
}
