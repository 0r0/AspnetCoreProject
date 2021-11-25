using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreProject.Services
{
    public class MyService : IMyService
    {
        public int Calculate(int a, int b)
        {
            return a * b + b * 10;
        }
    }
}
