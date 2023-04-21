namespace GameLib.Core.AsyncEx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
#if UNITY_2018_1_OR_NEWER
    using Cysharp.Threading.Tasks;
    using Cysharp.Threading.Tasks.CompilerServices;
#else
    using System.Threading.Tasks;
#endif

#if NETSTANDARD2_0_OR_GREATER || (CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NETSTANDARD2_0_OR_GREATER || NET_4_6)))
    [AsyncMethodBuilder(typeof(AsyncAwaitableMethodBuilder<>))]
    [StructLayout(LayoutKind.Auto)]
#endif
    public readonly struct Awaitable<TResponse>
    {
        internal readonly bool hasRawValue;
        internal readonly TResponse rawValue;
        internal readonly
#if UNITY_2018_1_OR_NEWER
            UniTask<TResponse>
#else
            Task<TResponse>
#endif
            rawTaskValue;


        public Awaitable(TResponse rawValue)
        {
            this.hasRawValue = true;
            this.rawValue = rawValue;
            this.rawTaskValue = default;
        }

        public Awaitable(
#if UNITY_2018_1_OR_NEWER
            UniTask<TResponse>
#else
            Task<TResponse>
#endif
                rawTaskValue)
        {
            this.hasRawValue = false;
            this.rawValue = default;
            this.rawTaskValue = rawTaskValue;
        }

        public
#if UNITY_2018_1_OR_NEWER
            UniTask<TResponse>.Awaiter
#else
            TaskAwaiter<TResponse>
#endif
            GetAwaiter()
        {
            if (hasRawValue)
                return
#if UNITY_2018_1_OR_NEWER
                    UniTask.FromResult(rawValue)
#else
                    Task.FromResult(rawValue)
#endif
                        .GetAwaiter();
            return rawTaskValue.GetAwaiter();
        }
    }



    public struct AsyncAwaitableMethodBuilder<T>
    {
#if UNITY_2018_1_OR_NEWER
        private AsyncUniTaskMethodBuilder<T> methodBuilder;
#else
        private AsyncTaskMethodBuilder<T> methodBuilder;
#endif
        private T result;
        private bool haveResult;
        private bool useBuilder;

        public static AsyncAwaitableMethodBuilder<T> Create()
            => new AsyncAwaitableMethodBuilder<T>
            {
#if UNITY_2018_1_OR_NEWER
                methodBuilder = AsyncUniTaskMethodBuilder<T>.Create()
#else
                methodBuilder = AsyncTaskMethodBuilder<T>.Create()
#endif
            };

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
            => methodBuilder.Start(ref stateMachine);

        public void SetStateMachine(IAsyncStateMachine stateMachine)
            => methodBuilder.SetStateMachine(stateMachine);

        public void SetResult(T result)
        {
            if (useBuilder)
                methodBuilder.SetResult(result);
            else
            {
                this.result = result;
                haveResult = true;
            }
        }

        public void SetException(Exception exception)
            => methodBuilder.SetException(exception);

        public Awaitable<T> Task
        {
            get
            {
                if (haveResult)
                    return new Awaitable<T>(result);
                useBuilder = true;
                return new Awaitable<T>(methodBuilder.Task);
            }
        }

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            useBuilder = true;
            methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);
        }

        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            useBuilder = true;
            methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
        }
    }

}