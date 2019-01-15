using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CodeProject.ObjectPool;

namespace MyIdClient
{
    public class MyId
    {

        internal ObjectPool<MyIdPooled> pool;

        public MyId(string server, int port, string pwd = null, int maxPool = 100, int timeout = 1000, int lifetime = 0)
        {
            pool = new ObjectPool<MyIdPooled>(maxPool, () => new MyIdPooled(server, port, pwd, timeout, lifetime));
        }

        public MyId(string connectionString)
        {
            string msg = "connectionString example --> server=127.0.0.1;port=8123;pwd=123456;maxPool=100;timeout=1000;lifetime=0";
            if (string.IsNullOrEmpty(connectionString) || !connectionString.Contains("server"))
                throw new Exception(msg);

            string server = null;
            int port = 8123;
            string pwd = null;
            int maxPool = 100;
            int timeout = 1000;
            int lifetime = 0;
            string[] arry = connectionString.Split(';');
            foreach (var item in arry)
            {
                string[] arry2 = item.Split('=');
                string key = arry2[0].ToLower().Trim();
                switch (key)
                {
                    case "server": server = arry2[1]; break;
                    case "pwd": pwd = arry2[1]; break;
                    case "port": port = Convert.ToInt32(arry2[1]); break;
                    case "maxpool": maxPool = Convert.ToInt32(arry2[1]); break;
                    case "timeout": timeout = Convert.ToInt32(arry2[1]); break;
                    case "lifetime": lifetime = Convert.ToInt32(arry2[1]); break;
                }
            }

            pool = new ObjectPool<MyIdPooled>(maxPool, () => new MyIdPooled(server, port, pwd, timeout, lifetime));
        }

        public string GetId(int idType, int count = 1)
        {
            using (var conn = pool.GetObject())
            {
                return conn.GetId(idType, count);
            }
        }

        public string GetGuid(int count = 1)
        {
            return GetId(0, count);
        }

        public string GetObjectId(int count = 1)
        {
            return GetId(1, count);
        }

        public string GetSnowflakeId(int count = 1)
        {
            return GetId(2, count);
        }

        public string GetBase16Id(int count = 1)
        {
            return GetId(3, count);
        }

        public string GetBase20Id(int count = 1)
        {
            return GetId(4, count);
        }

        public string GetBase25Id(int count = 1)
        {
            return GetId(5, count);
        }

        public string GetGuidToN(int count = 1)
        {
            return GetId(6, count);
        }

    }
}


//case 0: id = Guid.NewGuid().ToString(); break;
//case 1: id = ObjectId.GenerateNewId().ToString(); break;
//case 2: id = SnowflakeId.idWorker.NextId().ToString(); break;
//case 3: id = Base36Id.Base16.NewId(); break;
//case 4: id = Base36Id.Base20.NewId(); break;
//case 5: id = Base36Id.Base25.NewId(); break;