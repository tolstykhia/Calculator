using System;

namespace Calculator.Services
{
    public interface ICalculatorExecuter
    {
        Decimal? GetDecision(String expressionStr);
    }
}
