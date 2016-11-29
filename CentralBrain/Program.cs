using Fr.Lakitrid.CentralBrain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fr.Lakitrid.CentralBrain
{
    public class Program
    {        
        public static void Main(string[] args)
        {
            CentralService centralService = new CentralService();

            centralService.Start();

            WaitForEnd();

            centralService.Stop();
        }        

        private static void WaitForEnd()
        {
            string caption = string.Empty;

            do
            {
                caption = Console.ReadLine();
            }
            while (!caption.Equals("stop"));
        }
    }
}
