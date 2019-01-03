using System;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Domain;
using Game.Planning.Poker.Mobile.Infrastructure;
using Pattern.Mvvm;
using Pattern.Mvvm.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class JoinViewModel : PokerViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IQrCodeScan qrCodeScan;
        private readonly GameService gameService;

        public JoinViewModel(INavigationService navigationService, IQrCodeScan qrCodeScan, GameService gameService)
        {
            this.navigationService = navigationService;
            this.qrCodeScan = qrCodeScan;
            this.gameService = gameService;
        }
        
        public AsyncCommand ScanCommand => this.CreateCommand(this.ScanAsync);
        public AsyncCommand CreateCommand => this.CreateCommand(this.CreateAsync);

        private async Task CreateAsync()
        {
            var gameCode = Guid.NewGuid().ToString();

            await this.gameService.StartGameAsync(gameCode, true);
            
            await this.navigationService.Navigate(typeof(CreateNewPage));
        }

        private async Task ScanAsync()
        {
            var gameCode = await this.qrCodeScan.Scan();

            await this.gameService.StartGameAsync(gameCode);
            
            await this.navigationService.Navigate(typeof(CreateNewPage));
        }             
    }
}