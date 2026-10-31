namespace PokoHex.Core;

public sealed record ItemInfo(ushort Value, string Text)
{
    public string Display => $"{Text} ({Value:D4})";
}
