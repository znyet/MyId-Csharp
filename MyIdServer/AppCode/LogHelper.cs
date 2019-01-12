using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;

namespace MyIdServer
{
    internal class LogHelper
    {
        private static Logger debug = LogManager.GetLogger("debug");
        private static Logger info = LogManager.GetLogger("info");
        private static Logger error = LogManager.GetLogger("error");

        public static void Debug(string msg, params object[] args)
        {
            if (ConfigHelper.Debug)
                debug.Debug(msg, args);
        }

        public static void Info(string msg, params object[] args)
        {
            if (ConfigHelper.Debug)
                debug.Info(msg, args);
            else
                info.Info(msg, args);
        }

        public static void Error(string msg, params object[] args)
        {
            if (ConfigHelper.Debug)
                debug.Error(msg, args);
            else
                error.Error(msg, args);
        }

        //public static void Fatal(string msg, params object[] args)
        //{
        //    logger.Fatal(msg, args);
        //}

        //public static void Trace(string msg, params object[] args)
        //{
        //    logger.Trace(msg, args);
        //}

    }
}
