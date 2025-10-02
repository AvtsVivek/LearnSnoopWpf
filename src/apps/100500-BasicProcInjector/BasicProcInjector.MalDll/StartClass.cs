using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BasicProcInjector.MalDll
{
    public class StartClass
    {
        public static int StartMethod(string settingsFile)
        {
            MessageBox.Show("Hello from MalDll! Settings file: " + settingsFile);
            return 0;
        }
    }
}
