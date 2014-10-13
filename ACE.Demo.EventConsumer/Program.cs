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
            //ServiceLocator.EventBus.Handle("Account", "account.*.*");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
