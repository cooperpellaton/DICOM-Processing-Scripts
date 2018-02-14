namespace DifMod {
	partial class ParamMatrixUI {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose ( bool disposing ) {
			if ( disposing && ( components != null ) ) {
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent () {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParamMatrixUI));
			this.ParamMatrixGrid = new System.Windows.Forms.DataGridView();
			this.ParamMatrixCloseButton = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.paramMatrixContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			((System.ComponentModel.ISupportInitialize)(this.ParamMatrixGrid)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.paramMatrixContextMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// ParamMatrixGrid
			// 
			this.ParamMatrixGrid.AllowUserToAddRows = false;
			this.ParamMatrixGrid.AllowUserToDeleteRows = false;
			this.ParamMatrixGrid.AllowUserToOrderColumns = true;
			this.ParamMatrixGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ParamMatrixGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.ParamMatrixGrid.ColumnHeadersHeight = 24;
			this.ParamMatrixGrid.ContextMenuStrip = this.paramMatrixContextMenu;
			this.ParamMatrixGrid.Location = new System.Drawing.Point(12, 27);
			this.ParamMatrixGrid.Name = "ParamMatrixGrid";
			this.ParamMatrixGrid.ReadOnly = true;
			this.ParamMatrixGrid.RowTemplate.Height = 18;
			this.ParamMatrixGrid.Size = new System.Drawing.Size(405, 159);
			this.ParamMatrixGrid.TabIndex = 0;
			// 
			// ParamMatrixCloseButton
			// 
			this.ParamMatrixCloseButton.AutoSize = true;
			this.ParamMatrixCloseButton.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.ParamMatrixCloseButton.Location = new System.Drawing.Point(0, 192);
			this.ParamMatrixCloseButton.Name = "ParamMatrixCloseButton";
			this.ParamMatrixCloseButton.Size = new System.Drawing.Size(429, 23);
			this.ParamMatrixCloseButton.TabIndex = 1;
			this.ParamMatrixCloseButton.Text = "Close";
			this.ParamMatrixCloseButton.UseVisualStyleBackColor = true;
			this.ParamMatrixCloseButton.Click += new System.EventHandler(this.ParamMatrixCloseButton_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem1});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(429, 24);
			this.menuStrip1.TabIndex = 2;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// exitToolStripMenuItem1
			// 
			this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
			this.exitToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+W";
			this.exitToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.exitToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
			this.exitToolStripMenuItem1.Text = "&Close";
			this.exitToolStripMenuItem1.Click += new System.EventHandler(this.exitToolStripMenuItem1_Click);
			// 
			// fileToolStripMenuItem1
			// 
			this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem1});
			this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
			this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem1.Text = "&File";
			// 
			// paramMatrixContextMenu
			// 
			this.paramMatrixContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1,
            this.selectAllToolStripMenuItem1});
			this.paramMatrixContextMenu.Name = "paramMatrixContextMenu";
			this.paramMatrixContextMenu.Size = new System.Drawing.Size(123, 48);
			// 
			// copyToolStripMenuItem1
			// 
			this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
			this.copyToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
			this.copyToolStripMenuItem1.Text = "Copy";
			// 
			// selectAllToolStripMenuItem1
			// 
			this.selectAllToolStripMenuItem1.Name = "selectAllToolStripMenuItem1";
			this.selectAllToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
			this.selectAllToolStripMenuItem1.Text = "Select All";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+A";
			this.selectAllToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
			this.selectAllToolStripMenuItem.Text = "Select &All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem1
			// 
			this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.selectAllToolStripMenuItem});
			this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
			this.editToolStripMenuItem1.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem1.Text = "&Edit";
			// 
			// ParamMatrixUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(429, 215);
			this.Controls.Add(this.ParamMatrixCloseButton);
			this.Controls.Add(this.ParamMatrixGrid);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.MaximumSize = new System.Drawing.Size(1000, 360);
			this.MinimumSize = new System.Drawing.Size(445, 200);
			this.Name = "ParamMatrixUI";
			this.Text = "Parameter Matrix";
			((System.ComponentModel.ISupportInitialize)(this.ParamMatrixGrid)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.paramMatrixContextMenu.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView ParamMatrixGrid;
		private System.Windows.Forms.Button ParamMatrixCloseButton;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
		private System.Windows.Forms.ContextMenuStrip paramMatrixContextMenu;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
	}
}