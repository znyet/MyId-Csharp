using CodeProject.ObjectPool;
using Helios.Net;
using Helios.Net.Bootstrap;
using Helios.Serialization;
using Helios.Topology;
using System;
using System.Net;
using System.Text;
using System.Linq;

namespace MyIdClient
{
    internal class MyIdPooled : PooledObject
    {
        string server;
        int port;
        string pwd;
        int msgTimeout;
        INode serverNode;
        IConnectionFactory connectionFactory;
        IConnection Client;
        StringBuilder sb = new StringBuilder();
        public MyIdPooled(string server, int port, string pwd, int msgTimeout)
        {
            if (string.IsNullOrEmpty(pwd))
                pwd = "1";

            this.server = server;
            this.port = port;
            this.pwd = pwd;
            this.msgTimeout = msgTimeout;
            serverNode = NodeBuilder.BuildNode().Host(IPAddress.Parse(server)).WithPort(port);

            connectionFactory = new ClientBootstrap()
                .SetEncoder(new NoOpEncoder())
                .SetDecoder(new NoOpDecoder())
                .SetTransport(TransportType.Tcp).Build();

            InitSocket();
        }


        #region MyRegion

        private void InitSocket()
        {
            if (Client == null || !Client.IsOpen())
            {

                Client = connectionFactory.NewConnection(serverNode);
               
                Client.OnConnection += (remoteAddress, responseChannel) =>
                {
                    Client.BeginReceive();
                    byte[] data = Encoding.Default.GetBytes(pwd); //send login
                    Client.Send(data, 0, data.Length, serverNode);
                };

                Client.Receive += (incomingData, responseChannel) =>
                {
                    byte[] data = incomingData.Buffer;
                    string msg = Encoding.UTF8.GetString(data);
                    sb.Append(msg);
                };

                Client.OnDisconnection += (reason, closedChannel) =>
                {     
                    Client.Dispose();
                    throw new Exception(reason.Message);
                };

                Client.Open();

            }


        }

        #endregion



        public string GetId(int idType, int count = 1)
        {
            InitSocket();
            byte[] b1 = BitConverter.GetBytes(idType); //send login
            byte[] b2 = BitConverter.GetBytes(count);

            byte[] by = b1.Concat(b2).ToArray(); ;
            
            Client.Send(by, 0, by.Length, serverNode);
            
            return sb.ToString();

        }


    }
}
