using System;

namespace Calculator.Services
{
    public interface ICalculatorExecuter
    {
        decimal? GetDecision(string expressionStr);
    }
}
