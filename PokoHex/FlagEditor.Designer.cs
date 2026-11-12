using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PokoHex;

partial class FlagEditor
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
        DGV_Flags = new DataGridView();
        dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
        filters = new Panel();
        search = new Label();
        TB_Search = new TextBox();
        CHK_Known = new CheckBox();
        L_Count = new Label();
        GB_Flag = new GroupBox();
        L_Name = new Label();
        CHK_Enabled = new CheckBox();
        B_Set = new Button();
        warning = new Label();
        ((ISupportInitialize)split).BeginInit();
        split.Panel1.SuspendLayout();
        split.Panel2.SuspendLayout();
        split.SuspendLayout();
        ((ISupportInitialize)DGV_Flags).BeginInit();
        filters.SuspendLayout();
        GB_Flag.SuspendLayout();
        SuspendLayout();
        // 
        // split
        // 
        split.Dock = DockStyle.Fill;
        split.FixedPanel = FixedPanel.Panel2;
        split.Location = new Point(11, 66);
        split.Margin = new Padding(3, 4, 3, 4);
        split.Name = "split";
        // 
        // split.Panel1
        // 
        split.Panel1.Controls.Add(DGV_Flags);
        split.Panel1.Controls.Add(filters);
        split.Panel1.Controls.Add(L_Count);
        split.Panel1MinSize = 260;
        // 
        // split.Panel2
        // 
        split.Panel2.Controls.Add(GB_Flag);
        split.Panel2MinSize = 240;
        split.Size = new Size(1029, 721);
        split.SplitterDistance = 684;
        split.SplitterWidth = 5;
        split.TabIndex = 0;
        // 
        // DGV_Flags
        // 
        DGV_Flags.AllowUserToAddRows = false;
        DGV_Flags.AllowUserToDeleteRows = false;
        DGV_Flags.AllowUserToResizeRows = false;
        DGV_Flags.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        DGV_Flags.BackgroundColor = SystemColors.Window;
        DGV_Flags.ColumnHeadersHeight = 29;
        DGV_Flags.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewCheckBoxColumn1 });
        DGV_Flags.Dock = DockStyle.Fill;
        DGV_Flags.Location = new Point(0, 91);
        DGV_Flags.Margin = new Padding(3, 4, 3, 4);
        DGV_Flags.MultiSelect = false;
        DGV_Flags.Name = "DGV_Flags";
        DGV_Flags.ReadOnly = true;
        DGV_Flags.RowHeadersVisible = false;
        DGV_Flags.RowHeadersWidth = 51;
        DGV_Flags.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DGV_Flags.Size = new Size(684, 593);
        DGV_Flags.TabIndex = 0;
        DGV_Flags.CellContentClick += DGV_Flags_CellContentClick;
        DGV_Flags.CurrentCellChanged += DGV_Flags_CurrentCellChanged;
        // 
        // dataGridViewTextBoxColumn1
        // 
        dataGridViewTextBoxColumn1.MinimumWidth = 6;
        dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
        dataGridViewTextBoxColumn1.ReadOnly = true;
        // 
        // dataGridViewTextBoxColumn2
        // 
        dataGridViewTextBoxColumn2.MinimumWidth = 6;
        dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
        dataGridViewTextBoxColumn2.ReadOnly = true;
        // 
        // dataGridViewCheckBoxColumn1
        // 
        dataGridViewCheckBoxColumn1.MinimumWidth = 6;
        dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
        dataGridViewCheckBoxColumn1.ReadOnly = true;
        // 
        // filters
        // 
        filters.Controls.Add(search);
        filters.Controls.Add(TB_Search);
        filters.Controls.Add(CHK_Known);
        filters.Dock = DockStyle.Top;
        filters.Location = new Point(0, 0);
        filters.Margin = new Padding(3, 4, 3, 4);
        filters.Name = "filters";
        filters.Size = new Size(684, 91);
        filters.TabIndex = 1;
        // 
        // search
        // 
        search.AutoSize = true;
        search.Location = new Point(0, 11);
        search.Name = "search";
        search.Size = new Size(56, 20);
        search.TabIndex = 0;
        search.Text = "Search:";
        // 
        // TB_Search
        // 
        TB_Search.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TB_Search.Location = new Point(69, 5);
        TB_Search.Margin = new Padding(3, 4, 3, 4);
        TB_Search.Name = "TB_Search";
        TB_Search.Size = new Size(603, 27);
        TB_Search.TabIndex = 1;
        TB_Search.TextChanged += Filter_Changed;
        // 
        // CHK_Known
        // 
        CHK_Known.AutoSize = true;
        CHK_Known.Location = new Point(69, 51);
        CHK_Known.Margin = new Padding(3, 4, 3, 4);
        CHK_Known.Name = "CHK_Known";
        CHK_Known.Size = new Size(108, 24);
        CHK_Known.TabIndex = 2;
        CHK_Known.Text = "Known only";
        CHK_Known.CheckedChanged += Filter_Changed;
        // 
        // L_Count
        // 
        L_Count.Dock = DockStyle.Bottom;
        L_Count.Location = new Point(0, 684);
        L_Count.Name = "L_Count";
        L_Count.Size = new Size(684, 37);
        L_Count.TabIndex = 2;
        L_Count.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // GB_Flag
        // 
        GB_Flag.Controls.Add(L_Name);
        GB_Flag.Controls.Add(CHK_Enabled);
        GB_Flag.Controls.Add(B_Set);
        GB_Flag.Dock = DockStyle.Fill;
        GB_Flag.Location = new Point(0, 0);
        GB_Flag.Margin = new Padding(3, 4, 3, 4);
        GB_Flag.Name = "GB_Flag";
        GB_Flag.Padding = new Padding(3, 4, 3, 4);
        GB_Flag.Size = new Size(340, 721);
        GB_Flag.TabIndex = 0;
        GB_Flag.TabStop = false;
        GB_Flag.Text = "Flag";
        // 
        // L_Name
        // 
        L_Name.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        L_Name.Location = new Point(18, 37);
        L_Name.Name = "L_Name";
        L_Name.Size = new Size(299, 93);
        L_Name.TabIndex = 0;
        // 
        // CHK_Enabled
        // 
        CHK_Enabled.AutoSize = true;
        CHK_Enabled.Location = new Point(18, 149);
        CHK_Enabled.Margin = new Padding(3, 4, 3, 4);
        CHK_Enabled.Name = "CHK_Enabled";
        CHK_Enabled.Size = new Size(85, 24);
        CHK_Enabled.TabIndex = 1;
        CHK_Enabled.Text = "Enabled";
        CHK_Enabled.CheckedChanged += CHK_Enabled_CheckedChanged;
        // 
        // B_Set
        // 
        B_Set.Location = new Point(18, 200);
        B_Set.Margin = new Padding(3, 4, 3, 4);
        B_Set.Name = "B_Set";
        B_Set.Size = new Size(114, 40);
        B_Set.TabIndex = 2;
        B_Set.Text = "Set";
        B_Set.Click += B_Set_Click;
        // 
        // warning
        // 
        warning.Dock = DockStyle.Top;
        warning.Location = new Point(11, 13);
        warning.Name = "warning";
        warning.Size = new Size(1029, 53);
        warning.TabIndex = 1;
        warning.Text = "Warning: Flags may affect gameplay progression, toggle these at your own risk.";
        // 
        // FlagEditor
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(split);
        Controls.Add(warning);
        Margin = new Padding(3, 4, 3, 4);
        Name = "FlagEditor";
        Padding = new Padding(11, 13, 11, 13);
        Size = new Size(1051, 800);
        split.Panel1.ResumeLayout(false);
        split.Panel2.ResumeLayout(false);
        ((ISupportInitialize)split).EndInit();
        split.ResumeLayout(false);
        ((ISupportInitialize)DGV_Flags).EndInit();
        filters.ResumeLayout(false);
        filters.PerformLayout();
        GB_Flag.ResumeLayout(false);
        GB_Flag.PerformLayout();
        ResumeLayout(false);
    }

    private TextBox TB_Search = null!;
    private CheckBox CHK_Known = null!;
    private DataGridView DGV_Flags = null!;
    private Label L_Count = null!;
    private GroupBox GB_Flag = null!;
    private Label L_Name = null!;
    private CheckBox CHK_Enabled = null!;
    private Button B_Set = null!;
    private SplitContainer split;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private Panel filters;
    private Label search;
    private Label warning;
}
