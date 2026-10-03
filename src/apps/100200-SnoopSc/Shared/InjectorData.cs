using System;

public class InjectorData
{
    public string FullAssemblyPath { get; set; } = string.Empty;

    public string ClassName { get; set; } = string.Empty;

    public string MethodName { get; set; } = string.Empty;

    public string? SettingsFile { get; set; }

    public override string ToString()
    {
        var injectorDataString = $"FullAssemblyPath: {this.FullAssemblyPath}, " + Environment.NewLine +
            $"ClassName: {this.ClassName}, {Environment.NewLine}" +
            $"MethodName: {this.MethodName}, {Environment.NewLine}" +
            $"SettingsFile: {this.SettingsFile}";
        return injectorDataString;
    }
}