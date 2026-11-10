using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PokoHex;

partial class AbilityEditor
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
        split = new SplitContainer();
        DGV_Abilities = new DataGridView();
        B_Learn = new Button();
        B_LearnAll = new Button();
        B_UnlockDive = new Button();
        B_UnlockFood = new Button();
        B_UnlockAbsorption = new Button();
        L_Count = new Label();
        GB_Wheel = new GroupBox();
        LB_Wheel = new ListBox();
        CB_WheelAbility = new ComboBox();
        B_Set = new Button();
        buttons = new FlowLayoutPanel();
        B_Learn = new Button();
        B_LearnAll = new Button();
        L_Count = new Label();
        ((ISupportInitialize)split).BeginInit();
        split.Panel1.SuspendLayout();
        split.Panel2.SuspendLayout();
        split.SuspendLayout();
        ((ISupportInitialize)DGV_Abilities).BeginInit();
        GB_Wheel.SuspendLayout();
        buttons.SuspendLayout();
        SuspendLayout();
        // 
        // split
        // 
        split.Dock = DockStyle.Fill;
        split.FixedPanel = FixedPanel.Panel2;
        split.Location = new Point(11, 13);
        split.Margin = new Padding(3, 4, 3, 4);
        split.Name = "split";
        // 
        // split.Panel1
        // 
        split.Panel1.Controls.Add(DGV_Abilities);
        split.Panel1MinSize = 260;
        // 
        // split.Panel2
        // 
        split.Panel2.Controls.Add(GB_Wheel);
        split.Panel2.Padding = new Padding(14, 0, 14, 0);
        split.Panel2MinSize = 320;
        split.Size = new Size(1029, 718);
        split.SplitterDistance = 587;
        split.SplitterWidth = 5;
        split.TabIndex = 0;
        // 
        // DGV_Abilities
        // 
        DGV_Abilities.AllowUserToAddRows = false;
        DGV_Abilities.AllowUserToDeleteRows = false;
        DGV_Abilities.AllowUserToResizeRows = false;
        DGV_Abilities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DGV_Abilities.BackgroundColor = SystemColors.Window;
        DGV_Abilities.ColumnHeadersHeight = 29;
        DGV_Abilities.Columns.AddRange(new DataGridViewColumn[] { dataGridViewImageColumn1, dataGridViewTextBoxColumn1, dataGridViewCheckBoxColumn1, dataGridViewTextBoxColumn2 });
        DGV_Abilities.Dock = DockStyle.Fill;
        DGV_Abilities.Location = new Point(0, 0);
        DGV_Abilities.Margin = new Padding(3, 4, 3, 4);
        DGV_Abilities.MultiSelect = false;
        DGV_Abilities.Name = "DGV_Abilities";
        DGV_Abilities.ReadOnly = true;
        DGV_Abilities.RowHeadersVisible = false;
        DGV_Abilities.RowHeadersWidth = 51;
        DGV_Abilities.RowTemplate.Height = 72;
        DGV_Abilities.RowTemplate.Resizable = DataGridViewTriState.False;
        DGV_Abilities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DGV_Abilities.Size = new Size(587, 718);
        DGV_Abilities.TabIndex = 0;
        DGV_Abilities.CurrentCellChanged += DGV_Abilities_CurrentCellChanged;
        buttons.Dock = DockStyle.Bottom;
        buttons.Height = 42;
        buttons.AutoSize = true;
        buttons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        buttons.Padding = new Padding(0, 6, 0, 0);
        B_Learn.Name = "B_Learn";
        B_Learn.Text = "Learn";
        B_Learn.Size = new Size(100, 30);
        B_Learn.Click += B_Learn_Click;
        B_LearnAll.Name = "B_LearnAll";
        B_LearnAll.Text = "Learn all";
        B_LearnAll.Size = new Size(100, 30);
        B_LearnAll.Click += B_LearnAll_Click;
        B_UnlockDive.Name = "B_UnlockDive";
        B_UnlockDive.Text = "Unlock Dive";
        B_UnlockDive.Size = new Size(160, 30);
        B_UnlockDive.Visible = false;
        B_UnlockDive.Click += B_UnlockDive_Click;
        B_UnlockFood.Name = "B_UnlockFood";
        B_UnlockFood.Text = "Unlock food boost";
        B_UnlockFood.Size = new Size(160, 30);
        B_UnlockFood.Visible = false;
        B_UnlockFood.Click += B_UnlockFood_Click;
        B_UnlockAbsorption.Name = "B_UnlockAbsorption";
        B_UnlockAbsorption.Text = "Unlock absorption";
        B_UnlockAbsorption.Size = new Size(160, 30);
        B_UnlockAbsorption.Visible = false;
        B_UnlockAbsorption.Click += B_UnlockAbsorption_Click;
        buttons.Controls.AddRange([B_Learn, B_LearnAll, B_UnlockDive, B_UnlockFood, B_UnlockAbsorption]);
        L_Count.Dock = DockStyle.Bottom;
        L_Count.Height = 28;
        L_Count.TextAlign = ContentAlignment.MiddleLeft;
        split.Panel1.Controls.Add(DGV_Abilities);
        split.Panel1.Controls.Add(buttons);
        split.Panel1.Controls.Add(L_Count);
        GB_Wheel.Name = "GB_Wheel";
        GB_Wheel.Padding = new Padding(2, 24, 2, 0);
        GB_Wheel.Size = new Size(409, 586);
        GB_Wheel.TabIndex = 0;
        GB_Wheel.TabStop = false;
        GB_Wheel.Text = "Skill wheel order";
        // 
        // LB_Wheel
        // 
        LB_Wheel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        LB_Wheel.IntegralHeight = false;
        LB_Wheel.Location = new Point(6, 35);
        LB_Wheel.Margin = new Padding(3, 4, 3, 4);
        LB_Wheel.Name = "LB_Wheel";
        LB_Wheel.Size = new Size(396, 491);
        LB_Wheel.TabIndex = 0;
        LB_Wheel.SelectedIndexChanged += LB_Wheel_SelectedIndexChanged;
        // 
        // CB_WheelAbility
        // 
        CB_WheelAbility.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        CB_WheelAbility.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        CB_WheelAbility.AutoCompleteSource = AutoCompleteSource.ListItems;
        CB_WheelAbility.DropDownStyle = ComboBoxStyle.DropDownList;
        CB_WheelAbility.AutoCompleteSource = AutoCompleteSource.ListItems;
        CB_WheelAbility.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        CB_WheelAbility.DropDownWidth = 400;
        CB_WheelAbility.Location = new Point(13, 541);
        CB_WheelAbility.Margin = new Padding(3, 4, 3, 4);
        CB_WheelAbility.Name = "CB_WheelAbility";
        CB_WheelAbility.Size = new Size(268, 28);
        CB_WheelAbility.TabIndex = 1;
        CB_WheelAbility.SelectedIndexChanged += CB_WheelAbility_SelectedIndexChanged;
        // 
        // B_Set
        // 
        B_Set.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        B_Set.Location = new Point(287, 534);
        B_Set.Margin = new Padding(3, 4, 3, 4);
        B_Set.Name = "B_Set";
        B_Set.Size = new Size(114, 40);
        B_Set.TabIndex = 2;
        B_Set.Text = "Set";
        B_Set.Location = new Point(16, 480);
        B_Set.Size = new Size(100, 30);
        B_Set.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        B_Set.Click += B_Set_Click;
        // 
        // buttons
        // 
        buttons.Controls.Add(B_Learn);
        buttons.Controls.Add(B_LearnAll);
        buttons.Controls.Add(L_Count);
        buttons.Dock = DockStyle.Bottom;
        buttons.Location = new Point(11, 731);
        buttons.Margin = new Padding(3, 4, 3, 4);
        buttons.Name = "buttons";
        buttons.Padding = new Padding(0, 8, 0, 0);
        buttons.Size = new Size(1029, 56);
        buttons.TabIndex = 1;
        // 
        // B_Learn
        // 
        B_Learn.Location = new Point(3, 12);
        B_Learn.Margin = new Padding(3, 4, 3, 4);
        B_Learn.Name = "B_Learn";
        B_Learn.Size = new Size(114, 40);
        B_Learn.TabIndex = 0;
        B_Learn.Text = "Learn";
        B_Learn.Click += B_Learn_Click;
        // 
        // B_LearnAll
        // 
        B_LearnAll.Location = new Point(123, 12);
        B_LearnAll.Margin = new Padding(3, 4, 3, 4);
        B_LearnAll.Name = "B_LearnAll";
        B_LearnAll.Size = new Size(114, 40);
        B_LearnAll.TabIndex = 1;
        B_LearnAll.Text = "Learn all";
        B_LearnAll.Click += B_LearnAll_Click;
        // 
        // L_Count
        // 
        L_Count.Location = new Point(243, 16);
        L_Count.Margin = new Padding(3, 8, 3, 0);
        L_Count.Name = "L_Count";
        L_Count.Size = new Size(601, 37);
        L_Count.TabIndex = 2;
        L_Count.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // AbilityEditor
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(split);
        Controls.Add(buttons);
        Margin = new Padding(3, 4, 3, 4);
        Name = "AbilityEditor";
        Padding = new Padding(11, 13, 11, 13);
        Size = new Size(1051, 800);
        split.Panel1.ResumeLayout(false);
        split.Panel2.ResumeLayout(false);
        ((ISupportInitialize)split).EndInit();
        split.ResumeLayout(false);
        ((ISupportInitialize)DGV_Abilities).EndInit();
        GB_Wheel.ResumeLayout(false);
        buttons.ResumeLayout(false);
        ResumeLayout(false);
    }

    private DataGridView DGV_Abilities = null!;
    private Button B_Learn = null!;
    private Button B_LearnAll = null!;
    private Button B_UnlockDive = null!;
    private Button B_UnlockFood = null!;
    private Button B_UnlockAbsorption = null!;
    private Label L_Count = null!;
    private GroupBox GB_Wheel = null!;
    private ListBox LB_Wheel = null!;
    private ComboBox CB_WheelAbility = null!;
    private Button B_Set = null!;
    private SplitContainer split;
    private FlowLayoutPanel buttons;
    private DataGridViewImageColumn dataGridViewImageColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
}
