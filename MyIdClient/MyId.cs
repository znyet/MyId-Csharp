using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIdClient
{
    public class MyId
    {
        public MyId(string ip, int port, string password = null, int maxPool = 100, int msgTimeout = 2000, int socketGC = 1)
        {

        }

        public MyId(string connectionString)
        {

        }

    }
}
