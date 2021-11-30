using BoggleService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BoggleServiceHost
{
    internal class BoggleHost
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(BoggleServices)))
            {
                host.Open();
                Console.WriteLine("Server Started...");
                Console.ReadLine();
            }
        }
    }
}
