using System;
using System.IO;
using PokoHex.Core;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Parsing;

public static class GameMapDumper
{
    public static byte[] GetPartList(string path)
    {
        using var archive = new GameArchive(GameItemDumper.GetArchiveFolder(path));
        if (archive.EntryCount != 12298)
            throw new InvalidDataException("The map parameter version is not supported.");
        var parts = archive.Read(40);
        var variations = archive.Read(41);
        var sizes = archive.Read(42);
        var models = archive.Read(44);
        CheckTable(parts, 4096, 0, 80);
        CheckTable(variations, 4096, 3, 16);
        CheckTable(models, 4096, 0, 100);
        if (sizes.Length != 1088 || ReadInt32LittleEndian(sizes) != 255 || ReadInt32LittleEndian(sizes.AsSpan(4)) != 0)
            throw new InvalidDataException("The map size table is not supported.");
        var result = new byte[MapPartInfo.Count * MapPartInfo.SIZE];
        for (int i = 1; i < MapPartInfo.Count; i++)
        {
            var variation = variations.AsSpan(64 + i * 16, 16);
            var partId = ReadUInt16LittleEndian(variation[4..]);
            var modelId = ReadUInt16LittleEndian(variation);
            if (partId >= 4096 || modelId >= 4096)
                throw new InvalidDataException("A map definition is outside its table.");
            if (partId == 0)
                continue;
            var part = parts.AsSpan(64 + partId * 80, 80);
            var model = models.AsSpan(64 + modelId * 100, 100);
            var shape = model[0x5B];
            if (shape >= 255 || ReadUInt16LittleEndian(part[6..]) >= 4600)
                throw new InvalidDataException("The map definition is invalid.");
            var dimensions = sizes.AsSpan(64 + shape * 4, 3);
            var row = result.AsSpan(i * MapPartInfo.SIZE, MapPartInfo.SIZE);
            WriteUInt16LittleEndian(row, partId);
            WriteUInt16LittleEndian(row[2..], ReadUInt16LittleEndian(part[6..]));
            dimensions.CopyTo(row[4..]);
            // Soft furniture and toys use the rectangular, unconnected placement path.
            row[7] = (byte)(part[0x26] == 1 && model[0x5F] == 2 && dimensions.IndexOf((byte)0) < 0 ? 1 : 0);
        }
        return result;
    }

    private static void CheckTable(byte[] table, int count, int version, int stride)
    {
        if (table.Length != 64 + count * stride || ReadInt32LittleEndian(table) != count || ReadInt32LittleEndian(table.AsSpan(4)) != version)
            throw new InvalidDataException("The map parameter table is not supported.");
    }

    public static void UpdateDumps(string path, string destination)
    {
        var folder = Path.Combine(destination, "byte");
        Directory.CreateDirectory(folder);
        File.WriteAllBytes(Path.Combine(folder, "map_parts.bin"), GetPartList(path));
    }
}
