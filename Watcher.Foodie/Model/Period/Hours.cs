using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class Hours : IPeriod
    {
        private Hours() { }
        public static readonly Hours Instance = new Hours();

        public string Quantifier
        {
            get
            {
                return "hour(s)";
            }
        }

        public string Key(string @event)
        {
            return @event + "_h";
        }
        public string Field(DateTime dt)
        {
            return dt.ToString("yyyyMMddHH");
        }

        public IEnumerable<string> KeepFields(DateTime dt)
        {
            return new string[] { dt.AddDays(-1).ToString("yyyyMMdd"), dt.ToString("yyyyMMdd") };
        }

        public long HowLong(int n)
        {
            return new TimeSpan(n, 0, 0).Ticks;
        }

        public IEnumerable<string> PatrolFields(DateTime from, DateTime to)
        {
            for (DateTime dt = from; dt < to; dt = dt.AddHours(1))
            {
                yield return (Field(dt));
            }
        }
    }
}
