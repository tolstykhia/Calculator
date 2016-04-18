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
        private readonly Regex _standartArithOperationRegex;
        private readonly Regex _standartNumberRegex;
        private List<String> _postfixExpression;
        private readonly  List<Operation> _operations = new List<Operation>()
        {
            new Operation(){Description = "+", Priority = 2},
            new Operation(){Description = "-", Priority = 2},
            new Operation(){Description = "*", Priority = 1},
            new Operation(){Description = "/", Priority = 1},
        }; 

        public CalculatorParser()
        {
            this._standartNumberPattern = @"(?'Open'\(\-)?\d+(\.|\,)?\d*(?'Close-Open'\))?(?(Open)(?!))";
            var tmpOperationStr = @"(";
            _operations.ForEach(x => tmpOperationStr += @"\" + x.Description + (_operations.IndexOf(x) != _operations.Count - 1 ? @"|" : @")+"));
            this._standartArithOperationPattern = tmpOperationStr;
            this._standartNumberRegex = new Regex(_standartNumberPattern);
            this._standartArithOperationRegex = new Regex(_standartArithOperationPattern);
            this._postfixExpression = new List<String>();
        }

        public List<String> ConvertToPostfixNotation(String expression)
        {
            var result = new List<String>();
            var rgx = new Regex(@"(" + _standartNumberPattern + @"|" + _standartArithOperationPattern + @"|\(|\))");
            var matches = rgx.Matches(expression);

            var arithOpStack = new List<String>();

            foreach (Match match in matches)
            {
                if (_standartNumberRegex.IsMatch(match.Value))
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

                if (_standartArithOperationRegex.IsMatch(match.Value))
                {
                    var operationPreority = GetPriority(match.Value);

                    for (int i = arithOpStack.Count - 1; i >= 0; i--)
                    {
                        if (arithOpStack[i] == "(") break;
                        if (GetPriority(arithOpStack[i]) <= operationPreority)
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

        public ArithmeticExpression Parse(string expression)
        {
            this._postfixExpression = ConvertToPostfixNotation(expression);

            Int32 outIndex;
            var result = GetArithmeticExpression(null, _postfixExpression.Count - 1, out outIndex);

            return result;
        }

        private ArithmeticExpression GetArithmeticExpression(ArithmeticExpression result, Int32 index, out Int32 outIndex)
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
                Decimal decimalValue;

                Decimal.TryParse(_postfixExpression[index].Replace("(", "").Replace(")", "").Replace(".", ","), out decimalValue);

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

        private ArithmeticExpression GetSubArithmeticExpression(Int32 index, out Int32 outIndex)
        {
            var result = new ArithmeticExpression() { Operator = GetOperation(_postfixExpression[index]) };
            return GetArithmeticExpression(result, index - 1, out outIndex);
        }

        private Operation GetOperation(String operationStr)
        {
            return _operations.FirstOrDefault(x => x.Description == operationStr);
        }

        private Int32 GetPriority(String operationStr)
        {
            return _operations.Where(x => x.Description == operationStr).Select(x => x.Priority).FirstOrDefault();
        }
    }
}
