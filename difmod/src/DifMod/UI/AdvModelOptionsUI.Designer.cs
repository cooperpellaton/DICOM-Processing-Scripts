namespace DifMod {
	partial class AdvModelOptionsUI {
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
			this.AdvModelOptsDoneButton = new System.Windows.Forms.Button();
			this.GuessMethodBox = new System.Windows.Forms.ComboBox();
			this.GuessMethodLabel = new System.Windows.Forms.Label();
			this.CustomDesignMatrixButton = new System.Windows.Forms.Button();
			this.ConditionLabelsButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// AdvModelOptsDoneButton
			// 
			this.AdvModelOptsDoneButton.Location = new System.Drawing.Point(12, 110);
			this.AdvModelOptsDoneButton.Name = "AdvModelOptsDoneButton";
			this.AdvModelOptsDoneButton.Size = new System.Drawing.Size(148, 23);
			this.AdvModelOptsDoneButton.TabIndex = 0;
			this.AdvModelOptsDoneButton.Text = "Done";
			this.AdvModelOptsDoneButton.UseVisualStyleBackColor = true;
			this.AdvModelOptsDoneButton.Click += new System.EventHandler(this.AdvModelOptsDoneButton_Click);
			// 
			// GuessMethodBox
			// 
			this.GuessMethodBox.FormattingEnabled = true;
			this.GuessMethodBox.Items.AddRange(new object[] {
            "Default",
            "Custom"});
			this.GuessMethodBox.Location = new System.Drawing.Point(12, 25);
			this.GuessMethodBox.Name = "GuessMethodBox";
			this.GuessMethodBox.Size = new System.Drawing.Size(148, 21);
			this.GuessMethodBox.TabIndex = 31;
			this.GuessMethodBox.Text = "Default";
			this.GuessMethodBox.SelectedIndexChanged += new System.EventHandler(this.GuessMethodBox_SelectedIndexChanged);
			// 
			// GuessMethodLabel
			// 
			this.GuessMethodLabel.AutoSize = true;
			this.GuessMethodLabel.Location = new System.Drawing.Point(9, 9);
			this.GuessMethodLabel.Name = "GuessMethodLabel";
			this.GuessMethodLabel.Size = new System.Drawing.Size(113, 13);
			this.GuessMethodLabel.TabIndex = 30;
			this.GuessMethodLabel.Text = "Initial Estimate Method";
			// 
			// CustomDesignMatrixButton
			// 
			this.CustomDesignMatrixButton.Location = new System.Drawing.Point(12, 52);
			this.CustomDesignMatrixButton.Name = "CustomDesignMatrixButton";
			this.CustomDesignMatrixButton.Size = new System.Drawing.Size(148, 23);
			this.CustomDesignMatrixButton.TabIndex = 46;
			this.CustomDesignMatrixButton.Text = "Customize Design Matrices";
			this.CustomDesignMatrixButton.UseVisualStyleBackColor = true;
			this.CustomDesignMatrixButton.Click += new System.EventHandler(this.CustomDesignMatrixButton_Click);
			// 
			// ConditionLabelsButton
			// 
			this.ConditionLabelsButton.Location = new System.Drawing.Point(12, 81);
			this.ConditionLabelsButton.Name = "ConditionLabelsButton";
			this.ConditionLabelsButton.Size = new System.Drawing.Size(148, 23);
			this.ConditionLabelsButton.TabIndex = 47;
			this.ConditionLabelsButton.Text = "Condition Labels";
			this.ConditionLabelsButton.UseVisualStyleBackColor = true;
			this.ConditionLabelsButton.Click += new System.EventHandler(this.ConditionLabelsButton_Click);
			// 
			// AdvModelOptionsUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(173, 143);
			this.Controls.Add(this.ConditionLabelsButton);
			this.Controls.Add(this.CustomDesignMatrixButton);
			this.Controls.Add(this.GuessMethodBox);
			this.Controls.Add(this.GuessMethodLabel);
			this.Controls.Add(this.AdvModelOptsDoneButton);
			this.Name = "AdvModelOptionsUI";
			this.Text = "Advanced Model Options";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button AdvModelOptsDoneButton;
		private System.Windows.Forms.ComboBox GuessMethodBox;
		private System.Windows.Forms.Label GuessMethodLabel;
		private System.Windows.Forms.Button CustomDesignMatrixButton;
		private System.Windows.Forms.Button ConditionLabelsButton;
	}
}