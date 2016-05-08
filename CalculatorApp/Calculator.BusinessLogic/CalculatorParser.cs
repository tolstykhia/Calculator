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
        private List<string> _postfixExpression;
        private readonly  IOperationCollection _operations;

        public CalculatorParser(IOperationCollection operations)
        {
            this._operations = operations;
            this._standartNumberPattern = @"\d+(\.|\,)?\d*";
            var tmpOperationStr = @"(";
            _operations.OperationList.ForEach(x => tmpOperationStr += @"\" + x.Description + (_operations.OperationList.IndexOf(x) != _operations.OperationList.Count - 1 ? @"|" : @"){1}"));
            this._standartArithOperationPattern = tmpOperationStr;
            this._standartNumberRegex = new Regex(_standartNumberPattern);
            this._standartArithOperationRegex = new Regex(_standartArithOperationPattern);
            this._postfixExpression = new List<string>();
        }

        public List<string> ConvertToPostfixNotation(string expression)
        {
            var result = new List<string>();
            var rgx = new Regex(@"(" + _standartNumberPattern + @"|" + _standartArithOperationPattern + @"|\(|\))");
            var matches = rgx.Matches(expression);

            var arithOpStack = new List<string>();
            var startIdex = 0;
            if (matches[startIdex].Value == "-")
            {
                result.Add("0");
                result.Add(matches[1].Value);
                result.Add("-");
                startIdex = 2;
            }

            for (int j = startIdex; j < matches.Count; j++)
            {
                if (_standartNumberRegex.IsMatch(matches[j].Value))
                {
                    result.Add(matches[j].Value);
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

                if (_standartArithOperationRegex.IsMatch(matches[j].Value))
                {
                    if (matches[j].Value == "-" &&
                        (_standartArithOperationRegex.IsMatch(matches[j - 1].Value) || matches[j - 1].Value == "("))
                    {
                        result.Add("0");
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
                            result.Add(matches[j + 1].Value);
                            result.Add(countUnMinus % 2 == 0 ? "+" : "-");
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
                            result.Add(arithOpStack[i]);
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
                result.Add(arithOpStack[i]);
                arithOpStack.RemoveAt(i);
            }

            return result;
        }

        public ArithmeticExpression Parse(string expression)
        {
            expression = expression.Replace(" ", "");
            if (!IsValidExpression(expression)) return null;

            this._postfixExpression = ConvertToPostfixNotation(expression);

            int outIndex;
            var result = GetArithmeticExpression(null, _postfixExpression.Count - 1, out outIndex);

            return result;
        }

        private bool IsValidExpression(string expression)
        {
            var rgx = new Regex(@"(" + _standartNumberPattern + @"|" + _standartArithOperationPattern + @"|\(|\))");
            var matches = rgx.Matches(expression);


            string tmpExpression = matches.Cast<Match>().Aggregate("", (current, match) => current + match.Value);

            return tmpExpression == expression;
        }

        private ArithmeticExpression GetArithmeticExpression(ArithmeticExpression result, int index, out int outIndex)
        {
            outIndex = index;
            if (index < 0) return result;

            if (_standartArithOperationRegex.IsMatch(_postfixExpression[index]) && !_standartNumberRegex.IsMatch(_postfixExpression[index]))
            {
                if (result == null)
                {
                    result = GetSubArithmeticExpression(index, out outIndex);
                }
                else
                {
                    if (result.ExpressionY == null && result.y == null)
                    {
                        result.ExpressionY = GetSubArithmeticExpression(index, out outIndex);
                    }
                    else
                    {
                        if (result.ExpressionX == null && result.x == null)
                        {
                            result.ExpressionX = GetSubArithmeticExpression(index, out outIndex);
                        }
                    }
                }

                index = outIndex;

                if (index >= 0 && result.x == null && result.ExpressionX == null || result.y == null && result.ExpressionY == null)
                {
                    result = GetArithmeticExpression(result, index, out outIndex);
                    index = outIndex;
                    if (index < 0) return result;
                }
                else return result;
            }

            if (_standartNumberRegex.IsMatch(_postfixExpression[index]))
            {
                decimal decimalValue;

                decimal.TryParse(_postfixExpression[index].Replace("(", "").Replace(")", "").Replace(".", ","), out decimalValue);

                if (result.y == null && result.ExpressionY == null)
                {
                    result.y = decimalValue;
                    return GetArithmeticExpression(result, index - 1, out outIndex);
                }
                if (result.x == null && result.ExpressionX == null)
                {
                    result.x = decimalValue;
                    outIndex = index - 1;
                    return result;
                }
            }

            return result;
        }

        private ArithmeticExpression GetSubArithmeticExpression(int index, out int outIndex)
        {
            var result = new ArithmeticExpression() { Operator = GetOperation(_postfixExpression[index]) };
            return GetArithmeticExpression(result, index - 1, out outIndex);
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
