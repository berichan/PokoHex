using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Core;

public sealed class MapPart
{
    public const int SIZE = 16;
    private readonly ReadOnlyMemory<byte> Raw;
    private readonly ReadOnlyMemory<byte> Point;
    private ReadOnlySpan<byte> Data => Raw.Span;
    private uint Location => ReadUInt32LittleEndian(Data[4..]);

    public int Index { get; }
    public uint Link => ReadUInt32LittleEndian(Data);
    public int Handle => (int)((ReadUInt32LittleEndian(Data[8..]) >> 4) & 0xFFFFF);
    public int Chunk => (int)(ReadUInt32LittleEndian(Point.Span) & 0xFFF);
    public ushort Variation => (ushort)(Location & 0xFFF);
    public int Direction => (int)(Location >> 30);
    public int X => ((Chunk & 63) * 32) + (int)((Location >> 13) & 31);
    public int Y => (int)((Location >> 18) & 127);
    public int Z => ((Chunk >> 6) * 32) + (int)((Location >> 25) & 31);
    public MapPartInfo Info => MapPartInfo.Get(Variation);
    public MapBounds Bounds => Info.GetBounds(X, Y, Z, Direction);

    internal MapPart(int index, ReadOnlyMemory<byte> raw, ReadOnlyMemory<byte> point)
    {
        Index = index;
        Raw = raw;
        Point = point;
    }
}
