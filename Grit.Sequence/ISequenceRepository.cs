using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Sequence
{
    public interface ISequenceRepository
    {
        int Next(int id, int step);
        int NextWithTransactionScope(int id, int step);
    }
}
