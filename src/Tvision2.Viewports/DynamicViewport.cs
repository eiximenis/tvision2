﻿using System;
using System.Collections.Generic;
using System.Text;
using Tvision2.Core.Colors;
using Tvision2.Core.Render;

namespace Tvision2.Viewports
{
    public sealed class DynamicViewport : IViewport
    {
        private readonly IViewport _viewport;
        private readonly IViewportFactory _factory;
        private readonly Func<IViewportFactory, IViewport> _creator;

        public DynamicViewport(IViewportFactory factory, Func<IViewportFactory, IViewport> creator)
        {
            _factory = factory;
            _creator = creator;
            _viewport = _creator(_factory);
        }

        private DynamicViewport(IViewportFactory factory, Func<IViewportFactory, IViewport> creator, IViewport initialViewport)
        {
            _factory = factory;
            _creator = creator;
            _viewport = initialViewport;
        }

        public TvPoint Position => _viewport.Position;

        public IViewport Recalculate()
        {
            return new DynamicViewport(_factory, _creator);
        }

        public Layer ZIndex => _viewport.ZIndex;

        public TvBounds Bounds => _viewport.Bounds;

        public FlowModel Flow => _viewport.Flow;


        public IViewport Clone()
        {
            return new DynamicViewport(_factory, _creator);
        }


        public override bool Equals(object obj)
        {
            if (obj is IViewport)
            {
                return Equals((IViewport)obj);
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public bool Equals(IViewport other)
        {
            return _viewport.Equals(other);
        }

        public IViewport Grow(int ncols, int nrows)
        {
            return new DynamicViewport(_factory, _creator, _viewport.Grow(ncols, nrows));
        }

        public IViewport MoveTo(TvPoint newPos)
        {
            return new DynamicViewport(_factory, _creator, _viewport.MoveTo(newPos));
        }

        public IViewport ResizeToNewColsAndRows(int cols, int rows)
        {
            return new DynamicViewport(_factory, _creator, _viewport.ResizeToNewColsAndRows(cols, rows));
        }

        public IViewport ResizeTo(TvBounds bounds) => ResizeToNewColsAndRows(bounds.Cols, bounds.Cols);

        public IViewport Translate(TvPoint translation)
        {
            return new DynamicViewport(_factory, _creator, _viewport.Translate(translation));
        }
    }
}
