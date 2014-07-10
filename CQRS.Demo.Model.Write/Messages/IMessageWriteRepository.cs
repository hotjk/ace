using CQRS.Demo.Model.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Demo.Model.Write.Messages
{
    public interface IMessageWriteRepository
    {
        bool Add(Message message);
    }
}
