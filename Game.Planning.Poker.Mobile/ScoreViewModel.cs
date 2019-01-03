using System.Threading.Tasks;
using Pattern.Mvvm;
using Pattern.Mvvm.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class MainViewModel : PokerViewModelBase
    {
        private readonly INavigationService navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
        
        public AsyncCommand CreateNewCommand => this.CreateCommand(this.CreateNewAsync);
        public AsyncCommand JoinCommand => this.CreateCommand(this.JoinAsync);

        private Task CreateNewAsync()
        {
            return this.navigationService.Navigate(typeof(CreateNewPage));
        }

        private Task JoinAsync()
        {
            return this.navigationService.Navigate(typeof(JoinPage));
        }
    }
}