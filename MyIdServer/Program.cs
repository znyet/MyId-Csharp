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
