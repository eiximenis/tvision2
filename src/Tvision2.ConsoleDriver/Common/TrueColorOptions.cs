using Tvision2.ConsoleDriver.Terminfo;

namespace Tvision2.ConsoleDriver.Common
{
    public class TrueColorOptions : ITrueColorOptions 
    {
        public TrueColorProvider Provider { get; private set; }
        
        public ITerminfoColorManager GetTerminfoColorManager()
        {
            return Provider == TrueColorProvider.Terminfo
                ? (ITerminfoColorManager) new TerminfoTrueColorManager()
                : (ITerminfoColorManager) new BuiltinTrueColorManager();
        }


        void ITrueColorOptions.WithBuiltInSequences()
        {
            Provider = TrueColorProvider.BuiltIn;
        }

        void ITrueColorOptions.FromTerminfo()
        {
            Provider = TrueColorProvider.Terminfo;
        }

        public TrueColorOptions()
        {
            Provider = TrueColorProvider.BuiltIn;
        }
    }
}