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

        public static Airport FindAirport(string name)
        {
            var airport = AllAirports.Find(a => a.AirportName.ToLower().Contains(name) || a.City.ToLower().Contains(name) || a.Country.ToLower().Contains(name));
            return airport;
        }
    }
}