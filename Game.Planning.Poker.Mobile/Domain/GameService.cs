using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Game.Planning.Poker.Mobile.Domain
{
    public class GameService
    {
        private List<Player> players = new List<Player>();
        
        private readonly GameConnection gameConnection;
        private Func<Player, Task> updatePlayer;

        private Player currentPlayer;
        private Func<Player, int, Task> updateVote;

        public GameService(GameConnection gameConnection)
        {
            this.gameConnection = gameConnection;
        }

        public void CallbackUpdatePlayer(Func<Player, Task> updatePlayer)
        {
            this.updatePlayer = updatePlayer;
            this.gameConnection.CallbackUpdatePlayer(this.internalUpdatePlayer);
        }

        private Task internalUpdatePlayer(Player player)
        {
            this.players.Remove(player);
            
            this.players.Add(player);
            
            return this.updatePlayer(player);
        }

        public async Task StartGameAsync(string bareCodeValue, Player player)
        {
            this.currentPlayer = player;
            await this.gameConnection.StartGameAsync(bareCodeValue, player);
        }

        public async Task StartTurn()
        {
            await this.gameConnection.StartTurn();
        }

        public List<Player> GetPlayers()
        {
            return this.players;
        }

        public async Task Vote(int value)
        {
            await this.gameConnection.VoteAsync(this.currentPlayer, value);
        }

        public void CallbackVote(Func<Player, int, Task> updateVote)
        {
            this.updateVote = updateVote;
            this.gameConnection.CallbackVote(this.internalVote);
        }

        private Task internalVote(Player player, int vote)
        {            
            return this.updateVote(player, vote);
        }
    }
}