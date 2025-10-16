using BasicProcInjector.Core;
using BasicProcInjector.MalDll.Infrastructure;
using BasicProcInjector.MalDll.Windows;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;
using System.Windows.Media;

namespace BasicProcInjector.MalDll
{
    [Serializable]
    public class SnoopCrossAppDomainInjectorNew : MarshalByRefObject
    {
        public SnoopCrossAppDomainInjectorNew()
        {
            SnoopModesNew.MultipleAppDomainMode = true;

            // We have to do this in the constructor because we might not be able to cast correctly.
            // Not being able to cast might be the case if the app domain we should run in uses shadow copies for it's assemblies.
            var settingsFile = Environment.GetEnvironmentVariable("Snoop.SettingsFile");

            if (string.IsNullOrEmpty(settingsFile) == false)
            {
                this.RunInCurrentAppDomain(settingsFile);
            }
        }

        private void RunInCurrentAppDomain(string settingsFile)
        {
            var settingsData = TransientSettingsData.LoadCurrentIfRequired(settingsFile);
            new StartClass().RunInCurrentAppDomain(settingsData);
        }
    }

    public class StartClass
    {
        public static int StartMethod(string settingsFile)
        {
            // MessageBox.Show("Hello from MalDll! Settings file: " + settingsFile);

            var dllPathToload = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "BasicProcInjector.Core.dll");

            if (File.Exists(dllPathToload))
            {
                var assembly = Assembly.LoadFrom(dllPathToload);
                // MessageBox.Show("Loaded assembly: " + assembly.FullName);
            }
            else
            {
                // MessageBox.Show("DLL not found: " + dllPathToload);
            }

            //string tempSettingsFileText = File.ReadAllText(settingsFile);

            //if (string.IsNullOrWhiteSpace(tempSettingsFileText))
            //{
            //    MessageBox.Show("Settings file is empty!", "Error");
            //    return 1;
            //}

            //MessageBox.Show(tempSettingsFileText, "Settings File");

            try
            {
                var startClass = new StartClass();
                var isSuccess = startClass.StartClassInstance(settingsFile);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Failed to start Snoop");
            }

            //try
            //{
            //    return new StartClass().StartClassInstance(settingsFile)
            //        ? 0
            //        : 2;
            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show($"Failed to start Snoop.{Environment.NewLine}{exception.ToString()}");
            //    LogHelper.WriteError(exception);
            //    return 1;
            //}

            return 0;
        }

