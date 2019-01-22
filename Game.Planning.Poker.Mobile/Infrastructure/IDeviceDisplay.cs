using Xamarin.Essentials;

namespace Game.Planning.Poker.Mobile.Infrastructure
{
    public interface IDeviceDisplay
    {
        DisplayInfo MainDisplayInfo { get; }
    }
}