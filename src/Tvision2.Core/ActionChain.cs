using System;
using System.Collections.Generic;
using System.Linq;

namespace Tvision2.Core
{
    public class ActionChain<TData> : IActionChain<TData>
    {
        private readonly Lazy<Dictionary<Guid, (IAction<TData> cmd, bool once)>> _guidedCommands;
        public IEnumerable<Guid> Keys => _guidedCommands.Value.Select(x => x.Key);

        public ActionChain()
        {
            _guidedCommands = new Lazy<Dictionary<Guid, (IAction<TData>, bool)>>();
        }

        public void Invoke(TData item)
        {
            if (!_guidedCommands.IsValueCreated || !_guidedCommands.Value.Any())
            {
                return;
            }
            var guidsToDelete = new List<Guid>();
            foreach (var guidedCommand in _guidedCommands.Value)
            {
                var stop = guidedCommand.Value.cmd.Invoke(item);
                if (guidedCommand.Value.once)
                {
                    guidsToDelete.Add(guidedCommand.Key);
                }

                if (stop == ActionResult.Break) break;
            }

            foreach (var guid in guidsToDelete)
            {
                Remove(guid);
            }
        }

        public Guid Add(Action<TData> actionFunc) =>
            Add(new DelegateAction<TData>(actionFunc));

        public Guid Add(Func<TData, ActionResult> actionFunc) =>
            Add(new DelegateAction<TData>(actionFunc));

        public Guid Add(IAction<TData> action)
        {
            var guid = Guid.NewGuid();
            _guidedCommands.Value.Add(guid, (action, false));
            return guid;
        }


        public Guid AddOnce(IAction<TData> action)
        {
            var guid = Guid.NewGuid();
            _guidedCommands.Value.Add(guid, (action, true));
            return guid;
        }

        public Guid AddOnce(Action<TData> actionFunc) =>
            AddOnce(new DelegateAction<TData>(actionFunc));

        public Guid AddOnce(Func<TData, ActionResult> actionFunc) =>
            AddOnce(new DelegateAction<TData>(actionFunc));

        public void Remove(Guid id)
        {
            if (_guidedCommands.Value.ContainsKey(id))
            {
                _guidedCommands.Value.Remove(id);
            }
        }

        public void Clear()
        {
            _guidedCommands.Value.Clear();
        }

        Guid IActionChain.Add(IAction action)
            => Add(action as IAction<TData> ?? throw new ArgumentException($"Is not a ICommand<{typeof(TData).Name}> object", nameof(action)));

        Guid IOnceActionChain.AddOnce(IAction action)
            => AddOnce(action as IAction<TData> ?? throw new ArgumentException($"Is not a ICommand<{typeof(TData).Name}> object", nameof(action)));
    }
}
