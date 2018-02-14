/// <summary>
/// MathExtensions.cs
/// 
/// Version 1.0
/// Created by: Josh Tremel (tremeljosh@gmail.com)
/// Created on: January 26, 2012
/// 
/// Math extensions for descriptive stats and distribution calculations.
/// </summary>
using System;
using System.Collections.Generic;

namespace DifMod {
	public static class MathExtensions {

		#region VARIANCE
		/// <summary>
		/// Calculates the variance of an array of integers.
		/// </summary>
		/// <param name='array'> Integer array. </param>
		public static double Variance ( int[] array ) {
			double variance = 0;
			double delta = 0;
			double mean = Mean( array );
			for ( int i = 0; i < array.Length; i++ ) {
				delta = array[i] - mean;
				variance += ( delta * delta - variance ) / ( i + 1 );
			}
			return variance;
		}
		/// <summary>
		/// Calculates the variance of an array of floats.
		/// </summary>
		/// <param name='array'> Float array. </param>
		public static double Variance ( float[] array ) {
			double variance = 0;
			double delta = 0;
			double mean = Mean( array );
			for ( int i = 0; i < array.Length; i++ ) {
				delta = array[i] - mean;
				variance += ( delta * delta - variance ) / ( i + 1 );
			}
			return variance;
		}
		/// <summary>
		/// Calculates the variance of an array of doubles.
		/// </summary>
		/// <param name='array'> Double array. </param>
		public static double Variance ( double[] array ) {
			double variance = 0;
			double delta = 0;
			double mean = Mean( array );
			for ( int i = 0; i < array.Length; i++ ) {
				delta = array[i] - mean;
				variance += ( delta * delta - variance ) / ( i + 1 );
			}
			return variance;
		}
		#endregion

		#region SUM
		/// <summary>
		/// Calculates the sum of an array of integers.
		/// </summary>
		/// <param name='iArray'> Integer array. </param>
		public static int Sum ( int[] iArray ) {
			int sum = 0;
			for ( int i = 0; i < iArray.Length; i++ ) {
				sum += iArray[i];
			}
			return sum;
		}
		/// <summary>
		/// Calculates the sum of an array of floats.
		/// </summary>
		/// <param name='fArray'> Float array. </param>
		public static float Sum ( float[] fArray ) {
			float sum = 0;
			for ( int i = 0; i < fArray.Length; i++ ) {
				sum += fArray[i];
			}
			return sum;
		}
		/// <summary>
		/// Calculates the sum of an array of doubles.
		/// </summary>
		/// <param name='dArray'> Double array. </param>
		public static double Sum ( double[] dArray ) {
			double sum = 0;
			for ( int i = 0; i < dArray.Length; i++ ) {
				sum += dArray[i];
			}
			return sum;
		}
		#endregion

		#region MEAN
		/// <summary>
		/// Averages an array of integers.
		/// </summary>
		/// <param name='array'> Integer array. </param>
		public static float Mean ( int[] array ) {
			return Sum( array ) / array.Length;
		}
		/// <summary>
		/// Averages an array of floats.
		/// </summary>
		/// <param name='fArray'> Float array. </param>
		public static float Mean ( float[] fArray ) {
			return Sum( fArray ) / fArray.Length;
		}
		/// <summary>
		/// Averages an array of doubles.
		/// </summary>
		/// <param name='dArray'> Double array. </param>
		public static double Mean ( double[] dArray ) {
			return Sum( dArray ) / dArray.Length;
		}
		#endregion

		/// <summary> Calculates the logit of a value (inverse logistic function) </summary>
		/// <param name='dX'> X value for function. </param>
		public static double Logit ( double dX ) {
			return Math.Log( dX / ( 1 - dX ) );
		}

