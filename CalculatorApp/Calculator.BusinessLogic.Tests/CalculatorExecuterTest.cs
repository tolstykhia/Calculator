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
                "(24+3*2)/(-2)-3*(1-2)",
                -12,
                new ArithmeticExpression()
                {
                    x = null,
                    y = null,
                    Operator = new Substraction(),
                    ExpressionX = new ArithmeticExpression()
                    {
                        x = null,
                        y = -2,
                        Operator = new Division(),
                        ExpressionX = new ArithmeticExpression()
                        {
                            x = 24,
                            y = null,
                            Operator = new Addition(),
                            ExpressionX = null,
                            ExpressionY = new ArithmeticExpression()
                            {
                                x = 3,
                                y = 2,
                                Operator = new Multiplication(),
                                ExpressionX = null,
                                ExpressionY = null
                            }
                        },
                        ExpressionY = null
                    },
                    ExpressionY = new ArithmeticExpression()
                    {
                        x = 3,
                        y = null,
                        Operator = new Multiplication(),
                        ExpressionX = null,
                        ExpressionY = new ArithmeticExpression()
                        {
                            x = 1,
                            y = 2,
                            Operator = new Substraction(),
                            ExpressionX = null,
                            ExpressionY = null
                        }
                    }
                }
            };
            yield return new object[]
            {
                "((-3.5)-3/2)*2.5+3/(1+2)",
                -11.5,
                new ArithmeticExpression()
                {
                    x = null,
                    y = null,
                    Operator = new Addition(),
                    ExpressionX = new ArithmeticExpression()
                    {
                        x = null,
                        y = 2.5m,
                        Operator = new Multiplication(),
                        ExpressionX = new ArithmeticExpression()
                        {
                            x = -3.5m,
                            y = null,
                            Operator = new Substraction(),
                            ExpressionX = null,
                            ExpressionY = new ArithmeticExpression()
                            {
                                x = 3,
                                y = 2,
                                Operator = new Division(),
                                ExpressionX = null,
                                ExpressionY = null
                            }
                        },
                        ExpressionY = null
                    },
                    ExpressionY = new ArithmeticExpression()
                    {
                        x = 3,
                        y = null,
                        Operator = new Division(),
                        ExpressionX = null,
                        ExpressionY = new ArithmeticExpression()
                        {
                            x = 1,
                            y = 2,
                            Operator = new Addition(),
                            ExpressionX = null,
                            ExpressionY = null
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData("GetExpressions")]
        public void ReturnDecisionOfArithmeticExpression(string expressionStr, decimal expectedResult, ArithmeticExpression arithExpression)
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