        private bool StartClassInstance(string settingsFile)
        {
            LogHelper.WriteLine("Starting snoop...");
            var settingsData = TransientSettingsData.LoadCurrent(settingsFile);

            IList<AppDomain>? appDomains = null;

            if (settingsData.MultipleAppDomainMode != MultipleAppDomainModeNew.NeverUse)
            {
                appDomains = new AppDomainHelper().GetAppDomains();
            }

            var numberOfAppDomains = appDomains?.Count ?? 1;
            var succeeded = false;

            if (numberOfAppDomains < 1)
            {
                LogHelper.WriteLine("Snoop wasn't able to enumerate app domains or MultipleAppDomainMode was disabled. Trying to run in single app domain mode.");
                
                MessageBox.Show("Snoop wasn't able to enumerate app domains or MultipleAppDomainMode was disabled. Trying to run in single app domain mode.");
                
                succeeded = this.RunInCurrentAppDomain(settingsData);
            }
            else if (numberOfAppDomains == 1)
            {
                LogHelper.WriteLine("Only found one app domain. Running in single app domain mode.");

                // MessageBox.Show("Only found one app domain. Running in single app domain mode.");

                succeeded = this.RunInCurrentAppDomain(settingsData);
            }
            else
            {
                LogHelper.WriteLine($"Found {numberOfAppDomains} app domains. Running in multiple app domain mode.");

                MessageBox.Show($"Found {numberOfAppDomains} app domains. Running in multiple app domain mode.");

                var shouldUseMultipleAppDomainMode = settingsData.MultipleAppDomainMode is MultipleAppDomainModeNew.Ask 
                    or MultipleAppDomainModeNew.AlwaysUse;
                if (settingsData.MultipleAppDomainMode == MultipleAppDomainModeNew.Ask)
                {
                    var result =
                        MessageBox.Show(
                            "Snoop has noticed multiple app domains.\n\n" +
                            "Would you like to enter multiple app domain mode, and have a separate Snoop window for each app domain?\n\n" +
                            "Without having a separate Snoop window for each app domain, you will not be able to Snoop the windows in the app domains outside of the main app domain.",
                            "Enter Multiple AppDomain Mode",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Question,
                            MessageBoxResult.No);

                    if (result != MessageBoxResult.Yes)
                    {
                        shouldUseMultipleAppDomainMode = false;
                    }
                }

                if (shouldUseMultipleAppDomainMode == false
                    || appDomains is null)
                {
                    LogHelper.WriteLine("Running in single app domain mode.");

                    succeeded = this.RunInCurrentAppDomain(settingsData);
                }
                else
                {
                    LogHelper.WriteLine("Running in multiple app domain mode.");

                    SnoopModesNew.MultipleAppDomainMode = true;

                    // Use environment variable to transport snoop settings file across multiple app domains
                    Environment.SetEnvironmentVariable("Snoop.SettingsFile", settingsFile, EnvironmentVariableTarget.Process);

                    var assemblyFullName = typeof(StartClass).Assembly.Location;
                    var fullInjectorClassName = typeof(SnoopCrossAppDomainInjectorNew).FullName;

                    foreach (var appDomain in appDomains)
                    {
                        LogHelper.WriteLine($"Trying to create Snoop instance in app domain \"{appDomain.FriendlyName}\"...");

                        try
                        {
                            AttachAssemblyResolveHandler(appDomain);

                            // the injection code runs inside the constructor of SnoopCrossAppDomainManager
                            appDomain.CreateInstanceFrom(assemblyFullName, fullInjectorClassName!);

                            // if there is no exception we consider the injection successful
                            var appDomainSucceeded = true;
                            succeeded = succeeded || appDomainSucceeded;

                            LogHelper.WriteLine($"Successfully created Snoop instance in app domain \"{appDomain.FriendlyName}\".");
                        }
                        catch (Exception exception)
                        {
                            LogHelper.WriteLine($"Failed to create Snoop instance in app domain \"{appDomain.FriendlyName}\".");
                            LogHelper.WriteError(exception);
                        }
                    }
                }
            }

            if (succeeded == false)
            {
                MessageBox.Show("Can't find a current application or a PresentationSource root visual.",
                    "Can't Snoop",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
            }

            return false;
        }

        public bool RunInCurrentAppDomain(TransientSettingsData settingsData)
        {
            LogHelper.WriteLine($"Trying to run Snoop in app domain \"{AppDomain.CurrentDomain.FriendlyName}\"...");

            // MessageBox.Show($"Trying to run Snoop in app domain \"{AppDomain.CurrentDomain.FriendlyName}\"...");

            try
            {
                AttachAssemblyResolveHandler(AppDomain.CurrentDomain);

                // var instanceCreator = GetInstanceCreator(settingsData.StartTarget);
                var instanceCreator = GetInstanceCreator();

                var result = InjectSnoopIntoDispatchers(settingsData, 
                    (data, dispatcherRootObjectPair) => CreateSnoopWindow(data, dispatcherRootObjectPair, instanceCreator));

                LogHelper.WriteLine($"Successfully running Snoop in app domain \"{AppDomain.CurrentDomain.FriendlyName}\".");

                // MessageBox.Show($"Successfully running Snoop in app domain \"{AppDomain.CurrentDomain.FriendlyName}\".");

                return result;
            }
            catch (Exception exception)
            {
                LogHelper.WriteLine($"Failed to to run Snoop in app domain \"{AppDomain.CurrentDomain.FriendlyName}\".");

                MessageBox.Show($"Failed to to run Snoop in app domain \"{AppDomain.CurrentDomain.FriendlyName}\".");

                if (SnoopModesNew.MultipleAppDomainMode)
                {
                    LogHelper.WriteLine($"Could not snoop a specific app domain with friendly name of \"{AppDomain.CurrentDomain.FriendlyName}\" in multiple app domain mode.");
                    LogHelper.WriteError(exception);
                }
                else
                {
                    MessageBox.Show($"There was an error snooping the application.{Environment.NewLine}{exception}",
                        "Error snooping",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    // ErrorDialog.ShowDialog(exception, "Error snooping", "There was an error snooping the application.", exceptionAlreadyHandled: true);
                }

                return false;
            }
        }

        private static bool InjectSnoopIntoDispatchers(TransientSettingsData settingsData, 
            Func<TransientSettingsData, DispatcherRootObjectPair, SnoopMainBaseWindow> instanceCreator)
        {
            // Check and see if any of the PresentationSources have different dispatchers.
            // If so, ask the user if they wish to enter multiple dispatcher mode.
            // If they do, launch a snoop ui for every additional dispatcher.
            // See http://snoopwpf.codeplex.com/workitem/6334 for more info.

            // MessageBox.Show(nameof(InjectSnoopIntoDispatchers));

            var dispatcherRootObjectPairs = new List<DispatcherRootObjectPair>();

            foreach (PresentationSource? presentationSource in PresentationSource.CurrentSources)
            {
                if (presentationSource is null)
                {
                    continue;
                }

                var presentationSourceDispatcher = presentationSource.Dispatcher;
                Visual rootVisual = presentationSource.RunInDispatcher(() => presentationSource.RootVisual);

                object rootObject = rootVisual;

                if (Application.Current is not null
                    && Application.Current.Dispatcher == presentationSourceDispatcher)
                {
                    rootObject = Application.Current;
                }

                if (rootObject is null)
                {
                    continue;
                }

                var dispatcher = (rootObject as DispatcherObject)?.Dispatcher ?? presentationSourceDispatcher;
                var dispatcherRootObjectPair = new DispatcherRootObjectPair(dispatcher, rootObject);

                // Check if we have already seen this pair
                if (dispatcherRootObjectPairs.IndexOf(dispatcherRootObjectPair) == -1)
                {
                    dispatcherRootObjectPairs.Add(dispatcherRootObjectPair);
                }
            }

            var useMultipleDispatcherMode = false;
            if (dispatcherRootObjectPairs.Count <= 0)
            {
                return false;
            }

            switch (settingsData.MultipleDispatcherMode)
            {
                case MultipleDispatcherModeNew.NeverUse:
                    useMultipleDispatcherMode = false;
                    break;

                case MultipleDispatcherModeNew.AlwaysUse:
                    useMultipleDispatcherMode = dispatcherRootObjectPairs.Count > 1;
                    break;

                case MultipleDispatcherModeNew.Ask when dispatcherRootObjectPairs.Count == 1:
                    useMultipleDispatcherMode = false;
                    break;

                default:
                    {
                        var result =
                            MessageBox.Show(
                                "Snoop has noticed windows running in multiple dispatchers.\n\n" +
                                "Would you like to enter multiple dispatcher mode, and have a separate Snoop window for each dispatcher?\n\n" +
                                "Without having a separate Snoop window for each dispatcher, you will not be able to Snoop the windows in the dispatcher threads outside of the main dispatcher. " +
                                "Also, note, that if you bring up additional windows in additional dispatchers (after snooping), you will need to Snoop again in order to launch Snoop windows for those additional dispatchers.",
                                "Enter Multiple Dispatcher Mode",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            useMultipleDispatcherMode = true;
                        }

                        break;
                    }
            }

            if (useMultipleDispatcherMode)
            {
                SnoopModesNew.MultipleDispatcherMode = true;

                var thread = new Thread(DispatchOut)
                {
                    Name = "Snoop_DisptachOut_Thread"
                };

                thread.Start(new DispatchOutParameters(settingsData, instanceCreator, dispatcherRootObjectPairs));

                // todo: check if we really created something
                return true;
            }

            var dispatcherRootObjectPairForInstanceCreation = dispatcherRootObjectPairs.FirstOrDefault(x => x.RootObject is Application) ?? dispatcherRootObjectPairs[0];

            dispatcherRootObjectPairForInstanceCreation.Dispatcher.Invoke((Action)(() =>
            {
                _ = instanceCreator(settingsData, dispatcherRootObjectPairForInstanceCreation);
            }));

            return true;
        }

        private static void AttachAssemblyResolveHandler(AppDomain domain)
        {
            try
            {
                domain.AssemblyResolve += HandleDomainAssemblyResolve;
            }
            catch (Exception exception)
            {
                LogHelper.WriteError("Could not attach assembly resolver. Loading snoop assemblies might fail.");
                MessageBox.Show("Could not attach assembly resolver. Loading snoop assemblies might fail.");
                LogHelper.WriteError(exception);
            }
        }

        private static Assembly? HandleDomainAssemblyResolve(object? sender, ResolveEventArgs args)
        {
            if (args.Name?.StartsWith("Snoop.Core,") == true)
            {
                return Assembly.GetExecutingAssembly();
            }

#if NET6_0_OR_GREATER
            if (args.Name?.StartsWith("System.Management.Automation,") == true
                && PowerShell.ShellConstants.TryGetPowerShellCoreInstallPath(out var powershellCoreInstallPath))
            {
                return Assembly.LoadFrom(System.IO.Path.Combine(powershellCoreInstallPath, "System.Management.Automation.dll"));
            }
#endif

            return null;
        }

        //private static Func<SnoopMainBaseWindow> GetInstanceCreator(SnoopStartTarget startTarget)
        //{
        //    return () => new InjectedWindow();
        //    //switch (startTarget)
        //    //{
        //    //    case SnoopStartTarget.SnoopUI:
        //    //        return () => new SnoopUI();

        //    //    case SnoopStartTarget.Zoomer:
        //    //        return () => new Zoomer();

        //    //    default:
        //    //        throw new ArgumentOutOfRangeException(nameof(startTarget), startTarget, null);
        //    //}
        //}

        private static Func<SnoopMainBaseWindow> GetInstanceCreator()
        {
            return () => new InjectedWindow();
        }

        private static SnoopMainBaseWindow CreateSnoopWindow(TransientSettingsData settingsData,
            DispatcherRootObjectPair dispatcherRootObjectPair, Func<SnoopMainBaseWindow> instanceCreator)
        {
            var snoopWindow = instanceCreator();

            var targetWindowOnSameDispatcher = WindowHelperNew.GetVisibleWindow(settingsData.TargetWindowHandle, 
                dispatcherRootObjectPair.Dispatcher);

            snoopWindow.Title = TryGetWindowOrMainWindowTitle(targetWindowOnSameDispatcher);

            if (string.IsNullOrEmpty(snoopWindow.Title))
            {
                snoopWindow.Title = "Snoop";
            }
            else
            {
                snoopWindow.Title += " - Snoop";
            }

            // TODO: Implement Inspect method
            snoopWindow.Inspect(dispatcherRootObjectPair.RootObject);

            if (targetWindowOnSameDispatcher is not null)
            {
                snoopWindow.Target = targetWindowOnSameDispatcher;
            }

            return snoopWindow;
        }

        private static string TryGetWindowOrMainWindowTitle(Window? targetWindow)
        {
            if (targetWindow is not null)
            {
                return targetWindow.Title;
            }

            if (Application.Current is not null
                && Application.Current.CheckAccess()
                && Application.Current.MainWindow?.CheckAccess() == true)
            {
                return Application.Current.MainWindow.Title;
            }

            return string.Empty;
        }

        private static void DispatchOut(object? o)
        {
            var dispatchOutParameters = (DispatchOutParameters)o!;

            MessageBox.Show(nameof(DispatchOut) + " start1234");

            foreach (var dispatcherRootObjectPair in dispatchOutParameters.DispatcherRootObjectPairs)
            {
                // launch a snoop ui on each dispatcher
                dispatcherRootObjectPair.Dispatcher.Invoke(
                    (Action)(() =>
                    {
                        _ = dispatchOutParameters.InstanceCreator(dispatchOutParameters.SettingsData, dispatcherRootObjectPair);
                    }));
            }
        }

        private class DispatchOutParameters
        {
            public DispatchOutParameters(TransientSettingsData settingsData, 
                Func<TransientSettingsData, DispatcherRootObjectPair, Window> instanceCreator, 
                List<DispatcherRootObjectPair> dispatcherRootObjectPairs)
            {
                this.SettingsData = settingsData;
                this.InstanceCreator = instanceCreator;
                this.DispatcherRootObjectPairs = dispatcherRootObjectPairs;
            }

            public TransientSettingsData SettingsData { get; }

            public Func<TransientSettingsData, DispatcherRootObjectPair, Window> InstanceCreator { get; }

            public List<DispatcherRootObjectPair> DispatcherRootObjectPairs { get; }
        }

        private class DispatcherRootObjectPair : IEquatable<DispatcherRootObjectPair>
        {
            public DispatcherRootObjectPair(Dispatcher dispatcher, object rootObject)
            {
                this.Dispatcher = dispatcher;
                this.RootObject = rootObject;
            }

            public Dispatcher Dispatcher { get; }

            public object RootObject { get; }

            public bool Equals(DispatcherRootObjectPair? other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }

                if (ReferenceEquals(this, other))
                {
                    return true;
                }

                return this.Dispatcher.Equals(other.Dispatcher)
                       && this.RootObject.Equals(other.RootObject);
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                if (ReferenceEquals(this, obj))
                {
                    return true;
                }

                if (obj.GetType() != this.GetType())
                {
                    return false;
                }

                return this.Equals((DispatcherRootObjectPair)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (this.Dispatcher.GetHashCode() * 397) ^ this.RootObject.GetHashCode();
                }
            }

            public static bool operator ==(DispatcherRootObjectPair? left, DispatcherRootObjectPair? right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(DispatcherRootObjectPair? left, DispatcherRootObjectPair? right)
            {
                return !Equals(left, right);
            }
        }
    }
}
