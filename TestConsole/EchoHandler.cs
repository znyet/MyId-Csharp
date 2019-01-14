using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Helios.Channels;
using Helios.Buffers;
using System.Threading;

namespace TestConsole
{
    public class EchoHandler : ChannelHandlerAdapter
    {
        public IChannel channel;
        public string pwd = "1";
        public int msgTimeout=500; //消息超时时间
        
        StringBuilder sb = new StringBuilder();
        AutoResetEvent slim = new AutoResetEvent(false);

        //激活连接事件
        public override void ChannelActive(IChannelHandlerContext context)
        {
            channel = context.Channel;
            byte[] data = Encoding.Default.GetBytes(pwd);
            channel.WriteAndFlushAsync(Unpooled.WrappedBuffer(data)); // send login
        }

        //读取数据事件
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuf ibuff = message as IByteBuf;
            string data = Encoding.UTF8.GetString(ibuff.ToArray());
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
            slim.Reset();
            channel = null;
        }

        //发送消息
        public string SendMessage(IByteBuf ibuf)
        {
            if (channel == null)
            {
                sb.Clear();
                slim.WaitOne(msgTimeout);
                string loginData = sb.ToString();
                if (loginData == "-1")
                    return "1"; //password error
                else if (string.IsNullOrEmpty(loginData))
                    return "2"; //socket error 
            }

            sb.Clear();
            channel.WriteAndFlushAsync(ibuf);
            slim.WaitOne(msgTimeout);
            string data = sb.ToString();
            sb.Clear();
            if (string.IsNullOrEmpty(data))
                return "2";
            return data;
        }


    }
}
