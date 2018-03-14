using Tvision2.Controls.Text;

namespace Tvision2.Controls.Textbox
{
    public class TextboxState : IDirtyObject
    {

        private readonly TextProcessor _textProcessor;

        public TextboxState()
        {
            _textProcessor = new TextProcessor();
        }

        public bool IsDirty { get; private set; }
        public void Validate()
        {
            IsDirty = false;
        }

        public string Text
        {
            get => _textProcessor.ToString();
        }
    }
}