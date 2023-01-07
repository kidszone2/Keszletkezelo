using DotNetNuke.Web.Api;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace RF.Modules.TestFlightAppointment.Controllers.Api
{
    public class RestApiControllerBase : DnnApiController
    {
        protected HttpResponseMessage Json(object data)
            => Json(200, data);

        protected HttpResponseMessage Json(HttpStatusCode status, object data)
        {
            // Debugger.Launch();

            // var json = JsonConvert.SerializeObject(data); 
            var response = Request.CreateResponse(
                status,
                data,
                "application/json"
                );
            return response;
        }

        protected HttpResponseMessage Json(int status, object data)
            => Json((HttpStatusCode)status, data);
    }
}