namespace DifMod {
	partial class ModelOptionsUI {
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
			this.AdvancedOptsButton = new System.Windows.Forms.Button();
			this.DoneButton = new System.Windows.Forms.Button();
			this.OptsCancelButton = new System.Windows.Forms.Button();
			this.OutlierOptionsBox = new System.Windows.Forms.ComboBox();
			this.outlierTreatmentLabel = new System.Windows.Forms.Label();
			this.ModelTypeLabel = new System.Windows.Forms.Label();
			this.ModelTypeBox = new System.Windows.Forms.ComboBox();
			this.button4 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.Param6Label = new System.Windows.Forms.Label();
			this.Param7Label = new System.Windows.Forms.Label();
			this.Param4Label = new System.Windows.Forms.Label();
			this.Param5Label = new System.Windows.Forms.Label();
			this.Param6Box = new System.Windows.Forms.ComboBox();
			this.Param1Label = new System.Windows.Forms.Label();
			this.Param3Label = new System.Windows.Forms.Label();
			this.Param5Box = new System.Windows.Forms.ComboBox();
			this.Param3Box = new System.Windows.Forms.ComboBox();
			this.Param2Label = new System.Windows.Forms.Label();
			this.Param2Box = new System.Windows.Forms.ComboBox();
			this.Param7Box = new System.Windows.Forms.ComboBox();
			this.Param4Box = new System.Windows.Forms.ComboBox();
			this.Param1Box = new System.Windows.Forms.ComboBox();
			this.OutlierMaxBox = new System.Windows.Forms.TextBox();
			this.OutlierMinLabel = new System.Windows.Forms.Label();
			this.OutlierMaxLabel = new System.Windows.Forms.Label();
			this.OutlierMinUnitLabel = new System.Windows.Forms.Label();
			this.OutlierMaxUnitLabel = new System.Windows.Forms.Label();
			this.OutlierMinBox = new System.Windows.Forms.TextBox();
			this.FitAssessmentLabel = new System.Windows.Forms.Label();
			this.FitAssessmentBox = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// AdvancedOptsButton
			// 
			this.AdvancedOptsButton.Location = new System.Drawing.Point(12, 241);
			this.AdvancedOptsButton.Name = "AdvancedOptsButton";
			this.AdvancedOptsButton.Size = new System.Drawing.Size(127, 23);
			this.AdvancedOptsButton.TabIndex = 0;
			this.AdvancedOptsButton.Text = "Advanced Options";
			this.AdvancedOptsButton.UseVisualStyleBackColor = true;
			this.AdvancedOptsButton.Click += new System.EventHandler(this.AdvancedOptionsButton_Click);
			// 
			// DoneButton
			// 
			this.DoneButton.Location = new System.Drawing.Point(217, 325);
			this.DoneButton.Name = "DoneButton";
			this.DoneButton.Size = new System.Drawing.Size(75, 23);
			this.DoneButton.TabIndex = 1;
			this.DoneButton.Text = "Done";
			this.DoneButton.UseVisualStyleBackColor = true;
			this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
			// 
			// OptsCancelButton
			// 
			this.OptsCancelButton.Location = new System.Drawing.Point(12, 325);
			this.OptsCancelButton.Name = "OptsCancelButton";
			this.OptsCancelButton.Size = new System.Drawing.Size(75, 23);
			this.OptsCancelButton.TabIndex = 2;
			this.OptsCancelButton.Text = "Cancel";
			this.OptsCancelButton.UseVisualStyleBackColor = true;
			this.OptsCancelButton.Click += new System.EventHandler(this.NewModelCancelButton_Click);
			// 
			// OutlierOptionsBox
			// 
			this.OutlierOptionsBox.AllowDrop = true;
			this.OutlierOptionsBox.FormattingEnabled = true;
			this.OutlierOptionsBox.Items.AddRange(new object[] {
            "None",
            "Relative Bounds",
            "Absolute Bounds"});
			this.OutlierOptionsBox.Location = new System.Drawing.Point(12, 105);
			this.OutlierOptionsBox.Name = "OutlierOptionsBox";
			this.OutlierOptionsBox.Size = new System.Drawing.Size(127, 21);
			this.OutlierOptionsBox.TabIndex = 4;
			this.OutlierOptionsBox.Text = "None";
			this.OutlierOptionsBox.SelectedIndexChanged += new System.EventHandler(this.OutlierOptionsBox_SelectedIndexChanged);
			// 
			// outlierTreatmentLabel
			// 
			this.outlierTreatmentLabel.AutoSize = true;
			this.outlierTreatmentLabel.Location = new System.Drawing.Point(12, 89);
			this.outlierTreatmentLabel.Name = "outlierTreatmentLabel";
			this.outlierTreatmentLabel.Size = new System.Drawing.Size(91, 13);
			this.outlierTreatmentLabel.TabIndex = 5;
			this.outlierTreatmentLabel.Text = "Outlier Treatment:";
			// 
			// ModelTypeLabel
			// 
			this.ModelTypeLabel.AutoSize = true;
			this.ModelTypeLabel.Location = new System.Drawing.Point(12, 49);
			this.ModelTypeLabel.Name = "ModelTypeLabel";
			this.ModelTypeLabel.Size = new System.Drawing.Size(63, 13);
			this.ModelTypeLabel.TabIndex = 6;
			this.ModelTypeLabel.Text = "Model Type";
			// 
			// ModelTypeBox
			// 
			this.ModelTypeBox.FormattingEnabled = true;
			this.ModelTypeBox.Items.AddRange(new object[] {
            "Standard Diffusion"});
			this.ModelTypeBox.Location = new System.Drawing.Point(12, 65);
			this.ModelTypeBox.Name = "ModelTypeBox";
			this.ModelTypeBox.Size = new System.Drawing.Size(127, 21);
			this.ModelTypeBox.TabIndex = 7;
			this.ModelTypeBox.Text = "Standard Diffusion";
			this.ModelTypeBox.SelectedIndexChanged += new System.EventHandler(this.ModelTypeBox_SelectedIndexChanged);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(12, 23);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(127, 23);
			this.button4.TabIndex = 3;
			this.button4.Text = "Load Data File";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.LoadFileButton_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.Param6Label);
			this.groupBox1.Controls.Add(this.Param7Label);
			this.groupBox1.Controls.Add(this.Param4Label);
			this.groupBox1.Controls.Add(this.Param5Label);
			this.groupBox1.Controls.Add(this.Param6Box);
			this.groupBox1.Controls.Add(this.Param1Label);
			this.groupBox1.Controls.Add(this.Param3Label);
			this.groupBox1.Controls.Add(this.Param5Box);
			this.groupBox1.Controls.Add(this.Param3Box);
			this.groupBox1.Controls.Add(this.Param2Label);
			this.groupBox1.Controls.Add(this.Param2Box);
			this.groupBox1.Controls.Add(this.Param7Box);
			this.groupBox1.Controls.Add(this.Param4Box);
			this.groupBox1.Controls.Add(this.Param1Box);
			this.groupBox1.Location = new System.Drawing.Point(145, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(147, 307);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Parameter Settings";
			// 
			// Param6Label
			// 
			this.Param6Label.AutoSize = true;
			this.Param6Label.Location = new System.Drawing.Point(7, 261);
			this.Param6Label.Name = "Param6Label";
			this.Param6Label.Size = new System.Drawing.Size(91, 13);
			this.Param6Label.TabIndex = 13;
			this.Param6Label.Text = "Ter Variance (ST)";
			// 
			// Param7Label
			// 
			this.Param7Label.AutoSize = true;
			this.Param7Label.Location = new System.Drawing.Point(7, 91);
			this.Param7Label.Name = "Param7Label";
			this.Param7Label.Size = new System.Drawing.Size(97, 13);
			this.Param7Label.TabIndex = 9;
			this.Param7Label.Text = "Mean Drift Rate (v)";
			// 
			// Param4Label
			// 
			this.Param4Label.AutoSize = true;
			this.Param4Label.Location = new System.Drawing.Point(7, 54);
			this.Param4Label.Name = "Param4Label";
			this.Param4Label.Size = new System.Drawing.Size(99, 13);
			this.Param4Label.TabIndex = 8;
			this.Param4Label.Text = "Starting Position (Z)";
			// 
			// Param5Label
			// 
			this.Param5Label.AutoSize = true;
			this.Param5Label.Location = new System.Drawing.Point(7, 218);
			this.Param5Label.Name = "Param5Label";
			this.Param5Label.Size = new System.Drawing.Size(137, 13);
			this.Param5Label.TabIndex = 12;
			this.Param5Label.Text = "Start Position Variance (SZ)";
			// 
			// Param6Box
			// 
			this.Param6Box.FormattingEnabled = true;
			this.Param6Box.Items.AddRange(new object[] {
            "Free",
            "Fixed"});
			this.Param6Box.Location = new System.Drawing.Point(10, 277);
			this.Param6Box.Name = "Param6Box";
			this.Param6Box.Size = new System.Drawing.Size(117, 21);
			this.Param6Box.TabIndex = 6;
			this.Param6Box.Text = "Fixed";
			// 
			// Param1Label
			// 
			this.Param1Label.AutoSize = true;
			this.Param1Label.Location = new System.Drawing.Point(7, 16);
			this.Param1Label.Name = "Param1Label";
			this.Param1Label.Size = new System.Drawing.Size(121, 13);
			this.Param1Label.TabIndex = 7;
			this.Param1Label.Text = "Boundary Separation (a)";
			// 
			// Param3Label
			// 
			this.Param3Label.AutoSize = true;
			this.Param3Label.Location = new System.Drawing.Point(7, 174);
			this.Param3Label.Name = "Param3Label";
			this.Param3Label.Size = new System.Drawing.Size(121, 13);
			this.Param3Label.TabIndex = 11;
			this.Param3Label.Text = "Drift Rate Variance (eta)";
			// 
			// Param5Box
			// 
			this.Param5Box.FormattingEnabled = true;
			this.Param5Box.Items.AddRange(new object[] {
            "Free",
            "Fixed"});
			this.Param5Box.Location = new System.Drawing.Point(10, 237);
			this.Param5Box.Name = "Param5Box";
			this.Param5Box.Size = new System.Drawing.Size(117, 21);
			this.Param5Box.TabIndex = 5;
			this.Param5Box.Text = "Fixed";
			// 
			// Param3Box
			// 
			this.Param3Box.FormattingEnabled = true;
			this.Param3Box.Items.AddRange(new object[] {
            "Free",
            "Fixed"});
			this.Param3Box.Location = new System.Drawing.Point(10, 194);
			this.Param3Box.Name = "Param3Box";
			this.Param3Box.Size = new System.Drawing.Size(117, 21);
			this.Param3Box.TabIndex = 4;
			this.Param3Box.Text = "Fixed";
			// 
			// Param2Label
			// 
			this.Param2Label.AutoSize = true;
			this.Param2Label.Location = new System.Drawing.Point(7, 134);
			this.Param2Label.Name = "Param2Label";
			this.Param2Label.Size = new System.Drawing.Size(120, 13);
			this.Param2Label.TabIndex = 10;
			this.Param2Label.Text = "Non-decision Time (Ter)";
			// 
			// Param2Box
			// 
			this.Param2Box.FormattingEnabled = true;
			this.Param2Box.Items.AddRange(new object[] {
            "Free",
            "Fixed"});
			this.Param2Box.Location = new System.Drawing.Point(10, 150);
			this.Param2Box.Name = "Param2Box";
			this.Param2Box.Size = new System.Drawing.Size(117, 21);
			this.Param2Box.TabIndex = 3;
			this.Param2Box.Text = "Fixed";
			// 
			// Param7Box
			// 
			this.Param7Box.FormattingEnabled = true;
			this.Param7Box.Items.AddRange(new object[] {
            "Free",
            "Fixed"});
			this.Param7Box.Location = new System.Drawing.Point(10, 110);
			this.Param7Box.Name = "Param7Box";
			this.Param7Box.Size = new System.Drawing.Size(117, 21);
			this.Param7Box.TabIndex = 2;
			this.Param7Box.Text = "Fixed";
			// 
			// Param4Box
			// 
			this.Param4Box.FormattingEnabled = true;
			this.Param4Box.Items.AddRange(new object[] {
            "Free",
            "Fixed"});
			this.Param4Box.Location = new System.Drawing.Point(10, 67);
			this.Param4Box.Name = "Param4Box";
			this.Param4Box.Size = new System.Drawing.Size(117, 21);
			this.Param4Box.TabIndex = 1;
			this.Param4Box.Text = "Fixed";
			// 
			// Param1Box
			// 
			this.Param1Box.FormattingEnabled = true;
			this.Param1Box.Items.AddRange(new object[] {
            "Free",
            "Fixed"});
			this.Param1Box.Location = new System.Drawing.Point(10, 32);
			this.Param1Box.Name = "Param1Box";
			this.Param1Box.Size = new System.Drawing.Size(117, 21);
			this.Param1Box.TabIndex = 0;
			this.Param1Box.Text = "Fixed";
			// 
			// OutlierMaxBox
			// 
			this.OutlierMaxBox.Location = new System.Drawing.Point(42, 158);
			this.OutlierMaxBox.Name = "OutlierMaxBox";
			this.OutlierMaxBox.Size = new System.Drawing.Size(62, 20);
			this.OutlierMaxBox.TabIndex = 10;
			this.OutlierMaxBox.Visible = false;
			this.OutlierMaxBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OutlierMaxBox_KeyPress);
			// 
			// OutlierMinLabel
			// 
			this.OutlierMinLabel.AutoSize = true;
			this.OutlierMinLabel.Location = new System.Drawing.Point(12, 124);
			this.OutlierMinLabel.Name = "OutlierMinLabel";
			this.OutlierMinLabel.Size = new System.Drawing.Size(24, 13);
			this.OutlierMinLabel.TabIndex = 11;
			this.OutlierMinLabel.Text = "Min";
			this.OutlierMinLabel.Visible = false;
			// 
			// OutlierMaxLabel
			// 
			this.OutlierMaxLabel.AutoSize = true;
			this.OutlierMaxLabel.Location = new System.Drawing.Point(12, 150);
			this.OutlierMaxLabel.Name = "OutlierMaxLabel";
			this.OutlierMaxLabel.Size = new System.Drawing.Size(27, 13);
			this.OutlierMaxLabel.TabIndex = 12;
			this.OutlierMaxLabel.Text = "Max";
			this.OutlierMaxLabel.Visible = false;
			// 
			// OutlierMinUnitLabel
			// 
			this.OutlierMinUnitLabel.AutoSize = true;
			this.OutlierMinUnitLabel.Location = new System.Drawing.Point(110, 135);
			this.OutlierMinUnitLabel.Name = "OutlierMinUnitLabel";
			this.OutlierMinUnitLabel.Size = new System.Drawing.Size(12, 13);
			this.OutlierMinUnitLabel.TabIndex = 13;
			this.OutlierMinUnitLabel.Text = "s";
			this.OutlierMinUnitLabel.Visible = false;
			// 
			// OutlierMaxUnitLabel
			// 
			this.OutlierMaxUnitLabel.AutoSize = true;
			this.OutlierMaxUnitLabel.Location = new System.Drawing.Point(110, 161);
			this.OutlierMaxUnitLabel.Name = "OutlierMaxUnitLabel";
			this.OutlierMaxUnitLabel.Size = new System.Drawing.Size(12, 13);
			this.OutlierMaxUnitLabel.TabIndex = 14;
			this.OutlierMaxUnitLabel.Text = "s";
			this.OutlierMaxUnitLabel.Visible = false;
			// 
			// OutlierMinBox
			// 
			this.OutlierMinBox.Location = new System.Drawing.Point(42, 132);
			this.OutlierMinBox.MaxLength = 7;
			this.OutlierMinBox.Name = "OutlierMinBox";
			this.OutlierMinBox.Size = new System.Drawing.Size(62, 20);
			this.OutlierMinBox.TabIndex = 9;
			this.OutlierMinBox.Visible = false;
			this.OutlierMinBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OutlierMinBox_KeyPress);
			// 
			// FitAssessmentLabel
			// 
			this.FitAssessmentLabel.AutoSize = true;
			this.FitAssessmentLabel.Location = new System.Drawing.Point(12, 188);
			this.FitAssessmentLabel.Name = "FitAssessmentLabel";
			this.FitAssessmentLabel.Size = new System.Drawing.Size(116, 13);
			this.FitAssessmentLabel.TabIndex = 15;
			this.FitAssessmentLabel.Text = "Fit Assessment Method";
			// 
			// FitAssessmentBox
			// 
			this.FitAssessmentBox.FormattingEnabled = true;
			this.FitAssessmentBox.Items.AddRange(new object[] {
            "Multinomial Likelihood",
            "Chi-squared"});
			this.FitAssessmentBox.Location = new System.Drawing.Point(12, 204);
			this.FitAssessmentBox.Name = "FitAssessmentBox";
			this.FitAssessmentBox.Size = new System.Drawing.Size(127, 21);
			this.FitAssessmentBox.TabIndex = 16;
			this.FitAssessmentBox.Text = "Multinomial Likelihood";
			this.FitAssessmentBox.SelectedIndexChanged += new System.EventHandler(this.FitAssessmentBox_SelectedIndexChanged);
			// 
			// ModelOptionsUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(306, 359);
			this.Controls.Add(this.FitAssessmentBox);
			this.Controls.Add(this.FitAssessmentLabel);
			this.Controls.Add(this.OutlierMaxUnitLabel);
			this.Controls.Add(this.OutlierMinUnitLabel);
			this.Controls.Add(this.OutlierMaxLabel);
			this.Controls.Add(this.OutlierMinLabel);
			this.Controls.Add(this.OutlierMaxBox);
			this.Controls.Add(this.OutlierMinBox);
			this.Controls.Add(this.ModelTypeBox);
			this.Controls.Add(this.ModelTypeLabel);
			this.Controls.Add(this.outlierTreatmentLabel);
			this.Controls.Add(this.OutlierOptionsBox);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.OptsCancelButton);
			this.Controls.Add(this.DoneButton);
			this.Controls.Add(this.AdvancedOptsButton);
			this.Controls.Add(this.groupBox1);
			this.Name = "ModelOptionsUI";
			this.Text = "New Model Options";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button AdvancedOptsButton;
		private System.Windows.Forms.Button DoneButton;
		private System.Windows.Forms.Button OptsCancelButton;
		private System.Windows.Forms.ComboBox OutlierOptionsBox;
		private System.Windows.Forms.Label outlierTreatmentLabel;
		private System.Windows.Forms.Label ModelTypeLabel;
		private System.Windows.Forms.ComboBox ModelTypeBox;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label Param5Label;
		private System.Windows.Forms.Label Param3Label;
		private System.Windows.Forms.Label Param2Label;
		private System.Windows.Forms.Label Param7Label;
		private System.Windows.Forms.Label Param4Label;
		private System.Windows.Forms.Label Param1Label;
		private System.Windows.Forms.ComboBox Param6Box;
		private System.Windows.Forms.ComboBox Param5Box;
		private System.Windows.Forms.ComboBox Param3Box;
		private System.Windows.Forms.ComboBox Param2Box;
		private System.Windows.Forms.ComboBox Param7Box;
		private System.Windows.Forms.ComboBox Param4Box;
		private System.Windows.Forms.ComboBox Param1Box;
		private System.Windows.Forms.Label Param6Label;
		private System.Windows.Forms.TextBox OutlierMaxBox;
		private System.Windows.Forms.Label OutlierMinLabel;
		private System.Windows.Forms.Label OutlierMaxLabel;
		private System.Windows.Forms.Label OutlierMinUnitLabel;
		private System.Windows.Forms.Label OutlierMaxUnitLabel;
		private System.Windows.Forms.TextBox OutlierMinBox;
		private System.Windows.Forms.Label FitAssessmentLabel;
		private System.Windows.Forms.ComboBox FitAssessmentBox;
	}
}