using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Configuration
{
    public static class RabbitMQ
    {
        //rabbitmqctl add_vhost grit
        //rabbitmqctl set_permissions -p grit guest ".*" ".*" ".*"
        //rabbitmq-plugins enable rabbitmq_management
        //RabbitMQ Management Web http://localhost:15672
        //public static readonly string ACEQueueConnectionString = "host=localhost;virtualHost=grit;username=guest;password=guest;timeout=10;prefetchcount=10";
        //public static readonly string ACEQueueConnectionString = "host=192.168.3.134;virtualHost=grit;username=guest;password=guest;timeout=10;prefetchcount=50";
        public static readonly string ACEQueueConnectionString = "host=192.168.3.73;virtualHost=grit;username=user;password=user;timeout=10;prefetchcount=50";
        //public static readonly string ACEQueueConnectionString = "host=192.168.100.11;virtualHost=grit;username=user;password=user;timeout=10;prefetchcount=50";
    }
}
