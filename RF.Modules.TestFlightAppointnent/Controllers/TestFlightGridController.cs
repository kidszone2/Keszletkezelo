using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using System.Web.Mvc;

namespace RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Controllers
{
    [DnnHandleError]
    public class TestFlightGridController : DnnController
    {
        [ModuleAction(ControlKey = "Booking Grid Key", TitleKey = "Booking Grid Title")]
        public ActionResult Index()
        {
            return View();
        }
    }
}