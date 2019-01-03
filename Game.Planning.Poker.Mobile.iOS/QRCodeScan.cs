using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Planning.Poker.Mobile.Infrastructure;
using ZXing;
using ZXing.Mobile;

namespace Game.Planning.Poker.Mobile.iOS
{
    public class QrCodeScan : IQrCodeScan
    {
        public async Task<string> Scan()
        {
            var scanner = new ZXing.Mobile.MobileBarcodeScanner();

            var result = await scanner.Scan(new MobileBarcodeScanningOptions()
            {
                PossibleFormats = new List<BarcodeFormat>() {BarcodeFormat.QR_CODE}
            }, true);

            return result.Text;
        }
    }
}