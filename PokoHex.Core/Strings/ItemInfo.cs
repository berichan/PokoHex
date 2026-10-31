namespace PokoHex.Core;

public sealed record ItemInfo(ushort Value, string Text)
{
    private static readonly ushort[] ItemMenuIcons = ResourceUtil.GetBinaryResourceAsUshort("item_menuicon.bin");

    public string Display => $"{Text} ({Value:D4})";

    public static ushort GetMenuIcon(ushort id) => id < ItemMenuIcons.Length ? ItemMenuIcons[id] : (ushort)0;
}
