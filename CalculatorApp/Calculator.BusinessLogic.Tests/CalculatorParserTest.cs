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
                new List<string>(){"2", "2", "+"}
            };
            yield return new object[]
            {
                "-3+4*2/(1-5)",
                new List<string>() {"0","3","-","4","2","*","1","5","-","/","+"} 
            };
            yield return new object[]
            {
                "(24+3*2.5)/(-2)-3*(1-2)",
                new List<string>() {"24","3","2.5","*","+","0","2","-","/","3","1","2","-","*","-"} 
            };
        }


        public static IEnumerable<object[]> GetExpressions()
        {
            yield return new object[]
            {
                "2+2",
                new ArithmeticExpression()
                {
                    x = 2,
                    y = 2,
                    Operator = new Operation(){Description = "+", Priority = 2},
                }
            };
            yield return new object[]
            {
                "(24+3*2.5)/(-2)-3*(1-2)",
                new ArithmeticExpression()
                {
                    x = null,
                    y = null,
                    Operator = new Operation(){Description = "-", Priority = 2},
                    ExpressionX = new ArithmeticExpression()
                    {
                        x = null,
                        y = null,
                        Operator = new Operation(){Description = "/", Priority = 1},
                        ExpressionX = new ArithmeticExpression()
                        {
                            x = 24,
                            y = null,
                            Operator = new Operation(){Description = "+", Priority = 2},
                            ExpressionX = null,
                            ExpressionY = new ArithmeticExpression()
                            {
                                x = 3,
                                y = 2.5m,
                                Operator = new Operation(){Description = "*", Priority = 1},
                                ExpressionX = null,
                                ExpressionY = null
                            }
                        },
                        ExpressionY = new ArithmeticExpression()
                        {
                            x=0,
                            y=2,
                            Operator = new Operation(){ Description = "-", Priority = 2},
                            ExpressionX = null,
                            ExpressionY = null,
                        }
                    },
                    ExpressionY = new ArithmeticExpression()
                    {
                        x = 3,
                        y = null,
                        Operator = new Operation(){Description = "*", Priority = 1},
                        ExpressionX = null,
                        ExpressionY = new ArithmeticExpression()
                        {
                            x = 1,
                            y = 2,
                            Operator = new Operation(){Description = "-", Priority = 2},
                            ExpressionX = null,
                            ExpressionY = null
                        }
                    }
                }
            };
            yield return new object[]
            {
                "(-3.5-3/2)*2.5+3/(1+2)",
                new ArithmeticExpression()
                {
                    x = null,
                    y = null,
                    Operator = new Operation(){Description = "+", Priority = 2},
                    ExpressionX = new ArithmeticExpression()
                    {
                        x = null,
                        y = 2.5m,
                        Operator = new Operation(){Description = "*", Priority = 1},
                        ExpressionX = new ArithmeticExpression()
                        {
                            x = null,
                            y = null,
                            Operator = new Operation(){Description = "-", Priority = 2},
                            ExpressionX = new ArithmeticExpression()
                            {
                                x=0,
                                y=3.5m,
                                Operator = new Operation(){Description = "-", Priority = 2},
                                ExpressionX = null,
                                ExpressionY = null
                            },
                            ExpressionY = new ArithmeticExpression()
                            {
                                x = 3,
                                y = 2,
                                Operator = new Operation(){Description = "/", Priority = 1},
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
                        Operator = new Operation(){Description = "/", Priority = 1},
                        ExpressionX = null,
                        ExpressionY = new ArithmeticExpression()
                        {
                            x = 1,
                            y = 2,
                            Operator = new Operation(){Description = "+", Priority = 2},
                            ExpressionX = null,
                            ExpressionY = null
                        }
                    }
                }
            };
        }

        [Theory]
        [MemberData("GetExpressionInfixNotations")]
        public void ReturnPostfixNotationOfExpression(string expressionStr, List<string> expectedStock)
        {
            //average
            var parser = new CalculatorParser();

            //act
            var expressionStock = parser.ConvertToPostfixNotation(expressionStr);

            //assert
            Assert.Equal(expectedStock, expressionStock);
        }

        [Theory]
        [MemberData("GetExpressions")]
        public void ReturnArithmeticExpression(string expressionStr, ArithmeticExpression expectedArithmeticExpression)
        {
            //average
            var parser = new CalculatorParser();

            //act
            var expression = parser.Parse(expressionStr);

            //assert
            Assert.Equal(expectedArithmeticExpression, expression, new ArithmeticExpressionComparer());
        }

        [Theory]
        [InlineData("32.5^2+56%")]
        [InlineData("2add2")]
        [InlineData("((-62)/3+36")]
        public void ReturnNullArithmeticExpression(string expressionStr)
        {
            //average
            var parser = new CalculatorParser();

            //act
            var expression = parser.Parse(expressionStr);

            //assert
            Assert.Null(expression);
        }
    }
}
