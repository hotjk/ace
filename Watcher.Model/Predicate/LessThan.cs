using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Model
{
    public class LessThan : IPredicate
    {
        public string Quantifier
        {
            get
            {
                return "less than";
            }
        }

        public bool IsSatisfiedBy(int a, int b)
        {
            return a < b;
        }
    }
}
