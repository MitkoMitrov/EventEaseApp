using System;
using System.ComponentModel.DataAnnotations;

namespace EventEaseApp.Server.Data
{
    /// <summary>
    /// Represents an event attendance record in the EventEase system.
    /// Debug Points:
    /// - Verify attendance status changes
    /// - Monitor check-in times
    /// - Track attendance confirmation
    /// </summary>
    public class Attendance
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; } = DateTime.Now;
        public DateTime? CheckedInAt { get; set; }
        public AttendanceStatus Status { get; set; } = AttendanceStatus.Registered;

        // Debug helper method
        public bool IsCheckedIn()
        {
            return Status == AttendanceStatus.CheckedIn && CheckedInAt.HasValue;
        }

        public override string ToString()
        {
            return $"Attendance[{Id}]: Event {EventId}, User {UserId}, Status: {Status}";
        }
    }

    public enum AttendanceStatus
    {
        Registered,
        Confirmed,
        CheckedIn,
        Cancelled
    }
}