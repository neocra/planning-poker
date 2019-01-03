﻿using Foundation;
using Game.Planning.Poker.Mobile.Infrastructure;
using Pattern.Config;
using Pattern.Core.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Game.Planning.Poker.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        // class-level declarations

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
           
            ZXing.Net.Mobile.Forms.iOS.Platform.Init();

            Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.Init();

            this.LoadApplication(new App(this.LoadKernel));

            return base.FinishedLaunching(application, launchOptions);
        }

        private void LoadKernel(IKernel obj)
        {
            obj.Bind<IQrCodeScan>().To<QrCodeScan>();
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}