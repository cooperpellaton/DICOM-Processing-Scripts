namespace DifMod {
	partial class MainWindow {
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
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.DataGroup = new System.Windows.Forms.GroupBox();
			this.ViewDescriptivesButton = new System.Windows.Forms.Button();
			this.ViewDataButton = new System.Windows.Forms.Button();
			this.PreprocessDataButton = new System.Windows.Forms.Button();
			this.ResultsGroup = new System.Windows.Forms.GroupBox();
			this.OutlierReportButton = new System.Windows.Forms.Button();
			this.FitMeasuresButton = new System.Windows.Forms.Button();
			this.ViewOptimizationDetailsButton = new System.Windows.Forms.Button();
			this.ViewParamMatrixButton = new System.Windows.Forms.Button();
			this.AdvModelOptionsButton = new System.Windows.Forms.Button();
			this.ModelGroup = new System.Windows.Forms.GroupBox();
			this.NewModelButton = new System.Windows.Forms.Button();
			this.RunModelButton = new System.Windows.Forms.Button();
			this.RunModelBgWorker = new System.ComponentModel.BackgroundWorker();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.DataGroup.SuspendLayout();
			this.ResultsGroup.SuspendLayout();
			this.ModelGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusBarLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 176);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(487, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// StatusBarLabel
			// 
			this.StatusBarLabel.Name = "StatusBarLabel";
			this.StatusBarLabel.Size = new System.Drawing.Size(0, 17);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(487, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newModelToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
			// 
			// newModelToolStripMenuItem
			// 
			this.newModelToolStripMenuItem.Name = "newModelToolStripMenuItem";
			this.newModelToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+N";
			this.newModelToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newModelToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.newModelToolStripMenuItem.Text = "New Model";
			this.newModelToolStripMenuItem.Click += new System.EventHandler(this.newModelToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+W";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// dataToolStripMenuItem
			// 
			this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
			this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
			this.dataToolStripMenuItem.Text = "&Data";
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// contentsToolStripMenuItem
			// 
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.contentsToolStripMenuItem.Text = "&Contents";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(119, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			// 
			// DataGroup
			// 
			this.DataGroup.Controls.Add(this.OutlierReportButton);
			this.DataGroup.Controls.Add(this.ViewDescriptivesButton);
			this.DataGroup.Controls.Add(this.ViewDataButton);
			this.DataGroup.Controls.Add(this.PreprocessDataButton);
			this.DataGroup.Location = new System.Drawing.Point(168, 27);
			this.DataGroup.Name = "DataGroup";
			this.DataGroup.Size = new System.Drawing.Size(150, 140);
			this.DataGroup.TabIndex = 2;
			this.DataGroup.TabStop = false;
			this.DataGroup.Text = "Data";
			// 
			// ViewDescriptivesButton
			// 
			this.ViewDescriptivesButton.Enabled = false;
			this.ViewDescriptivesButton.Location = new System.Drawing.Point(6, 77);
			this.ViewDescriptivesButton.Name = "ViewDescriptivesButton";
			this.ViewDescriptivesButton.Size = new System.Drawing.Size(137, 23);
			this.ViewDescriptivesButton.TabIndex = 3;
			this.ViewDescriptivesButton.Text = "View Descriptives";
			this.ViewDescriptivesButton.UseVisualStyleBackColor = true;
			this.ViewDescriptivesButton.Click += new System.EventHandler(this.ViewDescriptivesButton_Click);
			// 
			// ViewDataButton
			// 
			this.ViewDataButton.Enabled = false;
			this.ViewDataButton.Location = new System.Drawing.Point(6, 47);
			this.ViewDataButton.Name = "ViewDataButton";
			this.ViewDataButton.Size = new System.Drawing.Size(138, 23);
			this.ViewDataButton.TabIndex = 2;
			this.ViewDataButton.Text = "View Data";
			this.ViewDataButton.UseVisualStyleBackColor = true;
			this.ViewDataButton.Click += new System.EventHandler(this.ViewDataButton_Click);
			// 
			// PreprocessDataButton
			// 
			this.PreprocessDataButton.Location = new System.Drawing.Point(6, 18);
			this.PreprocessDataButton.Name = "PreprocessDataButton";
			this.PreprocessDataButton.Size = new System.Drawing.Size(138, 23);
			this.PreprocessDataButton.TabIndex = 0;
			this.PreprocessDataButton.Text = "Preprocess Data";
			this.PreprocessDataButton.UseVisualStyleBackColor = true;
			this.PreprocessDataButton.Click += new System.EventHandler(this.PreprocessDataButton_Click);
			// 
			// ResultsGroup
			// 
			this.ResultsGroup.Controls.Add(this.FitMeasuresButton);
			this.ResultsGroup.Controls.Add(this.ViewOptimizationDetailsButton);
			this.ResultsGroup.Controls.Add(this.ViewParamMatrixButton);
			this.ResultsGroup.Location = new System.Drawing.Point(324, 27);
			this.ResultsGroup.Name = "ResultsGroup";
			this.ResultsGroup.Size = new System.Drawing.Size(150, 140);
			this.ResultsGroup.TabIndex = 3;
			this.ResultsGroup.TabStop = false;
			this.ResultsGroup.Text = "Results";
			// 
			// OutlierReportButton
			// 
			this.OutlierReportButton.Enabled = false;
			this.OutlierReportButton.Location = new System.Drawing.Point(6, 106);
			this.OutlierReportButton.Name = "OutlierReportButton";
			this.OutlierReportButton.Size = new System.Drawing.Size(137, 23);
			this.OutlierReportButton.TabIndex = 3;
			this.OutlierReportButton.Text = "View Outlier Report";
			this.OutlierReportButton.UseVisualStyleBackColor = true;
			this.OutlierReportButton.Click += new System.EventHandler(this.OutlierReportButton_Click);
			// 
			// FitMeasuresButton
			// 
			this.FitMeasuresButton.Enabled = false;
			this.FitMeasuresButton.Location = new System.Drawing.Point(7, 76);
			this.FitMeasuresButton.Name = "FitMeasuresButton";
			this.FitMeasuresButton.Size = new System.Drawing.Size(135, 23);
			this.FitMeasuresButton.TabIndex = 2;
			this.FitMeasuresButton.Text = "View Model Fit Measures";
			this.FitMeasuresButton.UseVisualStyleBackColor = true;
			this.FitMeasuresButton.Click += new System.EventHandler(this.FitMeasuresButton_Click);
			// 
			// ViewOptimizationDetailsButton
			// 
			this.ViewOptimizationDetailsButton.Enabled = false;
			this.ViewOptimizationDetailsButton.Location = new System.Drawing.Point(7, 47);
			this.ViewOptimizationDetailsButton.Name = "ViewOptimizationDetailsButton";
			this.ViewOptimizationDetailsButton.Size = new System.Drawing.Size(135, 23);
			this.ViewOptimizationDetailsButton.TabIndex = 1;
			this.ViewOptimizationDetailsButton.Text = "View Optimization Details";
			this.ViewOptimizationDetailsButton.UseVisualStyleBackColor = true;
			this.ViewOptimizationDetailsButton.Click += new System.EventHandler(this.ViewOptimizationDetailsButton_Click);
			// 
			// ViewParamMatrixButton
			// 
			this.ViewParamMatrixButton.Enabled = false;
			this.ViewParamMatrixButton.Location = new System.Drawing.Point(7, 20);
			this.ViewParamMatrixButton.Name = "ViewParamMatrixButton";
			this.ViewParamMatrixButton.Size = new System.Drawing.Size(135, 23);
			this.ViewParamMatrixButton.TabIndex = 0;
			this.ViewParamMatrixButton.Text = "View Parameter Matrix";
			this.ViewParamMatrixButton.UseVisualStyleBackColor = true;
			this.ViewParamMatrixButton.Click += new System.EventHandler(this.ViewParamMatrixButton_Click);
			// 
			// AdvModelOptionsButton
			// 
			this.AdvModelOptionsButton.Enabled = false;
			this.AdvModelOptionsButton.Location = new System.Drawing.Point(6, 78);
			this.AdvModelOptionsButton.Name = "AdvModelOptionsButton";
			this.AdvModelOptionsButton.Size = new System.Drawing.Size(137, 23);
			this.AdvModelOptionsButton.TabIndex = 1;
			this.AdvModelOptionsButton.Text = "Advanced Model Options";
			this.AdvModelOptionsButton.UseVisualStyleBackColor = true;
			this.AdvModelOptionsButton.Click += new System.EventHandler(this.AdvModelOptionsButton_Click);
			// 
			// ModelGroup
			// 
			this.ModelGroup.Controls.Add(this.NewModelButton);
			this.ModelGroup.Controls.Add(this.RunModelButton);
			this.ModelGroup.Controls.Add(this.AdvModelOptionsButton);
			this.ModelGroup.Location = new System.Drawing.Point(12, 27);
			this.ModelGroup.Name = "ModelGroup";
			this.ModelGroup.Size = new System.Drawing.Size(150, 140);
			this.ModelGroup.TabIndex = 4;
			this.ModelGroup.TabStop = false;
			this.ModelGroup.Text = "Model";
			// 
			// NewModelButton
			// 
			this.NewModelButton.Location = new System.Drawing.Point(6, 20);
			this.NewModelButton.Name = "NewModelButton";
			this.NewModelButton.Size = new System.Drawing.Size(137, 23);
			this.NewModelButton.TabIndex = 3;
			this.NewModelButton.Text = "New Model";
			this.NewModelButton.UseVisualStyleBackColor = true;
			this.NewModelButton.Click += new System.EventHandler(this.NewModelButton_Click);
			// 
			// RunModelButton
			// 
			this.RunModelButton.Enabled = false;
			this.RunModelButton.Location = new System.Drawing.Point(6, 49);
			this.RunModelButton.Name = "RunModelButton";
			this.RunModelButton.Size = new System.Drawing.Size(137, 23);
			this.RunModelButton.TabIndex = 2;
			this.RunModelButton.Text = "Run Model";
			this.RunModelButton.UseVisualStyleBackColor = true;
			this.RunModelButton.Click += new System.EventHandler(this.RunModelButton_Click);
			// 
			// RunModelBgWorker
			// 
			this.RunModelBgWorker.WorkerReportsProgress = true;
			this.RunModelBgWorker.WorkerSupportsCancellation = true;
			this.RunModelBgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RunModelBackground_DoWork);
			this.RunModelBgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RunModelBackground_RunWorkerCompleted);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(487, 198);
			this.Controls.Add(this.ModelGroup);
			this.Controls.Add(this.ResultsGroup);
			this.Controls.Add(this.DataGroup);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainWindow";
			this.Text = "Main Window";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.DataGroup.ResumeLayout(false);
			this.ResultsGroup.ResumeLayout(false);
			this.ModelGroup.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newModelToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel StatusBarLabel;
		private System.Windows.Forms.GroupBox DataGroup;
		private System.Windows.Forms.GroupBox ResultsGroup;
		private System.Windows.Forms.Button ViewDescriptivesButton;
		private System.Windows.Forms.Button ViewDataButton;
		private System.Windows.Forms.Button AdvModelOptionsButton;
		private System.Windows.Forms.Button PreprocessDataButton;
		private System.Windows.Forms.GroupBox ModelGroup;
		private System.Windows.Forms.Button RunModelButton;
		private System.Windows.Forms.Button ViewOptimizationDetailsButton;
		private System.Windows.Forms.Button ViewParamMatrixButton;
		private System.Windows.Forms.Button NewModelButton;
		private System.Windows.Forms.Button OutlierReportButton;
		private System.Windows.Forms.Button FitMeasuresButton;
		private System.ComponentModel.BackgroundWorker RunModelBgWorker;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
	}
}

