using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class Hours : IPeriod
    {
        public string Key(string @event)
        {
            return @event + "_h";
        }
        public string Field(DateTime dt)
        {
            return dt.ToString("yyyyMMddHH");
        }

        public IEnumerable<string> KeepKeys(DateTime dt)
        {
            return new string[] { dt.AddDays(-1).ToString("yyyyMMdd"), dt.ToString("yyyyMMdd") };
        }

        public TimeSpan HowLong(int n)
        {
            return new TimeSpan(n, 0, 0);
        }
    }
}
