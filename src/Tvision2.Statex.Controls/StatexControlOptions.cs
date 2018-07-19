using System;
using Tvision2.Controls;

namespace Tvision2.Statex.Controls
{
    public class StatexControlOptions<TControlState, TStatex>
        where TControlState : class, IDirtyObject

    {
        internal string StoreName { get; private set; }
        private Action<TStatex, TControlState> _controlStateUpdater;
        public void UseStore(string storeName)
        {
            StoreName = storeName;
        }

        public void UseStatexTransformation(Action<TStatex, TControlState> controlStateUpdater)
        {
            _controlStateUpdater = controlStateUpdater;
        }

        internal bool UpdateControlState(TControlState controlState, TStatex statex)
        {
            _controlStateUpdater?.Invoke(statex, controlState);
            return controlState.IsDirty;
        }
    }
}
