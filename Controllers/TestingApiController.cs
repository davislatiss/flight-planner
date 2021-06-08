using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FlightPlanner.Controllers
{
    public class TestingApiController : ApiController
    {
        [Route ("Testing-api/clear")]
        [HttpPost]
        public IHttpActionResult Clear ()
        {
            return Ok();
        }
    }
}
