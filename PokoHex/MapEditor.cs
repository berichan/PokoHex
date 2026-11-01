using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PokoHex.Core;

namespace PokoHex;

public partial class MapEditor : UserControl
{
    private SaveFolder? SAV;
    private StageSave? Stage;
    private MapPart? SelectedPart;
    private bool Loading;
    private int SelectedArea = -1;
    private int Chunk;
    private sealed record MapEntry(MapPart Part, string Text);

    public MapEditor() => InitializeComponent();

    public bool HasChanges => Stage != null && SelectedPart != null && CB_Item.SelectedItem is ItemInfo item &&
        (item.Value != SelectedPart.Variation || NUD_X.Value != SelectedPart.X || NUD_Y.Value != SelectedPart.Y ||
         NUD_Z.Value != SelectedPart.Z || CB_Direction.SelectedIndex != SelectedPart.Direction);

    public void LoadSave(SaveFolder save)
    {
        SAV = save;
        Stage = null;
        SelectedPart = null;
        Loading = true;
        TB_Search.Clear();
        BindAreas();
        CB_Area.SelectedIndex = save.StageIds.Length == 0 ? -1 : 0;
        SelectedArea = CB_Area.SelectedIndex;
        Loading = false;
        LoadArea();
    }

    private static string GetAreaName(int area)
    {
        var names = GameInfo.Strings.arealist;
        return (uint)area < names.Length && names[area].Length != 0 ? $"{names[area]} ({area})" : $"Area {area}";
    }

    private static string GetItemName(ushort item)
    {
        var names = GameInfo.Strings.itemlist;
        return item != 0 && item < names.Length && names[item].Length != 0 ? names[item] : $"Item {item:D4}";
    }

    private void BindAreas()
    {
        CB_Area.Items.Clear();
        if (SAV != null)
        {
            foreach (var area in SAV.StageIds)
                CB_Area.Items.Add(GetAreaName(area));
        }
    }

    public void RefreshLanguage()
    {
        var area = CB_Area.SelectedIndex;
        var pending = (CB_Item.SelectedItem as ItemInfo)?.Value;
        var x = NUD_X.Value;
        var y = NUD_Y.Value;
        var z = NUD_Z.Value;
        var direction = CB_Direction.SelectedIndex;
        Loading = true;
        TB_Search.Clear();
        BindAreas();
        CB_Area.SelectedIndex = area;
        Loading = false;
        LoadParts(false);
        Loading = true;
        if (pending.HasValue)
            CB_Item.SelectedValue = pending.Value;
        NUD_X.Value = x;
        NUD_Y.Value = y;
        NUD_Z.Value = z;
        CB_Direction.SelectedIndex = direction;
        Loading = false;
        UpdatePreview();
    }

    private void LoadArea()
    {
        Stage = null;
        SelectedPart = null;
        Loading = true;
        CB_Chunk.Items.Clear();
        try
        {
            if (SAV != null && SelectedArea >= 0)
            {
                Stage = SAV.GetStage(SAV.StageIds[SelectedArea]);
                foreach (var chunk in Stage.Chunks)
                    CB_Chunk.Items.Add(new ItemInfo((ushort)chunk, $"X {(chunk & 63) * 32} / Z {(chunk >> 6) * 32}"));
                CB_Chunk.DisplayMember = nameof(ItemInfo.Text);
                CB_Chunk.SelectedIndex = 0;
                Chunk = ((ItemInfo)CB_Chunk.SelectedItem!).Value;
            }
            L_Count.Text = Stage == null ? "No areas are available." : $"{Stage.Parts.Length:N0} placed objects";
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or ArgumentException)
        {
            L_Count.Text = ex.Message;
        }
        finally { Loading = false; }
        CB_Chunk.Enabled = Stage != null;
        TB_Search.Enabled = Stage != null;
        LoadParts(true);
    }

