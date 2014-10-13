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

namespace ACE.Demo.Sagas
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            BootStrapper.BootStrap();

            ServiceLocator.ActionBus.SubscribeInParallel(20);
            //ServiceLocator.ActionBus.Handle();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
