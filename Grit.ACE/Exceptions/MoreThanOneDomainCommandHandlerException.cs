using System;

namespace ACE
{
    public class MoreThanOneDomainCommandHandlerException : Exception
    {
        public MoreThanOneDomainCommandHandlerException(string message) : base(message) { }
    }
}
