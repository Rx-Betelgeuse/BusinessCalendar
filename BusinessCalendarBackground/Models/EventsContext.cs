﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BusinessCalendarBackground.Models
{
    public sealed class BusinessCalendarContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<Person> Persons { get; set; } 
        public DbSet<Location> Locations { get; set; }
        public DbSet<Reminding> Remindings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=BusinessCalendar.db");
        }

        public IEnumerable<Person> GetMatchingPersons(string query)
        {
            return Persons
                .Where(c => c.Name.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 ||
                            c.Surname.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) > -1 )
                .OrderByDescending(c => c.Name.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                .ThenByDescending(c => c.Surname.StartsWith(query, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
