using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameLib.Core.AsyncEx
{
	public class AsyncEvent
	{
		private UniTaskCompletionSource<bool> _tcs = new UniTaskCompletionSource<bool>();

		public void Reset()
		{
			var oldTcs = _tcs;
			oldTcs?.TrySetCanceled();

			_tcs = new UniTaskCompletionSource<bool>();
		}

		public void FireEvent()
		{
			Debug.LogError("FireEvent");
			_tcs.TrySetResult(true);
		}

		public void FireException(Exception ex)
		{
			_tcs.TrySetException(ex);
		}

		public async UniTask WaitAsync()
		{
			await _tcs.Task;
		}

		public async UniTask WaitAsync(CancellationToken ct)
		{
			ct.Register(state =>
			{
				var task = (UniTaskCompletionSource<bool>)state;
				task.TrySetCanceled(ct);
			}, _tcs);
			await _tcs.Task;
		}
		
	}
}