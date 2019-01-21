using Pattern.Config;
using Pattern.Core.Interfaces;
using Pattern.Module;
using Xamarin.Essentials;

namespace Game.Planning.Poker.Mobile.Domain
{
    public class ServiceModule : IModule
    {
        public void Load(IKernel kernel)
        {
            kernel.Bind<IDeviceDisplay>().To<DeviceDisplayImplementation>();
        }
    }

    public class DeviceDisplayImplementation : IDeviceDisplay
    {
        public DisplayInfo MainDisplayInfo => DeviceDisplay.MainDisplayInfo;
    }
}