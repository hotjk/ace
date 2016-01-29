using StackExchange.Redis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Watcher.Model;

namespace Watcher.Inspector
{
    class Program
    {
        private static ConnectionMultiplexer redis;
        private static log4net.ILog logger;

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = log4net.LogManager.GetLogger("message.logger");
            redis = ConnectionMultiplexer.Connect(Grit.Configuration.Redis.Configuration);

            Task.Run(() => {
                while (true)
                {
                    logger.Info("Inspect");
                    var warnings = Patrol().Result;
                    foreach (var warning in warnings)
                    {
                        logger.Warn(warning);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            });

            Thread.Sleep(Timeout.Infinite);
        }

        private static readonly IEnumerable<IPeriod> _countPeriods = new List<IPeriod>() { Seconds.Instance, Minutes.Instance, Hours.Instance, Days.Instance };
        private static ConcurrentDictionary<string, DateTime> _waitingForClean = new ConcurrentDictionary<string, DateTime>();
        private static IList<Rule> _rules = new List<Rule>()
        {
             new Rule { Key ="ACE.Demo.Contracts.Events.InvestmentStatusCreated", Interval = 3, Period = Seconds.Instance, Predicate = new GreatThan(), Times = 258, Repeats = 2, Cooldown = TimeSpan.FromSeconds(10)  },
             new Rule { Key ="ACE.Demo.Contracts.Events.InvestmentStatusCompleted", Interval = 1, Period = Minutes.Instance, Predicate = new LessThan(), Times = 60, Repeats = 1, Cooldown = TimeSpan.FromMinutes(10)  }
        };

        private static async Task<IList<string>> Patrol()
        {
            IList<string> warnings = new List<string>();
            foreach (var name in _rules.Select(n => n.Key).Distinct())
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
                if (rules.Any())
                {
                    var to = period.RemoveLastPeriod(now);
                    var maxTicks = rules.Max(n => n.HowLong());
                    var from = to.AddTicks(0 - maxTicks);
                    var keys = period.PatrolFields(from, to).Select(n => (RedisValue)n).ToArray();
                    var redisValues = await db.HashGetAsync(period.Key(name), keys);
                    var values = redisValues.Select(n => n.HasValue ? int.Parse(n) : 0).ToList();
                    foreach (var rule in rules)
                    {
                        if (rule.IsSatisfiedBy(values) && rule.Fire(now))
                        {
                            warnings.Add(rule.WarningMessage());
                        }
                    }
                }
            }
        }
    }
}
