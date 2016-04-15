using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Calculator.Domain;
using Calculator.Services;

namespace Calculator.BusinessLogic
{
    public class CalculatorParser : ICalculatorParser
    {
        private readonly String _standartNumberPattern;
        private readonly String _standartArithOperationPattern;

        public CalculatorParser()
        {
            this._standartNumberPattern = @"(?'Open'\(\-)?\d+(\.|\,)?\d*(?'Close-Open'\))?(?(Open)(?!))";
            this._standartArithOperationPattern = @"(\+|\-|\*|\/)+";
        }

        public List<String> ConvertToPostfixNotation(String expression)
        {
            var result = new List<String>();
            var rgx = new Regex(@"(" + _standartNumberPattern + @"|\(|\)|" + _standartArithOperationPattern + @")");
            var matches = rgx.Matches(expression);

            var arithOpStack = new List<String>();

            foreach (Match match in matches)
            {
                if (new Regex(_standartNumberPattern).IsMatch(match.Value))
                {
                    result.Add(match.Value);
                    continue;
                }

                if (match.Value == "(")
                {
                    arithOpStack.Add(match.Value);
                    continue;
                }

                if (match.Value == ")")
                {
                    for (int i = arithOpStack.Count - 1; i >= 0; i--)
                    {
                        if (arithOpStack[i] != "(")
                        {
                            result.Add(arithOpStack[i]);
                            arithOpStack.RemoveAt(i);
                        }
                        else
                        {
                            arithOpStack.RemoveAt(i);
                            break;
                        }
                    }
                    continue;
                }

                if (new Regex(_standartArithOperationPattern).IsMatch(match.Value))
                {
                    var operationPreority = GetOperation(match.Value);

                    for (int i = arithOpStack.Count - 1; i >= 0; i--)
                    {
                        if (arithOpStack[i] == "(") break;
                        if ((int) GetOperation(arithOpStack[i]) <= (int) operationPreority)
                        {
                            result.Add(arithOpStack[i]);
                            arithOpStack.RemoveAt(i);
                        }
                        else
                        {
                            break;
                        }
                    }
                    arithOpStack.Add(match.Value);
                }
            }

            for (int i = arithOpStack.Count - 1; i >= 0; i--)
            {
                result.Add(arithOpStack[i]);
                arithOpStack.RemoveAt(i);
            }

            return result;
        }

        private OperationType GetOperation(String operationStr)
        {
            switch (operationStr)
            {
                case "+": return OperationType.Sum;
                case "-": return OperationType.Subtraction;
                case "*": return OperationType.Multiplication; 
                case "/": return OperationType.Division;
                default: return OperationType.None;
            }
        }

        public ArithmeticExpression Parse(string expression)
        {
            return new ArithmeticExpression();
        }
    }
}
