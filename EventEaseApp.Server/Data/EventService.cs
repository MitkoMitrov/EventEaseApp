using System;
using System.Collections.Generic;
using System.Linq;

namespace EventEaseApp.Server.Data
{
    /// <summary>
    /// Service for managing events in the EventEase system.
    /// Debug Points:
    /// - Check event ordering in GetAll()
    /// - Verify event ID uniqueness
    /// - Monitor event data mutations
    /// - Validate event dates
    /// </summary>
    public class EventService
    {
        // In-memory storage for events
        private readonly List<Event> _events;
        private readonly object _lock = new object();

        public EventService()
        {
            _events = new List<Event>
            {
                // Sample events for testing and debugging
                new Event { 
                    Id = 1, 
                    Name = "Corporate Summit 2026", 
                    Date = DateTime.Today.AddDays(14), 
                    Location = "Seattle Convention Center", 
                    Description = "A full-day event for corporate partners and stakeholders." 
                },
                new Event { 
                    Id = 2, 
                    Name = "Charity Gala", 
                    Date = DateTime.Today.AddDays(30), 
                    Location = "Grand Ballroom, City Hotel", 
                    Description = "An evening gala to raise funds for local charities." 
                },
                new Event { 
                    Id = 3, 
                    Name = "Team Offsite", 
                    Date = DateTime.Today.AddDays(7), 
                    Location = "Lakeside Retreat", 
                    Description = "A two-day team-building offsite with workshops and activities." 
                },
            };
        }

        // Debug: Check ordering of events
        public IEnumerable<Event> GetAll() 
        {
            lock(_lock)
            {
                return _events
                    .Where(e => e.Date >= DateTime.Today) // Debug: Only future events
                    .OrderBy(e => e.Date)
                    .ToList(); // Debug: ToList() to materialize query
            }
        }

        // Debug: Verify null handling
        public Event? GetById(int id)
        {
            lock(_lock)
            {
                return _events.FirstOrDefault(e => e.Id == id);
            }
        }
    }
}
