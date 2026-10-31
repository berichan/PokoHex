using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PokoHex.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Parsing;

public static class GameItemDumper
{
    private const int ItemTable = 853;
    private static readonly IReadOnlyDictionary<string, int> ItemNames = new Dictionary<string, int>
    {
        {"en", 4454},
        {"jp", 4453},
        {"de", 4457},
        {"es", 4458},
        {"fr", 4455},
        {"it", 4456},
        {"ko", 4460},
        {"zhs", 4461},
        {"zht", 4459},
    };

    public static string[] GetItemList(string path, string language = GameLanguage.DefaultLanguage)
    {
        var result = new string[4600];
        Array.Fill(result, string.Empty);
        foreach (var item in ReadItems(path, language))
            result[item.Value] = item.Text;
        return result;
    }

    public static void UpdateDumps(string path, string destination)
    {
        foreach (var language in ItemNames.Keys)
        {
            var items = GetItemList(path, language);
            var folder = Path.Combine(destination, "text", language);
            Directory.CreateDirectory(folder);
            File.WriteAllLines(Path.Combine(folder, $"text_item_{language}.txt"), items, new UTF8Encoding(false));
        }
    }

    public static ItemInfo[] ReadItems(string path, string language = GameLanguage.DefaultLanguage)
    {
        if (!ItemNames.TryGetValue(language, out var nameTable))
            throw new ArgumentException("This language is not supported.", nameof(language));
        path = GetArchiveFolder(path);
        using var archive = new GameArchive(path);
        var items = archive.Read(ItemTable);
        var names = archive.Read(nameTable);
        if (items.Length < 64 || names.Length < 64 || ReadInt32LittleEndian(items.AsSpan(4)) != 2 || ReadInt32LittleEndian(names.AsSpan(4)) != 2)
            throw new InvalidDataException("The item tables are not supported.");
        var count = ReadInt32LittleEndian(items);
        var nameCount = ReadInt32LittleEndian(names);
        if (count != 4600 || items.Length != 64 + (count * 64) || nameCount != 3001 || names.Length < 64 + (nameCount * 32))
            throw new InvalidDataException("The item table version is not supported.");

        var result = new List<ItemInfo> { new(0, "(Empty)") };
        for (ushort i = 1; i < count; i++)
        {
            var nameIndex = ReadUInt16LittleEndian(items.AsSpan(64 + (i * 64) + 28));
            if (nameIndex == 0 || nameIndex >= nameCount)
                continue;
            var field = 64 + (nameIndex * 32);
            var offset = (long)field + ReadUInt32LittleEndian(names.AsSpan(field));
            if (offset < 64 + (nameCount * 32) || offset >= names.Length)
                throw new InvalidDataException("The item name offset is invalid.");
            var text = names.AsSpan((int)offset);
            var end = text.IndexOf((byte)0);
            if (end < 0)
                throw new InvalidDataException("The item name is not terminated.");
            var name = new UTF8Encoding(false, true).GetString(text[..end]);
            if (name.Length != 0)
                result.Add(new ItemInfo(i, name));
        }
        return result.ToArray();
    }

    public static string GetArchiveFolder(string path)
    {
        if (File.Exists(Path.Combine(path, "LI_C0_R.IDX")))
            return path;
        var romfs = Path.Combine(path, "romfs");
        if (File.Exists(Path.Combine(romfs, "LI_C0_R.IDX")))
            return romfs;
        throw new FileNotFoundException("Select the game dump folder containing romfs, or romfs itself.");
    }
}
