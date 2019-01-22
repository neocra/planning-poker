using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Domain;
using Game.Planning.Poker.Mobile.Infrastructure;
using Pattern.Config;
using Pattern.Core.Interfaces;
using Pattern.Module;
using Pattern.Mvvm.Forms;
using Pattern.Tasks;
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
    
    public class NavigationService : INavigationService
  {
    private readonly ConditionalWeakTable<object, Func<object, Task>> callbacks = new ConditionalWeakTable<object, Func<object, Task>>();
    private readonly IKernel kernel;
    private readonly NavigationPage navigationPage;
    private readonly INavigationHandler navigationHandler;
    private object parameter;

    public NavigationService(
      IKernel kernel,
      NavigationPage navigationPage,
      INavigationHandler navigationHandler)
    {
      this.kernel = kernel;
      this.navigationPage = navigationPage;
      this.navigationHandler = navigationHandler;
      this.navigationPage.Popped += this.NavigationPage_Popped;
    }

    public async Task Navigate(Type pageType)
    {
      var animated = true;
      if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
      {
        animated = false;
        await pageNavigateFrom.NavigateFrom(pageType);
      }
      
      Page page = this.ResolveView(pageType);
      this.navigationHandler?.Navigate(pageType.Name, page);

      await this.navigationPage.PushAsync(page, animated);
    }

    public async Task Navigate<T>(Type pageType, T parameterToNextViewModel)
    {
      var animated = true;
      if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
      {
        animated = false;
        await pageNavigateFrom.NavigateFrom(pageType);
      }

      this.parameter = (object) parameterToNextViewModel;
      Page page = this.ResolveView(pageType);
      this.navigationHandler?.Navigate(pageType.Name, page);
      await this.navigationPage.PushAsync(page, animated);
    }

    public async Task Navigate<T, TParameter>(
      Type pageType,
      Func<T, Task> callBackWhenViewBack,
      TParameter parameterToNextViewModel)
    {
      var animated = true;
      if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
      {
        animated = false;
        await pageNavigateFrom.NavigateFrom(pageType);
      }

      this.parameter = (object) parameterToNextViewModel;
      Page page = this.ResolveView(pageType);
      this.navigationHandler?.Navigate(pageType.Name, page);
      this.callbacks.Add(page.BindingContext, (Func<object, Task>) (o => callBackWhenViewBack((T) o)));
      await this.navigationPage.PushAsync(page, animated);
    }

    public async Task Navigate<T>(Type pageType, Func<T, Task> callBackWhenViewBack)
    {
      var animated = true;
      if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
      {
        animated = false;
        await pageNavigateFrom.NavigateFrom(pageType);
      }

      Page page = this.ResolveView(pageType);
      this.navigationHandler?.Navigate(pageType.Name, page);
      this.callbacks.Add(page.BindingContext, (Func<object, Task>) (o => callBackWhenViewBack((T) o)));
      await this.navigationPage.PushAsync(page, animated);
    }

    private void NavigationPage_Popped(object sender, NavigationEventArgs e)
    {
      Func<object, Task> func;
      if (!this.callbacks.TryGetValue(e.Page.BindingContext, out func))
        return;
      this.callbacks.Remove(e.Page.BindingContext);
      Pattern.Tasks.TaskExtensions.Fire(func(e.Page.BindingContext), (ILoadingHandler) null);
    }

    public async Task NavigateBack()
    {
      this.navigationHandler?.NavigateBack();
      Page page = await this.navigationPage.PopAsync(true);
      (this.navigationPage.CurrentPage.BindingContext as ViewModelBase)?.Resume();
    }

    private object Resolve(Type pageType)
    {
      return this.Instantiate(pageType);
    }

    private object Instantiate(Type pageType)
    {
      return KernelExtensions.Get(this.kernel, pageType);
    }

    private Page ResolveView(Type pageType)
    {
      return this.Resolve(pageType) as Page;
    }

    public async Task NavigateRoot(Type pageType)
    {
      if (this.navigationPage.CurrentPage is INavigateFrom pageNavigateFrom)
      {
        await pageNavigateFrom.NavigateFrom(pageType);
      }

      Page page = this.ResolveView(pageType);
      this.navigationHandler?.Navigate(pageType.Name, page);
      this.navigationPage.Navigation.InsertPageBefore(page, this.navigationPage.Navigation.NavigationStack.First<Page>());
      await this.navigationPage.PopToRootAsync(false);
    }

    public Task<T> GetParameter<T>()
    {
      return Task.FromResult<T>((T) this.parameter);
    }
  }

    public interface INavigateFrom
    {
      Task NavigateFrom(Type toPage);
    }
}