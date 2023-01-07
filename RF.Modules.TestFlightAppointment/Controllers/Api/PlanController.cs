using DotNetNuke.Entities.Users;
using RF.Modules.TestFlightAppointment.Services;
using System;
using System.Net.Http;
using System.Web.Http;

namespace RF.Modules.TestFlightAppointment.Controllers.Api
{
    public class PlanController : RestApiControllerBase
    {
        public PlanController(
            ITestFlightBookingManager bookingManager
            )
        {
            BookingManager = bookingManager
                ?? throw new ArgumentNullException(nameof(bookingManager));
        }

        private ITestFlightBookingManager BookingManager { get; }

        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage List()
        {
            try
            {
                var plans = BookingManager.FindFlightPlans(
                    UserInfo.IsAdmin
                    );

                return Json(new { plans });
            }
            catch (Exception ex)
            {
                return JsonException(ex);
            }
        }
    }
}