using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;
using Watcher.Model;
using System.Collections.Concurrent;

namespace Watcher.Foodie
{
    class Program
    {
        private static ConnectionMultiplexer redis;
        private static log4net.ILog logger;

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = log4net.LogManager.GetLogger("message.logger");
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
            logger.Info("Clean: " + list.Length);
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
    }
}
