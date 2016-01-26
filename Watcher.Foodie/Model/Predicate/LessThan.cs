using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Foodie.Model
{
    public class LessThan : IPredicate
    {
        public bool IsSatisfiedBy(int a, int b)
        {
            return a < b;
        }
    }
}
