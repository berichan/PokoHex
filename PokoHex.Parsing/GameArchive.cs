using System;
using System.IO;
using System.IO.Compression;
using static System.Buffers.Binary.BinaryPrimitives;

namespace PokoHex.Parsing;

public sealed class GameArchive : IDisposable
{
    private readonly byte[] Index;
    private readonly FileStream Data;
    public int EntryCount => Index.Length / 32;

    public GameArchive(string folder)
    {
        Index = File.ReadAllBytes(Path.Combine(folder, "LI_C0_R.IDX"));
        if (Index.Length == 0 || Index.Length % 32 != 0)
            throw new InvalidDataException("The game archive index is invalid.");
        Data = File.OpenRead(Path.Combine(folder, "LD_C0_R.BIN"));
    }

    public byte[] Read(int index)
    {
        if ((uint)index >= Index.Length / 32)
            throw new InvalidDataException("The game archive version is not supported.");
        var entry = Index.AsSpan(index * 32, 32);
        var offset = ReadInt64LittleEndian(entry);
        var rawSize = ReadInt64LittleEndian(entry[8..]);
        var packedSize = ReadInt64LittleEndian(entry[16..]);
        var flag = ReadInt64LittleEndian(entry[24..]);
        var size = flag == 0 ? rawSize : packedSize;
        if (flag is not (0 or 1) || rawSize is <= 0 or > 16_000_000 || size <= 0 || size > 16_000_000 ||
            offset < 0 || offset > Data.Length - size)
            throw new InvalidDataException("The game archive entry is invalid.");
        var data = new byte[(int)size];
        Data.Position = offset;
        Data.ReadExactly(data);
        if (flag == 0)
            return data;
        if (data.Length < 12 || ReadInt32LittleEndian(data) != 0x10000 || ReadInt32LittleEndian(data.AsSpan(8)) != rawSize)
            throw new InvalidDataException("The compressed archive header is invalid.");
        var count = ReadInt32LittleEndian(data.AsSpan(4));
        if (count <= 0 || count != (rawSize + 0xFFFF) / 0x10000)
            throw new InvalidDataException("The archive block count is invalid.");
        var position = (12 + (count * 4) + 127) & ~127;
        var result = new byte[(int)rawSize];
        var written = 0;
        for (int i = 0; i < count; i++)
        {
            if (position > data.Length - 4)
                throw new InvalidDataException("The archive block is truncated.");
            var length = ReadInt32LittleEndian(data.AsSpan(position));
            if (length <= 0 || length > data.Length - position - 4)
                throw new InvalidDataException("The archive block length is invalid.");
            using var input = new MemoryStream(data, position + 4, length, false);
            using var zlib = new ZLibStream(input, CompressionMode.Decompress);
            var expected = Math.Min(0x10000, result.Length - written);
            zlib.ReadExactly(result.AsSpan(written, expected));
            if (zlib.ReadByte() != -1)
                throw new InvalidDataException("The archive block size is invalid.");
            written += expected;
            position += (length + 4 + 127) & ~127;
        }
        return result;
    }

    public void Dispose() => Data.Dispose();
}
