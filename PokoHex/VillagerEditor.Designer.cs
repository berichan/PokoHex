using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PokoHex;

partial class VillagerEditor
{
    private IContainer components = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new Container();
        var split = new SplitContainer();
        var filter = new Panel();
        var search = new Label();
        var area = new Label();
        var friendship = new Label();
        var home = new Label();
        var set = new Button();
        TB_Search = new TextBox();
        CB_Area = new ComboBox();
        LB_Villagers = new ListBox();
        GB_Details = new GroupBox();
        NUD_Friendship = new NumericUpDown();
        CHK_NameConversation = new CheckBox();
        L_Index = new Label();
        L_Gifts = new Label();
        L_Home = new Label();
        L_Count = new Label();
        ((ISupportInitialize)split).BeginInit();
        split.Panel1.SuspendLayout();
        split.Panel2.SuspendLayout();
        split.SuspendLayout();
        ((ISupportInitialize)NUD_Friendship).BeginInit();
        SuspendLayout();
        split.Dock = DockStyle.Fill;
        split.Size = new Size(920, 500);
        split.SplitterDistance = 540;
        split.Panel1MinSize = 280;
        split.Panel2MinSize = 320;
        split.FixedPanel = FixedPanel.Panel2;
        filter.Dock = DockStyle.Top;
        filter.Size = new Size(540, 76);
        search.AutoSize = true;
        search.Location = new Point(0, 10);
        search.Text = "Search:";
        TB_Search.Name = "TB_Search";
        TB_Search.Location = new Point(66, 7);
        TB_Search.Size = new Size(460, 23);
        TB_Search.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TB_Search.TextChanged += Filter_Changed;
        area.AutoSize = true;
        area.Location = new Point(0, 43);
        area.Text = "Area:";
        CB_Area.Name = "CB_Area";
        CB_Area.Location = new Point(66, 40);
        CB_Area.Size = new Size(460, 23);
        CB_Area.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        CB_Area.DropDownStyle = ComboBoxStyle.DropDownList;
        CB_Area.SelectedIndexChanged += Filter_Changed;
        filter.Controls.AddRange([search, TB_Search, area, CB_Area]);
        LB_Villagers.Name = "LB_Villagers";
        LB_Villagers.Dock = DockStyle.Fill;
        LB_Villagers.IntegralHeight = false;
        LB_Villagers.SelectedIndexChanged += LB_Villagers_SelectedIndexChanged;
        L_Count.Dock = DockStyle.Bottom;
        L_Count.Height = 40;
        L_Count.Padding = new Padding(0, 8, 0, 0);
        split.Panel1.Controls.Add(LB_Villagers);
        split.Panel1.Controls.Add(filter);
        split.Panel1.Controls.Add(L_Count);
        GB_Details.Name = "GB_Details";
        GB_Details.Dock = DockStyle.Top;
        GB_Details.Height = 410;
        GB_Details.Text = "Villager";
        GB_Details.Enabled = false;
        L_Index.Location = new Point(16, 30);
        L_Index.Size = new Size(310, 23);
        friendship.AutoSize = true;
        friendship.Text = "Friendship points:";
        friendship.Location = new Point(16, 72);
        NUD_Friendship.Name = "NUD_Friendship";
        NUD_Friendship.Location = new Point(162, 69);
        NUD_Friendship.Size = new Size(96, 23);
        NUD_Friendship.Maximum = 170;
        CHK_NameConversation.Name = "CHK_NameConversation";
        CHK_NameConversation.AutoSize = true;
        CHK_NameConversation.Location = new Point(16, 111);
        CHK_NameConversation.Text = "Name conversation completed";
        set.Name = "B_Set";
        set.Location = new Point(16, 151);
        set.Size = new Size(100, 30);
        set.Text = "Set";
        set.Click += B_Set_Click;
        L_Gifts.Location = new Point(16, 207);
        L_Gifts.Size = new Size(310, 67);
        home.AutoSize = true;
        home.Text = "Home:";
        home.Location = new Point(16, 293);
        L_Home.Location = new Point(16, 318);
        L_Home.Size = new Size(310, 75);
        GB_Details.Controls.AddRange([L_Index, friendship, NUD_Friendship, CHK_NameConversation, set, L_Gifts, home, L_Home]);
        split.Panel2.AutoScroll = true;
        split.Panel2.Padding = new Padding(12, 0, 0, 0);
        split.Panel2.Controls.Add(GB_Details);
        Controls.Add(split);
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Name = "VillagerEditor";
        Padding = new Padding(8);
        Size = new Size(936, 516);
        split.Panel1.ResumeLayout(false);
        split.Panel2.ResumeLayout(false);
        ((ISupportInitialize)split).EndInit();
        split.ResumeLayout(false);
        ((ISupportInitialize)NUD_Friendship).EndInit();
        ResumeLayout(false);
    }

    private TextBox TB_Search = null!;
    private ComboBox CB_Area = null!;
    private ListBox LB_Villagers = null!;
    private GroupBox GB_Details = null!;
    private NumericUpDown NUD_Friendship = null!;
    private CheckBox CHK_NameConversation = null!;
    private Label L_Index = null!;
    private Label L_Gifts = null!;
    private Label L_Home = null!;
    private Label L_Count = null!;
}
