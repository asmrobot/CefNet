using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace CefNet.Internal
{
	public partial class WebViewGlue
	{
		public void CreateOrDestroyDownloadGlue()
		{
			if (AvoidOnBeforeDownload()
				&& AvoidOnDownloadUpdated())
			{
				this.DownloadGlue = null;
			}
			else if (this.DownloadGlue is null)
			{
				this.DownloadGlue = new CefDownloadHandlerGlue(this);
			}
		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnBeforeDownload();

		/// <summary>
		/// Called before a download begins.
		/// </summary>
		/// <param name="browser">The browser.</param>
		/// <param name="suggestedName">The suggested name for the download file.</param>
		/// <param name="downloadItem">Do not keep a reference to <paramref name="downloadItem"/> outside of this function.</param>
		/// <param name="callback">
		/// Execute <paramref name="callback"/> either asynchronously or in this function
		/// to continue the download if desired.
		/// </param>
		/// <remarks>By default the download will be canceled.</remarks>
		internal protected virtual void OnBeforeDownload(CefBrowser browser, CefDownloadItem downloadItem, string suggestedName, CefBeforeDownloadCallback callback)
		{

		}

		[MethodImpl(MethodImplOptions.ForwardRef)]
		internal extern bool AvoidOnDownloadUpdated();

		/// <summary>
		/// Called when a download&apos;s status or progress information has been updated.
		/// </summary>
		/// <param name="downloadItem">
		/// Do not keep a reference to <paramref name="downloadItem"/> outside of this function.
		/// </param>
		/// <param name="callback">
		/// Execute <paramref name="callback"/> either asynchronously or in this function to cancel the
		/// download if desired.
		/// </param>
		/// <remarks>This may be called multiple times before and after <see cref="OnBeforeDownload"/>.</remarks>
		internal protected virtual void OnDownloadUpdated(CefBrowser browser, CefDownloadItem downloadItem, CefDownloadItemCallback callback)
		{

		}
	}
}
