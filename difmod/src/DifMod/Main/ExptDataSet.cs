/// <summary>
/// ExptDataSet.cs
/// 
/// Version 1.0
/// Created by: Josh Tremel (tremeljosh@gmail.com)
/// Created on: January 26, 2012
/// 
/// Reads, holds, and manipulates a data set for the diffusion model.
/// </summary>
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;
using ICSharpCode;
namespace DifMod {
	public class ExptDataSet {
		#region Locals
		/// <summary> List of RTs. </summary>
		private List<double> _rt = new List<double>();
		/// <summary> List of responses. </summary>
		private List<int> _responses = new List<int>();
		/// <summary> List of event codes for each trial. </summary>
		private List<int> _event = new List<int>();
		/// <summary> List of event codes used in the dataset. </summary>
		private List<int> _eventCode = new List<int>();
		#endregion
		#region Container Classes
		/// <summary> Descriptive statistics for this data set. </summary>
		public DescriptiveStats descriptives = new DescriptiveStats();
		/// <summary> The model parameters for this data set. </summary>
		public Parameters parameters = new Parameters();
		/// <summary> Holds various descriptive statistics and data subsets for this data set. </summary>
		public class DescriptiveStats {
			/// <summary>
			/// Number of targets.
			/// </summary>
			public int nTarget;
			/// <summary>
			/// Number of nontargets.
			/// </summary>
			public int nNontarget;
			/// <summary>
			/// Total number of trials.
			/// </summary>
			public int nTotalTrials;
			/// <summary>
			/// Proportion of targets.
			/// </summary>
			public float fTargetProportion;
			/// <summary>
			/// Array of observed frequencies in each bin for targets.
			/// </summary>
			public int[] nBinObsFreqTarget = new int[6];
			/// <summary>
			/// Array of observed frequencies in each bin for nontargets.
			/// </summary>
			public int[] nBinObsFreqNontarget = new int[6];
			/// <summary>
			/// Values of quantiles (in seconds) for targets.
			/// </summary>
			public double[] dQuantilesTarget = new double[5];
			/// <summary>
			/// Values of quantiles (in seconds) for nontargets.
			/// </summary>
			public double[] dQuantilesNontarget = new double[5];
			/// <summary>
			/// Maximum RT for targets.
			/// </summary>
			public double dRtMaxTarget;
			/// <summary>
			/// Minimum RT for targets.
			/// </summary>
			public double dRtMinTarget;
			/// <summary>
			/// Mean RT of targets.
			/// </summary>
			public double dRtMeanTarget;
			/// <summary>
			/// RT Variance of targets.
			/// </summary>
			public double dRtVarTarget;
			/// <summary>
			/// Maximum RT for nontargets.
			/// </summary>
			public double dRtMaxNontarget;
			/// <summary>
			/// Minimum RT for nontargets.
			/// </summary>
			public double dRtMinNontarget;
			/// <summary>
			/// Mean RT of nontargets.
			/// </summary>
			public double dRtMeanNontarget;
			/// <summary>
			/// Variance of nontargets.
			/// </summary>
			public double dRtVarNontarget;
			/// <summary>
			/// Mean RT for all trials.
			/// </summary>
			public double dRtMean;
			/// <summary>
			/// Variance in RT for all trials.
			/// </summary>
			public double dRtVar;
			/// <summary>
			/// List of RTs for target items.
			/// </summary>
			public List<double> rtTargets;
			/// <summary>
			/// List of RTs for nontargets.
			/// </summary>
			public List<double> rtNontargets;
		}
		/// <summary> Holds the model parameters for this data set. </summary>
		public class Parameters {
			public ModelParameter[] param;
		}
		#endregion
		#region Data Methods
		/// <summary> Returns the number of unique events in this data set. </summary>
		/// <returns> Integer count of unique events. </returns>
		public int GetEventCount () {
			if ( _eventCode.Count > 0 ) {
				return _eventCode.Count;
			} else {
				for ( int i = 0; i < _event.Count; i++ ) {
					if ( !IsKnownEvent( _event[i] ) ) {
						_eventCode.Add( _event[i] );
					}
				}
				return _eventCode.Count;
			}
		}
		/// <summary> Returns the event code at specified index </summary>
		/// <returns> The event at specified index. </returns>
		/// <param name='nIndex'> Integer index of event. </param>
		public int GetEvent ( int nIndex ) {
			return _event[nIndex];
		}
		/// <summary> Returns the response at specified index. '1' for target, '0' for non-target. </summary>
		/// <returns> Integer response at this index. </returns>
		/// <param name='nIndex'> Integer index of event </param>
		public int GetResponse ( int nIndex ) {
			return _responses[nIndex];
		}
		/// <summary> Returns the response time (RT) at the specified index. </summary>
		/// <returns> Float response time. </returns>
		/// <param name='nIndex'> Integer index. </param>
		public double GetRT ( int nIndex ) {
			return _rt[nIndex];
		}
		/// <summary> Returns the length of the data set (i.e., number of trials). Returns
		/// -1 if the lists in the data set are of different lengths.</summary>
		/// <returns> Integer count of elements in the data set.</returns>
		public int GetDataLength () {
			int nLength = _rt.Count;
			if ( _responses.Count != nLength || _event.Count != nLength ) {
				Console.WriteLine( "ExptData ERROR: GetDataLength(): data columns are of different sizes." );
				return -1;
			}
			return nLength;
		}
		/// <summary> Adds a "row" to the data set. Realistically, adds one element to the RT, Response, and Event lists. </summary>
		/// <param name='nEvent'> Integer of the event to add. </param>
		/// <param name='nAccuracy'> Integer accuracy to add. </param>
		/// <param name='dRt'> Float RT to add. </param>
		public void AddRow ( int nEvent, int nAccuracy, double dRt ) {
			_event.Add( nEvent );
			_responses.Add( nAccuracy );
			_rt.Add( dRt );
		}
		/// <summary> Removes a row from the data set at the specified index. Realistically,
		/// removes the element at index in the rt, response, and event lists. </summary>
		/// <param name='nIndex'> Zero-based index of element in the list to remove. </param>
		public void RemoveRow ( int nIndex ) {
			_event.RemoveAt( nIndex );
			_responses.RemoveAt( nIndex );
			_rt.RemoveAt( nIndex );
		}
		/// <summary> Initializes a set of model parameters for this data set. </summary>
		/// <param name="input"> Set of input options. </param>
		public void SetUpParameters ( ModelInput input ) {

			//FIXME: THIS IS PRETTY HACKISH... NEED TO ACTUALLY HANDLE INPUT...
			parameters.param = new ModelParameter[input.nParamCount];

			for ( int i = 0; i < parameters.param.Length; i++ ) {
				parameters.param[i] = new ModelParameter();
			}
		}
		/// <summary> Checks if the model is aware of this event's existance. Returns 
		/// true if it has this event code cached and false otherwise. </summary>
		/// <returns> Bool whether it knows about this event number. </returns>
		/// <param name='nEventCode'> Integer event number </param>
		private bool IsKnownEvent ( int nEventCode ) {
			bool bKnown = false;
			for ( int i = 0; i < _eventCode.Count; i++ ) {
				if ( nEventCode == _eventCode[i] ) {
					bKnown = true;
					break;
				}
			}
			return bKnown;
		}
		/// <summary> Reads in data from an XLS file. </summary>
		/// <param name='sPath'> String path to file </param>
		public void ReadFromXLS ( string sPath ) {
			if ( sPath == null ) {
				throw new NullReferenceException( "Input data path is null" );
			}
			FileStream stream = File.Open( sPath, FileMode.Open, FileAccess.Read );
			IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader( stream );
			excelReader.IsFirstRowAsColumnNames = true;
			DataSet dsInData = excelReader.AsDataSet();
			stream.Close();
			stream.Dispose();
			int nColCount = dsInData.Tables[0].Columns.Count;
			int nRowCount = dsInData.Tables[0].Rows.Count;
			if ( nColCount != 3 ) {
				throw new InvalidDataException( "Column count is not 3. Invalid data." );
			}
			for ( int i = 0; i < nRowCount; i++ ) {
				AddRow( Convert.ToInt32( dsInData.Tables[0].Rows[i][0] ),
						Convert.ToInt32( dsInData.Tables[0].Rows[i][1] ),
						Convert.ToDouble( dsInData.Tables[0].Rows[i][2] )
					   );
			}
			// Set up list of unique event codes
			GetEventCount();
			for ( int i = 0; i < nRowCount; i++ ) {
				if ( GetRT( i ) < 0 ) {
					throw new DataException( "Found a negative RT in the imported data. Please check your input." );
				}
				if ( GetResponse( i ) != 1 && GetResponse( i ) != 0 ) {
					throw new DataException( "Found an invalid accuracy code (i.e., not 1 or 0) in the imported data. Please check your input." );
				}
				// Check events--make sure they are consecutive, starting from 0
			}
		}
		#endregion
		#region Stats Methods
		/// <summary> Calculates various descriptive statistics for this data set. Resulting stats will be held in 
		/// a <c>DescriptiveStats</c> struct called "descriptives." <c>ModelInput</c> controls some options regarding 
		/// how calculations are performed, etc. </summary>
		/// <param name='input'> Input struct containing options for calculations. </param>
		public void CalculateStats ( ModelInput input ) {
			// Set up lists of target item rts and list of nontarget item rts
			descriptives.rtTargets = new List<double>();
			descriptives.rtNontargets = new List<double>();
			descriptives.rtTargets = GetTargetRtList();
			descriptives.rtNontargets = GetNontargetRtList();
			// Trial Counts
			descriptives.nTarget = CountTargetItems();
			descriptives.nNontarget = CountNontargetItems();
			descriptives.nTotalTrials = GetDataLength();
			// Proportion of targets
			descriptives.fTargetProportion = ( (float)descriptives.nTarget / (float)descriptives.nTotalTrials );
			// RT Mean and variance
			descriptives.dRtMean = GetMeanRT( _rt );
			descriptives.dRtVar = GetRTVariance( _rt );
			// RT Mean for target and nontarget items
			descriptives.dRtMeanTarget = GetMeanRT( descriptives.rtTargets );
			descriptives.dRtMeanNontarget = GetMeanRT( descriptives.rtNontargets );
			// RT Variance for target and nontarget items.
			descriptives.dRtVarTarget = GetRTVariance( descriptives.rtTargets );
			descriptives.dRtVarNontarget = GetRTVariance( descriptives.rtNontargets );
			// Quintiles
			for ( int i = 0; i < input.dUserQuantilesCor.Length; i++ ) {
				descriptives.dQuantilesTarget[i] = input.dUserQuantilesCor[i];
				descriptives.dQuantilesNontarget[i] = input.dUserQuantilesInc[i];
			}
			// If user specifies to use percentiles, calculate quantiles
			if ( input.nEstMethodBins == consts_h.EST_METHOD_BINS_USER_PERCENT ) {
				if ( descriptives.nTarget > consts_h.MIN_TRAILS_NEEDED ) {
					for ( int i = 0; i < descriptives.dQuantilesTarget.Length; i++ ) {
						descriptives.dQuantilesTarget[i] = GetPercentileValue( input.dUserPercentilesCor[i], descriptives.rtTargets );
					}
				}
				if ( descriptives.nNontarget > consts_h.MIN_TRAILS_NEEDED ) {
					for ( int i = 0; i < descriptives.dQuantilesNontarget.Length; i++ ) {
						descriptives.dQuantilesNontarget[i] = GetPercentileValue( input.dUserPercentilesInc[i], descriptives.rtNontargets );
					}
				}
			}
			// RT extrema
			descriptives.dRtMaxTarget = GetMaxRT( descriptives.rtTargets );
			descriptives.dRtMaxNontarget = GetMaxRT( descriptives.rtNontargets );
			descriptives.dRtMinTarget = GetMinRT( descriptives.rtTargets );
			descriptives.dRtMinNontarget = GetMinRT( descriptives.rtNontargets );
			// Quintile observed frequencies (relative frequency density: occurances/total observerations)
			if ( descriptives.nTarget > consts_h.MIN_TRAILS_NEEDED ) {
				for ( int i = 0; i < descriptives.nBinObsFreqTarget.Length; i++ ) {
					if ( i == 0 ) {
						descriptives.nBinObsFreqTarget[i] = GetObservedFrequency( descriptives.dRtMinTarget, descriptives.dQuantilesTarget[i], descriptives.rtTargets );
					} else if ( i > 0 && i <= descriptives.nBinObsFreqTarget.Length - 2 ) {
						// -2 is from: -1 for index adjustment, another -1 for second-to-last index.
						descriptives.nBinObsFreqTarget[i] = GetObservedFrequency( descriptives.dQuantilesTarget[i - 1], descriptives.dQuantilesTarget[i], descriptives.rtTargets );
					} else {
						descriptives.nBinObsFreqTarget[i] = GetObservedFrequency( descriptives.dQuantilesTarget[i - 1], descriptives.dRtMaxTarget + 0.1d, descriptives.rtTargets );
					}
				}
			}
			if ( descriptives.nNontarget > consts_h.MIN_TRAILS_NEEDED ) {
				for ( int i = 0; i < descriptives.nBinObsFreqNontarget.Length; i++ ) {
					if ( i == 0 ) {
						descriptives.nBinObsFreqNontarget[i] = GetObservedFrequency( descriptives.dRtMinNontarget, descriptives.dQuantilesTarget[i], descriptives.rtNontargets );
					} else if ( i > 0 && i <= descriptives.nBinObsFreqTarget.Length - 2 ) {
						descriptives.nBinObsFreqNontarget[i] = GetObservedFrequency( descriptives.dQuantilesNontarget[i - 1], descriptives.dQuantilesNontarget[i], descriptives.rtNontargets );
					} else {
						descriptives.nBinObsFreqNontarget[i] = GetObservedFrequency( descriptives.dQuantilesNontarget[i - 1], descriptives.dRtMaxNontarget + 0.1d, descriptives.rtNontargets );
					}
				}
			}
		}
		/// <summary> Calculates the proportion of Target responses for this data set. </summary>
		/// <returns> The proportion of targets. </returns>
		public float GetTargetProportion () {
			int[] nArray = _responses.ToArray();
			float fPropTarget = MathExtensions.Mean( nArray );
			return fPropTarget;
		}
		/// <summary> Calculates the average RT for this data set. </summary>
		/// <returns> Average RT </returns>
		public double GetMeanRT ( List<double> rtList ) {
			double[] fArray = rtList.ToArray();
			double fRtMean = MathExtensions.Mean( fArray );
			return fRtMean;
		}
		/// <summary> Finds the minimum response time (RT) value for this data set. </summary>
		/// <returns> The value of the minimum RT </returns>
		public double GetMinRT ( List<double> rtList ) {
			double min = rtList[0];
			for ( int i = 0; i < rtList.Count; i++ ) {
				if ( rtList[i] < min ) {
					min = rtList[i];
				}
			}
			return min;
		}
		/// <summary> Finds the maximum response time (RT) value for this data set. </summary>
		/// <returns> The value of the maximum RT. </returns>
		public double GetMaxRT ( List<double> rtList ) {
			double max = rtList[0];
			for ( int i = 0; i < rtList.Count; i++ ) {
				if ( rtList[i] > max ) {
					max = rtList[i];
				}
			}
			return max;
		}
		/// <summary> Returns the variance of the RT values. </summary>
		/// <returns> Double variance of the RTs </returns>
		public double GetRTVariance ( List<double> rtList ) {
			double dVar = MathExtensions.Variance( rtList.ToArray() );
			return dVar;
		}
		/// <summary> Returns the count of Target items. </summary>
		/// <returns> Integer count of target trials. </returns>
		private int CountTargetItems () {
			int nCount = 0;
			for ( int i = 0; i < GetDataLength(); i++ ) {
				if ( GetResponse( i ) == 1 ) {
					nCount += 1;
				}
			}
			return nCount;
		}
		/// <summary> Returns the count of Non-target items. </summary>
		/// <returns> Integer count of non-target items. </returns>
		private int CountNontargetItems () {
			int nCount = 0;
			for ( int i = 0; i < GetDataLength(); i++ ) {
				if ( GetResponse( i ) == 0 ) {
					nCount += 1;
				}
			}
			return nCount;
		}
		/// <summary> Returns the rt value for this percentile. </summary>
		/// <returns> The value for this percentile. </returns>
		/// <param name='percentile'> Float percentile. </param>
		/// <param name='rtList'> List of rts to use. </param>
		private double GetPercentileValue ( double percentile, List<double> rtList ) {
			// Check and fix units for percentiles.
			if ( percentile > 1.0 ) {
				percentile = percentile * 0.01d;
			}
			rtList.Sort();
			var k = (int)( rtList.Count * percentile );
			var pk = ( k - 0.5f ) / rtList.Count;
			return rtList[k - 1] + ( rtList.Count * ( percentile - pk ) * ( rtList[k] - rtList[k - 1] ) );
		}
		/// <summary> Returns the observed frequency for a bin within lower and upper bounds. </summary>
		/// <returns> The count of observations in specified bin. </returns>
		/// <param name='fLowerBound'> Lower boundary for this bin (must be greater than or equal to this). </param>
		/// <param name='fUpperBound'> Upper boundary for this bin (must be less than this). </param>
		/// <param name='rtList'> List of rts to use. </param>
		private int GetObservedFrequency ( double fLowerBound, double fUpperBound, List<double> rtList ) {
			int nCount = 0;
			for ( int i = 0; i < rtList.Count; i++ ) {
				if ( rtList[i] >= fLowerBound && rtList[i] < fUpperBound ) {
					nCount += 1;
				}
			}
			return nCount;
		}
		/// <summary> Returns a list of RTs for target items </summary>
		/// <returns> List of rts for target items. </returns>
		private List<double> GetTargetRtList () {
			List<double> targetRt = new List<double>();
			for ( int i = 0; i < _rt.Count; i++ ) {
				if ( GetResponse( i ) == 1 ) {
					targetRt.Add( _rt[i] );
				}
			}
			// Make sure list contains at least one value
			if ( targetRt.Count > 0 ) {
				return targetRt;
			} else {
				return null;
			}
		}
		/// <summary> Returns a list of RTs for nontarget items. </summary>
		/// <returns> List of rts for nontargets. </returns>
		private List<double> GetNontargetRtList () {
			List<double>ntRt = new List<double>();
			for ( int i = 0; i < _rt.Count; i++ ) {
				if ( GetResponse( i ) == 0 ) {
					ntRt.Add( _rt[i] );
				}
			}
			if ( ntRt.Count > 0 ) {
				return ntRt;
			} else {
				return null;
			}
		}
		#endregion
	}
}