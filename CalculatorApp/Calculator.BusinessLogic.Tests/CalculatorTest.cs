﻿using System;
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
using Xunit.Extensions;

namespace Calculator.BusinessLogic.Tests
{

    public class CalculatorTest
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
                                y = 2,
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
                -11.5,
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
        [MemberData("GetExpressions")]
        public void ReturnDecisionOfArithmeticExpression(String expressionStr, Decimal expectedResult, ArithmeticExpression arithExpression)
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