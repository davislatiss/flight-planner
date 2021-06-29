using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlightPlanner.Models
{
    public class PageResult<T> where T : Flight

    {
    public int Page { get; set; }
    public int TotalItems { get; set; }
    public List<T> Items = new List<T>();
    }
}