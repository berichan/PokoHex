using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using PokoHex.Core;

namespace PokoHex;

public static class VillagerSprite
{
    private static readonly ResourceManager Resources = new("PokoHex.Properties.Resources", typeof(VillagerSprite).Assembly);
    private static readonly Dictionary<ushort, Image?> VillagerImages = new();

    public static Image? GetVillagerSprite(int index)
    {
        var portrait = VillagerInfo.GetPortrait(index);
        if (portrait == 0)
            return null;
        if (VillagerImages.TryGetValue(portrait, out var image))
            return image;
        image = Resources.GetObject($"villager_{portrait:0000}") as Image;
        VillagerImages.Add(portrait, image);
        return image;
    }
}
