using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Components;

namespace Tvision2.Core.Engine
{
    public interface IAddChildComponentOptions
    {
        IAddChildComponentOptions RetrieveParentStatusChanges();
        IAddChildComponentOptions DoNotNotifyParentOnAdd();

    }
    public class AddComponentOptions : IAddChildComponentOptions
    {
        internal TvComponentMetadata ComponentMetadata { get; private set; }
        internal Guid ParentId { get; private set; }
        internal TvComponentMetadata Before { get; private set; }
        internal Action<ITuiEngine> AfterAddAction { get; private set; }

        internal bool NotifyParentOnAdd { get; private set; }

        internal bool AddedAsChild { get => ParentId != Guid.Empty; }

        internal bool RetrieveParentStatusChanges { get; private set; }

        internal static AddComponentOptions AddAfter(TvComponentMetadata metadata, TvComponentMetadata before) =>
            new AddComponentOptions(metadata)
            {
                Before = before
            };

        internal static AddComponentOptions AddAsChild(TvComponentMetadata metadata, Guid parentId) =>
            new AddComponentOptions(metadata)
            {
                ParentId = parentId
            };

        internal static AddComponentOptions AddAsChild(TvComponentMetadata metadata, TvComponentMetadata parent) =>
            new AddComponentOptions(metadata)
            {
                ParentId = parent.Id
            };

        internal AddComponentOptions(TvComponentMetadata component)
        {
            ComponentMetadata = component;
            RetrieveParentStatusChanges = false;
            NotifyParentOnAdd = true;
            ParentId = Guid.Empty;
        }


        public AddComponentOptions WithParent(Guid parentId, Action<IAddChildComponentOptions> childOptions = null)
        {
            ParentId = parentId;
            childOptions?.Invoke(this);
            return this;
        }

        public AddComponentOptions WithParent(TvComponent parent, Action<IAddChildComponentOptions> childOptions = null) =>
            WithParent(parent.ComponentId, childOptions);

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

        IAddChildComponentOptions IAddChildComponentOptions.RetrieveParentStatusChanges()
        {
            RetrieveParentStatusChanges = true;
            return this;
        }

        IAddChildComponentOptions IAddChildComponentOptions.DoNotNotifyParentOnAdd()
        {
            NotifyParentOnAdd = false;
            return this;
        }
    }
}
