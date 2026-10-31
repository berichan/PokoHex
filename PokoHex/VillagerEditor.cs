using System;
using System.Linq;
using System.Windows.Forms;
using PokoHex.Core;

namespace PokoHex;

public partial class VillagerEditor : UserControl
{
    private SaveFile? SAV;
    private bool Loading;
    private int SelectedVillager = -1;
    private int[] Areas = [];

    public VillagerEditor() => InitializeComponent();

    public bool HasChanges
    {
        get
        {
            if (SAV == null || SelectedVillager < 0)
                return false;
            var villager = SAV.Villagers[SelectedVillager];
            return NUD_Friendship.Value != villager.Friendship || CHK_NameConversation.Checked != villager.NameConversationFinished;
        }
    }

    public void LoadSave(SaveFile save)
    {
        SAV = save;
        Loading = true;
        SelectedVillager = -1;
        TB_Search.Clear();
        CB_Area.Items.Clear();
        Areas = [-1, .. save.Villagers.Where(z => !z.IsEmpty).Select(z => (int)z.Home.StageId).Distinct().Order()];
        foreach (var area in Areas)
            CB_Area.Items.Add(area < 0 ? "All areas" : GetAreaName(area));
        CB_Area.SelectedIndex = 0;
        TB_Search.Enabled = CB_Area.Enabled = save.Villagers.Length != 0;
        Loading = false;
        LoadVillagers();
    }

    public void RefreshLanguage()
    {
        SaveChanges();
        Loading = true;
        TB_Search.Clear();
        Loading = false;
        LoadVillagers();
    }

    private static string GetAreaName(int area) => area switch
    {
        0 => "No home",
        1 => "Withered Wasteland",
        2 => "Bleak Beach",
        3 => "Rocky Ridges",
        _ => $"Area {area}",
    };

    private static string GetVillagerName(int index)
    {
        var names = GameInfo.Strings.villagerlist;
        return (uint)index < names.Length && names[index].Length != 0 ? names[index] : $"Villager {index:D4}";
    }

    private void LoadVillagers()
    {
        if (SAV == null)
            return;
        Loading = true;
        var selected = SelectedVillager;
        var area = CB_Area.SelectedIndex < 0 ? -1 : Areas[CB_Area.SelectedIndex];
        var entries = SAV.Villagers.Where(z => !z.IsEmpty && (area < 0 || z.Home.StageId == area))
            .Select(z => new VillagerInfo(z.Index, GetVillagerName(z.Index)))
            .Where(z => z.Display.Contains(TB_Search.Text, StringComparison.CurrentCultureIgnoreCase))
            .OrderBy(z => z.Text).ThenBy(z => z.Value).ToArray();
        LB_Villagers.DataSource = entries;
        LB_Villagers.DisplayMember = nameof(VillagerInfo.Display);
        LB_Villagers.ValueMember = nameof(VillagerInfo.Value);
        LB_Villagers.SelectedIndex = Array.FindIndex(entries, z => z.Value == selected);
        if (LB_Villagers.SelectedIndex < 0 && entries.Length != 0)
            LB_Villagers.SelectedIndex = 0;
        SelectedVillager = (LB_Villagers.SelectedItem as VillagerInfo)?.Value ?? -1;
        L_Count.Text = SAV.Villagers.Length == 0
            ? "Villager editing is not available for this save version."
            : $"{entries.Length} of {SAV.Villagers.Count(z => !z.IsEmpty)} villagers";
        Loading = false;
        LoadVillager();
    }

    private void LoadVillager()
    {
        GB_Details.Enabled = SelectedVillager >= 0;
        if (SAV == null || SelectedVillager < 0)
        {
            GB_Details.Text = "Villager";
            L_Home.Text = L_Gifts.Text = L_Index.Text = string.Empty;
            Loading = true;
            NUD_Friendship.Value = 0;
            CHK_NameConversation.Checked = false;
            Loading = false;
            return;
        }
        Loading = true;
        var villager = SAV.Villagers[SelectedVillager];
        GB_Details.Text = GetVillagerName(SelectedVillager);
        L_Index.Text = $"Resident ID: {SelectedVillager:D4}";
        NUD_Friendship.Maximum = Math.Max(Villager.FriendshipTierMaximum, (int)villager.Friendship);
        NUD_Friendship.Value = villager.Friendship;
        CHK_NameConversation.Checked = villager.NameConversationFinished;
        L_Gifts.Text = $"Thank points: {villager.ThankPoints}\r\nPresent level received: {villager.ReceivedPresentLevel}\r\nWaiting for a present: {(villager.WaitingForPresent ? "Yes" : "No")}";
        var home = villager.Home;
        L_Home.Text = home.Type == 0 ? "No home assigned." : $"{GetAreaName(home.StageId)}\r\nHome ID: {home.UniqueId}\r\nLast home block: {home.BlockX}, {home.BlockY}, {home.BlockZ}";
        Loading = false;
    }

    public void SaveChanges()
    {
        if (Loading || SAV == null || SelectedVillager < 0)
            return;
        var villager = SAV.Villagers[SelectedVillager];
        if (NUD_Friendship.Value != villager.Friendship)
            villager.Friendship = (ushort)NUD_Friendship.Value;
        if (CHK_NameConversation.Checked != villager.NameConversationFinished)
            villager.NameConversationFinished = CHK_NameConversation.Checked;
    }

    private void LB_Villagers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Loading)
            return;
        SaveChanges();
        SelectedVillager = (LB_Villagers.SelectedItem as VillagerInfo)?.Value ?? -1;
        LoadVillager();
    }

    private void Filter_Changed(object sender, EventArgs e)
    {
        if (Loading)
            return;
        SaveChanges();
        LoadVillagers();
    }

    private void B_Set_Click(object sender, EventArgs e)
    {
        ValidateChildren();
        SaveChanges();
        LoadVillager();
    }
}
