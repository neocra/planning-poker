using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Domain;
using Pattern.Mvvm;
using Pattern.Mvvm.Forms;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class ScoreViewModel : PokerViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly GameService gameService;
        private IList<ScorePlayer> scorePlayers;

        public ScoreViewModel(INavigationService navigationService, GameService gameService)
        {
            this.navigationService = navigationService;
            this.gameService = gameService;
            this.ScorePlayers = new List<ScorePlayer>
            {
                new ScorePlayer{ Name= ""}
            };

            this.gameService.ClearCallback();
            this.gameService.CallbackUpdatePlayers(this.UpdatePlayersCallback);
            this.gameService.CallbackDisplay(this.DisplayCallback);
            this.gameService.CallbackStartTurn(this.StartTurnCallback);
        }

        public AsyncCommand NextTurnCommand => this.CreateCommand(this.NextTurnAsync);
        public AsyncCommand DisplayCommand => this.CreateCommand(this.DisplayAsync);

        public IList<ScorePlayer> ScorePlayers
        {
            get => this.scorePlayers;
            set => this.Set(ref this.scorePlayers, value);
        }
             
        public override Task InitAsync()
        {                                    
            this.ScorePlayers = this.gameService.GetPlayers();
                     
            return base.InitAsync();
        }

        private Task UpdatePlayersCallback()
        {
            this.ScorePlayers = this.gameService.GetPlayers();
            return Task.CompletedTask;
        }

        private Task DisplayCallback()
        {
            this.ScorePlayers = this.gameService.GetPlayers();            
            return Task.CompletedTask;
        }
            
        private Task DisplayAsync()
        {
            return this.gameService.Display();
        }

        private async Task NextTurnAsync()
        {
            await this.gameService.StartTurn();
            await this.navigationService.Navigate(typeof(GamePage));
        }

        private async Task StartTurnCallback()
        {
            await this.navigationService.Navigate(typeof(GamePage));
        }
    }
}