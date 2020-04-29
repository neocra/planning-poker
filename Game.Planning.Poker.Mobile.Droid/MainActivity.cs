using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Game.Planning.Poker.Mobile.Domain;
using Game.Planning.Poker.Mobile.Infrastructure;
using Lottie.Forms.Droid;
using Pattern.Core.Interfaces;
using Xamarin.Forms;
using Pattern.Config;
using Pattern.Xamarin.Config;
using Pattern.Xamarin.Config.platform.android;
using ZXing;
using ZXing.Mobile;

namespace Game.Planning.Poker.Mobile.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/MainTheme", MainLauncher = true)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            Xamarin.Forms.Forms.Init(this, savedInstanceState);
            AnimationViewRenderer.Init();

            this.LoadApplication(new App(this.LoadKernel));
        }

        private void LoadKernel(IKernel obj)
        {
            obj.Bind<IQrCodeScan>().ToMethod(() => new QrCodeScan(this));
            obj.LoadConfig<ConfigReader<AppConfig>, AppConfig>(new ConfigReader<AppConfig>(this)
            {
                Filename = "appsettings.json"
            });
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult (requestCode, permissions, grantResults);           
        }
    }

    internal class QrCodeScan : IQrCodeScan
    {
        private Context context;

        public QrCodeScan(Context context)
        {
            this.context = context;
        }

        public async Task<string> Scan()
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            var result = await scanner.Scan(this.context, new MobileBarcodeScanningOptions()
            {
                PossibleFormats = new List<BarcodeFormat>() {BarcodeFormat.QR_CODE}
            });

            return result.Text;
        }
    }
}