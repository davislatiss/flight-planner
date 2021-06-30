using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FlightPlanner.Attributes;
using FlightPlanner.DbContext;
using FlightPlanner.Models;


namespace FlightPlanner.Controllers
{   
    public class CustomerFlightController : ApiController
    {
        private readonly object _flightLock = new object();

        [Route("api/airports")]
        [HttpGet]
        public IHttpActionResult GetAirports(string search)
        {
            lock (_flightLock)
            {
                using (var ctx = new FlightPlannerDbContext())
                {
                    var output = new List<Airport>();
                    search = search.ToUpper().Trim();

                    foreach (var x in ctx.Flights.Include(x => x.To).Include(x => x.From))
                    {
                        if (x.To.AirportName.ToUpper().Contains(search) ||
                            x.To.City.ToUpper().Contains(search) ||
                            x.To.Country.ToUpper().Contains(search))
                        {
                            output.Add(x.To);
                        }

                        if (x.From.AirportName.ToUpper().Contains(search) ||
                            x.From.City.ToUpper().Contains(search) ||
                            x.From.Country.ToUpper().Contains(search))
                        {
                            output.Add(x.From);
                        }
                    }

                    return Ok(output);
                }
            }
        }

        [HttpPost]
        [Route("api/flights/search")]
        public IHttpActionResult PostFlights(SearchFlightRequest request)
        {
            lock (_flightLock)
            {
                using (var ctx = new FlightPlannerDbContext())
                {
                    var page = new PageResult<Flight>();
                    if (request?.To == null || request?.From == null || request.DepartureDate == null ||
                        request.To == request.From)
                    {
                        return BadRequest();
                    }

                    foreach (var flight in ctx.Flights)
                    {
                        page.TotalItems++;
                        page.Items.Add(flight);
                    }
                    return Ok(page);
                }
            }
        }

        [Route("api/flights/{id}")]
        [HttpGet]
        public IHttpActionResult FindFlightById(int id)
        {
            lock (_flightLock)
            {
                using (var ctx = new FlightPlannerDbContext())
                {
                    var flight = ctx.Flights.Include(f => f.From).Include(f => f.To).FirstOrDefault(f => f.Id == id);

                    if (flight == null)
                    {
                        return NotFound();
                    }

                    return Ok(flight);
                }
            }
        }
    }
}
