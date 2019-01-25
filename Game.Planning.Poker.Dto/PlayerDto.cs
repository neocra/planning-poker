using System;

namespace Game.Planning.Poker.Dto
{
    public class PlayerDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }
    }
}