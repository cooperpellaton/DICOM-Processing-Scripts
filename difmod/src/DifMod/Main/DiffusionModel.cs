//#define DEBUG_LOG

/// <summary>
/// DiffusionModel.cs
/// 
/// Version 1.0
/// Created by: Josh Tremel (tremeljosh@gmail.com)
/// Created on: January 26, 2012
/// 
/// Main diffusion model class which controls all calculations and fitting of the model.
/// </summary>
using System;
using System.Collections.Generic;

namespace DifMod {
	/// <summary> Diffusion model. </summary>
	public class DiffusionModel {
		#region LOCAL VARIABLES
		/// <summary> Number of events in provided dataset to model. </summary>
		public int _nEvents;
		/// <summary> User input and options for the model. </summary>
		private ModelInput _input = new ModelInput();
		/// <summary> Full parameter matrix ([event, parameter]). </summary> 
		public double[,] _paramMatrix;
		/// <summary> List of free parameters in the model. </summary>
		private List<double> _freeParameters = new List<double>();
		/// <summary> Full raw dataset </summary>
		private ExptDataSet _raw = new ExptDataSet();
		/// <summary> Temporary working dataset. </summary>
		private ExptDataSet _workingData = new ExptDataSet();
		/// <summary> Condition-wise data sets. </summary>
		public ExptDataSet[] _eventData;
		/// <summary> Optimization results. </summary>
		public List<RegressionResult> _nmsResults = new List<RegressionResult>();
		#endregion

		#region PUBLIC METHODS
		/// <summary> Initializes the model and reads in data. </summary>
		public void InitModel () {
			// Read in data
			_raw = new ExptDataSet();
			_raw.ReadFromXLS( _input.sFilePath );
			// Cache the number of events
			_nEvents = _raw.GetEventCount();
			// Set up parameter matrix.
			_paramMatrix = new double[_nEvents, _input.nParamCount];
		}
		/// <summary> Sets the input options for this model. </summary>
		/// <param name='input'> User's custom model input. </param>
		public void SetModelInput ( ModelInput input ) {
			_input = input;
		}
		/// <summary> Preprocesses the model. Removes outliers, splits data condition-wise, calculates descriptive 
		/// stats on the data, and calculates starting estimations of the diffusion parameters. </summary>
		public void PreprocessModel () {
			// Copy to working dataset.
			_workingData = _raw;
			// Process outliers.
			ProcessOutliers( _input.nOutlierTreatment );
			// Split data into condition-wise data sets.
			SplitEventData();
			// Calculate quantiles and stats for each event
			for ( int i = 0; i < _nEvents; i++ ) {
				_eventData[i].CalculateStats( _input );
			}
			// Set up parameter guess values.
			GuessParameters( _input.nParameterGuessMethod );
		}
		/// <summary> Uses a Nelder-Mead Simplex to optimize the values of the free parameters 
		/// in the context of the model. </summary>
		public void OptimizeModel () {
			// Set up the free parameter list.
			InitFreeParamsFromGuesses();

#if DEBUG_LOG
			Console.WriteLine( "\n***Fitting Model***\n" );
#endif
			
			// Run the Nelder-Mead routine.
			RunNelderMeadSimplex();
			// TODO: refine with a BFGS optimizer to prevent getting caught in a local minimum.
			List<double[]> oldParams = new List<double[]>();
			RegressionResult[] oldNmsResults = new RegressionResult[_nmsResults.Count];
			for ( int i = 0; i < _input.nRetry; i++ ) {
				if ( !IsGoodSolution() ) {
#if DEBUG_LOG
					Console.WriteLine( "Some parameters are close to the bounds of parameter space.\nRunning optimization again." );
#endif		
					oldParams.Add( _freeParameters.ToArray() );
					_nmsResults.CopyTo( oldNmsResults );
					_nmsResults.Clear();
					
					RunNelderMeadSimplex();
					if ( !IsGoodSolution() ) {
#if DEBUG_LOG
						Console.WriteLine( "Some parameters are still suspect. Please revise your design." );
#endif
						double curError = _nmsResults[_nmsResults.Count - 1].ErrorValue;
						double oldError = oldNmsResults[oldNmsResults.Length - 1].ErrorValue;
						// if old error is less than current error, that one had a better fit, so revert...
						if ( oldError < curError ) {
							_nmsResults.Clear();
							for ( int j = 0; j < oldNmsResults.Length; j++ ) {
								_nmsResults.Add( oldNmsResults[j] );
							}
							SetFreeParams( _nmsResults[_nmsResults.Count - 1] );
							SetFullParamMatrix( _freeParameters.ToArray() );
						}
					}
				} else {
					break;
				}
			}

			// TODO: Get Final Fit info;
			//GetFinalFit();

			// TODO: Finish Output
			//time, outlier report
			//df = length(freeparameters)
			//DesignVector = freeParameters
			//FitInfo=absfit(minimum,dcel2,df);
		}
		#endregion

