/// <summary>
/// DesignMatrix.cs
/// 
/// Version 0.1
/// Created by: Josh Tremel (tremeljosh@gmail.com)
/// Created on: January 26, 2012
/// 
/// Functionality for design matricies. Note, this is not yet fully functional, but
/// a framework to add in later.
/// </summary>
using System;
namespace DifMod {
	public class DesignMatrix {
		#region Locals
		/// <summary>
		/// The design matrix.
		/// </summary>
		private int[,] _matrix;
		/// <summary>
		/// Type of design matrix.
		/// </summary>
		private int _matrixType;
		/// <summary>
		/// Total number of rows in the matrix.
		/// </summary>
		private int _nRow;
		/// <summary>
		/// Total number of columns in the matrix.
		/// </summary>
		private int _nCol;
		#endregion
		
		#region Initialization
		/// <summary>
		/// Creates a new instance of a design matrix. Sets type as invalid.
		/// </summary>
		public DesignMatrix () {
			SetMatrixType( consts_h.MATRIX_TYPE_INVALID );
			_nRow = 0;
			_nCol = 0;
		}
		/// <summary>
		/// Initializes a blank design matrix with specified number of entries (rows) and 
		/// conditions (columns).
		/// </summary>
		/// <param name='nRows'> Number of entries (rows/events) in the matrix. </param>
		/// <param name='nCols'> Number of conditions (columns) in the matrix. </param>
		public void CreateBlankMatrix ( int nRows, int nCols ) {
			_matrix = new int[nRows, nCols];
			_nRow = nRows;
			_nCol = nCols;
		}
		/// <summary>
		/// Creates an identity matrix (i.e., no design constraints).
		/// </summary>
		/// <param name='nOrder'> Order of the matrix. </param>
		public void CreateIdentityMatrix ( int nOrder ) {
			_matrix = new int[nOrder, nOrder];
			for ( int i = 0; i < nOrder; i++ ) {
				_matrix[i, i] = 1;
			}
			SetMatrixType( consts_h.MATRIX_TYPE_IDENTITY );
			_nRow = nOrder;
			_nCol = nOrder;
		}
		/// <summary>
		/// Creates a no-effects matrix (column of ones). This constrains the design so that 
		/// parameter controlled by this matrix is fixed across conditions.
		/// </summary>
		/// <param name='nRows'> Number of conditions. </param>
		public void CreateNoEffectsMatrix ( int nRows ) {
			_matrix = new int[nRows, 1];
			for ( int i = 0; i < nRows; i++ ) {
				_matrix[i, 0] = 1;
			}
			SetMatrixType( consts_h.MATRIX_TYPE_NO_EFFECTS );
			_nRow = nRows;
			_nCol = 1;
		}
		#endregion
		
		/// <summary>
		/// Checks the matrix validity. Returns true if your matrix is usable.
		/// </summary>
		/// <returns> True if matrix is usable. </returns>
		public bool IsValidMatrix () {
			bool bValid = false;
			if ( _nRow == 0 || _nCol == 0 ) {
				Console.WriteLine( "ERROR: CheckMatrixValidity(): Matrix has not been initialized." );
				return bValid;
			}
			if ( GetMatrixType() == consts_h.MATRIX_TYPE_IDENTITY || GetMatrixType() == consts_h.MATRIX_TYPE_NO_EFFECTS ) {
				bValid = true;
				return bValid;
			}

			// If still ambigious, loop through each column and make sure there's at least one +1 or -1 value.
			//TODO: support for different types of matrices.
			for ( int iCol = 0; iCol < _nCol; iCol++ ) {
				bool bValidCol = false;
				for ( int iRow = 0; iRow < _nRow; iRow++ ) {
					if ( _matrix[iRow, iCol] == -1 || _matrix[iRow, iCol] == 1 ) {
						bValidCol = true;
					}
				}
				if ( !bValidCol ) {
					bValid = false;
					Console.WriteLine( "ERROR: CheckMatrixValidity(): Column " + iCol + " does not have at least one true value." );
					return bValid;
				}
			}
			// If we get here, the matrix seems to be valid.
			bValid = true;
			// Set the type to custom.
			if ( bValid && GetMatrixType() == consts_h.MATRIX_TYPE_INVALID ) {
				SetMatrixType( consts_h.MATRIX_TYPE_CUSTOM );
			}
			return bValid;
		}
		/// <summary>
		/// Gets the type of the matrix. See enumerated types in consts_h.
		/// </summary>
		/// <returns> The integer matrix type. </returns>
		public int GetMatrixType () {
			return _matrixType;
		}
		/// <summary>
		/// Sets the type of the matrix. See enumerated types in consts_h.
		/// </summary>
		/// <param name='nType'> Integer constant denoting matrix type. </param>
		private void SetMatrixType ( int nType ) {
			_matrixType = nType;
		}
		/// <summary>
		/// Returns the value of the matrix at the specified location.
		/// </summary>
		/// <returns> The value stored at specified location </returns>
		/// <param name='nRow'> Row number </param>
		/// <param name='nCol'> Column number </param>
		public int GetMatrixValue ( int nRow, int nCol ) {
			return _matrix[nRow, nCol];
		}
		/// <summary>
		/// Sets or unsets a value at the specified location in the design matrix. If value
		/// is 0, it will toggle it to 1. If value is 1, this will toggle it to 0.
		/// </summary>
		/// <param name='nRow'> Number of the row. </param>
		/// <param name='nCol'> Number of the column. </param>
		public void SetMatrixValue ( int nRow, int nCol ) {
			if ( GetMatrixValue( nRow, nCol ) == 1 ) {
				_matrix[nRow, nCol] = 0;
			} else if ( GetMatrixValue( nRow, nCol ) == 0 ) {
				_matrix[nRow, nCol] = 1;
			} else {
				Console.WriteLine( "ERROR: SetMatrixValue(): Invalid value in design matrix at col " + nCol + ", row " + nRow + "." );
				return;
			}
		}
		/// <summary>
		/// Returns the number of entries (rows) in the design matrix.
		/// </summary>
		/// <returns> Integer number of rows. </returns>
		public int GetRowCount () {
			return _nRow;
		}
		/// <summary>
		/// Returns the number of conditions (columns) in the design matrix.
		/// </summary>
		/// <returns> Integer number of conditions. </returns>
		public int GetColCount () {
			return _nCol;
		}
		/// <summary>
		/// Print some information about the matrix.
		/// </summary>
		public override string ToString () {
			string returnString = "| Design Matrix information \r\n" +
				"|=============================" + "\r\n" +
				"| Number of columns:   " + _nCol.ToString() + "\r\n" +
				"| Number of rows:      " + _nRow.ToString() + "\r\n" +
				"|=============================" + "\r\n";

			for ( int iRow = 0; iRow < _nRow; iRow++ ) {
				for ( int iCol = 0; iCol < _nCol; iCol++ ) {
					returnString += GetMatrixValue( iRow, iCol ).ToString() + "| ";
				}
				returnString += "\r\n";
			}
			returnString += "|=============================";

			return returnString;
		}
	}
}