namespace WOSNManager
{
    partial class frmMain
    {
        /// <summary>
        /// Vyžaduje se proměnná návrháře.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Uvolněte všechny používané prostředky.
        /// </summary>
        /// <param name="disposing">hodnota true, když by se měl spravovaný prostředek odstranit; jinak false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kód generovaný Návrhářem Windows Form

        /// <summary>
        /// Metoda vyžadovaná pro podporu Návrháře - neupravovat
        /// obsah této metody v editoru kódu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.wakeUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.screenshotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listProcesůToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zaslatZprávuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.zamknoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.odhlásitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartovatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vypnoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.souborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konecToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nástrojeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pingerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nápovědaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oProgramuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.listView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtZmenyNTFS = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Interval = 2000;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wakeUpToolStripMenuItem,
            this.toolStripSeparator1,
            this.screenshotToolStripMenuItem,
            this.listProcesůToolStripMenuItem,
            this.zaslatZprávuToolStripMenuItem,
            this.toolStripSeparator2,
            this.zamknoutToolStripMenuItem,
            this.odhlásitToolStripMenuItem,
            this.restartovatToolStripMenuItem,
            this.vypnoutToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(181, 214);
            // 
            // wakeUpToolStripMenuItem
            // 
            this.wakeUpToolStripMenuItem.Name = "wakeUpToolStripMenuItem";
            this.wakeUpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.wakeUpToolStripMenuItem.Text = "WakeUp";
            this.wakeUpToolStripMenuItem.Click += new System.EventHandler(this.wakeUpToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // screenshotToolStripMenuItem
            // 
            this.screenshotToolStripMenuItem.Name = "screenshotToolStripMenuItem";
            this.screenshotToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.screenshotToolStripMenuItem.Text = "Screenshot";
            this.screenshotToolStripMenuItem.Click += new System.EventHandler(this.screenshotToolStripMenuItem_Click);
            // 
            // listProcesůToolStripMenuItem
            // 
            this.listProcesůToolStripMenuItem.Name = "listProcesůToolStripMenuItem";
            this.listProcesůToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.listProcesůToolStripMenuItem.Text = "List procesů";
            this.listProcesůToolStripMenuItem.Click += new System.EventHandler(this.listProcesůToolStripMenuItem_Click);
            // 
            // zaslatZprávuToolStripMenuItem
            // 
            this.zaslatZprávuToolStripMenuItem.Name = "zaslatZprávuToolStripMenuItem";
            this.zaslatZprávuToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zaslatZprávuToolStripMenuItem.Text = "Zaslat zprávu";
            this.zaslatZprávuToolStripMenuItem.Click += new System.EventHandler(this.zaslatZprávuToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // zamknoutToolStripMenuItem
            // 
            this.zamknoutToolStripMenuItem.Name = "zamknoutToolStripMenuItem";
            this.zamknoutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.zamknoutToolStripMenuItem.Text = "Zamknout";
            this.zamknoutToolStripMenuItem.Click += new System.EventHandler(this.zamknoutToolStripMenuItem_Click);
            // 
            // odhlásitToolStripMenuItem
            // 
            this.odhlásitToolStripMenuItem.Name = "odhlásitToolStripMenuItem";
            this.odhlásitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.odhlásitToolStripMenuItem.Text = "Odhlásit";
            this.odhlásitToolStripMenuItem.Click += new System.EventHandler(this.odhlásitToolStripMenuItem_Click);
            // 
            // restartovatToolStripMenuItem
            // 
            this.restartovatToolStripMenuItem.Name = "restartovatToolStripMenuItem";
            this.restartovatToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.restartovatToolStripMenuItem.Text = "Restartovat";
            this.restartovatToolStripMenuItem.Click += new System.EventHandler(this.restartovatToolStripMenuItem_Click);
            // 
            // vypnoutToolStripMenuItem
            // 
            this.vypnoutToolStripMenuItem.Name = "vypnoutToolStripMenuItem";
            this.vypnoutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.vypnoutToolStripMenuItem.Text = "Vypnout";
            this.vypnoutToolStripMenuItem.Click += new System.EventHandler(this.vypnoutToolStripMenuItem_Click);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "button-blue.ico");
            this.imageList.Images.SetKeyName(1, "button-green.ico");
            this.imageList.Images.SetKeyName(2, "button-red.ico");
            this.imageList.Images.SetKeyName(3, "8794_48x48x32.ico");
            this.imageList.Images.SetKeyName(4, "button-grey.ico");
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.souborToolStripMenuItem,
            this.nástrojeToolStripMenuItem,
            this.nápovědaToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(434, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // souborToolStripMenuItem
            // 
            this.souborToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.konecToolStripMenuItem});
            this.souborToolStripMenuItem.Name = "souborToolStripMenuItem";
            this.souborToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.souborToolStripMenuItem.Text = "Soubor";
            // 
            // konecToolStripMenuItem
            // 
            this.konecToolStripMenuItem.Name = "konecToolStripMenuItem";
            this.konecToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.konecToolStripMenuItem.Text = "Konec";
            this.konecToolStripMenuItem.Click += new System.EventHandler(this.konecToolStripMenuItem_Click);
            // 
            // nástrojeToolStripMenuItem
            // 
            this.nástrojeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pingerToolStripMenuItem});
            this.nástrojeToolStripMenuItem.Name = "nástrojeToolStripMenuItem";
            this.nástrojeToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.nástrojeToolStripMenuItem.Text = "Nástroje";
            // 
            // pingerToolStripMenuItem
            // 
            this.pingerToolStripMenuItem.Checked = true;
            this.pingerToolStripMenuItem.CheckOnClick = true;
            this.pingerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.pingerToolStripMenuItem.Name = "pingerToolStripMenuItem";
            this.pingerToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.pingerToolStripMenuItem.Text = "Pinger";
            this.pingerToolStripMenuItem.Click += new System.EventHandler(this.pingerToolStripMenuItem_Click);
            // 
            // nápovědaToolStripMenuItem
            // 
            this.nápovědaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.oProgramuToolStripMenuItem});
            this.nápovědaToolStripMenuItem.Name = "nápovědaToolStripMenuItem";
            this.nápovědaToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.nápovědaToolStripMenuItem.Text = "Nápověda";
            // 
            // oProgramuToolStripMenuItem
            // 
            this.oProgramuToolStripMenuItem.Name = "oProgramuToolStripMenuItem";
            this.oProgramuToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.oProgramuToolStripMenuItem.Text = "O programu";
            this.oProgramuToolStripMenuItem.Click += new System.EventHandler(this.oProgramuToolStripMenuItem_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.statusStrip);
            this.splitContainer1.Panel1.Controls.Add(this.listView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtZmenyNTFS);
            this.splitContainer1.Size = new System.Drawing.Size(434, 742);
            this.splitContainer1.SplitterDistance = 593;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 5;
            // 
            // statusStrip
            // 
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip.Location = new System.Drawing.Point(0, 571);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip.Size = new System.Drawing.Size(434, 22);
            this.statusStrip.TabIndex = 7;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(0, 17);
            // 
            // listView
            // 
            this.listView.BackColor = System.Drawing.SystemColors.Window;
            this.listView.BackgroundImageTiled = true;
            this.listView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView.ContextMenuStrip = this.contextMenuStrip;
            this.listView.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.ForeColor = System.Drawing.Color.Black;
            this.listView.FullRowSelect = true;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Margin = new System.Windows.Forms.Padding(0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(434, 593);
            this.listView.SmallImageList = this.imageList;
            this.listView.TabIndex = 6;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.ItemMouseHover += new System.Windows.Forms.ListViewItemMouseHoverEventHandler(this.listView_ItemMouseHover);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Jméno stanice";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Přihlášený uživatel";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Aktivní okno";
            this.columnHeader3.Width = 240;
            // 
            // txtZmenyNTFS
            // 
            this.txtZmenyNTFS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtZmenyNTFS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtZmenyNTFS.Location = new System.Drawing.Point(0, 0);
            this.txtZmenyNTFS.Margin = new System.Windows.Forms.Padding(0);
            this.txtZmenyNTFS.Multiline = true;
            this.txtZmenyNTFS.Name = "txtZmenyNTFS";
            this.txtZmenyNTFS.ReadOnly = true;
            this.txtZmenyNTFS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtZmenyNTFS.Size = new System.Drawing.Size(434, 148);
            this.txtZmenyNTFS.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 766);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "frmMain";
            this.Text = "WOSNManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem souborToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem konecToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem screenshotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wakeUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem listProcesůToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zaslatZprávuToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem zamknoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem odhlásitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartovatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vypnoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nástrojeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nápovědaToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.TextBox txtZmenyNTFS;
        private System.Windows.Forms.ToolStripMenuItem pingerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oProgramuToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

