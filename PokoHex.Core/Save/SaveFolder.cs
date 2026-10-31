using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PokoHex.Core;

public sealed class SaveFolder
{
    public string Folder { get; }
    public SaveFile Common { get; }
    private readonly Dictionary<string, byte[]> Files;
    public bool IsEdited => Common.IsEdited;

    private SaveFolder(string folder, Dictionary<string, byte[]> files)
    {
        Folder = folder;
        Files = files;
        Common = SaveFile.Read(files[Path.Combine("G00", "CMNDAT.BIN")]);
    }

    public static SaveFolder Read(string path)
    {
        path = Path.GetFullPath(path);
        if (File.Exists(path))
            path = Directory.GetParent(Path.GetDirectoryName(path)!)!.FullName;
        if (Path.GetFileName(path).Equals("G00", StringComparison.OrdinalIgnoreCase))
            path = Directory.GetParent(path)!.FullName;
        if (!File.Exists(Path.Combine(path, "G00", "CMNDAT.BIN")) || !Directory.Exists(Path.Combine(path, "S00")))
            throw new InvalidDataException("Select the save folder containing G00 and S00.");

        var files = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
        foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
        {
            if ((File.GetAttributes(file) & FileAttributes.ReparsePoint) != 0)
                throw new IOException("Linked save files are not supported.");
            files.Add(Path.GetRelativePath(path, file), File.ReadAllBytes(file));
        }
        return new SaveFolder(path, files);
    }

    public void Export(string destination)
    {
        destination = Path.GetFullPath(destination).TrimEnd(Path.DirectorySeparatorChar);
        var source = Folder.TrimEnd(Path.DirectorySeparatorChar);
        if (destination.Equals(source, StringComparison.OrdinalIgnoreCase) ||
            destination.StartsWith(source + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new IOException("Choose a new folder outside the opened save.");
        if (File.Exists(destination) || (Directory.Exists(destination) && Directory.EnumerateFileSystemEntries(destination).Any()))
            throw new IOException("The export folder must be empty.");

        var common = Common.Write();
        var check = SaveFile.Read(common);
        if (!check.Data.AsSpan().SequenceEqual(Common.Data))
            throw new InvalidDataException("The edited save could not be verified.");

        Directory.CreateDirectory(destination);
        foreach (var (relative, original) in Files)
        {
            var target = Path.Combine(destination, relative);
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.WriteAllBytes(target, relative.Equals(Path.Combine("G00", "CMNDAT.BIN"), StringComparison.OrdinalIgnoreCase) ? common : original);
        }
    }
}
