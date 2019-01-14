using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Helios.Channels;
using Helios.Buffers;
using System.Threading;

namespace MyIdClient
{
    public class EchoHandler : ChannelHandlerAdapter
    {
        public IChannel channel;
        public string pwd;
        public int msgTimeout; //消息超时时间
        public StringBuilder sb = new StringBuilder();
        public AutoResetEvent slim = new AutoResetEvent(false);
        //激活连接事件
        public override void ChannelActive(IChannelHandlerContext context)
        {
            //channel = context.Channel;
            //byte[] data = Encoding.Default.GetBytes(pwd);
            //channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(data)); // send login
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
            slim.Set();
        }

        //异常事件
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {

        }

        //关闭连接
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            channel = null;
            slim.Reset();
        }

    }
}
