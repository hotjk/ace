using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class Rule
    {
        public int Interval { get; set; }
        public IPeriod Period { get; set; }
        public IPredicate Predicate { get; set; }
        public int Times { get; set; }
        public int Repeats { get; set; }

        public TimeSpan HowLong()
        {
            return new TimeSpan(Period.HowLong(Interval).Ticks * Repeats);
        }
    }
}
