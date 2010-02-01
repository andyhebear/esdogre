namespace MyOgre
{
    partial class MainOgreForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainOgreForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.file_item = new System.Windows.Forms.ToolStripMenuItem();
            this.open_item = new System.Windows.Forms.ToolStripMenuItem();
            this.close_item = new System.Windows.Forms.ToolStripMenuItem();
            this.exit_item = new System.Windows.Forms.ToolStripMenuItem();
            this.操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.move_item = new System.Windows.Forms.ToolStripMenuItem();
            this.更换漫游方式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_AllView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_QueryInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_Rain = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_snow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_sun = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_People = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_help = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.tianxia_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.shanglai_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.zuozuan_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.youzuan_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.manyou_toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton16 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.axWindowsMediaPlayer1 = new AxWMPLib.AxWindowsMediaPlayer();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.remark_textBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.name_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.menuStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file_item,
            this.操作ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(648, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // file_item
            // 
            this.file_item.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.open_item,
            this.close_item,
            this.exit_item});
            this.file_item.Name = "file_item";
            this.file_item.Size = new System.Drawing.Size(65, 20);
            this.file_item.Text = "场景管理";
            // 
            // open_item
            // 
            this.open_item.Name = "open_item";
            this.open_item.Size = new System.Drawing.Size(94, 22);
            this.open_item.Text = "打开";
            // 
            // close_item
            // 
            this.close_item.Name = "close_item";
            this.close_item.Size = new System.Drawing.Size(94, 22);
            this.close_item.Text = "关闭";
            this.close_item.Click += new System.EventHandler(this.关闭ToolStripMenuItem_Click);
            // 
            // exit_item
            // 
            this.exit_item.Name = "exit_item";
            this.exit_item.Size = new System.Drawing.Size(94, 22);
            this.exit_item.Text = "退出";
            this.exit_item.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 操作ToolStripMenuItem
            // 
            this.操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.move_item,
            this.更换漫游方式ToolStripMenuItem,
            this.toolStripMenuItem_AllView,
            this.toolStripMenuItem_QueryInfo});
            this.操作ToolStripMenuItem.Name = "操作ToolStripMenuItem";
            this.操作ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.操作ToolStripMenuItem.Text = "场景操作";
            // 
            // move_item
            // 
            this.move_item.Name = "move_item";
            this.move_item.Size = new System.Drawing.Size(124, 22);
            this.move_item.Text = "手动漫游";
            this.move_item.Click += new System.EventHandler(this.move_item_Click);
            // 
            // 更换漫游方式ToolStripMenuItem
            // 
            this.更换漫游方式ToolStripMenuItem.Name = "更换漫游方式ToolStripMenuItem";
            this.更换漫游方式ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.更换漫游方式ToolStripMenuItem.Text = "跟随浏览 ";
            this.更换漫游方式ToolStripMenuItem.Click += new System.EventHandler(this.更换漫游方式ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem_AllView
            // 
            this.toolStripMenuItem_AllView.Name = "toolStripMenuItem_AllView";
            this.toolStripMenuItem_AllView.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem_AllView.Text = "全景";
            this.toolStripMenuItem_AllView.Click += new System.EventHandler(this.toolStripMenuItem_AllView_Click);
            // 
            // toolStripMenuItem_QueryInfo
            // 
            this.toolStripMenuItem_QueryInfo.Name = "toolStripMenuItem_QueryInfo";
            this.toolStripMenuItem_QueryInfo.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem_QueryInfo.Text = "信息查询";
            this.toolStripMenuItem_QueryInfo.Click += new System.EventHandler(this.toolStripMenuItem_QueryInfo_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_Rain,
            this.toolStripMenuItem_snow,
            this.toolStripMenuItem_sun,
            this.toolStripMenuItem_People});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(65, 20);
            this.toolStripMenuItem1.Text = "场景效果";
            // 
            // toolStripMenuItem_Rain
            // 
            this.toolStripMenuItem_Rain.Name = "toolStripMenuItem_Rain";
            this.toolStripMenuItem_Rain.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem_Rain.Text = "雨";
            this.toolStripMenuItem_Rain.Click += new System.EventHandler(this.toolStripMenuItem_Rain_Click);
            // 
            // toolStripMenuItem_snow
            // 
            this.toolStripMenuItem_snow.Name = "toolStripMenuItem_snow";
            this.toolStripMenuItem_snow.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem_snow.Text = "雪";
            this.toolStripMenuItem_snow.Click += new System.EventHandler(this.toolStripMenuItem_snow_Click);
            // 
            // toolStripMenuItem_sun
            // 
            this.toolStripMenuItem_sun.Name = "toolStripMenuItem_sun";
            this.toolStripMenuItem_sun.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem_sun.Text = "光照";
            this.toolStripMenuItem_sun.Click += new System.EventHandler(this.toolStripMenuItem_sun_Click);
            // 
            // toolStripMenuItem_People
            // 
            this.toolStripMenuItem_People.Name = "toolStripMenuItem_People";
            this.toolStripMenuItem_People.Size = new System.Drawing.Size(94, 22);
            this.toolStripMenuItem_People.Text = "人";
            this.toolStripMenuItem_People.Click += new System.EventHandler(this.toolStripMenuItem_People_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_help,
            this.关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // toolStripMenuItem_help
            // 
            this.toolStripMenuItem_help.Name = "toolStripMenuItem_help";
            this.toolStripMenuItem_help.Size = new System.Drawing.Size(118, 22);
            this.toolStripMenuItem_help.Text = "关于系统";
            this.toolStripMenuItem_help.Click += new System.EventHandler(this.toolStripMenuItem_help_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.关于ToolStripMenuItem.Text = "帮助";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(64, 64);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "所有文件|*.*|模型文件|*.xml";
            this.openFileDialog1.RestoreDirectory = true;
            // 
            // toolStrip2
            // 
            this.toolStrip2.AutoSize = false;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tianxia_toolStripButton,
            this.shanglai_toolStripButton,
            this.zuozuan_toolStripButton,
            this.youzuan_toolStripButton,
            this.manyou_toolStripButton,
            this.toolStripButton12,
            this.toolStripButton13,
            this.toolStripButton16});
            this.toolStrip2.Location = new System.Drawing.Point(0, 24);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(648, 37);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // tianxia_toolStripButton
            // 
            this.tianxia_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tianxia_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("tianxia_toolStripButton.Image")));
            this.tianxia_toolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tianxia_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tianxia_toolStripButton.Name = "tianxia_toolStripButton";
            this.tianxia_toolStripButton.Size = new System.Drawing.Size(36, 34);
            this.tianxia_toolStripButton.Text = "躺下";
            this.tianxia_toolStripButton.ToolTipText = "透视";
            this.tianxia_toolStripButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tooli_toolStripButton_MouseUp);
            this.tianxia_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tooli_toolStripButton_MouseDown);
            this.tianxia_toolStripButton.MouseLeave += new System.EventHandler(this.tooli_toolStripButton_MouseLeave);
            // 
            // shanglai_toolStripButton
            // 
            this.shanglai_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.shanglai_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("shanglai_toolStripButton.Image")));
            this.shanglai_toolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.shanglai_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.shanglai_toolStripButton.Name = "shanglai_toolStripButton";
            this.shanglai_toolStripButton.Size = new System.Drawing.Size(36, 34);
            this.shanglai_toolStripButton.Text = "toolStripButton2";
            this.shanglai_toolStripButton.ToolTipText = "俯视";
            this.shanglai_toolStripButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tooli_toolStripButton_MouseUp);
            this.shanglai_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tooli_toolStripButton_MouseDown);
            this.shanglai_toolStripButton.MouseLeave += new System.EventHandler(this.tooli_toolStripButton_MouseLeave);
            // 
            // zuozuan_toolStripButton
            // 
            this.zuozuan_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.zuozuan_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("zuozuan_toolStripButton.Image")));
            this.zuozuan_toolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zuozuan_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zuozuan_toolStripButton.Name = "zuozuan_toolStripButton";
            this.zuozuan_toolStripButton.Size = new System.Drawing.Size(36, 34);
            this.zuozuan_toolStripButton.Text = "toolStripButton3";
            this.zuozuan_toolStripButton.ToolTipText = "左转";
            this.zuozuan_toolStripButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tooli_toolStripButton_MouseUp);
            this.zuozuan_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tooli_toolStripButton_MouseDown);
            this.zuozuan_toolStripButton.MouseLeave += new System.EventHandler(this.tooli_toolStripButton_MouseLeave);
            // 
            // youzuan_toolStripButton
            // 
            this.youzuan_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.youzuan_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("youzuan_toolStripButton.Image")));
            this.youzuan_toolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.youzuan_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.youzuan_toolStripButton.Name = "youzuan_toolStripButton";
            this.youzuan_toolStripButton.Size = new System.Drawing.Size(36, 34);
            this.youzuan_toolStripButton.Text = "toolStripButton4";
            this.youzuan_toolStripButton.ToolTipText = "右转";
            this.youzuan_toolStripButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tooli_toolStripButton_MouseUp);
            this.youzuan_toolStripButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tooli_toolStripButton_MouseDown);
            this.youzuan_toolStripButton.MouseLeave += new System.EventHandler(this.tooli_toolStripButton_MouseLeave);
            // 
            // manyou_toolStripButton
            // 
            this.manyou_toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.manyou_toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("manyou_toolStripButton.Image")));
            this.manyou_toolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.manyou_toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.manyou_toolStripButton.Name = "manyou_toolStripButton";
            this.manyou_toolStripButton.Size = new System.Drawing.Size(36, 34);
            this.manyou_toolStripButton.Text = "toolStripButton6";
            this.manyou_toolStripButton.ToolTipText = "漫游";
            this.manyou_toolStripButton.Click += new System.EventHandler(this.move_item_Click);
            // 
            // toolStripButton12
            // 
            this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton12.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton12.Image")));
            this.toolStripButton12.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton12.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton12.Name = "toolStripButton12";
            this.toolStripButton12.Size = new System.Drawing.Size(36, 34);
            this.toolStripButton12.Text = "toolStripButton7";
            this.toolStripButton12.ToolTipText = "全图";
            this.toolStripButton12.Click += new System.EventHandler(this.toolStripButton12_Click);
            // 
            // toolStripButton13
            // 
            this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton13.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton13.Image")));
            this.toolStripButton13.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton13.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton13.Name = "toolStripButton13";
            this.toolStripButton13.Size = new System.Drawing.Size(36, 34);
            this.toolStripButton13.Text = "选择模型";
            this.toolStripButton13.ToolTipText = "查询";
            this.toolStripButton13.Click += new System.EventHandler(this.select_item_Click);
            // 
            // toolStripButton16
            // 
            this.toolStripButton16.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton16.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton16.Image")));
            this.toolStripButton16.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton16.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton16.Name = "toolStripButton16";
            this.toolStripButton16.Size = new System.Drawing.Size(36, 34);
            this.toolStripButton16.Text = "帮助";
            this.toolStripButton16.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 61);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(648, 586);
            this.splitContainer1.SplitterDistance = 332;
            this.splitContainer1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(332, 586);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(312, 586);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.axWindowsMediaPlayer1);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.pictureBox);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.remark_textBox);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.name_textBox);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(304, 561);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "属性";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // axWindowsMediaPlayer1
            // 
            this.axWindowsMediaPlayer1.Enabled = true;
            this.axWindowsMediaPlayer1.Location = new System.Drawing.Point(58, 359);
            this.axWindowsMediaPlayer1.Name = "axWindowsMediaPlayer1";
            this.axWindowsMediaPlayer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWindowsMediaPlayer1.OcxState")));
            this.axWindowsMediaPlayer1.Size = new System.Drawing.Size(238, 194);
            this.axWindowsMediaPlayer1.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 359);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "视频:";
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(58, 172);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(238, 170);
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 172);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "图片:";
            // 
            // remark_textBox
            // 
            this.remark_textBox.Enabled = false;
            this.remark_textBox.Location = new System.Drawing.Point(58, 46);
            this.remark_textBox.Multiline = true;
            this.remark_textBox.Name = "remark_textBox";
            this.remark_textBox.Size = new System.Drawing.Size(238, 108);
            this.remark_textBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "介绍:";
            // 
            // name_textBox
            // 
            this.name_textBox.Enabled = false;
            this.name_textBox.Location = new System.Drawing.Point(58, 14);
            this.name_textBox.Name = "name_textBox";
            this.name_textBox.Size = new System.Drawing.Size(238, 21);
            this.name_textBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.treeView1);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(304, 561);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "模型树";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 3);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(298, 555);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // MainOgreForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 647);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainOgreForm";
            this.Text = "电子沙盘-浏览器";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainOgreForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainOgreForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWindowsMediaPlayer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem file_item;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem 操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem move_item;
        private System.Windows.Forms.ToolStripMenuItem open_item;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem close_item;
        private System.Windows.Forms.ToolStripMenuItem exit_item;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton tianxia_toolStripButton;
        private System.Windows.Forms.ToolStripButton shanglai_toolStripButton;
        private System.Windows.Forms.ToolStripButton zuozuan_toolStripButton;
        private System.Windows.Forms.ToolStripButton youzuan_toolStripButton;
        private System.Windows.Forms.ToolStripButton manyou_toolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripButton toolStripButton16;
        private System.Windows.Forms.SplitContainer splitContainer1;
        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox remark_textBox;
        public System.Windows.Forms.TextBox name_textBox;
        public System.Windows.Forms.PictureBox pictureBox;
        public AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
        private System.Windows.Forms.ToolStripMenuItem 更换漫游方式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_AllView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_QueryInfo;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Rain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_snow;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_sun;
        public System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_People;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_help;
    }
}

