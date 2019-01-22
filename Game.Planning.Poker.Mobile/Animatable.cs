using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public static class Animatable
    {
        public static Task AnimateAsync(this IAnimatable animatable, Action<double> callback, double start, double end, uint length)
        {
            var taskSource = new TaskCompletionSource<bool>();
            animatable.Animate(
                "GridHeight",
                callback,
                start,
                end,
                easing: Easing.CubicOut,
                length: length,
                finished: (d, b) => taskSource.SetResult(true));
            return taskSource.Task;
        }
    }
}