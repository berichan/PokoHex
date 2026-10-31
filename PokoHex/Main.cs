using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PokoHex.Core;

namespace PokoHex;

public partial class Main : Form
{
    private SaveFolder? SAV;
    private ItemInfo[] Items = GameInfo.Strings.ItemDataSource;
    private bool Loading;
    private int SelectedSlot;

    public Main()
    {
        InitializeComponent();
        Menu_Language.SelectedIndex = GameLanguage.GetLanguageIndex(GameInfo.CurrentLanguage);
        foreach (var arg in Environment.GetCommandLineArgs().AsSpan(1))
        {
            if (Directory.Exists(arg) || File.Exists(arg))
                TryOpen(arg);
        }
    }

    private bool HasChanges => SAV != null && (SAV.IsEdited || TB_Name.Text != SAV.Common.PlayerName || HasSlotChanges);

    private bool HasSlotChanges
    {
        get
        {
            if (SAV == null || CB_Item.SelectedItem is not ItemInfo item)
                return false;
            var slot = SAV.Common.Inventory[SelectedSlot];
            return slot.ItemId != item.Value || slot.Count != NUD_Count.Value;
        }
    }

    private void Menu_Open_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog { Description = "Select the save folder containing your G00 and S00 folders", UseDescriptionForTitle = true };
        if (dialog.ShowDialog() == DialogResult.OK)
            TryOpen(dialog.SelectedPath);
    }

    private void TryOpen(string path)
    {
        if (!ConfirmDiscard())
            return;
        try
        {
            var save = SaveFolder.Read(path);
            SAV = save;
            Loading = true;
            TB_Name.Text = save.Common.PlayerName;
            DGV_Items.Rows.Clear();
            for (int i = 0; i < save.Common.Inventory.Length; i++)
            {
                var item = save.Common.Inventory[i];
                DGV_Items.Rows.Add(i + 1, GetItemName(item.ItemId), item.Count);
            }
            TC_Editor.Enabled = true;
            Menu_Export.Enabled = true;
            SelectedSlot = 0;
            Loading = false;
            BindItems();
            LoadSlot();
            Text = $"PokoHex / {Path.GetFileName(save.Folder)}";
            UpdateStatus();
        }
        catch (Exception ex) when (ex is IOException or ArgumentException or InvalidOperationException)
        {
            ShowError(ex.Message);
        }
        finally { Loading = false; }
    }

    private string GetItemName(ushort id) => Items.FirstOrDefault(z => z.Value == id)?.Text ?? (id == 0 ? "(Empty)" : $"Item {id:D4}");

    private void Menu_Language_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((uint)Menu_Language.SelectedIndex >= GameLanguage.LanguageCount)
            return;
        var itemId = (CB_Item.SelectedItem as ItemInfo)?.Value ?? 0;
        var count = NUD_Count.Value;
        GameInfo.SetLanguage2Char(Menu_Language.SelectedIndex);
        Items = GameInfo.Strings.ItemDataSource;
        BindItems();
        Loading = true;
        try
        {
            if (SAV != null)
            {
                for (int i = 0; i < SAV.Common.Inventory.Length; i++)
                    DGV_Items.Rows[i].Cells[1].Value = GetItemName(SAV.Common.Inventory[i].ItemId);
            }
            CB_Item.SelectedValue = itemId;
            NUD_Count.Value = count;
            L_Catalog.Text = $"{Items.Length - 1:N0} items are available";
        }
        finally { Loading = false; }
    }

    private void BindItems()
    {
        Loading = true;
        var list = Items.ToList();
        if (list.Count == 0)
            list.Add(new ItemInfo(0, "(Empty)"));
        if (SAV != null)
        {
            foreach (var id in SAV.Common.Inventory.Select(z => z.ItemId).Distinct())
            {
                if (list.All(z => z.Value != id))
                    list.Add(new ItemInfo(id, GetItemName(id)));
            }
        }
        CB_Item.DataSource = list.OrderBy(z => z.Value != 0).ThenBy(z => z.Text).ThenBy(z => z.Value).ToArray();
        CB_Item.DisplayMember = nameof(ItemInfo.Display);
        CB_Item.ValueMember = nameof(ItemInfo.Value);
        B_Set.Enabled = Items.Length != 0;
        Loading = false;
    }

    private void DGV_Items_CurrentCellChanged(object sender, EventArgs e)
    {
        if (Loading || SAV == null || DGV_Items.CurrentRow == null)
            return;
        try
        {
            SaveSlot();
            SelectedSlot = DGV_Items.CurrentRow.Index;
            LoadSlot();
        }
        catch (ArgumentException ex) { ShowError(ex.Message); }
    }

    private void LoadSlot()
    {
        if (SAV == null)
            return;
        Loading = true;
        var item = SAV.Common.Inventory[SelectedSlot];
        CB_Item.SelectedValue = item.ItemId;
        NUD_Count.Value = item.Count;
        L_Slot.Text = $"Slot {SelectedSlot + 1} of {SAV.Common.Inventory.Length}";
        Loading = false;
    }

    private void SaveSlot()
    {
        if (Loading || SAV == null || CB_Item.SelectedItem is not ItemInfo item)
            return;
        if (CB_Item.Text != item.Display)
            throw new ArgumentException("Select an item from the list.");
        SAV.Common.Inventory[SelectedSlot].Set(item.Value, (int)NUD_Count.Value);
        RefreshSlot();
    }

    private void RefreshSlot()
    {
        if (SAV == null)
            return;
        var item = SAV.Common.Inventory[SelectedSlot];
        var row = DGV_Items.Rows[SelectedSlot];
        row.Cells[1].Value = GetItemName(item.ItemId);
        row.Cells[2].Value = item.Count;
        UpdateStatus();
    }

    private void B_Set_Click(object sender, EventArgs e)
    {
        try
        {
            SaveSlot();
            LoadSlot();
        }
        catch (ArgumentException ex) { ShowError(ex.Message); }
    }

    private void B_Clear_Click(object sender, EventArgs e)
    {
        if (SAV == null)
            return;
        SAV.Common.Inventory[SelectedSlot].Set(0, 0);
        RefreshSlot();
        LoadSlot();
    }

    private void CB_Item_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading || CB_Item.SelectedItem is not ItemInfo item)
            return;
        if (item.Value == 0)
            NUD_Count.Value = 0;
        else if (NUD_Count.Value == 0)
            NUD_Count.Value = 1;
    }

    private void Menu_Export_Click(object sender, EventArgs e)
    {
        if (SAV == null)
            return;
        try
        {
            ValidateChildren();
            SaveSlot();
            if (TB_Name.Text != SAV.Common.PlayerName)
                SAV.Common.PlayerName = TB_Name.Text;
            using var dialog = new FolderBrowserDialog { Description = "Select a new, empty folder for the edited save", UseDescriptionForTitle = true };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            SAV.Export(dialog.SelectedPath);
            SAV = SaveFolder.Read(dialog.SelectedPath);
            Text = $"PokoHex / {Path.GetFileName(SAV.Folder)}";
            LoadSlot();
            L_Status.Text = $"Exported to {dialog.SelectedPath}";
            MessageBox.Show(this, "The edited save was exported and verified.", "PokoHex", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is IOException or ArgumentException or InvalidOperationException)
        {
            ShowError(ex.Message);
        }
    }

    private void UpdateStatus()
    {
        L_Status.Text = SAV == null ? "Open a save folder to begin." : $"{SAV.Common.Inventory.Count(z => !z.IsEmpty)} / {SAV.Common.Inventory.Length} item slots are occupied. You can only export to a new, empty folder to keep your old saves safe!";
    }

    private bool ConfirmDiscard() => !HasChanges || MessageBox.Show(this, "Discard changes to the opened save?", "PokoHex", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    private void ShowError(string message) => MessageBox.Show(this, message, "PokoHex", MessageBoxButtons.OK, MessageBoxIcon.Error);
    private void Main_FormClosing(object sender, FormClosingEventArgs e) => e.Cancel = !ConfirmDiscard();
    private void Menu_Exit_Click(object sender, EventArgs e) => Close();

    private void Main_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            e.Effect = DragDropEffects.Copy;
    }

    private void Main_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is string[] { Length: > 0 } paths)
            TryOpen(paths[0]);
    }
}
