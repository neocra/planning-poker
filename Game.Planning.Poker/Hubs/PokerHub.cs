using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Game.Planning.Poker.Dto;
using Microsoft.AspNetCore.SignalR;

namespace Game.Planning.Poker.Hubs
{
    public class PokerHub: Hub
    {
        public async Task StartTurn(string gameCode)
        {
            await this.Clients
                .GroupExcept(gameCode, this.Context.ConnectionId)
                .SendAsync("StartTurn");
        }
        
        public async Task UpdatePlayer(string gameCode, PlayerDto playerDto)
        {
            await this.Clients
                .GroupExcept(gameCode, this.Context.ConnectionId)
                .SendAsync("UpdatePlayer", playerDto);
        }
        
        public async Task Vote(string gameCode, PlayerDto playerDto, double number)
        {
            await this.Clients
                .GroupExcept(gameCode, this.Context.ConnectionId)
                .SendAsync("Vote", playerDto, number);
        }
        
        public async Task Display(string gameCode)
        {
            await this.Clients
                .GroupExcept(gameCode, this.Context.ConnectionId)
                .SendAsync("Display");
        }
        
        public async Task JoinGame(string gameCode, PlayerDto playerDto)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, gameCode);
            await this.Clients
                .GroupExcept(gameCode, this.Context.ConnectionId)
                .SendAsync("UpdatePlayer", playerDto);
        }
    }
}