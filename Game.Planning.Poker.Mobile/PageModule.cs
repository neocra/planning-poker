using Game.Planning.Poker.Mobile.Domain;
using Game.Planning.Poker.Mobile.Infrastructure;
using Pattern.Config;
using Pattern.Core.Interfaces;
using Pattern.Module;
using Pattern.Mvvm.Forms;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class PageModule : IModule
    {
        void IModule.Load(IKernel kernel)
        {
            kernel.Bind<NavigationConfig>().ToMethod(() => new NavigationConfig
            {
                RootPage = typeof(MainPage)
            });

            kernel.Bind<INavigationService>().To<NavigationService>().InSingletonScope();
            kernel.Bind<NavigationPage>().ToSelf().InSingletonScope();
            kernel.Bind<INavigationHandler>().To<NavigationHandler>();

            kernel.BindViewModel<MainPage, MainViewModel>();
            kernel.BindViewModel<CreateNewPage, CreateNewViewModel>();
            kernel.BindViewModel<JoinPage, JoinViewModel>();
            kernel.BindViewModel<ScorePage, ScoreViewModel>();
            kernel.BindViewModel<GamePage, GameViewModel>();
            
            kernel.Bind<GameConnection>().ToSelf().InSingletonScope();
            kernel.Bind<GameService>().ToSelf().InSingletonScope();
        }
    }
}