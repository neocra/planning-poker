using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Pattern.Mvvm;
using Pattern.Tasks;
using ViewModelBase = Pattern.Mvvm.Forms.ViewModelBase;

namespace Game.Planning.Poker.Mobile
{
    public class PokerViewModelBase : ViewModelBase
    {
        private readonly Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();

        protected AsyncCommand<T> CreateCommand<T>(Func<T, Task> method, ILoadingHandler loadingHandler = null,
            [CallerMemberName] string name = null)
        {
            if (this.commands.ContainsKey(name))
            {
                return this.commands[name] as AsyncCommand<T>;
            }

            var command = new AsyncCommand<T>(method, loadingHandler);
            this.commands.Add(name, command);

            return command;
        }
 
        protected AsyncCommand CreateCommand(Func<Task> method, ILoadingHandler loadingHandler = null,
            [CallerMemberName] string name = null)
        {
            if (this.commands.ContainsKey(name))
            {
                return this.commands[name] as AsyncCommand;
            }

            var command = new AsyncCommand(method, loadingHandler);
            this.commands.Add(name, command);

            return command;
        }
        
        public override Task InitAsync()
        {
            return Task.CompletedTask;
        }

        public override Task Resume()
        {
            return Task.CompletedTask;
        }

        public override void StartLoading()
        {
        }

        public override void StopLoading()
        {
        }
    }
}