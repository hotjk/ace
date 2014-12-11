using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.ACE.Events
{
    public enum EventPublishOptions
    {
        BalckHole = 0,
        CurrentThread = 1,
        ThreadPool = 2,
        Queue = 4,
    }
}
