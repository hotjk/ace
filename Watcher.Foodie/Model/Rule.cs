using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Watcher.Foodie.Model;

public class Rule
{
    public string Key { get; set; }
    public int Interval { get; set; }
    public IPeriod Period { get; set; }
    public IPredicate Predicate { get; set; }
    public int Times { get; set; }
    public int Repeats { get; set; }

    public long HowLong()
    {
        return Period.HowLong(Interval) * Repeats;
    }

    public bool IsSatisfiedBy(IEnumerable<int> values)
    {
        int counter = 0;
        foreach(var value in values)
        {
            if(Predicate.IsSatisfiedBy(value, Times))
            {
                counter++;
                if (counter >= Repeats)
                {
                    return true;
                }
            }
            else
            {
                counter = 0;
            }
        }
        return false;
    }
}