		#region INTERNAL PREPROCESSING METHODS
		/// <summary> Processes outliers according to specified option. </summary>
		/// <param name='nOption'> Integer constant for option </param>
		/// <exception cref='NullReferenceException'> Thrown when working data is not initialized. </exception>
		private void ProcessOutliers ( int nOption ) {
			if ( _workingData == null ) {
				throw new NullReferenceException( "Working dataset is null" );
			}
			switch ( nOption ) {
				// By N standard deviations (note dRtMax/min are standard deviations from mean)
				case consts_h.OUTLIER_OPT_RELATIVE_CUTOFF:
					double dMean = _workingData.descriptives.dRtMean;
					double dSd = Math.Sqrt( _workingData.descriptives.dRtVar );
					double dMax = dMean + _input.dUserRtMax * dSd;
					double dMin = dMean + _input.dUserRtMin * dSd;
					RemoveOutliers( _workingData, dMax, dMin );
					break;
				// By absolute mean
				case consts_h.OUTLIER_OPT_ABSOLUTE_CUTOFF:
					RemoveOutliers( _workingData, _input.dUserRtMax, _input.dUserRtMin );
					break;
				// Use everything
				default:
					//case consts_h.OUTLIER_OPT_MIXED_MODEL:
					//case consts_h.OUTLIER_OPT_NONE:
					break;
			}
			// TODO: Check how much was censored (length data v length workingdata)
			// TODO: Check censored per condition
		}
		/// <summary> Removes RT outliers from a dataset if the value is above the upper bound or 
		/// below the lower bound. </summary>
		/// <param name='dataset'> Dataset to process. </param>
		/// <param name='dUpper'> Upper limit for RT. </param>
		/// <param name='dLower'> Lower limit for RT. </param>
		private void RemoveOutliers ( ExptDataSet dataset, double dUpper, double dLower ) {
			int nProcessed = 0;
			int nTotalEntries = dataset.GetDataLength();
			int nIndex = 0;
			// Check each entry, remove any outside the specified boundaries.
			while ( nProcessed < nTotalEntries ) {
				if ( dataset.GetRT( nIndex ) >= dUpper || dataset.GetRT( nIndex ) <= dLower ) {
					dataset.RemoveRow( nIndex );
				} else {
					nIndex++;
				}
				nProcessed++;
			}
		}
		/// <summary> Splits working data set into separate event data sets.
		/// </summary>
		private void SplitEventData () {
			// Make sure event data doesn't exist yet.
			if ( _eventData == null ) {
				_eventData = new ExptDataSet[_nEvents];
			} else {
				Console.WriteLine( "DiffusionModel WARNING: SplitEventData(): event data array is already instantiated. Attempting to continue..." );
				_eventData = new ExptDataSet[_nEvents];
			}
			// Get data length, check for error.
			int nDataLength = _workingData.GetDataLength();
			if ( nDataLength == -1 ) {
				throw new InvalidOperationException( "DiffusionModel ERROR: SplitEventData(): invalid data length." );
			}
			// Initialize event data sets
			for ( int i = 0; i < _nEvents; i++ ) {
				_eventData[i] = new ExptDataSet();
			}
			// Loop through working data, sort events into _eventData[nEvents] array.
			for ( int iEvent = 0; iEvent < _nEvents; iEvent++ ) {
				for ( int i = 0; i < nDataLength; i++ ) {
					int nEventNum = _workingData.GetEvent( i );
					if ( nEventNum == iEvent ) {
						_eventData[iEvent].AddRow( _workingData.GetEvent( i ),
											  _workingData.GetResponse( i ),
											  _workingData.GetRT( i ) );
					}
				}
			}
		}
		#endregion

