namespace ProcInjectorNoSettingsFile.WpfInjectorHost
{
    using System.Reflection;
    using ProcInjectorNoSettingsFile.Core;
    using ProcInjectorNoSettingsFile.InjectorLauncher;
    using ProcInjectorNoSettingsFile.MalDll;

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
                //methodInfo = typeof(SnoopManager).GetMethod(nameof(SnoopManager.StartSnoop))!;
                //snoopAssemblyPath = Assembly.GetAssembly(typeof(SnoopManager))!.Location;
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

            string transientSettingsJson = this.GetTransientSettingsJson(targetHwnd);

            var injectorData = new InjectorData
            {
                FullAssemblyPath = snoopAssemblyPath,
                ClassName = className,
                MethodName = methodInfo.Name,
                SettingsJson = transientSettingsJson
            };

            Injector.InjectIntoProcess(processWrapper, injectorData);
        }

        public string GetTransientSettingsJson(IntPtr targetWindowHandle)
        {
            var transientSettingData = new TransientSettingsData
            {
                TargetWindowHandle = targetWindowHandle.ToInt64(),
            };

            var jsonString = transientSettingData.ToJson();

            return jsonString;
        }

        public string GetTransientSettingsFile(IntPtr targetWindowHandle)
        {
            var transientSettingData = new TransientSettingsData
            {
                TargetWindowHandle = targetWindowHandle.ToInt64(),
            };

            return transientSettingData.WriteToFile();
        }
    }
}