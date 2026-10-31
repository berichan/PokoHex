namespace PokoHex.Core;

public sealed record VillagerInfo(int Value, string Text)
{
    public string Display => $"{Text} ({Value:D4})";
}
