using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using PokoHex.Core;

namespace PokoHex.Web.Controls;

public partial class MapEditor
{
    [Parameter, EditorRequired] public SaveFolder SAV { get; set; } = null!;
    [Parameter] public int Language { get; set; }
    [Parameter] public EventCallback Changed { get; set; }
    [Parameter, EditorRequired] public Action<string> Error { get; set; } = null!;
    private Task NotifyChanged() => Changed.InvokeAsync();

    private SaveFolder? Loaded;
    private StageSave? Stage;
    private MapPart? SelectedPart;
    private ElementReference PB_Map;
    private MapPart[] Entries = [];
    private MapPartInfo[] Choices = [];
    private int PreviousLanguage = -1;
    private int Area;
    private int Chunk;
    private int Height;
    private int X;
    private int Y;
    private int Z;
    private int Direction;
    private ushort Variation;
    private bool AllHeights = true;
    private bool Loading;
    private string Search = string.Empty;
    private string LoadError = "No areas are available.";
    private int OriginX => (Chunk & 63) * 32;
    private int OriginZ => (Chunk >> 6) * 32;
    private int Layer => Math.Clamp(Height, 0, 127);
    public bool HasChanges => SelectedPart is { } part && (Variation != part.Variation || X != part.X || Y != part.Y || Z != part.Z || Direction != part.Direction);
    private string? EditError => Stage == null || SelectedPart == null ? "Select an object." : Stage.GetEditError(SelectedPart, Variation, X, Y, Z, Direction);
    private MapBounds? PendingBounds => Direction is >= 0 and <= 3 ? MapPartInfo.Get(Variation).GetBounds(X, Y, Z, Direction) : null;
    private MapPart[] VisibleParts => Stage?.Parts.Where(z => GetVisibleBounds(z).Intersects(new(OriginX, 0, OriginZ, 32, 128, 32)) &&
        (AllHeights || Layer >= z.Y && Layer < z.Y + Math.Max(1, (int)z.Info.Height))).OrderBy(z => z.Y).ThenBy(z => z.Index).ToArray() ?? [];

    protected override async Task OnParametersSetAsync()
    {
        if (Loaded != SAV)
        {
            Loaded = SAV;
            Area = SAV.StageIds.FirstOrDefault();
            await LoadArea();
        }
        if (PreviousLanguage != Language)
        {
            Search = string.Empty;
            LoadParts();
            BindChoices();
        }
        PreviousLanguage = Language;
    }

    private async Task LoadArea()
    {
        Stage = null;
        SelectedPart = null;
        Entries = [];
        if (!SAV.StageIds.Contains(Area))
            return;
        Loading = true;
        StateHasChanged();
        await Task.Delay(30);
        try
        {
            Stage = SAV.GetStage(Area);
            Chunk = Stage.Chunks[0];
            Search = string.Empty;
            LoadParts();
            SelectedPart = Entries.FirstOrDefault(z => z.Info.CanEdit) ?? Entries.FirstOrDefault();
            LoadPart(true);
        }
        catch (Exception ex) when (ex is IOException or InvalidDataException or ArgumentException)
        {
            LoadError = ex.Message;
        }
        finally { Loading = false; }
    }

    private static string PartText(MapPart part) => $"{SpriteUtil.GetItemName(part.Info.ItemId)} [{part.X}, {part.Y}, {part.Z}] (#{part.Index})";
    private void LoadParts() => Entries = Stage?.Parts.Where(z => PartText(z).Contains(Search, StringComparison.CurrentCultureIgnoreCase))
        .OrderBy(z => SpriteUtil.GetItemName(z.Info.ItemId)).ThenBy(z => z.Index).ToArray() ?? [];

    private void BindChoices() => Choices = SelectedPart == null ? [] : MapPartInfo.All.ToArray().Where(z => z.CanEdit || z.Value == SelectedPart.Variation)
        .OrderBy(z => SpriteUtil.GetItemName(z.ItemId)).ThenBy(z => z.Value).ToArray();

    private void LoadPart(bool center)
    {
        BindChoices();
        if (SelectedPart is not { } part)
            return;
        Variation = part.Variation;
        X = part.X;
        Y = part.Y;
        Z = part.Z;
        Direction = part.Direction;
        if (center)
        {
            Chunk = part.Chunk;
            Height = part.Y;
        }
    }

    public void SaveChanges()
    {
        if (Stage != null && SelectedPart != null && HasChanges)
            Stage.SetPart(SelectedPart, Variation, X, Y, Z, Direction);
    }

    private bool TrySaveChanges()
    {
        try { SaveChanges(); return true; }
        catch (ArgumentException ex) { Error(ex.Message); return false; }
    }

    private async Task CB_Area_Changed(ChangeEventArgs e)
    {
        if (Loading || !TrySaveChanges())
            return;
        Area = int.Parse(e.Value!.ToString()!);
        await LoadArea();
    }

    private void SelectPart(MapPart part)
    {
        if (!TrySaveChanges())
            return;
        SelectedPart = part;
        LoadPart(true);
    }

    private void TB_Search_Changed(ChangeEventArgs e)
    {
        if (!TrySaveChanges())
            return;
        Search = e.Value?.ToString() ?? string.Empty;
        LoadParts();
        if (!Entries.Contains(SelectedPart))
        {
            SelectedPart = Entries.FirstOrDefault();
            LoadPart(true);
        }
    }

    private void B_Set_Click()
    {
        if (!TrySaveChanges())
            return;
        LoadParts();
        LoadPart(false);
    }

    private void B_Undo_Click() { Stage?.UndoEdit(); LoadParts(); LoadPart(false); }
    private void B_Redo_Click() { Stage?.RedoEdit(); LoadParts(); LoadPart(false); }

    private async Task PB_Map_MouseDown(MouseEventArgs e)
    {
        if (Stage == null || e.Button is not (0 or 2))
            return;
        var point = await JS.InvokeAsync<int[]>("pokohex.mapPoint", PB_Map, e.ClientX, e.ClientY);
        if (point.Length != 2 || point.Any(z => z is < 0 or > 31))
            return;
        var x = OriginX + point[0];
        var z = OriginZ + point[1];
        if (e.Button == 2 && SelectedPart?.Info.CanEdit == true)
        {
            X = x;
            Z = z;
        }
        else if (e.Button == 0 && TrySaveChanges())
        {
            var part = Stage.Parts.Where(p => GetVisibleBounds(p).Contains(x, AllHeights ? p.Y : Layer, z))
                .OrderByDescending(p => p.Y).ThenByDescending(p => p.Index).FirstOrDefault();
            if (part == null)
                return;
            Search = string.Empty;
            SelectedPart = part;
            LoadParts();
            LoadPart(false);
        }
        await NotifyChanged();
    }

    private static MapBounds GetVisibleBounds(MapPart part) => part.Bounds is { Width: > 0, Height: > 0, Depth: > 0 } bounds ? bounds : new(part.X, part.Y, part.Z, 1, 1, 1);
    private (int X, int Z, int Width, int Height) GetRectangle(MapBounds bounds) => ((bounds.X - OriginX) * 16, (bounds.Z - OriginZ) * 16, bounds.Width * 16, bounds.Depth * 16);

    private string GetGroundPath()
    {
        var path = new StringBuilder();
        for (int z = 0; z < 32; z++)
        for (int x = 0; x < 32; x++)
        {
            if (Stage!.GetCell(OriginX + x, Layer, OriginZ + z) != 0)
                path.Append($"M{x * 16} {z * 16}h16v16h-16z");
        }
        return path.ToString();
    }
}
