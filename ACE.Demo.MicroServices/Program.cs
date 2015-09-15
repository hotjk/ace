using ACE.Demo.Contracts;
using ACE.Demo.Contracts.Events;
using ACE;
using ACE.Exceptions;
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

            BootStrapper.ActionStation.SubscribeInParallel<ACE.Demo.Contracts.Actions.InvestmentActionBase>(10);

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
    }
}
