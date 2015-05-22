using ACE.Demo.Contracts.Events;
using ACE.Demo.EventConsumer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Demo.EventProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            BootStrapper.BootStrap();

            InvestmentStatusCreated investmentStatusCreated = new InvestmentStatusCreated(3,1,4,2);
            for (int i = 0; i < 3600*100; i++)
            {
                BootStrapper.EventBus.Publish(investmentStatusCreated);
                BootStrapper.EventBus.Flush();
                if (i % 10000 == 0)
                {
                    Console.WriteLine(i);
                }
                Thread.Sleep(10);
            }
        }
    }
}
