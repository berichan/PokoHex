using PokoHex.Core;
using PokoHex.Core.Structures;
using PokoHex.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokoHex;

public partial class ItemPaintEditor : UserControl
{
    private static readonly ResourceManager Resources = Properties.Resources.ResourceManager;
    private SaveFile? SAV;
    private bool Loading;
    private Item SelectedItem => SAV.Inventory[SelectedPaintableItemSlot];
    private int SelectedPaintableItemSlot;
    private int SelectedPaintSlot;
    private ushort[] SelectedPaint = [0, 0, 0];
    private ushort SelectedPattern = 0;
    private List<Image> ColorSwatches = new List<Image>();
    private List<Image> PatternSwatches = new List<Image>();
    private Image Outline;
    private bool AllowsPattern = false;
    private bool PatternSet = false;

    public ItemPaintEditor()
    {
        InitializeComponent();
    }

    public bool HasChanges
    {
        get
        {
            if (SAV == null || SelectedPaintableItemSlot < 0)
                return false;
            return true;
        }
    }

    public void ShowForItem(int itemIndex)
    {
        SelectedPaintableItemSlot = itemIndex;
        paintingBody.Visible = (SelectedPaintableItemSlot >= 0);
        if (SelectedPaintableItemSlot < 0 || SelectedItem.PaintSlots == 0)
        {
            SelectedPaintableItemSlot = -1;
            Visible = false;
            return;
        }

        Visible = true;
        Dock = DockStyle.Fill;

        L_Item.Text = $"Selected item: (slot {itemIndex + 1})";

        PB_Item.Image = ItemSprite.GetItemSprite(SelectedItem.ItemId);
        AllowsPattern = SelectedItem.AllowsPattern;
        RB_Pattern.Enabled = AllowsPattern;
        SC_Swatches.Panel2Collapsed = !AllowsPattern;
        slot2container.Enabled = SelectedItem.PaintSlots > 1;
        slot3container.Enabled = SelectedItem.PaintSlots > 2;

        Slot_Selected(PB_Slot1, EventArgs.Empty);
        PatternSet = SelectedItem.HasItemPattern;
        SelectedPaint = [PatternSet ? (ushort)0 : SelectedItem.ItemPaint1, SelectedItem.ItemPaint2, SelectedItem.ItemPaint3];
        SelectedPattern = (ushort)(PatternSet ? SelectedItem.ItemPattern : 0);
        PB_Slot1.Image = SelectedItem.ItemPaint1 == 0 ? null : Outline;
        if (PatternSet)
        {
            PB_Slot1.BackgroundImage = null;
            PB_Slot1.Image = PatternSwatches[SelectedItem.ItemPattern - 1];
        }
        else
        {
            PB_Slot1.BackgroundImage = SelectedItem.ItemPaint1 == 0 ? null : ColorSwatches[SelectedItem.ItemPaint1 - 1];
            PB_Slot2.Image = SelectedItem.ItemPaint2 == 0 ? null : Outline;
        }
        PB_Slot2.BackgroundImage = SelectedItem.ItemPaint2 == 0 ? null : ColorSwatches[SelectedItem.ItemPaint2 - 1];
        PB_Slot3.Image = SelectedItem.ItemPaint3 == 0 ? null : Outline;
        PB_Slot3.BackgroundImage = SelectedItem.ItemPaint3 == 0 ? null : ColorSwatches[SelectedItem.ItemPaint3 - 1];
    }

    public void LoadSave(SaveFile save)
    {
        SAV = save;
        Loading = true;
        SelectedPaintableItemSlot = -1;
        SelectedPaintSlot = 0;
        Outline = CreateImageOutline((Image)Resources.GetObject($"color_aquamarine"), Color.Black);
        InitSwatches();
        LoadSwatches();
        ShowForItem(-1);
        Loading = false;
    }

    public void InitSwatches()
    {
        var colorNames = ItemPaintInfo.ItemPaints.Values.Select(s => s.Item1);
        foreach (var c in colorNames)
            ColorSwatches.Add((Image)Resources.GetObject($"color_{c}"));

        for (int i = 1; i <= 100; i++)
            PatternSwatches.Add((Image)Resources.GetObject($"pattern_{i}"));
    }

    public void LoadSwatches()
    {
        FLP_ColorSwatches.Controls.Clear();
        FLP_PatternSwatches.Controls.Clear();
        for (int i = 0; i < ColorSwatches.Count; i++)
        {
            var pictureBox = new PictureBox() { BackgroundImage = ColorSwatches[i], Image = Outline, Height = 100, Width = 100, SizeMode = PictureBoxSizeMode.Zoom, BackgroundImageLayout = ImageLayout.Zoom, Tag = i };
            pictureBox.Click += ColorSlot_SelectionChanged;
            FLP_ColorSwatches.Controls.Add(pictureBox);
        }

        _ = Task.Run(() =>
        {
            PatternSwatches = PatternSwatches.Select(s => (Image)ApplyAlphaMask(s, (Image)Resources.GetObject($"pattern_99"))).ToList();

            BeginInvoke(() =>
            {
                for (int i = 0; i < PatternSwatches.Count; i++)
                {
                    var pictureBox = new PictureBox() { Image = PatternSwatches[i], Height = 100, Width = 100, SizeMode = PictureBoxSizeMode.Zoom, BackgroundImageLayout = ImageLayout.Zoom, Tag = i };
                    pictureBox.Click += PatternSlot_SelectionChanged;
                    FLP_PatternSwatches.Controls.Add(pictureBox);
                }
            });
        });
    }

    public void RefreshLanguage()
    {
    }

