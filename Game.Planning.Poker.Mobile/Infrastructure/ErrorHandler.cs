using System;
using System.Diagnostics;
using Microsoft.AppCenter.Crashes;
using Pattern.Tasks;

namespace Game.Planning.Poker.Mobile.Infrastructure
{
    public class ErrorHandler : IErrorHandler
    {
        public void Handle(Exception ex)
        {
            Crashes.TrackError(ex);
        }
    }
}