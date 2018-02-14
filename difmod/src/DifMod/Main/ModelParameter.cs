/// <summary>
/// ModelParameter.cs
/// 
/// Version 0.1
/// Created by: Josh Tremel (tremeljosh@gmail.com)
/// Created on: January 26, 2012
/// 
/// Container for a model parameter. Note, this was a convenient way to handle parameters 
/// at one point in time and will be deprecated soon.
/// </summary>
namespace DifMod {
	public class ModelParameter {
		/// <summary> Value of the parameter. </summary>
		private double _dValue;
		/// <summary> Returns the current value of the parameter. </summary>
		/// <returns> Parameter's current value. </returns>
		public double GetParamValue () {
			return _dValue;
		}
		/// <summary> Sets the current value of the parameter. </summary>
		/// <param name="dValue"> Value to set. </param>
		public void SetParamValue ( double dValue ) {
			_dValue = dValue;
		}
	}
}