using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EventEaseApp.Server.Data
{
    /// <summary>
    /// Service for managing user sessions in the EventEase system.
    /// Debug Points:
    /// - Monitor active sessions count
    /// - Track session cleanup
    /// - Verify session timeouts
    /// </summary>
    public class UserSessionService
    {
        private readonly Dictionary<string, UserSession> _sessions = new();
        private readonly object _lock = new();
        private readonly ILogger<UserSessionService> _logger;
        private readonly TimeSpan _sessionTimeout = TimeSpan.FromHours(2);

        public UserSessionService(ILogger<UserSessionService> logger)
        {
            _logger = logger;
        }

        public UserSession CreateSession(string userId)
        {
            lock (_lock)
            {
                var session = new UserSession { UserId = userId };
                _sessions[session.SessionId] = session;
                _logger.LogInformation($"Created new session: {session}");
                return session;
            }
        }

        public bool ValidateSession(string sessionId)
        {
            lock (_lock)
            {
                if (_sessions.TryGetValue(sessionId, out var session))
                {
                    if (session.HasTimedOut(_sessionTimeout))
                    {
                        _logger.LogWarning($"Session timed out: {session}");
                        _sessions.Remove(sessionId);
                        return false;
                    }
                    session.LastActivity = DateTime.Now;
                    return true;
                }
                return false;
            }
        }

        public void EndSession(string sessionId)
        {
            lock (_lock)
            {
                if (_sessions.TryGetValue(sessionId, out var session))
                {
                    session.IsActive = false;
                    _sessions.Remove(sessionId);
                    _logger.LogInformation($"Ended session: {session}");
                }
            }
        }

        public IEnumerable<UserSession> GetActiveSessions()
        {
            lock (_lock)
            {
                return _sessions.Values
                    .Where(s => s.IsActive && !s.HasTimedOut(_sessionTimeout))
                    .ToList();
            }
        }
    }
}