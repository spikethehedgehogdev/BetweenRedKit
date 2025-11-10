using System.Threading;
using BetweenRedKit.Core;
using Cysharp.Threading.Tasks;

namespace BetweenRedKit.Integrations.Cysharp.Integrations.CySharp
{
    public static class BetweenAwaiters
    {
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