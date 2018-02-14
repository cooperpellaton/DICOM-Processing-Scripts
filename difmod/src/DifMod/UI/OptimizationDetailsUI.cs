using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DifMod {
	public partial class OptimizationDetailsUI : Form {
		public OptimizationDetailsUI () {
			InitializeComponent();

			RoutineSelectionBox.MaxDropDownItems = Program.oModel._nmsResults.Count;
			for ( int i = 0; i < Program.oModel._nmsResults.Count; i++ ) {
				RoutineSelectionBox.Items.Add( "Simplex " + ( i + 1 ) );
			}
			RoutineSelectionBox.SelectedIndex = 0;
		}
		private void CloseButton_Click ( object sender, EventArgs e ) {
			this.Close();
		}
		private void RoutineSelectionBox_SelectedIndexChanged ( object sender, EventArgs e ) {
			UpdateResults( RoutineSelectionBox.SelectedIndex );
		}
		private void UpdateResults ( int nSimplex ) {
			ErrorValueLabel.Text = string.Format( "Error Value: {0:F3}",
				Program.oModel._nmsResults[nSimplex].ErrorValue );
			EvalCountLabel.Text = string.Format( "Number of function evaluations: {0:F0}",
				Program.oModel._nmsResults[nSimplex].EvaluationCount );
			if ( Program.oModel._nmsResults[nSimplex].TerminationReason == TerminationReason.Converged ) {
				TerminationReasonLabel.Text = "Termination reason: Converged";
			} else if ( Program.oModel._nmsResults[nSimplex].TerminationReason == TerminationReason.MaxFunctionEvaluations ) {
				TerminationReasonLabel.Text = "Termination reason: Max Evaluations Reached";
			} else {
				TerminationReasonLabel.Text = "Termination reason: Unspecified";
			}
		}

		private void ViewRegConstantsButton_Click ( object sender, EventArgs e ) {

		}
	}
}
