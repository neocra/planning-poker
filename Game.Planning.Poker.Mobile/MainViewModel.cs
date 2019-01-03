using System;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Domain;
using Pattern.Mvvm;
using Pattern.Mvvm.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class MainViewModel : PokerViewModelBase
    {
        private string fullName;
        private readonly INavigationService navigationService;
        private readonly GameService gameService;

        public MainViewModel(INavigationService navigationService, GameService gameService)
        {
            this.navigationService = navigationService;
            this.gameService = gameService;
        }
        
        public AsyncCommand UpdateCommand => this.CreateCommand(this.UpdateAsync);

        public string FullName
        {
            get => this.fullName;
            set =>  this.Set(ref this.fullName, value);
        }

        private Task UpdateAsync()
        {
            this.gameService.SetPlayer(new Player()
            {
                Id = Guid.NewGuid(),
                Name = this.FullName
            });
            
            return this.navigationService.Navigate(typeof(JoinPage));
        }
    }
}