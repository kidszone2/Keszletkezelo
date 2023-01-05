﻿using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Services.Implementations;
using System.Web.Mvc;

namespace RF.Modules.TestFlightAppointmentRF.Modules.TestFlightAppointnent.Controllers
{
    
    public class TestFlightBookingApiController : DnnController
    {
        [Route("TestFlightBooking/Api/Plans/List")]
        [HttpGet]
        public ActionResult ListPlans()
        {
            var plans = TestFlightBookingManager.Instance.FindFlightPlans(
                User.IsAdmin
                );

            return Json(plans);
        }
    }
}