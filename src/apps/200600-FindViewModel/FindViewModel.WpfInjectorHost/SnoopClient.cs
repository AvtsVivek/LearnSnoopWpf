namespace FindViewModel.WpfInjectorHost
{
    using System.Reflection;
    using FindViewModel.WpfInjectorHost.Utilities;
    using FindViewModel.Core;
    using FindViewModel.InjectorLauncher;
    using Snoop.Infrastructure;
    using FindViewModel.MalDll;

    internal class SnoopClient
    {
        public SnoopClient()
        {
        }

        public void StartSnoopProcessNew(int processId, IntPtr targetHwnd)
        {
            var processWrapper = ProcessWrapper.From(processId, targetHwnd);

            MethodInfo methodInfo = null!;

            string snoopAssemblyPath = null!;

            if (AppSettings.Default.FirstOrSecond == "First")
            {
                methodInfo = typeof(SnoopManager).GetMethod(nameof(SnoopManager.StartSnoop))!;
                snoopAssemblyPath = Assembly.GetAssembly(typeof(SnoopManager))!.Location;
            }
            else
            {
                methodInfo = typeof(StartClass).GetMethod(nameof(StartClass.StartMethod))!;
                snoopAssemblyPath = Assembly.GetAssembly(typeof(StartClass))!.Location;
            }

            var assemblyName = methodInfo.DeclaringType!.Assembly.GetName().Name;

            var className = methodInfo.DeclaringType.FullName!;

            string transientSettingsFile = this.GetTransientSettingsFile(FindViewModelStartTargetNew.SnoopUI, targetHwnd);

            var injectorData = new InjectorData
            {
                FullAssemblyPath = snoopAssemblyPath,
                ClassName = className,
                MethodName = methodInfo.Name,
                SettingsFile = transientSettingsFile
            };
            Injector.InjectIntoProcess(processWrapper, injectorData);
        }

        public string GetTransientSettingsFile(FindViewModelStartTargetNew startTarget, IntPtr targetWindowHandle)
        {
            var settings = new SettingsSnoop().Load();

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