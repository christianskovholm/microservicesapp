using System;

namespace OrganizationService.Infrastructure
{
    /// <summary>
    /// Database representation of a domain event.
    /// </summary>
    public class Event
    {
        public int Id { get; private set; }
        public int OrganizationId { get; set; }
        public string Payload { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string EventType { get; set; }

        public Event(int organizationId, string payload, DateTime timestamp, string eventType)
        {
            OrganizationId = organizationId;
            Payload = payload;
            Timestamp = timestamp;
            EventType = eventType;
        }
    }
}