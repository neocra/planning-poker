using System.Threading.Tasks;

namespace Game.Planning.Poker.Mobile.Infrastructure
{
    public interface IQrCodeScan
    {
        Task<string> Scan();
    }
}