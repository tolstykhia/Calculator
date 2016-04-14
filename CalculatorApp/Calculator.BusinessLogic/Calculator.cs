using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Services;
using Calculator.Domain;

namespace Calculator.BusinessLogic
{
    public class CalculatorExecuter
    {
        private readonly ICalculatorParser _calculatorParser;

        public CalculatorExecuter(ICalculatorParser calculatorParser)
        {
            _calculatorParser = calculatorParser;
        }

        public Decimal Sum(Decimal x, Decimal y)
        {
            return x + y;
        }

        public Decimal GetDecision(String expressionStr)
        {
            Decimal result = 0m;
            var expression = _calculatorParser.Parse(expressionStr);

            if (expression.Operator == OperationType.Sum)
                result = Sum(expression.x, expression.y);

            return result;
        }
    }
}
