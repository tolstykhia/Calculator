using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Calculator.BusinessLogic;
using Calculator.Domain;
using Calculator.Services;
using Moq;

namespace Calculator.BusinessLogic.Tests
{
    public class CalculatorTest
    {
        [Theory]
        [InlineData("2+2", 4)]
        [InlineData("-2+(-2)", -4)]
        [InlineData("(-2)+(-2.5)", -4.5)]
        [InlineData("-2,5+2", -0.5)]
        public void ReturnSumTwoNumbers(String expressionStr, Decimal expectedSum)
        {
            //averrage
            var numbers = GetNumbers(expressionStr);

            var calculatorParser = new Mock<ICalculatorParser>();
            calculatorParser.Setup(x => x.Parse(expressionStr)).Returns(new ArithmeticExpression() { x = numbers[0], y = numbers[1], Operator = OperationType.Sum});

            var calculator = new CalculatorExecuter(calculatorParser.Object);

            //act
            var sum = calculator.GetDecision(expressionStr);

            //assert
            Assert.Equal(expectedSum, sum);
        }

        private List<Decimal> GetNumbers(String expressionStr)
        {
            var rgx = new Regex(@"\(?\-?\d+(\.|\,)?\d*\)?");
            var matches = rgx.Matches(expressionStr);
            var values = new List<String>(2);
            for (int i = 0; i < matches.Count; i++)
            {
                values.Add(matches[i].Value.Replace("(", "").Replace(")", "").Replace(".", ","));
            }

            return values.Select(Decimal.Parse).ToList();
        }
    }
}
