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
        //rabbitmqctl add_user event_user event_password
        //rabbitmqctl set_permissions -p grit event_user ".*" ".*" ".*"
        //rabbitmqctl set_permissions -p grit guest ".*" ".*" ".*"
        //rabbitmq-plugins enable rabbitmq_management
        //rabbitmqctl set_user_tags event_user administrator
        //RabbitMQ Management Web http://localhost:15672
        public static readonly string ACEQueueConnectionString = "host=localhost;virtualHost=grit;username=event_user;password=event_password;timeout=10;prefetchcount=50";
    }
}
