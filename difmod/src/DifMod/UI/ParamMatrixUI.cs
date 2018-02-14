using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DifMod {
	public partial class ParamMatrixUI : Form {
		public ParamMatrixUI () {
			InitializeComponent();
			this.Text = "Parameter Matrix";
			ParamMatrixGrid.ColumnCount = Program.oModelInput.nParamCount;
			
			ParamMatrixGrid.Columns[0].Name = "a";
			ParamMatrixGrid.Columns[1].Name = "Ter";
			ParamMatrixGrid.Columns[2].Name = "Eta";
			ParamMatrixGrid.Columns[3].Name = "Z";
			ParamMatrixGrid.Columns[4].Name = "S(Z)";
			ParamMatrixGrid.Columns[5].Name = "S(T)";
			ParamMatrixGrid.Columns[6].Name = "Nu";
			for ( int i = 0; i < ParamMatrixGrid.Columns.Count; i++ ) {
				ParamMatrixGrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
			}
			for ( int i = 0; i < Program.oModel._nEvents; i++ ) {
				object[] row = new object[]{
					Program.oModel._paramMatrix[i,0],
					Program.oModel._paramMatrix[i,1],
					Program.oModel._paramMatrix[i,2],
					Program.oModel._paramMatrix[i,3],
					Program.oModel._paramMatrix[i,4],
					Program.oModel._paramMatrix[i,5],
					Program.oModel._paramMatrix[i,6]
				};
				ParamMatrixGrid.Rows.Add(row);
			}
			ParamMatrixGrid.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
		}

		private void ParamMatrixCloseButton_Click ( object sender, EventArgs e ) {
			this.Close();
		}
		private void exitToolStripMenuItem1_Click ( object sender, EventArgs e ) {
			this.Close();
		}

		private void copyToolStripMenuItem_Click ( object sender, EventArgs e ) {
			if ( this.ParamMatrixGrid.GetCellCount( DataGridViewElementStates.Selected ) > 0 ) {
				try {
					Clipboard.SetDataObject( this.ParamMatrixGrid.GetClipboardContent() );
				} catch ( System.Runtime.InteropServices.ExternalException ) {
					MessageBoxButtons button = MessageBoxButtons.OK;
					string msgBoxText = "Unable to access system clipboard.";
					string caption = "External Error";
					DialogResult result = MessageBox.Show( msgBoxText, caption, button);

				}
			}
		}

		private void selectAllToolStripMenuItem_Click ( object sender, EventArgs e ) {
			ParamMatrixGrid.SelectAll();
		}
	}
}
