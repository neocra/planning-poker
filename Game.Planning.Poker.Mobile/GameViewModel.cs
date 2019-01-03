using System;
using System.Threading.Tasks;
using Pattern.Mvvm;
using Pattern.Mvvm.Forms;

namespace Game.Planning.Poker.Mobile
{
    public class JoinViewModel : PokerViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IQrCodeScan qrCodeScan;
        private readonly GameService gameService;
        private string fullName;

        public JoinViewModel(INavigationService navigationService, IQrCodeScan qrCodeScan, GameService gameService)
        {
            this.navigationService = navigationService;
            this.qrCodeScan = qrCodeScan;
            this.gameService = gameService;
        }
        
        public AsyncCommand ScanCommand => this.CreateCommand(this.ScanAsync);
        public AsyncCommand JoinCommand => this.CreateCommand(this.JoinAsync);

        public string FullName
        {
            get => this.fullName;
            set =>  this.Set(ref this.fullName, value);
        }

        private async Task ScanAsync()
        {
            var gameCode = await this.qrCodeScan.Scan();

            await this.gameService.StartGameAsync(gameCode, new Player()
            {
                Id = Guid.NewGuid(),
                Name = this.FullName
            });
        }
        
        private Task JoinAsync()
        {
            return this.navigationService.Navigate(typeof(JoinPage));
        }
    }
}