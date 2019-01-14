using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MyIdClient;

namespace Test
{
    public class MyIdHelper
    {
        private static readonly object _lock = new object();
        private static MyId _myId;

        public static MyId myId
        {
            get
            {
                if (_myId == null)
                {
                    lock (_lock)
                    {
                        if (_myId == null)
                            _myId = new MyId("127.0.0.1", 8123);
                    }
                }
                return _myId;
            }

        }
    }
}