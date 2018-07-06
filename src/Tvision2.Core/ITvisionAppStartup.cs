using System.Threading.Tasks;
using Tvision2.Core.Engine;

namespace Tvision2.Core
{
    public interface ITvisionAppStartup
    {
        Task Startup(ITuiEngine tui);
    }
}