using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Calculator.BusinessLogic.Operations;
using Calculator.Services;

namespace Calculator.BusinessLogic
{
    public class CalculatorParser : ICalculatorParser
    {
        private readonly string _standartNumberPattern;
        private readonly string _standartArithOperationPattern;
        private readonly Regex _standartArithOperationRegex;
        private readonly Regex _standartNumberRegex;
        private readonly  IOperationCollection _operations;
        private readonly Regex _standartExpressionRegex;

        public CalculatorParser(IOperationCollection operations)
        {
            this._operations = operations;
            this._standartNumberPattern = @"\d+(\.|\,)?\d*";
            var tmpOperationStr = @"(";
            _operations.OperationList.ForEach(x => tmpOperationStr += @"\" + x.Description + (_operations.OperationList.IndexOf(x) != _operations.OperationList.Count - 1 ? @"|" : @"){1}"));
            this._standartArithOperationPattern = tmpOperationStr;
            this._standartNumberRegex = new Regex(_standartNumberPattern);
            this._standartArithOperationRegex = new Regex(_standartArithOperationPattern);
            this._standartExpressionRegex = new Regex(@"(" + _standartNumberPattern + @"|" + _standartArithOperationPattern + @"|\(|\))");
        }

        public Stack<object> ConvertToPostfixNotation(string expression)
        {
            var result = new Stack<object>();
            var matches = _standartExpressionRegex.Matches(expression);

            var arithOpStack = new List<string>();

            for (int j = 0; j < matches.Count; j++)
            {
                decimal decimalValue;
                if (_standartNumberRegex.IsMatch(matches[j].Value))
                {
                    decimal.TryParse(matches[j].Value.Replace("(", "").Replace(")", "").Replace(".", ","), out decimalValue);
                    result.Push(decimalValue);
                    continue;
                }

                if (matches[j].Value == "(")
                {
                    arithOpStack.Add(matches[j].Value);
                    continue;
                }

                if (matches[j].Value == ")")
                {
                    for (int i = arithOpStack.Count - 1; i >= 0; i--)
                    {
                        if (arithOpStack[i] != "(")
                        {
                            result.Push(GetOperation(arithOpStack[i]));
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

                if (_standartArithOperationRegex.IsMatch(matches[j].Value))
                {
                    if (matches[j].Value == "-" &&
                        (j == 0 || _standartArithOperationRegex.IsMatch(matches[j - 1].Value) || matches[j - 1].Value == "("))
                    {
                        result.Push(0m);
                        int countUnMinus = 1;
                        while (!_standartNumberRegex.IsMatch(matches[j + 1].Value))
                        {
                            if (matches[j + 1].Value == "-")
                                countUnMinus++;
                            else break;
                            j++;
                        }
                        if (_standartNumberRegex.IsMatch(matches[j + 1].Value))
                        {
                            decimal.TryParse(matches[j + 1].Value.Replace("(", "").Replace(")", "").Replace(".", ","), out decimalValue);
                            result.Push(decimalValue);
                            result.Push(countUnMinus % 2 == 0 ? GetOperation("+") : GetOperation("-"));
                            j++;
                            continue;
                        }
                    }

                    var operationPriority = GetPriority(matches[j].Value);

                    for (int i = arithOpStack.Count - 1; i >= 0; i--)
                    {
                        if (arithOpStack[i] == "(") break;
                        if (GetPriority(arithOpStack[i]) <= operationPriority)
                        {
                            result.Push(GetOperation(arithOpStack[i]));
                            arithOpStack.RemoveAt(i);
                        }
                        else
                        {
                            break;
                        }
                    }
                    arithOpStack.Add(matches[j].Value);
                }
            }
            
            for (int i = arithOpStack.Count - 1; i >= 0; i--)
            {
                result.Push(GetOperation(arithOpStack[i]));
                arithOpStack.RemoveAt(i);
            }

            return result;
        }

        public Stack<object> Parse(string expression)
        {
            expression = expression.Replace(" ", "");
            if (!IsValidExpression(expression)) return null;

            return ConvertToPostfixNotation(expression);
        }

        private bool IsValidExpression(string expression)
        {
            if (new Regex(@"\(").Matches(expression).Count != new Regex(@"\)").Matches(expression).Count) return false;
            var matches = _standartExpressionRegex.Matches(expression);

            string tmpExpression = matches.Cast<Match>().Aggregate("", (current, match) => current + match.Value);

            return tmpExpression == expression;
        }

        private IOperation GetOperation(string operationStr)
        {
            return _operations.OperationList.FirstOrDefault(x => x.Description == operationStr);
        }

        private int GetPriority(string operationStr)
        {
            return _operations.OperationList.Where(x => x.Description == operationStr).Select(x => x.Priority).FirstOrDefault();
        }
    }
}
