using Xamarin.Essentials;

namespace Game.Planning.Poker.Mobile.Infrastructure
{
    public class DeviceDisplayImplementation : IDeviceDisplay
    {
        public DisplayInfo MainDisplayInfo => DeviceDisplay.MainDisplayInfo;
    }
}