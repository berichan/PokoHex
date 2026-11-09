using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PokoHex.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Parsing;

public static class GameBuildingDumper
{
    private static readonly IReadOnlyDictionary<string, int> BuildingNames = new Dictionary<string, int>
    {
        {"jp", 2197},
        {"en", 2198},
        {"fr", 2199}, 
        {"it", 2200}, 
        {"de", 2201}, 
        {"es", 2202},
        {"zht", 2203},
        {"ko", 2204},
        {"zhs", 2205},
    };

    private const int StringStart = 0x644;

    public static string[] GetBuildingList(string path, string language = GameLanguage.DefaultLanguage) 
    {
        return ReadBuildings(path, language);
    }

    public static void UpdateDumps(string path, string destination)
    {
        foreach (var language in BuildingNames.Keys)
        {
            var buildings = GetBuildingList(path, language);
            var folder = Path.Combine(destination, "text", language);
            Directory.CreateDirectory(folder);
            File.WriteAllLines(Path.Combine(folder, $"text_building_{language}.txt"), buildings, new UTF8Encoding(false));
        }
    }

    public static string[] ReadBuildings(string path, string language = GameLanguage.DefaultLanguage)
    {
        if (!BuildingNames.TryGetValue(language, out var nameTable))
            throw new ArgumentException(
                "This language is not supported.",
                nameof(language));

        path = GameItemDumper.GetArchiveFolder(path);
        using var archive = new GameArchive(path);

        if (archive.EntryCount != 12298)
            throw new InvalidDataException(
                "The game archive version is not supported.");

        var data = archive.Read(nameTable);
        if (data.Length <= StringStart)
            throw new InvalidDataException(
                "The building name table is invalid.");

        var result = new List<string>() { "" };
        var encoding = new UTF8Encoding(false, true);
        bool skipLocationGrammar = language != "jp";
        int position = StringStart;
        int lineIndex = 0;

        while (position < data.Length)
        {
            int end = Array.IndexOf(data, (byte)0, position);
            if (end < 0)
                end = data.Length;
            if (end > position)
            {
                string text = encoding.GetString(data.AsSpan(position, end - position));
                if (!skipLocationGrammar || lineIndex % 2 == 0)
                    result.Add(text);
                lineIndex++;
            }
            position = end + 1;
        }

        if (result.Count == 0)
            throw new InvalidDataException("No building names were found in the table.");

        return result.ToArray();
    }
    public static void DumpBuildingStringAtOffset(
    string archiveFolder,
    int offset,
    string destination)
    {
        var archive = new GameArchive(archiveFolder);
        byte[] data = archive.Read(2198);

        var output = new List<string>
    {
        $"Archive entry: 2198",
        $"Size: {data.Length}",
        $"Offset: 0x{offset:X}",
        ""
    };

        if (offset < 0 || offset >= data.Length)
        {
            output.Add("Offset outside entry.");
        }
        else
        {
            int end = offset;

            while (end < data.Length && data[end] != 0)
                end++;

            output.Add(
                Encoding.UTF8.GetString(
                    data,
                    offset,
                    end - offset));

            output.Add("");
            output.Add(
                Convert.ToHexString(
                    data.AsSpan(offset, Math.Min(64, data.Length - offset))));
        }

        File.WriteAllLines(
            destination,
            output,
            new UTF8Encoding(false));
    }

