using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FlightPlanner.Models
{
    
    public static class FlightStorage
    {
        private static readonly object _flightLock = new object();
        public static HashSet<Flight> AllFlights = new HashSet<Flight>();
        private static int _id;
        
        public static Flight AddFlight(Flight flight)
        {
            lock (_flightLock)
            {
                flight.Id = _id;
                _id++;
                AllFlights.Add(flight);
                return flight;
            }
        }

        public static Flight FindFlight(int id)
        {
            lock (_flightLock)
            {
                var flight = AllFlights.FirstOrDefault(x => x.Id == id);
                return flight;
            }
        }
    }
}