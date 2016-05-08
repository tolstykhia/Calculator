using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Calculator.Services;
using Ninject;

namespace Calculator.UI.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorExecuter _calculator;

        public CalculatorController(ICalculatorExecuter calculator)
        {
            this._calculator = calculator;
        }

        public ActionResult _CalculatorView()
        {
            return PartialView("_CalculatorView");
        }

        public ActionResult GetDecision(string expression)
        {
            var result = _calculator.GetDecision(expression);

            if (result == null) return Json("Error", JsonRequestBehavior.AllowGet);

            return Json(result.Value, JsonRequestBehavior.AllowGet);
        }
    }
}
