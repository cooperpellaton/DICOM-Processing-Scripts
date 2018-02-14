using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DifMod {
	
	public partial class AdvancedOptionsUI : Form {
		
		public AdvancedOptionsUI () {
			InitializeComponent();
			ShortEvaluationsBox.Text = consts_h.NMS_SHORT_EVALS_DEFAULT.ToString();
			ShortRunBox.Text = consts_h.NMS_SHORT_RUNS_DEFAULT.ToString();
			ShortToleranceBox.Text = consts_h.NMS_SHORT_TOLERANCE_DEFAULT.ToString();
			LongEvaluationsBox.Text = consts_h.NMS_LONG_EVALS_DEFAULT.ToString();
			LongRunsBox.Text = consts_h.NMS_LONG_RUNS_DEFAULT.ToString();
			LongToleranceBox.Text = consts_h.NMS_LONG_TOLERANCE_DEFAULT.ToString();
			PerturbationBox.Text = consts_h.NMS_INITIAL_PERTURB_DEFAULT.ToString();
			NumberRetriesBox.Text = Program.oModelInput.nRetry.ToString();
		}

		private void QuantileMethodBox_SelectedIndexChanged ( object sender, EventArgs e ) {
			if ( QuantileMethodBox.Text == "Default" ) {
				ToggleCustomQuantileBoxes( false );
			} else if ( QuantileMethodBox.Text == "Custom Quantiles" ) {
				ToggleCustomQuantileBoxes( true );
				DisplayCustomQuantileDefaults( consts_h.EST_METHOD_BINS_USER_QUANTILE );
			} else if ( QuantileMethodBox.Text == "Custom Percentiles" ) {
				ToggleCustomQuantileBoxes( true );
				DisplayCustomQuantileDefaults( consts_h.EST_METHOD_BINS_USER_PERCENT );
			}
		}
		private void ToggleCustomQuantileBoxes ( bool bShow ) {
			if ( !bShow ) {
				TargetQuantileBox1.Hide(); TargetQuantileBox2.Hide(); TargetQuantileBox3.Hide();
				TargetQuantileBox4.Hide(); TargetQuantileBox5.Hide(); TargetsLabel.Hide();
				NontargetQuantileBox1.Hide(); NontargetQuantileBox2.Hide();
				NontargetQuantileBox3.Hide(); NontargetQuantileBox4.Hide();
				NontargetQuantileBox5.Hide(); NontargetsLabel.Hide();
			} else {
				TargetQuantileBox1.Show(); TargetQuantileBox2.Show(); TargetQuantileBox3.Show();
				TargetQuantileBox4.Show(); TargetQuantileBox5.Show(); TargetsLabel.Show();
				NontargetQuantileBox1.Show(); NontargetQuantileBox2.Show();
				NontargetQuantileBox3.Show(); NontargetQuantileBox4.Show();
				NontargetQuantileBox5.Show(); NontargetsLabel.Show();
			}
		}
		private void DisplayCustomQuantileDefaults ( int nOption ) {
			if ( nOption == consts_h.EST_METHOD_BINS_USER_QUANTILE ) {
				TargetQuantileBox1.Text = consts_h.dUserQuantilesCor[0].ToString();
				TargetQuantileBox2.Text = consts_h.dUserQuantilesCor[1].ToString();
				TargetQuantileBox3.Text = consts_h.dUserQuantilesCor[2].ToString();
				TargetQuantileBox4.Text = consts_h.dUserQuantilesCor[3].ToString();
				TargetQuantileBox5.Text = consts_h.dUserQuantilesCor[4].ToString();
				NontargetQuantileBox1.Text = consts_h.dUserQuantilesInc[0].ToString();
				NontargetQuantileBox2.Text = consts_h.dUserQuantilesInc[1].ToString();
				NontargetQuantileBox3.Text = consts_h.dUserQuantilesInc[2].ToString();
				NontargetQuantileBox4.Text = consts_h.dUserQuantilesInc[3].ToString();
				NontargetQuantileBox5.Text = consts_h.dUserQuantilesInc[4].ToString();
			} else if ( nOption == consts_h.EST_METHOD_BINS_USER_PERCENT ) {
				TargetQuantileBox1.Text = consts_h.dUserPercentilesCor[0].ToString();
				TargetQuantileBox2.Text = consts_h.dUserPercentilesCor[1].ToString();
				TargetQuantileBox3.Text = consts_h.dUserPercentilesCor[2].ToString();
				TargetQuantileBox4.Text = consts_h.dUserPercentilesCor[3].ToString();
				TargetQuantileBox5.Text = consts_h.dUserPercentilesCor[4].ToString();
				NontargetQuantileBox1.Text = consts_h.dUserPercentilesCor[0].ToString();
				NontargetQuantileBox2.Text = consts_h.dUserPercentilesCor[1].ToString();
				NontargetQuantileBox3.Text = consts_h.dUserPercentilesCor[2].ToString();
				NontargetQuantileBox4.Text = consts_h.dUserPercentilesCor[3].ToString();
				NontargetQuantileBox5.Text = consts_h.dUserPercentilesCor[4].ToString();
			}
		}

		private void AdvCancelButton_Click ( object sender, EventArgs e ) {
			this.Close();
		}

		private void AdvDoneButton_Click ( object sender, EventArgs e ) {
			#region Save Quantile Settings To Input
			if ( QuantileMethodBox.Text == "Custom Quantiles" ) {
				Program.oModelInput.dUserQuantilesCor[0] = Convert.ToDouble( TargetQuantileBox1.Text );
				Program.oModelInput.dUserQuantilesCor[1] = Convert.ToDouble( TargetQuantileBox2.Text );
				Program.oModelInput.dUserQuantilesCor[2] = Convert.ToDouble( TargetQuantileBox3.Text );
				Program.oModelInput.dUserQuantilesCor[3] = Convert.ToDouble( TargetQuantileBox4.Text );
				Program.oModelInput.dUserQuantilesCor[4] = Convert.ToDouble( TargetQuantileBox5.Text );
				Program.oModelInput.dUserQuantilesInc[0] = Convert.ToDouble( NontargetQuantileBox1.Text );
				Program.oModelInput.dUserQuantilesInc[1] = Convert.ToDouble( NontargetQuantileBox2.Text );
				Program.oModelInput.dUserQuantilesInc[2] = Convert.ToDouble( NontargetQuantileBox3.Text );
				Program.oModelInput.dUserQuantilesInc[3] = Convert.ToDouble( NontargetQuantileBox4.Text );
				Program.oModelInput.dUserQuantilesInc[4] = Convert.ToDouble( NontargetQuantileBox5.Text );
			} else if ( QuantileMethodBox.Text == "Custom Percentiles" ) {
				Program.oModelInput.dUserPercentilesCor[0] = Convert.ToDouble( TargetQuantileBox1.Text );
				Program.oModelInput.dUserPercentilesCor[1] = Convert.ToDouble( TargetQuantileBox2.Text );
				Program.oModelInput.dUserPercentilesCor[2] = Convert.ToDouble( TargetQuantileBox3.Text );
				Program.oModelInput.dUserPercentilesCor[3] = Convert.ToDouble( TargetQuantileBox4.Text );
				Program.oModelInput.dUserPercentilesCor[4] = Convert.ToDouble( TargetQuantileBox5.Text );
				Program.oModelInput.dUserPercentilesInc[0] = Convert.ToDouble( NontargetQuantileBox1.Text );
				Program.oModelInput.dUserPercentilesInc[1] = Convert.ToDouble( NontargetQuantileBox2.Text );
				Program.oModelInput.dUserPercentilesInc[2] = Convert.ToDouble( NontargetQuantileBox3.Text );
				Program.oModelInput.dUserPercentilesInc[3] = Convert.ToDouble( NontargetQuantileBox4.Text );
				Program.oModelInput.dUserPercentilesInc[4] = Convert.ToDouble( NontargetQuantileBox5.Text );
			}
			#endregion
			#region Save Optimization Routine Options to Input
			Program.oModelInput.dLongNmsTolerance = Convert.ToDouble( LongToleranceBox.Text );
			Program.oModelInput.nLongNmsEvals = Convert.ToInt32( LongEvaluationsBox.Text );
			Program.oModelInput.nLongNmsRuns = Convert.ToInt32( LongRunsBox.Text );
			Program.oModelInput.dShortNmsTolerance = Convert.ToDouble( ShortToleranceBox.Text );
			Program.oModelInput.nShortNmsEvals = Convert.ToInt32( ShortEvaluationsBox.Text );
			Program.oModelInput.nShortNmsRuns = Convert.ToInt32( ShortRunBox.Text );
			Program.oModelInput.dNmsInitPerturb = Convert.ToDouble( PerturbationBox.Text );
			Program.oModelInput.nRetry = Convert.ToInt32( NumberRetriesBox.Text );
			#endregion
			this.Close();
		}

		#region Box Input Data Type Handlers
		private void TargetQuantileBox1_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void TargetQuantileBox2_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void TargetQuantileBox3_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void TargetQuantileBox4_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void TargetQuantileBox5_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}

		private void NontargetQuantileBox1_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void NontargetQuantileBox2_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void NontargetQuantileBox3_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void NontargetQuantileBox4_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void NontargetQuantileBox5_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}

		private void ShortRunBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Back ) {
				e.Handled = true;
			}
		}
		private void ShortToleranceBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void LongToleranceBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void PerturbationBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Decimal &&
				e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Convert.ToChar( "." ) ) {
				e.Handled = true;
			}
		}
		private void LongRunsBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Back ) {
				e.Handled = true;
			}
		}
		private void ShortEvaluationsBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Back ) {
				e.Handled = true;
			}
		}
		private void LongEvaluationsBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Back ) {
				e.Handled = true;
			}
		}
		private void NumberRetriesBox_KeyPress ( object sender, KeyPressEventArgs e ) {
			if ( !char.IsNumber( e.KeyChar ) && e.KeyChar != (char)Keys.Back ) {
				e.Handled = true;
			}
		}
		#endregion

	}
}