    private void LoadParts(bool center)
    {
        Loading = true;
        var entries = Stage?.Parts.Select(z => new MapEntry(z,
                $"{GetItemName(z.Info.ItemId)} [{z.X}, {z.Y}, {z.Z}] (#{z.Index})"))
            .Where(z => z.Text.Contains(TB_Search.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(z => GetItemName(z.Part.Info.ItemId)).ThenBy(z => z.Part.Index).ToArray() ?? [];
        var selected = SelectedPart;
        LB_Parts.DataSource = entries;
        LB_Parts.DisplayMember = nameof(MapEntry.Text);
        LB_Parts.SelectedIndex = Array.FindIndex(entries, z => z.Part == selected);
        if (LB_Parts.SelectedIndex < 0 && entries.Length != 0)
        {
            var first = Array.FindIndex(entries, z => z.Part.Info.CanEdit);
            LB_Parts.SelectedIndex = first >= 0 ? first : 0;
        }
        SelectedPart = (LB_Parts.SelectedItem as MapEntry)?.Part;
        Loading = false;
        LoadPart(center);
    }

    private void LoadPart(bool center)
    {
        Loading = true;
        var part = SelectedPart;
        GB_Details.Enabled = part != null;
        if (part == null)
        {
            PB_Item.Image = null;
            L_Item.Text = L_Details.Text = string.Empty;
            CB_Item.DataSource = null;
        }
        else
        {
            var choices = MapPartInfo.All.ToArray().Where(z => z.CanEdit || z.Value == part.Variation)
                .Select(z => new ItemInfo(z.Value, GetItemName(z.ItemId))).OrderBy(z => z.Text).ThenBy(z => z.Value).ToArray();
            CB_Item.DataSource = choices;
            CB_Item.DisplayMember = nameof(ItemInfo.Display);
            CB_Item.ValueMember = nameof(ItemInfo.Value);
            CB_Item.SelectedValue = part.Variation;
            NUD_X.Value = part.X;
            NUD_Y.Value = part.Y;
            NUD_Z.Value = part.Z;
            CB_Direction.SelectedIndex = part.Direction;
            CB_Item.Enabled = NUD_X.Enabled = NUD_Y.Enabled = NUD_Z.Enabled = CB_Direction.Enabled = part.Info.CanEdit;
            if (center)
            {
                CB_Chunk.SelectedIndex = Array.FindIndex(Stage!.Chunks, z => z == part.Chunk);
                Chunk = part.Chunk;
                NUD_Height.Value = part.Y;
            }
        }
        Loading = false;
        UpdatePreview();
    }

    public void SaveChanges()
    {
        if (Loading || Stage == null || SelectedPart == null || !HasChanges || CB_Item.SelectedItem is not ItemInfo item)
            return;
        Stage.SetPart(SelectedPart, item.Value, (int)NUD_X.Value, (int)NUD_Y.Value, (int)NUD_Z.Value, CB_Direction.SelectedIndex);
    }

    private bool TrySaveChanges()
    {
        try { SaveChanges(); return true; }
        catch (ArgumentException ex)
        {
            MessageBox.Show(this, ex.Message, "Map", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    private void UpdatePreview()
    {
        B_Undo.Enabled = Stage?.CanUndo == true;
        B_Redo.Enabled = Stage?.CanRedo == true;
        B_Set.Enabled = false;
        if (SelectedPart != null && Stage != null && CB_Item.SelectedItem is ItemInfo choice)
        {
            var info = MapPartInfo.Get(choice.Value);
            var bounds = info.GetBounds((int)NUD_X.Value, (int)NUD_Y.Value, (int)NUD_Z.Value, CB_Direction.SelectedIndex);
            PB_Item.Image = ItemSprite.GetItemSprite(info.ItemId);
            L_Item.Text = GetItemName(info.ItemId);
            L_Details.Text = $"Item {info.ItemId:D4} / Part {info.Value:D4}\r\nSize: {bounds.Width} X, {bounds.Height} Y, {bounds.Depth} Z";
            var error = Stage.GetEditError(SelectedPart, choice.Value, (int)NUD_X.Value, (int)NUD_Y.Value, (int)NUD_Z.Value, CB_Direction.SelectedIndex);
            L_EditStatus.Text = error ?? (HasChanges ? "Press Set to place." : "Right-click a map cell to position this object, then press Set.");
            L_EditStatus.ForeColor = error == null ? SystemColors.ControlText : SystemColors.GrayText;
            B_Set.Enabled = error == null && HasChanges;
        }
        else
            L_EditStatus.Text = string.Empty;
        PB_Map.Invalidate();
    }

    private void CB_Area_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading)
            return;
        if (!TrySaveChanges())
        {
            Loading = true;
            CB_Area.SelectedIndex = SelectedArea;
            Loading = false;
            return;
        }
        SelectedArea = CB_Area.SelectedIndex;
        LoadArea();
    }

    private void LB_Parts_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading)
            return;
        if (!TrySaveChanges())
        {
            Loading = true;
            LB_Parts.SelectedIndex = Array.FindIndex((MapEntry[])LB_Parts.DataSource!, z => z.Part == SelectedPart);
            Loading = false;
            return;
        }
        SelectedPart = (LB_Parts.SelectedItem as MapEntry)?.Part;
        LoadPart(true);
    }

    private void TB_Search_TextChanged(object sender, EventArgs e)
    {
        if (!Loading && TrySaveChanges())
            LoadParts(true);
    }

    private void View_Changed(object sender, EventArgs e)
    {
        if (Loading)
            return;
        if (CB_Chunk.SelectedItem is ItemInfo chunk)
            Chunk = chunk.Value;
        PB_Map.Invalidate();
    }

    private void Placement_Changed(object sender, EventArgs e)
    {
        if (!Loading)
            UpdatePreview();
    }

    private void B_Set_Click(object sender, EventArgs e)
    {
        ValidateChildren();
        if (TrySaveChanges())
            LoadParts(false);
    }

    private void B_Undo_Click(object sender, EventArgs e)
    {
        Stage?.UndoEdit();
        LoadParts(false);
    }

    private void B_Redo_Click(object sender, EventArgs e)
    {
        Stage?.RedoEdit();
        LoadParts(false);
    }

    private void LB_Parts_DrawItem(object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index < 0 || LB_Parts.Items[e.Index] is not MapEntry entry)
            return;
        var image = ItemSprite.GetItemSprite(entry.Part.Info.ItemId);
        var size = e.Bounds.Height - 4;
        if (image != null)
            e.Graphics.DrawImage(image, new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, size, size));
        var bounds = new Rectangle(e.Bounds.Left + size + 6, e.Bounds.Top, e.Bounds.Width - size - 6, e.Bounds.Height);
        TextRenderer.DrawText(e.Graphics, entry.Text, e.Font ?? LB_Parts.Font, bounds, e.ForeColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        e.DrawFocusRectangle();
    }

