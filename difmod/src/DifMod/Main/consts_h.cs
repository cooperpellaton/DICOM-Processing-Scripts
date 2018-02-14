/// <summary>
/// consts_h.cs
/// 
/// Version 1.0
/// Created by: Josh Tremel (tremeljosh@gmail.com)
/// Created on: January 26, 2012
/// 
/// Main "header" file for the diffusion model with useful constants...
/// </summary>

namespace DifMod {
	public static class consts_h {
		#region Model Types
		/// <summary> Standard Ratcliff diffusion model. </summary>
		public const int MODEL_TYPE_STANDARD	= 0;
		/// <summary> Mixed model approach. </summary>
		public const int MODEL_TYPE_MIXED		= 1;
		/// <summary> Number of parameters for the standard diffusion model. </summary>
		public const int PARAM_COUNT_STANDARD_MODEL	= 7;
		/// <summary> Number of parameters for the mixed model. </summary>
		public const int PARAM_COUNT_MIXED_MODEL	= 9;
		#endregion
		#region Parameters
		/// <summary> Boundary separation (a). </summary>
		public const int PARAM_A				= 0;
		/// <summary> Non-decision processes time (Ter). </summary>
		public const int PARAM_TER				= 1;
		/// <summary> Variance in drift rate (eta). </summary>
		public const int PARAM_ETA				= 2;
		/// <summary> Starting position (z). </summary>
		public const int PARAM_Z				= 3;
		/// <summary> Variance of starting position (Sz). </summary>
		public const int PARAM_SZ				= 4;
		/// <summary> Variance of non-decision process time (STer || ST). </summary>
		public const int PARAM_ST				= 5;
		/// <summary> Mean drift rate (v || nu). </summary>
		public const int PARAM_NU				= 6;
		/// <summary> Parameter pi for mixed model approach. </summary>
		public const int PARAM_PI				= 7;
		/// <summary> Parameter gamma for mixed model approach. </summary>
		public const int PARAM_GAMMA			= 8;
		#endregion
		#region Parameter Space
		/// <summary> Minimum number of trials needed for reasonably accurate calculations. </summary>
		public const int MIN_TRAILS_NEEDED = 11;

