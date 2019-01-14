using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace Game.Planning.Poker.Mobile.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
//            ZXing.Net.Mobile.Forms.iOS.Platform.Init();
//
//            Forms.SetFlags("CollectionView_Experimental");
//            Xamarin.Forms.Forms.Init();
//
//            this.LoadApplication(new App(this.LoadKernel));
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
        }
    }
}