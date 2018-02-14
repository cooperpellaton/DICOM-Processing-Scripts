using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DifMod {
	
	public partial class AdvModelOptionsUI : Form {
		
		public AdvModelOptionsUI () {
			InitializeComponent();
			Text = "Advanced Model Options";
		}

		private void AdvModelOptsDoneButton_Click ( object sender, EventArgs e ) {
			this.Close();
		}

		private void GuessMethodBox_SelectedIndexChanged ( object sender, EventArgs e ) {
			if ( GuessMethodBox.Text == "Default" ) {
			} else if ( GuessMethodBox.Text == "Custom" ) {
				MessageBoxButtons button = MessageBoxButtons.OK;
				MessageBoxIcon icon = MessageBoxIcon.Exclamation;
				string msgBoxText = "Parameter guesses haven't been implemented yet.";
				string caption = "Not implemented yet.";
				DialogResult result = MessageBox.Show( msgBoxText, caption, button, icon );
				GuessMethodBox.Text = "Default";
				return;
			}
		}

		private void CustomDesignMatrixButton_Click ( object sender, EventArgs e ) {
			MessageBoxButtons button = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Exclamation;
			string msgBoxText = "Custom design matrices haven't been implemented yet.";
			string caption = "Not implemented yet.";
			DialogResult result = MessageBox.Show( msgBoxText, caption, button, icon );
			GuessMethodBox.Text = "Default";
			return;
		}

		private void ConditionLabelsButton_Click ( object sender, EventArgs e ) {
			MessageBoxButtons button = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Exclamation;
			string msgBoxText = "Condition labels coming soon!";
			string caption = "Feature in development!";
			DialogResult result = MessageBox.Show( msgBoxText, caption, button, icon );
			return;
		}


		#region Box Input Data Type Handlers
		private void AdvParam1Box_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void AdvParam4Box_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void AdvParam7Box_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void AdvParam2Box_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void AdvParam3Box_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void AdvParam5Box_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void AdvParam6Box_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		#endregion

	}
}
