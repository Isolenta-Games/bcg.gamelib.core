using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameLib.Core.AsyncEx
{
	public class AsyncEvent
	{
		private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();

		public void Reset()
		{
			var oldTcs = _tcs;
			oldTcs?.TrySetCanceled();

			_tcs = new TaskCompletionSource<bool>();
		}

		public void FireEvent()
		{
			_tcs.TrySetResult(true);
		}

		public void FireException(Exception ex)
		{
			_tcs.TrySetException(ex);
		}

		public async Task WaitAsync()
		{
			await _tcs.Task;
		}

		public async Task WaitAsync(CancellationToken ct)
		{
			await _tcs.Task.WaitAsync(ct);
		}
		
	}
}