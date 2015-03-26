using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ACE.Demo.Web.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected IActionBus ActionBus { get; private set; }
        public ControllerBase(IActionBus actionBus)
        {
            ActionBus = actionBus;
        }
    }
}