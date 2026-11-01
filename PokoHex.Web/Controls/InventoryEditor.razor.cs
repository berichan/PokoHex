using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using PokoHex.Core;

namespace PokoHex.Web.Controls;

public partial class InventoryEditor
{
    [Parameter, EditorRequired] public SaveFile SAV { get; set; } = null!;
    [Parameter] public int Language { get; set; }
    [Parameter] public EventCallback Changed { get; set; }
    [Parameter, EditorRequired] public Action<string> Error { get; set; } = null!;
    private Task NotifyChanged() => Changed.InvokeAsync();

    private SaveFile? Loaded;
    private int SelectedSlot;
    private int InventoryStart;
    private ushort ItemId;
    private int Count;
    private int InventoryCount => InventoryStart == 0 ? SaveLayout.MainInventoryCount : SAV.Inventory.Length - InventoryStart;
    private ItemInfo[] Choices => GameInfo.Strings.ItemDataSource.Concat(SAV.Inventory.Select(z => z.ItemId).Append(ItemId).Distinct()
        .Where(id => !GameInfo.Strings.ItemDataSource.Any(z => z.Value == id)).Select(id => new ItemInfo(id, SpriteUtil.GetItemName(id))))
        .OrderBy(z => z.Value != 0).ThenBy(z => z.Text).ThenBy(z => z.Value).ToArray();
    public bool HasChanges => SAV.Inventory[SelectedSlot].ItemId != ItemId || SAV.Inventory[SelectedSlot].Count != Count;

    protected override void OnParametersSet()
    {
        if (Loaded == SAV)
            return;
        Loaded = SAV;
        SelectedSlot = InventoryStart = 0;
        LoadSlot();
    }

    private void LoadSlot()
    {
        var item = SAV.Inventory[SelectedSlot];
        ItemId = item.ItemId;
        Count = item.Count;
    }

    public void SaveChanges()
    {
        if (HasChanges)
            SAV.Inventory[SelectedSlot].Set(ItemId, Count);
    }

    private void SelectSlot(int slot)
    {
        try { SaveChanges(); SelectedSlot = slot; LoadSlot(); }
        catch (ArgumentException ex) { Error(ex.Message); }
    }

    private void CB_Inventory_Changed(ChangeEventArgs e)
    {
        try
        {
            SaveChanges();
            InventoryStart = e.Value?.ToString() == "120" ? SaveLayout.MainInventoryCount : 0;
            SelectedSlot = InventoryStart;
            LoadSlot();
        }
        catch (ArgumentException ex) { Error(ex.Message); }
    }

    private void CB_Item_Changed(ushort id)
    {
        ItemId = id;
        if (id == 0)
            Count = 0;
        else if (Count == 0)
            Count = 1;
    }

    private void B_Set_Click()
    {
        try { SaveChanges(); LoadSlot(); }
        catch (ArgumentException ex) { Error(ex.Message); }
    }

    private void B_Clear_Click()
    {
        SAV.Inventory[SelectedSlot].Set(0, 0);
        LoadSlot();
    }
}
