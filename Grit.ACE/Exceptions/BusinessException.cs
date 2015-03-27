using System;

namespace ACE.Exceptions
{
    public class BusinessException : Exception
    {
        public int Type { get; private set; }
        public BusinessException(int type, string message) : base(message) 
        {
            this.Type = type;
        }
    }
}
