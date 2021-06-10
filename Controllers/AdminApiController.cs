using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!ValidateNullOrEmpty(flight))
            {
                return BadRequest("property is null or empty");
            }

            if (!ValidateFlightsAreUnique(flight))
            {
                return Conflict();
            }

            if (!ValidateAirports(flight))
            {
                return BadRequest("Airports cant be the same");
            }

            if (!ValidateDates(flight))
            {
                return BadRequest("Arrival later than departure");
            }

            Flight output = new Flight();
            output.ArrivalTime = flight.ArrivalTime;
            output.DepartureTime = flight.DepartureTime;
            output.From = flight.From;
            output.To = flight.To;
            output.Carrier = flight.Carrier;
            FlightStorage.AddFlight(output);

            return Created("", output);
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
            foreach (var flight in FlightStorage.AllFlights)
            {
                if (flight.ArrivalTime == newFlight.ArrivalTime &&
                    flight.DepartureTime == newFlight.DepartureTime &&
                    flight.To.AirportName == newFlight.To.AirportName &&
                    flight.To.City == newFlight.To.City &&
                    flight.To.Country == newFlight.To.Country &&
                    flight.From.AirportName == newFlight.From.AirportName &&
                    flight.From.City == newFlight.From.City &&
                    flight.From.Country == newFlight.From.Country
                )
                {
                    return false;   
                }
            }
            return true;
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
