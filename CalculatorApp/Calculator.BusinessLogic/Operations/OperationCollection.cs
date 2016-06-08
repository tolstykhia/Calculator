using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calculator.BusinessLogic.Operations;
using Calculator.Services;

namespace Calculator.BusinessLogic.Operations
{
    public class OperationCollection : IOperationCollection
    {
        public OperationCollection()
        {
            OperationList = new List<IOperation>
            {
                    new Addition(),
                    new Substraction(),
                    new Multiplication(),
                    new Division(),
                    new Pow(),
                    new Cos(),
                    new Sin()
            };
        }

        public List<IOperation> OperationList { get; set; }
    }
}
