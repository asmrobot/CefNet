using System.Threading;
using System.Threading.Tasks;

namespace CefNet.Internal
{
	internal sealed class CefNetStringVisitor : CefStringVisitor
	{
		private readonly TaskCompletionSource<string> _completion;

		public CefNetStringVisitor()
		{
			_completion = new TaskCompletionSource<string>((TaskCreationOptions)64); // RunContinuationsAsynchronously
		}

		protected internal override void Visit(string @string)
		{
			_completion.TrySetResult(@string);
		}

		public async Task<string> GetAsync(CancellationToken cancellationToken)
		{
			using (cancellationToken.Register(Cancel))
			{
				return await _completion.Task.ConfigureAwait(false);
			}
		}

		private void Cancel()
		{
			_completion.TrySetCanceled();
		}

	}
}
