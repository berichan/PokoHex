using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using PokoHex.Core;

namespace PokoHex;

public static class ItemSprite
{
    private static readonly ResourceManager Resources = new("PokoHex.Properties.Resources", typeof(ItemSprite).Assembly);
    private static readonly Dictionary<ushort, Image?> ItemImages = new();

    public static Image? GetItemSprite(ushort id)
    {
        if (id == 0)
            return null;
        var icon = ItemInfo.GetMenuIcon(id);
        if (icon == 0)
            icon = 565;
        if (ItemImages.TryGetValue(icon, out var image))
            return image;
        image = Resources.GetObject($"item_{icon:0000}") as Image;
        ItemImages.Add(icon, image);
        return image;
    }
}
