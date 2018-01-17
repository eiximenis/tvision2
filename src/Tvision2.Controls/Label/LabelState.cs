namespace Tvision2.Controls.Label
{
    public class LabelState : TvControlState
    {
        private string _text;

        public string Text
        {
            get => _text;
            set { _text = value; IsDirty = true; }
        }
    }
}