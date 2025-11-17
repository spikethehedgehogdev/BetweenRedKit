using System.Threading;
using BetweenRedKit.Core;
using Cysharp.Threading.Tasks;

namespace BetweenRedKit.Integrations.Cysharp.Integrations.CySharp
{
    /// <summary>
    /// Provides UniTask awaiters for <see cref="BetweenHandle"/> completion.
    /// </summary>
    /// <remarks>
    /// This integration allows Between tweens to be awaited using async/await syntax
    /// without generating garbage or relying on coroutines.
    /// </remarks>
    public static class BetweenAwaiters
    {
        /// <summary>
        /// Returns a <see cref="UniTask"/> that completes when the tween finishes.
        /// </summary>
        /// <param name="handle">The tween handle to await.</param>
        /// <returns>A <see cref="UniTask"/> that resolves upon tween completion.</returns>
        public static UniTask AwaitCompletion(this BetweenHandle handle)
        {
            var tcs = new UniTaskCompletionSource();
            if (!handle.IsActive)
            {
                tcs.TrySetResult();
                return tcs.Task;
            }

            handle.OnComplete(() => tcs.TrySetResult());
            return tcs.Task;
        }

        /// <summary>
        /// Returns a cancellable <see cref="UniTask"/> that completes when the tween finishes.
        /// </summary>
        /// <param name="handle">The tween handle to await.</param>
        /// <param name="token">Cancellation token used to stop awaiting.</param>
        /// <remarks>
        /// If the token is cancelled before the tween completes,
        /// the returned task transitions to the canceled state.
        /// </remarks>
        public static async UniTask AwaitCompletion(this BetweenHandle handle, CancellationToken token)
        {
            if (!handle.IsActive) return;

            var tcs = new UniTaskCompletionSource();

            await using (token.Register(() => tcs.TrySetCanceled()))
            {
                handle.OnComplete(() => tcs.TrySetResult());
                await tcs.Task;
            }
        }
    }
}