using System;
using System.Collections.Generic;
//using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ModelsLibrary.Models
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        private DateTime birthday;
        public DateTime Birthday { get { return birthday.Date; } set { birthday = value; } }
        public byte[] Photo { get; set; }         
        //public ImageSource Image { get { return ImageFromBytes(Photo).Result; } }     

        public string Phone { get; set; }

        public string DisplayMember
        {
            get
            {
                return Name + " " + Surname;
            }
        }

        public List<Event> Events { get; set; }
    }
}
