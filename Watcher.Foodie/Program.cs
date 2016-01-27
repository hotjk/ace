using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using Watcher.Foodie.Model;
using System.Collections.Concurrent;

namespace Watcher.Foodie
{
    class Program
    {
        private static ConnectionMultiplexer redis;
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            BootStrapper.BootStrap();
            redis = ConnectionMultiplexer.Connect(Grit.Configuration.Redis.Configuration);

            BootStrapper.EventStation.SubscribeAsync("WatcherFoodie", new string[] { "#" }, x => Increase(x.ToString()), true, true);

            Console.WriteLine("Ctrl-C to exit");
            Console.CancelKeyPress += (source, cancelKeyPressArgs) =>
            {
                Console.WriteLine("Shut down...");
                BootStrapper.Dispose();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine("Shut down completed");
            };

            Task.Run(() => {
                while (true)
                {
                    Console.WriteLine("Patrol");
                    var warnings = Patrol().Result;
                    foreach(var warning in warnings)
                    {
                        Console.WriteLine(warning);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    CleanAll().Wait();
                    Thread.Sleep(TimeSpan.FromSeconds(30));
                }
            });

            Thread.Sleep(Timeout.Infinite);
        }

        private static readonly IEnumerable<IPeriod> _countPeriods = new List<IPeriod>() { Seconds.Instance, Minutes.Instance, Hours.Instance, Days.Instance };
        private static ConcurrentDictionary<string, DateTime> _waitingForClean = new ConcurrentDictionary<string, DateTime>();
        private static IList<Rule> _rules = new List<Rule>()
        {
             new Rule { Key ="ACE.Demo.Contracts.Events.InvestmentStatusCreated", Interval = 3, Period = Seconds.Instance, Predicate = new GreatThan(), Times = 258, Repeats = 2, Cooldown = TimeSpan.FromSeconds(10)  },
             new Rule { Key ="ACE.Demo.Contracts.Events.InvestmentStatusCompleted", Interval = 1, Period = Minutes.Instance, Predicate = new LessThan(), Times = 10, Repeats = 2, Cooldown = TimeSpan.FromSeconds(10)  }
        };

        private static async Task Increase(string name)
        {
            DateTime now = DateTime.Now;
            IDatabase db = redis.GetDatabase();
            foreach (var period in _countPeriods)
            {
                await db.HashIncrementAsync(period.Key(name), period.Field(now), 1);
            }
            _waitingForClean.TryAdd(name, now);
        }

        private static async Task CleanAll()
        {
            var list = _waitingForClean.ToArray();
            _waitingForClean.Clear();
            foreach (var item in list)
            {
                await Clean(item.Key);
            }
            Console.WriteLine("Clean: " + list.Length);
        }

        private static async Task Clean(string name)
        {
            DateTime now = DateTime.Now;
            IDatabase db = redis.GetDatabase();
            foreach (var period in _countPeriods)
            {
                var keys = await db.HashKeysAsync(period.Key(name));
                var keeps = period.KeepFields(now);
                var removed = keys
                    .Select(n => n.ToString())
                    .Where(n => !keeps.Any(k => n.StartsWith(k)))
                    .Select(n => (RedisValue)n)
                    .ToArray();
                await db.HashDeleteAsync(period.Key(name), removed);
            }
        }

        private static async Task<IList<string>> Patrol()
        {
            IList<string> warnings = new List<string>();
            foreach(var name in _rules.Select(n=>n.Key).Distinct())
            {
                await Inspect(name, warnings);
            }
            return warnings;
        }

        private static async Task Inspect(string name, IList<string> warnings)
        {
            DateTime now = DateTime.Now;
            IDatabase db = redis.GetDatabase();
            foreach (var period in _countPeriods)
            {
                var rules = _rules.Where(n => n.Period == period);
                if(rules.Any())
                {
                    var maxTicks = rules.Max(n => n.HowLong());
                    var from = now.AddTicks(0 - maxTicks);
                    var keys = period.PatrolFields(from, now).Select(n => (RedisValue)n).ToArray();
                    var redisValues = await db.HashGetAsync(period.Key(name), keys);
                    var values = redisValues.Select(n=>n.HasValue? int.Parse(n):0).ToList();
                    //Console.WriteLine(string.Join(", ", keys.Zip(values, (x, y) => x + ":" + y)));
                    foreach (var rule in rules)
                    {
                        if(rule.IsSatisfiedBy(values) && rule.Fire(now))
                        {
                            warnings.Add(rule.WarningMessage());
                        }
                    }
                }
            }
        }
    }
}
