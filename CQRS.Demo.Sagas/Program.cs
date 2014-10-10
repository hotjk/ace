using CQRS.Demo.Contracts;
using CQRS.Demo.Contracts.Events;
using Grit.CQRS;
using Grit.CQRS.Actions;
using Grit.CQRS.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CQRS.Demo.Sagas
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            BootStrapper.BootStrap();

            ServiceLocator.ActionBus.HandleInParallel(20);
            //ServiceLocator.ActionBus.Handle();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
