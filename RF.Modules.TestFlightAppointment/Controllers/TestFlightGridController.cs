using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace RF.Modules.TestFlightAppointment.Controllers
{
    [DnnHandleError]
    public class TestFlightGridController : DnnController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}