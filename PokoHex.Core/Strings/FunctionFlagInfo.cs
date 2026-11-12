using System.Collections.Generic;
using System.Linq;

namespace PokoHex.Core;

public static class FunctionFlagInfo
{
    // These labels describe known behavior; the game stores numeric flag IDs.
    public static Dictionary<int, string> Names = new Dictionary<int, string>()
    {
        [54] = "Liquid absorption",
        [148] = "Dive unlocked",
        [56] = null,
        [57] = null,
        [71] = null,
        [72] =  null,
        [73] = null,
        [74] = null,
        [125] = null,
        [153] = null,
        [3] = "Workbench unlocked",
        [7] = "Poké Center PC unlocked",
        [27] = "PC: Challenges function unlocked",
        [28] = "PC: Link Play function unlocked",
        [30] = "PC: Shop function unlocked",
        [63] = "Rookie Packing Tips (Inventory upgrade 1)",
        [64] = "Intermediate Packing Tips (Inventory upgrade 2)",
        [65] = "Pro Packing Tips (Inventory upgrade 3)",
        [66] = "Expert Packing Tips (Inventory upgrade 4)",
        [76] = "Pause menu unlocked",
        [77] = "Smeargle: Pattern function unlocked (requires #79)",
        [78] = "Smeargle: Multi-Part Paint function unlocked (requires #79)",
        [79] = "Smeargle: Paint function unlocked",
        [82] = "Jump unlocked",
        [102] = "PP Up (increases max PP to 1333)",
        [103] = "PP Up (increases max PP to 1666)",
        [104] = "PP Up (increases max PP to 2000)",
        [105] = "PP Up (increases max PP to 2333)",
        [107] = "Handy Bag (hot bar) unlocked",
        [108] = "PC: Stamp function unlocked",
        [133] = "PC: Environment Level function unlocked",
    };
    public static bool IsKnown(int index) => Names.Keys.Contains(index);

    public static string GetName(int index) => index switch
    {
        56 => $"{GameInfo.Strings.skilllist[10]} unlocked",
        57 => $"{GameInfo.Strings.skilllist[11]} unlocked",
        71 => $"{GameInfo.Strings.skilllist[1]} food boost",
        72 => $"{GameInfo.Strings.skilllist[5]} food boost",
        73 => $"{GameInfo.Strings.skilllist[3]} food boost",
        74 => $"{GameInfo.Strings.skilllist[4]} food boost",
        125 => $"{GameInfo.Strings.skilllist[4]} extra food effect",
        153 => $"{GameInfo.Strings.skilllist[10]} food boost",

        _ => Names.TryGetValue(index, out var name) ? name : $"function_{index}",
    };
}