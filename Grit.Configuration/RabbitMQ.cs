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
        public static readonly string ACEQueueConnectionString = "host=localhost;virtualHost=grit;username=guest;password=guest;timeout=10;prefetchcount=50";
    }
}
