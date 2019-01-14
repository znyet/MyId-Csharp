using CodeProject.ObjectPool;
using Helios.Buffers;
using Helios.Channels;
using Helios.Channels.Bootstrap;
using Helios.Channels.Sockets;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace MyIdClient
{
    internal class MyIdPooled : PooledObject
    {
        string server;
        int port;
        string pwd;
        int msgTimeout;
        EchoHandler echoHandler;
        IEventLoopGroup clientGroup = new MultithreadEventLoopGroup(1);
        ClientBootstrap clientBootstrap;
        public MyIdPooled(string server, int port, string pwd, int msgTimeout)
        {
            this.server = server;
            this.port = port;
            this.msgTimeout = msgTimeout;
            if (string.IsNullOrEmpty(pwd))
                pwd = "1";
            this.pwd = pwd;
            echoHandler = new EchoHandler();
        }

        #region Method

        private void InitSocket()
        {
            echoHandler.pwd = pwd;
            echoHandler.msgTimeout = 500;

            new Thread(() => //new thread to run socket
            {
                clientBootstrap = new ClientBootstrap()
                 .Group(clientGroup)
                 .Option(ChannelOption.TcpNodelay, true)
                 .Channel<TcpSocketChannel>()
                 .RemoteAddress(IPAddress.Parse(server), port)
                 .Handler(new ActionChannelInitializer<TcpSocketChannel>(channel =>
                 {
                     IChannelPipeline pip = channel.Pipeline;
                     pip.AddLast(echoHandler);
                 }));

                clientBootstrap.ConnectAsync().ContinueWith(task =>
                {
                    //if connect success
                    echoHandler.channel = task.Result;
                    echoHandler.slim.Set();
                });

            }) { IsBackground = true }.Start();

            echoHandler.slim.WaitOne(1000);
            if (echoHandler.channel == null)
            {
                throw new Exception("MyIdServer Socket error");
            }
            else //send login
            {
                echoHandler.sb.Clear();
                byte[] byteArry = Encoding.Default.GetBytes(pwd);
                echoHandler.channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(byteArry));
                echoHandler.slim.WaitOne(1000);
                if (echoHandler.sb.ToString() == "-1")
                {
                    echoHandler.channel.CloseAsync();
                    echoHandler.channel = null;
                    throw new Exception("MyIdServer password error");
                }
            }
        }

        #endregion

        public string GetId(int idType, int count = 1)
        {
            if (echoHandler.channel == null)
                InitSocket();

            echoHandler.sb.Clear();
            IByteBuf buf = Unpooled.Buffer().WriteByte(idType).WriteInt(count);
            echoHandler.channel.WriteAndFlushAsync(buf);
            echoHandler.slim.WaitOne(1000);
            return echoHandler.sb.ToString();
        }


    }
}
