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

        public Decimal? GetDecision(String expressionStr)
        {
            var expression = _calculatorParser.Parse(expressionStr);

            return GetResult(expression);
        }

        private Decimal? GetResult(ArithmeticExpression expression)
        {
            var x = expression.x;
            var y = expression.y;
            if (x == null)
                x = GetResult(expression.ExpressionX);
            if (y == null)
                y = GetResult(expression.ExpressionY);

            switch (expression.Operator)
            {
                case OperationType.Sum:
                    return x + y;
                case OperationType.Subtraction:
                    return x - y;
                case OperationType.Multiplication:
                    return x * y;
                case OperationType.Division:
                    return y != 0 ? x / y : null;
                default:
                    return null;
            }
        }
    }
}
