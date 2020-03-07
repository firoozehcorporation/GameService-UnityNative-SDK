using Plugins.GameService.Utils.AndroidSupport.Handlers;
using Plugins.GameService.Utils.AndroidSupport.Interfaces;
using Plugins.GameService.Utils.AndroidSupport.Models;

namespace Plugins.GameService.Utils.AndroidSupport.Utils
{
    internal static class DialogUtil
    {
        internal static void LoginOrSignUp()
        {
            var gameService = NativePluginHandler.GetGameServiceInstance();
            gameService.Call("loginUser",new IGameServiceCallback(c =>
                {
                    var data = c.Split('^');
                    if (data.Length == 3)
                        LoginHandlers.UserEvent?.Invoke(null,new UserDataEvent
                        {
                            NickName = data[0],
                            Email = data[1],
                            Password = data[2]
                        });
                    else
                        LoginHandlers.UserEvent?.Invoke(null,new UserDataEvent
                        {
                            NickName = null,
                            Email = data[0],
                            Password = data[1]
                        });          
                }
                , e =>
                {}));
        }

        internal static void DownloadObbFile(string clientId,string tag)
        {
            var downloadHandler = NativePluginHandler.GetDownloadInstance();

            downloadHandler.Call("DownloadObbDataFile",clientId,tag,
                new IGameServiceCallback(c =>
                {
                    DownloadHandler.Downloaded?.Invoke(null,new DownloadEvent
                    {
                        Callback = c
                    });
                }, e =>
                { 
                    DownloadHandler.Downloaded?.Invoke(null,new DownloadEvent
                    {
                        Error = e
                    });
                    
                }));
        }
        
    }
}