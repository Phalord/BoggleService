using BoggleService.Services;
using BoggleService.Utils;
using System;
using System.ServiceModel;

namespace BoggleServiceHost
{
    internal class BoggleHost
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();

        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(BoggleServices)))
            {
                host.Open();
                log.Info("Server Started...");
                Console.ReadLine();
            }
        }
    }
}
