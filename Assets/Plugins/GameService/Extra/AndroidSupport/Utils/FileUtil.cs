using Plugins.GameService.Utils.AndroidSupport.Handlers;

namespace Plugins.GameService.Utils.AndroidSupport.Utils
{
    internal static class FileUtil
    {
        internal static bool IsUserLogin()
        {
            var gameService = NativePluginHandler.GetGameServiceInstance();
            return gameService.Call<bool>("IsUserLogin");
        }
        
        internal static void SetUserToken(string userToken)
        {
            var gameService = NativePluginHandler.GetGameServiceInstance();
            gameService.Call("SetUserToken",userToken);
        }
        
        internal static string GetUserToken()
        {
            var gameService = NativePluginHandler.GetGameServiceInstance();
            return gameService.Call<string>("GetUserToken");
        }
        
    }
}