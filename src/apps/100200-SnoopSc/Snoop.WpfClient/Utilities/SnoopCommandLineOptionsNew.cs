namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CommandLine;

    public interface IOptionsWithTargetNew
    {
        [Option('t', "targetPID", Required = true, HelpText = "The target process id.")]
        int TargetPID { get; set; }

        [Option('h', "targetHwnd", HelpText = "The target window handle.")]
        int TargetHwnd { get; set; }
    }

    [Verb("inspect", HelpText = "Inspect the UI of target process.")]
    public class InspectCommandLineOptionsNew : BaseCommandLineOptionsNew, IOptionsWithTargetNew
    {
        public int TargetPID { get; set; }

        public int TargetHwnd { get; set; }
    }

    [Verb("magnify", HelpText = "Magnify the UI of target process.")]
    public class MagnifyCommandLineOptionsNew : BaseCommandLineOptionsNew, IOptionsWithTargetNew
    {
        public int TargetPID { get; set; }

        public int TargetHwnd { get; set; }
    }

    [Verb("run", HelpText = "Run snoop.")]
    public class SnoopCommandLineOptionsNew : BaseCommandLineOptionsNew
    {
    }

    public class BaseCommandLineOptionsNew
    {
        [Option('d', "debug")]
        public bool Debug { get; set; }

        [Option("showConsole", HelpText = "Shows the console even when the application is running with a GUI.")]
        public bool ShowConsole { get; set; }
    }

}