		/// <summary> Returns an array of distances between each array element (automatically puts 
		/// in a 0 for the first value). Returned array will be of size InputArray.Length - 1. </summary>
		/// <param name='dArray'> Array of bounding probabilites to get the difference of. </param>
		public static double[] Diff ( double[] dArray ) {
			List<double> diffs = new List<double>();

			double dif;

			//start at 1, use current-previous
			for ( int i = 1; i < dArray.Length; i++ ) {
				dif = dArray[i] - dArray[i - 1];
				diffs.Add( dif );
			}

			if ( diffs.Count != dArray.Length - 1 ) {
				Console.WriteLine( "WARNING: Difference array is not exactly one value shorter than input array..." );
			}

			return diffs.ToArray();
		}

		/// <summary> Returns the log-likelihood of input quantile statistics. </summary>
		/// <returns> The log-likelihood</returns>
		/// <param name='dArray'> Array of probabilities for the quantiles. </param>
		/// <param name='dObsFreq'> Array of observed frequencies for the quantiles. </param>
		public static double LogLikelihood ( double[] dArray, int[] dObsFreq ) {
			// any value less than 1e-5, change to 1e-5:
			for ( int i = 0; i < dArray.Length; i++ ) {
				if ( dArray[i] < 1e-5d ) {
					dArray[i] = 1e-5d;
				}
			}

			// Natural log of each value times the quantile's observed frequency
			double[] dLogs = new double[dArray.Length];
			for ( int i = 0; i < dArray.Length; i++ ) {
				dLogs[i] = Math.Log( dArray[i] );
				dLogs[i] *= (double)dObsFreq[i];
			}

			double dLL = MathExtensions.Sum( dLogs );

			return dLL;
		}

