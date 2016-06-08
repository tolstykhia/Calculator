using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Services;

namespace Calculator.BusinessLogic
{
    public class CalculatorExecuter : ICalculatorExecuter
    {
        private readonly ICalculatorParser _calculatorParser;

        public CalculatorExecuter(ICalculatorParser calculatorParser)
        {
            _calculatorParser = calculatorParser;
        }

        public decimal? GetDecision(string expressionStr)
        {
            var expressionStack = _calculatorParser.Parse(expressionStr);

            if (expressionStack == null) return null;
            
            return GetResult(expressionStack, expressionStack.Pop());
        }

        private decimal? GetResult(Stack<object> expression, object elem)
        {
            IOperation operation;
            if (elem is IOperation)
            {
                operation = elem as IOperation;
                var y = GetResult(expression, expression.Pop());
                var x = operation.IsFuction ? null : GetResult(expression, expression.Pop());
                return operation.Execute(x, y);
            }

            return (decimal) elem;
        }
    }
}
