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
        private ObservableCollection<ScorePlayer> scorePlayers = new ObservableCollection<ScorePlayer>();
        private double? average;
        private double? averageFibo;

        public ScoreViewModel(INavigationService navigationService, GameService gameService)
        {
            this.navigationService = navigationService;
            this.gameService = gameService;

            this.gameService.ClearCallback();
            this.gameService.CallbackUpdatePlayers(this.UpdatePlayersCallback);
            this.gameService.CallbackDisplay(this.DisplayCallback);
            this.gameService.CallbackStartTurn(this.StartTurnCallback);
        }

        public double? Average
        {
            get => this.average;
            set => this.Set(ref this.average, value);
        }

        public double? AverageFibo
        {
            get => this.averageFibo;
            set => this.Set(ref this.averageFibo, value);
        }

        public AsyncCommand NextTurnCommand => this.CreateCommand(this.NextTurnAsync);
        public AsyncCommand DisplayCommand => this.CreateCommand(this.DisplayAsync);

        public ObservableCollection<ScorePlayer> ScorePlayers
        {
            get => this.scorePlayers;
            set => this.Set(ref this.scorePlayers, value);
        }
             
        public override Task InitAsync()
        {                                    
            this.ScorePlayers.Update(this.gameService.GetPlayers());
                     
            return base.InitAsync();
        }

        private Task UpdatePlayersCallback()
        {
            this.ScorePlayers.Update(this.gameService.GetPlayers());
            return Task.CompletedTask;
        }

        private double[] fibo = new[] {0, 0.5, 1, 2, 3, 5, 8, 13, 20, 40, 100, 1000};

        private Task DisplayCallback()
        {
            var players = this.gameService.GetPlayers();
            this.ScorePlayers.Update(players);

            var scores = players
                .Where(p => p.Score != null && p.Score.Value != -1)
                .ToList();
            
            this.Average = scores
                .Average(s => s.Score.Value);

            var minFibo = this.fibo.Where(p => p<=this.Average).Max();
            var maxFibo = this.fibo.Where(p => p>=this.Average).Min();

            if (this.Average - minFibo < maxFibo - this.Average)
            {
                this.AverageFibo = minFibo;
            }
            else
            {
                this.AverageFibo = maxFibo;                
            }
            
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