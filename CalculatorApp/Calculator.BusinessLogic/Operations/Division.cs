using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculator.Services;

namespace Calculator.BusinessLogic.Operations
{
    public class Division : IOperation
    {
        public Division()
        {
            Description = "/";
            Priority = 1;
            IsFuction = false;
        }

        #region IOperationMembers

        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsFuction { get; set; }

        public decimal? Execute(decimal? x, decimal? y)
        {
            return (y ?? 0) != 0 && x != null ? x/y : null;
        }

        #endregion
    }
}
