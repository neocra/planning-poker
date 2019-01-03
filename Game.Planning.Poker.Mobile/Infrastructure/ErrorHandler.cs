using System;
using System.Diagnostics;
using Pattern.Tasks;

namespace Game.Planning.Poker.Mobile.Infrastructure
{
    public class ErrorHandler : IErrorHandler
    {
        public void Handle(Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}