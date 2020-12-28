using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Tvision2.Core.Hooks;

namespace Tvision2.Core
{
    public abstract class Tvision2Setup
    {
        private readonly Tvision2Options _options;
        public ITvision2Options Options => _options;

        private readonly Dictionary<string, object> _setupSteps;

        
        public Tvision2Setup()
        {
            _options = new Tvision2Options();
            _setupSteps = new Dictionary<string, object>();
        }

        public bool HasSetupStep(string step) => _setupSteps.ContainsKey(step);

        public void AddSetupStep<T>(string step, T value)
        {
            if (!HasSetupStep(step)) _setupSteps.Add(step, value);
        }

        public T GetSetupStep<T>(string step) => _setupSteps.TryGetValue(step, out var data) ? (T)data : default;

        public abstract void ConfigureServices(Action<IServiceCollection> configureDelegate);

        public Tvision2Setup AddHook<THook>()
            where THook : class, IEventHook
        {

            ConfigureServices(sc =>
            {
                sc.AddTransient<THook>();
            });
            _options.AddHook<THook>();

            return this;

        }

    }

}
