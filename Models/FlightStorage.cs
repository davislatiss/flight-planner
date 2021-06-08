using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FlightPlanner.Models
{
    public static class FlightStorage
    {
        public static List<Flight> AllFlights = new List<Flight>();
        private static int _id;

        public static Flight AddFlight(Flight flight)
        {
            flight.Id = _id;
            _id++;
            AllFlights.Add(flight);
            return flight;
        }

        public static Flight FindFlight(int id)
        {
            return AllFlights.FirstOrDefault(x => x.Id == id);
        }
    }
}