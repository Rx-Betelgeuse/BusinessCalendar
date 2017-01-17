using BusinessCalendar.Models;
using System;
using System.Collections;

namespace ModelsLibrary.Models
{
    public sealed class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTimeOffset Date { get; set; }
        public TimeSpan StartDate { get; set; }
        public TimeSpan FinishDate { get; set; }

        public string Duration
        {
            get
            {
                if (StartDate == FinishDate)
                {
                    return String.Format("{0}:{1}",
                    StartDate.Hours, StartDate.Minutes);
                }
                else
                {
                    return String.Format("{0}:{1}-{2}:{3}",
                   StartDate.Hours, StartDate.Minutes, FinishDate.Hours, FinishDate.Minutes);
                }
            }
        }

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

        public static explicit operator ShortEvent(Event ev)
        {
            return new ShortEvent() { Description = ev.Description, Date = ev.Date, FinishDate = ev.FinishDate, StartDate = ev.StartDate, Title = ev.Title };
        }

    }
}
