using System;

using System.ComponentModel.DataAnnotations;

namespace EventEaseApp.Server.Data
{
    /// <summary>
    /// Represents an event in the EventEase system.
    /// This class is used for both display and data binding in forms.
    /// Debug Points:
    /// - Check Id assignment in EventService
    /// - Validate Date is not in the past
    /// - Ensure required fields are populated
    /// </summary>
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(100, ErrorMessage = "Name is too long (100 character limit)")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date is required")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(200, ErrorMessage = "Location is too long (200 character limit)")]
        public string Location { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description is too long (1000 character limit)")]
        public string? Description { get; set; }

        // Debug helper method
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) 
                && !string.IsNullOrWhiteSpace(Location) 
                && Date > DateTime.Now;
        }

        // Debug: String representation for logging
        public override string ToString()
        {
            return $"Event[{Id}]: {Name} on {Date:d} at {Location}";
        }
    }
}
