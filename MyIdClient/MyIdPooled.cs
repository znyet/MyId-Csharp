using CodeProject.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIdClient
{
    internal class MyIdPooled : PooledObject
    {
        public MyIdPooled()
        {
            OnReleaseResources = () =>
            {
                Console.WriteLine("release");
            };
        }

        public string GetId(int idType)
        {
            return null;
        }
    }
}
