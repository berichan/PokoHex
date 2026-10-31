using System;
using System.IO;
using System.Linq;
using PokoHex.Core;
using PokoHex.Parsing;
using Xunit;

namespace PokoHex.Tests;

public static class DumpTests
{
    [Fact]
    public static void ReadItemTables()
    {
        var path = Environment.GetEnvironmentVariable("POKOHEX_GAME_PATH");
        Assert.SkipUnless(Directory.Exists(path), "Set POKOHEX_GAME_PATH to a game dump.");
        var items = GameItemDumper.ReadItems(path!);
        Assert.InRange(items.Length, 1000, 4600);
        Assert.Equal(items.Length, items.Select(z => z.Value).Distinct().Count());
        Assert.Contains(items, z => z.Value == 404 && z.Text.Length != 0);
        Assert.Contains(items, z => z.Value == 1419 && z.Text.Length != 0);
    }

    [Fact]
    public static void DumpItemResources()
    {
        var path = Environment.GetEnvironmentVariable("POKOHEX_GAME_PATH");
        var destination = Environment.GetEnvironmentVariable("POKOHEX_RESOURCE_PATH");
        Assert.SkipUnless(Directory.Exists(path), "Set POKOHEX_GAME_PATH to a game dump.");
        Assert.SkipUnless(!string.IsNullOrWhiteSpace(destination), "Set POKOHEX_RESOURCE_PATH to the resource output folder.");
        GameItemDumper.UpdateDumps(path!, destination!);
        for (int i = 0; i < GameLanguage.LanguageCount; i++)
        {
            var language = GameLanguage.Language2Char(i);
            var items = File.ReadAllLines(Path.Combine(destination!, "text", language, $"text_item_{language}.txt"));
            Assert.Equal(GameItemDumper.GetItemList(path!, language), items);
        }
    }

    [Fact]
    public static void ReadEmbeddedItems()
    {
        var path = Environment.GetEnvironmentVariable("POKOHEX_GAME_PATH");
        Assert.SkipUnless(Directory.Exists(path), "Set POKOHEX_GAME_PATH to a game dump.");
        for (int i = 0; i < GameLanguage.LanguageCount; i++)
        {
            var language = GameLanguage.Language2Char(i);
            var strings = GameInfo.GetStrings(language);
            Assert.Equal(GameItemDumper.GetItemList(path!, language), strings.itemlist);
            Assert.Equal(GameItemDumper.ReadItems(path!, language), strings.ItemDataSource);
        }
    }

    [Fact]
    public static void EditSaveCopy()
    {
        var path = Environment.GetEnvironmentVariable("POKOHEX_SAVE_PATH");
        Assert.SkipUnless(Directory.Exists(path), "Set POKOHEX_SAVE_PATH to a save folder.");
        var save = SaveFolder.Read(path!);
        var file = Path.Combine(save.Folder, "G00", "CMNDAT.BIN");
        var original = File.ReadAllBytes(file);
        Assert.Equal(original, save.Common.Write());
        var before = (byte[])save.Common.Data.Clone();
        var name = save.Common.PlayerName;
        Assert.NotEmpty(name);
        save.Common.PlayerName = "Editor Test";
        save.Common.Inventory[0].Set(404, 99);
        var written = save.Common.Write();
        var reopened = SaveFile.Read(written);
        Assert.Equal("Editor Test", reopened.PlayerName);
        Assert.Equal(404, reopened.Inventory[0].ItemId);
        Assert.Equal(99, reopened.Inventory[0].Count);
        Assert.Equal(save.Common.Data, reopened.Data);
        var layout = reopened.Layout;
        for (int i = 0; i < before.Length; i++)
        {
            if (i >= layout.PlayerNameOffset && i < layout.PlayerNameOffset + (layout.PlayerNameSize * layout.PlayerNameCount))
                continue;
            if (i >= layout.InventoryOffset && i < layout.InventoryOffset + Item.SIZE)
                continue;
            Assert.Equal(before[i], reopened.Data[i]);
        }
        Assert.Equal(original, File.ReadAllBytes(file));
    }
}
