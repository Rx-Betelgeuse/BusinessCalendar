using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessCalendarBackground.Models
{
    public class Reminding
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan Delay { get; set; }
        public string Composition { get; set; }
        public string AudioPath { get; set; }
        public List<Event> Events { get; set; }

        public Reminding()
        {
            Name = "";
            Delay = new TimeSpan(0, 10, 0);
            Composition = "Аудіо відсутнє";
            AudioPath = "";
        }
    }
}
