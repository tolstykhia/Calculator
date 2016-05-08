using System;
using System.Collections.Generic;
using Calculator.Domain;

namespace Calculator.Services
{
    public interface ICalculatorParser
    {
        ArithmeticExpression Parse(string expression);
    }
}
