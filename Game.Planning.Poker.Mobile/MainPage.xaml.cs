using System;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Infrastructure;
using Pattern.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public partial class MainPage : ContentPage, INavigateFrom
    {
        private readonly IDeviceDisplay deviceDisplay;

        public MainPage(IDeviceDisplay deviceDisplay)
        {
            this.deviceDisplay = deviceDisplay;
            this.InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.Appearing += this.OnAppearing;
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            this.TopGrid.HeightRequest = this.deviceDisplay.MainDisplayInfo.Height / this.deviceDisplay.MainDisplayInfo.Density;
            this.Open().Fire();
        }

        private bool firstLoad = true;

        private async Task Open()
        {
            if (!this.firstLoad)
            {
                return;
            }

            this.firstLoad = false;
             
            var start = this.deviceDisplay.MainDisplayInfo.Height / this.deviceDisplay.MainDisplayInfo.Density;
            var end = (this.deviceDisplay.MainDisplayInfo.Height / this.deviceDisplay.MainDisplayInfo.Density) / 2.0;
            await Task.WhenAll(
                this.TopGrid.AnimateAsync(d => this.TopGrid.HeightRequest = d, start, end, 1500),
                this.AnimateFade(),
                this.AnimateLogo());
        }
        
        private async Task AnimateFade()
        {
            await Task.Delay(1400);
            await this.GridUserName.FadeTo(1.0, easing: Easing.CubicIn);
        }
        
        private async Task AnimateLogo()
        {
            await Task.Delay(1250);
            this.Logo.Play();
        }
        
        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            int fontSize = 50;

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.White.ToSKColor(),
                StrokeWidth = 0,
                TextSize = fontSize
            };
            SKPaint paintRed = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.FromHex("#2e3645").ToSKColor(),
                StrokeWidth = 1
            };

            canvas.DrawRect(0, 0, info.Width, info.Height, paintRed);
            float x = 200;
            float y = 200;
            
            canvas.DrawText("Poker", x, y, paint);
            canvas.DrawText("Planning", x + 20, y + fontSize, paint);

            DrawCard(100f, 100f, canvas);
        }

        private static void DrawCard(float xOffset, float yOffset, SKCanvas canvas)
        {
            SKPaint paintCard = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Black.ToSKColor(),
                StrokeWidth = 2,
            };
            var path = new SKPath();

            var cardWith = 120.0f;
            var cardHeight = 180.0f;

            path.MoveTo(0.0f, 0.0f);
            path.LineTo(0.0f, 1.0f);
            path.LineTo(0.7f, 1.0f);
            path.LineTo(0.7f, 0.0f);
            path.Close();
            path.Transform(SKMatrix.MakeRotation((float)(45.0f * Math.PI / 180), 0.7f, 1.0f));
            path.Transform(SKMatrix.MakeScale(cardWith, cardHeight));
            path.Offset(xOffset, yOffset);

            canvas.DrawPath(path, paintCard);


            var pathHeart = new SKPath();

            pathHeart.MoveTo(0.5f , 1.0f);
            pathHeart.LineTo(0.0f , 0.4f );
            pathHeart.CubicTo(0.0f , 0.0f ,
                0.5f , 0.0f ,
                0.5f , 0.4f );
            pathHeart.CubicTo(0.5f , 0.0f ,
                1.0f , 0.0f ,
                1.0f , 0.4f );
            pathHeart.LineTo(0.5f , 1.0f );
            pathHeart.Close();
            pathHeart.Transform(SKMatrix.MakeScale(cardWith, cardHeight));
            pathHeart.Offset(xOffset, yOffset);

            canvas.DrawPath(pathHeart, paintCard);
        }

        public Task NavigateFrom(Type toPage)
        {
            return Task.WhenAll(
                this.GridUserName.FadeTo(0),
                this.Logo.FadeTo(0),
                this.TopGrid.AnimateAsync(d => this.TopGrid.HeightRequest = d, this.TopGrid.Height, this.Height, 500));
        }
    }
}
