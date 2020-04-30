using System;
using Game.Planning.Poker.Mobile.Domain;
using Game.Planning.Poker.Mobile.Infrastructure;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Pattern.Core;
using Pattern.Core.Interfaces;
using Pattern.Module;
using Pattern.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Game.Planning.Poker.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

        public App(Action<IKernel> configKernel)
        {
            new ErrorHandler().UseDefault();

           
            
            this.InitializeComponent();


            var kernel = new Kernel();

            kernel.LoadModule<PageModule>();
            kernel.LoadModule<ServiceModule>();
            configKernel(kernel);

            kernel.StartModules();
            
            var config = kernel.Get<AppConfig>();
            
            AppCenter.Start(config.AppCenter,
                typeof(Analytics), typeof(Crashes));
            
            this.MainPage = kernel.Get<NavigationPage>();

            var bootStrap = kernel.Get<BootStrap>();

            bootStrap.Start().Fire();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
