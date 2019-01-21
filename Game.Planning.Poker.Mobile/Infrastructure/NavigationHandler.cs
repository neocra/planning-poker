using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using Pattern.Mvvm.Forms;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile.Infrastructure
{
    public class NavigationHandler : INavigationHandler
    {
        public void Navigate(string path, Page page)
        {
            Analytics.TrackEvent("Navigate", new Dictionary<string, string>
            {
                {"Page", path}
            });
        }

        public void NavigateBack()
        {
        }
    }
}