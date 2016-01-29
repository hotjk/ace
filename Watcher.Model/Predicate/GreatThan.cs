using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Model
{
    public class GreatThan : IPredicate
    {
        public string Quantifier
        {
            get
            {
                return "great than";
            }
        }

        public bool IsSatisfiedBy(int a, int b)
        {
            return a > b;
        }
    }
}
