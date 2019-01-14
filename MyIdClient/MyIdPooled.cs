using CodeProject.ObjectPool;
using Helios.Buffers;
using Helios.Channels;
using Helios.Channels.Bootstrap;
using Helios.Channels.Sockets;
using System;
using System.Net;

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
                echoHandler.slim.Set();
            });

            echoHandler.slim.WaitOne(5000);

        }

        #endregion

        public string GetId(int idType, int count = 1)
        {
            if (echoHandler.channel == null)
                InitSocket();

            IByteBuf buf = Unpooled.Buffer().WriteByte(idType).WriteInt(count);
            string data = echoHandler.SendMessage(buf);
            //switch (data)
            //{
            //    case "1": throw new Exception("MyIdServer login time out");
            //    case "2": throw new Exception("MyIdServer socket error");
            //}
            return data;
        }


    }
}
