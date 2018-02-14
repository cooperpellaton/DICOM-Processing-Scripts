using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DifMod {
	public partial class ViewDescriptivesUI : Form {
		public ViewDescriptivesUI () {
			InitializeComponent();

			ConditionDropDown.MaxDropDownItems = Program.oModel._nEvents;
			for ( int i = 0; i < Program.oModel._nEvents; i++ ) {
				ConditionDropDown.Items.Add( "Condition " + ( i + 1 ) );
			}
			ConditionDropDown.SelectedIndex = 0;
		}

		private void ConditionDropDown_SelectedIndexChanged ( object sender, EventArgs e ) {
			UpdateLabels( ConditionDropDown.SelectedIndex );
		}

		private void UpdateLabels ( int nEvent ) {
			NTargetsLabel.Text = string.Format( "N: {0}",
				Program.oModel._eventData[nEvent].descriptives.nTarget.ToString() );
			NNonTargetsLabel.Text = string.Format( "N: {0}",
				Program.oModel._eventData[nEvent].descriptives.nNontarget.ToString() );
			TargetProportionLabel.Text = string.Format( "Proportion Targets: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.fTargetProportion );
			NonTargetProportionLabel.Text = string.Format( "Proportion Non-Targets: {0:F3}",
				( 1f - Program.oModel._eventData[nEvent].descriptives.fTargetProportion ) );

			TargetObsFreq1Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqTarget[0] );
			TargetObsFreq2Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqTarget[1] );
			TargetObsFreq3Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqTarget[2] );
			TargetObsFreq4Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqTarget[3] );
			TargetObsFreq5Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqTarget[4] );
			TargetObsFreq6Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqTarget[5] );

			NonTargetObsFreq1Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqNontarget[0] );
			NonTargetObsFreq2Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqNontarget[1] );
			NonTargetObsFreq3Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqNontarget[2] );
			NonTargetObsFreq4Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqNontarget[3] );
			NonTargetObsFreq5Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqNontarget[4] );
			NonTargetObsFreq6Label.Text = string.Format( "{0}",
				Program.oModel._eventData[nEvent].descriptives.nBinObsFreqNontarget[5] );

			TargetQuantile1Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesTarget[0] );
			TargetQuantile2Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesTarget[1] );
			TargetQuantile3Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesTarget[2] );
			TargetQuantile4Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesTarget[3] );
			TargetQuantile5Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesTarget[4] );

			NonTargetQuantile1Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesNontarget[0] );
			NonTargetQuantile2Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesNontarget[1] );
			NonTargetQuantile3Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesNontarget[2] );
			NonTargetQuantile4Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesNontarget[3] );
			NonTargetQuantile5Label.Text = string.Format( "{0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dQuantilesNontarget[4] );

			TargetMinRtLabel.Text = string.Format( "Min RT: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtMinTarget );
			TargetMaxRtLabel.Text = string.Format( "Max RT: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtMaxTarget );
			TargetMeanRtLabel.Text = string.Format( "Mean RT: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtMean );
			TargetRtVarianceLabel.Text = string.Format( "RT Variance: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtVarTarget );

			NonTargetMinRtLabel.Text = string.Format( "Min RT: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtMinNontarget );
			NonTargetMaxRtLabel.Text = string.Format( "Max RT: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtMaxNontarget );
			NonTargetsMeanRtLabel.Text = string.Format( "Mean RT: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtMeanNontarget );
			NonTargetRtVarianceLabel.Text = string.Format( "RT Variance: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtVarNontarget );

			NTotalTrialsLabel.Text = string.Format( "N Total Trials: {0}",
				Program.oModel._eventData[nEvent].descriptives.nTotalTrials );
			TotalMeanRtLabel.Text = string.Format( "Mean RT: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtMean );
			TotalRtVarianceLabel.Text = string.Format( "RT Variance: {0:F3}",
				Program.oModel._eventData[nEvent].descriptives.dRtVar );
		}

		private void CloseDescrButton_Click ( object sender, EventArgs e ) {
			this.Close();
		}
	}
}
