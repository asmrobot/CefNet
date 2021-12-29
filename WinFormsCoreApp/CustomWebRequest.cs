using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CefNet;
using CefNet.Net;

namespace WinFormsCoreApp
{
	/// <summary>
	/// Provides an example of loading data into a file.
	/// </summary>
	internal sealed class CustomWebRequest : CefNetWebRequest
	{
		private string _filename;
		private long _total;

		public bool IgnoreSize { get; internal set; }

		public async Task DownloadFileAsync(CefRequest request, CefRequestContext context, string filename, CancellationToken cancellationToken)
		{
			if (!Directory.Exists(Path.GetDirectoryName(filename)))
				throw new DirectoryNotFoundException();

			_total = 0;
			_filename = filename;
			await SendAsync(request, context, cancellationToken).ConfigureAwait(false);
			GetResponseStream()?.Dispose();
		}

		protected override void OnDownloadData(CefUrlRequest request, IntPtr data, long dataLength)
		{
			_total += dataLength;
			try
			{
				ThrowIfMoreThan5MB(request, _total);
				base.OnDownloadData(request, data, dataLength);
			}
			catch (Exception e)
			{
				SetException(e);
				request.Cancel();
			}
		}

		protected override void OnDownloadProgress(CefUrlRequest request, long current, long total)
		{
			try
			{
				ThrowIfMoreThan5MB(request, total);
				base.OnDownloadProgress(request, current, total);
			}
			catch (Exception e)
			{
				SetException(e);
				request.Cancel();
			}
		}

		private void ThrowIfMoreThan5MB(CefUrlRequest request, long total)
		{
			if (total > 5 * 1024 * 1024 && !IgnoreSize)
				throw new OperationCanceledException("The file size more than 5 MB.");
		}

		protected override Stream CreateResourceStream(int initialCapacity)
		{
			return File.Open(_filename, FileMode.Create, FileAccess.ReadWrite);
		}


	}
}
