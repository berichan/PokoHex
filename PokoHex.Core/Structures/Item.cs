using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Core;

public sealed class Item(Memory<byte> raw)
{
    public const int SIZE = 5;
    public const uint ItemIdMask = 0x1FFF;
    public const int MaxCount = byte.MaxValue;

    private Span<byte> Data => raw.Span;

    public uint RawValue
    {
        get => ReadUInt32LittleEndian(Data);
        set => WriteUInt32LittleEndian(Data, value);
    }

    public ushort ItemId
    {
        get => (ushort)(RawValue & ItemIdMask);
        set
        {
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, ItemIdMask);
            RawValue = (RawValue & ~ItemIdMask) | value;
        }
    }

    public byte Count { get => Data[4]; set => Data[4] = value; }
    public bool IsEmpty => ItemId == 0;

    public void Set(ushort itemId, int count)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(itemId, ItemIdMask);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, MaxCount);
        if (itemId == 0 || count == 0)
        {
            Data.Clear();
            return;
        }

        // Variant and placement bits belong to the old item.
        if (ItemId != itemId)
            RawValue = 0;
        ItemId = itemId;
        Count = (byte)count;
    }
}