    public static void FindBuildingStringReferences(
    string folder,
    string destination)
    {
        var output = new List<string>();

        output.Add($"Input folder: {folder}");
        output.Add("");

        var tables = new[]
        {
        (Entry: 40, Stride: 80, FieldOffset: 6),
        (Entry: 43, Stride: 8, FieldOffset: 2)
    };

        foreach (var table in tables)
        {
            string file = Path.Combine(
                folder,
                $"entry_{table.Entry}.bin");

            output.Add($"========== ENTRY {table.Entry} ==========");

            if (!File.Exists(file))
            {
                output.Add($"FILE NOT FOUND: {file}");
                output.Add("");
                continue;
            }

            byte[] data = File.ReadAllBytes(file);

            int count = (data.Length - 64) / table.Stride;

            output.Add($"Size: {data.Length}");
            output.Add($"Stride: {table.Stride}");
            output.Add($"Field offset: 0x{table.FieldOffset:X}");
            output.Add("");

            for (int index = 0; index < count; index++)
            {
                int offset = 64 + index * table.Stride;

                ushort value =
                    System.Buffers.Binary.BinaryPrimitives
                        .ReadUInt16LittleEndian(
                            data.AsSpan(offset + table.FieldOffset, 2));

                bool isBuildingType = value == 14 || value == 31;

                bool isStringOffset =
                    value >= 0x0600 &&
                    value <= 0x0F00;

                if (!isBuildingType && !isStringOffset)
                    continue;

                output.Add(
                    $"RECORD {index} | FIELD +0x{table.FieldOffset:X2} | " +
                    $"VALUE 0x{value:X4} ({value})");

                output.Add(
                    Convert.ToHexString(
                        data.AsSpan(offset, table.Stride)));
            }

            output.Add("");
        }

        string? outputDirectory = Path.GetDirectoryName(destination);

        if (!string.IsNullOrEmpty(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        File.WriteAllLines(
            destination,
            output,
            new UTF8Encoding(false));
    }
    public static void DumpBuildingReferenceRecords(
    string folder,
    string destination)
    {
        const int headerSize = 64;
        const int stride = 80;

        var records = new[]
        {
        (Label: "Leaf Cottage", Type: 14, Reference: 994),
        (Label: "Poke Mart", Type: 31, Reference: 1031)
    };

        var output = new List<string>();

        output.Add($"Input folder: {folder}");
        output.Add("");

        string file = Path.Combine(folder, "entry_40.bin");

        if (!File.Exists(file))
        {
            output.Add($"FILE NOT FOUND: {file}");
            File.WriteAllLines(destination, output, new UTF8Encoding(false));
            return;
        }

        byte[] data = File.ReadAllBytes(file);

        output.Add($"Entry 40 size: {data.Length}");
        output.Add($"Record stride: {stride}");
        output.Add("");

        foreach (var item in records)
        {
            output.Add($"========== {item.Label} ==========");
            output.Add($"Building type: {item.Type}");
            output.Add($"Building record: {item.Type}");
            output.Add($"Reference value: {item.Reference}");
            output.Add("");

            foreach (int index in new[] { item.Type, item.Reference })
            {
                int offset = headerSize + index * stride;

                if (offset + stride > data.Length)
                {
                    output.Add($"RECORD {index}: OUT OF RANGE");
                    continue;
                }

                ReadOnlySpan<byte> record = data.AsSpan(offset, stride);

                output.Add($"--- RECORD {index} | OFFSET 0x{offset:X} ---");
                output.Add($"HEX: {Convert.ToHexString(record)}");
                output.Add("");

                for (int field = 0; field < stride; field += 4)
                {
                    uint value32 = System.Buffers.Binary.BinaryPrimitives
                        .ReadUInt32LittleEndian(record.Slice(field, 4));

                    ushort value16a = System.Buffers.Binary.BinaryPrimitives
                        .ReadUInt16LittleEndian(record.Slice(field, 2));

                    ushort value16b = System.Buffers.Binary.BinaryPrimitives
                        .ReadUInt16LittleEndian(record.Slice(field + 2, 2));

                    output.Add(
                        $"+0x{field:X2}: " +
                        $"{value32:X8} | " +
                        $"{value16a:X4} {value16b:X4} | " +
                        $"{value32}");
                }

                output.Add("");
            }
        }

        string? outputDirectory = Path.GetDirectoryName(destination);

        if (!string.IsNullOrEmpty(outputDirectory))
            Directory.CreateDirectory(outputDirectory);

        File.WriteAllLines(
            destination,
            output,
            new UTF8Encoding(false));
    }

    public static void DumpBuildingReferenceEntries(
    string path,
    string destination)
    {
        path = GameItemDumper.GetArchiveFolder(path);

        using var archive = new GameArchive(path);

        int[] entries = { 40, 41, 43, 44 };

        Directory.CreateDirectory(destination);

        foreach (int index in entries)
        {
            byte[] data = archive.Read(index);

            File.WriteAllBytes(
                Path.Combine(destination, $"entry_{index}.bin"),
                data);
        }
    }
    public static void FindKnownBuildingReferences(
    string path,
    string destination)
    {
        path = GameItemDumper.GetArchiveFolder(path);

        using var archive = new GameArchive(path);

        var targets = new Dictionary<int, string>
    {
        { 0x078B, "Leaf Cottage (type 14)" },
        { 0x095D, "Poké Mart (type 31)" },
    };

        var output = new List<string>();

        output.Add($"Archive entries: {archive.EntryCount}");
        output.Add("");

        for (int entryIndex = 0; entryIndex < archive.EntryCount; entryIndex++)
        {
            byte[] data;

            try
            {
                data = archive.Read(entryIndex);
            }
            catch
            {
                continue;
            }

            var matches = new List<string>();

            for (int offset = 0; offset < data.Length - 1; offset++)
            {
                ushort value16 = ReadUInt16LittleEndian(data.AsSpan(offset));

                if (targets.TryGetValue(value16, out var name16))
                {
                    int start = Math.Max(0, offset - 16);
                    int length = Math.Min(data.Length - start, 48);

                    matches.Add(
                        $"  16-bit | +0x{offset:X} | {name16}\n" +
                        $"  Bytes: {Convert.ToHexString(data.AsSpan(start, length))}");
                }

                if (offset <= data.Length - 4)
                {
                    uint value32 = ReadUInt32LittleEndian(data.AsSpan(offset));

                    if (value32 <= int.MaxValue &&
                        targets.TryGetValue((int)value32, out var name32))
                    {
                        int start = Math.Max(0, offset - 16);
                        int length = Math.Min(data.Length - start, 48);

                        matches.Add(
                            $"  32-bit | +0x{offset:X} | {name32}\n" +
                            $"  Bytes: {Convert.ToHexString(data.AsSpan(start, length))}");
                    }
                }
            }

            if (matches.Count > 0)
            {
                output.Add($"========== ENTRY {entryIndex} ==========");
                output.AddRange(matches);
                output.Add("");
            }
        }

        File.WriteAllLines(
            destination,
            output,
            new UTF8Encoding(false));
    }
}