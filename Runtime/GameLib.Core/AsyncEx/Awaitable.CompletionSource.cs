namespace GameLib.Core.AsyncEx
{
    using System;
    using System.Diagnostics;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
#if UNITY_2018_1_OR_NEWER
    using Cysharp.Threading.Tasks;
    using Cysharp.Threading.Tasks.CompilerServices;
#else
    using System.Threading.Tasks;
    using System.Threading;
#endif

    public class AwaitableCompletionSource<T>
#if UNITY_2017_1_OR_NEWER
    :  IUniTaskSource<T>, IPromise<T>
#endif

    {
#if UNITY_2018_1_OR_NEWER
        public UniTaskCompletionSource<T> _originalSource;
#else
        public TaskCompletionSource<T> _originalSource;
#endif


        public Awaitable<T> Task { get; }


        public AwaitableCompletionSource()
        {
#if UNITY_2018_1_OR_NEWER
            _originalSource = new UniTaskCompletionSource<T>();
#else
            _originalSource = new TaskCompletionSource<T>();
#endif
        }

        [DebuggerHidden]
        public bool TrySetResult(T result) 
            => _originalSource.TrySetResult(result);

        [DebuggerHidden]
        public bool TrySetCanceled(CancellationToken cancellationToken = default)
            => _originalSource.TrySetCanceled(cancellationToken);

        [DebuggerHidden]
        public bool TrySetException(Exception exception) 
            => _originalSource.TrySetException(exception);

#if UNITY_2017_1_OR_NEWER
        [DebuggerHidden]
        void IUniTaskSource.GetResult(short token)
        {
            ((IUniTaskSource)_originalSource).GetResult(token);
        }
#endif
    }

}