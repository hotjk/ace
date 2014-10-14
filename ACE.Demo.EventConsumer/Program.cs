using Grit.ACE;
using Grit.ACE.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Demo.EventConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            BootStrapper.BootStrap();

            ServiceLocator.EventBus.SubscribeInParallel("Account", "account.*.*", 20);
            //ServiceLocator.EventBus.Subscribe("Account", "account.*.*");
            
            Console.WriteLine("Ctrl-C to exit");
            Console.CancelKeyPress += (source, cancelKeyPressArgs) =>
            {
                Console.WriteLine("Shut down...");
                ServiceLocator.Dispose();
                Thread.Sleep(TimeSpan.FromSeconds(10));
                Console.WriteLine("Shut down complete");
            };

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