		#region INTERNAL FITTING METHODS
		/// <summary> Serves as the objective cost function for the optimization routine. Fits all 
		/// free parameters in the model for each condition/event. 
		///  
		/// Note: Instead of using hard linear constraints for the parameters, this will check for 
		/// errors or parameters that are out of space and return a large error. This sort of "soft" 
		/// constraint will force the optimizer to try new parameter values to minimize the error. </summary>
		/// <returns> Loss/cost value for model at these (input) parameters. </returns>
		/// <param name='dFreeParameters'> Values of the free parameters to fit in the model. </param>
		private double FitModel ( double[] dFreeParameters ) {
			double dLoss, x2;
			// Check parameters for infinities or NaNs
			for ( int i = 0; i < dFreeParameters.Length; i++ ) {
				if ( Double.IsInfinity( dFreeParameters[i] ) || Double.IsNaN( dFreeParameters[i] ) ) {
					Console.WriteLine( "Parameter {0} is infinity or NaN.", i );
					// Return large cost to force model to try new parameters.
					return 1e10d;
				}
			}
			// Get the full parameter matrix from the input free parameters.
			SetFullParamMatrix( dFreeParameters );
			// Check the current parameter set for validity.
			if ( !IsInParamSpace( _paramMatrix ) ) {
				// Return large cost to force model to try new parameters.
				return 1e10d;
			}
			// Fit each condition/event in the model
			dLoss = 0.0d;
			for ( int i = 0; i < _nEvents; i++ ) {
				x2 = FitModelEvent( i );
				// If we have a negative deviance for these parameters, return a large loss.
				if ( x2 < 0 ) {
					x2 = 1e10d;
				}
				// Accumulate loss across events.
				dLoss += x2;
			}
			// Loss shouldn't be NaN/null, negative, or infinity
			if ( dLoss < 0 || Double.IsInfinity( dLoss ) || Double.IsNaN( dLoss ) ) {
				Console.WriteLine( "Loss is NaN, negative, or infinity. Something went horribly wrong." );
				// Return large cost to force model to try new parameters.
				return 1e10d;
			}
			return dLoss;
		}
		/// <summary> Computes the deviance of the model for a specific parameter set for each 
		/// condition/event. Wrapped by FitModel(); </summary>
		/// <returns> Deviance of the model for current free parameters. </returns>
		/// <param name='nEvent'> Index of the event dataset. </param>
		private double FitModelEvent ( int nEvent ) {
			double dDeviance, dDevianceY, dDevianceN;
			// Find deviance of "No" Responses:
			if ( _eventData[nEvent].descriptives.nNontarget < consts_h.MIN_TRAILS_NEEDED ) {
				// If we don't have enough trials, we have no deviance.
				dDevianceN = 0d;
			} else {
				dDevianceN = FitAnswer( nEvent, 0 );
			}
			// Find deviance of "Yes" Responses
			if ( _eventData[nEvent].descriptives.nTarget < consts_h.MIN_TRAILS_NEEDED ) {
				// If we don't have enough trials, we have no deviance.
				dDevianceY = 0d;
			} else {
				dDevianceY = FitAnswer( nEvent, 1 );
			}
			// Calculate full model deviance.
			dDeviance = dDevianceY + dDevianceN;
			return dDeviance;
		}
		/// <summary> Computes the comparative fit index of the free parameters in the model for this response
		/// ("yes" or "no"). </summary>
		/// <returns> Comparative fit index (-2*logLikelihood). </returns>
		/// <param name='nEvent'> Index of the event/condition to fit. </param>
		/// <param name='nResp'> Response to fit (1 = yes, 0 = no). </param>
		unsafe private double FitAnswer ( int nEvent, int nResp ) {
			// Conditional probability of a nResp response.
			double px;
			// Cumulative Distribution Function of the quantiles for this event and response.
			double[] P = PrepareCDF( nEvent, nResp, &px );
			// Calculate conditional probability of specified argument response.
			px = 2 * nResp * px - nResp - px + 1;
			
			/*
			//TODO: Mixed model, if requested:
			if (_input.nModelType == consts_h.MODEL_TYPE_MIXED ){
				double olpi = _paramMatrix[consts_h.PARAM_PI];
				double gamm = _paramMatrix[consts_h.PARAM_GAMMA];
				//G = olpi*[P px] + (1-olpi)* [(Q-minMaxRt(1))/(minMaxRt(2)-MinMaxRt(1)) 1] * (gamm/2+(1-gamm)*px);
				//px = G(end);
				//P = G(1:end-1);
			}
			*/

			// Assemble array from CDF[] with bounds 0 to px:
			double[] pArray = new double[P.Length + 2];
			for ( int i = 0; i < pArray.Length; i++ ) {
				if ( i == 0 ) {
					// First element is 0.
					pArray[i] = 0d;
				} else if ( i == pArray.Length - 1 ) {
					// Last element is px.
					pArray[i] = px;
				} else {
					// Fill in middle elements from CDF[].
					pArray[i] = P[i - 1];
				}
			}
			// Calculate difference array (length of P-1; 1 per quantile)
			double[] pDiff = new double[pArray.Length - 1];
			pDiff = MathExtensions.Diff( pArray );
			// Get observed frequencies for each quantile
			int[] nObsFreq;
			if ( nResp == 1 ) {
				nObsFreq = _eventData[nEvent].descriptives.nBinObsFreqTarget;
			} else {
				nObsFreq = _eventData[nEvent].descriptives.nBinObsFreqNontarget;
			}
			// Calculate comparative fit index (chi-square or -2*LogLikelihood)
			double X2 = 0d;
			if ( _input.nEstMethodObjective == consts_h.EST_METHOD_OBJECTIVE_CHISQUARE ) {
				double[] E = new double[pDiff.Length];
				for ( int i = 0; i < pDiff.Length; i++ ) {
					E[i] = (double)_eventData[nEvent].descriptives.nTotalTrials * pDiff[i];
					if ( E[i] < 1e-5 ) {
						E[i] = 1e-5;
					}
				}
				double[] x = new double[pDiff.Length];
				for ( int i = 0; i < pDiff.Length; i++ ) {
					// ((ObservedFreq-E)^2) / E
					x[i] = ( ( (double)nObsFreq[i] - E[i] ) * ( (double)nObsFreq[i] - E[i] ) ) / E[i];
				}
				X2 = MathExtensions.Sum( x );
			} else {
				X2 = -2 * MathExtensions.LogLikelihood( pDiff, nObsFreq );
			}
			return X2;
		}
		/// <summary> Calculates the CDF and conditional probability for a condition in the model. </summary>
		/// <returns> CDF for each quantile for this event and response. </returns>
		/// <param name='nEvent'> Index of event to fit. </param>
		/// <param name='nResp'> Response for this distribution (1 = yes, 0 = no). </param>
		/// <param name='px'> Pointer for the conditional probability. </param>
		unsafe private double[] PrepareCDF ( int nEvent, int nResp, double* px ) {
			// Get the parameter values for this event from the full, redundant parameter matrix.
			double[] p = new double[_input.nParamCount];
			for ( int i = 0; i < _input.nParamCount; i++ ) {
				p[i] = _paramMatrix[nEvent, i];
			}
			// Yes or no flag.
			int x = nResp;
			// Quantiles (rts) of this condition's distribution.
			double[] t;
			// Quick value to check if a value is at least marginally above zero.
			double dNonzero = 1e-10d;
			// Number of quantiles.
			int nt;
			// Output: cdfs per quantile.
			double[] y;
			// Check for approximately "non-zero" parameter values. Keep them marginally above zero.
			if ( p[consts_h.PARAM_TER] < dNonzero ) {
				p[consts_h.PARAM_TER] = dNonzero;
			}
			if ( p[consts_h.PARAM_SZ] < dNonzero ) {
				p[consts_h.PARAM_SZ] = dNonzero;
			}
			if ( p[consts_h.PARAM_Z] < dNonzero ) {
				p[consts_h.PARAM_Z] = dNonzero;
			}
			// Get quantiles for this event and response.
			if ( nResp == 1 ) {
				t = _eventData[nEvent].descriptives.dQuantilesTarget;
			} else {
				t = _eventData[nEvent].descriptives.dQuantilesNontarget;
			}
			// Get number of quantiles.
			nt = t.Length;
			y = new double[nt];
			// Compute cdf for each quantile. (Note: px in this function should point back to local px.)
			for ( int i = 0; i < nt; i++ ) {
				y[i] = MathExtensions.CDF( t[i], x, p, px );
			}
			// Return the cdfs.
			return y;
		}
		#endregion

