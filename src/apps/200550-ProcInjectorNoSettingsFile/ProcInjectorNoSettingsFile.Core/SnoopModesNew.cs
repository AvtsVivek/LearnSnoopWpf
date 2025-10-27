using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcInjectorNoSettingsFile.Core
{
    public static class SnoopModesNew
    {
        /// <summary>
        /// Whether Snoop is snooping in a situation where there are multiple app domains.
        /// The main Snoop UI is needed for each app domain.
        /// </summary>
        public static bool MultipleAppDomainMode { get; set; }

        /// <summary>
        /// Whether Snoop is snooping in a situation where there are multiple dispatchers.
        /// The main Snoop UI is needed for each dispatcher.
        /// </summary>
        public static bool MultipleDispatcherMode { get; set; }

        public static bool SwallowExceptions { get; set; }

        public static bool IgnoreExceptions { get; set; }
    }
}
