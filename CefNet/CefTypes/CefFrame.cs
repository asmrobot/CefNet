using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefNet.CApi;
using CefNet.Internal;

namespace CefNet
{
	public partial class CefFrame
	{
		/// <summary>
		/// Returns the direct sub-frames of the current frame.
		/// </summary>
		/// <returns>The direct sub-frames of the current frame.</returns>
		public unsafe CefFrame[] GetFrames()
		{
			long frameid = this.Identifier;
			CefBrowser browser = this.Browser;
			if (browser is null)
			{
#if NET45
				return new CefFrame[0];
#else
				return Array.Empty<CefFrame>();
#endif
			}

			long[] ids = browser.GetFrameIdentifiers();
			var frames = new List<CefFrame>(ids.Length);
			foreach (long fid in ids)
			{
				cef_frame_t* frame = browser.NativeInstance->GetFrameByIdent(fid);
				if (frame == null)
					continue;

				cef_frame_t* parent = frame->GetParent();
				if (parent != null)
				{
					long parentid = parent->GetIdentifier();
					((cef_base_ref_counted_t*)parent)->Release();
					if (parentid == frameid)
					{
						frames.Add(CefFrame.Wrap<CefFrame>(f => new CefFrame((cef_frame_t*)f), frame));
						continue;
					}
				}
				((cef_base_ref_counted_t*)frame)->Release();
			}
			GC.KeepAlive(browser);
			return frames.ToArray();
		}

#pragma warning disable CS1591

		public override bool Equals(object obj)
		{
			var frame = obj as CefFrame;
			if (frame is null)
				return false;
			return this == frame;
		}

		public unsafe override int GetHashCode()
		{
			if (CefApi.UseUnsafeImplementation)
				return _instance.GetHashCode();
			return IsDisposed ? 0 : NativeInstance->GetIdentifier().GetHashCode();
		}

#pragma warning restore CS1591

		/// <summary>
		/// Tests whether two specified <see cref="CefFrame"/> objects are equivalent.
		/// </summary>
		/// <param name="a">The <see cref="CefFrame"/> that is to the left of the equality operator.</param>
		/// <param name="b">The <see cref="CefFrame"/> that is to the right of the equality operator.</param>
		/// <returns>true if the two <see cref="CefFrame"/> objects are equal; otherwise, false.</returns>
		public static bool operator ==(CefFrame a, CefFrame b)
		{
			if (ReferenceEquals(a, b))
				return true;

			if (!CefApi.UseUnsafeImplementation)
			{
				if (a is null)
					return b is null;
				if (b is null)
					return false;

				try
				{
					return a.Identifier == b.Identifier;
				}
				catch (ObjectDisposedException) { }
			}
			return false;
		}

		/// <summary>
		/// Tests whether two specified <see cref="CefFrame"/> objects are not equal.
		/// </summary>
		/// <param name="a">The first object to compare.</param>
		/// <param name="b">The second object to compare..</param>
		/// <returns>true if the two <see cref="CefFrame"/> objects are not equal; otherwise, false.</returns>
		public static bool operator !=(CefFrame a, CefFrame b)
		{
			return !(a == b);
		}

		/// <summary>
		/// Retrieves this frame&apos;s HTML source as an asynchronous operation.
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns>The task object that when completed returns the frame source as a string.</returns>
		public Task<string> GetSourceAsync(CancellationToken cancellationToken)
		{
			var visitor = new CefNetStringVisitor();
			this.GetSource(visitor);
			return visitor.GetAsync(cancellationToken);
		}

	}
}
