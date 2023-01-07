using System.Net.Http;
using System.Web.Http;

namespace RF.Modules.TestFlightAppointment.Controllers.Api
{
    public class PlanController : RestApiControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Hello()
        {
            return Json(new { message = "Hello, world!" });
        }
    }
}