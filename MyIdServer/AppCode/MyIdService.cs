using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Topshelf;

namespace MyIdServer
{
    internal class MyIdService : ServiceControl
    {

        public bool Start(HostControl hostControl)
        {
            ConfigHelper.ReadConfig();

            LogHelper.Info("程序启动");
            LogHelper.Debug("有客户端连接");
            LogHelper.Error("发生错误");
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            LogHelper.Info("程序停止");
            return true;
        }
    }
}
