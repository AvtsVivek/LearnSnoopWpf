namespace BasicProcInjector.Core
{
    public class InjectorDataNew
    {
        public string FullAssemblyPath { get; set; } = string.Empty;

        public string ClassName { get; set; } = string.Empty;

        public string MethodName { get; set; } = string.Empty;

        public string? SettingsFile { get; set; }
    }
}
