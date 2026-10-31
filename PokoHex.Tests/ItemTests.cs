using System;
using System.Linq;
using PokoHex.Core;
using Xunit;

namespace PokoHex.Tests;

public static class ItemTests
{
    [Fact]
    public static void ReadEmbeddedCatalog()
    {
        var ids = GameInfo.GetStrings("en").ItemDataSource.Select(z => z.Value).ToArray();
        for (int i = 0; i < GameLanguage.LanguageCount; i++)
        {
            var language = GameLanguage.Language2Char(i);
            var strings = GameInfo.GetStrings(language);
            Assert.Equal(4600, ResourceUtil.GetStringList("item", language).Length);
            Assert.Equal(4600, strings.itemlist.Length);
            Assert.Equal(3586, strings.ItemDataSource.Length);
            Assert.Equal(ids, strings.ItemDataSource.Select(z => z.Value).ToArray());
            Assert.Contains(strings.ItemDataSource, z => z.Value == 404 && z.Text.Length != 0);
            Assert.Contains(strings.ItemDataSource, z => z.Value == 1419 && z.Text.Length != 0);
        }
    }

    [Fact]
    public static void PreserveVariantAndClearSlot()
    {
        var item = new Item(new byte[Item.SIZE]) { RawValue = 0x80000194, Count = 2 };
        item.Set(404, 255);
        Assert.Equal(0x80000194u, item.RawValue);
        Assert.Equal(255, item.Count);
        item.Set(1419, 1);
        Assert.Equal(1419u, item.RawValue);
        Assert.Throws<ArgumentOutOfRangeException>(() => item.Set(1419, 256));
        item.Set(1419, 0);
        Assert.Equal(0u, item.RawValue);
        Assert.Equal(0, item.Count);
    }
}
