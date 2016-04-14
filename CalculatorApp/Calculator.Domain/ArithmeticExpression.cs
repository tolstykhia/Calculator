using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Domain;

namespace Calculator.Domain
{
    public class ArithmeticExpression
    {
        public Decimal? x { get; set; }
        public Decimal? y { get; set; }

        public OperationType Operator { get; set; }

        public ArithmeticExpression ExpressionX { get; set; }

        public ArithmeticExpression ExpressionY { get; set; }

    }
}
