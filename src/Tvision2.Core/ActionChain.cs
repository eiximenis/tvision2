using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tvision2.Core
{
    public class ActionChain<TData> : IActionChain<TData>
    {

        private readonly Dictionary<Guid, (IAction<TData> cmd, bool once)> _guidedCommands;

        public IEnumerable<Guid> Keys => _guidedCommands.Select(x => x.Key);

        public ActionChain()
        {
            _guidedCommands = new Dictionary<Guid, (IAction<TData>, bool)>();
        }

        public async Task Invoke(TData item)
        {
            var guidsToDelete = new List<Guid>();
            foreach (var guidedCommand in _guidedCommands)
            {
                await guidedCommand.Value.cmd.Invoke(item);
                if (guidedCommand.Value.once)
                {
                    guidsToDelete.Add(guidedCommand.Key);
                }
            }

            foreach (var guid in guidsToDelete)
            {
                Remove(guid);
            }
        }

        public Guid Add(Func<TData, Task<bool>> commandFunc) =>
            Add(new DelegateAction<TData>(commandFunc));

        public Guid AddOnce(Func<TData, Task<bool>> commandFunc) =>
            AddOnce(new DelegateAction<TData>(commandFunc));

        public Guid Add(IAction<TData> command)
        {
            var guid = Guid.NewGuid();
            _guidedCommands.Add(guid, (command, false));
            return guid;
        }

        public Guid AddOnce(IAction<TData> action)
        {
            var guid = Guid.NewGuid();
            _guidedCommands.Add(guid, (action, true));
            return guid;
        }



        public void Remove(Guid id)
        {
            if (_guidedCommands.ContainsKey(id))
            {
                _guidedCommands.Remove(id);
            }
        }

        Guid IActionChain.Add(IAction command)
            => Add(command as IAction<TData> ?? throw new ArgumentException($"Is not a ICommand<{typeof(TData).Name}> object", nameof(command)));

        Guid IActionChain.AddOnce(IAction action)
            => AddOnce(action as IAction<TData> ?? throw new ArgumentException($"Is not a ICommand<{typeof(TData).Name}> object", nameof(action)));

        public void Clear()
        {
            _guidedCommands.Clear();
        }

    }
}
