using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class Seconds : IPeriod
    {
        public string Key(string @event)
        {
            return @event + "_s";
        }
        public string Field(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmmss");
        }

        public IEnumerable<string> KeepKeys(DateTime dt)
        {
            return new string[] { dt.AddMinutes(-1).ToString("yyyyMMddHHmm"), dt.ToString("yyyyMMddHHmm") };
        }

        public TimeSpan HowLong(int n)
        {
            return new TimeSpan(0, 0, n);
        }
    }
}
