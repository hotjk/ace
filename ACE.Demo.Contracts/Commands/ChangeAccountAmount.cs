using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Demo.Contracts.Commands
{
    public class ChangeAccountAmount : ICommand
    {
        public int AccountId { get; set; }
        public decimal Change { get; set; }
    }
}
