using Pattern.Mvvm;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile.Domain
{
    public class Score
    {
        public string Name { get; set; }

        public double Value { get; set; }

        public AsyncCommand<Score> VoteCommand { get; set; }
        
        public Color Color { get; set; }
    }
}