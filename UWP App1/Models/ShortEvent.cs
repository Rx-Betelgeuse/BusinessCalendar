using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessCalendar.Models
{
    public class ShortEvent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public TimeSpan StartDate { get; set; }
        public TimeSpan FinishDate { get; set; }
    }
}
