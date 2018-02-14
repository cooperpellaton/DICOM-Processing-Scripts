using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DifMod {
	public partial class ModelOptionsUI : Form {

		string sFileName = string.Empty;
		
		public ModelOptionsUI () {
			InitializeComponent();
			Program.oModelInput = new ModelInput();
			this.Text = "Model Options";
		}

		private void AdvancedOptionsButton_Click ( object sender, EventArgs e ) {
			AdvancedOptionsUI advOpts = new AdvancedOptionsUI();
			advOpts.Show();
		}

		private void NewModelCancelButton_Click ( object sender, EventArgs e ) {
			this.Close();
		}

		private void LoadFileButton_Click ( object sender, EventArgs e ) {
			OpenFileDialog open = new OpenFileDialog();
			open.Title = "Select your data file...";
			open.Filter = "Excel 1997-2004 files (*.xls)|*.xls";
			open.FilterIndex = 1;
			if ( open.ShowDialog() == DialogResult.OK ) {
				sFileName = open.FileName;
			}
		}

		private void ModelTypeBox_SelectedIndexChanged ( object sender, EventArgs e ) {
			if ( ModelTypeBox.Text == "Standard Diffusion" ) {
				Program.oModelInput.nParamCount = consts_h.PARAM_COUNT_STANDARD_MODEL;
				Program.oModelInput.nModelType = consts_h.MODEL_TYPE_STANDARD;
			} else if ( ModelTypeBox.Text == "Mixed Model" ) {
				Program.oModelInput.nParamCount = consts_h.PARAM_COUNT_MIXED_MODEL;
				Program.oModelInput.nModelType = consts_h.MODEL_TYPE_MIXED;
			}
		}
		private void OutlierOptionsBox_SelectedIndexChanged ( object sender, EventArgs e ) {
			if ( OutlierOptionsBox.Text == "None" ) {
				ToggleOutlierOptions(false);
				Program.oModelInput.nOutlierTreatment = consts_h.OUTLIER_OPT_NONE;
			} else {
				ToggleOutlierOptions(true);
				if ( OutlierOptionsBox.Text == "Relative Bounds" ) {
					Program.oModelInput.nOutlierTreatment = consts_h.OUTLIER_OPT_RELATIVE_CUTOFF;
					OutlierMaxUnitLabel.Text = "SD";
					OutlierMinUnitLabel.Text = "SD";
				} else if ( OutlierOptionsBox.Text == "Absolute Bounds" ) {
					Program.oModelInput.nOutlierTreatment = consts_h.OUTLIER_OPT_ABSOLUTE_CUTOFF;
					OutlierMaxUnitLabel.Text = "s";
					OutlierMinUnitLabel.Text = "s";
				}
			}
		}
		private void ToggleOutlierOptions ( bool bShow ) {
			if ( !bShow ) {
				OutlierMaxLabel.Hide(); OutlierMinLabel.Hide(); OutlierMaxUnitLabel.Hide();
				OutlierMinUnitLabel.Hide(); OutlierMinBox.Hide(); OutlierMaxBox.Hide();
			} else {
				OutlierMaxLabel.Show(); OutlierMinLabel.Show(); OutlierMaxBox.Show();
				OutlierMinBox.Show(); OutlierMinUnitLabel.Show(); OutlierMaxUnitLabel.Show();
			}
		}

		private void OutlierMinBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar(".") ) {
				e.Handled = true;
			}
		}
		private void OutlierMaxBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar(".") ) {
				e.Handled = true;
			}
		}

		private void DoneButton_Click ( object sender, EventArgs e ) {
			Program.oModel = new DiffusionModel();
			if ( sFileName == string.Empty ) {
				MessageBoxButtons button = MessageBoxButtons.OK;
				MessageBoxIcon icon = MessageBoxIcon.Exclamation;
				string msgBoxText = "Please load a data file.";
				string caption = "No data file specified.";
				DialogResult result = MessageBox.Show( msgBoxText, caption, button, icon );
				return;
			}
			Program.oModelInput.sFilePath = sFileName;
			if ( Program.oModelInput.nOutlierTreatment != consts_h.OUTLIER_OPT_NONE ) {
				if ( OutlierMinBox.Text == "" || OutlierMaxBox.Text == "" ) {
					MessageBoxButtons button = MessageBoxButtons.OK;
					MessageBoxIcon icon = MessageBoxIcon.Exclamation;
					string msgBoxText = "Please fill in the outlier cutoff min and max values.";
					string caption = "Invalid outlier cutoffs.";
					DialogResult result = MessageBox.Show( msgBoxText, caption, button, icon );
					return;
				}
				Program.oModelInput.dUserRtMin = Convert.ToDouble( OutlierMinBox.Text );
				Program.oModelInput.dUserRtMax = Convert.ToDouble( OutlierMaxBox.Text );
			}
			
			Program.oModel.SetModelInput( Program.oModelInput );
			Program.oModel.InitModel();

			#region Set Design Matrices
			int nCond = Program.oModel._nEvents;
			Program.oModelInput.designMatrix = new DesignMatrix[Program.oModelInput.nParamCount];
			for ( int i = 0; i < Program.oModelInput.nParamCount; i++ ) {
				Program.oModelInput.designMatrix[i] = new DesignMatrix();
			}
			if ( Param1Box.Text == "Fixed" ) {
				Program.oModelInput.designMatrix[0].CreateNoEffectsMatrix( nCond );
			} else { Program.oModelInput.designMatrix[0].CreateIdentityMatrix( nCond );	}
			if ( Param2Box.Text == "Fixed" ) {
				Program.oModelInput.designMatrix[1].CreateNoEffectsMatrix( nCond );
			} else { Program.oModelInput.designMatrix[1].CreateIdentityMatrix( nCond ); }
			if ( Param3Box.Text == "Fixed" ) {
				Program.oModelInput.designMatrix[2].CreateNoEffectsMatrix( nCond );
			} else { Program.oModelInput.designMatrix[2].CreateIdentityMatrix( nCond ); }
			if ( Param4Box.Text == "Fixed" ) {
				Program.oModelInput.designMatrix[3].CreateNoEffectsMatrix( nCond );
			} else { Program.oModelInput.designMatrix[3].CreateIdentityMatrix( nCond ); }
			if ( Param5Box.Text == "Fixed" ) {
				Program.oModelInput.designMatrix[4].CreateNoEffectsMatrix( nCond );
			} else { Program.oModelInput.designMatrix[4].CreateIdentityMatrix( nCond ); }
			if ( Param6Box.Text == "Fixed" ) {
				Program.oModelInput.designMatrix[5].CreateNoEffectsMatrix( nCond );
			} else { Program.oModelInput.designMatrix[5].CreateIdentityMatrix( nCond ); }
			if ( Param7Box.Text == "Fixed" ) {
				Program.oModelInput.designMatrix[6].CreateNoEffectsMatrix( nCond );
			} else { Program.oModelInput.designMatrix[6].CreateIdentityMatrix( nCond ); }
			#endregion

			// Reset the model input with the design matrices in place.
			Program.oModel.SetModelInput( Program.oModelInput );
			this.Close();
		}

		private void FitAssessmentBox_SelectedIndexChanged ( object sender, EventArgs e ) {
			if ( FitAssessmentBox.Text == "Multinomial Likelihood" ) {
				Program.oModelInput.nEstMethodObjective = consts_h.EST_METHOD_OBJECTIVE_ML;
			} else if ( FitAssessmentBox.Text == "Chi-squared" ) {
				Program.oModelInput.nEstMethodObjective = consts_h.EST_METHOD_OBJECTIVE_CHISQUARE;
			}
		}



	}
}
