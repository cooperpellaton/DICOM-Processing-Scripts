namespace DifMod {
	partial class AdvancedOptionsUI {
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
			this.QuantileLabel = new System.Windows.Forms.Label();
			this.QuantileMethodBox = new System.Windows.Forms.ComboBox();
			this.TargetQuantileBox1 = new System.Windows.Forms.TextBox();
			this.TargetQuantileBox2 = new System.Windows.Forms.TextBox();
			this.TargetQuantileBox3 = new System.Windows.Forms.TextBox();
			this.TargetQuantileBox4 = new System.Windows.Forms.TextBox();
			this.TargetQuantileBox5 = new System.Windows.Forms.TextBox();
			this.NontargetQuantileBox1 = new System.Windows.Forms.TextBox();
			this.NontargetQuantileBox2 = new System.Windows.Forms.TextBox();
			this.NontargetQuantileBox3 = new System.Windows.Forms.TextBox();
			this.NontargetQuantileBox4 = new System.Windows.Forms.TextBox();
			this.NontargetQuantileBox5 = new System.Windows.Forms.TextBox();
			this.TargetsLabel = new System.Windows.Forms.Label();
			this.NontargetsLabel = new System.Windows.Forms.Label();
			this.AdvDoneButton = new System.Windows.Forms.Button();
			this.AdvCancelButton = new System.Windows.Forms.Button();
			this.OptimizationGroupBox = new System.Windows.Forms.GroupBox();
			this.NumberRetriesBox = new System.Windows.Forms.TextBox();
			this.NumberRetriesLabel = new System.Windows.Forms.Label();
			this.LongToleranceLabel = new System.Windows.Forms.Label();
			this.LongEvaluationsLabel = new System.Windows.Forms.Label();
			this.LongRunsLabel = new System.Windows.Forms.Label();
			this.LongToleranceBox = new System.Windows.Forms.TextBox();
			this.LongEvaluationsBox = new System.Windows.Forms.TextBox();
			this.LongRunsBox = new System.Windows.Forms.TextBox();
			this.PerturbationLabel = new System.Windows.Forms.Label();
			this.ShortToleranceLabel = new System.Windows.Forms.Label();
			this.ShortEvaluationsLabel = new System.Windows.Forms.Label();
			this.ShortRunsLabel = new System.Windows.Forms.Label();
			this.PerturbationBox = new System.Windows.Forms.TextBox();
			this.ShortToleranceBox = new System.Windows.Forms.TextBox();
			this.ShortEvaluationsBox = new System.Windows.Forms.TextBox();
			this.ShortRunBox = new System.Windows.Forms.TextBox();
			this.OptimizationGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// QuantileLabel
			// 
			this.QuantileLabel.AutoSize = true;
			this.QuantileLabel.Location = new System.Drawing.Point(12, 9);
			this.QuantileLabel.Name = "QuantileLabel";
			this.QuantileLabel.Size = new System.Drawing.Size(140, 13);
			this.QuantileLabel.TabIndex = 1;
			this.QuantileLabel.Text = "Quantile Calculation Method";
			// 
			// QuantileMethodBox
			// 
			this.QuantileMethodBox.FormattingEnabled = true;
			this.QuantileMethodBox.Items.AddRange(new object[] {
            "Default",
            "Custom Quantiles",
            "Custom Percentiles"});
			this.QuantileMethodBox.Location = new System.Drawing.Point(162, 6);
			this.QuantileMethodBox.Name = "QuantileMethodBox";
			this.QuantileMethodBox.Size = new System.Drawing.Size(121, 21);
			this.QuantileMethodBox.TabIndex = 3;
			this.QuantileMethodBox.Text = "Default";
			this.QuantileMethodBox.SelectedIndexChanged += new System.EventHandler(this.QuantileMethodBox_SelectedIndexChanged);
			// 
			// TargetQuantileBox1
			// 
			this.TargetQuantileBox1.Location = new System.Drawing.Point(77, 33);
			this.TargetQuantileBox1.Name = "TargetQuantileBox1";
			this.TargetQuantileBox1.Size = new System.Drawing.Size(56, 20);
			this.TargetQuantileBox1.TabIndex = 11;
			this.TargetQuantileBox1.Visible = false;
			this.TargetQuantileBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TargetQuantileBox1_KeyPress);
			// 
			// TargetQuantileBox2
			// 
			this.TargetQuantileBox2.Location = new System.Drawing.Point(139, 33);
			this.TargetQuantileBox2.Name = "TargetQuantileBox2";
			this.TargetQuantileBox2.Size = new System.Drawing.Size(56, 20);
			this.TargetQuantileBox2.TabIndex = 12;
			this.TargetQuantileBox2.Visible = false;
			this.TargetQuantileBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TargetQuantileBox2_KeyPress);
			// 
			// TargetQuantileBox3
			// 
			this.TargetQuantileBox3.Location = new System.Drawing.Point(201, 33);
			this.TargetQuantileBox3.Name = "TargetQuantileBox3";
			this.TargetQuantileBox3.Size = new System.Drawing.Size(56, 20);
			this.TargetQuantileBox3.TabIndex = 13;
			this.TargetQuantileBox3.Visible = false;
			this.TargetQuantileBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TargetQuantileBox3_KeyPress);
			// 
			// TargetQuantileBox4
			// 
			this.TargetQuantileBox4.Location = new System.Drawing.Point(263, 33);
			this.TargetQuantileBox4.Name = "TargetQuantileBox4";
			this.TargetQuantileBox4.Size = new System.Drawing.Size(56, 20);
			this.TargetQuantileBox4.TabIndex = 14;
			this.TargetQuantileBox4.Visible = false;
			this.TargetQuantileBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TargetQuantileBox4_KeyPress);
			// 
			// TargetQuantileBox5
			// 
			this.TargetQuantileBox5.Location = new System.Drawing.Point(325, 33);
			this.TargetQuantileBox5.Name = "TargetQuantileBox5";
			this.TargetQuantileBox5.Size = new System.Drawing.Size(56, 20);
			this.TargetQuantileBox5.TabIndex = 15;
			this.TargetQuantileBox5.Visible = false;
			this.TargetQuantileBox5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TargetQuantileBox5_KeyPress);
			// 
			// NontargetQuantileBox1
			// 
			this.NontargetQuantileBox1.Location = new System.Drawing.Point(77, 59);
			this.NontargetQuantileBox1.Name = "NontargetQuantileBox1";
			this.NontargetQuantileBox1.Size = new System.Drawing.Size(56, 20);
			this.NontargetQuantileBox1.TabIndex = 16;
			this.NontargetQuantileBox1.Visible = false;
			this.NontargetQuantileBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NontargetQuantileBox1_KeyPress);
			// 
			// NontargetQuantileBox2
			// 
			this.NontargetQuantileBox2.Location = new System.Drawing.Point(139, 59);
			this.NontargetQuantileBox2.Name = "NontargetQuantileBox2";
			this.NontargetQuantileBox2.Size = new System.Drawing.Size(56, 20);
			this.NontargetQuantileBox2.TabIndex = 17;
			this.NontargetQuantileBox2.Visible = false;
			this.NontargetQuantileBox2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NontargetQuantileBox2_KeyPress);
			// 
			// NontargetQuantileBox3
			// 
			this.NontargetQuantileBox3.Location = new System.Drawing.Point(201, 59);
			this.NontargetQuantileBox3.Name = "NontargetQuantileBox3";
			this.NontargetQuantileBox3.Size = new System.Drawing.Size(56, 20);
			this.NontargetQuantileBox3.TabIndex = 18;
			this.NontargetQuantileBox3.Visible = false;
			this.NontargetQuantileBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NontargetQuantileBox3_KeyPress);
			// 
			// NontargetQuantileBox4
			// 
			this.NontargetQuantileBox4.Location = new System.Drawing.Point(263, 59);
			this.NontargetQuantileBox4.Name = "NontargetQuantileBox4";
			this.NontargetQuantileBox4.Size = new System.Drawing.Size(56, 20);
			this.NontargetQuantileBox4.TabIndex = 19;
			this.NontargetQuantileBox4.Visible = false;
			this.NontargetQuantileBox4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NontargetQuantileBox4_KeyPress);
			// 
			// NontargetQuantileBox5
			// 
			this.NontargetQuantileBox5.Location = new System.Drawing.Point(325, 59);
			this.NontargetQuantileBox5.Name = "NontargetQuantileBox5";
			this.NontargetQuantileBox5.Size = new System.Drawing.Size(56, 20);
			this.NontargetQuantileBox5.TabIndex = 20;
			this.NontargetQuantileBox5.Visible = false;
			this.NontargetQuantileBox5.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NontargetQuantileBox5_KeyPress);
			// 
			// TargetsLabel
			// 
			this.TargetsLabel.AutoSize = true;
			this.TargetsLabel.Location = new System.Drawing.Point(12, 36);
			this.TargetsLabel.Name = "TargetsLabel";
			this.TargetsLabel.Size = new System.Drawing.Size(43, 13);
			this.TargetsLabel.TabIndex = 21;
			this.TargetsLabel.Text = "Targets";
			this.TargetsLabel.Visible = false;
			// 
			// NontargetsLabel
			// 
			this.NontargetsLabel.AutoSize = true;
			this.NontargetsLabel.Location = new System.Drawing.Point(12, 62);
			this.NontargetsLabel.Name = "NontargetsLabel";
			this.NontargetsLabel.Size = new System.Drawing.Size(62, 13);
			this.NontargetsLabel.TabIndex = 22;
			this.NontargetsLabel.Text = "Non-targets";
			this.NontargetsLabel.Visible = false;
			// 
			// AdvDoneButton
			// 
			this.AdvDoneButton.Location = new System.Drawing.Point(353, 221);
			this.AdvDoneButton.Name = "AdvDoneButton";
			this.AdvDoneButton.Size = new System.Drawing.Size(75, 23);
			this.AdvDoneButton.TabIndex = 30;
			this.AdvDoneButton.Text = "Done";
			this.AdvDoneButton.UseVisualStyleBackColor = true;
			this.AdvDoneButton.Click += new System.EventHandler(this.AdvDoneButton_Click);
			// 
			// AdvCancelButton
			// 
			this.AdvCancelButton.Location = new System.Drawing.Point(15, 221);
			this.AdvCancelButton.Name = "AdvCancelButton";
			this.AdvCancelButton.Size = new System.Drawing.Size(75, 23);
			this.AdvCancelButton.TabIndex = 31;
			this.AdvCancelButton.Text = "Cancel";
			this.AdvCancelButton.UseVisualStyleBackColor = true;
			this.AdvCancelButton.Click += new System.EventHandler(this.AdvCancelButton_Click);
			// 
			// OptimizationGroupBox
			// 
			this.OptimizationGroupBox.Controls.Add(this.NumberRetriesBox);
			this.OptimizationGroupBox.Controls.Add(this.NumberRetriesLabel);
			this.OptimizationGroupBox.Controls.Add(this.LongToleranceLabel);
			this.OptimizationGroupBox.Controls.Add(this.LongEvaluationsLabel);
			this.OptimizationGroupBox.Controls.Add(this.LongRunsLabel);
			this.OptimizationGroupBox.Controls.Add(this.LongToleranceBox);
			this.OptimizationGroupBox.Controls.Add(this.LongEvaluationsBox);
			this.OptimizationGroupBox.Controls.Add(this.LongRunsBox);
			this.OptimizationGroupBox.Controls.Add(this.PerturbationLabel);
			this.OptimizationGroupBox.Controls.Add(this.ShortToleranceLabel);
			this.OptimizationGroupBox.Controls.Add(this.ShortEvaluationsLabel);
			this.OptimizationGroupBox.Controls.Add(this.ShortRunsLabel);
			this.OptimizationGroupBox.Controls.Add(this.PerturbationBox);
			this.OptimizationGroupBox.Controls.Add(this.ShortToleranceBox);
			this.OptimizationGroupBox.Controls.Add(this.ShortEvaluationsBox);
			this.OptimizationGroupBox.Controls.Add(this.ShortRunBox);
			this.OptimizationGroupBox.Location = new System.Drawing.Point(15, 85);
			this.OptimizationGroupBox.Name = "OptimizationGroupBox";
			this.OptimizationGroupBox.Size = new System.Drawing.Size(413, 128);
			this.OptimizationGroupBox.TabIndex = 33;
			this.OptimizationGroupBox.TabStop = false;
			this.OptimizationGroupBox.Text = "Optimization Routine Options";
			// 
			// NumberRetriesBox
			// 
			this.NumberRetriesBox.Location = new System.Drawing.Point(301, 97);
			this.NumberRetriesBox.Name = "NumberRetriesBox";
			this.NumberRetriesBox.Size = new System.Drawing.Size(100, 20);
			this.NumberRetriesBox.TabIndex = 15;
			this.NumberRetriesBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NumberRetriesBox_KeyPress);
			// 
			// NumberRetriesLabel
			// 
			this.NumberRetriesLabel.AutoSize = true;
			this.NumberRetriesLabel.Location = new System.Drawing.Point(207, 100);
			this.NumberRetriesLabel.Name = "NumberRetriesLabel";
			this.NumberRetriesLabel.Size = new System.Drawing.Size(92, 13);
			this.NumberRetriesLabel.TabIndex = 14;
			this.NumberRetriesLabel.Text = "Number of Retries";
			// 
			// LongToleranceLabel
			// 
			this.LongToleranceLabel.AutoSize = true;
			this.LongToleranceLabel.Location = new System.Drawing.Point(207, 74);
			this.LongToleranceLabel.Name = "LongToleranceLabel";
			this.LongToleranceLabel.Size = new System.Drawing.Size(82, 13);
			this.LongToleranceLabel.TabIndex = 13;
			this.LongToleranceLabel.Text = "Long Tolerance";
			// 
			// LongEvaluationsLabel
			// 
			this.LongEvaluationsLabel.AutoSize = true;
			this.LongEvaluationsLabel.Location = new System.Drawing.Point(207, 48);
			this.LongEvaluationsLabel.Name = "LongEvaluationsLabel";
			this.LongEvaluationsLabel.Size = new System.Drawing.Size(89, 13);
			this.LongEvaluationsLabel.TabIndex = 12;
			this.LongEvaluationsLabel.Text = "Long Evaluations";
			// 
			// LongRunsLabel
			// 
			this.LongRunsLabel.AutoSize = true;
			this.LongRunsLabel.Location = new System.Drawing.Point(207, 22);
			this.LongRunsLabel.Name = "LongRunsLabel";
			this.LongRunsLabel.Size = new System.Drawing.Size(59, 13);
			this.LongRunsLabel.TabIndex = 11;
			this.LongRunsLabel.Text = "Long Runs";
			// 
			// LongToleranceBox
			// 
			this.LongToleranceBox.Location = new System.Drawing.Point(301, 71);
			this.LongToleranceBox.Name = "LongToleranceBox";
			this.LongToleranceBox.Size = new System.Drawing.Size(100, 20);
			this.LongToleranceBox.TabIndex = 10;
			this.LongToleranceBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LongToleranceBox_KeyPress);
			// 
			// LongEvaluationsBox
			// 
			this.LongEvaluationsBox.Location = new System.Drawing.Point(301, 45);
			this.LongEvaluationsBox.Name = "LongEvaluationsBox";
			this.LongEvaluationsBox.Size = new System.Drawing.Size(100, 20);
			this.LongEvaluationsBox.TabIndex = 9;
			this.LongEvaluationsBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LongEvaluationsBox_KeyPress);
			// 
			// LongRunsBox
			// 
			this.LongRunsBox.Location = new System.Drawing.Point(301, 19);
			this.LongRunsBox.Name = "LongRunsBox";
			this.LongRunsBox.Size = new System.Drawing.Size(100, 20);
			this.LongRunsBox.TabIndex = 8;
			this.LongRunsBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LongRunsBox_KeyPress);
			// 
			// PerturbationLabel
			// 
			this.PerturbationLabel.AutoSize = true;
			this.PerturbationLabel.Location = new System.Drawing.Point(6, 100);
			this.PerturbationLabel.Name = "PerturbationLabel";
			this.PerturbationLabel.Size = new System.Drawing.Size(94, 13);
			this.PerturbationLabel.TabIndex = 7;
			this.PerturbationLabel.Text = "Value Perturbation";
			// 
			// ShortToleranceLabel
			// 
			this.ShortToleranceLabel.AutoSize = true;
			this.ShortToleranceLabel.Location = new System.Drawing.Point(6, 74);
			this.ShortToleranceLabel.Name = "ShortToleranceLabel";
			this.ShortToleranceLabel.Size = new System.Drawing.Size(83, 13);
			this.ShortToleranceLabel.TabIndex = 6;
			this.ShortToleranceLabel.Text = "Short Tolerance";
			// 
			// ShortEvaluationsLabel
			// 
			this.ShortEvaluationsLabel.AutoSize = true;
			this.ShortEvaluationsLabel.Location = new System.Drawing.Point(5, 48);
			this.ShortEvaluationsLabel.Name = "ShortEvaluationsLabel";
			this.ShortEvaluationsLabel.Size = new System.Drawing.Size(90, 13);
			this.ShortEvaluationsLabel.TabIndex = 5;
			this.ShortEvaluationsLabel.Text = "Short Evaluations";
			// 
			// ShortRunsLabel
			// 
			this.ShortRunsLabel.AutoSize = true;
			this.ShortRunsLabel.Location = new System.Drawing.Point(5, 22);
			this.ShortRunsLabel.Name = "ShortRunsLabel";
			this.ShortRunsLabel.Size = new System.Drawing.Size(60, 13);
			this.ShortRunsLabel.TabIndex = 4;
			this.ShortRunsLabel.Text = "Short Runs";
			// 
			// PerturbationBox
			// 
			this.PerturbationBox.Location = new System.Drawing.Point(101, 97);
			this.PerturbationBox.Name = "PerturbationBox";
			this.PerturbationBox.Size = new System.Drawing.Size(100, 20);
			this.PerturbationBox.TabIndex = 3;
			this.PerturbationBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.PerturbationBox_KeyPress);
			// 
			// ShortToleranceBox
			// 
			this.ShortToleranceBox.Location = new System.Drawing.Point(101, 71);
			this.ShortToleranceBox.Name = "ShortToleranceBox";
			this.ShortToleranceBox.Size = new System.Drawing.Size(100, 20);
			this.ShortToleranceBox.TabIndex = 2;
			this.ShortToleranceBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ShortToleranceBox_KeyPress);
			// 
			// ShortEvaluationsBox
			// 
			this.ShortEvaluationsBox.Location = new System.Drawing.Point(101, 45);
			this.ShortEvaluationsBox.Name = "ShortEvaluationsBox";
			this.ShortEvaluationsBox.Size = new System.Drawing.Size(100, 20);
			this.ShortEvaluationsBox.TabIndex = 1;
			this.ShortEvaluationsBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ShortEvaluationsBox_KeyPress);
			// 
			// ShortRunBox
			// 
			this.ShortRunBox.Location = new System.Drawing.Point(101, 19);
			this.ShortRunBox.Name = "ShortRunBox";
			this.ShortRunBox.Size = new System.Drawing.Size(100, 20);
			this.ShortRunBox.TabIndex = 0;
			this.ShortRunBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ShortRunBox_KeyPress);
			// 
			// AdvancedOptionsUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(441, 256);
			this.Controls.Add(this.AdvCancelButton);
			this.Controls.Add(this.AdvDoneButton);
			this.Controls.Add(this.NontargetsLabel);
			this.Controls.Add(this.TargetsLabel);
			this.Controls.Add(this.NontargetQuantileBox5);
			this.Controls.Add(this.NontargetQuantileBox4);
			this.Controls.Add(this.NontargetQuantileBox3);
			this.Controls.Add(this.NontargetQuantileBox2);
			this.Controls.Add(this.NontargetQuantileBox1);
			this.Controls.Add(this.TargetQuantileBox5);
			this.Controls.Add(this.TargetQuantileBox4);
			this.Controls.Add(this.TargetQuantileBox3);
			this.Controls.Add(this.TargetQuantileBox2);
			this.Controls.Add(this.TargetQuantileBox1);
			this.Controls.Add(this.QuantileMethodBox);
			this.Controls.Add(this.QuantileLabel);
			this.Controls.Add(this.OptimizationGroupBox);
			this.Name = "AdvancedOptionsUI";
			this.Text = "AdvancedOptionsUI";
			this.OptimizationGroupBox.ResumeLayout(false);
			this.OptimizationGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label QuantileLabel;
		private System.Windows.Forms.ComboBox QuantileMethodBox;
		private System.Windows.Forms.TextBox TargetQuantileBox1;
		private System.Windows.Forms.TextBox TargetQuantileBox2;
		private System.Windows.Forms.TextBox TargetQuantileBox3;
		private System.Windows.Forms.TextBox TargetQuantileBox4;
		private System.Windows.Forms.TextBox TargetQuantileBox5;
		private System.Windows.Forms.TextBox NontargetQuantileBox1;
		private System.Windows.Forms.TextBox NontargetQuantileBox2;
		private System.Windows.Forms.TextBox NontargetQuantileBox3;
		private System.Windows.Forms.TextBox NontargetQuantileBox4;
		private System.Windows.Forms.TextBox NontargetQuantileBox5;
		private System.Windows.Forms.Label TargetsLabel;
		private System.Windows.Forms.Label NontargetsLabel;
		private System.Windows.Forms.Button AdvDoneButton;
		private System.Windows.Forms.Button AdvCancelButton;
		private System.Windows.Forms.GroupBox OptimizationGroupBox;
		private System.Windows.Forms.TextBox PerturbationBox;
		private System.Windows.Forms.TextBox ShortToleranceBox;
		private System.Windows.Forms.TextBox ShortEvaluationsBox;
		private System.Windows.Forms.TextBox ShortRunBox;
		private System.Windows.Forms.Label PerturbationLabel;
		private System.Windows.Forms.Label ShortToleranceLabel;
		private System.Windows.Forms.Label ShortEvaluationsLabel;
		private System.Windows.Forms.Label ShortRunsLabel;
		private System.Windows.Forms.Label LongToleranceLabel;
		private System.Windows.Forms.Label LongEvaluationsLabel;
		private System.Windows.Forms.Label LongRunsLabel;
		private System.Windows.Forms.TextBox LongToleranceBox;
		private System.Windows.Forms.TextBox LongEvaluationsBox;
		private System.Windows.Forms.TextBox LongRunsBox;
		private System.Windows.Forms.Label NumberRetriesLabel;
		private System.Windows.Forms.TextBox NumberRetriesBox;
	}
}