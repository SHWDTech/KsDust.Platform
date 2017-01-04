using System;
using System.Web.Security;
using Dust.Platform.Storage.Repository;
using Dust.Platform.Web.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dust.Platform.Web.Process
{
    public class AccountProcess
    {
        public static SignInResult PasswordSignIn(LoginViewModel model)
        {
            var result = new SignInResult();
            var auth = new AuthRepository();
            var user = auth.FindUser(model.LoginName, model.Password);
            if (user.Result == null)
            {
                result.Status = SignInStatus.Failure;
                result.ErrorElement = "Password";
                result.ErrorMessage = "无效的用户名或登陆密码";
                return result;
            }
            FormsAuthentication.SetAuthCookie(model.LoginName, false);
            result.Status = SignInStatus.Success;

            return result;
        }

        public static bool UserIsInRole(string userId, string roleName)
        {
            var auth = new AuthRepository();

            return auth.UserInRole(userId, roleName);
        }

        public static IdentityUser FindUserByName(string name)
        {
            var auth = new AuthRepository();
            return auth.FindByName(name);
        }

        public static Guid FindVendorId(IdentityUser usr)
        {
            var auth = new AuthRepository();
            return auth.FindVendorId(usr);
        }
    }
}