    private void CB_Item_DrawItem(object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        if (e.Index < 0 || CB_Item.Items[e.Index] is not ItemInfo item)
            return;
        var image = ItemSprite.GetItemSprite(MapPartInfo.Get(item.Value).ItemId);
        var size = e.Bounds.Height - 4;
        if (image != null)
            e.Graphics.DrawImage(image, new Rectangle(e.Bounds.Left + 2, e.Bounds.Top + 2, size, size));
        var bounds = new Rectangle(e.Bounds.Left + size + 6, e.Bounds.Top, e.Bounds.Width - size - 6, e.Bounds.Height);
        TextRenderer.DrawText(e.Graphics, item.Display, e.Font ?? CB_Item.Font, bounds, e.ForeColor,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        e.DrawFocusRectangle();
    }

    private int CellSize => Math.Max(1, Math.Min(PB_Map.Width - 1, PB_Map.Height - 1) / 32);
    private Rectangle MapRectangle => new((PB_Map.Width - CellSize * 32) / 2, (PB_Map.Height - CellSize * 32) / 2, CellSize * 32, CellSize * 32);

    private Rectangle GetRectangle(MapBounds bounds)
    {
        var map = MapRectangle;
        return new Rectangle(map.X + (bounds.X - (Chunk & 63) * 32) * CellSize,
            map.Y + (bounds.Z - (Chunk >> 6) * 32) * CellSize, bounds.Width * CellSize, bounds.Depth * CellSize);
    }

    private void PB_Map_Paint(object sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(SystemColors.AppWorkspace);
        if (Stage == null)
            return;
        var map = MapRectangle;
        g.SetClip(map);
        g.FillRectangle(Brushes.White, map);
        using var ground = new SolidBrush(Color.FromArgb(225, 229, 222));
        var x0 = (Chunk & 63) * 32;
        var z0 = (Chunk >> 6) * 32;
        var layer = (int)NUD_Height.Value;
        for (int z = 0; z < 32; z++)
        for (int x = 0; x < 32; x++)
        {
            if (Stage.GetCell(x0 + x, layer, z0 + z) != 0)
                g.FillRectangle(ground, map.X + x * CellSize, map.Y + z * CellSize, CellSize, CellSize);
        }
        using var grid = new Pen(Color.FromArgb(210, 210, 210));
        for (int i = 0; i <= 32; i++)
        {
            g.DrawLine(grid, map.X + i * CellSize, map.Top, map.X + i * CellSize, map.Bottom);
            g.DrawLine(grid, map.Left, map.Y + i * CellSize, map.Right, map.Y + i * CellSize);
        }
        var view = new MapBounds(x0, 0, z0, 32, 128, 32);
        foreach (var part in Stage.Parts.OrderBy(z => z.Y).ThenBy(z => z.Index))
        {
            var bounds = part.Bounds;
            if (bounds.Width == 0 || bounds.Height == 0 || bounds.Depth == 0)
                bounds = new MapBounds(part.X, part.Y, part.Z, 1, 1, 1);
            if (!bounds.Intersects(view) || !CHK_AllHeights.Checked && (layer < bounds.Y || layer >= bounds.Y + bounds.Height))
                continue;
            var rect = GetRectangle(bounds);
            using var fill = new SolidBrush(Color.FromArgb(110, part.Info.CanEdit ? Color.LightSteelBlue : Color.LightGray));
            g.FillRectangle(fill, rect);
            g.DrawRectangle(Pens.Gray, rect);
            var image = ItemSprite.GetItemSprite(part.Info.ItemId);
            if (image != null)
            {
                var size = Math.Min(36, Math.Min(rect.Width, rect.Height));
                g.DrawImage(image, new Rectangle(rect.X + (rect.Width - size) / 2, rect.Y + (rect.Height - size) / 2, size, size));
            }
        }
        if (SelectedPart != null)
        {
            using var selected = new Pen(Color.RoyalBlue, 3);
            g.DrawRectangle(selected, GetRectangle(SelectedPart.Bounds));
            if (HasChanges && CB_Item.SelectedItem is ItemInfo choice && CB_Direction.SelectedIndex >= 0)
            {
                var bounds = MapPartInfo.Get(choice.Value).GetBounds((int)NUD_X.Value, (int)NUD_Y.Value, (int)NUD_Z.Value, CB_Direction.SelectedIndex);
                using var preview = new Pen(B_Set.Enabled ? Color.ForestGreen : Color.Firebrick, 3) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };
                g.DrawRectangle(preview, GetRectangle(bounds));
            }
        }
        g.ResetClip();
        g.DrawRectangle(Pens.Black, map);
    }

