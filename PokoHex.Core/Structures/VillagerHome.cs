using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Core;

public sealed class VillagerHome(ReadOnlyMemory<byte> raw)
{
    public const int SIZE = 21;
    private ReadOnlySpan<byte> Data => raw.Span;

    public byte Type => Data[0];
    public ushort UniqueId => ReadUInt16LittleEndian(Data[1..]);
    public byte StageId => Data[3];
    public short BlockX => ReadInt16LittleEndian(Data[6..]);
    public short BlockY => ReadInt16LittleEndian(Data[8..]);
    public short BlockZ => ReadInt16LittleEndian(Data[10..]);
}
