using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Provides data for the <see cref="CefNetApplication.BeforeChildProcessLaunch"/> event.
	/// </summary>
	public class BeforeChildProcessLaunchEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BeforeChildProcessLaunchEventArgs"/> class.
		/// </summary>
		/// <param name="commandLine">The command line.</param>
		public BeforeChildProcessLaunchEventArgs(CefCommandLine commandLine)
		{
			this.CommandLine = commandLine;
		}

		/// <summary>
		/// Provides an opportunity to modify the child process command line.<para/>
		/// Do not keep a reference to <see cref="CommandLine"/> outside
		/// of <see cref="CefNetApplication.BeforeChildProcessLaunch"/> event.
		/// </summary>
		public CefCommandLine CommandLine { get; }
	}
}
