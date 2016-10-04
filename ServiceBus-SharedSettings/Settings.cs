using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBus_SharedSettings
{
    public class Settings
    {
        public static string ConnectionString = "";
        public static string QueueName = "sqlrelaymessages";
        public static string TopicName = "relay2016topic";
        public static bool SendSync = true;
        public static bool DoDbWrite = true;
    }
}
