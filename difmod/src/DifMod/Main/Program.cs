/// <summary>
/// Program.cs
/// 
/// Version 1.0
/// Created by: Josh Tremel (tremeljosh@gmail.com)
/// Created on: March 26, 2012
/// 
/// Main entry point for the diffusion model program.
/// </summary>
using System;
using System.Windows.Forms;

namespace DifMod {
	static class Program {
		/// <summary> User input for the diffusion model. </summary>
		public static ModelInput oModelInput;
		/// <summary> Main diffusion model. </summary>
		public static DiffusionModel oModel;
		/// <summary> Main entry point for the application. </summary>
		[STAThread]
		static void Main () {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new MainWindow() );
		}
	}
}
