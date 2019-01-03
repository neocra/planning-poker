using System;
using Game.Planning.Poker.Mobile.Domain;
using Pattern.Mvvm;

namespace Game.Planning.Poker.Mobile
{
    public class ScorePlayer : ViewModelBase
    {
        private Score score;
        private bool show;
        public string Name { get; set; }

        public Score Score
        {
            get => this.score;
            set
            {
                this.Set(ref this.score, value);
                this.RaisePropertyChanged(nameof(this.ShowHasScore));
            }
        }

        public bool ShowHasScore => this.Score != null && !this.Show;

        public Guid Id { get; set; }

        public bool Show
        {
            get => this.show;
            set
            {
                this.Set(ref this.show, value);
                this.RaisePropertyChanged(nameof(this.ShowHasScore));
            }
        }
    }
}