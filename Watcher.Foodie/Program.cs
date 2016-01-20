using ACE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.Redis;

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

            BootStrapper.EventStation.SubscribeAsync("WatcherFoodie", new string[] { "#" }, OnMessage);

            Console.WriteLine("Ctrl-C to exit");
            Console.CancelKeyPress += (source, cancelKeyPressArgs) =>
            {
                Console.WriteLine("Shut down...");
                BootStrapper.Dispose();
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine("Shut down completed");
            };

            Thread.Sleep(Timeout.Infinite);
        }

        private static async Task OnMessage(IEvent @event)
        {
            string eventName = @event.ToString();
            IDatabase db = redis.GetDatabase();
            string s = DateTime.Now.ToString("yyyyMMddHHmmss");
            string m = DateTime.Now.ToString("yyyyMMddHHmm");
            string h = DateTime.Now.ToString("yyyyMMddHH");
            string d = DateTime.Now.ToString("yyyyMMdd");
            await db.HashIncrementAsync(eventName + "_s", s, 1);
            await db.HashIncrementAsync(eventName + "_m", m, 1);
            await db.HashIncrementAsync(eventName + "_h", h, 1);
            await db.HashIncrementAsync(eventName + "_d", d, 1);
        }
    }
}
