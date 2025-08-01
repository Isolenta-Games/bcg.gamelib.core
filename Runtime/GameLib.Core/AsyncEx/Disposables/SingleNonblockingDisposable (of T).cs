﻿using System;
using GameLib.Core.AsyncEx.Disposables.Internals;

namespace GameLib.Core.AsyncEx.Disposables
{
	/// <summary>
	/// A base class for disposables that need exactly-once semantics in a threadsafe way.
	/// </summary>
	/// <typeparam name="T">The type of "context" for the derived disposable. Since the context should not be modified, strongly consider making this an immutable type.</typeparam>
	/// <remarks>
	/// <para>If <see cref="Dispose()"/> is called multiple times, only the first call will execute the disposal code. Other calls to <see cref="Dispose()"/> will not wait for the disposal to complete.</para>
	/// </remarks>
	public abstract class SingleNonblockingDisposable<T> : IDisposable
	{
		/// <summary>
		/// The context. This is never <c>null</c>. This is empty if this instance has already been disposed (or is being disposed).
		/// </summary>
		private readonly BoundActionField<T> _context;

		/// <summary>
		/// Creates a disposable for the specified context.
		/// </summary>
		/// <param name="context">The context passed to <see cref="Dispose(T)"/>.</param>
		protected SingleNonblockingDisposable(T context)
		{
			_context = new BoundActionField<T>(Dispose, context);
		}

		/// <summary>
		/// Whether this instance has been disposed (or is being disposed).
		/// </summary>
		public bool IsDisposed => _context.IsEmpty;

		/// <summary>
		/// The actul disposal method, called only once from <see cref="Dispose()"/>.
		/// </summary>
		/// <param name="context">The context for the disposal operation.</param>
		protected abstract void Dispose(T context);

		/// <summary>
		/// Disposes this instance.
		/// </summary>
		/// <remarks>
		/// <para>If <see cref="Dispose()"/> is called multiple times, only the first call will execute the disposal code. Other calls to <see cref="Dispose()"/> will not wait for the disposal to complete.</para>
		/// </remarks>
		public void Dispose() => _context.TryGetAndUnset()?.Invoke();

		/// <summary>
		/// Attempts to update the stored context. This method returns <c>false</c> if this instance has already been disposed (or is being disposed).
		/// </summary>
		/// <param name="contextUpdater">The function used to update an existing context. This may be called more than once if more than one thread attempts to simultanously update the context.</param>
		protected bool TryUpdateContext(Func<T, T> contextUpdater) => _context.TryUpdateContext(contextUpdater);
	}
}
