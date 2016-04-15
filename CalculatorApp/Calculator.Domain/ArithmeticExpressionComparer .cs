using System.Collections.Generic;

namespace Calculator.Domain
{
    public class ArithmeticExpressionComparer : IEqualityComparer<ArithmeticExpression>
    {
        public bool Equals(ArithmeticExpression objA, ArithmeticExpression objB)
        {
            if (objA == null && objB == null)
                return true;
            if (objA == null || objB == null)
                return false;
            return objA.x == objB.x && objA.y == objB.y && objA.Operator == objB.Operator &&
                   Equals(objA.ExpressionX, objB.ExpressionX) && Equals(objA.ExpressionY, objB.ExpressionY);
        }

        public int GetHashCode(ArithmeticExpression obj)
        {
            return obj.GetHashCode();
        }
    }
}
