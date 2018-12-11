using Tvision2.ConsoleDriver.Win32;
using Tvision2.Events;

namespace Tvision2.Events.Win32
{
    internal class Win32WindowsEvent : TvWindowEvent
    {
        private WINDOW_BUFFER_SIZE_RECORD _windowBufferSizeEvent;

        public Win32WindowsEvent(WINDOW_BUFFER_SIZE_RECORD windowBufferSizeEvent) : base(cols: windowBufferSizeEvent.dwSize.X, rows: windowBufferSizeEvent.dwSize.Y)
        {
            _windowBufferSizeEvent = windowBufferSizeEvent;
        }
    }
}