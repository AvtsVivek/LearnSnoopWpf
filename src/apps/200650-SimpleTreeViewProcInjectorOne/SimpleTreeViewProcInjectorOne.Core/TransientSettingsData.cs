namespace SimpleTreeViewProcInjectorOne.Core
{
    using System;
    using System.IO;
    using System.Text.Json;
    using System.Xml.Serialization;

    public sealed class TransientSettingsData
    {
        private static readonly XmlSerializer serializer = new(typeof(TransientSettingsData));

        public static TransientSettingsData? Current { get; private set; }

        public bool SetOwnerWindow { get; set; } = true;

        public bool ShowActivated { get; set; } = true;

        public long TargetWindowHandle { get; set; }

        public string WriteToFile()
        {
            var settingsFile = Path.GetTempFileName();

            LogHelper.WriteLine($"Writing transient settings file to \"{settingsFile}\"");

            using var stream = new FileStream(settingsFile, FileMode.Create);
            serializer.Serialize(stream, this);

            return settingsFile;
        }

        public string ToJson()
        {
            var jsonSerializer = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            return JsonSerializer.Serialize(this, jsonSerializer);
        }

        public static TransientSettingsData FromJson(string json)
        {
            return JsonSerializer.Deserialize<TransientSettingsData>(json) ?? new TransientSettingsData();
        }
    }
}
