using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PokoHex;

partial class ItemPaintEditor
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
        paintingBody = new Panel();
        swatches = new Panel();
        SC_Swatches = new SplitContainer();
        FLP_ColorSwatches = new FlowLayoutPanel();
        FLP_PatternSwatches = new FlowLayoutPanel();
        TLP_RadioButtons = new TableLayoutPanel();
        RB_Pattern = new RadioButton();
        RB_Paint = new RadioButton();
        head = new Panel();
        TLP_Slots = new TableLayoutPanel();
        B_Apply = new Button();
        itemContainer = new Panel();
        panel2 = new Panel();
        PB_Item = new PictureBox();
        L_Item = new Label();
        slot3container = new Panel();
        slot3selected = new Panel();
        PB_Slot3 = new PictureBox();
        L_PaintSlot3 = new Label();
        slot2container = new Panel();
        slot2selected = new Panel();
        PB_Slot2 = new PictureBox();
        L_PaintSlot2 = new Label();
        slot1container = new Panel();
        slot1selected = new Panel();
        PB_Slot1 = new PictureBox();
        L_PaintSlot1 = new Label();
        L_NoPaint = new Label();
        paintingBody.SuspendLayout();
        swatches.SuspendLayout();
        ((ISupportInitialize)SC_Swatches).BeginInit();
        SC_Swatches.Panel1.SuspendLayout();
        SC_Swatches.Panel2.SuspendLayout();
        SC_Swatches.SuspendLayout();
        TLP_RadioButtons.SuspendLayout();
        head.SuspendLayout();
        TLP_Slots.SuspendLayout();
        itemContainer.SuspendLayout();
        panel2.SuspendLayout();
        ((ISupportInitialize)PB_Item).BeginInit();
        slot3container.SuspendLayout();
        slot3selected.SuspendLayout();
        ((ISupportInitialize)PB_Slot3).BeginInit();
        slot2container.SuspendLayout();
        slot2selected.SuspendLayout();
        ((ISupportInitialize)PB_Slot2).BeginInit();
        slot1container.SuspendLayout();
        slot1selected.SuspendLayout();
        ((ISupportInitialize)PB_Slot1).BeginInit();
        SuspendLayout();
        // 
        // paintingBody
        // 
        paintingBody.BackColor = SystemColors.Control;
        paintingBody.Controls.Add(swatches);
        paintingBody.Controls.Add(head);
        paintingBody.Dock = DockStyle.Fill;
        paintingBody.Location = new Point(0, 0);
        paintingBody.Name = "paintingBody";
        paintingBody.Size = new Size(1347, 850);
        paintingBody.TabIndex = 2;
        // 
        // swatches
        // 
        swatches.Controls.Add(SC_Swatches);
        swatches.Controls.Add(TLP_RadioButtons);
        swatches.Dock = DockStyle.Fill;
        swatches.Location = new Point(0, 210);
        swatches.Name = "swatches";
        swatches.Size = new Size(1347, 640);
        swatches.TabIndex = 4;
        // 
        // SC_Swatches
        // 
        SC_Swatches.Dock = DockStyle.Fill;
        SC_Swatches.IsSplitterFixed = true;
        SC_Swatches.Location = new Point(0, 43);
        SC_Swatches.Name = "SC_Swatches";
        // 
        // SC_Swatches.Panel1
        // 
        SC_Swatches.Panel1.Controls.Add(FLP_ColorSwatches);
        // 
        // SC_Swatches.Panel2
        // 
        SC_Swatches.Panel2.Controls.Add(FLP_PatternSwatches);
        SC_Swatches.Size = new Size(1347, 597);
        SC_Swatches.SplitterDistance = 669;
        SC_Swatches.TabIndex = 6;
        SC_Swatches.Resize += SC_Swatches_Resize;
        // 
        // FLP_ColorSwatches
        // 
        FLP_ColorSwatches.AutoScroll = true;
        FLP_ColorSwatches.BackColor = SystemColors.Control;
        FLP_ColorSwatches.Dock = DockStyle.Fill;
        FLP_ColorSwatches.Location = new Point(0, 0);
        FLP_ColorSwatches.Name = "FLP_ColorSwatches";
        FLP_ColorSwatches.Size = new Size(669, 597);
        FLP_ColorSwatches.TabIndex = 2;
        // 
        // FLP_PatternSwatches
        // 
        FLP_PatternSwatches.AutoScroll = true;
        FLP_PatternSwatches.BackColor = SystemColors.Control;
        FLP_PatternSwatches.Dock = DockStyle.Fill;
        FLP_PatternSwatches.Location = new Point(0, 0);
        FLP_PatternSwatches.Name = "FLP_PatternSwatches";
        FLP_PatternSwatches.Size = new Size(674, 597);
        FLP_PatternSwatches.TabIndex = 2;
        // 
        // TLP_RadioButtons
        // 
        TLP_RadioButtons.ColumnCount = 4;
        TLP_RadioButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.75F));
        TLP_RadioButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31.25F));
        TLP_RadioButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31.25F));
        TLP_RadioButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.75F));
        TLP_RadioButtons.Controls.Add(RB_Pattern, 2, 0);
        TLP_RadioButtons.Controls.Add(RB_Paint, 1, 0);
        TLP_RadioButtons.Dock = DockStyle.Top;
        TLP_RadioButtons.Location = new Point(0, 0);
        TLP_RadioButtons.Name = "TLP_RadioButtons";
        TLP_RadioButtons.RowCount = 1;
        TLP_RadioButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TLP_RadioButtons.Size = new Size(1347, 43);
        TLP_RadioButtons.TabIndex = 4;
        // 
        // RB_Pattern
        // 
        RB_Pattern.Appearance = Appearance.Button;
        RB_Pattern.Dock = DockStyle.Fill;
        RB_Pattern.Location = new Point(675, 3);
        RB_Pattern.Name = "RB_Pattern";
        RB_Pattern.Size = new Size(414, 37);
        RB_Pattern.TabIndex = 1;
        RB_Pattern.Text = "Pattern";
        RB_Pattern.TextAlign = ContentAlignment.MiddleCenter;
        RB_Pattern.CheckedChanged += RB_Pattern_CheckedChanged;
        // 
        // RB_Paint
        // 
        RB_Paint.Appearance = Appearance.Button;
        RB_Paint.Checked = true;
        RB_Paint.Dock = DockStyle.Fill;
        RB_Paint.Location = new Point(255, 3);
        RB_Paint.Name = "RB_Paint";
        RB_Paint.Size = new Size(414, 37);
        RB_Paint.TabIndex = 0;
        RB_Paint.TabStop = true;
        RB_Paint.Text = "Paint";
        RB_Paint.TextAlign = ContentAlignment.MiddleCenter;
        RB_Paint.CheckedChanged += RB_Paint_CheckedChanged;
        // 
        // head
        // 
        head.Controls.Add(TLP_Slots);
        head.Dock = DockStyle.Top;
        head.Location = new Point(0, 0);
        head.Name = "head";
        head.Size = new Size(1347, 210);
        head.TabIndex = 3;
        // 
        // TLP_Slots
        // 
        TLP_Slots.ColumnCount = 5;
        TLP_Slots.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        TLP_Slots.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Slots.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Slots.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
        TLP_Slots.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        TLP_Slots.Controls.Add(B_Apply, 4, 0);
        TLP_Slots.Controls.Add(itemContainer, 0, 0);
        TLP_Slots.Controls.Add(slot3container, 3, 0);
        TLP_Slots.Controls.Add(slot2container, 2, 0);
        TLP_Slots.Controls.Add(slot1container, 1, 0);
        TLP_Slots.Dock = DockStyle.Fill;
        TLP_Slots.Location = new Point(0, 0);
        TLP_Slots.Name = "TLP_Slots";
        TLP_Slots.Padding = new Padding(5);
        TLP_Slots.RowCount = 1;
        TLP_Slots.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TLP_Slots.Size = new Size(1347, 210);
        TLP_Slots.TabIndex = 6;
        // 
        // B_Apply
        // 
        B_Apply.Anchor = AnchorStyles.None;
        B_Apply.Location = new Point(1124, 87);
        B_Apply.Name = "B_Apply";
        B_Apply.Size = new Size(99, 35);
        B_Apply.TabIndex = 8;
        B_Apply.Text = "Apply";
        B_Apply.UseVisualStyleBackColor = true;
        B_Apply.Click += B_Apply_Click;
        // 
        // itemContainer
        // 
        itemContainer.Anchor = AnchorStyles.None;
        itemContainer.Controls.Add(panel2);
        itemContainer.Controls.Add(L_Item);
        itemContainer.Location = new Point(81, 39);
        itemContainer.Name = "itemContainer";
        itemContainer.Size = new Size(182, 132);
        itemContainer.TabIndex = 6;
        // 
        // panel2
        // 
        panel2.BackColor = Color.Transparent;
        panel2.Controls.Add(PB_Item);
        panel2.Dock = DockStyle.Fill;
        panel2.Location = new Point(0, 22);
        panel2.Name = "panel2";
        panel2.Padding = new Padding(50, 5, 50, 5);
        panel2.Size = new Size(182, 110);
        panel2.TabIndex = 5;
        // 
        // PB_Item
        // 
        PB_Item.BackColor = SystemColors.Control;
        PB_Item.BackgroundImageLayout = ImageLayout.Zoom;
        PB_Item.Dock = DockStyle.Fill;
        PB_Item.Location = new Point(50, 5);
        PB_Item.Name = "PB_Item";
        PB_Item.Size = new Size(82, 100);
        PB_Item.SizeMode = PictureBoxSizeMode.Zoom;
        PB_Item.TabIndex = 5;
        PB_Item.TabStop = false;
        PB_Item.Tag = "0";
        // 
        // L_Item
        // 
        L_Item.Dock = DockStyle.Top;
        L_Item.Location = new Point(0, 0);
        L_Item.Name = "L_Item";
        L_Item.Size = new Size(182, 22);
        L_Item.TabIndex = 0;
        L_Item.Text = "Selected item: (slot 000)";
        L_Item.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // slot3container
        // 
        slot3container.Anchor = AnchorStyles.None;
        slot3container.Controls.Add(slot3selected);
        slot3container.Controls.Add(L_PaintSlot3);
        slot3container.Location = new Point(839, 39);
        slot3container.Name = "slot3container";
        slot3container.Size = new Size(110, 132);
        slot3container.TabIndex = 5;
        // 
        // slot3selected
        // 
        slot3selected.BackColor = Color.Transparent;
        slot3selected.Controls.Add(PB_Slot3);
        slot3selected.Dock = DockStyle.Fill;
        slot3selected.Location = new Point(0, 22);
        slot3selected.Name = "slot3selected";
        slot3selected.Padding = new Padding(5);
        slot3selected.Size = new Size(110, 110);
        slot3selected.TabIndex = 5;
        // 
        // PB_Slot3
        // 
        PB_Slot3.BackColor = SystemColors.Control;
        PB_Slot3.BackgroundImageLayout = ImageLayout.Zoom;
        PB_Slot3.Dock = DockStyle.Fill;
        PB_Slot3.Location = new Point(5, 5);
        PB_Slot3.Name = "PB_Slot3";
        PB_Slot3.Size = new Size(100, 100);
        PB_Slot3.SizeMode = PictureBoxSizeMode.Zoom;
        PB_Slot3.TabIndex = 1;
        PB_Slot3.TabStop = false;
        PB_Slot3.Tag = "2";
        PB_Slot3.Click += Slot_Selected;
        // 
        // L_PaintSlot3
        // 
        L_PaintSlot3.Dock = DockStyle.Top;
        L_PaintSlot3.Location = new Point(0, 0);
        L_PaintSlot3.Name = "L_PaintSlot3";
        L_PaintSlot3.Size = new Size(110, 22);
        L_PaintSlot3.TabIndex = 0;
        L_PaintSlot3.Text = "Slot 3";
        L_PaintSlot3.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // slot2container
        // 
        slot2container.Anchor = AnchorStyles.None;
        slot2container.Controls.Add(slot2selected);
        slot2container.Controls.Add(L_PaintSlot2);
        slot2container.Location = new Point(617, 39);
        slot2container.Name = "slot2container";
        slot2container.Size = new Size(110, 132);
        slot2container.TabIndex = 4;
        // 
        // slot2selected
        // 
        slot2selected.BackColor = Color.Transparent;
        slot2selected.Controls.Add(PB_Slot2);
        slot2selected.Dock = DockStyle.Fill;
        slot2selected.Location = new Point(0, 22);
        slot2selected.Name = "slot2selected";
        slot2selected.Padding = new Padding(5);
        slot2selected.Size = new Size(110, 110);
        slot2selected.TabIndex = 5;
        // 
        // PB_Slot2
        // 
        PB_Slot2.BackColor = SystemColors.Control;
        PB_Slot2.BackgroundImageLayout = ImageLayout.Zoom;
        PB_Slot2.Dock = DockStyle.Fill;
        PB_Slot2.Location = new Point(5, 5);
        PB_Slot2.Name = "PB_Slot2";
        PB_Slot2.Size = new Size(100, 100);
        PB_Slot2.SizeMode = PictureBoxSizeMode.Zoom;
        PB_Slot2.TabIndex = 1;
        PB_Slot2.TabStop = false;
        PB_Slot2.Tag = "1";
        PB_Slot2.Click += Slot_Selected;
        // 
        // L_PaintSlot2
        // 
        L_PaintSlot2.Dock = DockStyle.Top;
        L_PaintSlot2.Location = new Point(0, 0);
        L_PaintSlot2.Name = "L_PaintSlot2";
        L_PaintSlot2.Size = new Size(110, 22);
        L_PaintSlot2.TabIndex = 0;
        L_PaintSlot2.Text = "Slot 2";
        L_PaintSlot2.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // slot1container
        // 
        slot1container.Anchor = AnchorStyles.None;
        slot1container.Controls.Add(slot1selected);
        slot1container.Controls.Add(L_PaintSlot1);
        slot1container.Location = new Point(395, 39);
        slot1container.Name = "slot1container";
        slot1container.Size = new Size(110, 132);
        slot1container.TabIndex = 4;
        // 
        // slot1selected
        // 
        slot1selected.BackColor = SystemColors.Highlight;
        slot1selected.Controls.Add(PB_Slot1);
        slot1selected.Dock = DockStyle.Fill;
        slot1selected.Location = new Point(0, 22);
        slot1selected.Name = "slot1selected";
        slot1selected.Padding = new Padding(5);
        slot1selected.Size = new Size(110, 110);
        slot1selected.TabIndex = 5;
        // 
        // PB_Slot1
        // 
        PB_Slot1.BackColor = SystemColors.Control;
        PB_Slot1.BackgroundImageLayout = ImageLayout.Zoom;
        PB_Slot1.Dock = DockStyle.Fill;
        PB_Slot1.Location = new Point(5, 5);
        PB_Slot1.Name = "PB_Slot1";
        PB_Slot1.Size = new Size(100, 100);
        PB_Slot1.SizeMode = PictureBoxSizeMode.Zoom;
        PB_Slot1.TabIndex = 1;
        PB_Slot1.TabStop = false;
        PB_Slot1.Tag = "0";
        PB_Slot1.Click += Slot_Selected;
        // 
        // L_PaintSlot1
        // 
        L_PaintSlot1.Dock = DockStyle.Top;
        L_PaintSlot1.Location = new Point(0, 0);
        L_PaintSlot1.Name = "L_PaintSlot1";
        L_PaintSlot1.Size = new Size(110, 22);
        L_PaintSlot1.TabIndex = 0;
        L_PaintSlot1.Text = "Slot 1";
        L_PaintSlot1.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // L_NoPaint
        // 
        L_NoPaint.BackColor = SystemColors.Control;
        L_NoPaint.Dock = DockStyle.Fill;
        L_NoPaint.Location = new Point(0, 0);
        L_NoPaint.Name = "L_NoPaint";
        L_NoPaint.Size = new Size(1347, 850);
        L_NoPaint.TabIndex = 1;
        L_NoPaint.Text = "Select a paintable item first";
        L_NoPaint.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // ItemPaintEditor
        // 
        BackColor = SystemColors.ActiveCaption;
        Controls.Add(paintingBody);
        Controls.Add(L_NoPaint);
        Name = "ItemPaintEditor";
        Size = new Size(1347, 850);
        paintingBody.ResumeLayout(false);
        swatches.ResumeLayout(false);
        SC_Swatches.Panel1.ResumeLayout(false);
        SC_Swatches.Panel2.ResumeLayout(false);
        ((ISupportInitialize)SC_Swatches).EndInit();
        SC_Swatches.ResumeLayout(false);
        TLP_RadioButtons.ResumeLayout(false);
        head.ResumeLayout(false);
        TLP_Slots.ResumeLayout(false);
        itemContainer.ResumeLayout(false);
        panel2.ResumeLayout(false);
        ((ISupportInitialize)PB_Item).EndInit();
        slot3container.ResumeLayout(false);
        slot3selected.ResumeLayout(false);
        ((ISupportInitialize)PB_Slot3).EndInit();
        slot2container.ResumeLayout(false);
        slot2selected.ResumeLayout(false);
        ((ISupportInitialize)PB_Slot2).EndInit();
        slot1container.ResumeLayout(false);
        slot1selected.ResumeLayout(false);
        ((ISupportInitialize)PB_Slot1).EndInit();
        ResumeLayout(false);
    }
    private Panel paintingBody;
    private Label L_NoPaint;
    private Panel head;
    private Label L_PaintSlot1;
    private FlowLayoutPanel flowLayoutPanel1;
    private Panel slot1container;
    private FlowLayoutPanel paintMode;
    private RadioButton RB_Paint;
    private RadioButton RB_Pattern;
    private Panel swatches;
    private Panel slot2container;
    private PictureBox PB_Slot2;
    private Label L_PaintSlot2;
    private PictureBox PB_Slot1;
    private TableLayoutPanel TLP_RadioButtons;
    private FlowLayoutPanel FLP_PatternSwatches;
    private FlowLayoutPanel FLP_ColorSwatches;
    private Panel slot1selected;
    private Panel slot2selected;
    private PictureBox PB_Item;
    private TableLayoutPanel TLP_Slots;
    private Panel slot3container;
    private Panel slot3selected;
    private PictureBox PB_Slot3;
    private Label L_PaintSlot3;
    private SplitContainer SC_Swatches;
    private Button B_Apply;
    private Panel itemContainer;
    private Panel panel2;
    private Label L_Item;
}
