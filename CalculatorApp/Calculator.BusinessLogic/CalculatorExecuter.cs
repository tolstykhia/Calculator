﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Services;
using Calculator.Domain;

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
            var expression = _calculatorParser.Parse(expressionStr);

            return GetResult(expression);
        }

        private decimal? GetResult(ArithmeticExpression expression)
        {
            if (expression == null) return null;
            var x = expression.x;
            var y = expression.y;
            if (x == null)
                x = GetResult(expression.ExpressionX);
            if (y == null)
                y = GetResult(expression.ExpressionY);

            if (x == null || y == null) return null;

            switch (expression.Operator.Description)
            {
                case "+":
                    return x + y;
                case "-":
                    return x - y;
                case "*":
                    return x * y;
                case "/":
                    return y != 0 ? x / y : null;
                case "^":
                    return (decimal?) Math.Pow((double)x, (double)y);
                default:
                    return null;
            }
        }
    }
}