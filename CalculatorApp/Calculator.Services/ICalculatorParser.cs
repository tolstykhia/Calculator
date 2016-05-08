using System;
using System.Collections.Generic;

namespace Calculator.Services
{
    public interface ICalculatorParser
    {
        ArithmeticExpression Parse(string expression);
    }
}
