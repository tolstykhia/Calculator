using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Services
{
    public class ArithmeticExpression
    {
        public decimal? x { get; set; }
        public decimal? y { get; set; }

        public IOperation Operator { get; set; }

        public ArithmeticExpression ExpressionX { get; set; }

        public ArithmeticExpression ExpressionY { get; set; }

    }
}
