using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightPlanner.Models
{
    public class AirportStorage
    {
        public static List<Airport> AllAirports = new List<Airport>();

        public static Airport AddAirport(Airport airport)
        {
            AllAirports.Add(airport);
            return airport;
        }

        public static Airport FindAirport(string airportName)
        {
            var res = AllAirports.FirstOrDefault(airport => airport.AirportName.Contains(airportName));
            return res;
        }
    }
}