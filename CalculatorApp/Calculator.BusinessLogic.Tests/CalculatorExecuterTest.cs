using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Calculator.BusinessLogic;
using Calculator.BusinessLogic.Operations;
using Calculator.Services;
using Moq;
using Xunit.Extensions;

namespace Calculator.BusinessLogic.Tests
{

    public class CalculatorExecuterTest
    {
        public static IEnumerable<object[]> GetExpressions()
        {
            yield return new object[]
            {
                "2+2",
                4,
                new Stack<object>(new List<object>(){2m, 2m, new Addition()})
            };
            yield return new object[]
            {
                "-3+4*2/(1-5)",
                -5,
                new Stack<object>(new List<object>() {0m, 3m, new Substraction(), 4m, 2m, new Multiplication(),1m,5m,new Substraction(),new Division(), new Addition()}) 
            };
            yield return new object[]
            {
                "(24+3*2.5)/(-2)-3*(1-2)",
                -12.75,
                new Stack<object>(new List<object>() {24m,3m,2.5m,new Multiplication(),new Addition(),0m,2m,new Substraction(),new Division(),3m,1m,2m,new Substraction(), new Multiplication(),new Substraction()}) 
            };
            yield return new object[]
            {
                "2^2+2^3",
                12,
                new Stack<object>(new List<object>(){2m, 2m, new Pow(), 2m, 3m, new Pow(), new Addition()}), 
            };
            yield return new object[]
            {
                "sin(-cos(90+90)+89)",
                1,
                new Stack<object>(new List<object>(){0m,90m, 90m, new Addition(), new Cos(), new Substraction(), 89m, new Addition(), new Sin()}), 
            };
        }

        [Theory]
        [MemberData("GetExpressions")]
        public void ReturnDecisionOfArithmeticExpression(string expressionStr, decimal expectedResult, Stack<object> arithExpression)
        {
            //averrage
            var calculatorParser = new Mock<ICalculatorParser>();
            calculatorParser.Setup(x => x.Parse(expressionStr)).Returns(arithExpression);

            var calculator = new CalculatorExecuter(calculatorParser.Object);

            //act
            var result = calculator.GetDecision(expressionStr);

            //assert
            Assert.Equal(expectedResult, result);
        }
        
    }
}
