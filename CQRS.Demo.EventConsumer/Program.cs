using Grit.CQRS;
using Grit.CQRS.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Demo.EventConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            BootStrapper.BootStrap();

            ServiceLocator.EventBus.HandleInParallel("Account", "account.*.*", 20);
            //ServiceLocator.EventBus.Handle("Account", "account.*.*");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
