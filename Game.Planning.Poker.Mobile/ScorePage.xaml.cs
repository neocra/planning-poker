using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
