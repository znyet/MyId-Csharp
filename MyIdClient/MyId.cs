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

        internal TimedObjectPool<MyIdPooled> pool;

        public MyId(string server, int port, string pwd = null, int maxPool = 100, int msgTimeout = 3000)
        {
            pool = new TimedObjectPool<MyIdPooled>(
                maxPool,
                () => new MyIdPooled(server, port, pwd, msgTimeout),
                TimeSpan.FromMinutes(1));
        }

        public MyId(string connectionString)
        {
            string msg = "connectionString example --> server=127.0.0.1;port=8123;pwd=123;maxPool=100;msgTimeout=3000";
            if (string.IsNullOrEmpty(connectionString) || !connectionString.Contains("server"))
                throw new Exception(msg);


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

    }
}


//case 0: id = Guid.NewGuid().ToString(); break;
//case 1: id = ObjectId.GenerateNewId().ToString(); break;
//case 2: id = SnowflakeId.idWorker.NextId().ToString(); break;
//case 3: id = Base36Id.Base16.NewId(); break;
//case 4: id = Base36Id.Base20.NewId(); break;
//case 5: id = Base36Id.Base25.NewId(); break;