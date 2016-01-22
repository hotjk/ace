using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class Days : IPeriod
    {
        public string Key(string @event)
        {
            return @event + "_d";
        }
        public string Field(DateTime dt)
        {
            return dt.ToString("yyyyMMdd");
        }

        public IEnumerable<string> KeepKeys(DateTime dt)
        {
            return new string[] { dt.AddMinutes(-1).ToString("yyyyMM"), dt.ToString("yyyyMM") };
        }

        public TimeSpan HowLong(int n)
        {
            return new TimeSpan(n, 0, 0, 0);
        }
    }
}
