using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public interface IAddChildComponentOptions
    {
        AddComponentOptions RetrieveParentStatusChanges();
        AddComponentOptions DoNotNotifyParentOnAdd();

    }
    public class AddComponentOptions : IAddChildComponentOptions
    {
        internal TvComponentMetadata ComponentMetadata { get; private set; }
        internal TvComponentMetadata Parent { get; private set; }
        internal TvComponentMetadata Before { get; private set; }
        internal Action<ITuiEngine> AfterAddAction { get; private set; }

        internal bool NotifyParentOnAdd { get; private set; }

        internal bool AddedAsChild { get => Parent != null; }

        internal bool RetrieveParentStatusChanges { get; private set; }

        internal static AddComponentOptions AddAfter(TvComponentMetadata metadata, TvComponentMetadata before) =>
            new AddComponentOptions(metadata)
            {
                Before = before
            };

        internal static AddComponentOptions AddAsChild(TvComponentMetadata metadata, TvComponentMetadata parent) =>
            new AddComponentOptions(metadata)
            {
                Parent = parent
            };

        internal AddComponentOptions(TvComponentMetadata component)
        {
            ComponentMetadata = component;
            RetrieveParentStatusChanges = false;
            NotifyParentOnAdd = true;
        }

        public IAddChildComponentOptions WithParent(TvComponent parent)
        {
            Parent = parent.Metadata;
            return this;
        }

        public AddComponentOptions After(TvComponent before)
        {
            Before = before.Metadata;
            return this;
        }

        public AddComponentOptions ExecuteAfterAdding(Action<ITuiEngine> afterAddAction)
        {
            AfterAddAction = afterAddAction;
            return this;
        }

        AddComponentOptions IAddChildComponentOptions.RetrieveParentStatusChanges()
        {
            RetrieveParentStatusChanges = true;
            return this;
        }

        AddComponentOptions IAddChildComponentOptions.DoNotNotifyParentOnAdd()
        {
            NotifyParentOnAdd = false;
            return this;
        }
    }
}
