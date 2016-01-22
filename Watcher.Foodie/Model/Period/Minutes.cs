using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class Minutes : IPeriod
    {
        public string Key(string @event)
        {
            return @event + "_m";
        }
        public string Field(DateTime dt)
        {
            return dt.ToString("yyyyMMddHHmm");
        }

        public IEnumerable<string> KeepKeys(DateTime dt)
        {
            return new string[] { dt.AddHours(-1).ToString("yyyyMMddHH"), dt.ToString("yyyyMMddHH") };
        }

        public TimeSpan HowLong(int n)
        {
            return new TimeSpan(0, n, 0);
        }
    }
}
