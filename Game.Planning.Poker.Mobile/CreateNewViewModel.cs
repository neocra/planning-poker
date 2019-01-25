using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Domain;
using Pattern.Mvvm;
using Pattern.Mvvm.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class CreateNewViewModel : PokerViewModelBase
    {
        private readonly GameService gameService;
        private readonly INavigationService navigationService;
        private string bareCodeValue;
        private ObservableCollection<ScorePlayer> players = new ObservableCollection<ScorePlayer>();
        private bool backOnStart;

        public CreateNewViewModel(GameService gameService, INavigationService navigationService)
        {
            this.gameService = gameService;
            this.navigationService = navigationService;
            this.gameService.ClearCallback();
            this.gameService.CallbackUpdatePlayers(this.UpdatePlayers);
            this.gameService.CallbackStartTurn(this.StartTurn);
        }
        
        public AsyncCommand StartCommand => this.CreateCommand(this.StartCommandAsync);

        public string BareCodeValue
        {
            get => this.bareCodeValue;
            set => this.Set(ref this.bareCodeValue, value);
        }

        public ObservableCollection<ScorePlayer> Players
        {
            get => this.players;
            set => this.Set(ref this.players, value);
        }

        public override async Task InitAsync()
        {
            this.backOnStart = await this.navigationService.GetParameter<bool>();
            
            this.BareCodeValue = this.gameService.GetGameCode();

            this.Players.Update(this.gameService.GetPlayers());
        }

        private async Task StartTurn()
        {
            await this.navigationService.Navigate(typeof(GamePage));                
        }

        public Task UpdatePlayers()
        {
            this.Players.Update(this.gameService.GetPlayers());

            return Task.CompletedTask;
        }

        private async Task StartCommandAsync()
        {
            if (this.backOnStart)
            {
                await this.navigationService.NavigateBack();
            }
            else
            {
                await this.gameService.StartTurn();
                await this.navigationService.Navigate(typeof(GamePage));
            }
        }
    }
}