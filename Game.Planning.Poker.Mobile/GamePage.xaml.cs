using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public partial class GamePage : ContentPage
    {
        public GamePage()
        {
            this.InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vm = this.BindingContext as GameViewModel;
            
            vm.VoteCommand.Execute(e.Item);
        }
    }
}
