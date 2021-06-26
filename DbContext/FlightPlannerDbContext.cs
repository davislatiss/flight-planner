using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FlightPlanner.Models;

namespace FlightPlanner.DbContext
{
    public class FlightPlannerDbContext :System.Data.Entity.DbContext
    {
        public FlightPlannerDbContext() : base("flight-planner")
        {
            
        }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airport> Airports { get; set; }
    }
}