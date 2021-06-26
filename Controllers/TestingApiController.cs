using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Http;
using FlightPlanner.DbContext;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    public class TestingApiController : ApiController
    {
        [Route ("Testing-api/clear")]
        [HttpPost]
        public IHttpActionResult Clear ()
        {
            using (var ctx = new FlightPlannerDbContext())
            {
                var airports = from f in ctx.Airports 
                    select f;

                foreach (var airport in airports)
                {
                    ctx.Airports.Remove(airport);
                }

                var flights = from f in ctx.Flights
                    select f;

                foreach (var flight in flights)
                {
                    ctx.Flights.Remove(flight);
                }

                ctx.SaveChanges();
                return Ok(); 
            }
            
        }
    }
}
