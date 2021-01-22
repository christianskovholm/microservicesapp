using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OrganizationService.Domain.SeedWork;
using OrganizationService.Infrastructure;

namespace OrganizationService.Application.Factories
{
    /// <summary>
    /// Factory for creating events from domain events. 
    /// </summary>
    public static class EventFactory
    {
        /// <summary>
        /// ContractResolver used for skipping properties in serialization.
        /// </summary>
        class EventFactoryContractResolver : CamelCasePropertyNamesContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                if (member.Name == "DomainObject" || member.Name == "Timestamp")
                    return null;

                return base.CreateProperty(member, memberSerialization);
            }
        }

        /// <summary>
        /// Serializer used for converting a DomainObject to a JObject.
        /// </summary>
        public static JsonSerializer JsonSerializer = new JsonSerializer
        {
            ContractResolver = new EventFactoryContractResolver(),
            Formatting = Formatting.None
        };

        /// <summary>
        /// Creates an Event from the specified domain event.
        /// </summary>
        /// <returns>Event object representation of a domain object.</returns>
        public static Event Create(DomainEvent domainEvent, int organizationId)
        {
            if (domainEvent == null) 
                throw new ArgumentNullException(nameof(domainEvent));

            if (organizationId == 0)
                throw new ArgumentException("organizationId cannot be 0");

            var payload = JObject.FromObject(domainEvent, JsonSerializer);
            payload["id"] = domainEvent.DomainObject.Id;

            var eventType = domainEvent.GetType().Name.Split("DomainEvent")[0];
            var words = Regex.Split(eventType, @"(?<!^)(?=[A-Z])");
            eventType = string.Join("_", words).ToLower();

            var serializedPayload = payload.ToString(Formatting.None);
            var e = new Event(organizationId, serializedPayload, domainEvent.Timestamp.UtcDateTime, eventType);

            return e;
        }
    }
}