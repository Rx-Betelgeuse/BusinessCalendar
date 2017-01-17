using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCalendarBackground.Models
{
    public class Location
    {
        public int Id { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Adress { get; set; }

        public List<Event> Events { get; set; }
    }
}
