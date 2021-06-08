using System.Collections.Generic;
using System.Web.Http;
using FlightPlanner.Attributes;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    [BasicAuthentication]
    public class AdminApiController : ApiController
    {
        [Route ("admin-api/flights/{id}")]
        
        public IHttpActionResult GetFlights(int id)
        {
            var flight = FlightStorage.FindFlight(id);
            return flight == null ? (IHttpActionResult) NotFound() : Ok();
        }

        [Route("admin-api/flights")]
        [BasicAuthentication]
        public IHttpActionResult PutFlight(AddFlightRequest flight)
        {
            Flight output = new Flight();
            output.ArrivalTime = flight.ArrivalTime;
            output.DepartureTime = flight.DepartureTime;
            output.From = flight.From;
            output.To = flight.To;
            output.Carrier = flight.Carrier;
            FlightStorage.AddFlight(output);

            return Created("", output);
        }
    }
}
