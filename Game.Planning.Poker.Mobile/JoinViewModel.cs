using System;
using System.Threading.Tasks;
using System.Transactions;
using Game.Planning.Poker.Mobile.Domain;
using Game.Planning.Poker.Mobile.Infrastructure;
using Pattern.Mvvm;
using Pattern.Mvvm.Forms;
using Pattern.Tasks;

namespace Game.Planning.Poker.Mobile
{
    public class LambdaLoadingHandler : ILoadingHandler
    {
        private readonly Action startLoading;
        private readonly Action stopLoading;

        public LambdaLoadingHandler(Action startLoading = null, Action stopLoading = null)
        {
            this.startLoading = startLoading;
            this.stopLoading = stopLoading;
        }

        public void StartLoading()
        {
            this.startLoading?.Invoke();
        }

        public void StopLoading()
        {
            this.stopLoading?.Invoke();
        }
    }
    
    public class JoinViewModel : PokerViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly IQrCodeScan qrCodeScan;
        private readonly GameService gameService;
        private bool isLoading;

        public JoinViewModel(INavigationService navigationService, IQrCodeScan qrCodeScan, GameService gameService)
        {
            this.navigationService = navigationService;
            this.qrCodeScan = qrCodeScan;
            this.gameService = gameService;
        }

        public AsyncCommand ScanCommand => this.CreateCommand(this.ScanAsync, this.LoadingCommand());

        public AsyncCommand CreateCommand => this.CreateCommand(this.CreateAsync, this.LoadingCommand());
        
        private ILoadingHandler LoadingCommand() =>
            new LambdaLoadingHandler(
                startLoading: this.StartLoadingCommand,
                stopLoading: this.StopLoadingCommand);

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.Set(ref this.isLoading, value);
        }

        private void StartLoadingCommand()
        {
            this.IsLoading = true;
        }

        private void StopLoadingCommand()
        {
            this.IsLoading = false;
        }

        private async Task CreateAsync()
        {
            var gameCode = Guid.NewGuid().ToString();

            await this.gameService.StartGameAsync(gameCode);
            
            await this.navigationService.Navigate(typeof(CreateNewPage), false);
        }

        private async Task ScanAsync()
        {
            var gameCode = await this.qrCodeScan.Scan();

            await this.gameService.StartGameAsync(gameCode);
            
            await this.navigationService.Navigate(typeof(CreateNewPage));
        }             
    }
}