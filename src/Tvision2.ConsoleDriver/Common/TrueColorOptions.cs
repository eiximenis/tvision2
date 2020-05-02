
namespace Tvision2.ConsoleDriver.Common
{
    public class TrueColorOptions : ITrueColorOptions 
    {
        public TrueColorProvider Provider { get; private set; }


        void ITrueColorOptions.WithBuiltInSequences()
        {
            Provider = TrueColorProvider.BuiltIn;
        }
        
        public TrueColorOptions()
        {
            Provider = TrueColorProvider.BuiltIn;
        }
    }
}