using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.Services;

namespace Calculator.BusinessLogic.Operations
{
    public class Cos : IOperation
    {
        public Cos()
        {
            Description = "cos";
            Priority = 0;
            IsFuction = true;
        }

        #region IOperationMembers

        public string Description { get; set; }
        public int Priority { get; set; }
        public bool IsFuction { get; set; }

        public decimal? Execute(decimal? x, decimal? y)
        {
            return (y == null) ? null : (decimal?) Math.Cos((double) y*Math.PI/180);
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (!(obj is Cos)) return false;
            return Description == (obj as Cos).Description;
        }

        public override int GetHashCode()
        {
            return Description.GetHashCode();
        }
    }
}
