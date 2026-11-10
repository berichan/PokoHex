using PokoHex.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PokoHex;

public partial class Main : Form
{
    private SaveFolder? SAV;
    private ItemInfo[] Items = GameInfo.Strings.ItemDataSource;
    private bool Loading;
    private int SelectedSlot;
    private int InventoryStart;
    private ToolStripDropDown? ItemDropDown;
    private DataGridView? ItemGrid;
    private ComboBoxMouseHook? ItemMouseHook;

    public Main()
    {
        InitializeComponent();
        ItemMouseHook = new ComboBoxMouseHook(CB_Item, ShowItemPicker);
        DGV_Items.Columns["dataGridViewTextBoxColumn2"]!.SortMode = DataGridViewColumnSortMode.Programmatic;
        DGV_Items.ColumnHeaderMouseClick += DGV_Items_ColumnHeaderMouseClick;
        CB_Item.MouseDown += CB_Item_MouseDown;
        CB_Item.KeyDown += CB_Item_KeyDown;

        CB_Inventory.SelectedIndex = 0;
        Menu_Language.SelectedIndex = GameLanguage.GetLanguageIndex(GameInfo.CurrentLanguage);
        foreach (var arg in Environment.GetCommandLineArgs().AsSpan(1))
        {
            if (Directory.Exists(arg) || File.Exists(arg))
                TryOpen(arg);
        }
    }

    private bool HasChanges => SAV != null && (SAV.IsEdited || TB_Name.Text != SAV.Common.PlayerName || HasSlotChanges || Editor_Villagers.HasChanges || Editor_Map.HasChanges || Editor_Skills.HasChanges || Editor_Reactions.HasChanges || Editor_Flags.HasChanges);

    private bool HasSlotChanges
    {
        get
        {
            if (SAV == null)
                return false;

            ushort itemId;

            if (chk_CustomItem.Checked)
            {
                itemId = (ushort)NUD_CustomItemID.Value;
            }
            else if (CB_Item.SelectedItem is ItemInfo item)
            {
                itemId = item.Value;
            }
            else
            {
                return false;
            }

            var slot = SAV.Common.Inventory[SelectedSlot];

            if (slot.ItemId != itemId || slot.Count != NUD_Count.Value)
                return true;

            if (chkPaint.Checked)
                return true;

            if (chkPattern.Checked)
                return true;

            return false;
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
            CB_Inventory.SelectedIndex = 0;
            InventoryStart = 0;
            TC_Editor.Enabled = true;
            Menu_Export.Enabled = true;
            SelectedSlot = 0;
            Loading = false;
            BindItems();
            LoadInventory();
            Editor_Villagers.LoadSave(save.Common);
            Editor_Map.LoadSave(save);
            Editor_Skills.LoadSave(save.Common);
            Editor_Reactions.LoadSave(save.Common);
            Editor_Flags.LoadSave(save.Common);
            RB_Skills.Checked = true;
            Text = $"PokoHex / {Path.GetFileName(save.Folder)}";
            UpdateStatus();
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or ArgumentException or InvalidOperationException)
        {
            ShowError(ex.Message);
        }
        finally { Loading = false; }
    }

    private string GetItemName(ushort id) => Items.FirstOrDefault(z => z.Value == id)?.Text ?? (id == 0 ? "(Empty)" : $"Item {id:D4}");

    private void RB_PlayerSection_CheckedChanged(object sender, EventArgs e)
    {
        Editor_Skills.Visible = RB_Skills.Checked;
        Editor_Reactions.Visible = RB_Reactions.Checked;
    }

    private static string GetItemExtras(Item item)
    {
        var extras = new List<string>();

        if (item.HasItemPattern)
            extras.Add($"Pattern: {item.ItemPattern:000}");

        if (item.ItemPaint1 != 0)
            extras.Add(Item.ItemPaints.TryGetValue(item.ItemPaint1, out var p1)
                ? $"Paint 1: {p1.Item1}"
                : $"Paint 1: Unknown ({item.ItemPaint1})");

        if (item.ItemPaint2 != 0)
            extras.Add(Item.ItemPaints.TryGetValue(item.ItemPaint2, out var p2)
                ? $"Paint 2: {p2.Item1}"
                : $"Paint 2: Unknown ({item.ItemPaint2})");

        if (item.ItemPaint3 != 0)
            extras.Add(Item.ItemPaints.TryGetValue(item.ItemPaint3, out var p3)
                ? $"Paint 3: {p3.Item1}"
                : $"Paint 3: Unknown ({item.ItemPaint3})");

        return string.Join(Environment.NewLine, extras);
    }

