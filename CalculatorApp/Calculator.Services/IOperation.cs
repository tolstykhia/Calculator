using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Services
{
    public interface IOperation
    {
        string Description { get; set; }

        int Priority { get; set; }

        bool IsFuction { get; set; }

        decimal? Execute(decimal? x, decimal? y);
    }
}
