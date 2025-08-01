﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameLib.Core.AsyncEx
{
	/// <summary>
	/// Provides synchronous extension methods for tasks.
	/// </summary>
	public static class SynchronousTaskExtensions
	{
		/// <summary>
		/// Waits for the task to complete, unwrapping any exceptions.
		/// </summary>
		/// <param name="task">The task. May not be <c>null</c>.</param>
		public static void WaitAndUnwrapException(this Task task)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			task.GetAwaiter().GetResult();
		}

		/// <summary>
		/// Waits for the task to complete, unwrapping any exceptions.
		/// </summary>
		/// <param name="task">The task. May not be <c>null</c>.</param>
		/// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
		/// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> was cancelled before the <paramref name="task"/> completed, or the <paramref name="task"/> raised an <see cref="OperationCanceledException"/>.</exception>
		public static void WaitAndUnwrapException(this Task task, CancellationToken cancellationToken)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			try
			{
				task.Wait(cancellationToken);
			}
			catch (AggregateException ex)
			{
				throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
			}
		}

		/// <summary>
		/// Waits for the task to complete, unwrapping any exceptions.
		/// </summary>
		/// <typeparam name="TResult">The type of the result of the task.</typeparam>
		/// <param name="task">The task. May not be <c>null</c>.</param>
		/// <returns>The result of the task.</returns>
		public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			return task.GetAwaiter().GetResult();
		}

		/// <summary>
		/// Waits for the task to complete, unwrapping any exceptions.
		/// </summary>
		/// <typeparam name="TResult">The type of the result of the task.</typeparam>
		/// <param name="task">The task. May not be <c>null</c>.</param>
		/// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
		/// <returns>The result of the task.</returns>
		/// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> was cancelled before the <paramref name="task"/> completed, or the <paramref name="task"/> raised an <see cref="OperationCanceledException"/>.</exception>
		public static TResult WaitAndUnwrapException<TResult>(this Task<TResult> task, CancellationToken cancellationToken)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			try
			{
				task.Wait(cancellationToken);
				return task.Result;
			}
			catch (AggregateException ex)
			{
				throw ExceptionHelpers.PrepareForRethrow(ex.InnerException);
			}
		}

		/// <summary>
		/// Waits for the task to complete, but does not raise task exceptions. The task exception (if any) is unobserved.
		/// </summary>
		/// <param name="task">The task. May not be <c>null</c>.</param>
		public static void WaitWithoutException(this Task task)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			try
			{
				task.Wait();
			}
			catch (AggregateException)
			{
			}
		}

		/// <summary>
		/// Waits for the task to complete, but does not raise task exceptions. The task exception (if any) is unobserved.
		/// </summary>
		/// <param name="task">The task. May not be <c>null</c>.</param>
		/// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
		/// <exception cref="OperationCanceledException">The <paramref name="cancellationToken"/> was cancelled before the <paramref name="task"/> completed.</exception>
		public static void WaitWithoutException(this Task task, CancellationToken cancellationToken)
		{
			if (task == null)
				throw new ArgumentNullException(nameof(task));
			try
			{
				task.Wait(cancellationToken);
			}
			catch (AggregateException)
			{
				cancellationToken.ThrowIfCancellationRequested();
			}
		}
	}
}
