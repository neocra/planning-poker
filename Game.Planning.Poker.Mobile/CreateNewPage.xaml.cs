using System;
using System.Linq;
using System.Threading.Tasks;
using Pattern.Tasks;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public partial class CreateNewPage : ContentPage
    {
        public CreateNewPage()
        {
            this.InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
        }

        private void Cell_OnAppearing(object sender, EventArgs e)
        {
            var viewCell = sender as ViewCell;

            var descendants = viewCell.Descendants().ToList();
            var grid = descendants.OfType<Grid>().Skip(1).First();
            var label = descendants.OfType<Label>().First();
            Task.WhenAll(           
                    grid.AnimateAsync(d => grid.HeightRequest = d, 0, 42, 1000),
                    label.FadeTo(1.0))
                .Fire();
        }
    }
}
