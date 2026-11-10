using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PokoHex;

partial class Main
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
        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        menu = new MenuStrip();
        file = new ToolStripMenuItem();
        Menu_Open = new ToolStripMenuItem();
        Menu_Export = new ToolStripMenuItem();
        Menu_Exit = new ToolStripMenuItem();
        language = new ToolStripLabel();
        Menu_Language = new ToolStripComboBox();
        status = new StatusStrip();
        L_Status = new ToolStripStatusLabel();
        TC_Editor = new TabControl();
        player = new TabPage();
        playerBody = new Panel();
        Editor_Skills = new AbilityEditor();
        Editor_Reactions = new AbilityEditor();
        playerHeader = new Panel();
        tableLayoutPanel1 = new TableLayoutPanel();
        playerSections = new FlowLayoutPanel();
        RB_Skills = new RadioButton();
        RB_Reactions = new RadioButton();
        name = new Label();
        TB_Name = new TextBox();
        nameHint = new Label();
        items = new TabPage();
        split = new SplitContainer();
        DGV_Items = new DataGridView();
        dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        dataGridViewImageColumn1 = new DataGridViewImageColumn();
        dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
        inventoryPanel = new Panel();
        inventoryLabel = new Label();
        CB_Inventory = new ComboBox();
        L_Paints = new Label();
        NUD_Pattern = new NumericUpDown();
        chkPattern = new CheckBox();
        NUD_Paint3 = new NumericUpDown();
        NUD_Paint2 = new NumericUpDown();
        NUD_Paint1 = new NumericUpDown();
        chkPaint = new CheckBox();
        NUD_CustomItemID = new NumericUpDown();
        chk_CustomItem = new CheckBox();
        L_Slot = new Label();
        itemLabel = new Label();
        CB_Item = new ComboBox();
        amount = new Label();
        NUD_Count = new NumericUpDown();
        B_Set = new Button();
        clear = new Button();
        L_Catalog = new Label();
        villagers = new TabPage();
        Editor_Villagers = new VillagerEditor();
        map = new TabPage();
        Editor_Map = new MapEditor();
        flags = new TabPage();
        Editor_Flags = new FlagEditor();
        menu.SuspendLayout();
        status.SuspendLayout();
        TC_Editor.SuspendLayout();
        player.SuspendLayout();
        playerBody.SuspendLayout();
        playerHeader.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        playerSections.SuspendLayout();
        items.SuspendLayout();
        ((ISupportInitialize)split).BeginInit();
        split.Panel1.SuspendLayout();
        split.Panel2.SuspendLayout();
        split.SuspendLayout();
        ((ISupportInitialize)DGV_Items).BeginInit();
        inventoryPanel.SuspendLayout();
        ((ISupportInitialize)NUD_Pattern).BeginInit();
        ((ISupportInitialize)NUD_Paint3).BeginInit();
        ((ISupportInitialize)NUD_Paint2).BeginInit();
        ((ISupportInitialize)NUD_Paint1).BeginInit();
        ((ISupportInitialize)NUD_CustomItemID).BeginInit();
        ((ISupportInitialize)NUD_Count).BeginInit();
        villagers.SuspendLayout();
        map.SuspendLayout();
        flags.SuspendLayout();
        SuspendLayout();
        // 
        // menu
        // 
        menu.ImageScalingSize = new Size(20, 20);
        menu.Items.AddRange(new ToolStripItem[] { file, language, Menu_Language });
        menu.Location = new Point(0, 0);
        menu.Name = "menu";
        menu.Size = new Size(1804, 32);
        menu.TabIndex = 2;
        // 
        // file
        // 
        file.DropDownItems.AddRange(new ToolStripItem[] { Menu_Open, Menu_Export, Menu_Exit });
        file.Name = "file";
        file.Size = new Size(46, 28);
        file.Text = "&File";
        // 
        // Menu_Open
        // 
        Menu_Open.Name = "Menu_Open";
        Menu_Open.ShortcutKeys = Keys.Control | Keys.O;
        Menu_Open.Size = new Size(267, 26);
        Menu_Open.Text = "Open save folder...";
        Menu_Open.Click += Menu_Open_Click;
        // 
        // Menu_Export
        // 
        Menu_Export.Name = "Menu_Export";
        Menu_Export.ShortcutKeys = Keys.Control | Keys.S;
        Menu_Export.Size = new Size(267, 26);
        Menu_Export.Text = "Export save copy...";
        Menu_Export.Click += Menu_Export_Click;
        // 
        // Menu_Exit
        // 
        Menu_Exit.Name = "Menu_Exit";
        Menu_Exit.Size = new Size(267, 26);
        Menu_Exit.Text = "Exit";
        Menu_Exit.Click += Menu_Exit_Click;
        // 
        // language
        // 
        language.Name = "language";
        language.Size = new Size(77, 25);
        language.Text = "Language:";
        // 
        // Menu_Language
        // 
        Menu_Language.DropDownStyle = ComboBoxStyle.DropDownList;
        Menu_Language.Items.AddRange(new object[] { "English", "日本語", "Deutsch", "Español", "Français", "Italiano", "한국어", "简体中文", "繁體中文" });
        Menu_Language.Name = "Menu_Language";
        Menu_Language.Size = new Size(130, 28);
        Menu_Language.SelectedIndexChanged += Menu_Language_SelectedIndexChanged;
        // 
        // status
        // 
        status.ImageScalingSize = new Size(20, 20);
        status.Items.AddRange(new ToolStripItem[] { L_Status });
        status.Location = new Point(0, 1485);
        status.Name = "status";
        status.Size = new Size(1804, 26);
        status.TabIndex = 1;
        // 
        // L_Status
        // 
        L_Status.Name = "L_Status";
        L_Status.Size = new Size(197, 20);
        L_Status.Text = "Open a save folder to begin.";
        // 
        // TC_Editor
        // 
        TC_Editor.Controls.Add(player);
        TC_Editor.Controls.Add(items);
        TC_Editor.Controls.Add(villagers);
        TC_Editor.Controls.Add(map);
        TC_Editor.Controls.Add(flags);
        TC_Editor.Dock = DockStyle.Fill;
        TC_Editor.Enabled = false;
        TC_Editor.Location = new Point(0, 32);
        TC_Editor.Margin = new Padding(3, 4, 3, 4);
        TC_Editor.Name = "TC_Editor";
        TC_Editor.SelectedIndex = 0;
        TC_Editor.Size = new Size(1804, 1453);
        TC_Editor.TabIndex = 0;
        // 
        // player
        // 
        player.Controls.Add(playerBody);
        player.Controls.Add(playerHeader);
        player.Location = new Point(4, 29);
        player.Margin = new Padding(3, 4, 3, 4);
        player.Name = "player";
        player.Padding = new Padding(0, 5, 0, 0);
        player.Size = new Size(1796, 1420);
        player.TabIndex = 0;
        player.Text = "Player";
        // 
        // playerBody
        // 
        playerBody.Controls.Add(Editor_Skills);
        playerBody.Controls.Add(Editor_Reactions);
        playerBody.Dock = DockStyle.Fill;
        playerBody.Location = new Point(0, 56);
        playerBody.Name = "playerBody";
        playerBody.Size = new Size(1796, 1364);
        playerBody.TabIndex = 3;
        // 
        // Editor_Skills
        // 
        Editor_Skills.Dock = DockStyle.Fill;
        Editor_Skills.Location = new Point(0, 0);
        Editor_Skills.Margin = new Padding(3, 4, 3, 4);
        Editor_Skills.Name = "Editor_Skills";
        Editor_Skills.Padding = new Padding(11, 0, 0, 0);
        Editor_Skills.Reactions = false;
        Editor_Skills.Size = new Size(1796, 1364);
        Editor_Skills.TabIndex = 1;
        // 
        // Editor_Reactions
        // 
        Editor_Reactions.Dock = DockStyle.Fill;
        Editor_Reactions.Location = new Point(0, 0);
        Editor_Reactions.Margin = new Padding(3, 4, 3, 4);
        Editor_Reactions.Name = "Editor_Reactions";
        Editor_Reactions.Padding = new Padding(11, 0, 0, 0);
        Editor_Reactions.Reactions = true;
        Editor_Reactions.Size = new Size(1796, 1364);
        Editor_Reactions.TabIndex = 0;
        // 
        // playerHeader
        // 
        playerHeader.Controls.Add(tableLayoutPanel1);
        playerHeader.Dock = DockStyle.Top;
        playerHeader.Location = new Point(0, 5);
        playerHeader.Margin = new Padding(3, 4, 3, 4);
        playerHeader.Name = "playerHeader";
        playerHeader.Size = new Size(1796, 51);
        playerHeader.TabIndex = 2;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.BackColor = SystemColors.Control;
        tableLayoutPanel1.ColumnCount = 6;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90F));
        tableLayoutPanel1.Controls.Add(playerSections, 4, 0);
        tableLayoutPanel1.Controls.Add(name, 0, 0);
        tableLayoutPanel1.Controls.Add(TB_Name, 1, 0);
        tableLayoutPanel1.Controls.Add(nameHint, 2, 0);
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.Location = new Point(0, 0);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 1;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutPanel1.Size = new Size(1796, 51);
        tableLayoutPanel1.TabIndex = 3;
        // 
        // playerSections
        // 
        playerSections.Anchor = AnchorStyles.Top;
        playerSections.Controls.Add(RB_Skills);
        playerSections.Controls.Add(RB_Reactions);
        playerSections.Location = new Point(613, 4);
        playerSections.Margin = new Padding(3, 4, 3, 4);
        playerSections.Name = "playerSections";
        playerSections.Size = new Size(271, 43);
        playerSections.TabIndex = 2;
        playerSections.WrapContents = false;
        // 
        // RB_Skills
        // 
        RB_Skills.Appearance = Appearance.Button;
        RB_Skills.Checked = true;
        RB_Skills.Location = new Point(0, 0);
        RB_Skills.Margin = new Padding(0);
        RB_Skills.Name = "RB_Skills";
        RB_Skills.Size = new Size(133, 40);
        RB_Skills.TabIndex = 0;
        RB_Skills.TabStop = true;
        RB_Skills.Text = "Skills";
        RB_Skills.TextAlign = ContentAlignment.MiddleCenter;
        RB_Skills.CheckedChanged += RB_PlayerSection_CheckedChanged;
        // 
        // RB_Reactions
        // 
        RB_Reactions.Appearance = Appearance.Button;
        RB_Reactions.Location = new Point(133, 0);
        RB_Reactions.Margin = new Padding(0);
        RB_Reactions.Name = "RB_Reactions";
        RB_Reactions.Size = new Size(133, 40);
        RB_Reactions.TabIndex = 1;
        RB_Reactions.Text = "Reactions";
        RB_Reactions.TextAlign = ContentAlignment.MiddleCenter;
        RB_Reactions.CheckedChanged += RB_PlayerSection_CheckedChanged;
        // 
        // name
        // 
        name.AutoSize = true;
        name.Dock = DockStyle.Fill;
        name.Location = new Point(3, 0);
        name.Name = "name";
        name.Padding = new Padding(5, 14, 0, 0);
        name.Size = new Size(114, 51);
        name.TabIndex = 0;
        name.Text = "Player name:";
        // 
        // TB_Name
        // 
        TB_Name.Dock = DockStyle.Fill;
        TB_Name.Location = new Point(123, 12);
        TB_Name.Margin = new Padding(3, 12, 3, 4);
        TB_Name.MaxLength = 24;
        TB_Name.Name = "TB_Name";
        TB_Name.Size = new Size(194, 27);
        TB_Name.TabIndex = 1;
        // 
        // nameHint
        // 
        nameHint.AutoSize = true;
        nameHint.Dock = DockStyle.Fill;
        nameHint.Location = new Point(323, 0);
        nameHint.Name = "nameHint";
        nameHint.Padding = new Padding(0, 14, 0, 0);
        nameHint.Size = new Size(174, 51);
        nameHint.TabIndex = 2;
        nameHint.Text = "Up to 24 UTF-8 bytes.";
        // 
        // items
        // 
        items.Controls.Add(split);
        items.Location = new Point(4, 29);
        items.Margin = new Padding(3, 4, 3, 4);
        items.Name = "items";
        items.Padding = new Padding(9, 11, 9, 11);
        items.Size = new Size(1796, 1420);
        items.TabIndex = 1;
        items.Text = "Items";
        // 
        // split
        // 
        split.Dock = DockStyle.Fill;
        split.FixedPanel = FixedPanel.Panel2;
        split.IsSplitterFixed = true;
        split.Location = new Point(9, 11);
        split.Margin = new Padding(3, 4, 3, 4);
        split.Name = "split";
        // 
        // split.Panel1
        // 
        split.Panel1.Controls.Add(DGV_Items);
        split.Panel1.Controls.Add(inventoryPanel);
        // 
        // split.Panel2
        // 
        split.Panel2.BackColor = SystemColors.Control;
        split.Panel2.Controls.Add(L_Paints);
        split.Panel2.Controls.Add(NUD_Pattern);
        split.Panel2.Controls.Add(chkPattern);
        split.Panel2.Controls.Add(NUD_Paint3);
        split.Panel2.Controls.Add(NUD_Paint2);
        split.Panel2.Controls.Add(NUD_Paint1);
        split.Panel2.Controls.Add(chkPaint);
        split.Panel2.Controls.Add(NUD_CustomItemID);
        split.Panel2.Controls.Add(chk_CustomItem);
        split.Panel2.Controls.Add(L_Slot);
        split.Panel2.Controls.Add(itemLabel);
        split.Panel2.Controls.Add(CB_Item);
        split.Panel2.Controls.Add(amount);
        split.Panel2.Controls.Add(NUD_Count);
        split.Panel2.Controls.Add(B_Set);
        split.Panel2.Controls.Add(clear);
        split.Panel2.Controls.Add(L_Catalog);
        split.Size = new Size(1778, 1398);
        split.SplitterDistance = 1501;
        split.TabIndex = 0;
        // 
        // DGV_Items
        // 
        DGV_Items.AllowUserToAddRows = false;
        DGV_Items.AllowUserToDeleteRows = false;
        DGV_Items.AllowUserToResizeRows = false;
        DGV_Items.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DGV_Items.BackgroundColor = SystemColors.Window;
        DGV_Items.ColumnHeadersHeight = 29;
        DGV_Items.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewImageColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4 });
        DGV_Items.Dock = DockStyle.Fill;
        DGV_Items.Location = new Point(0, 36);
        DGV_Items.MultiSelect = false;
        DGV_Items.Name = "DGV_Items";
        DGV_Items.ReadOnly = true;
        DGV_Items.RowHeadersVisible = false;
        DGV_Items.RowHeadersWidth = 51;
        DGV_Items.RowTemplate.Height = 72;
        DGV_Items.RowTemplate.ReadOnly = true;
        DGV_Items.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DGV_Items.ShowEditingIcon = false;
        DGV_Items.Size = new Size(1501, 1362);
        DGV_Items.TabIndex = 0;
        DGV_Items.CurrentCellChanged += DGV_Items_CurrentCellChanged;
        // 
        // dataGridViewTextBoxColumn1
        // 
        dataGridViewTextBoxColumn1.FillWeight = 90.55259F;
        dataGridViewTextBoxColumn1.HeaderText = "Slot #";
        dataGridViewTextBoxColumn1.MinimumWidth = 6;
        dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
        dataGridViewTextBoxColumn1.ReadOnly = true;
        // 
        // dataGridViewImageColumn1
        // 
        dataGridViewImageColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        dataGridViewImageColumn1.FillWeight = 128.342239F;
        dataGridViewImageColumn1.HeaderText = "Image";
        dataGridViewImageColumn1.MinimumWidth = 6;
        dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
        dataGridViewImageColumn1.ReadOnly = true;
        dataGridViewImageColumn1.Resizable = DataGridViewTriState.False;
        dataGridViewImageColumn1.Width = 125;
        // 
        // dataGridViewTextBoxColumn2
        // 
        dataGridViewTextBoxColumn2.FillWeight = 90.55259F;
        dataGridViewTextBoxColumn2.HeaderText = "Name";
        dataGridViewTextBoxColumn2.MinimumWidth = 6;
        dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
        dataGridViewTextBoxColumn2.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn3
        // 
        dataGridViewTextBoxColumn3.FillWeight = 90.55259F;
        dataGridViewTextBoxColumn3.HeaderText = "Count";
        dataGridViewTextBoxColumn3.MinimumWidth = 6;
        dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
        dataGridViewTextBoxColumn3.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn4
        // 
        dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
        dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle1;
        dataGridViewTextBoxColumn4.HeaderText = "Extras";
        dataGridViewTextBoxColumn4.MinimumWidth = 6;
        dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
        dataGridViewTextBoxColumn4.ReadOnly = true;
        dataGridViewTextBoxColumn4.Resizable = DataGridViewTriState.False;
        // 
        // inventoryPanel
        // 
        inventoryPanel.Controls.Add(inventoryLabel);
        inventoryPanel.Controls.Add(CB_Inventory);
        inventoryPanel.Dock = DockStyle.Top;
        inventoryPanel.Location = new Point(0, 0);
        inventoryPanel.Name = "inventoryPanel";
        inventoryPanel.Size = new Size(1501, 36);
        inventoryPanel.TabIndex = 1;
        // 
        // inventoryLabel
        // 
        inventoryLabel.AutoSize = true;
        inventoryLabel.Location = new Point(0, 8);
        inventoryLabel.Name = "inventoryLabel";
        inventoryLabel.Size = new Size(73, 20);
        inventoryLabel.TabIndex = 0;
        inventoryLabel.Text = "Inventory:";
        // 
        // CB_Inventory
        // 
        CB_Inventory.DropDownStyle = ComboBoxStyle.DropDownList;
        CB_Inventory.Items.AddRange(new object[] { "Main inventory", "Secondary bar" });
        CB_Inventory.Location = new Point(80, 4);
        CB_Inventory.Name = "CB_Inventory";
        CB_Inventory.Size = new Size(180, 28);
        CB_Inventory.TabIndex = 1;
        CB_Inventory.SelectedIndexChanged += CB_Inventory_SelectedIndexChanged;
        // 
        // L_Paints
        // 
        L_Paints.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        L_Paints.BorderStyle = BorderStyle.FixedSingle;
        L_Paints.ForeColor = SystemColors.GrayText;
        L_Paints.Location = new Point(16, 365);
        L_Paints.Name = "L_Paints";
        L_Paints.Size = new Size(254, 141);
        L_Paints.TabIndex = 19;
        L_Paints.Text = "Note:\r\n• Paint 1 and Pattern cannot be set at the same time. \r\n• Paint 2 and Pattern can be set at the same time, but it has no effect in the game.";
        // 
        // NUD_Pattern
        // 
        NUD_Pattern.Enabled = false;
        NUD_Pattern.Location = new Point(180, 335);
        NUD_Pattern.Maximum = new decimal(new int[] { 131, 0, 0, 0 });
        NUD_Pattern.Name = "NUD_Pattern";
        NUD_Pattern.Size = new Size(90, 27);
        NUD_Pattern.TabIndex = 18;
        // 
        // chkPattern
        // 
        chkPattern.AutoSize = true;
        chkPattern.Location = new Point(16, 335);
        chkPattern.Name = "chkPattern";
        chkPattern.Size = new Size(105, 24);
        chkPattern.TabIndex = 17;
        chkPattern.Text = "Set Pattern:";
        chkPattern.UseVisualStyleBackColor = true;
        chkPattern.CheckedChanged += chkPattern_CheckedChanged;
        // 
        // NUD_Paint3
        // 
        NUD_Paint3.Enabled = false;
        NUD_Paint3.Location = new Point(180, 302);
        NUD_Paint3.Maximum = new decimal(new int[] { 18, 0, 0, 0 });
        NUD_Paint3.Name = "NUD_Paint3";
        NUD_Paint3.Size = new Size(90, 27);
        NUD_Paint3.TabIndex = 16;
        // 
        // NUD_Paint2
        // 
        NUD_Paint2.Enabled = false;
        NUD_Paint2.Location = new Point(180, 269);
        NUD_Paint2.Maximum = new decimal(new int[] { 18, 0, 0, 0 });
        NUD_Paint2.Name = "NUD_Paint2";
        NUD_Paint2.Size = new Size(90, 27);
        NUD_Paint2.TabIndex = 14;
        // 
        // NUD_Paint1
        // 
        NUD_Paint1.Enabled = false;
        NUD_Paint1.Location = new Point(180, 236);
        NUD_Paint1.Maximum = new decimal(new int[] { 18, 0, 0, 0 });
        NUD_Paint1.Name = "NUD_Paint1";
        NUD_Paint1.Size = new Size(90, 27);
        NUD_Paint1.TabIndex = 12;
        // 
        // chkPaint
        // 
        chkPaint.AutoSize = true;
        chkPaint.Location = new Point(16, 237);
        chkPaint.Name = "chkPaint";
        chkPaint.Size = new Size(91, 24);
        chkPaint.TabIndex = 11;
        chkPaint.Text = "Set Paint:";
        chkPaint.UseVisualStyleBackColor = true;
        chkPaint.CheckedChanged += chkPaint_CheckedChanged;
        // 
        // NUD_CustomItemID
        // 
        NUD_CustomItemID.Enabled = false;
        NUD_CustomItemID.Location = new Point(180, 123);
        NUD_CustomItemID.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        NUD_CustomItemID.Name = "NUD_CustomItemID";
        NUD_CustomItemID.Size = new Size(90, 27);
        NUD_CustomItemID.TabIndex = 10;
        // 
        // chk_CustomItem
        // 
        chk_CustomItem.AutoSize = true;
        chk_CustomItem.Location = new Point(16, 124);
        chk_CustomItem.Name = "chk_CustomItem";
        chk_CustomItem.Size = new Size(84, 24);
        chk_CustomItem.TabIndex = 9;
        chk_CustomItem.Text = "Custom:";
        chk_CustomItem.UseVisualStyleBackColor = true;
        chk_CustomItem.CheckedChanged += chk_CustomItem_CheckedChanged;
        // 
        // L_Slot
        // 
        L_Slot.AutoSize = true;
        L_Slot.Location = new Point(16, 16);
        L_Slot.Name = "L_Slot";
        L_Slot.Size = new Size(35, 20);
        L_Slot.TabIndex = 0;
        L_Slot.Text = "Slot";
        // 
        // itemLabel
        // 
        itemLabel.AutoSize = true;
        itemLabel.Location = new Point(16, 55);
        itemLabel.Name = "itemLabel";
        itemLabel.Size = new Size(42, 20);
        itemLabel.TabIndex = 1;
        itemLabel.Text = "Item:";
        // 
        // CB_Item
        // 
        CB_Item.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        CB_Item.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        CB_Item.AutoCompleteSource = AutoCompleteSource.ListItems;
        CB_Item.DrawMode = DrawMode.OwnerDrawFixed;
        CB_Item.DropDownStyle = ComboBoxStyle.DropDownList;
        CB_Item.DropDownWidth = 440;
        CB_Item.ItemHeight = 30;
        CB_Item.Location = new Point(16, 78);
        CB_Item.Name = "CB_Item";
        CB_Item.Size = new Size(254, 36);
        CB_Item.TabIndex = 2;
        CB_Item.DrawItem += CB_Item_DrawItem;
        CB_Item.SelectedIndexChanged += CB_Item_SelectedIndexChanged;
        // 
        // amount
        // 
        amount.AutoSize = true;
        amount.Location = new Point(16, 163);
        amount.Name = "amount";
        amount.Size = new Size(65, 20);
        amount.TabIndex = 3;
        amount.Text = "Amount:";
        // 
        // NUD_Count
        // 
        NUD_Count.Location = new Point(180, 161);
        NUD_Count.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        NUD_Count.Name = "NUD_Count";
        NUD_Count.Size = new Size(90, 27);
        NUD_Count.TabIndex = 4;
        // 
        // B_Set
        // 
        B_Set.Enabled = false;
        B_Set.Location = new Point(19, 538);
        B_Set.Name = "B_Set";
        B_Set.Size = new Size(100, 30);
        B_Set.TabIndex = 5;
        B_Set.Text = "Set";
        B_Set.Click += B_Set_Click;
        // 
        // clear
        // 
        clear.Location = new Point(173, 538);
        clear.Name = "clear";
        clear.Size = new Size(100, 30);
        clear.TabIndex = 6;
        clear.Text = "Clear";
        clear.Click += B_Clear_Click;
        // 
        // L_Catalog
        // 
        L_Catalog.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        L_Catalog.Location = new Point(16, 870);
        L_Catalog.Name = "L_Catalog";
        L_Catalog.Size = new Size(254, 30);
        L_Catalog.TabIndex = 7;
        L_Catalog.Text = "Item catalog";
        // 
        // villagers
        // 
        villagers.Controls.Add(Editor_Villagers);
        villagers.Location = new Point(4, 29);
        villagers.Name = "villagers";
        villagers.Size = new Size(1796, 1420);
        villagers.TabIndex = 2;
        villagers.Text = "Villagers";
        // 
        // Editor_Villagers
        // 
        Editor_Villagers.Dock = DockStyle.Fill;
        Editor_Villagers.Location = new Point(0, 0);
        Editor_Villagers.Margin = new Padding(3, 4, 3, 4);
        Editor_Villagers.Name = "Editor_Villagers";
        Editor_Villagers.Padding = new Padding(9, 11, 9, 11);
        Editor_Villagers.Size = new Size(1796, 1420);
        Editor_Villagers.TabIndex = 0;
        // 
        // map
        // 
        map.Controls.Add(Editor_Map);
        map.Location = new Point(4, 29);
        map.Name = "map";
        map.Size = new Size(1796, 1420);
        map.TabIndex = 3;
        map.Text = "Map";
        // 
        // Editor_Map
        // 
        Editor_Map.Dock = DockStyle.Fill;
        Editor_Map.Location = new Point(0, 0);
        Editor_Map.Margin = new Padding(3, 4, 3, 4);
        Editor_Map.Name = "Editor_Map";
        Editor_Map.Padding = new Padding(7, 8, 7, 8);
        Editor_Map.Size = new Size(1796, 1420);
        Editor_Map.TabIndex = 0;
        // 
        // flags
        // 
        flags.Controls.Add(Editor_Flags);
        flags.Location = new Point(4, 29);
        flags.Name = "flags";
        flags.Padding = new Padding(3);
        flags.Size = new Size(1796, 1420);
        flags.TabIndex = 4;
        flags.Text = "Flags";
        flags.UseVisualStyleBackColor = true;
        // 
        // Editor_Flags
        // 
        Editor_Flags.Dock = DockStyle.Fill;
        Editor_Flags.Location = new Point(3, 3);
        Editor_Flags.Margin = new Padding(3, 4, 3, 4);
        Editor_Flags.Name = "Editor_Flags";
        Editor_Flags.Padding = new Padding(11, 13, 11, 13);
        Editor_Flags.Size = new Size(1790, 1414);
        Editor_Flags.TabIndex = 0;
        // 
        // Main
        // 
        AllowDrop = true;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1804, 1511);
        Controls.Add(TC_Editor);
        Controls.Add(status);
        Controls.Add(menu);
        MainMenuStrip = menu;
        Margin = new Padding(3, 4, 3, 4);
        MinimumSize = new Size(912, 624);
        Name = "Main";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "PokoHex";
        FormClosing += Main_FormClosing;
        ResizeEnd += Main_ResizeEnd;
        DragDrop += Main_DragDrop;
        DragEnter += Main_DragEnter;
        menu.ResumeLayout(false);
        menu.PerformLayout();
        status.ResumeLayout(false);
        status.PerformLayout();
        TC_Editor.ResumeLayout(false);
        player.ResumeLayout(false);
        playerBody.ResumeLayout(false);
        playerHeader.ResumeLayout(false);
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        playerSections.ResumeLayout(false);
        items.ResumeLayout(false);
        split.Panel1.ResumeLayout(false);
        split.Panel2.ResumeLayout(false);
        split.Panel2.PerformLayout();
        ((ISupportInitialize)split).EndInit();
        split.ResumeLayout(false);
        ((ISupportInitialize)DGV_Items).EndInit();
        inventoryPanel.ResumeLayout(false);
        inventoryPanel.PerformLayout();
        ((ISupportInitialize)NUD_Pattern).EndInit();
        ((ISupportInitialize)NUD_Paint3).EndInit();
        ((ISupportInitialize)NUD_Paint2).EndInit();
        ((ISupportInitialize)NUD_Paint1).EndInit();
        ((ISupportInitialize)NUD_CustomItemID).EndInit();
        ((ISupportInitialize)NUD_Count).EndInit();
        villagers.ResumeLayout(false);
        map.ResumeLayout(false);
        flags.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private ToolStripComboBox Menu_Language;
    private ToolStripStatusLabel L_Status;
    private TabControl TC_Editor;
    private TextBox TB_Name;
    private DataGridView DGV_Items;
    private ComboBox CB_Inventory;
    private ComboBox CB_Item;
    private NumericUpDown NUD_Count;
    private Label L_Slot;
    private Label L_Catalog;
    private Button B_Set;
    private VillagerEditor Editor_Villagers;
    private MapEditor Editor_Map;
    private MenuStrip menu;
    private ToolStripMenuItem file;
    private ToolStripLabel language;
    private StatusStrip status;
    private TabPage player;
    private Label name;
    private Label nameHint;
    private TabPage items;
    private SplitContainer split;
    private Panel inventoryPanel;
    private Label inventoryLabel;
    private Label itemLabel;
    private Label amount;
    private Button clear;
    private TabPage villagers;
    private TabPage map;
    private ToolStripMenuItem Menu_Open;
    private ToolStripMenuItem Menu_Export;
    private ToolStripMenuItem Menu_Exit;
    private NumericUpDown NUD_CustomItemID;
    private CheckBox chk_CustomItem;
    private AbilityEditor Editor_Reactions;
    private AbilityEditor Editor_Skills;
    private FlagEditor Editor_Flags;
    private Panel playerHeader;
    private FlowLayoutPanel playerSections;
    private RadioButton RB_Skills;
    private RadioButton RB_Reactions;
    private Panel playerBody;
    private TableLayoutPanel tableLayoutPanel1;
    private NumericUpDown NUD_Paint3;
    private NumericUpDown NUD_Paint2;
    private NumericUpDown NUD_Paint1;
    private CheckBox chkPaint;
    private NumericUpDown NUD_Pattern;
    private CheckBox chkPattern;
    private Label L_Paints;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewImageColumn dataGridViewImageColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    private TabPage flags;
}