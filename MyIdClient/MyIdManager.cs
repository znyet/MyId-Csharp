using CodeProject.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIdClient
{
    internal class MyIdManager
    {
        public static readonly TimedObjectPool<MyIdPooled> pool = new TimedObjectPool<MyIdPooled>(1, TimeSpan.FromSeconds(2)); //2秒不用进行释放
    }
}
