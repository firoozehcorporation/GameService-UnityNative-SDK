using System;

namespace Plugins.GameService.Utils.AndroidSupport.Models
{
    internal class UserDataEvent : EventArgs
    {
        internal string NickName { set; get; }
        internal string Email { set; get; }
        internal string Password { set; get; }
    }
}