		#region INTERNAL OPTIMIZATION METHODS
		/// <summary> Runs the parameter optimization routine using the Nelder-Mead method.
		/// </summary>
		private void RunNelderMeadSimplex () {
			// Keep track of how many routines we've run already.
			int nSimplexRun = 0;
			// Initialize parameters.
			SimplexConstant[] parameters = new SimplexConstant[_freeParameters.Count];
			for ( int i = 0; i < _freeParameters.Count; i++ ) {
				parameters[i] = new SimplexConstant( _freeParameters[i], _input.dNmsInitPerturb );
			}
			// Point to the cost function.
			ObjectiveFunctionDelegate costFunction = new ObjectiveFunctionDelegate( FitModel );
			// Do a few short runs to prime the algorithm and avoid false positives.
			for ( int i = 0; i < _input.nShortNmsRuns; i++ ) {
				// Run simplex.
				RegressionResult result = NelderMeadSimplex.Regress( parameters, _input.dShortNmsTolerance,
												   _input.nShortNmsEvals, costFunction );
				_nmsResults.Add( result );
				// Update the free model parameters.
				SetFreeParams( _nmsResults[nSimplexRun] );
				// Update the starting constants.
				parameters = SetUpSimplexConstants();
				nSimplexRun++;
			}
			// Run series of longer nms runs to try to find the optimum.
			for ( int i = 0; i < _input.nLongNmsRuns; i++ ) {
				// Run simplex.
				RegressionResult result = NelderMeadSimplex.Regress( parameters, _input.dLongNmsTolerance,
																	 _input.nLongNmsEvals, costFunction );
				_nmsResults.Add( result );
				// Update the free model parameters.
				SetFreeParams( _nmsResults[nSimplexRun] );
				// Stop if NMS has converged.
				if ( _nmsResults[nSimplexRun].TerminationReason == TerminationReason.Converged ) {
#if DEBUG_LOG
					Console.WriteLine( "The simplex has converged--stopping the optimization routine..." );
#endif
					break;
				}
				// Update the simplex constants for next run
				parameters = SetUpSimplexConstants();
				if ( i != _input.nLongNmsRuns - 1 ) {
					nSimplexRun++;
				}
			}
#if DEBUG_LOG
			Console.WriteLine( " ** Free parameters from Regression Result** " );
			PrintRegressionResult( _nmsResults[nSimplexRun] );
			for ( int i = 0; i < _freeParameters.Count; i++ ) {
				Console.WriteLine( _freeParameters[i] );
			}
#endif
		}
		/// <summary> Sets up the free parameters for input to the optimization routine. Uses the local 
		/// freeParameters list and outputs an array of SimplexConstants. </summary>
		/// <returns> Free parameters as simplex constants. </returns>
		private SimplexConstant[] SetUpSimplexConstants () {
			SimplexConstant[] parameters = new SimplexConstant[_freeParameters.Count];
			for ( int i = 0; i < _freeParameters.Count; i++ ) {
				parameters[i] = new SimplexConstant( _freeParameters[i], _input.dNmsInitPerturb );
			}
			return parameters;
		}
#if DEBUG_LOG
		private void PrintRegressionResult ( RegressionResult result ) {
			for ( int i=0; i < result.Constants.Length; i++ ) {
				Console.WriteLine( "Regression Constant {0}: {1}", i, result.Constants[i] );
			}
			Console.WriteLine( "Termination Reason: {0}", result.TerminationReason.ToString() );
			Console.WriteLine( "Error Value: {0}", result.ErrorValue );
			Console.WriteLine( "Evaluation Count: {0}", result.EvaluationCount );
		}
#endif
		#endregion

