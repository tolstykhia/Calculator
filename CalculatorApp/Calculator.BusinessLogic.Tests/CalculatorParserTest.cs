using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Domain;
using Xunit;

namespace Calculator.BusinessLogic.Tests
{
    public class CalculatorParserTest
    {
        public static IEnumerable<object[]> GetExpressionInfixNotations()
        {
            yield return new object[]
            {
                "2+2",
                new List<String>(){"2", "2", "+"}
            };
            yield return new object[]
            {
                "3+4*2/(1-5)",
                new List<String>() {"3","4","2","*","1","5","-","/","+"} 
            };
            yield return new object[]
            {
                "(24+3*2.5)/(-2)-3*(1-2)",
                new List<String>() {"24","3","2.5","*","+","(-2)","/","3","1","2","-","*","-"} 
            };
        }

        [Theory]
        [MemberData("GetExpressionInfixNotations")]
        public void ReturnPostfixNotationOfExpression(String expressionStr, List<String> expectedStock)
        {
            //average
            var parser = new CalculatorParser();

            //act
            var expressionStock = parser.ConvertToPostfixNotation(expressionStr);

            //assert
            Assert.Equal(expectedStock, expressionStock);
        }
    }
}
