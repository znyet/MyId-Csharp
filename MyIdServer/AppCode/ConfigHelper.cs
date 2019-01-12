using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpConfig;
using System.Net;
using Snowflake;
using Funcular.IdGenerators;

namespace MyIdServer
{
    internal class ConfigHelper
    {
        public static IPAddress Ip;
        public static int Port;
        public static string Password;
        public static string MachineName;
        public static int WorkerId;
        public static int DatacenterId;

        public static bool Debug;

        public static void ReadConfig()
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\Config.ini";
            var config = Configuration.LoadFromFile(file);
            var section = config["config"];
            string ip = section["Ip"].StringValue;
            if (string.IsNullOrEmpty(ip))
                Ip = IPAddress.Any;
            else
                Ip = IPAddress.Parse(ip);

            Port = section["Port"].IntValue;
            Password = section["Password"].StringValue;
            Debug = section["Debug"].BoolValue;

        }
    }
}
