using DotNetNuke.Framework;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using RF.Modules.TestFlightAppointment.Models;
using RF.Modules.TestFlightAppointment.Services;
using System;
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
        private static void InitPopup()
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.jQuery);
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);
            ServicesFramework.Instance.RequestAjaxScriptSupport();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var utcNow = DateTime.UtcNow;
            var from = utcNow
                .AddDays(-(int)utcNow.DayOfWeek + 1)
                .Date;
            var to = from
                .AddDays(8)
                .Date;
            var bookings = BookingManager.FindBookingsByDate(from, to, false);
            ViewBag.Bookings = bookings;

            return View();
        }

        [HttpGet]
        public ActionResult Create(DateTime? departureAt)
        {
            InitPopup();

            var model = new CreateBookingParameters()
            {
                DepartureAt = departureAt ?? DateTime.UtcNow,
            };

            ViewBag.Plans = BookingManager.FindFlightPlans(
                User.IsAdmin
                );

            return PartialView("Create", model);
        }


        [HttpGet]
        public ActionResult Detail(int bookingID)
        {
            InitPopup();

            var booking = BookingManager.FindBookingByID(bookingID);
            return View(booking);
        }
    }
}