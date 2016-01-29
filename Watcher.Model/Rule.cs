using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Watcher.Model
{

    public class Rule
    {
        public string Key { get; set; }
        public int Interval { get; set; }
        public IPeriod Period { get; set; }
        public IPredicate Predicate { get; set; }
        public int Times { get; set; }
        public int Repeats { get; set; }
        public TimeSpan Cooldown { get; set; }

        public DateTime? _lastFireAt;
        public bool Fire(DateTime dt)
        {
            if(_lastFireAt == null)
            {
                _lastFireAt = dt;
                return false; // Warm up
            }
            if (dt - _lastFireAt > Cooldown)
            {
                _lastFireAt = dt;
                return true;
            }
            return false; // Cool down
        }

        public long HowLong()
        {
            return Period.HowLong(Interval) * Repeats;
        }

        public bool IsSatisfiedBy(IEnumerable<int> values)
        {
            int counter = 0;
            Queue<int> valueQueue = new Queue<int>(Interval);
            foreach (var value in values)
            {
                valueQueue.Enqueue(value);
                if (valueQueue.Count == Interval)
                {
                    if (Predicate.IsSatisfiedBy(valueQueue.Sum(), Times))
                    {
                        valueQueue.Clear();
                        counter++;
                        if (counter >= Repeats)
                        {
                            return true;
                        }
                        continue;
                    }
                    counter = 0;
                    valueQueue.Dequeue();
                }
            }
            return false;
        }

        public string WarningMessage()
        {
            return string.Format("{0} event {1} {2} within {3} {4} repeat {5} times.",
                Key, Predicate.Quantifier, Times, Interval, Period.Quantifier, Repeats);
        }
    }
}