		#region INTERNAL SET AND GET METHODS
		/// <summary> Sets the free parameter list from a regression result (from the N-M Simplex routine) </summary>
		/// <param name='res'> RegressionResult containing new free parameters (simplex constants). </param>
		private void SetFreeParams ( RegressionResult res ) {
			int nFreeParams = _freeParameters.Count;
			// Make sure we have the same number of free parameters as simplex constants.
			if ( res.Constants.Length != nFreeParams ) {
				throw new DataMisalignedException( "Free parameter count is not equal to the number of constants in the regression result" );
			}
			// Clear the current free parameters list
			_freeParameters.Clear();
			// Add each result constant as new free parameter value.
			for ( int i = 0; i < res.Constants.Length; i++ ) {
				_freeParameters.Add( res.Constants[i] );
			}
		}
		/// <summary> Sets up a list of free parameters based on values currently in the parameter matrix. </summary>
		private void SetFreeParamsFromFullMatrix () {
			_freeParameters.Clear();
			// Process restrictions
			for ( int i = 0; i < _input.nParamCount; i++ ) {
				// Get parameter design
				DesignMatrix dm = _input.designMatrix[i];
				if ( dm.GetMatrixType() == consts_h.MATRIX_TYPE_NO_EFFECTS ) {
					List<double> guesses = new List<double>();
					// Grab guess of each event.
					for ( int j = 0; j < _nEvents; j++ ) {
						guesses.Add( _paramMatrix[j, i] );
					}
					// Use the average across events as the guess.
					double guess = MathExtensions.Mean( guesses.ToArray() );
					// Add guess to free parameters list.
					_freeParameters.Add( guess );
					// Custom matrix type: some restrictions expected, but not as rigid as no effects.	
				} else if ( dm.GetMatrixType() == consts_h.MATRIX_TYPE_CUSTOM ) {
					throw new NotImplementedException( "Custom design matrices are not yet supported. Bug Josh to implement it." );
				} else {
					// Add guess for each event as a free parameter.
					for ( int j = 0; j < _nEvents; j++ ) {
						double guess = _paramMatrix[j, i];
						_freeParameters.Add( guess );
					}
				}
			}
#if DEBUG_LOG
			Console.WriteLine( "List of Free Parameters: " );
			for ( int i = 0; i < _freeParameters.Count; i++ ) {
				Console.WriteLine( _freeParameters[i] );
			}
			Console.WriteLine( "" );
#endif
		}
		/// <summary> Sets the full parameter matrix from a list of free parameters. </summary>
		/// <param name='dFreeParameters'> Double array of free parameters. </param>
		/// <exception cref='NotImplementedException'> Thrown when a feature is not yet implemented. </exception>
		private void SetFullParamMatrix ( double[] dFreeParameters ) {
			//TODO: Check for biases ( isnan(_input.biases) )
			bool bHasBias = false;
			List<double> freePars = new List<double>();
			// Set up a list containing free parameters from argument for easy element add/remove.
			for ( int i = 0; i < dFreeParameters.Length; i++ ) {
				freePars.Add( dFreeParameters[i] );
			}
			// Determine number of parameters to extract for each parameter and event.
			for ( int i = 0; i < _input.nParamCount; i++ ) {
				// TODO: Check for value fixes
				bool bHasFixedValues = false;
				// Get parameter design
				DesignMatrix dm = _input.designMatrix[i];
				// No effects matrix have one parameter value across all events.
				if ( dm.GetMatrixType() == consts_h.MATRIX_TYPE_NO_EFFECTS ) {
					// Get our next free parameter
					double dPar = freePars[0];
					// Remove this from list.
					freePars.RemoveAt( 0 );
					// Put this value in the matrix for each event.
					for ( int j = 0; j < _nEvents; j++ ) {
						_paramMatrix[j, i] = dPar;
					}
					// Custom matrices have special case number of parameters.
				} else if ( dm.GetMatrixType() == consts_h.MATRIX_TYPE_CUSTOM ) {
					throw new NotImplementedException( "Custom design matrices are not yet supported. Bug Josh to implement it." );
					//nParamToExtract = dm.GetConditionCount();
					//get nParamToExtract values from freeParams.
					//freePars.RemoveRange (0,nParamToExtract);
					// Process if we have fixed values.
				} else if ( bHasFixedValues ) {
					// TODO: implement fixed values (though this might be possible through DM class expansion)
					throw new NotImplementedException( "Fixed values not yet supported. Bug Josh to implement it." );
					// Process if we have biases (only for one parameter).
				} else if ( bHasBias && i == consts_h.PARAM_Z ) {
					//TODO: implement Z:A biases (DMs and fixed vals will take preference (one bias per condition/event)
					throw new NotImplementedException( "Bias not yet supported. Bug Josh to implement it." );
					// No restrictions (free parameter).
				} else {
					// Add one of the free parameters for each event.
					for ( int j = 0; j < _nEvents; j++ ) {
						_paramMatrix[j, i] = freePars[0];
						freePars.RemoveAt( 0 );
					}
				}
			}
		}
		/// <summary> Returns a default guess value for the specified parameter. Note: 
		/// the decided values here are arbitrary, but reasonable. </summary>
		/// <returns> Value of the default guess for specified parameter. </returns>
		/// <param name='nParam'> Index of parameter. </param>
		private double GetDefaultGuesses ( int nParam ) {
			if ( nParam == consts_h.PARAM_A ) {
				return 0.003d;
			} else if ( nParam == consts_h.PARAM_TER ) {
				return 0.001d;
			} else if ( nParam == consts_h.PARAM_Z ) {
				return 0.0015d;
			} else {
				return 0.0d;
			}
		}
		/// <summary> Determines whether the value of the specified parameter for specified event is in 
		/// a reasonable parameter space. </summary>
		/// <returns> <c>true</c> if parameter value for specified event is in reasonable space; otherwise, <c>false</c>. </returns>
		/// <param name='nParam'> Index of the parameter. </param>
		/// <param name='nEvent'> Index of the event. </param>
		private bool IsInParamSpace ( int nParam, int nEvent ) {
			// Get current parameter value.
			double dVal = _eventData[nEvent].parameters.param[nParam].GetParamValue();
			switch ( nParam ) {
				case consts_h.PARAM_A:
					// boundary minimum is at least 0.001 over the starting point value.
					double dMinA = _eventData[nEvent].parameters.param[consts_h.PARAM_Z].GetParamValue() + 0.001d;
					if ( dVal < dMinA || dVal > consts_h.SPACE_MAX_A ) {
						return false;
					} break;
				case consts_h.PARAM_TER:
					if ( dVal < consts_h.SPACE_MIN_TER || dVal > consts_h.SPACE_MAX_TER ) {
						return false;
					} break;
				case consts_h.PARAM_ETA:
					if ( dVal < consts_h.SPACE_MIN_ETA || dVal > consts_h.SPACE_MAX_ETA ) {
						return false;
					} break;
				case consts_h.PARAM_Z:
					// starting point maximum is at least 0.001 under the boundary.
					double dMaxZ = _eventData[nEvent].parameters.param[consts_h.PARAM_A].GetParamValue() - 0.001d;
					if ( dVal < consts_h.SPACE_MIN_Z || dVal > dMaxZ ) {
						return false;
					} break;
				case consts_h.PARAM_SZ:
					// SD of Z must be less than twice itself (-0.001 pad) or less than  (Z-a)
					double dMaxSZ = _eventData[nEvent].parameters.param[consts_h.PARAM_Z].GetParamValue();
					double dTemp = _eventData[nEvent].parameters.param[consts_h.PARAM_A].GetParamValue() - dMaxSZ;
					dMaxSZ = System.Math.Min( dMaxSZ, dTemp );
					dMaxSZ = 2 * dMaxSZ - 0.001d;
					if ( dVal < consts_h.SPACE_MIN_SZ || dVal > dMaxSZ ) {
						return false;
					} break;
				case consts_h.PARAM_ST:
					// SD of Ter must be less than twice itself (-0.001 pad).
					double dMaxST = _eventData[nEvent].parameters.param[consts_h.PARAM_TER].GetParamValue();
					dMaxST = 2 * dMaxST - 0.001d;
					if ( dVal < consts_h.SPACE_MIN_ST || dVal > dMaxST ) {
						return false;
					} break;
				case consts_h.PARAM_NU:
					if ( dVal < consts_h.SPACE_MIN_V || dVal > consts_h.SPACE_MAX_V ) {
						return false;
					} break;
				default:
					throw new IndexOutOfRangeException( "Invalid Parameter index" );
			}
			return true;
		}
		/// <summary> Determines whether the value of the specified parameter for specified event is in 
		/// a reasonable parameter space. </summary>
		/// <returns> <c>true</c> if all parameters are in reasonable space; otherwise, <c>false</c>. </returns>
		/// <param name='dParamMatrix'> Full parameter matrix to assess. </param>
		private bool IsInParamSpace ( double[,] dParamMatrix ) {
			for ( int i = 0; i < _input.nParamCount; i++ ) {
				for ( int j = 0; j < _nEvents; j++ ) {
					double dVal = dParamMatrix[j, i];
					switch ( i ) {
						case consts_h.PARAM_A:
							double dMinA = dParamMatrix[j, consts_h.PARAM_Z] + 0.001d;
							if ( dVal < dMinA || dVal > consts_h.SPACE_MAX_A ) {
								return false;
							} break;
						case consts_h.PARAM_TER:
							if ( dVal < consts_h.SPACE_MIN_TER || dVal > consts_h.SPACE_MAX_TER ) {
								return false;
							} break;
						case consts_h.PARAM_ETA:
							if ( dVal < consts_h.SPACE_MIN_ETA || dVal > consts_h.SPACE_MAX_ETA ) {
								return false;
							} break;
						case consts_h.PARAM_Z:
							double dMaxZ = dParamMatrix[j, consts_h.PARAM_A] - 0.001d;
							if ( dVal < consts_h.SPACE_MIN_Z || dVal > dMaxZ ) {
								return false;
							} break;
						case consts_h.PARAM_SZ:
							double dMaxSZ = dParamMatrix[j, consts_h.PARAM_Z];
							double dTemp = dParamMatrix[j, consts_h.PARAM_A] - dMaxSZ;
							dMaxSZ = Math.Min( dMaxSZ, dTemp );
							dMaxSZ = 2 * dMaxSZ - 0.001d;
							if ( dVal < consts_h.SPACE_MIN_SZ || dVal > dMaxSZ ) {
								return false;
							} break;
						case consts_h.PARAM_ST:
							double dMaxST = dParamMatrix[j, consts_h.PARAM_TER];
							dMaxST = 2 * dMaxST - 0.001d;
							if ( dVal < consts_h.SPACE_MIN_ST || dVal > dMaxST ) {
								return false;
							} break;
						case consts_h.PARAM_NU:
							if ( dVal < consts_h.SPACE_MIN_V || dVal > consts_h.SPACE_MAX_V ) {
								return false;
							} break;
						default:
							Console.WriteLine( "Invalid Parameter..." );
							break;
					}
				}
			}
			return true;
		}
		/// <summary> Checks the current parameter matrix for values near parameter space bounds. Returns 
		/// <c>true</c> if parameters seems reasonable; otherwise <c>false</c>. If <c>false</c>, it is 
		/// recommended to rerun the optimization routine to try to get a different minimum (i.e., maybe it 
		/// got caught in a local minimum). </summary>
		/// <returns> <c>True</c> if parameters seem reasonable; otherwise, <c>false</c>. </returns>
		private bool IsGoodSolution () {
			bool isEveryParameterGood = true;
			for ( int i = 0; i < _nEvents; i++ ) {
				if ( _paramMatrix[i, consts_h.PARAM_TER] < 0.02d ) {
					isEveryParameterGood = false;
					Console.WriteLine( "Parameter TER is suspect..." );
					_paramMatrix[i, consts_h.PARAM_TER] = 0.95d * _paramMatrix[i, consts_h.PARAM_TER];
				}
				if ( _paramMatrix[i, consts_h.PARAM_ETA] < 0.0005d ) {
					isEveryParameterGood = false;
					Console.WriteLine( "Parameter Eta is suspect..." );
					_paramMatrix[i, consts_h.PARAM_ETA] = 0.2d;
				}
			}
			if ( !isEveryParameterGood ) {
				SetFreeParamsFromFullMatrix();
			}
			return isEveryParameterGood;
		}
		//TODO:
		private void GetFinalFit () {
			double dFinalFit = FitModel( _freeParameters.ToArray() );
			// LogLikelihoodModel: -2*log-likelihood of the data, given the model.
			// LogLikelihoodData: -2*log-likelihood of the data, given the saturated model.
			// LogLikelihoodRatio: The log-likelihood ratio statistic G2.
			// ChiSquare: The Chi-Square statistic X2.
			// AIC: The Akaike Information Criterion.
			// AICc: The Small-Sample Akaike Information Criterion.
			// BIC: The Bayesian Information Criterion.
		}
		#endregion