    private void LoadInventory()
    {
        if (SAV == null)
            return;
        Loading = true;
        try
        {
            DGV_Items.Rows.Clear();
            var count = InventoryStart == 0 ? SaveLayout.MainInventoryCount : SAV.Common.Inventory.Length - InventoryStart;
            for (int i = 0; i < count; i++)
            {
                var item = SAV.Common.Inventory[InventoryStart + i];

                int row = DGV_Items.Rows.Add(i + 1, ItemSprite.GetItemSprite(item.ItemId)!, GetItemName(item.ItemId), item.Count, GetItemExtras(item));
                DGV_Items.Rows[row].Tag = InventoryStart + i;
            }
            SelectedSlot = InventoryStart;
            DGV_Items.CurrentCell = DGV_Items.Rows[0].Cells[2];
        }
        finally { Loading = false; }
        LoadSlot();
    }

    private void CB_Inventory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading || SAV == null || CB_Inventory.SelectedIndex < 0)
            return;
        try
        {
            SaveSlot();
            InventoryStart = CB_Inventory.SelectedIndex == 0 ? 0 : SaveLayout.MainInventoryCount;
            LoadInventory();
        }
        catch (ArgumentException ex)
        {
            Loading = true;
            CB_Inventory.SelectedIndex = InventoryStart == 0 ? 0 : 1;
            Loading = false;
            ShowError(ex.Message);
        }
    }

    private void Menu_Language_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((uint)Menu_Language.SelectedIndex >= GameLanguage.LanguageCount)
            return;
        var itemId = (CB_Item.SelectedItem as ItemInfo)?.Value ?? 0;
        var count = NUD_Count.Value;
        GameInfo.SetLanguage2Char(Menu_Language.SelectedIndex);
        Editor_Villagers.RefreshLanguage();
        Editor_Map.RefreshLanguage();
        Editor_Skills.RefreshLanguage();
        Editor_Reactions.RefreshLanguage();
        Editor_Flags.RefreshLanguage();
        Items = GameInfo.Strings.ItemDataSource;
        BindItems();
        Loading = true;
        try
        {
            if (SAV != null)
            {
                foreach (DataGridViewRow row in DGV_Items.Rows)
                {
                    int index = (int)row.Tag!;
                    row.Cells[2].Value = GetItemName(SAV.Common.Inventory[index].ItemId);
                }
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
            SelectedSlot = (int)DGV_Items.CurrentRow.Tag!;
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
        if (!item.HasCustomId)
            CB_Item.SelectedValue = item.ItemId;
        NUD_Count.Value = item.Count;
        NUD_CustomItemID.Value = item.ItemId;
        if (item.HasItemPattern)
            NUD_Pattern.Value = item.ItemPattern;
        else
            NUD_Paint1.Value = item.ItemPaint1;
        NUD_Paint2.Value = item.ItemPaint2;
        NUD_Paint3.Value = item.ItemPaint3;
        
        chkPaint.Checked = chkPattern.Checked = false;
        chk_CustomItem.Checked = item.HasCustomId;
        L_Slot.Text = SelectedSlot < SaveLayout.MainInventoryCount
            ? $"Slot {SelectedSlot + 1} of {SaveLayout.MainInventoryCount}"
            : $"Secondary bar slot {SelectedSlot + 1 - SaveLayout.MainInventoryCount} of {SAV.Common.Inventory.Length - SaveLayout.MainInventoryCount}";
        Loading = false;
    }

    private void SaveSlot()
    {
        if (Loading || SAV == null)
            return;

        if (chkPaint.Checked)
        {
            SAV.Common.Inventory[SelectedSlot].SetPaint((ushort)(chkPattern.Checked ? 0 : NUD_Paint1.Value), (ushort)NUD_Paint2.Value, (ushort)NUD_Paint3.Value);
        }
        if (chkPattern.Checked)
        {
            SAV.Common.Inventory[SelectedSlot].SetPattern((ushort)NUD_Pattern.Value);
        }

        if (chk_CustomItem.Checked)
        {
            SAV.Common.Inventory[SelectedSlot].Set((ushort)NUD_CustomItemID.Value, (int)NUD_Count.Value, true);
        }
        else
        {
            if (CB_Item.SelectedItem is not ItemInfo item)
                return;
            if (CB_Item.Text != item.Display)
                throw new ArgumentException("Select an item from the list.");
            if (!HasSlotChanges)
                return;
            SAV.Common.Inventory[SelectedSlot].Set(item.Value, (int)NUD_Count.Value);
        }
        RefreshSlot();
    }

    private void RefreshSlot()
    {
        if (SAV == null)
            return;

        var row = DGV_Items.Rows.Cast<DataGridViewRow>()
            .First(z => (int)z.Tag! == SelectedSlot);

        var item = SAV.Common.Inventory[SelectedSlot];
        row.Cells[1].Value = ItemSprite.GetItemSprite(item.ItemId);
        row.Cells[2].Value = GetItemName(item.ItemId);
        row.Cells[3].Value = item.Count;
        row.Cells[4].Value = GetItemExtras(item);
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

    private void CB_Item_DrawItem(object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index < 0 || CB_Item.Items[e.Index] is not ItemInfo item)
            return;
        var size = e.Bounds.Height - 4;
        var image = ItemSprite.GetItemSprite(item.Value);
        if (image != null)
            e.Graphics.DrawImage(image, new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, size, size));
        var bounds = new Rectangle(e.Bounds.Left + size + 6, e.Bounds.Top, e.Bounds.Width - size - 6, e.Bounds.Height);
        TextRenderer.DrawText(e.Graphics, item.Display, e.Font ?? CB_Item.Font, bounds, e.ForeColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        e.DrawFocusRectangle();
    }

    private void Menu_Export_Click(object sender, EventArgs e)
    {
        if (SAV == null)
            return;
        try
        {
            ValidateChildren();
            SaveSlot();
            Editor_Villagers.SaveChanges();
            Editor_Map.SaveChanges();
            Editor_Skills.SaveChanges();
            Editor_Reactions.SaveChanges();
            Editor_Flags.SaveChanges();
            if (TB_Name.Text != SAV.Common.PlayerName)
                SAV.Common.PlayerName = TB_Name.Text;
            using var dialog = new FolderBrowserDialog { Description = "Select a new, empty folder for the edited save", UseDescriptionForTitle = true };
            if (dialog.ShowDialog() != DialogResult.OK)
                return;
            SAV.Export(dialog.SelectedPath);
            SAV = SaveFolder.Read(dialog.SelectedPath);
            Editor_Villagers.LoadSave(SAV.Common);
            Editor_Map.LoadSave(SAV);
            Editor_Skills.LoadSave(SAV.Common);
            Editor_Reactions.LoadSave(SAV.Common);
            Editor_Flags.LoadSave(SAV.Common);
            Text = $"PokoHex / {Path.GetFileName(SAV.Folder)}";
            LoadSlot();
            L_Status.Text = $"Exported to {dialog.SelectedPath}";
            MessageBox.Show(this, "The edited save was exported and verified.", "PokoHex", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or ArgumentException or InvalidOperationException)
        {
            ShowError(ex.Message);
        }
    }

    private void TC_Editor_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading || SAV == null)
            return;
        Editor_Flags.RefreshValues();
        Editor_Skills.RefreshValues();
        Editor_Reactions.RefreshValues();
    }

    private void UpdateStatus()
    {
        L_Status.Text = SAV == null ? "Open a save folder to begin." : $"Loaded: {SAV.Common.PlayerName}.";
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

    private void DGV_Items_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        foreach (DataGridViewColumn col in DGV_Items.Columns)
        {
            if (col.Index != e.ColumnIndex)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
        }

        var column = DGV_Items.Columns[e.ColumnIndex];

        SortOrder order = SortOrder.None;
        if (column.SortMode != DataGridViewColumnSortMode.Programmatic)
            return;

        order = column.HeaderCell.SortGlyphDirection == SortOrder.Ascending
            ? SortOrder.Descending
            : SortOrder.Ascending;

        DGV_Items.Sort(new EmptyLastComparer(e.ColumnIndex, order));

        column.HeaderCell.SortGlyphDirection = order;
        DGV_Items.Invalidate();
    }
    private void CB_Item_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            ShowItemPicker();
    }

    private void CB_Item_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter ||
            e.KeyCode == Keys.Down)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            ShowItemPicker();
        }
    }

    private void ShowItemPicker()
    {
        if (CB_Item.Items.Count == 0)
            return;

        if (ItemDropDown == null)
        {
            ItemGrid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                RowHeadersVisible = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.None,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            ItemGrid.Columns.Add(new DataGridViewImageColumn
            {
                Name = "ItemIcon",
                HeaderText = "",
                FillWeight = 10,
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                SortMode = DataGridViewColumnSortMode.NotSortable
            });

            ItemGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ItemName",
                HeaderText = "Item",
                FillWeight = 85,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            ItemGrid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ItemID",
                HeaderText = "Item ID",
                FillWeight = 25,
                SortMode = DataGridViewColumnSortMode.Automatic
            });

            ItemGrid.CellClick += ItemGrid_CellClick;

            var host = new ToolStripControlHost(ItemGrid)
            {
                AutoSize = false,
                Size = new Size(Math.Max(400, CB_Item.Width), ((split.Panel2.Height - CB_Item.Location.Y - CB_Item.Height) / ItemGrid.RowTemplate.Height) * ItemGrid.RowTemplate.Height),
                Padding = Padding.Empty,
                Margin = Padding.Empty
            };

            ItemDropDown = new ToolStripDropDown
            {
                Padding = Padding.Empty
            };

            ItemDropDown.Items.Add(host);
        }

        ItemGrid!.Rows.Clear();

        foreach (ItemInfo item in CB_Item.Items)
        {
            if (item.Value == 0)
                continue;

            int row = ItemGrid.Rows.Add(
                ItemSprite.GetItemSprite(item.Value)!,
                item.Text,
                (int)item.Value
            );

            ItemGrid.Rows[row].Tag = item;
        }

        ItemGrid.ClearSelection();

        if (CB_Item.SelectedItem is ItemInfo selected)
        {
            foreach (DataGridViewRow row in ItemGrid.Rows)
            {
                if (row.Tag is ItemInfo item && item.Value == selected.Value)
                {
                    row.Selected = true;
                    ItemGrid.CurrentCell = row.Cells[0];
                    break;
                }
            }
        }
        ItemGrid.FirstDisplayedScrollingRowIndex = 0;
        ItemGrid.ClearSelection();
        ItemGrid.CurrentCell = null;

        ItemDropDown!.Show(CB_Item, new Point(0, CB_Item.Height));
    }

    private void ItemGrid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || ItemGrid!.Rows[e.RowIndex].Tag is not ItemInfo item)
            return;

        CB_Item.SelectedValue = item.Value;
        ItemDropDown!.Close();
    }

    private void Main_ResizeEnd(object sender, EventArgs e)
    {
        ItemDropDown = null;
    }

    private void chk_CustomItem_CheckedChanged(object sender, EventArgs e)
    {
        NUD_CustomItemID.Enabled = chk_CustomItem.Checked;
        CB_Item.Enabled = !chk_CustomItem.Checked;
    }

    private void chkPaint_CheckedChanged(object sender, EventArgs e)
    {
        NUD_Paint1.Enabled = NUD_Paint2.Enabled = NUD_Paint3.Enabled = chkPaint.Checked;
    }

    private void chkPattern_CheckedChanged(object sender, EventArgs e)
    {
        NUD_Pattern.Enabled = chkPattern.Checked;
    }
}

