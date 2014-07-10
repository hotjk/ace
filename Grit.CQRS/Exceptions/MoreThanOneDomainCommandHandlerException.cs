using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.CQRS
{
    public class MoreThanOneDomainCommandHandlerException : Exception
    {
        public MoreThanOneDomainCommandHandlerException(string message) : base(message) { }
    }
}
