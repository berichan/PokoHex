using System;

namespace PokoHex.Core;

public static class GameLanguage
{
    public const string DefaultLanguage = "en";
    private static readonly string[] LanguageCodes = ["en", "jp", "de", "es", "fr", "it", "ko", "zhs", "zht"];

    public static int LanguageCount => LanguageCodes.Length;
    public static int DefaultLanguageIndex => Array.IndexOf(LanguageCodes, DefaultLanguage);
    public static string Language2Char(int index) => (uint)index < LanguageCodes.Length ? LanguageCodes[index] : DefaultLanguage;

    public static int GetLanguageIndex(string language)
    {
        var index = Array.IndexOf(LanguageCodes, language);
        return index < 0 ? DefaultLanguageIndex : index;
    }

    public static string[] GetStrings(string name, string language)
    {
        var strings = ResourceUtil.GetStringList(name, language);
        return strings.Length == 0 ? ResourceUtil.GetStringList(name, DefaultLanguage) : strings;
    }
}
