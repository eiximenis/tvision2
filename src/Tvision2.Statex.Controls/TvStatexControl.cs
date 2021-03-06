﻿using System;
using Tvision2.Controls;
using Tvision2.Core.Components;
using Tvision2.Core.Components.Behaviors;
using Tvision2.Statex.Controls.Behaviors;

namespace Tvision2.Statex.Controls
{

    public static class TvStatexControl
    {
        public static TvStatexControl<TControl, TControlState, TStatex> Wrap<TControl, TControlState, TStatex>(TControl control, Action<StatexControlOptions<TControl, TControlState,TStatex>> optionsAction)
            where TControl : TvControl<TControlState>
            where TControlState : class, IDirtyObject
            where TStatex : class
        {
            var options = new StatexControlOptions<TControl, TControlState, TStatex>();
            optionsAction?.Invoke(options);
            return new TvStatexControl<TControl, TControlState, TStatex>(control, options);
        }
    }

    public class TvStatexControl<TControl, TControlState, TStatex>
        where TControl : TvControl<TControlState>
        where TControlState : class, IDirtyObject
        where TStatex : class
    {
        public TControl Control { get; }
        private readonly StatexControlOptions<TControl,TControlState, TStatex> _options;

        internal TvStatexControl(TControl control, StatexControlOptions<TControl, TControlState, TStatex> options)
        {
            Control = control;
            AddSatexBehavors(control.AsComponent());
            _options = options;
        }


        private void AddSatexBehavors(TvComponent<TControlState> component)
        {
            if (!component.HasBehavior<StatexBehavior<TControl, TControlState, TStatex>>())
            {
                component.AddBehavior<StatexBehavior<TControl, TControlState, TStatex>>(data =>
                {
                    data.UseScheduler(BehaviorSchedule.OncePerFrame);
                    data.OnCreate(behavior =>
                    {
                        behavior.SetOptions(_options);
                        behavior.SetupControl(Control);
                    });
                });
            }
        }
    }
}
