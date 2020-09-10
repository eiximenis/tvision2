using System;
using System.Collections.Generic;
using System.Text;

namespace Tvision2.Controls.Label
{

    public enum TextAlignment
    {
        Left,
        Right,
        Center
    }

    public interface ILabelOptions
    {
        public TextAlignment Alignment { get; }
    }

    public interface ITvLabelOptionsBuilder 
    {
        ITvLabelOptionsBuilder Center();
    }

    public class TvLabelOptions : ILabelOptions, ITvLabelOptionsBuilder
    {
        public TextAlignment Alignment { get; private set; }


        ITvLabelOptionsBuilder ITvLabelOptionsBuilder.Center()
        {
            Alignment = TextAlignment.Center;
            return this;
        }
    }
}
