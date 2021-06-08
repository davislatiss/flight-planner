using System.Collections.Generic;
using System.Web.Http;
using FlightPlanner.Attributes;

namespace FlightPlanner.Controllers
{
    public class AdminApiController : ApiController
    {
        [Route ("admin-api/flights/{id}")]
        [BasicAuthentication]
        public IEnumerable<string> GetFlights(int id)
        {
            return new string[] { "value1", "value2" };
        }
    }
}
