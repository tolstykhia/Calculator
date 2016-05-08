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
        public decimal? x { get; set; }
        public decimal? y { get; set; }

        public Operation Operator { get; set; }

        public ArithmeticExpression ExpressionX { get; set; }

        public ArithmeticExpression ExpressionY { get; set; }

    }
}
