using System;
using System.Collections;

namespace ModelsLibrary.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Date { get; set; }
        public TimeSpan StartDate { get; set; }
        public TimeSpan FinishDate { get; set; }

        public int AdressId { get; set; }
        public Location Adress { get; set; }

        public int RemindingId { get; set; }
        public Reminding Reminding { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
     
        public Event()
        {
            Adress = new Location();
            Person = new Person();
            Title = "";
            Description = "";
        }

        
    }
}
