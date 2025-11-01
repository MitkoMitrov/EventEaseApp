using System;

namespace EventEaseApp.Server.Data
{
    /// <summary>
    /// Represents a user session in the EventEase system.
    /// Debug Points:
    /// - Monitor session duration
    /// - Track session state changes
    /// - Validate session timeout
    /// </summary>
    public class UserSession
    {
        public string SessionId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastActivity { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        // Debug helper method
        public TimeSpan GetSessionDuration()
        {
            return DateTime.Now - CreatedAt;
        }

        // Debug helper method
        public bool HasTimedOut(TimeSpan timeout)
        {
            return DateTime.Now - LastActivity > timeout;
        }

        public override string ToString()
        {
            return $"Session[{SessionId}] for User {UserId} - Active: {IsActive}, Duration: {GetSessionDuration().TotalMinutes:F1}m";
        }
    }
}