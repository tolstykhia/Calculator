using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.Services;

namespace Calculator.BusinessLogic.Operations
{
    public class Multiplication : IOperation
    {
        public Multiplication()
        {
            Description = "*";
            Priority = 1;
            IsFuction = false;
        }

        #region IOperationMembers

        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsFuction { get; set; }

        public decimal? Execute(decimal? x, decimal? y)
        {
            return (x == null || y == null) ? null : x*y;
        }

        #endregion
    }
}
