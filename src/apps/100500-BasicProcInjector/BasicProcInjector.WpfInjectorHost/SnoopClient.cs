namespace BasicProcInjector.WpfInjectorHost
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using BasicProcInjector.WpfInjectorHost.Utilities;
    using CommandLine;
    using BasicProcInjector.Core;
    using BasicProcInjector.InjectorLauncher;
    using Snoop.Infrastructure;

    internal class SnoopClient
    {
        public SnoopClient()
        {
        }

        public void StartSnoopProcessNew(int processId, IntPtr targetHwnd)
        {
            var processWrapper = ProcessWrapper.From(processId, targetHwnd);

            MethodInfo methodInfo = typeof(SnoopManager).GetMethod(nameof(SnoopManager.StartSnoop))!;

            var assemblyName = methodInfo.DeclaringType!.Assembly.GetName().Name;

            var currentAssemblyPath = Assembly.GetExecutingAssembly().Location;

            var snoopAssemblyPath = Assembly.GetAssembly(typeof(SnoopManager))!.Location;

            var className = methodInfo.DeclaringType.FullName!;

            string transientSettingsFile = this.GetTransientSettingsFile(BasicProcInjectorStartTargetNew.SnoopUI, targetHwnd);

            var injectorData = new InjectorDataNew
            {
                FullAssemblyPath = snoopAssemblyPath,
                ClassName = className,
                MethodName = methodInfo.Name,
                SettingsFile = transientSettingsFile
            };

            //        C:\Trials\LearnSnoopWpf\src\apps\100500 - BasicProcInjector\BasicProcInjector.WpfInjectorHost\bin\Debug\Snoop.Core.dll
            //Snoop.Infrastructure.SnoopManager.StartSnoop
            // C:\Trials\LearnSnoopWpf\src\apps\100200-SnoopSc\bin\Debug\net6.0-windows

            //injectorData.FullAssemblyPath = @"C:\Trials\LearnSnoopWpf\src\apps\100500-BasicProcInjector\BasicProcInjector.WpfInjectorHost\bin\Debug\Snoop.Core.dll";
            //// injectorData.FullAssemblyPath = @"C:\Trials\LearnSnoopWpf\src\apps\100200-SnoopSc\bin\Debug\net6.0-windows\Snoop.Core.dll";
            //injectorData.ClassName = "Snoop.Infrastructure.SnoopManager";
            //injectorData.MethodName = "StartSnoop";
            Clipboard.SetText(injectorData.FullAssemblyPath);
            Injector.InjectIntoProcess(processWrapper, injectorData);
        }

        public string GetTransientSettingsFile(BasicProcInjectorStartTargetNew startTarget, IntPtr targetWindowHandle)
        {
            var settings = new SettingsSnoopNew().Load();

            var transientSettingData = new TransientSettingsData
            {
                StartTarget = startTarget,
                TargetWindowHandle = targetWindowHandle.ToInt64(),
                MultipleAppDomainMode = settings.MultipleAppDomainMode,
                MultipleDispatcherMode = settings.MultipleDispatcherMode,
                SetOwnerWindow = settings.SetOwnerWindow,
                ShowActivated = settings.ShowActivated,
                EnableDiagnostics = settings.EnableDiagnostics,
                ILSpyPath = settings.ILSpyPath
            };

            return transientSettingData.WriteToFile();
        }
    }
}