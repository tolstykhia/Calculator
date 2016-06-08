using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Services;

namespace Calculator.BusinessLogic.Operations
{
    public class Pow : IOperation
    {
        public Pow()
        {
            Description = "^";
            Priority = 0;
            IsFuction = false;
        }

        #region IOperationMembers

        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsFuction { get; set; }

        public decimal? Execute(decimal? x, decimal? y)
        {
            return (x == null || y == null) ? null : (decimal?)Math.Pow((double)x, (double)y);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is Pow)) return false;
            return Description == (obj as Pow).Description;
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }
    }
}
