using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using RF.Modules.TestFlightAppointment.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace RF.Modules.TestFlightAppointment.Controllers
{
    [DnnHandleError]
    public class TestFlightGridController : DnnController
    {
        private ITestFlightBookingManager BookingManager { get; }

        public TestFlightGridController(
            ITestFlightBookingManager bookingManager
            )
        {
            BookingManager = bookingManager
                ?? throw new ArgumentNullException(nameof(bookingManager));
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("Create");
        }
    }
}