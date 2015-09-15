using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACE.Demo.Light.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected IActionStation ActionStation { get; private set; }
        public ControllerBase(IActionStation actionStation)
        {
            ActionStation = actionStation;
        }
    }
}