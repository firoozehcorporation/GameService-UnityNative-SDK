using System.Threading.Tasks;
using FiroozehGameService.Models;
using Plugins.GameService.Utils.AndroidSupport.Handlers;
using Plugins.GameService.Utils.AndroidSupport.Models;
using Plugins.GameService.Utils.AndroidSupport.Utils;

namespace Plugins.GameService.Utils.AndroidSupport
{
    public class GameServiceAndroid
    {
        public static async Task LoginOrSignUp()
        {
            LoginHandlers.UserEvent += UserEvent;
            if (FileUtil.IsUserLogin())
            {
                var userToken = FileUtil.GetUserToken();
                await FiroozehGameService.Core.GameService.Login(userToken);
            }
            else
                DialogUtil.LoginOrSignUp();
            
        }

        public static void DownloadObbFile(string clientId,string tag)
        {
            if (string.IsNullOrEmpty(clientId) && string.IsNullOrEmpty(tag))
                throw new GameServiceException("clientId and tag Must Not be NULL");
            DialogUtil.DownloadObbFile(clientId,tag);
        }

        private static async void UserEvent(object sender, UserDataEvent e)
        {
            string userToken;
            if (e.NickName == null) // Is Login
                userToken = await FiroozehGameService.Core.GameService.Login(e.Email,e.Password);
            else // Is SignUp
                userToken = await FiroozehGameService.Core.GameService.SignUp(e.NickName,e.Email,e.Password);
          FileUtil.SetUserToken(userToken);
        }
    }
}
