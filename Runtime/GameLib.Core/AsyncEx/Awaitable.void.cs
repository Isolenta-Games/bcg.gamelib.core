namespace GameLib.Core.AsyncEx
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Diagnostics;
#if UNITY_2018_1_OR_NEWER
    using Cysharp.Threading.Tasks;
    using Cysharp.Threading.Tasks.CompilerServices;
#else
    using System.Threading.Tasks;
#endif

#if UNITY_2018_1_OR_NEWER
    internal sealed class AsyncMethodBuilderAttribute : Attribute
    {
        public Type BuilderType { get; }

        public AsyncMethodBuilderAttribute(Type builderType) => this.BuilderType = builderType;
    }
#endif

#if NETSTANDARD2_0_OR_GREATER || (CSHARP_7_OR_LATER || (UNITY_2018_3_OR_NEWER && (NETSTANDARD2_0_OR_GREATER || NET_4_6)))
    [AsyncMethodBuilder(typeof(AsyncAwaitableMethodBuilder))]
    [StructLayout(LayoutKind.Auto)]
#endif
    public readonly struct Awaitable
    {
        internal readonly
#if UNITY_2018_1_OR_NEWER
            UniTask
#else
            Task
#endif
            rawTaskValue;

        
        public Awaitable(
#if UNITY_2018_1_OR_NEWER
            UniTask
#else
            Task
#endif
                rawTaskValue)
        {
            this.rawTaskValue = rawTaskValue;
        }
        [DebuggerHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public
#if UNITY_2018_1_OR_NEWER
            UniTask.Awaiter
#else
            TaskAwaiter
#endif
            GetAwaiter()
        {
            return rawTaskValue.GetAwaiter();
        }
    }



    public struct AsyncAwaitableMethodBuilder
    {
#if UNITY_2018_1_OR_NEWER
        private AsyncUniTaskMethodBuilder methodBuilder;
#else
        private AsyncTaskMethodBuilder methodBuilder;
#endif
        public static AsyncAwaitableMethodBuilder Create()
            => new AsyncAwaitableMethodBuilder
            {
#if UNITY_2018_1_OR_NEWER
                methodBuilder = AsyncUniTaskMethodBuilder.Create()
#else
                methodBuilder = AsyncTaskMethodBuilder.Create()
#endif
            };

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
            => methodBuilder.Start(ref stateMachine);

        public void SetStateMachine(IAsyncStateMachine stateMachine)
            => methodBuilder.SetStateMachine(stateMachine);
        

        public void SetException(Exception exception)
            => methodBuilder.SetException(exception);

        public Awaitable Task 
            => new Awaitable(methodBuilder.Task);

        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            methodBuilder.AwaitOnCompleted(ref awaiter, ref stateMachine);

        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine =>
            methodBuilder.AwaitUnsafeOnCompleted(ref awaiter, ref stateMachine);
    }

}