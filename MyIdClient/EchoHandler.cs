using System;
using System.Text;
using Helios.Channels;
using Helios.Buffers;
using System.Threading;

namespace MyIdClient
{
    public class EchoHandler : ChannelHandlerAdapter
    {
        public IChannel channel;
        public StringBuilder sb = new StringBuilder();
        public AutoResetEvent slim = new AutoResetEvent(false);
        public int lifetime;
        Timer timer = null;
        DateTime lastTime = DateTime.Now;
        //激活连接事件
        public override void ChannelActive(IChannelHandlerContext context)
        {
            if (lifetime != 0)
            {
                timer = new Timer(delegate
                {
                    TimeSpan ts = DateTime.Now - lastTime;
                    if (ts > TimeSpan.FromSeconds(lifetime))
                    {
                        if (channel != null)
                        {
                            channel.CloseAsync();
                        }
                    }

                }, null, lifetime * 1000, 2000);
            }
        }

        //读取数据事件
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuf ibuff = message as IByteBuf;
            string data = Encoding.Default.GetString(ibuff.ToArray());
            sb.Append(data);
        }

        //数据读取完毕事件
        public override void ChannelReadComplete(IChannelHandlerContext context)
        {
            lastTime = DateTime.Now;
            context.Flush();
            slim.Set();
        }

        ////异常事件
        //public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        //{

        //}

        //关闭连接
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            channel = null;
            slim.Reset();
            if (timer != null)
                timer.Dispose();
        }

    }
}
