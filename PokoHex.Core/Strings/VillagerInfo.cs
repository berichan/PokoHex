namespace PokoHex.Core;

public sealed record VillagerInfo(int Value, string Text)
{
    private static readonly ushort[] Portraits = ResourceUtil.GetBinaryResourceAsUshort("villager_portrait.bin");

    public static ushort GetPortrait(int index) => (uint)index < Portraits.Length ? Portraits[index] : (ushort)0;

    public string Display => $"{Text} ({Value:D4})";
}
