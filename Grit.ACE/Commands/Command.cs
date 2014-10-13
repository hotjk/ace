using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE
{
    public class Command : DomainMessage, ICommand
    {
        public Command()
        {
            CommandId = Guid.NewGuid();
        }
        public override Guid Id
        {
            get
            {
                return CommandId;
            }
        }
        public Guid ActionId { get; set; }
        public Guid CommandId { get; set; }
    }
}
