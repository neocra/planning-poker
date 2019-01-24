using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Domain;
using Pattern.Mvvm;
using Pattern.Mvvm.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class GameViewModel : PokerViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly GameService gameService;
        private IList<Score> scores;

        public GameViewModel(INavigationService navigationService, GameService gameService)
        {
            this.navigationService = navigationService;
            this.gameService = gameService;
            this.gameService.ClearCallback();
            this.gameService.CallbackDisplay(DisplayCallback);

            this.Scores = new List<Score>
            {
                new Score{ Name= "0"}
            };
        }

        private Task DisplayCallback()
        {
            return this.navigationService.Navigate(typeof(ScorePage));
        }

        public IList<Score> Scores
        {
            get => this.scores;
            set => this.Set(ref this.scores, value);
        }

        public AsyncCommand<Score> VoteCommand => this.CreateCommand<Score>(this.VoteAsync);

        private async Task VoteAsync(Score score)
        {
            await this.gameService.Vote(score.Value);
            await this.navigationService.Navigate(typeof(ScorePage));
        }

        public override Task InitAsync()
        {
            var scores = this.gameService.GetScores(this.VoteCommand);

            this.Scores = new ObservableCollection<Score>(scores);
                        
            return base.InitAsync();
        }
    }
}