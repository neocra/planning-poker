using System;
using System.Diagnostics;
using Pattern.Core;
using Pattern.Core.Interfaces;
using Pattern.Module;
using Pattern.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Game.Forms
{
    public partial class App : Application
    {
        public App(Action<IKernel> configKernel)
        {
            this.InitializeComponent();

            new ErrorHandler().UseDefault();

            var kernel = new Kernel();

            kernel.LoadModule<PageModule>();
            kernel.LoadModule<ServiceModule>();
            configKernel(kernel);

            kernel.StartModules();

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

    public class ErrorHandler : IErrorHandler
    {
        public void Handle(Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
