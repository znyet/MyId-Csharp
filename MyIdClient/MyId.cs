using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIdClient
{
    public class MyId
    {
        public MyId(string ip, int port, string pwd = null, int maxPool = 100, int msgTimeout = 2000, int socketGC = 1)
        {

        }

        public MyId(string connectionString)
        {

        }

        public string GetId(int idType)
        {
            using (var conn = MyIdManager.pool.GetObject())
            {
                return conn.GetId(idType);
            }
        }

        public string GetGuid()
        {
            return GetId(0);
        }

        public string GetObjectId()
        {
            return GetId(1);
        }

        public string GetSnowflakeId()
        {
            return GetId(2);
        }

        public long GetSnowflakeAsLong()
        {
            return Convert.ToInt64(GetSnowflakeId());
        }

        public string GetBase16Id()
        {
            return GetId(3);
        }

        public string GetBase20Id()
        {
            return GetId(3);
        }

        public string GetBase25Id()
        {
            return GetId(3);
        }

    }
}


//case 0: id = Guid.NewGuid().ToString(); break;
//case 1: id = ObjectId.GenerateNewId().ToString(); break;
//case 2: id = SnowflakeId.idWorker.NextId().ToString(); break;
//case 3: id = Base36Id.Base16.NewId(); break;
//case 4: id = Base36Id.Base20.NewId(); break;
//case 5: id = Base36Id.Base25.NewId(); break;