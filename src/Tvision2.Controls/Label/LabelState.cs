namespace Tvision2.Controls.Label
{
    public class LabelState : TvControlData
    {
        private string _text;

        public string Text
        {
            get => _text;
            set { _text = value; IsDirty = true; }
        }
    }
}