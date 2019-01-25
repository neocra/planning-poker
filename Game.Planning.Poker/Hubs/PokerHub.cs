using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Game.Planning.Poker.Dto;
using Microsoft.AspNetCore.SignalR;

namespace Game.Planning.Poker.Hubs
{
    public class PokerHub: Hub
    {
        private static Dictionary<string, string> groups = new Dictionary<string, string>();
        
        public async Task StartTurn(string gameCode)
        {
            await this.Clients
                .GroupExcept(gameCode, this.Context.ConnectionId)
                .SendAsync("StartTurn");
        }
        
        public async Task UpdatePlayer(string gameCode, PlayerDto playerDto)
        {
            playerDto.ConnectionId = this.Context.ConnectionId;
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
            if (groups.ContainsKey(this.Context.ConnectionId))
            {
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groups[this.Context.ConnectionId]);
                groups[this.Context.ConnectionId] = gameCode;
            }
            else
            {
                groups.Add(this.Context.ConnectionId, gameCode);
            }
            
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, gameCode);
            playerDto.ConnectionId = this.Context.ConnectionId;
            await this.Clients
                .GroupExcept(gameCode, this.Context.ConnectionId)
                .SendAsync("UpdatePlayer", playerDto);
        }
        
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (groups.ContainsKey(this.Context.ConnectionId))
            {
                var groupName = groups[this.Context.ConnectionId];
                await this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, groupName);
                groups.Remove(this.Context.ConnectionId);
                await this.Clients
                    .GroupExcept(groupName, this.Context.ConnectionId)
                    .SendAsync("RemovePlayer", this.Context.ConnectionId);
            }
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}