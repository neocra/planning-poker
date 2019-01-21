using System;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Domain;
using Pattern.Mvvm.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class BootStrap
    {
        private readonly INavigationService navigationService;
        private readonly GameService gameService;

        public BootStrap(INavigationService navigationService, GameService gameService)
        {
            this.navigationService = navigationService;
            this.gameService = gameService;
        }

        public async Task Start()
        {
            //this.gameService.SetPlayer(new Player { Name = "Toto", Id = Guid.NewGuid() });
            //this.gameService.SetPlayer(new Player { Name = "Aurélien", Id = Guid.NewGuid() });
            //this.gameService.SetPlayer(new Player { Name = "Mick", Id = Guid.NewGuid() });
            //this.gameService.SetPlayer(new Player { Name = "Maxime", Id = Guid.NewGuid() });
            //this.gameService.SetPlayer(new Player { Name = "Guillaume", Id = Guid.NewGuid() });
            //this.gameService.SetPlayer(new Player { Name = "Kiril", Id = Guid.NewGuid() });
            //await this.navigationService.Navigate(typeof(ScorePage));

            await this.navigationService.NavigateRoot(typeof(MainPage));
        }
    }
}