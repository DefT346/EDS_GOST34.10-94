using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDS_GHOST34._10_94
{
    internal class Interface
    {
        public static void Run()
        {
            while (true)
            {
                Console.Write("\n> ");
                Commands.Parse(Console.ReadLine());
            }
        }
    }
}