class EmptyLastComparer : System.Collections.IComparer
{
    private readonly int columnIndex;
    private readonly SortOrder sortOrder;

    public EmptyLastComparer(int columnIndex, SortOrder sortOrder)
    {
        this.columnIndex = columnIndex;
        this.sortOrder = sortOrder;
    }

    public int Compare(object? x, object? y)
    {
        var row1 = x as DataGridViewRow;
        var row2 = y as DataGridViewRow;

        string value1 = row1!.Cells[columnIndex].Value?.ToString() ?? "";
        string value2 = row2!.Cells[columnIndex].Value?.ToString() ?? "";

        bool empty1 = value1 == "(Empty)";
        bool empty2 = value2 == "(Empty)";

        // Always put (Empty) entries last, regardless of sort direction.
        if (empty1 && empty2) return 0;
        if (empty1) return 1;
        if (empty2) return -1;

        int result = string.Compare(
            value1,
            value2,
            StringComparison.CurrentCultureIgnoreCase
        );

        return sortOrder == SortOrder.Descending ? -result : result;
    }
}

class ComboBoxMouseHook : NativeWindow
{
    private readonly ComboBox Combo;
    private readonly Action ShowPicker;

    private const int WM_LBUTTONDOWN = 0x0201;
    private const int WM_LBUTTONDBLCLK = 0x0203;

    public ComboBoxMouseHook(ComboBox combo, Action showPicker)
    {
        Combo = combo;
        ShowPicker = showPicker;

        AssignHandle(combo.Handle);

        combo.HandleCreated += Combo_HandleCreated;
        combo.HandleDestroyed += Combo_HandleDestroyed;
    }

    private void Combo_HandleCreated(object? sender, EventArgs e)
    {
        AssignHandle(Combo.Handle);
    }

    private void Combo_HandleDestroyed(object? sender, EventArgs e)
    {
        ReleaseHandle();
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_LBUTTONDOWN || m.Msg == WM_LBUTTONDBLCLK)
        {
            Combo.Focus();
            ShowPicker();
            return;
        }

        base.WndProc(ref m);
    }
}