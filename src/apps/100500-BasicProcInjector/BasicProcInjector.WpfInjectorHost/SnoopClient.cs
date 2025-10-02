namespace BasicProcInjector.WpfInjectorHost
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using BasicProcInjector.WpfInjectorHost.Utilities;
    using CommandLine;
    using BasicProcInjector.Core;
    using BasicProcInjector.InjectorLauncher;

    internal class SnoopClient
    {
        public SnoopClient()
        {
        }

        public void StartSnoopProcessNew(int processId, IntPtr targetHwnd)
        {
            var processWrapper = ProcessWrapper.From(processId, targetHwnd);

            MethodInfo methodInfo = typeof(SnoopManagerNew).GetMethod(nameof(SnoopManagerNew.StartSnoop))!;

            var assemblyName = methodInfo.DeclaringType!.Assembly.GetName().Name;

            var fullAssemblyPath = Assembly.GetExecutingAssembly().Location;

            var className = methodInfo.DeclaringType.FullName!;

            string transientSettingsFile = this.GetTransientSettingsFile(BasicProcInjectorStartTargetNew.SnoopUI, targetHwnd);

            var injectorData = new InjectorDataNew
            {
                FullAssemblyPath = fullAssemblyPath,
                ClassName = className,
                MethodName = methodInfo.Name,
                SettingsFile = transientSettingsFile
            };

            //        C:\Trials\LearnSnoopWpf\src\apps\100500 - BasicProcInjector\BasicProcInjector.WpfInjectorHost\bin\Debug\Snoop.Core.dll
            //Snoop.Infrastructure.SnoopManager.StartSnoop
            // C:\Trials\LearnSnoopWpf\src\apps\100200-SnoopSc\bin\Debug\net6.0-windows
            injectorData.FullAssemblyPath = @"C:\Trials\LearnSnoopWpf\src\apps\100200-SnoopSc\bin\Debug\net6.0-windows\Snoop.Core.dll";
            // injectorData.FullAssemblyPath = @"C:\Trials\LearnSnoopWpf\src\apps\100500-BasicProcInjector\BasicProcInjector.WpfInjectorHost\bin\Debug\Snoop.Core.dll";
            injectorData.ClassName = "Snoop.Infrastructure.SnoopManager";
            injectorData.MethodName = "StartSnoop";
            Injector.InjectIntoProcess(processWrapper, injectorData);
        }

        public AttachResultNew StartSnoopProcess(int processId, IntPtr targetHwnd)
        {
            var processInfo = new ProcessInfoNew(processId);
            string transientSettingsFile = this.GetTransientSettingsFile(BasicProcInjectorStartTargetNew.SnoopUI, targetHwnd);
            try
            {
                MethodInfo methodInfo = typeof(SnoopManagerNew).GetMethod(nameof(SnoopManagerNew.StartSnoop))!;
                if (!File.Exists(transientSettingsFile))
                {
                    throw new FileNotFoundException("The generated temporary settings file could not be found.", transientSettingsFile);
                }

                var location = Assembly.GetExecutingAssembly().Location;
                var directory = Path.GetDirectoryName(location) ?? string.Empty;
                // If we get the architecture wrong here the InjectorLauncher will fix this by starting a secondary instance.
                //var architecture = NativeMethodsNew.GetArchitectureWithoutException(processInfo.Process);
                //var injectorLauncherExe = Path.Combine(directory, $"Snoop.InjectorLauncher.{architecture}.exe");

                var injectorLauncherExe = Path.Combine(directory, $"Snoop.InjectorLauncher.exe");

                Clipboard.SetText(injectorLauncherExe); // For diagnostic purposes only. To be removed later.

                if (File.Exists(injectorLauncherExe) is false)
                {
                    var message = @$"Could not find the injector launcher ""{injectorLauncherExe}"".
                    Snoop requires this component, which is part of the Snoop project, to do it's job.
                    - If you compiled snoop yourself, you should compile all required components.
                    - If you downloaded snoop you should not omit any files contained in the archive you downloaded and make sure that no anti virus deleted the file.";
                    throw new FileNotFoundException(message, injectorLauncherExe);
                }

                var assemblyName = methodInfo.DeclaringType!.Assembly.GetName().Name;
                var className = methodInfo.DeclaringType.FullName!;
                var injectorLauncherCommandLineOptions = new InjectorLauncherCommandLineOptionsNew
                {
                    TargetPID = processInfo.Process.Id,
                    TargetHwnd = targetHwnd.ToInt32(),
                    Assembly = assemblyName,
                    ClassName = className,
                    MethodName = methodInfo.Name,
                    SettingsFile = transientSettingsFile,
                    Debug = ProgramNew.Debug,
                    AttachConsoleToParent = true
                };

                var commandLine = Parser.Default.FormatCommandLine(injectorLauncherCommandLineOptions);

                Clipboard.SetText(commandLine);

                var processStartInfo = new ProcessStartInfo(injectorLauncherExe, commandLine)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProgramNew.Debug ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                    Verb = processInfo.IsProcessElevated
                    ? "runas"
                    : null,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                LogHelperNew.WriteLine($"Launching injector \"{processStartInfo.FileName}\".");
                LogHelperNew.WriteLine($"Arguments: {commandLine}.");

                using var process = new Process();
                process.StartInfo = processStartInfo;

                // Subscribe to output streams
                process.OutputDataReceived += (_, args) =>
                {
                    if (args.Data is not null)
                    {
                        LogHelperNew.WriteLine($"[Injector Output] {args.Data}");
                    }
                };

                process.ErrorDataReceived += (_, args) =>
                {
                    if (args.Data is not null)
                    {
                        LogHelperNew.WriteLine($"[Injector Error] {args.Data}");
                    }
                };

                if (process.Start())
                {
                    // Begin reading output/error streams asynchronously
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    LogHelperNew.WriteLine($"Injector returned exit code: {process.ExitCode}.");
                }
                else
                {
                    LogHelperNew.WriteLine("Injector could not be started.");
                }
            }
            catch (Exception e)
            {
                return new AttachResultNew(e);
            }
            finally
            {
                File.Delete(transientSettingsFile);
            }

            return new AttachResultNew();
        }

        public string GetTransientSettingsFile(BasicProcInjectorStartTargetNew startTarget, IntPtr targetWindowHandle)
        {
            var settings = new SettingsSnoopNew().Load();

            var transientSettingData = new TransientSettingsDataNew
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