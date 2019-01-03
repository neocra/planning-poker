using System;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace Game.Planning.Poker.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
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
                Color = Color.Black.ToSKColor(),
                StrokeWidth = 0,
                TextSize = fontSize
            };
            SKPaint paintRed = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = Color.Red.ToSKColor(),
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
    }
}
