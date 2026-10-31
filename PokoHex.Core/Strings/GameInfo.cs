namespace PokoHex.Core;

public static class GameInfo
{
    private static readonly GameStrings?[] Languages = new GameStrings[GameLanguage.LanguageCount];

    public static string CurrentLanguage { get; private set; } = GameLanguage.DefaultLanguage;
    public static GameStrings Strings { get; private set; } = GetStrings(CurrentLanguage);

    public static GameStrings GetStrings(string language)
    {
        var index = GameLanguage.GetLanguageIndex(language);
        return Languages[index] ??= new GameStrings(GameLanguage.Language2Char(index));
    }

    public static void SetLanguage2Char(int index)
    {
        CurrentLanguage = GameLanguage.Language2Char(index);
        Strings = GetStrings(CurrentLanguage);
    }
}
