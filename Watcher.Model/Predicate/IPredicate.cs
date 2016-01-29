using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watcher.Model
{
    public interface IPredicate
    {
        string Quantifier { get; }
        bool IsSatisfiedBy(int a, int b);
    }
}