		/// <summary> Cumulative Distribution Function for the diffusion model with random cross-trial 
		/// drift, starting point, and nondecision time. Uses 20 quadrature points. 
		///  
		/// See this citation for more detailed methods and derivation: 
		/// Tuerlinckx, F. (2004). The efficient compution of the cumulative distribution function and probability 
		/// density functions in the diffusion model. Behavior Research Methods, Instruments, and Computers: 36 (4), 
		/// 702-716. </summary>
		/// <param name='t'> quantiles </param>
		/// <param name='x'> 1 or 0 for response given </param>
		/// <param name='par'> Array of diffusion model parameters. </param>
		/// <param name='prob'> Marginal probability of a 1 response. </param>
		/// <returns> Conditional cumulative density for each reaction time. </returns>
		unsafe public static double CDF ( double t, int x, double[] par, double* prob ) {
			#region Locals
			double a 	= par[0];	// boundary separation
			double Ter	= par[1];	// non-decision time
			double eta	= par[2];	// SD of drift rate
			double z	= par[3];	// starting position
			double sZ	= par[4];	// SD of starting position
			double st	= par[5];	// SD of TER
			double nu	= par[6];	// Drift rate (v)

			double a2 = a * a;

			// Upper and lower bounds of starting point distributions
			double dZUpper = ( 1d - x ) * z + x * ( a - z ) + sZ / 2d;
			double dZLower = ( 1d - x ) * z + x * ( a - z ) - sZ / 2d;

			// Upper and lower bounds of Ter (non-decision time) distribution.
			double dTLower = Ter - st / 2d;
			double dTUpper;

			// Convergence values for terms added to partial sum
			double delta = 1e-29d;
			// Check for deviation from zero (default 1e-7d)
			double epsilon = double.Epsilon;
			// Min RT to complete decision process.
			double dRtMin = 0.001d;
			// Maximum number of terms in the partial sum.
			int dVMaxTerms = 5000;

			// CDF, partial sums, probability variables
			double Fnew = 0d, sumZ = 0d, sumNu = 0d, p0, p1;

			double[] sumHist = new double[3];

			double denom, sifa, upp, low, fact, exdif, su, sl, zzz, ser;

			// Number of Gauss-Hermite quadrature points.
			int nr_nu = 20;
			// Number of Gaussian quadrature points.
			int nr_z = 20;
			#endregion

			#region Gaussian Quadrature Nodes and Weights
			//Gauss-Hermite quadrature weights
			double[] w_gh = new double[20] {
				0.22293936455341646528e-12d, 0.43993409922731809293e-9d,
				0.10860693707692821796e-6d, 0.78025564785320632605e-5d,
				0.22833863601635307687e-3d, 0.32437733422378566862e-2d,
				0.24810520887340880430e-1d, 0.10901720602002162863d,
				0.28667550536283409324d, 0.46224366960061014087d,
				0.46224366960061014087d, 0.28667550536283409324d,
				0.10901720602002162863d, 0.24810520887340880430e-1d,
				0.32437733422378566862e-2d, 0.22833863601635307687e-3d,
				0.78025564785320632605e-5d, 0.10860693707692821796e-6d,
				0.43993409922731809293e-9d, 0.22293936455341646528e-12d};
			// Gauss-Hermite quadrature nodes
			double[] gk = new double[20] {
				-5.3874808900112327592d, -4.6036824495507442379d,
				-3.9447640401156252032d, -3.3478545673832162954d,
				-2.7888060584281304521d, -2.2549740020892756753d,
				-1.7385377121165861425d, -1.2340762153953230840d,
				-.73747372854539439135d, -.24534070830090126680d,
				0.24534070830090126680d, 0.73747372854539439135d,
				1.2340762153953230840d, 1.7385377121165861425d,
				2.2549740020892756753d, 2.7888060584281304521d,
				3.3478545673832162954d, 3.9447640401156252032d,
				4.6036824495507442379d, 5.3874808900112327592d};
			// Gaussian quadrature weights
			double[] w_g = new double[20] {
				0.17614007115893910022e-1d, 0.40601429824919738065e-1d,
				0.62672048327592072559e-1d, 0.83276741580984969815e-1d,
				0.10193011981663034626d, 0.11819453196160842334d,
				0.13168863844919270756d, 0.14209610931837018954d,
				0.14917298647260451849d, 0.15275338713072578178d,
				0.15275338713072586505d, 0.14917298647260440747d,
				0.14209610931836397230d, 0.13168863844922096273d,
				0.11819453196171304798d, 0.10193011981627890516d,
				0.83276741581628954680e-1d, 0.62672048318034509484e-1d,
				0.40601429821751071347e-1d, 0.17614007115704332501e-1d };
			// Guassian quadrature nodes
			double[] gz = new double[20] {
				-0.99312859919393192687d, -0.96397192725643798816d,
				-0.91223442827299427993d, -0.83911697180954192277d,
				-0.74633190646438019034d, -0.63605368072610402042d,
				-0.51086700195056844453d, -0.37370608871551552754d,
				-0.22778585114163685255d, -0.76526521133497449334e-1d,
				0.76526521133497338312e-1d, 0.22778585114163671377d,
				0.37370608871551153074d, 0.51086700195060519292d,
				0.63605368072587842310d, 0.74633190646520863876d,
				0.83911697180775823846d, 0.91223442827524447996d,
				0.96397192725477587327d, 0.99312859919448925883d };
			#endregion

			#region Quadrature Scaling
			// Drift rate uses GH-quadrature rules
			for ( int i = 0; i < nr_nu; i++ ) {
				// Scaling of GH quadrature nodes based on normal kernel
				gk[i] = 1.41421356237309505d * gk[i] * eta + nu;
				// Weighting of GH quadrature weights based on normal kernel
				w_gh[i] = w_gh[i] / 1.772453850905515882d;
			}

			// Others will use standard Gaussian quadrature rules
			for ( int i = 0; i < nr_z; i++ ) {
				// Scaling of Guassian quadrature nodes
				gz[i] = ( 0.5d * sZ * gz[i] ) + z;
			}
			#endregion

			#region Compute integrated probability Pr(X=1)
			// Numerical intergration with respect to z0
			for ( int i = 0; i < nr_z; i++ ) {
				sumNu = 0d;
				// Numerical integration with respect to xi
				for ( int m = 0; m < nr_nu; m++ ) {
					// if GK[j] is approximately zero (within precision):
					if ( Math.Abs( gk[m] ) > epsilon ) {
						sumNu += ( Math.Exp( -200d * gz[i] * gk[m] ) - 1d ) / ( Math.Exp( -200d * a * gk[m] ) - 1d ) * w_gh[m];
					} else {
						sumNu += gz[i] / a * w_gh[m];
					}
				}
				sumZ += sumNu * w_g[i] / 2d;
			}
			*prob = sumZ;
			#endregion

			#region Compute Second Part Distribution Function
			// If t is above lower bound of Ter distribution
			if ( t - Ter + st / 2d > dRtMin ) {
				dTUpper = Math.Min( t, ( Ter + st / 2d ) );

				// Integrate probability with respect to t (for 1s)
				p1 = *prob * ( dTUpper - dTLower ) / st;
				// Probability for 0s
				p0 = ( 1 - *prob ) * ( dTUpper - dTLower ) / st;

				// If t is above upper bound of Ter distribution
				if ( t > ( Ter + st / 2d ) ) {
					sumHist[0] = 0; sumHist[1] = 0; sumHist[2] = 0;
					// Approximate infinite series
					for ( int v = 0; v < dVMaxTerms; v++ ) {
						sumHist[0] = sumHist[1];
						sumHist[1] = sumHist[2];
						sumNu = 0;
						sifa = Math.PI * v / a;
						// Integration across xi
						for ( int m = 0; m < nr_nu; m++ ) {
							denom = ( 100d * gk[m] * gk[m] + ( Math.PI * Math.PI ) * ( v * v ) / ( 100d * a2 ) );
							upp = Math.Exp( ( 2d * x - 1d ) * dZUpper * gk[m] * 100d - 3 * Math.Log( denom ) + Math.Log( w_gh[m] ) - 2 * Math.Log( 100d ) );
							low = Math.Exp( ( 2d * x - 1d ) * dZLower * gk[m] * 100d - 3 * Math.Log( denom ) + Math.Log( w_gh[m] ) - 2 * Math.Log( 100d ) );
							fact = upp * ( ( 2 * x - 1d ) * gk[m] * Math.Sin( sifa * dZUpper ) * 100d - sifa * Math.Cos( sifa * dZUpper ) ) -
								low * ( ( 2d * x - 1d ) * gk[m] * Math.Sin( sifa * dZLower ) * 100d - sifa * Math.Cos( sifa * dZLower ) );
							exdif = Math.Exp( ( -0.5d * denom * ( t - dTUpper ) ) + Math.Log( 1 - Math.Exp( -0.5d * denom * ( dTUpper - dTLower ) ) ) );
							sumNu += fact * exdif;
						}
						sumHist[2] = sumHist[1] + v * sumNu;
						if ( ( Math.Abs( sumHist[0] - sumHist[1] ) < delta ) &&
							( Math.Abs( sumHist[1] - sumHist[2] ) < delta ) && ( sumHist[2] > 0 ) ) {
							break;
						}
					}
					// CDF for t and x
					Fnew = ( p0 * ( 1d - x ) + p1 * x ) - sumHist[2] * 4d * Math.PI / ( a2 * sZ * st );
					// if t is lower than the upper bound of the Ter distribution
				} else if ( t <= ( Ter + st / 2d ) ) {
					sumNu = 0;
					for ( int m = 0; m < nr_nu; m++ ) {
						if ( Math.Abs( gk[m] ) > epsilon ) {
							sumZ = 0;
							for ( int i = 0; i < nr_z; i++ ) {
								zzz = ( a - gz[i] ) * x + gz[i] * ( 1d - x );
								ser = -( ( a * a2 ) / ( ( 1d - 2d * x ) * gk[m] * Math.PI * 0.01d ) ) *
									Math.Sinh( zzz * ( 1d - 2d * x ) * gk[m] / 0.01d ) /
									( Math.Sinh( ( 1d - 2d * x ) * gk[m] * a / 0.01d ) * Math.Sinh( ( 1d - 2d * x ) * gk[m] * a / 0.01d ) ) +
									( zzz * a2 ) / ( ( 1d - 2d * x ) * gk[m] * Math.PI * 0.01d ) *
									Math.Cosh( ( a - zzz ) * ( 1d - 2d * x ) * gk[m] / 0.01d ) /
									Math.Sinh( ( 1d - 2d * x ) * gk[m] * a / 0.01d );
								sumHist[0] = 0;
								sumHist[1] = 0;
								sumHist[2] = 0;
								for ( int v = 0; v < dVMaxTerms; v++ ) {
									sumHist[0] = sumHist[1];
									sumHist[1] = sumHist[2];
									sifa = Math.PI * v / a;
									denom = ( gk[m] * gk[m] * 100d + ( Math.PI * v ) * ( Math.PI * v ) / ( a2 * 100d ) );
									sumHist[2] = sumHist[1] + v * Math.Sin( sifa * zzz ) * Math.Exp( -0.5d * denom * ( t - dTLower )
																								  - 2 * Math.Log( denom ) );
									if ( ( Math.Abs( sumHist[0] - sumHist[1] ) < delta ) &&
										( Math.Abs( sumHist[1] - sumHist[2] ) < delta ) && ( sumHist[2] > 0 ) ) {
										break;
									}
								}
								sumZ += 0.5d * w_g[i] * ( ser - 4d * sumHist[2] ) * ( Math.PI / 100d ) /
									( a2 * st ) * Math.Exp( ( 2d * x - 1d ) * zzz * gk[m] * 100d );
							}
						} else {
							sumHist[0] = 0;
							sumHist[1] = 0;
							sumHist[2] = 0;
							su = -( dZUpper * dZUpper ) / ( 12d * a2 ) +
								( dZUpper * dZUpper * dZUpper ) / ( 12d * a * a2 ) -
									( dZUpper * dZUpper * dZUpper * dZUpper ) / ( 48d * a2 * a2 );
							sl = -( dZLower * dZLower ) / ( 12d * a2 ) +
								( dZLower * dZLower * dZLower ) / ( 12d * a * a2 ) -
									( dZLower * dZLower * dZLower * dZLower ) / ( 48d * a2 * a2 );
							for ( int v = 1; v < dVMaxTerms; v++ ) {
								sumHist[0] = sumHist[1];
								sumHist[1] = sumHist[2];
								sifa = Math.PI * v / a;
								denom = ( Math.PI * v ) * ( Math.PI * v ) / ( a2 * 100d );
								sumHist[2] = sumHist[1] + 1 / ( Math.Pow( Math.PI, 4 ) * Math.Pow( v, 4 ) ) * ( Math.Cos( sifa * dZLower ) - Math.Cos( sifa * dZUpper ) ) * Math.Exp( -0.5d * denom * ( t - dTLower ) );
								if ( ( Math.Abs( sumHist[0] - sumHist[1] ) < delta ) && ( Math.Abs( sumHist[1] - sumHist[2] ) < delta ) && ( sumHist[2] > 0 ) ) {
									break;
								}
							}
							sumZ = 400d * a2 * a * ( sl - su - sumHist[2] ) / ( st * sZ );
						}
						sumNu += sumZ * w_gh[m];
					}
					Fnew = ( p0 * ( 1d - x ) + p1 * x ) - sumNu;
				}
			} else if ( ( t - Ter + st / 2 ) <= dRtMin || Fnew < delta ) {	//if t lower than lower bound of ter dist
				Fnew = 0;
			}
			#endregion;

			return Fnew;
		}
	}
}