using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Sequence
{
    public class SequenceRange
    {
        public SequenceRange(int id)
        {
            this.Id = id;
            this.Current = 0;
            this.Last = 0;
        }

        public int Id { get; private set; }
        public int Current { get; private set; }
        public int Last { get; set; }

        public object LockObject = new object();

        public int Next(int step, ISequenceRepository repository)
        {
            lock (LockObject)
            {
                Current = Current + 1;
                if (Current < this.Last)
                {
                    return Current;
                }

                Current = repository.NextWithTransactionScope(this.Id, step);
                if(Current < Last)
                {
                    throw new ApplicationException("The next value from SequenceRepository should large than orignal sequence value.");
                }
                Last = Current + step;
                return Current;
            }
        }
    }
}
