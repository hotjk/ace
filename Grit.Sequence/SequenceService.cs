using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Sequence
{
    public class SequenceService : ISequenceService
    {
        public SequenceService(ISequenceRepository SequenceRepository)
        {
            this.SequenceRepository = SequenceRepository;
        }

        public static readonly int DEFAULT_STEP = 100;
        public static readonly int MIN_STEP = 1;
        public static readonly int MAX_STEP = 100000;

        private static object LockObject = new object();
        private static IDictionary<int, SequenceRange> cache = new Dictionary<int, SequenceRange>();
        private ISequenceRepository SequenceRepository { get; set; }

        public int Next(int sequence, int step)
        {
            if (step < MIN_STEP || step > MAX_STEP)
            {
                throw new ArgumentOutOfRangeException("step", step,
                    string.Format("should less between {0} and {1}.",
                    MIN_STEP, MAX_STEP));
            }

            SequenceRange range;
            if (!cache.TryGetValue(sequence, out range))
            {
                lock (LockObject)
                {
                    if (!cache.TryGetValue(sequence, out range))
                    {
                        range = new SequenceRange(sequence);
                        cache[sequence] = range;
                    }
                }
            }
            return range.Next(step, this.SequenceRepository);
        }
    }
}
