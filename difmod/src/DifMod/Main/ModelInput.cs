/// <summary>
/// ModelInput.cs
/// 
/// Version 1.0
/// Created by: Josh Tremel (tremeljosh@gmail.com)
/// Created on: January 26, 2012
/// 
/// Contains user options for a new model instance.
/// </summary>
namespace DifMod {
	/// <summary> Model options </summary>
	public class ModelInput {
		/// <summary> Full path to input file. </summary>
		public string sFilePath;
		/// <summary> Option for how we treat any outliers. </summary>
		public int nOutlierTreatment = consts_h.OUTLIER_OPT_NONE;
		/// <summary> Type of model. </summary>
		public int nModelType = consts_h.MODEL_TYPE_STANDARD;
		/// <summary> Number of parameters in the model. </summary>
		public int nParamCount = consts_h.PARAM_COUNT_STANDARD_MODEL;
		/// <summary> Design matrix for each parameter. This controls whether this parameter is free or fixed. </summary>
		public DesignMatrix[] designMatrix = new DesignMatrix[consts_h.PARAM_COUNT_STANDARD_MODEL];
		/// <summary> Maximum allowable RT in data (for outlier filtering). </summary>
		public double dUserRtMax = 0d;
		/// <summary> Minimum allowable RT in data (for outlier filtering). </summary>
		public double dUserRtMin = 0d;
		/// <summary> Method of assessing the quality of fit (Chi-square or Multinomial Likelihood. </summary>
		public int nEstMethodObjective = consts_h.EST_METHOD_OBJECTIVE_ML;
		/// <summary> Method for estimating the quantiles (calculate from percentiles, user-defined quantiles, etc.). </summary>
		public int nEstMethodBins = consts_h.EST_METHOD_BINS_USER_PERCENT;
		/// <summary> Quantiles for "YES" responses (target items). </summary>
		public double[] dUserQuantilesCor = new double[] { 0.30d, 0.36d, 0.42d, 0.52d, 0.80d };
		/// <summary> Quantiles for "NO" responses (non-target items). </summary>
		public double[] dUserQuantilesInc = new double[] { 0.38d, 0.47d, 0.56d, 0.70d, 1.00d };
		/// <summary> Optional percentiles for "YES" (target) responses. Quantiles will be calculated. </summary>
		public double[] dUserPercentilesCor = new double[] { 10, 30, 50, 70, 90 };
		/// <summary> Optional percentiles for "NO" (non-target) responses. Quantiles will be calculated. </summary>
		public double[] dUserPercentilesInc = new double[] { 10, 30, 50, 70, 90 };
		/// <summary> Option for initial parameter estimate (Compute or user-defined). </summary>
		public int nParameterGuessMethod = consts_h.GUESS_METHOD_COMPUTE;
		/// <summary> Parameter guess values defined by the user. </summary>
		public double[] dUserParamGuess = new double[7];
		//TODO: Bias
		//TODO: Fix bound values
		#region OPTIMIZATION OPTIONS
		/// <summary> Tolerance for longer Nelder-Mead Simplex routines. </summary>
		public double dLongNmsTolerance = 1e-7;
		/// <summary> Number of evaluations for the longer Nelder-Mead Simplex routines. </summary>
		public int nLongNmsEvals = 5000;
		/// <summary> Number of longer Nelder-Mead Simplex routines to run. </summary>
		public int nLongNmsRuns = 1;
		/// <summary> Number of initial shorter Nelder-Mead Simplex routines to run. </summary>
		public int nShortNmsRuns = 3;
		/// <summary> Number of evaluations for the intial shorter Nelder-Mead Simplex routines. </summary>
		public int nShortNmsEvals = 250;
		/// <summary> Tolerance for the initial short Nelder-Mead Simplex routines. </summary>
		public double dShortNmsTolerance = 1e-7;
		/// <summary> Value for the initial perturbation for the optimization routine. </summary>
		public double dNmsInitPerturb = 0.0001d;
		/// <summary> Number of retries if the parameters seem out of space or unrealistic. </summary>
		public int nRetry = 3;
		#endregion
	}
}