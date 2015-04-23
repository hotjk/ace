using System;

namespace ACE.Exceptions
{
    public class BusinessException : Exception
    {
        public int Code { get; private set; }
        public BusinessException(int code, string message) : base(message) 
        {
            this.Code = code;
        }
    }
}
