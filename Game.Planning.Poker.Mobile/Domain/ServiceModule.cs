using Pattern.Config;
using Pattern.Core.Interfaces;
using Pattern.Module;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;

namespace Game.Planning.Poker.Mobile.Domain
{
    public class ServiceModule : IModule
    {
        public void Load(IKernel kernel)
        {
            kernel.Bind<IDeviceDisplay>().To<DeviceDisplayImplementation>();
        }
    }
}