using System;

namespace Game.Planning.Poker.Mobile.Domain
{
    public class Player
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Player p)
            {
                return this.Id.Equals(p.Id);
            }

            return false;
        }
    }
}