using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Services;

namespace Calculator.BusinessLogic.Operations
{
    public class Addition : IOperation
    {
        public Addition()
        {
            Description = "+";
            Priority = 2;
            IsFuction = false;
        }

        #region IOperationMembers

        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsFuction { get; set; }

        public decimal? Execute(decimal? x, decimal? y)
        {
            return (x == null || y == null) ? null : x + y;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if(!(obj is Addition)) return false;
            return Description == (obj as Addition).Description;
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }
    }
}
