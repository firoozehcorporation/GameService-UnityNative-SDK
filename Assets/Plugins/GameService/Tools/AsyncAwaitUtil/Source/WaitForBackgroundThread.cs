using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Plugins.GameService.Tools.AsyncAwaitUtil.Source
{
    public class WaitForBackgroundThread
    {
        public ConfiguredTaskAwaitable.ConfiguredTaskAwaiter GetAwaiter()
        {
            return Task.Run(() => {}).ConfigureAwait(false).GetAwaiter();
        }
    }
}
