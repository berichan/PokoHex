namespace PokoHex.Core;

public sealed class SaveLayout
{
    public static readonly SaveLayout Version70 = new();

    public uint Version => 0x02000046;
    public int DataSize => 0x3EF3DC;
    public int HeaderSize => 0x130;
    public int PlayerNameOffset => 0x3ECF92;
    public int PlayerNameSize => 25;
    public int PlayerNameCount => 9;
    public int InventoryOffset => 0x3ED7C1;
    public int InventoryCount => 130;

    private SaveLayout() { }
}
