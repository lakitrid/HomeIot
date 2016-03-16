using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HomeBrain
{
    class Program
    {
        private static void Main(string[] args)
        {
#if DEBUG
            new RuntimeService().Run();
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new RuntimeService()
            };

            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
