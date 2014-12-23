using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts.Commands
{
    public class CreateAccount : Command
    {
        public int AccountId { get; set; }
    }
}