		/// <summary> Maximum possible value for boundary separation. </summary>
		public const double SPACE_MAX_A		= 2.0d;
		/// <summary> Maximum possible time for non-decision processes. (Potentially infinite, but let's just make it 30 s.) </summary>
		public const double SPACE_MAX_TER	= 30d;
		/// <summary> Minimum possible time for non-decision processes. </summary>
		public const double SPACE_MIN_TER	= 0d;
		/// <summary> Maximum possible value for drift variance. </summary>
		public const double SPACE_MAX_ETA	= 0.5d;
		/// <summary> Minimum possible value for drift variance. </summary>
		public const double SPACE_MIN_ETA	= 0d;
		/// <summary> Mininum possible value for starting point Z. </summary>
		public const double SPACE_MIN_Z		= 0d;
		/// <summary> Minimum possible value for variance in starting point. </summary>
		public const double SPACE_MIN_SZ	= 0d;
		/// <summary> Minimum possiblevalue for variance in non-decision time. </summary>
		public const double SPACE_MIN_ST	= 0d;
		/// <summary> Maximum possible value of drift rate. </summary>
		public const double SPACE_MAX_V		= 5.0d;
		/// <summary> Minimum possible value for drift rate. </summary>
		public const double SPACE_MIN_V		= -5.0d;
		#endregion
		#region Design Matrix
		/// <summary> Setting for default matrix type (see below constants); </summary>
		public const int DEFAULT_MATRIX_TYPE			= 2;
		/// <summary> Invalid design matrix. </summary>
		public const int MATRIX_TYPE_INVALID			= 0;
		/// <summary> Identity design matrix. </summary>
		public const int MATRIX_TYPE_IDENTITY			= 1;
		/// <summary> No effects design matrix. </summary>
		public const int MATRIX_TYPE_NO_EFFECTS			= 2;
		/// <summary> Custom design matrix. </summary>
		public const int MATRIX_TYPE_CUSTOM				= 3;
		#endregion
		#region Initial Guess Method
		/// <summary> Computes initial parameter guesses. </summary>
		public const int GUESS_METHOD_COMPUTE			= 0;
		/// <summary> User supplies initial parameter guesses. </summary>
		public const int GUESS_METHOD_USER				= 1;
		#endregion
		#region Outlier Treatment
		/// <summary> Default outlier treatment (see below constants) </summary>
		public const int DEFAULT_OUTLIER_OPT			= 0;
		/// <summary> No outlier removal. </summary>
		public const int OUTLIER_OPT_NONE				= 0;
		/// <summary> Use relative standard deviation cutoff for outliers. </summary>
		public const int OUTLIER_OPT_RELATIVE_CUTOFF 	= 1;
		/// <summary> Use an absolute RT cutoff (in seconds) for outliers. </summary>
		public const int OUTLIER_OPT_ABSOLUTE_CUTOFF	= 2;
		/// <summary> Treat outliers according to the mixed model. </summary>
		public const int OUTLIER_OPT_MIXED_MODEL		= 3;
		#endregion
		#region Fit Assessment
		/// <summary> Chi-square for non-normal distributions. </summary>
		public const int EST_METHOD_OBJECTIVE_CHISQUARE 	= 0;
		/// <summary> Multinomial likelihood (-2*loglikelihood). </summary>
		public const int EST_METHOD_OBJECTIVE_ML			= 1;
		#endregion
		#region Quantile Calculation
		/// <summary> Use default quantile calculations from predetermined percentiles. </summary>
		public const int EST_METHOD_BINS_DEFAULT			= 0;
		/// <summary> User-specified quantiles. </summary>
		public const int EST_METHOD_BINS_USER_QUANTILE		= 1;
		/// <summary> User-specified percentiles. </summary>
		public const int EST_METHOD_BINS_USER_PERCENT		= 2;
		/// <summary> Quantiles for "YES" responses (target items). </summary>
		public static double[] dUserQuantilesCor = new double[] {
			0.30d, 0.36d, 0.42d, 0.52d, 0.80d };
		/// <summary> Quantiles for "NO" responses (non-target items). </summary>
		public static double[] dUserQuantilesInc = new double[] {
			0.38d, 0.47d, 0.56d, 0.70d, 1.00d };
		/// <summary> Optional percentiles for "YES" (target) responses. Quantiles will be calculated. </summary>
		public static double[] dUserPercentilesCor = new double[] {
			10, 30, 50, 70, 90 };
		/// <summary> Optional percentiles for "NO" (non-target) responses. Quantiles will be calculated. </summary>
		public static double[] dUserPercentilesInc = new double[] {
			10, 30, 50, 70, 90 };
		#endregion
		#region OPTIMIZATION DEFAULTS
		/// <summary> Tolerance for long optimization runs. </summary>
		public const double NMS_LONG_TOLERANCE_DEFAULT = 1e-7d;
		/// <summary> Maximum function evaluations for long optimization runs. </summary>
		public const int NMS_LONG_EVALS_DEFAULT = 5000;
		/// <summary> Number of long optimization runs. </summary>
		public const int NMS_LONG_RUNS_DEFAULT = 1;
		/// <summary> Number of short optimization runs. </summary>
		public const int NMS_SHORT_RUNS_DEFAULT = 3;
		/// <summary> Maximum function evaluations for short runs. </summary>
		public const int NMS_SHORT_EVALS_DEFAULT = 250;
		/// <summary> Tolerance for short optimization runs. </summary>
		public const double NMS_SHORT_TOLERANCE_DEFAULT = 1e-7d;
		/// <summary> Initial value perturbation amount for optimization routine. </summary>
		public const double NMS_INITIAL_PERTURB_DEFAULT = 0.0001d;
		#endregion
	}
}