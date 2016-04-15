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


        public static IEnumerable<object[]> GetExpressions()
        {
            yield return new object[]
            {
                "2+2",
                new ArithmeticExpression()
                {
                    x = 2,
                    y = 2,
                    Operator = OperationType.Sum,
                }
            };
            yield return new object[]
            {
                "(24+3*2.5)/(-2)-3*(1-2)",
                new ArithmeticExpression()
                {
                    x = null,
                    y = null,
                    Operator = OperationType.Subtraction,
                    ExpressionX = new ArithmeticExpression()
                    {
                        x = null,
                        y = -2,
                        Operator = OperationType.Division,
                        ExpressionX = new ArithmeticExpression()
                        {
                            x = 24,
                            y = null,
                            Operator = OperationType.Sum,
                            ExpressionX = null,
                            ExpressionY = new ArithmeticExpression()
                            {
                                x = 3,
                                y = 2.5m,
                                Operator = OperationType.Multiplication,
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
                        Operator = OperationType.Multiplication,
                        ExpressionX = null,
                        ExpressionY = new ArithmeticExpression()
                        {
                            x = 1,
                            y = 2,
                            Operator = OperationType.Subtraction,
                            ExpressionX = null,
                            ExpressionY = null
                        }
                    }
                }
            };
            yield return new object[]
            {
                "((-3.5)-3/2)*2.5+3/(1+2)",
                new ArithmeticExpression()
                {
                    x = null,
                    y = null,
                    Operator = OperationType.Sum,
                    ExpressionX = new ArithmeticExpression()
                    {
                        x = null,
                        y = 2.5m,
                        Operator = OperationType.Multiplication,
                        ExpressionX = new ArithmeticExpression()
                        {
                            x = -3.5m,
                            y = null,
                            Operator = OperationType.Subtraction,
                            ExpressionX = null,
                            ExpressionY = new ArithmeticExpression()
                            {
                                x = 3,
                                y = 2,
                                Operator = OperationType.Division,
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
                        Operator = OperationType.Division,
                        ExpressionX = null,
                        ExpressionY = new ArithmeticExpression()
                        {
                            x = 1,
                            y = 2,
                            Operator = OperationType.Sum,
                            ExpressionX = null,
                            ExpressionY = null
                        }
                    }
                }
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

        [Theory]
        [MemberData("GetExpressions")]
        public void ReturnArithmeticExpression(String expressionStr, ArithmeticExpression expectedArithmeticExpression)
        {
            //average
            var parser = new CalculatorParser();

            //act
            var expression = parser.Parse(expressionStr);

            //assert
            Assert.Equal(expectedArithmeticExpression, expression, new ArithmeticExpressionComparer());
        }
    }
}
