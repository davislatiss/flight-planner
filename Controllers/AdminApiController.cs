using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using FlightPlanner.Attributes;
using FlightPlanner.DbContext;
using FlightPlanner.Models;

namespace FlightPlanner.Controllers
{
    [BasicAuthentication]
    public class AdminApiController : ApiController
    {
        private static readonly object _flightLock = new object();

        [Route ("admin-api/flights/{id}")]
        public IHttpActionResult GetFlights(int id)
        {
            lock (_flightLock)
            {
                using (var ctx = new FlightPlannerDbContext())
                {
                    var flight = ctx.Flights.Include(f => f.From).Include(f => f.To).SingleOrDefault(f => f.Id == id);
                    return flight == null ? (IHttpActionResult)NotFound() : Ok(flight);
                }
            }
        }

        [Route("admin-api/flights")]
        [BasicAuthentication]
        public IHttpActionResult PutFlight(AddFlightRequest flight)
        {
            lock (_flightLock)
            {
                Flight output = new Flight();

                if (!ValidateNullOrEmpty(flight))
                {
                    return BadRequest("property is null or empty");
                }

                if (ValidateFlightsAreUnique(flight))
                {
                    return Conflict();
                }

                if (!ValidateAirports(flight))
                {
                    return BadRequest("Airports can't be the same");
                }

                if (!ValidateDates(flight))
                {
                    return BadRequest("Arrival can't be later than departure");
                }

                output.ArrivalTime = flight.ArrivalTime;
                output.DepartureTime = flight.DepartureTime;
                output.From = flight.From;
                output.To = flight.To;
                output.Carrier = flight.Carrier;
                
                using (var ctx = new FlightPlannerDbContext())
                {
                    ctx.Flights.Add(output);
                    ctx.SaveChanges();
                }

                return Created("", output);
            }
        }

        [Route("admin-api/flights/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteFlight(int id)
        {
            lock (_flightLock)
            {
                using (var ctx = new FlightPlannerDbContext())
                {
                    var flight = ctx.Flights.SingleOrDefault(f => f.Id == id);
                    if (flight == null)
                    {
                        return Ok();
                    }
                    ctx.Flights.Remove(flight);
                    ctx.SaveChanges();
                    return Ok();
                }
            }
        }

        private bool ValidateNullOrEmpty(AddFlightRequest flight)
        {
            
                if (string.IsNullOrEmpty(flight.Carrier) ||
                    flight.To == null ||
                    flight.From == null ||
                    string.IsNullOrEmpty(flight.To?.AirportName) ||
                    string.IsNullOrEmpty(flight.To?.City) ||
                    string.IsNullOrEmpty(flight.To?.Country) ||
                    string.IsNullOrEmpty(flight.From?.AirportName) ||
                    string.IsNullOrEmpty(flight.From?.City) ||
                    string.IsNullOrEmpty(flight.From?.Country) ||
                    string.IsNullOrEmpty(flight.DepartureTime) ||
                    string.IsNullOrEmpty(flight.ArrivalTime)
                )
                {
                    return false;
                }

                return true;
            
        }

        private bool ValidateFlightsAreUnique(AddFlightRequest newFlight)
        {
            lock (_flightLock)
            {
                using (var ctx = new FlightPlannerDbContext())
                {
                    return (ctx.Flights.Any(x =>
                        x.From.AirportName == newFlight.From.AirportName &&
                        x.From.City == newFlight.From.City &&
                        x.From.Country == newFlight.From.Country &&
                        x.To.AirportName == newFlight.To.AirportName &&
                        x.To.City == newFlight.To.City &&
                        x.To.Country == newFlight.To.Country &&
                        x.Carrier == newFlight.Carrier &&
                        x.ArrivalTime == newFlight.ArrivalTime &&
                        x.DepartureTime == newFlight.DepartureTime
                    ));
                }
            }
        }

        private bool ValidateAirports(AddFlightRequest flight)
        {
            
                if (flight.From.AirportName.ToLower().Trim() == flight.To.AirportName.ToLower().Trim())
                {
                    return false;
                }

                return true;
            
        }

        private bool ValidateDates(AddFlightRequest flight)
        {
            
                var arrivalTime = DateTime.Parse(flight.ArrivalTime);
                var departureTime = DateTime.Parse(flight.DepartureTime);
                if (departureTime >= arrivalTime)
                {
                    return false;
                }
                return true;
            
        }
    }
}
