using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Planning.Poker.Dto;
using Microsoft.AspNetCore.SignalR.Client;
using Pattern.Tasks;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile.Domain
{
    public class GameConnection
    {
        private readonly HubConnection connection;

        private Func<Player, Task> newPlayer;
        private Func<Task> startTurn;
        private Func<Task> displayCallback;
        private Func<Player, int, Task> newVote;
        private string gameCode;

        public GameConnection()
        {
            this.connection = new HubConnectionBuilder()
//                .WithUrl("http://10.1.1.97:5000/pokerhub")
                .WithUrl("http://10.10.1.36:5000/pokerhub")
                .Build();

            this.connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
                await this.connection.StartAsync();
            };

            this.connection.On<PlayerDto>("UpdatePlayer", (playerDto) =>
            {
                var player = new Player
                {
                    Id = playerDto.Id,
                    Name = playerDto.Name,
                };
                
                Device.BeginInvokeOnMainThread(() => this.newPlayer(player).Fire());
            });
            
            this.connection.On<PlayerDto, int>("Vote", (playerDto, vote) =>
            {
                var player = new Player
                {
                    Id = playerDto.Id,
                    Name = playerDto.Name,
                };
                
                Device.BeginInvokeOnMainThread(() => this.newVote(player, vote).Fire());
            });
            
            this.connection.On("Display", () =>
            {                
                Device.BeginInvokeOnMainThread(() => this.displayCallback().Fire());
            });
            
            this.connection.On("StartTurn", () =>
            {                
                Device.BeginInvokeOnMainThread(() => this.startTurn().Fire());
            });       
        }

        public async Task StartGameAsync(string gameCode, Player player)
        {
            this.gameCode = gameCode;
            var playerDto = new PlayerDto
            {
                Id = player.Id, 
                Name = player.Name
            };
            
            await this.connection.StartAsync();
            await this.connection.InvokeAsync("JoinGame", gameCode, playerDto);
        }

        public void CallbackUpdatePlayer(Func<Player, Task> newPlayer)
        {
            this.newPlayer = newPlayer;
        }
        
        public async Task UpdatePlayerAsync(Player player)
        {
            var playerDto = new PlayerDto
            {
                Id = player.Id, 
                Name = player.Name
            };
            
            await this.connection.InvokeAsync("JoinGame", gameCode, playerDto);
        }

        public async Task VoteAsync(Player player, double vote)
        {
            await this.connection.InvokeAsync("Vote", this.gameCode, player, vote);
        }
                
        public void CallbackVote(Func<Player, int, Task> newVote)
        {
            this.newVote = newVote;
        }

        public void CallbackStartTurn(Func<Task> startTurn)
        {
            this.startTurn = startTurn;
        }

        public async Task StartTurn()
        {
            await this.connection.InvokeAsync("StartTurn", this.gameCode);
        }

        public void CallbackDisplay(Func<Task> displayCallback)
        {
            this.displayCallback = displayCallback;
        }

        public async Task Display()
        {
            await this.connection.InvokeAsync("Display", this.gameCode);
        }
    }
}