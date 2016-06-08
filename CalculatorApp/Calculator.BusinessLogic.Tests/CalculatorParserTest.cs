using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.BusinessLogic.Operations;
using Calculator.Services;
using Moq;
using Xunit;

namespace Calculator.BusinessLogic.Tests
{
    public class CalculatorParserTest
    {
        private Mock<IOperationCollection> _operations;
        private CalculatorParser _parser;
        public CalculatorParserTest()
        {
            this._operations = new Mock<IOperationCollection>();
            _operations.Setup(x => x.OperationList)
                .Returns(new List<IOperation>()
                {
                    new Addition(),
                    new Substraction(),
                    new Multiplication(),
                    new Division(),
                    new Pow(),
                    new Cos(),
                    new Sin()
                });

            this._parser = new CalculatorParser(_operations.Object);
        }

        public static IEnumerable<object[]> GetExpressionInfixNotations()
        {
            yield return new object[]
            {
                "2+2",
                new Stack<object>(new List<object>(){2m, 2m, new Addition()}), 
            };
            yield return new object[]
            {
                "-3+4*2/(1-5)",
                new Stack<object>(new List<object>() {0m, 3m, new Substraction(), 4m, 2m, new Multiplication(),1m,5m,new Substraction(),new Division(), new Addition()}),  
            };
            yield return new object[]
            {
                "(24+3*2.5)/(-2)-3*(1-2)",
                new Stack<object>(new List<object>() {24m,3m,2.5m,new Multiplication(),new Addition(),0m,2m,new Substraction(),new Division(),3m,1m,2m,new Substraction(), new Multiplication(),new Substraction()}),  
            };
            yield return new object[]
            {
                "2^2+2^3",
                new Stack<object>(new List<object>(){2m, 2m, new Pow(), 2m, 3m, new Pow(), new Addition()}), 
            };
            yield return new object[]
            {
                "-sin(cos(90+90)+91)",
                new Stack<object>(new List<object>(){0m,90m, 90m, new Addition(), new Cos(), 91m, new Addition(), new Sin(), new Substraction()}), 
            };
            yield return new object[]
            {
                "sin(-cos(90+90)+91)",
                new Stack<object>(new List<object>(){0m,90m, 90m, new Addition(), new Cos(), new Substraction(), 91m, new Addition(), new Sin()}), 
            };
        }

        [Theory]
        [MemberData("GetExpressionInfixNotations")]
        public void ReturnPostfixNotationOfExpression(string expressionStr, Stack<object> expectedStack)
        {
            //average

            //act
            var expressionStack = _parser.ConvertToPostfixNotation(expressionStr);

            //assert
            Assert.Equal(expectedStack, expressionStack);
        }

        [Theory]
        [InlineData("32.5^2+56%")]
        [InlineData("2add2")]
        [InlineData("((-62)/3+36")]
        public void ReturnNullArithmeticExpression(string expressionStr)
        {
            //average

            //act
            var expression = _parser.Parse(expressionStr);

            //assert
            Assert.Null(expression);
        }
    }
}