		#region INTERNAL ESTIMATION METHODS
		/// <summary> Processes each parameter's design matrix, restrictions, biases, etc. and forms the free parameter list. </summary>
		/// <exception cref='NotImplementedException'> Thrown when a certain feature is not yet implemented. </exception>
		private void InitFreeParamsFromGuesses () {
			// Check for user biases ( isnan(_input.bHasBias) )
			bool bHasBias = false;
			// Process restrictions
			for ( int i = 0; i < _input.nParamCount; i++ ) {
				// Check for fixed values
				bool bHasFixedValues = false;
				// Get parameter design
				DesignMatrix dm = _input.designMatrix[i];
				// No effects parameter: one value across all conditions.
				if ( dm.GetMatrixType() == consts_h.MATRIX_TYPE_NO_EFFECTS ) {
					List<double> guesses = new List<double>();
					// Grab guess of each event.
					for ( int j = 0; j < _nEvents; j++ ) {
						guesses.Add( _eventData[j].parameters.param[i].GetParamValue() );
					}
					// Use the average across events as the guess.
					double guess = MathExtensions.Mean( guesses.ToArray() );
					// Add guess to free parameters list.
					_freeParameters.Add( guess );
				// Custom matrix type: some restrictions expected, but not as rigid as no effects.	
				} else if ( dm.GetMatrixType() == consts_h.MATRIX_TYPE_CUSTOM ) {
					throw new NotImplementedException( "Custom design matrices are not yet supported. Bug Josh to implement it." );
					//slightly more complicated than the above no effects condition
				// Fixed Values:
				} else if ( bHasFixedValues ) {
					//TODO: implement fixed values: though couldn't this be implied from use of DMs?
					throw new NotImplementedException( "Fixed values not yet supported. Bug Josh to implement it." );
					// check that fixVals length for param is equal to condition number.
					// get param design at i
					// add free param
					// if param==4 (z), bias=null;
				// Boundary distance biases effect one parameter only:
				} else if ( bHasBias && i == consts_h.PARAM_Z ) {
					// TODO: implement Z:A biases. (DMs and fixed vals take preference here, though). (one Z:a bias per condition).
					throw new NotImplementedException( "Bias not yet supported. Bug Josh to implement it." );
					// Check that bias.length == nevents
					// Check that biases comply with DM.
					// param matrix = (bias,4);
					// add free param
				// No restrictions (free parameter):
				} else {
					// Add guess for each event as a free parameter.
					for ( int j = 0; j < _nEvents; j++ ) {
						double guess = _eventData[j].parameters.param[i].GetParamValue();
						_freeParameters.Add( guess );
					}
				}
			}
#if DEBUG_LOG
			Console.WriteLine( "List of Free Parameters: " );
			for ( int i = 0; i < _freeParameters.Count; i++ ) {
				Console.WriteLine( _freeParameters[i] );
			}
			Console.WriteLine( "" );
#endif
		}
		/// <summary> Sets up starting parameter values for the model. Uses either user-input guesses 
		/// or generates guesses using the Wagenmakers et al. algorithm. </summary>
		/// <param name='nGuessMethod'> Integer guess method option. </param>
		/// <exception cref='ArgumentException'> Thrown when the option argument is invalid. </exception>
		/// <exception cref='InvalidOperationException'> Thrown when an invalid parameter number is detected. </exception>
		private void GuessParameters ( int nGuessMethod ) {
			// Compute guesses
			if ( nGuessMethod == consts_h.GUESS_METHOD_COMPUTE ) {
				for ( int i = 0; i < _nEvents; i++ ) {
					// Set up parameters for this event
					_eventData[i].SetUpParameters( _input );
					// Compute guesses with Wagenmakers algorithm.
					EstimateDiffusionParameters( i );
					// If nondecision time (Ter) seems unreasonable, set a default guess value
					if ( _eventData[i].parameters.param[consts_h.PARAM_TER].GetParamValue() <= 0 ) {
						_eventData[i].parameters.param[consts_h.PARAM_TER].SetParamValue( 0.3d );
					}
				}
			// User specified guesses:
			} else if ( nGuessMethod == consts_h.GUESS_METHOD_USER ) {
				for ( int iEvent = 0; iEvent < _nEvents; iEvent++ ) {
					_eventData[iEvent].SetUpParameters( _input );
					// Grab each guess for each condition and parameter
					for ( int iParam = 0; iParam < _input.dUserParamGuess.Length; iParam++ ) {
						_eventData[iEvent].parameters.param[iParam].SetParamValue( _input.dUserParamGuess[iParam] );
					}
				}
			} else {
				throw new ArgumentException( "Specified guess method for GuessParameters() is invalid" );
			}
			// TODO: If mixed model (9 parameters), guess for pi and lambda
			if ( _input.nParamCount == 9 ) {
				for ( int i = 0; i < _nEvents; i++ ) {
					_eventData[i].parameters.param[consts_h.PARAM_GAMMA].SetParamValue( 0.97d );
					_eventData[i].parameters.param[consts_h.PARAM_PI].SetParamValue( 0.97d );
				}
			}
			// Check that everything is in appropriate parameter space
			for ( int iEvent = 0; iEvent < _nEvents; iEvent++ ) {
				// Make sure we have an appropriate number of parameters.
				if ( _eventData[iEvent].parameters.param.Length != 7 && _eventData[iEvent].parameters.param.Length != 9 ) {
					throw new InvalidOperationException( "Bad number of parameters for event " + iEvent );
				}
				for ( int iParam = 0; iParam < _eventData[iEvent].parameters.param.Length; iParam++ ) {
					// Get current parameter value.
					double dParamVal = _eventData[iEvent].parameters.param[iParam].GetParamValue();
					// If value is outside of parameter space, try to fix it.
					if ( !IsInParamSpace( iParam, iEvent ) ) {
						bool bGood = false;
						Console.WriteLine( "Guess is not in parameter space. Trying to fix it..." );
						// Try 50 iterations to get the parameter into reasonable space.
						for ( int j = 0; j < 50; j++ ) {
							// Try halving the parameter, check again.s
							dParamVal /= 2;
							bGood = IsInParamSpace( iParam, iEvent );
							// If value is back in bounds, set and break.
							if ( bGood ) {
								_eventData[iEvent].parameters.param[iParam].SetParamValue( dParamVal );
								break;
							}
						}
						// If we're still not in param space, assign a default.
						if ( !bGood ) {
							Console.WriteLine( "WARNING: parameter is still not in parameter space. Assigning a default guess." );
							dParamVal = GetDefaultGuesses( iParam );
							_eventData[iEvent].parameters.param[iParam].SetParamValue( dParamVal );
						}
					}
				}
			}
		}
		/// <summary> Uses the Wagenmakers et al. diffusion algorithm to get an estimate of 
		/// the parameter values so that we have a reasonable starting point for model 
		/// optimization. 
		///  
		/// Source: Wagenmakers, E.-J., van der Maas, H., & Grasman, R. (2007). An 
		/// EZ-diffusion model for response time and accuracy. Psychonomic 
		/// Bulletin & Review. </summary>
		/// <param name='nEvent'> Event number of data set to use. </param>
		/// <exception cref='InvalidOperationException'> Is thrown when the data set contains too few trials to analyze. </exception>
		private void EstimateDiffusionParameters ( int nEvent ) {
			double dVRt, dPC, dMrt;
			int n, nVSign;
			// Set up local variables for either correct trials or incorrect trials
			if ( _eventData[nEvent].descriptives.nTarget > 2 ) {
				// RT variance for correct trials
				dVRt = _eventData[nEvent].descriptives.dRtVarTarget;
				// Proportion of correct trials to total trials
				dPC = _eventData[nEvent].descriptives.fTargetProportion;
				// Mean RT for correct trials
				dMrt = _eventData[nEvent].descriptives.dRtMeanTarget;
				// Number of trials we're dealing with
				n = _eventData[nEvent].descriptives.nTarget;
				// Sign for drift parameter (negative if dealing with incorrects)
				nVSign = 1;
			} else {
				// RT variance for incorrect trials
				dVRt = _eventData[nEvent].descriptives.dRtVarNontarget;
				// Proportion of incorrect trials to total trials
				dPC = 1d - _eventData[nEvent].descriptives.fTargetProportion;
				// Mean RT for incorrect trials
				dMrt = _eventData[nEvent].descriptives.dRtMeanNontarget;
				// Number of trials we're dealing with
				n = _eventData[nEvent].descriptives.nNontarget;
				// Sign for drift parameter (negative if dealing with incorrects)
				nVSign = -1;
			}
			// If we don't have enough trials for either operation, quit.
			if ( _eventData[nEvent].descriptives.nTarget <= 2 && _eventData[nEvent].descriptives.nNontarget <= 2 ) {
				throw new InvalidOperationException( "EstimateDiffusionParameters ERROR: Correct and Incorrect data have too few trials to analyze." );
			}
			// Adjust values for special cases that will break the algorithm.
			if ( dPC == 0.0d ) {
				dPC = 0.0d + 1 / n;
			} else if ( dPC == 0.5d ) {
				dPC = 0.5d + 1 / n;
			} else if ( dPC == 1.0d ) {
				dPC = 1.0d - 1 / n;
			}
			if ( dVRt == 0.0d ) {
				dVRt = 1e-9d;
			}
			// Run through the algorithm:
			double dS = 0.1d;
			double dS2 = dS * dS;

			double dLogit = MathExtensions.Logit( dPC );

			double dX = dLogit * ( dLogit * ( dPC * dPC ) - dLogit * dPC + dPC - 0.5d ) / dVRt;
			// Drift Rate, V
			double dV = Math.Sign( dPC - 0.5d ) * dS * Math.Pow( dX, 0.25d );
			// Boundary Separation, a
			double dA = dS2 * dLogit / dV;

			double dY = -dV * dA / dS2;
			// Mean decision time
			double dMdt = ( dA / ( 2d * dV ) ) * ( 1d - Math.Exp( dY ) ) / ( 1d + Math.Exp( dY ) );
			// Non-decision time, Ter
			double dTer = dMrt - dMdt;

			dV = dV * nVSign;

			// Set parameter values as our guesses.
			_eventData[nEvent].parameters.param[consts_h.PARAM_NU].SetParamValue( dV );
			_eventData[nEvent].parameters.param[consts_h.PARAM_A].SetParamValue( dA );
			_eventData[nEvent].parameters.param[consts_h.PARAM_TER].SetParamValue( dTer );
			// Set other parameters as defaults.
			_eventData[nEvent].parameters.param[consts_h.PARAM_ETA].SetParamValue( 0.2d );
			_eventData[nEvent].parameters.param[consts_h.PARAM_Z].SetParamValue( dA * 0.5d );
			_eventData[nEvent].parameters.param[consts_h.PARAM_SZ].SetParamValue( 0.45d * dA );
			_eventData[nEvent].parameters.param[consts_h.PARAM_ST].SetParamValue( 0.9d * dTer );
		}
		#endregion

	}
}