using System;
using System.Text;
using Helios.Channels;
using Helios.Buffers;

namespace MyIdServer
{
    public class EchoHandler : ChannelHandlerAdapter
    {

        //socket connect
        public override void ChannelActive(IChannelHandlerContext context)
        {
            LogHelper.DebugGreen("one client connet " + context.Channel.Id);
        }

        //socket close
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            LogHelper.DebugRed("one client close ChannelInactive " + context.Channel.Id);
        }

        //socket message
        bool login = false;
        StringBuilder sb = new StringBuilder();
        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            IByteBuf ibuff = message as IByteBuf;
            if (!login)
            {
                if (string.IsNullOrEmpty(ConfigHelper.Password))
                {
                    login = true;
                    LogHelper.DebugGreen("login success server password empty");
                    context.WriteAndFlushAsync(Unpooled.WrappedBuffer(Encoding.Default.GetBytes("1")));
                }
                else
                {
                    string pwd = Encoding.Default.GetString(ibuff.ToArray());
                    if (pwd.Equals(ConfigHelper.Password))
                    {
                        login = true;
                        LogHelper.DebugGreen("login success client password " + pwd);
                        context.WriteAndFlushAsync(Unpooled.WrappedBuffer(Encoding.Default.GetBytes("1")));
                    }
                    else //password error
                    {
                        context.WriteAndFlushAsync(Unpooled.WrappedBuffer(Encoding.Default.GetBytes("-1")));
                        LogHelper.DebugRed("login error client password " + pwd + " <> server password " + ConfigHelper.Password);
                    }
                }
                return;
            }

            int idType = ibuff.GetByte(0); //请求类型
            int count = ibuff.GetInt(1); //请求个数
            string idString = null;
            if (idType == 0)
            {
                if (count == 1)
                    idString = Guid.NewGuid().ToString();
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        sb.Append(Guid.NewGuid().ToString());
                        if (i != count - 1)
                            sb.Append(",");
                    }
                    idString = sb.ToString();
                    sb.Clear();
                }
            }
            else if (idType == 1)
            {
                if (count == 1)
                    idString = ObjectId.GenerateNewId().ToString();
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        sb.Append(ObjectId.GenerateNewId().ToString());
                        if (i != count - 1)
                            sb.Append(",");
                    }
                    idString = sb.ToString();
                    sb.Clear();
                }
            }
            else if (idType == 2)
            {
                if (count == 1)
                    idString = SnowflakeId.idWorker.NextId().ToString();
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        sb.Append(SnowflakeId.idWorker.NextId().ToString());
                        if (i != count - 1)
                            sb.Append(",");
                    }
                    idString = sb.ToString();
                    sb.Clear();
                }
            }
            else if (idType == 3)
            {
                if (count == 1)
                    idString = Base36Id.Base16.NewId();
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        sb.Append(Base36Id.Base16.NewId());
                        if (i != count - 1)
                            sb.Append(",");
                    }
                    idString = sb.ToString();
                    sb.Clear();
                }
            }
            else if (idType == 4)
            {
                if (count == 1)
                    idString = Base36Id.Base20.NewId();
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        sb.Append(Base36Id.Base20.NewId());
                        if (i != count - 1)
                            sb.Append(",");
                    }
                    idString = sb.ToString();
                    sb.Clear();
                }
            }
            else if (idType == 5)
            {
                if (count == 1)
                    idString = Base36Id.Base25.NewId();
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        sb.Append(Base36Id.Base25.NewId());
                        if (i != count - 1)
                            sb.Append(",");
                    }
                    idString = sb.ToString();
                    sb.Clear();
                }
            }
            else if (idType == 6)
            {
                if (count == 1)
                    idString = Guid.NewGuid().ToString("N");
                else
                {
                    for (int i = 0; i < count; i++)
                    {
                        sb.Append(Guid.NewGuid().ToString("N"));
                        if (i != count - 1)
                            sb.Append(",");
                    }
                    idString = sb.ToString();
                    sb.Clear();
                }
            }
            else
            {
                idString = "0";
            }
            context.WriteAndFlushAsync(Unpooled.WrappedBuffer(Encoding.Default.GetBytes(idString)));
            LogHelper.DebugGreen("send id " + idString);

        }

        //socket exception
        bool isClose = false;
        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            if (!isClose)
            {
                LogHelper.DebugRed("ExceptionCaught " + context.Channel.Id + " " + exception.Message);
                isClose = true;
            }
        }
    }
}