    public void SaveChanges()
    {
        if (Loading || SAV == null || SelectedPaintableItemSlot < 0)
            return;

        SelectedItem.SetPaint(SelectedPaint);
        if (PatternSet)
            SelectedItem.SetPattern(SelectedPattern);
    }

    private void Slot_Selected(object sender, EventArgs e)
    {
        SelectedPaintSlot = int.Parse((string)(sender as PictureBox).Tag);
        var onColor = ColorTranslator.FromHtml("Highlight");
        var offColor = Color.Transparent;
        slot1selected.BackColor = SelectedPaintSlot == 0 ? onColor : offColor;
        slot2selected.BackColor = SelectedPaintSlot == 1 ? onColor : offColor;
        slot3selected.BackColor = SelectedPaintSlot == 2 ? onColor : offColor;

        if (SelectedPaintSlot > 0)
        {
            RB_Pattern.Enabled = false;
            SC_Swatches.Panel2Collapsed = true;
        }
        else if (AllowsPattern)
        {
            RB_Pattern.Enabled = true;
            SC_Swatches.Panel2Collapsed = false;
        }
    }

    private void SetPattern(int pattern)
    {
        PatternSet = true;
        if (SelectedItem.PaintSlots == 3)
        {
            slot2container.Enabled = false;
            //PB_Slot2.Image = PB_Slot2.BackgroundImage = null;
        }
        PB_Slot1.Image = PatternSwatches[pattern];
        SelectedPattern = (ushort)(pattern + 1);
    }
    private void SetColorForSlot(int slot, int color)
    {
        switch (slot)
        {
            case 0:
                PatternSet = false;
                slot2container.Enabled = SelectedItem.PaintSlots > 1;
                PB_Slot1.Image = Outline;
                PB_Slot1.BackgroundImage = ColorSwatches[color];
                SelectedPaint[0] = (ushort)(color + 1);
                break;
            case 1:
                PB_Slot2.Image = Outline;
                PB_Slot2.BackgroundImage = ColorSwatches[color];
                SelectedPaint[1] = (ushort)(color + 1);
                break;
            case 2:
                PB_Slot3.Image = Outline;
                PB_Slot3.BackgroundImage = ColorSwatches[color];
                SelectedPaint[2] = (ushort)(color + 1);
                break;
        }
    }


    public static Bitmap CreateImageOutline(Image source, Color outlineColor, int thickness = 3)
    {
        int width = source.Width;
        int height = source.Height;

        Bitmap original = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        Bitmap outline = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        using (Graphics g = Graphics.FromImage(original))
        {
            g.Clear(Color.Transparent);
            g.DrawImage(source, 0, 0, width, height);
        }

        using (Graphics g = Graphics.FromImage(outline))
        {
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        }

        using (Bitmap expanded = new Bitmap(width, height, PixelFormat.Format32bppArgb))
        using (Graphics g = Graphics.FromImage(expanded))
        {
            g.Clear(Color.Transparent);
            for (int y = -thickness; y <= thickness; y++)
            {
                for (int x = -thickness; x <= thickness; x++)
                {
                    if (x * x + y * y > thickness * thickness)
                        continue;
                    g.DrawImageUnscaled(original, x, y);
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int expandedAlpha = expanded.GetPixel(x, y).A;
                    int originalAlpha = original.GetPixel(x, y).A;
                    int borderAlpha = expandedAlpha * (255 - originalAlpha) / 255;
                    if (borderAlpha > 0)
                        outline.SetPixel(x, y, Color.FromArgb(borderAlpha, outlineColor));
                }
            }
        }
        original.Dispose();
        return outline;
    }

    public static async Task<Bitmap[]> ApplyAlphaMaskToImagesAsync(IReadOnlyList<Image> sources, Image alphaMask)
    {
        return await Task.Run(() => sources.Select(s => ApplyAlphaMask(s, alphaMask)).ToArray());
    }
    public static Bitmap ApplyAlphaMask(Image source, Image alphaMask)
    {
        int width = source.Width;
        int height = source.Height;
        Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        using (Bitmap sourceBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
        using (Bitmap maskBitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb))
        using (Graphics sg = Graphics.FromImage(sourceBitmap))
        using (Graphics mg = Graphics.FromImage(maskBitmap))
        {
            sg.DrawImage(source, 0, 0, width, height);
            mg.DrawImage(alphaMask, 0, 0, width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color src = sourceBitmap.GetPixel(x, y);
                    Color mask = maskBitmap.GetPixel(x, y);
                    int alpha = src.A * mask.A / 255;
                    result.SetPixel(x, y, Color.FromArgb(alpha, src.R, src.G, src.B));
                }
            }
        }

        return result;
    }

    private void SC_Swatches_Resize(object sender, EventArgs e)
    {
        SC_Swatches.SplitterDistance = Math.Max(0, SC_Swatches.Width / 2 - 2);
    }

    private void RB_Paint_CheckedChanged(object sender, EventArgs e)
    {
        SC_Swatches.Panel1Collapsed = false;
        SC_Swatches.Panel2Collapsed = true;
    }

    private void RB_Pattern_CheckedChanged(object sender, EventArgs e)
    {
        SC_Swatches.Panel1Collapsed = true;
        SC_Swatches.Panel2Collapsed = false;
    }

    private void ColorSlot_SelectionChanged(object sender, EventArgs e)
    {
        int selectedColor = (int)((PictureBox)sender).Tag;
        SetColorForSlot(SelectedPaintSlot, selectedColor);
    }

    private void PatternSlot_SelectionChanged(object sender, EventArgs e)
    {
        int selectedPattern = (int)((PictureBox)sender).Tag;
        SetPattern(selectedPattern);
    }

    private void B_Apply_Click(object sender, EventArgs e)
    {
        SaveChanges();
        OnApplyPaint?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler OnApplyPaint;
}
