using ACE.Demo.Contracts;
using ACE.Demo.Contracts.Events;
using Grit.ACE;
using Grit.ACE.Actions;
using Grit.ACE.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Demo.MicroServices
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            BootStrapper.BootStrap();

            ServiceLocator.ActionBus.SubscribeInParallel(20);
            //ServiceLocator.ActionBus.Subscribe();

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
