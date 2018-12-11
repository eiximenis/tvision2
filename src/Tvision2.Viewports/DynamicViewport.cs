using System;
using System.Collections.Generic;
using System.Text;
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

        public int ZIndex => _viewport.ZIndex;

        public int Columns => _viewport.Columns;

        public int Rows => _viewport.Rows;

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

        public IViewport ResizeTo(int cols, int rows)
        {
            return new DynamicViewport(_factory, _creator, _viewport.ResizeTo(cols, rows));
        }

        public IViewport Translate(TvPoint translation)
        {
            return new DynamicViewport(_factory, _creator, _viewport.Translate(translation));
        }
    }
}
