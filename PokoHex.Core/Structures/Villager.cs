using System;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Core;

public sealed class Villager
{
    public const int SIZE = 374;
    public const int FriendshipTierMaximum = 170;
    private readonly Memory<byte> Raw;
    private Span<byte> Data => Raw.Span;

    public int Index { get; }
    public VillagerHome Home { get; }
    public bool IsEmpty => Data.Slice(1, 16).IndexOfAnyExcept((byte)0) < 0;

    public Villager(int index, Memory<byte> raw, ReadOnlyMemory<byte> home)
    {
        Index = index;
        Raw = raw;
        Home = new VillagerHome(home);
    }

    public ushort Friendship
    {
        get => ReadUInt16LittleEndian(Data[0x11..]);
        set => WriteUInt16LittleEndian(Data[0x11..], value);
    }

    public ushort ThankPoints => ReadUInt16LittleEndian(Data[0x13..]);
    public byte ReceivedPresentLevel => Data[0x15];
    public bool WaitingForPresent => Data[0x16] != 0;

    public bool NameConversationFinished
    {
        get => Data[0xC4] != 0;
        set => Data[0xC4] = (byte)(value ? 1 : 0);
    }
}
