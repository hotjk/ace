using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public interface IPeriod
    {
        string Key(string @event);
        string Field(DateTime dt);
        IEnumerable<string> KeepKeys(DateTime dt);
        TimeSpan HowLong(int n);
    }
}
