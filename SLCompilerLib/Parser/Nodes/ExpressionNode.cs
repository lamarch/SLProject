using System;

namespace SLProject.SLCompilerLib.Parser.Nodes{

    public class ExpressionNode : NumberNode{

        Func<double, double, double> op_bin;
        Func<double, double> op_un;

        NumberNode num_1;
        NumberNode num_2;

        public ExpressionNode(Func<double, double, double> op, NumberNode num_1, NumberNode num_2) : base(0)
        {
            this.num_1 = num_1;
            this.num_2 = num_2;
            this.op_bin = op;
        }

        public ExpressionNode(Func<double, double> op, NumberNode num_1) : base(0)
        {
            this.num_1 = num_1;
            this.op_un = op;
        }

        public override double Eval(){
            if(op_bin != null){
                return op_bin(num_1.Eval(), num_2.Eval());
            }else{
                return op_un(num_1.Eval());
            }
        }
    }
}