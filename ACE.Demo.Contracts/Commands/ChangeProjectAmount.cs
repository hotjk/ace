using Grit.ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts.Commands
{
    public class ChangeProjectAmount : Command
    {
        public int ProjectId { get; set; }
        public decimal Change { get; set; }
    }
}
