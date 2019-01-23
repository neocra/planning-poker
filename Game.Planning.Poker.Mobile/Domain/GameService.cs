using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Pattern.Mvvm;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile.Domain
{
    public class GameService
    {
        private readonly List<ScorePlayer> scorePlayers = new List<ScorePlayer>();
        
        private readonly GameConnection gameConnection;
        private Func<Task> updatePlayers;
        private Func<Task> startTurn;
        private Func<Task> display;

        private Player currentPlayer;
        private bool master;
        private string gameCode;

        public GameService(GameConnection gameConnection)
        {
            this.gameConnection = gameConnection;
            this.gameConnection.CallbackVote(this.InternalVote);
            this.gameConnection.CallbackUpdatePlayer(this.InternalUpdatePlayer);
            this.gameConnection.CallbackStartTurn(this.InternalStartTurn);
            this.gameConnection.CallbackDisplay(this.InternalDisplayCallback);
        }

        private async Task InternalDisplayCallback()
        {
            foreach (var scorePlayer in this.scorePlayers)
            {
                scorePlayer.Show = true;
            }

            if (this.display != null)
            {
                await this.display();
            }
        }

        public void CallbackUpdatePlayers(Func<Task> updatePlayers)
        {
            this.updatePlayers = updatePlayers;
        }
        
        public void CallbackStartTurn(Func<Task> startTurn)
        {
            this.startTurn = startTurn;
        }
        
        public void CallbackDisplay(Func<Task> displayCallback)
        {
            this.display = displayCallback;
        }
        
        private async Task InternalStartTurn()
        {
            foreach (var scorePlayer in this.scorePlayers)
            {
                scorePlayer.Show = false;
                scorePlayer.Score = null;
            }
            
            if (this.startTurn != null)
            {
                await this.startTurn?.Invoke();
            }
        }

        private async Task InternalUpdatePlayer(Player playerUpdate)
        {
            this.UpdateOrAddPlayer(playerUpdate);

            if (this.master)
            {
                foreach (var player in this.scorePlayers)
                {
                    await this.gameConnection.UpdatePlayerAsync(new Player() {Id = player.Id, Name = player.Name});
                }
            }

            if (this.updatePlayers != null)
            {
                await this.updatePlayers();                
            }
        }

        private void UpdateOrAddPlayer(Player playerUpdate)
        {
            var hasSet = false;
            foreach (var scorePlayer in this.scorePlayers)
            {
                if (playerUpdate.Id == scorePlayer.Id)
                {
                    scorePlayer.Name = playerUpdate.Name;
                    hasSet = true;
                }
            }

            if (!hasSet)
            {
                this.scorePlayers.Add(new ScorePlayer()
                {
                    Id = playerUpdate.Id,
                    Name = playerUpdate.Name
                });
            }
        }

        public void SetPlayer(Player player)
        {
            this.currentPlayer = player;
            this.UpdateOrAddPlayer(player);
        }

        public async Task StartGameAsync(string bareCodeValue, bool master = false)
        {
            this.master = master;
            this.gameCode = bareCodeValue; 
            await this.gameConnection.StartGameAsync(bareCodeValue, this.currentPlayer);
        }

        public async Task StartTurn()
        {
            foreach (var scorePlayer in this.scorePlayers)
            {
                scorePlayer.Show = false;
                scorePlayer.Score = null;
            }

            await this.gameConnection.StartTurn();
        }

        public List<ScorePlayer> GetPlayers()
        {
            return this.scorePlayers;
        }

        public async Task Vote(double value)
        {
            await Vote(this.currentPlayer, value);

            await this.gameConnection.VoteAsync(this.currentPlayer, value);
        }

        private Task Vote(Player player, double vote)
        {
            foreach (var scorePlayer in this.scorePlayers)
            {
                if (scorePlayer.Id == player.Id)
                {
                    scorePlayer.Score = this.GetScores(null).FirstOrDefault(s => Math.Abs(s.Value - vote) < 0.1);
                }
            }

            if (this.scorePlayers.All(s => s.Score != null))
            {
                foreach (var scorePlayer in this.scorePlayers)
                {
                    scorePlayer.Show = true;
                }                
            }

            return Task.CompletedTask;
        }

        private async Task InternalVote(Player player, double vote)
        {
            await this.Vote(player, vote);
            
            if (this.updatePlayers != null)
            {
                await this.updatePlayers();                
            }
        }

        public string GetGameCode()
        {
            return this.gameCode;
        }
     
        public Task Display()
        {
            foreach (var scorePlayer in this.scorePlayers)
            {
                scorePlayer.Show = true;
            }

            return this.gameConnection.Display();
        }
                
        private Color GetColor(double value)
        {
            switch (value)
            {
                case -1:
                    return Color.FromRgb(107, 113, 113);
                case 0:
                    return Color.FromRgb(107, 113, 113);
                case 0.5:
                    return Color.FromRgb(95, 31, 117);
                case 1:
                    return Color.FromRgb(189, 40, 104);
                case 2:
                    return Color.FromRgb(83, 189, 249);
                case 3:
                    return Color.FromRgb(21, 59, 166);
                case 5:
                    return Color.FromRgb(45, 143, 34);
                case 8:
                    return Color.FromRgb(196, 172, 1);
                case 13:
                    return Color.FromRgb(201, 109, 0);
                case 20:
                    return Color.FromRgb(198, 41, 22);
                case 40:
                    return Color.FromRgb(121, 32, 26);
                case 100:
                    return Color.FromRgb(11, 46, 39);
                case 1000:
                    return Color.FromRgb(9, 10, 12);
                default:
                    return Color.Black;
            }
        }
        private string GetName(double value)
        {
            switch (value)
            {
                case 0.5:
                    return "1/2";
                case 1000:
                    return "?";
                default:
                    return value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public List<Score> GetScores(AsyncCommand<Score> voteCommand)
        {
            return new[] { 0, 0.5, 1, 2, 3, 5, 8, 13, 20, 40, 100, 1000 }
                .Select(s => new Score
                {
                    Name = this.GetName(s), 
                    Value = s, 
                    Color = this.GetColor(s),
                    VoteCommand = voteCommand
                })
                .ToList();
        }

        public void ClearCallback()
        {
            this.updatePlayers = null;
            this.display = null;
            this.startTurn = null;
        }
    }
}