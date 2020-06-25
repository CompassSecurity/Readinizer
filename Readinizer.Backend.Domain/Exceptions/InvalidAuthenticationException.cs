using System;

namespace Readinizer.Backend.Domain.Exceptions
{
    public class InvalidAuthenticationException : Exception
    {
        public string Details { get; set; } 

        public InvalidAuthenticationException(string message, string details = null) : base(message)
        {
            Details = details;
        }
    }
}
