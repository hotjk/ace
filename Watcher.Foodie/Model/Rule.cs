using Watcher.Foodie.Model;

public class Rule
{
    public int Interval { get; set; }
    public IPeriod Period { get; set; }
    public IPredicate Predicate { get; set; }
    public int Times { get; set; }
    public int Repeats { get; set; }

    public long HowLong()
    {
        return Period.HowLong(Interval) * Repeats;
    }
}