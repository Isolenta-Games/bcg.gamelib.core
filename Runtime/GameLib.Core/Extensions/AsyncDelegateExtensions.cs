#if UNITY_2020_3_OR_NEWER
#define HAS_UNITASK
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#if HAS_UNITASK
using Cysharp.Threading.Tasks;
#endif

namespace GameLib.Core.Extensions
{
	public static class AsyncDelegateExtensions
	{
		public static async Task WaitSequenceAsync(this Delegate call, params object[] args)
		{
			if (call == null)
			{
				return;
			}

			var list = call.GetInvocationList();

			foreach (var func in list)
			{
				var obj = func.DynamicInvoke(args);

				await WaitInternalAsync(obj);
			}
		}

		private static async Task WaitInternalAsync(object obj)
		{
			switch (obj)
			{
				case Task task:
					await task;
					break;
#if HAS_UNITASK
				case UniTask uniTask:
					await uniTask;
					break;
#endif
				default:
					throw new Exception($"Dont know how await {obj.GetType().FullName}");
			}
		}

		public static async Task WaitWhenAllAsync(this Delegate call, params object[] args)
		{
			if (call == null)
			{
				return;
			}

			var list = call.GetInvocationList();
			var tasks = new List<Task>(list.Length);
			tasks.AddRange(list.Select(func => WaitInternalAsync(func.DynamicInvoke(args))));

			await Task.WhenAll(tasks);
		}
	}
}