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
        private readonly object _flightLock = new object();

        [Route("api/airports")]
        [HttpGet]
        public IHttpActionResult SearchAirports(string search)
        {
            lock (_flightLock)
            {
                var lst = new List<Airport>();
                var res = AirportStorage.FindAirport(search.ToLower().Trim());

                if (string.IsNullOrEmpty(res.AirportName) ||
                    string.IsNullOrEmpty(res.City) ||
                    string.IsNullOrEmpty(res.Country))
                {
                    return NotFound();
                }

                lst.Add(res);
                return Ok(lst);
            }
        }

        [Route("api/flights/search")]
        [HttpPost]
        public IHttpActionResult SearchFlights(SearchFlightRequest flight)
        {
            lock (_flightLock)
            {
                var page = new PageResult();
                if (flight?.To == null || flight?.From == null || flight.DepartureDate == null ||
                    flight.To == flight.From)
                {
                    return BadRequest();
                }

                foreach (var f in FlightStorage.AllFlights)
                {
                    page.Page += 1;
                    page.TotalItems += 1;
                }

                return Ok(page);
            }
        }

        [Route("api/flights/{id}")]
        [HttpGet]
        public IHttpActionResult FindFlightById(int id)
        {
            lock (_flightLock)
            {
                var flight = FlightStorage.FindFlight(id);
                if (flight == null)
                {
                    return NotFound();
                }

                return Ok(flight);
            }
        }
    }
}
