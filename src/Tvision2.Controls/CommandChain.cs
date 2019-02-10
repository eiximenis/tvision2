using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tvision2.Controls
{
    public class CommandChain<TData> : ICommandChain<TData>
    {
        private readonly List<(Guid id, ICommand<TData> cmd)> _guidedCommands;

        public CommandChain()
        {
            _guidedCommands = new List<(Guid id, ICommand<TData> cmd)>();
        }

        public async Task Invoke(TData item)
        {
            foreach (var guidedCommand in _guidedCommands)
            {
                await guidedCommand.cmd.Invoke(item);
            }
        }



        public Guid Add(Func<TData, Task<bool>> commandFunc) =>
            Add(new DelegateCommand<TData>(commandFunc));

        public Guid Add(ICommand<TData> command)
        {
            var guid = Guid.NewGuid();
            _guidedCommands.Add((guid, command));
            return guid;
        }

        public void Remove(Guid id)
        {
            for (var idx = 0; idx < _guidedCommands.Count; idx++)
            {
                if (_guidedCommands[idx].Item1 == id)
                {
                    _guidedCommands.RemoveAt(idx);
                    return;
                }
            }
        }

        Guid ICommandChain.Add(ICommand command)
            => Add(command as ICommand<TData> ?? throw new ArgumentException($"Is not a ICommand<{typeof(TData).Name}> object", nameof(command)));
    }
}
