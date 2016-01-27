using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class Minutes : IPeriod
    {
        private Minutes() { }
        public static readonly Minutes Instance = new Minutes();

        public string Quantifier
        {
            get
            {
                return "minute(s)";
            }
        }

        public string Key(string @event)
        {
            return @event + "_m";
        }
        public string Field(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmm");
        }

        public IEnumerable<string> KeepFields(DateTime dt)
        {
            return new string[] { dt.AddHours(-1).ToString("yyyyMMddHH"), dt.ToString("yyyyMMddHH") };
        }

        public long HowLong(int n)
        {
            return new TimeSpan(0, n, 0).Ticks;
        }

        public IEnumerable<string> PatrolFields(DateTime from, DateTime to)
        {
            for (DateTime dt = from; dt < to; dt = dt.AddMinutes(1))
            {
                yield return (Field(dt));
            }
        }
    }
}
