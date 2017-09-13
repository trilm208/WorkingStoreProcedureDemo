using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class MyDebugger
    {
        public static void Log(string data,params string[] argItems)
        {
            Debug.WriteLine(string.Format(data, argItems));
        }
    }
}
