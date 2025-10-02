namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using BasicProcInjector.Core;
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;


    public class ProcessInfoNew
    {
        private bool? isOwningProcessElevated;

        public ProcessInfoNew(int processId)
            : this(Process.GetProcessById(processId))
        {
        }

        public ProcessInfoNew(Process process)
        {
            this.Process = process;
        }

        public Process Process { get; }

        public bool IsProcessElevated => this.isOwningProcessElevated ??= NativeMethodsNew.IsProcessElevated(this.Process);

        public AttachResultNew Snoop(IntPtr targetHwnd)
        {
            if (Application.Current?.CheckAccess() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }

            try
            {
                InjectorLauncherManagerNew.Launch(this, targetHwnd,
                    typeof(SnoopManagerNew).GetMethod(nameof(SnoopManagerNew.StartSnoop))!,
                    CreateTransientSettingsData(BasicProcInjectorStartTargetNew.SnoopUI, targetHwnd));
            }
            catch (Exception e)
            {
                return new AttachResultNew(e);
            }
            finally
            {
                if (Application.Current?.CheckAccess() == true)
                {
                    Mouse.OverrideCursor = null;
                }
            }

            return new AttachResultNew();
        }

        public AttachResultNew Magnify(IntPtr targetHwnd)
        {
            if (Application.Current?.CheckAccess() == true)
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }

            try
            {
                InjectorLauncherManagerNew.Launch(this, targetHwnd,
                    typeof(SnoopManagerNew).GetMethod(nameof(SnoopManagerNew.StartSnoop))!,
                    CreateTransientSettingsData(BasicProcInjectorStartTargetNew.Zoomer, targetHwnd));
            }
            catch (Exception e)
            {
                return new AttachResultNew(e);
            }
            finally
            {
                if (Application.Current?.CheckAccess() == true)
                {
                    Mouse.OverrideCursor = null;
                }
            }

            return new AttachResultNew();
        }

        private static TransientSettingsDataNew CreateTransientSettingsData(BasicProcInjectorStartTargetNew startTarget, IntPtr targetWindowHandle)
        {
            var settings = SettingsSnoopNew.Default;

            return new TransientSettingsDataNew
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
        }
    }
}
