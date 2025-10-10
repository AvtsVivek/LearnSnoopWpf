namespace PresentationSourceInConsoleAppOne.WpfInjectorHost
{
    using System.Reflection;
    using PresentationSourceInConsoleAppOne.WpfInjectorHost.Utilities;
    using PresentationSourceInConsoleAppOne.Core;
    using PresentationSourceInConsoleAppOne.InjectorLauncher;
    
    using PresentationSourceInConsoleAppOne.MalDll;

    internal class SnoopClient
    {
        public SnoopClient()
        {
        }

        public void StartSnoopProcessNew(int processId)// , IntPtr targetHwnd)
        {
            var processWrapper = ProcessWrapper.From(processId); // , null);

            MethodInfo methodInfo = null!;

            string snoopAssemblyPath = null!;

            if (AppSettings.Default.FirstOrSecond == "First")
            {
                methodInfo = typeof(StartClass).GetMethod(nameof(StartClass.StartMethod))!;
                snoopAssemblyPath = Assembly.GetAssembly(typeof(StartClass))!.Location;
            }
            else
            {
                methodInfo = typeof(StartClass).GetMethod(nameof(StartClass.StartMethod))!;
                snoopAssemblyPath = Assembly.GetAssembly(typeof(StartClass))!.Location;
            }

            var assemblyName = methodInfo.DeclaringType!.Assembly.GetName().Name;

            var className = methodInfo.DeclaringType.FullName!;

            string transientSettingsFile = this.GetTransientSettingsFile(BasicProcInjectorStartTargetNew.SnoopUI); //, targetHwnd);

            var injectorData = new InjectorData
            {
                FullAssemblyPath = snoopAssemblyPath,
                ClassName = className,
                MethodName = methodInfo.Name,
                SettingsFile = transientSettingsFile
            };
            Injector.InjectIntoProcess(processWrapper, injectorData);
        }

        public string GetTransientSettingsFile(BasicProcInjectorStartTargetNew startTarget) //, IntPtr targetWindowHandle)
        {
            var settings = new SettingsSnoop().Load();

            var transientSettingData = new TransientSettingsData
            {
                StartTarget = startTarget,
                // TargetWindowHandle = targetWindowHandle.ToInt64(),
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