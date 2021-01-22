using System;

namespace Domain.Exceptions
{
    /// <summary>
    /// Represents an error in retrieval of a domain object.
    /// </summary>
    public class DomainObjectNotFoundException : DomainException
    {
        public DomainObjectNotFoundException()
        {
            
        }

        public DomainObjectNotFoundException(string message) : base(message)
        {

        }

        public DomainObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}