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
                    new Division()
                });

            this._parser = new CalculatorParser(_operations.Object);
        }

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
                    Operator = new Addition(),
                }
            };
            yield return new object[]
            {
                "(24+3*2.5)/(-2)-3*(1-2)",
                new ArithmeticExpression()
                {
                    x = null,
                    y = null,
                    Operator = new Substraction(),
                    ExpressionX = new ArithmeticExpression()
                    {
                        x = null,
                        y = null,
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
                                y = 2.5m,
                                Operator = new Multiplication(),
                                ExpressionX = null,
                                ExpressionY = null
                            }
                        },
                        ExpressionY = new ArithmeticExpression()
                        {
                            x=0,
                            y=2,
                            Operator = new Substraction(),
                            ExpressionX = null,
                            ExpressionY = null,
                        }
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
                "(-3.5-3/2)*2.5+3/(1+2)",
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
                            x = null,
                            y = null,
                            Operator = new Substraction(),
                            ExpressionX = new ArithmeticExpression()
                            {
                                x=0,
                                y=3.5m,
                                Operator = new Substraction(),
                                ExpressionX = null,
                                ExpressionY = null
                            },
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
        [MemberData("GetExpressionInfixNotations")]
        public void ReturnPostfixNotationOfExpression(string expressionStr, List<string> expectedStock)
        {
            //average

            //act
            var expressionStock = _parser.ConvertToPostfixNotation(expressionStr);

            //assert
            Assert.Equal(expectedStock, expressionStock);
        }

        [Theory]
        [MemberData("GetExpressions")]
        public void ReturnArithmeticExpression(string expressionStr, ArithmeticExpression expectedArithmeticExpression)
        {
            //average

            //act
            var expression = _parser.Parse(expressionStr);

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

            //act
            var expression = _parser.Parse(expressionStr);

            //assert
            Assert.Null(expression);
        }
    }
}
