using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace EventEaseApp.Server.Data
{
    /// <summary>
    /// Service for managing event attendance in the EventEase system.
    /// Debug Points:
    /// - Monitor attendance counts
    /// - Track status changes
    /// - Verify check-in process
    /// </summary>
    public class AttendanceService
    {
        private readonly List<Attendance> _attendances = new();
        private readonly object _lock = new();
        private readonly ILogger<AttendanceService> _logger;
        private int _lastId = 0;

        public AttendanceService(ILogger<AttendanceService> logger)
        {
            _logger = logger;
        }

        public Attendance RegisterAttendance(int eventId, string userId)
        {
            lock (_lock)
            {
                var attendance = new Attendance
                {
                    Id = ++_lastId,
                    EventId = eventId,
                    UserId = userId
                };
                _attendances.Add(attendance);
                _logger.LogInformation($"Registered new attendance: {attendance}");
                return attendance;
            }
        }

        public bool UpdateStatus(int attendanceId, AttendanceStatus newStatus)
        {
            lock (_lock)
            {
                var attendance = _attendances.FirstOrDefault(a => a.Id == attendanceId);
                if (attendance == null) return false;

                attendance.Status = newStatus;
                if (newStatus == AttendanceStatus.CheckedIn)
                {
                    attendance.CheckedInAt = DateTime.Now;
                }
                
                _logger.LogInformation($"Updated attendance status: {attendance}");
                return true;
            }
        }

        public IEnumerable<Attendance> GetEventAttendances(int eventId)
        {
            lock (_lock)
            {
                return _attendances
                    .Where(a => a.EventId == eventId)
                    .OrderBy(a => a.RegisteredAt)
                    .ToList();
            }
        }

        public IEnumerable<Attendance> GetUserAttendances(string userId)
        {
            lock (_lock)
            {
                return _attendances
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.RegisteredAt)
                    .ToList();
            }
        }

        public int GetEventAttendeeCount(int eventId)
        {
            lock (_lock)
            {
                return _attendances.Count(a => 
                    a.EventId == eventId && 
                    a.Status != AttendanceStatus.Cancelled);
            }
        }

        public bool HasUserRegistered(int eventId, string userId)
        {
            lock (_lock)
            {
                return _attendances.Any(a => 
                    a.EventId == eventId && 
                    a.UserId == userId && 
                    a.Status != AttendanceStatus.Cancelled);
            }
        }
    }
}