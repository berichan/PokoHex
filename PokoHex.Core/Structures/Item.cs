using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Core;

public sealed class Item(Memory<byte> raw)
{
    public const int SIZE = 5;

    public const uint ItemIdMask = 0x00001FFF;

    public const uint ItemPaint1Mask = 0x0003E000;
    public const uint ItemPaint2Mask = 0x03E00000;
    public const uint ItemPaint3Mask = 0x7C000000;

    public const uint ItemPatternMask = 0x001FE000;
    public const uint ItemPatternDiscriminatorMask = 0x001C0000;

    public const int MaxCount = byte.MaxValue;
    public bool HasCustomId = false;

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

    public ushort ItemPaint1
    {
        get => (ushort)(HasItemPattern ? 0 : (RawValue & ItemPaint1Mask) >> 13);
        private set
        {
            if (value > 18)
                throw new ArgumentOutOfRangeException(nameof(value));

            RawValue = (RawValue & ~(ItemPaint1Mask | ItemPatternDiscriminatorMask)) | ((uint)value << 13);
        }
    }

    public ushort ItemPaint2
    {
        get => (ushort)((RawValue & ItemPaint2Mask) >> 21);
        private set
        {
            if (value > 18)
                throw new ArgumentOutOfRangeException(nameof(value));

            RawValue = (RawValue & ~ItemPaint2Mask) | ((uint)value << 21);
        }
    }

    public ushort ItemPaint3
    {
        get => (ushort)((RawValue & ItemPaint3Mask) >> 26);
        private set
        {
            if (value >= ItemPaints.Count)
                throw new ArgumentOutOfRangeException(nameof(value));

            RawValue = (RawValue & ~ItemPaint3Mask) | ((uint)value << 26);
        }
    }

    // TODO color 4

    public ushort ItemPattern => (ushort)(_ItemPattern == 0 ? 0 : _ItemPattern - 31);
    private ushort _ItemPattern
    {
        get => (ushort)(!HasItemPattern ? 0 : ((RawValue >> 13) & 0xFF));
        set
        {
            if (value > 255)
                throw new ArgumentOutOfRangeException(nameof(value));

            RawValue = (RawValue & ~ItemPatternMask) | ((uint)value << 13);
        }
    }

    public bool HasItemPattern => (RawValue & ItemPatternDiscriminatorMask) != 0;

    public bool HasItemPainted => !HasItemPattern && (RawValue & (ItemPaint1Mask | ItemPaint2Mask | ItemPaint3Mask)) != 0;   

    public byte Count
    {
        get => Data[4];
        set => Data[4] = value;
    }

    public bool IsEmpty => ItemId == 0;

    public void SetPaint(ushort paint1, ushort paint2, ushort paint3)
    {
        if (!HasItemPattern || paint1 != 0)
            ItemPaint1 = paint1;
        ItemPaint2 = paint2;
        ItemPaint3 = paint3;
    }
    public void SetPattern(ushort pattern)
    {
        if (pattern == 0)
            _ItemPattern = 0;
        else
            _ItemPattern = (ushort)(pattern + 31);
    }

    public void Set(ushort itemId, int count, bool isCustom = false)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(itemId, ItemIdMask);
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(count, MaxCount);

        if (itemId == 0 || count == 0)
        {
            Data.Clear();
            return;
        }

        HasCustomId = isCustom;

        // Clear only the 13 item ID bits.
        RawValue &= ~ItemIdMask;

        ItemId = itemId;
        Count = (byte)count;
    }

    public static Dictionary<uint, (string, Color)> ItemPaints = new()
    {
        { 0b00001, ("white", Color.White) },
        { 0b00010, ("red", Color.Red) },
        { 0b00011, ("blue", Color.Blue) },
        { 0b00100, ("yellow", Color.Yellow) },
        { 0b00101, ("green", Color.Green) },
        { 0b00110, ("aquamarine", Color.Aquamarine) },
        { 0b00111, ("orange", Color.Orange) },
        { 0b01000, ("purple", Color.Purple) },
        { 0b01001, ("brown", Color.Brown) },
        { 0b01010, ("cyan", Color.Cyan) },
        { 0b01011, ("rose", Color.HotPink) },
        { 0b01100, ("lime", Color.Lime) },
        { 0b01101, ("beige", Color.Beige) },
        { 0b01110, ("plum", Color.Plum) },
        { 0b01111, ("navy", Color.Navy) },
        { 0b10000, ("black", Color.Black) },
        { 0b10001, ("gray", Color.Gray) },
        { 0b10010, ("pink", Color.LightPink) }
    };
}