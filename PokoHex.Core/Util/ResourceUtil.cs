using System;
using System.IO;
using System.Reflection;

namespace PokoHex.Core;

public static class ResourceUtil
{
    private static readonly Assembly ThisAssembly = typeof(ResourceUtil).Assembly;
    private static readonly string[] ResourceNames = ThisAssembly.GetManifestResourceNames();

    public static string[] GetStringList(string name, string language) => GetStringList($"text_{name}_{language}");

    public static string[] GetStringList(string name)
    {
        var text = GetStringResource(name);
        if (text == null)
            return [];
        var lines = text.Split('\n');
        if (lines.Length != 0 && lines[^1].Length == 0)
            Array.Resize(ref lines, lines.Length - 1);
        for (int i = 0; i < lines.Length; i++)
            lines[i] = lines[i].TrimEnd('\r');
        return lines;
    }

    public static string? GetStringResource(string name)
    {
        var resourceName = Array.Find(ResourceNames, z => z.StartsWith("PokoHex.Core.Resources.text.", StringComparison.Ordinal) &&
            z.EndsWith($".{name}.txt", StringComparison.OrdinalIgnoreCase));
        if (resourceName == null)
            return null;
        using var stream = ThisAssembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            return null;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
