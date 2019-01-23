using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Pattern.Tasks;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public partial class ScorePage : ContentPage
    {
        public ScorePage()
        {
            this.InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
    
    public class CustomBehavior : Behavior<Grid>
    {
        private Grid control;
        private ScorePlayer scorePlayer;

        protected override void OnAttachedTo (Grid bindable)
        {
            base.OnAttachedTo (bindable);

            this.control = bindable;
            var noScore = this.control.FindByName<Grid>("NoScore");
            noScore.Opacity = 0.2;
            this.control.BindingContextChanged += this.ControlOnBindingContextChanged;
        }

        private void ControlOnBindingContextChanged(object sender, EventArgs e)
        {
            if (this.scorePlayer != null)
            {
                this.scorePlayer.PropertyChanged -= this.ModelOnPropertyChanged;                
            }
            
            this.scorePlayer = this.control.BindingContext as ScorePlayer;
            
            if (this.scorePlayer != null)
            {
                var showScore = this.control.FindByName<Grid>("ShowScore");
                if (this.scorePlayer.Show)
                {
                    showScore.FadeTo(1.0, 1000).Fire();
                }
                else
                {
                    showScore.FadeTo(0.0, 2000).Fire();                    
                }
                var noScore = this.control.FindByName<Grid>("NoScore");
                if (this.scorePlayer.ShowHasScore)
                {
                    noScore.FadeTo(1.0, 2000).Fire();                        
                }
                else
                {
                    noScore.FadeTo(0.2, 2000).Fire();
                }

 
                this.scorePlayer.PropertyChanged += this.ModelOnPropertyChanged;                
            }
        }

        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ScorePlayer.Show):
                    var showScore = this.control.FindByName<Grid>("ShowScore");
                    if (this.scorePlayer.Show)
                    {
                        showScore.FadeTo(1.0, 1000).Fire();
                    }
                    else
                    {
                        showScore.FadeTo(0.0, 2000).Fire();                                                
                    }
                    break;
                case nameof(ScorePlayer.ShowHasScore):
                    var noScore = this.control.FindByName<Grid>("NoScore");
                    if (this.scorePlayer.ShowHasScore)
                    {
                        noScore.FadeTo(1.0, 2000).Fire();                        
                    }
                    else
                    {
                        noScore.FadeTo(0.0, 2000).Fire();                                                
                    }
                    break;
            }
        }

        protected override void OnDetachingFrom (Grid bindable)
        {
            base.OnDetachingFrom (bindable);
            
            this.control.BindingContextChanged -= this.ControlOnBindingContextChanged;
            
            if (this.scorePlayer != null)
            {
                this.scorePlayer.PropertyChanged -= this.ModelOnPropertyChanged;                
            }
        }
    }
}
