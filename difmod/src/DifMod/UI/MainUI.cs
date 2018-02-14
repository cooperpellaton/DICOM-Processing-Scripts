using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DifMod {
	public partial class MainWindow : Form {

		public MainWindow () {
			InitializeComponent();
			Text = "Main Window";
		}

		private void exitToolStripMenuItem_Click ( object sender, EventArgs e ) {
			Close();
		}

		private void newModelToolStripMenuItem_Click ( object sender, EventArgs e ) {
			if ( Program.oModel != null ) {
				ResetModel();
			} else {
				NewModelStart();
			}
		}
		private void NewModelButton_Click ( object sender, EventArgs e ) {
			if ( Program.oModel != null ) {
				ResetModel();
			} else {
				NewModelStart();
			}
		}
		private void NewModelStart () {
			ModelOptionsUI options = new ModelOptionsUI();
			options.Show();
		}
		private void ResetModel () {
			string messageBoxText = "Do you really want to create a new model? Current model will be discarded";
			string caption = "New Model";
			MessageBoxButtons button = MessageBoxButtons.YesNo;
			MessageBoxIcon icon = MessageBoxIcon.Exclamation;
			DialogResult result = MessageBox.Show( messageBoxText, caption, button, icon );
			if ( result == DialogResult.Yes ) {
				AdvModelOptionsButton.Enabled = false;
				RunModelButton.Enabled = false;
				ViewParamMatrixButton.Enabled = false;
				ViewOptimizationDetailsButton.Enabled = false;
				ViewDataButton.Enabled = false;
				ViewDescriptivesButton.Enabled = false;
				FitMeasuresButton.Enabled = false;
				OutlierReportButton.Enabled = false;

				Program.oModel = new DiffusionModel();
				Program.oModelInput = new ModelInput() ;

				NewModelStart();
			} else if ( result == DialogResult.No ) {
				return;
			}
		}

		private void PreprocessDataButton_Click ( object sender, EventArgs e ) {
			if ( Program.oModelInput == null ) {
				MessageBoxButtons button = MessageBoxButtons.OK;
				MessageBoxIcon icon = MessageBoxIcon.Exclamation;
				string msgBoxText = "No dataset loaded!";
				string caption = "";
				MessageBox.Show( msgBoxText, caption, button, icon );
			}
			StatusBarLabel.Text = "Preprocessing...";
			Program.oModel.PreprocessModel();
			RunModelButton.Enabled = true;
			ViewDescriptivesButton.Enabled = true;
			AdvModelOptionsButton.Enabled = true;
			ViewDataButton.Enabled = true;
			StatusBarLabel.Text = "";
		}

		private void RunModelButton_Click ( object sender, EventArgs e ) {
			this.RunModelBgWorker.RunWorkerAsync();
		}
		
		private void ViewParamMatrixButton_Click ( object sender, EventArgs e ) {
			ParamMatrixUI pmForm = new ParamMatrixUI();
			pmForm.Show();
		}

		private void AdvModelOptionsButton_Click ( object sender, EventArgs e ) {
			AdvModelOptionsUI advModelOpts = new AdvModelOptionsUI();
			advModelOpts.Show();
		}

		private void ViewDataButton_Click ( object sender, EventArgs e ) {
			MessageBoxButtons button = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Exclamation;
			string msgBoxText = "Data viewer coming soon!";
			string caption = "Feature in development.";
			MessageBox.Show( msgBoxText, caption, button, icon );
		}

		private void ViewDescriptivesButton_Click ( object sender, EventArgs e ) {
			ViewDescriptivesUI descViewer = new ViewDescriptivesUI();
			descViewer.Show();
		}

		private void OutlierReportButton_Click ( object sender, EventArgs e ) {
			MessageBoxButtons button = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Exclamation;
			string msgBoxText = "Outlier reports coming soon!";
			string caption = "Feature in development.";
			MessageBox.Show( msgBoxText, caption, button, icon );
		}

		private void FitMeasuresButton_Click ( object sender, EventArgs e ) {
			MessageBoxButtons button = MessageBoxButtons.OK;
			MessageBoxIcon icon = MessageBoxIcon.Exclamation;
			string msgBoxText = "Fit statistics viewer coming soon!";
			string caption = "Feature in development.";
			MessageBox.Show( msgBoxText, caption, button, icon );
		}

		private void ViewOptimizationDetailsButton_Click ( object sender, EventArgs e ) {
			OptimizationDetailsUI optDetails = new OptimizationDetailsUI();
			optDetails.Show();
		}

		private void RunModelBackground_DoWork ( object sender, DoWorkEventArgs e ) {
			BackgroundWorker bw = sender as BackgroundWorker;
			RunModelBackgroundOperation(bw);
			if ( bw.CancellationPending ) {
				e.Cancel = true;
			}
		}
		private void RunModelBackground_RunWorkerCompleted ( object sender, RunWorkerCompletedEventArgs e ) {
			if ( e.Cancelled ) {
//				MessageBox.Show( "Optimization canceled" );
			} else if ( e.Error != null ) {
//				string msg = string.Format( "Error: {0}", e.Error.Message );
//				MessageBox.Show( msg );
			} else {
//				string msg = string.Format( "Result: {0}", e.Result );
//				MessageBox.Show( msg );
			}
		}
		private void RunModelBackgroundOperation ( BackgroundWorker bw ) {
			StatusBarLabel.Text = "Fitting Model...";
			Program.oModel.OptimizeModel();
			StatusBarLabel.Text = "";

			SetControlPropertyValue( ViewParamMatrixButton, "Enabled", true );
			SetControlPropertyValue( ViewOptimizationDetailsButton, "Enabled", true );
			SetControlPropertyValue( FitMeasuresButton, "Enabled", true );
			SetControlPropertyValue( OutlierReportButton, "Enabled", true );
		}

		delegate void SetControlValueCallback ( Control oControl, string sPropName, object oPropValue );
		/// <summary> Thread-safe property setter for UI controls. </summary>
		/// <param name="oControl"> Control containing property to set. </param>
		/// <param name="sPropName"> Name of property to set. </param>
		/// <param name="oPropValue"> Value to set. </param>
		private void SetControlPropertyValue ( Control oControl, string sPropName, object oPropValue ) {
			if ( oControl.InvokeRequired ) {
				SetControlValueCallback d = new SetControlValueCallback( SetControlPropertyValue );
				oControl.Invoke( d, new object[] { oControl, sPropName, oPropValue } );
			} else {
				Type t = oControl.GetType();
				PropertyInfo[] props = t.GetProperties();
				foreach ( PropertyInfo p in props ) {
					if ( p.Name.ToUpper() == sPropName.ToUpper() ) {
						p.SetValue( oControl, oPropValue, null );
					}
				}
			}
		}

		private void fileToolStripMenuItem_Click ( object sender, EventArgs e ) {

		}

	}
}