    private bool GetMapPosition(Point location, out int x, out int z)
    {
        var map = MapRectangle;
        x = (location.X - map.X) / CellSize + (Chunk & 63) * 32;
        z = (location.Y - map.Y) / CellSize + (Chunk >> 6) * 32;
        return Stage != null && map.Contains(location);
    }

    private void PB_Map_MouseMove(object sender, MouseEventArgs e)
    {
        if (GetMapPosition(e.Location, out var x, out var z))
            L_Coordinates.Text = $"X: {x}  Y: {NUD_Height.Value}  Z: {z}     Click to select; right-click to position. Blue: selected. Green/red: pending footprint.";
    }

    private void PB_Map_MouseDown(object sender, MouseEventArgs e)
    {
        if (!GetMapPosition(e.Location, out var x, out var z) || Stage == null)
            return;
        if (e.Button == MouseButtons.Right && SelectedPart?.Info.CanEdit == true)
        {
            Loading = true;
            NUD_X.Value = x;
            NUD_Z.Value = z;
            Loading = false;
            UpdatePreview();
            return;
        }
        if (e.Button != MouseButtons.Left || !TrySaveChanges())
            return;
        var part = Stage.Parts.Where(p => p.Bounds.Contains(x, CHK_AllHeights.Checked ? p.Y : (int)NUD_Height.Value, z))
            .OrderByDescending(p => p.Y).ThenByDescending(p => p.Index).FirstOrDefault();
        if (part == null)
            return;
        Loading = true;
        TB_Search.Clear();
        SelectedPart = part;
        Loading = false;
        LoadParts(false);
    }
}
