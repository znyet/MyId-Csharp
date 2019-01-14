using Helios.Buffers;
using Helios.Channels;
using Helios.Channels.Bootstrap;
using Helios.Channels.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var echoHandler = new EchoHandler();
            IEventLoopGroup clientGroup = new MultithreadEventLoopGroup(1);
            ClientBootstrap clientBootstrap = new ClientBootstrap()
                .Group(clientGroup)
                .Option(ChannelOption.TcpNodelay, true)
                .Channel<TcpSocketChannel>()
                .RemoteAddress(IPAddress.Parse("127.0.0.1"), 8123)
                .Handler(new ActionChannelInitializer<TcpSocketChannel>(channel =>
                {
                    IChannelPipeline pip = channel.Pipeline;
                    pip.AddLast(echoHandler);
                }));
            clientBootstrap.ConnectAsync();

            for (int i = 0; i < 5; i++)
            {
                IByteBuf buf = Unpooled.Buffer().WriteByte(1).WriteInt(1);
                var data = echoHandler.SendMessage(buf);
                Console.WriteLine(data);
            }

            Console.ReadKey();
        }
    }
}
