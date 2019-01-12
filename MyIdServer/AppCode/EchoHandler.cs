using System;
using System.Text;
using Helios.Channels;
using Helios.Buffers;

namespace MyIdServer
{
    public class EchoHandler : ChannelHandlerAdapter
    {
        bool login = false;

        public override void ChannelActive(IChannelHandlerContext context)
        {
            LogHelper.DebugGreen("one client connet " + context.Channel.Id);
        }

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuf ibuff = message as IByteBuf;
            byte[] buff = ibuff.ToArray();
            if (!login)
            {
                if (string.IsNullOrEmpty(ConfigHelper.Password))
                {
                    login = true;
                    LogHelper.DebugGreen("login success use password empty");
                    context.WriteAndFlushAsync(Unpooled.WrappedBuffer(Encoding.UTF8.GetBytes("1")));
                }
                else
                {
                    string pwd = Encoding.UTF8.GetString(buff);
                    if (pwd.Equals(ConfigHelper.Password))
                    {
                        login = true;
                        LogHelper.DebugGreen("login success use password " + pwd);
                        context.WriteAndFlushAsync(Unpooled.WrappedBuffer(Encoding.UTF8.GetBytes("1")));
                    }
                    else //password error
                    {
                        context.WriteAndFlushAsync(Unpooled.WrappedBuffer(Encoding.UTF8.GetBytes("-1")));
                        LogHelper.DebugRed("login error use password " + pwd);
                    }
                }
                return;
            }

            int msg = BitConverter.ToInt32(buff, 0);
            string id;
            switch (msg)
            {
                case 0: id = Guid.NewGuid().ToString(); break;
                case 1: id = ObjectId.GenerateNewId().ToString(); break;
                case 2: id = SnowflakeId.idWorker.NextId().ToString(); break;
                case 3: id = Base36Id.Base16.NewId(); break;
                case 4: id = Base36Id.Base20.NewId(); break;
                case 5: id = Base36Id.Base25.NewId(); break;
                default: id = "-2"; break;
            }
            context.WriteAndFlushAsync(Unpooled.WrappedBuffer(Encoding.Default.GetBytes(id)));
            LogHelper.DebugGreen("send id " + id);

        }

        bool isClose = false;
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            if (!isClose)
            {
                LogHelper.DebugRed("one client close " + context.Channel.Id);
                isClose = true;
            }
        }
    }
}
