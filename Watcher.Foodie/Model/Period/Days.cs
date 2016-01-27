using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class Days : IPeriod
    {
        private Days() { }
        public static readonly Days Instance = new Days();

        public string Key(string @event)
        {
            return @event + "_d";
        }
        public string Field(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        public IEnumerable<string> KeepFields(DateTime dt)
        {
            return new string[] { dt.AddMinutes(-1).ToString("yyyyMM"), dt.ToString("yyyyMM") };
        }

        public long HowLong(int n)
        {
            return new TimeSpan(n, 0, 0, 0).Ticks;
        }

        public IEnumerable<string> PatrolFields(DateTime from, DateTime to)
        {
            for (DateTime dt = from; dt < to; dt = dt.AddDays(1))
            {
                yield return (Field(dt));
            }
        }
    }
}
