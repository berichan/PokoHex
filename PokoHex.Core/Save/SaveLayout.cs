using System.IO;

namespace PokoHex.Core;

public sealed class SaveLayout
{
    public const int MainInventoryCount = 120;
    public static readonly SaveLayout Version70 = new(0x02000046, 0x3EF3DC, 0x3ECF92, 0x3ED7C1); // 1.0.4
    public static readonly SaveLayout Version381 = new(0x0200017D, 0x470CED, 0x46E0E5, 0x46ECEA); // 2.0.1

    public uint Version { get; }
    public int DataSize { get; }
    public int HeaderSize => 0x130;
    public int PlayerNameOffset { get; }
    public int PlayerNameSize => 25;
    public int PlayerNameCount => 9;
    public int InventoryOffset { get; }
    public int InventoryCount => 130;

    private SaveLayout(uint version, int dataSize, int playerNameOffset, int inventoryOffset)
    {
        Version = version;
        DataSize = dataSize;
        PlayerNameOffset = playerNameOffset;
        InventoryOffset = inventoryOffset;
    }

    internal static SaveLayout GetLayout(uint version) => version switch
    {
        0x02000046 => Version70,
        0x0200017D => Version381,
        _ => throw new InvalidDataException("This save version is not supported."),
    };
}
