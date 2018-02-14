namespace DifMod {
	partial class OptimizationDetailsUI {
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
			this.CloseButton = new System.Windows.Forms.Button();
			this.ViewBoxLabel = new System.Windows.Forms.Label();
			this.RoutineSelectionBox = new System.Windows.Forms.ComboBox();
			this.ViewRegConstantsButton = new System.Windows.Forms.Button();
			this.RegressionConstantsTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.ErrorValueLabel = new System.Windows.Forms.Label();
			this.EvalCountLabel = new System.Windows.Forms.Label();
			this.TerminationReasonLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.Location = new System.Drawing.Point(175, 160);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(75, 23);
			this.CloseButton.TabIndex = 0;
			this.CloseButton.Text = "Close";
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
			// 
			// ViewBoxLabel
			// 
			this.ViewBoxLabel.AutoSize = true;
			this.ViewBoxLabel.Location = new System.Drawing.Point(12, 15);
			this.ViewBoxLabel.Name = "ViewBoxLabel";
			this.ViewBoxLabel.Size = new System.Drawing.Size(139, 13);
			this.ViewBoxLabel.TabIndex = 1;
			this.ViewBoxLabel.Text = "View optimization results for:";
			// 
			// RoutineSelectionBox
			// 
			this.RoutineSelectionBox.FormattingEnabled = true;
			this.RoutineSelectionBox.Location = new System.Drawing.Point(42, 31);
			this.RoutineSelectionBox.Name = "RoutineSelectionBox";
			this.RoutineSelectionBox.Size = new System.Drawing.Size(121, 21);
			this.RoutineSelectionBox.TabIndex = 2;
			this.RoutineSelectionBox.SelectedIndexChanged += new System.EventHandler(this.RoutineSelectionBox_SelectedIndexChanged);
			// 
			// ViewRegConstantsButton
			// 
			this.ViewRegConstantsButton.Location = new System.Drawing.Point(12, 67);
			this.ViewRegConstantsButton.Name = "ViewRegConstantsButton";
			this.ViewRegConstantsButton.Size = new System.Drawing.Size(151, 23);
			this.ViewRegConstantsButton.TabIndex = 3;
			this.ViewRegConstantsButton.Text = "View Regression Constants";
			this.RegressionConstantsTooltip.SetToolTip(this.ViewRegConstantsButton, "Regression constants are the free parameters in the model \r\nthat are output from " +
        "each run of the optimization algorithm.");
			this.ViewRegConstantsButton.UseVisualStyleBackColor = true;
			this.ViewRegConstantsButton.Click += new System.EventHandler(this.ViewRegConstantsButton_Click);
			// 
			// ErrorValueLabel
			// 
			this.ErrorValueLabel.AutoSize = true;
			this.ErrorValueLabel.Location = new System.Drawing.Point(13, 97);
			this.ErrorValueLabel.Name = "ErrorValueLabel";
			this.ErrorValueLabel.Size = new System.Drawing.Size(62, 13);
			this.ErrorValueLabel.TabIndex = 4;
			this.ErrorValueLabel.Text = "Error Value:";
			// 
			// EvalCountLabel
			// 
			this.EvalCountLabel.AutoSize = true;
			this.EvalCountLabel.Location = new System.Drawing.Point(13, 119);
			this.EvalCountLabel.Name = "EvalCountLabel";
			this.EvalCountLabel.Size = new System.Drawing.Size(157, 13);
			this.EvalCountLabel.TabIndex = 5;
			this.EvalCountLabel.Text = "Number of function evaluations:";
			this.RegressionConstantsTooltip.SetToolTip(this.EvalCountLabel, "The number of evaluations of the cost function before\r\nthe routine terminated.");
			// 
			// TerminationReasonLabel
			// 
			this.TerminationReasonLabel.AutoSize = true;
			this.TerminationReasonLabel.Location = new System.Drawing.Point(13, 141);
			this.TerminationReasonLabel.Name = "TerminationReasonLabel";
			this.TerminationReasonLabel.Size = new System.Drawing.Size(105, 13);
			this.TerminationReasonLabel.TabIndex = 6;
			this.TerminationReasonLabel.Text = "Termination Reason:";
			this.RegressionConstantsTooltip.SetToolTip(this.TerminationReasonLabel, "The optimization routine will terminate when it converges\r\non an optimal solution" +
        " or reaches the maximum number \r\nof evaluations specified in the model options.");
			// 
			// OptimizationDetailsUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(262, 195);
			this.Controls.Add(this.TerminationReasonLabel);
			this.Controls.Add(this.EvalCountLabel);
			this.Controls.Add(this.ErrorValueLabel);
			this.Controls.Add(this.ViewRegConstantsButton);
			this.Controls.Add(this.RoutineSelectionBox);
			this.Controls.Add(this.ViewBoxLabel);
			this.Controls.Add(this.CloseButton);
			this.Name = "OptimizationDetailsUI";
			this.Text = "Optimization Results";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button CloseButton;
		private System.Windows.Forms.Label ViewBoxLabel;
		private System.Windows.Forms.ComboBox RoutineSelectionBox;
		private System.Windows.Forms.Button ViewRegConstantsButton;
		private System.Windows.Forms.ToolTip RegressionConstantsTooltip;
		private System.Windows.Forms.Label ErrorValueLabel;
		private System.Windows.Forms.Label EvalCountLabel;
		private System.Windows.Forms.Label TerminationReasonLabel;
	}
}