using System;
using System.Collections.Generic;

namespace Calculator.Services
{
    public interface ICalculatorParser
    {
        Stack<object> Parse(string expression);
    }
}
