using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Core;

public sealed record MapPartInfo(ushort Value, ushort BasePart, ushort ItemId, byte Width, byte Height, byte Depth, bool CanEdit)
{
    public const int SIZE = 8;
    public const int Count = 4096;
    private static readonly MapPartInfo[] Parts = ReadParts();

    public static MapPartInfo Get(ushort variation) => variation < Parts.Length ? Parts[variation] : Parts[0];
    public static ReadOnlySpan<MapPartInfo> All => Parts;

    private static MapPartInfo[] ReadParts()
    {
        var data = ResourceUtil.GetBinaryResource("map_parts.bin");
        if (data.Length != Count * SIZE)
            throw new InvalidOperationException("The map part catalog is invalid.");
        var result = new MapPartInfo[Count];
        for (ushort i = 0; i < result.Length; i++)
        {
            var row = data.AsSpan(i * SIZE, SIZE);
            result[i] = new MapPartInfo(i, ReadUInt16LittleEndian(row), ReadUInt16LittleEndian(row[2..]), row[4], row[5], row[6], row[7] != 0);
        }
        return result;
    }

    public MapBounds GetBounds(int x, int y, int z, int direction) => direction switch
    {
        0 => new(x, y, z + 1 - Depth, Width, Height, Depth),
        1 => new(x + 1 - Depth, y, z + 1 - Width, Depth, Height, Width),
        2 => new(x + 1 - Width, y, z, Width, Height, Depth),
        3 => new(x, y, z, Depth, Height, Width),
        _ => throw new ArgumentOutOfRangeException(nameof(direction)),
    };
}
