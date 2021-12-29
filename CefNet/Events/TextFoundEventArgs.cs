using System;
using System.Collections.Generic;
using System.Text;

namespace CefNet
{
	/// <summary>
	/// Represents find results.
	/// </summary>
	public sealed class TextFoundEventArgs : EventArgs, ITextFoundEventArgs
	{
		public TextFoundEventArgs(int identifier, int count, CefRect selectionRect, int index, bool finalUpdate)
		{
			this.ID = identifier;
			this.Count = count;
			this.SelectionRect = selectionRect;
			this.Index = index;
			this.FinalUpdate = finalUpdate;
		}

		public int ID { get; }

		public int Index { get; }

		public int Count { get; }

		public CefRect SelectionRect { get; }

		public bool FinalUpdate { get; }

	}
}
