namespace BasicProcInjector.Core
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using JetBrains.Annotations;

    [PublicAPI]
    public sealed class TransientSettingsDataNew
    {
        private static readonly XmlSerializer serializer = new(typeof(TransientSettingsDataNew));

        public static TransientSettingsDataNew? Current { get; private set; }

        public BasicProcInjectorStartTargetNew StartTarget { get; set; } = BasicProcInjectorStartTargetNew.SnoopUI;

        public MultipleAppDomainModeNew MultipleAppDomainMode { get; set; } = MultipleAppDomainModeNew.Ask;

        public MultipleDispatcherModeNew MultipleDispatcherMode { get; set; } = MultipleDispatcherModeNew.Ask;

        public bool SetOwnerWindow { get; set; } = true;

        public bool ShowActivated { get; set; } = true;

        public long TargetWindowHandle { get; set; }

        public string? ILSpyPath { get; set; } = "%path%";

        public bool EnableDiagnostics { get; set; } = true;

        public string? BasicProcInjectorInstallPath { get; set; } = Environment.GetEnvironmentVariable(SettingsHelperNew.SNOOP_INSTALL_PATH_ENV_VAR);

        public string WriteToFile()
        {
            var settingsFile = Path.GetTempFileName();

            LogHelperNew.WriteLine($"Writing transient settings file to \"{settingsFile}\"");

            using var stream = new FileStream(settingsFile, FileMode.Create);
            serializer.Serialize(stream, this);

            return settingsFile;
        }

        public static TransientSettingsDataNew LoadCurrentIfRequired(string settingsFile)
        {
            if (Current is not null)
            {
                return Current;
            }

            return LoadCurrent(settingsFile);
        }

        public static TransientSettingsDataNew LoadCurrent(string settingsFile)
        {
            LogHelperNew.WriteLine($"Loading transient settings file from \"{settingsFile}\"");

            using var stream = new FileStream(settingsFile, FileMode.Open);
            Current = (TransientSettingsDataNew?)serializer.Deserialize(stream) ?? new TransientSettingsDataNew();

            Environment.SetEnvironmentVariable(SettingsHelperNew.SNOOP_INSTALL_PATH_ENV_VAR, Current.BasicProcInjectorInstallPath, EnvironmentVariableTarget.Process);

            return Current;
        }
    }

    [PublicAPI]
    public enum MultipleAppDomainModeNew
    {
        Ask = 0,
        AlwaysUse = 1,
        NeverUse = 2
    }

    [PublicAPI]
    public enum MultipleDispatcherModeNew
    {
        Ask = 0,
        AlwaysUse = 1,
        NeverUse = 2
    }

    [PublicAPI]
    public enum BasicProcInjectorStartTargetNew
    {
        SnoopUI = 0,
        Zoomer = 1
    }
}
