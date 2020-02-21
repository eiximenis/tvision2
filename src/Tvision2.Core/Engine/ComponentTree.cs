﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using Tvision2.Core.Components;
using Tvision2.Core.Render;
using Tvision2.Events;

namespace Tvision2.Core.Engine
{

    public class ComponentTree : IComponentTree
    {
        private readonly Dictionary<string, TvComponentMetadata> _componentsDict;
        private readonly LinkedList<TvComponentMetadata> _components;
        private readonly Dictionary<string, (TvComponentMetadata Metadata, TvComponentMetadata before, Action<ITuiEngine> AfterAdd)> _pendingAdds;
        private readonly Dictionary<string, TvComponentMetadata> _pendingRemovalsPhase1;
        private readonly Dictionary<string, TvComponentMetadata> _pendingRemovalsPhase2;
        private readonly List<IViewport> _viewportsToClear;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITuiEngine _engine;

        public event EventHandler<TreeUpdatedEventArgs> ComponentAdded;
        public event EventHandler<TreeUpdatedEventArgs> ComponentRemoved;

        public IEnumerable<TvComponent> Components => _components.Select(cm => cm.Component);

        public TvComponent GetComponent(string name) => _componentsDict.TryGetValue(name, out var metadata) ? metadata.Component : null;

        public T GetInstanceOf<T>() => (T)_engine.ServiceProvider.GetService(typeof(T));


        public bool Remove(TvComponent component)
        {
            var name = component.Name;
            var found = _componentsDict.ContainsKey(name);
            if (found)
            {
                _pendingRemovalsPhase1.Add(name, component.Metadata as TvComponentMetadata);
            }
            return found;
        }

        private void DoPendingRemovalsPhase1()
        {
            if (_pendingRemovalsPhase1.Count == 0)
            {
                return;
            }
            var toDelete = _pendingRemovalsPhase1.ToArray();
            _pendingRemovalsPhase1.Clear();
            foreach (var kvp in toDelete)
            {   
                var canBeUnmounted = kvp.Value.CanBeUnmountedFrom(_engine);
                if (canBeUnmounted)
                {
                    _pendingRemovalsPhase2.Add(kvp.Key, kvp.Value);
                    _viewportsToClear.AddRange(kvp.Value.Component.Viewports.Select(x => x.Value));
                    _componentsDict.Remove(kvp.Key);
                }
            }
        }

        private void DoPendingRemovalsPhase2()
        {
            if (_pendingRemovalsPhase2.Count == 0)
            {
                return;
            }

            foreach (var kvp in _pendingRemovalsPhase2)
            {
                kvp.Value.UnmountedFrom(_engine);
                OnComponentRemoved(kvp.Value);
            }

            _pendingRemovalsPhase2.Clear();
        }

        public ComponentTree(ITuiEngine engine, IServiceProvider serviceProvider)
        {
            _componentsDict = new Dictionary<string, TvComponentMetadata>();
            _components = new LinkedList<TvComponentMetadata>();
            _pendingAdds = new Dictionary<string, (TvComponentMetadata Metadata, TvComponentMetadata before, Action< ITuiEngine> AfterAdd)>();
            _pendingRemovalsPhase1 = new Dictionary<string, TvComponentMetadata>();
            _pendingRemovalsPhase2 = new Dictionary<string, TvComponentMetadata>();
            _viewportsToClear = new List<IViewport>();
            _engine = engine;
            _serviceProvider = serviceProvider;
        }

        public IComponentMetadata Add(TvComponent component, Action<ITuiEngine> afterAddAction = null)
        {
            _pendingAdds.Add(component.Name, (component.Metadata as TvComponentMetadata, null, afterAddAction));
            return component.Metadata;
        }

        public IComponentMetadata AddAfter(TvComponent componentToAdd, TvComponent componentBefore, Action<ITuiEngine> afterAddAction = null)
        {
            _pendingAdds.Add(componentToAdd.Name, (componentToAdd.Metadata as TvComponentMetadata, componentBefore.Metadata as TvComponentMetadata, afterAddAction));
            return componentToAdd.Metadata;
        }


        private void DoPendingAdds()
        {
            if (_pendingAdds.Count == 0)
            {
                return;
            }

            var toAdd = _pendingAdds.ToArray();
            _pendingAdds.Clear();

            foreach (var kvp in toAdd)
            {
                (var metadata, var componentBefore, var afterAdd) = kvp.Value;
                _componentsDict.Add(kvp.Key, metadata);

                var cnode = _components.First;

                if (componentBefore != null)
                {
                    while (cnode != null)
                    {
                        if (cnode.Value == componentBefore)
                        {
                            _components.AddAfter(cnode, metadata);
                            break;
                        }
                        cnode = cnode.Next;
                    }
                }
                else
                {
                    _components.AddLast(metadata);
                }


                CreateNeededBehaviors(metadata.Component);
                metadata.MountedTo(_engine);
                metadata.Component.Invalidate();
                afterAdd?.Invoke(_engine);
                OnComponentAdded(metadata);
            }
        }

        private void CreateNeededBehaviors(TvComponent component)
        {
            var behaviorsToBeCreated = component.BehaviorsMetadatas.Where(bm => !bm.Created).ToList();
            foreach (var bm in behaviorsToBeCreated)
            {
                bm.CreateBehavior(_serviceProvider);
            }
        }

        private void OnComponentAdded(IComponentMetadata metadata)
        {
            ComponentAdded?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        private void OnComponentRemoved(IComponentMetadata metadata)
        {
            ComponentRemoved?.Invoke(this, new TreeUpdatedEventArgs(metadata));
        }

        public void ClearViewport(IViewport viewportToClear)
        {
            if (viewportToClear != null)
            {
                _viewportsToClear.Add(viewportToClear);
            }
        }

        public void Clear()
        {
            foreach (var child in Components)
            {
                Remove(child);
            }
        }

        internal void Update(ITvConsoleEvents evts)
        {
            DoPendingRemovalsPhase1();
            DoPendingAdds();
            foreach (var cdata in _components)
            {
                cdata.Component.Update(evts);
            }
        
        }

        internal void Draw(VirtualConsole console, bool force)
        {
            foreach (var viewport in _viewportsToClear)
            {
                console.Clear(viewport);
            }

            foreach (var cdata in _components
                .Where(c => force || c.Component.NeedToRedraw != RedrawNeededAction.None))
            {
                cdata.Component.Draw(console);
            }

            _viewportsToClear.Clear();

            DoPendingRemovalsPhase2();
        }

    }
}
