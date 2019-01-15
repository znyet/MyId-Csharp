using System;
using SharpConfig;
using System.Net;
using Snowflake;

namespace MyIdServer
{
    internal class ConfigHelper
    {
        public static IPAddress Ip;
        public static int Port;
        public static string Password;

        public static int WorkerId;
        public static int DatacenterId;

        public static long GCTime;
        public static bool Debug;

        public static void ReadConfig()
        {
            var file = System.AppDomain.CurrentDomain.BaseDirectory + "\\Config.ini";
            var config = Configuration.LoadFromFile(file);

            //server
            var section1 = config["server"];
            string ip = section1["Ip"].StringValue;
            if (string.IsNullOrEmpty(ip))
                Ip = IPAddress.Any;
            else
                Ip = IPAddress.Parse(ip);
            Port = section1["Port"].IntValue;
            Password = section1["Password"].StringValue;


            //snowflake
            var section3 = config["snowflake"];
            WorkerId = section3["WorkerId"].IntValue;
            DatacenterId = section3["DatacenterId"].IntValue;
            SnowflakeId.idWorker = new IdWorker(WorkerId, DatacenterId);

            //other
            var section4 = config["other"];
            GCTime = section4["GCTime"].IntValue;
            Debug = section4["Debug"].BoolValue;

        }
    }
}
