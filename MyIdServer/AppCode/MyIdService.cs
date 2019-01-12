using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

using Helios.Channels;
using Helios.Channels.Bootstrap;
using Helios.Channels.Sockets;
using System.Runtime.InteropServices;
using System.Threading;

namespace MyIdServer
{
    internal class MyIdService : ServiceControl
    {
        #region GC

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        #endregion

        static IEventLoopGroup serverGroup = new MultithreadEventLoopGroup(Environment.ProcessorCount / 2);
        static IEventLoopGroup workerGroup = new MultithreadEventLoopGroup(1);
        static IChannel serverChannel;
        public bool Start(HostControl hostControl)
        {

            ConfigHelper.ReadConfig();
            ServerBootstrap serverBootstrap = new ServerBootstrap()
               .Group(serverGroup, workerGroup)
               .Channel<TcpServerSocketChannel>()
               .ChildOption(ChannelOption.TcpNodelay, true)
               .LocalAddress(ConfigHelper.Ip, ConfigHelper.Port)
               .ChildHandler(new ActionChannelInitializer<TcpSocketChannel>(channel =>
               {
                   IChannelPipeline pip = channel.Pipeline;
                   pip.AddLast(new EchoHandler());
               }));
            try
            {
                serverChannel = serverBootstrap.BindAsync().Result;
                Console.WriteLine("--> server start,listen in port:" + ConfigHelper.Port);
                Console.WriteLine("--> Run Debug=" + ConfigHelper.Debug);
                Console.WriteLine("--> if you want to debug edit Config.ini set Debug=True");
                Console.WriteLine("--> in production environment Debug must be False");
                Console.WriteLine("--> now waitting for client connect...");

                LogHelper.Info("server start");
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message + ex.StackTrace);
            }

            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromMinutes(ConfigHelper.GCTime));
                    ClearMemory();
                }
            }) { IsBackground = true }.Start();

            return true;


        }

        public bool Stop(HostControl hostControl)
        {
            LogHelper.Info("server stop");
            return true;
        }
    }
}
