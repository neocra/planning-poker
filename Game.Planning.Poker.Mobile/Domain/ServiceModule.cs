using Game.Planning.Poker.Mobile.Infrastructure;
using Pattern.Config;
using Pattern.Core.Interfaces;
using Pattern.Module;

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