using System.Collections.Generic;

namespace PokoHex.Core;

public sealed class GameStrings
{
    public readonly string[] itemlist;
    public readonly ItemInfo[] ItemDataSource;

    public GameStrings(string language)
    {
        itemlist = GameLanguage.GetStrings("item", language);
        var items = new List<ItemInfo>();
        for (int i = 0; i < itemlist.Length; i++)
        {
            if (itemlist[i].Length != 0)
                items.Add(new ItemInfo(checked((ushort)i), itemlist[i]));
        }
        ItemDataSource = items.ToArray();
    }
}
