using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Topshelf;

namespace MyIdServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<MyIdService>();
                x.RunAsLocalSystem();
                x.OnException(e =>
                {
                    LogHelper.Error(e.Message);
                });
            });
        }
    }
}
