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
        public IHttpActionResult SearchAirports(string search)
        {
            lock (_flightLock)
            {
                using (var ctx = new FlightPlannerDbContext())
                {
                    var res = ctx.Airports.Where(a => a.AirportName.ToLower().Contains(search) ||
                                                               a.City.ToLower().Contains(search) ||
                                                               a.Country.ToLower().Contains(search));
                    return Ok(res);
                }
            }
        }

        [Route("api/flights/search")]
        [HttpPost]
        public IHttpActionResult SearchFlights(SearchFlightRequest request)
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
                        if (flight.From.AirportName == request.From &&
                            flight.To.AirportName == request.To &&
                            flight.DepartureTime.Substring(0, 10) == request.DepartureDate)
                        {
                            page.TotalItems++;
                            page.Items.Add(flight);
                        }
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
