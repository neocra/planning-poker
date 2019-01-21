using System;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Infrastructure;
using Pattern.Tasks;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public partial class JoinPage : ContentPage
    {
        public JoinPage(IDeviceDisplay deviceDisplay)
        {
            this.deviceDisplay = deviceDisplay;
            this.InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.Appearing += this.OnAppearing;
        }
        
        private void OnAppearing(object sender, EventArgs e)
        {
            this.Open().Fire();
        }

        private bool firstLoad = true;
        private readonly IDeviceDisplay deviceDisplay;

        private async Task Open()
        {
            if (!this.firstLoad)
            {
                return;
            }

            this.firstLoad = false;

            var start = this.deviceDisplay.MainDisplayInfo.Height / this.deviceDisplay.MainDisplayInfo.Density;
            var end = (this.deviceDisplay.MainDisplayInfo.Height / this.deviceDisplay.MainDisplayInfo.Density) / 2.0;
            await Task.WhenAll(
                this.TopGrid.AnimateAsync(d => this.TopGrid.HeightRequest = d, start, end, 1500),
                this.Label.FadeTo(1.0),
                this.Create.FadeTo(1.0),
                this.Scan.FadeTo(1.0));
        }
    }
}
