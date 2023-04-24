using System;
using System.Threading;

namespace GameLib.Core.AsyncEx
{
	public class AsyncEvent
	{
		private AwaitableCompletionSource<bool> _tcs = new AwaitableCompletionSource<bool>();

		public void Reset()
		{
			var oldTcs = _tcs;
			oldTcs?.TrySetCanceled();

			_tcs = new AwaitableCompletionSource<bool>();
		}

		public void FireEvent()
		{
			_tcs.TrySetResult(true);
		}

		public void FireException(Exception ex)
		{
			_tcs.TrySetException(ex);
		}

		public async Awaitable WaitAsync()
		{
			await _tcs.Task;
		}

		public async Awaitable WaitAsync(CancellationToken ct)
		{
			ct.Register(state =>
			{
				var task = (AwaitableCompletionSource<bool>)state;
				task.TrySetCanceled(ct);
			}, _tcs);
			await _tcs.Task;
		}
		
	}
}