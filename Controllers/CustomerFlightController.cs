using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FlightPlanner.Attributes;
using FlightPlanner.Models;


namespace FlightPlanner.Controllers
{
    public class CustomerFlightController : ApiController
    {
        [Route("api/airports")]
        [HttpGet]
        public IHttpActionResult SearchAirports(string airport)
        {
            var res = AirportStorage.FindAirport(airport); //must implement condition to find airport
            if ()
            {
                
            }
            return Ok(res);
        }

        [Route("api/flights/search")]
        [HttpPost]
        public IHttpActionResult SearchFlights(SearchFlightRequest flight)
        {
            return Ok(); // must implement
        }

        [Route("api/flights/{id}")]
        [HttpGet]
        public IHttpActionResult FindFlightById(int id)
        {
            return Ok(); // must implement
        }
    }
}
