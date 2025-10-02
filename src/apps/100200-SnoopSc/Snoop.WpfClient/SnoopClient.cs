namespace Snoop.WpfClient
{
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Windows;
    using CommandLine;
    using Snoop.Data;
    using Snoop.Infrastructure;
    using Snoop.InjectorLauncher;

    internal class SnoopClient
    {
        public SnoopClient()
        {
        }

        public AttachResult StartSnoopProcess(int processId, IntPtr targetHwnd)
        {
            var processInfo = new ProcessInfo(processId);
            string transientSettingsFile = this.GetTransientSettingsFile(SnoopStartTarget.SnoopUI, targetHwnd);

            Clipboard.SetText(transientSettingsFile); // For diagnostic purposes only. To be removed later. 

            try
            {
                MethodInfo methodInfo = typeof(SnoopManager).GetMethod(nameof(SnoopManager.StartSnoop))!;
                if (!File.Exists(transientSettingsFile))
                {
                    throw new FileNotFoundException("The generated temporary settings file could not be found.", transientSettingsFile);
                }

                var location = Assembly.GetExecutingAssembly().Location;
                var directory = Path.GetDirectoryName(location) ?? string.Empty;
                // If we get the architecture wrong here the InjectorLauncher will fix this by starting a secondary instance.
                var architecture = NativeMethods.GetArchitectureWithoutException(processInfo.Process);
                var injectorLauncherExe = Path.Combine(directory, $"Snoop.InjectorLauncher.{architecture}.exe");

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
                var injectorLauncherCommandLineOptions = new InjectorLauncherCommandLineOptions
                {
                    TargetPID = processInfo.Process.Id,
                    TargetHwnd = targetHwnd.ToInt32(),
                    Assembly = assemblyName,
                    ClassName = className,
                    MethodName = methodInfo.Name,
                    SettingsFile = transientSettingsFile,
                    Debug = Program.Debug,
                    AttachConsoleToParent = true
                };

                var commandLine = Parser.Default.FormatCommandLine(injectorLauncherCommandLineOptions);

                Clipboard.SetText(commandLine);

                var processStartInfo = new ProcessStartInfo(injectorLauncherExe, commandLine)
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    // WindowStyle = Program.Debug ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                    WindowStyle = ProcessWindowStyle.Normal,
                    Verb = processInfo.IsProcessElevated
                    ? "runas"
                    : null,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                };

                LogHelper.WriteLine($"Launching injector \"{processStartInfo.FileName}\".");
                LogHelper.WriteLine($"Arguments: {commandLine}.");

                using var process = new Process();
                process.StartInfo = processStartInfo;

                // Subscribe to output streams
                process.OutputDataReceived += (_, args) =>
                {
                    if (args.Data is not null)
                    {
                        LogHelper.WriteLine($"[Injector Output] {args.Data}");
                    }
                };

                process.ErrorDataReceived += (_, args) =>
                {
                    if (args.Data is not null)
                    {
                        LogHelper.WriteLine($"[Injector Error] {args.Data}");
                    }
                };

                if (process.Start())
                {
                    // Begin reading output/error streams asynchronously
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    LogHelper.WriteLine($"Injector returned exit code: {process.ExitCode}.");
                }
                else
                {
                    LogHelper.WriteLine("Injector could not be started.");
                }
            }
            catch (Exception e)
            {
                return new AttachResult(e);
            }
            finally
            {
                File.Delete(transientSettingsFile);
            }

            return new AttachResult();
        }

        public string GetTransientSettingsFile(SnoopStartTarget startTarget, IntPtr targetWindowHandle)
        {
            var settings = new Snoop.Settings().Load();

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