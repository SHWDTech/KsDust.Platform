using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Web.Models.Account
{
    public class UserPasswordModel
    {
        public string UserId { get; set; }

        [Display(Name = "用户登录名：")]
        public string UserName { get; set; }

        [Display(Name = "当前密码：")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Display(Name = "新密码：")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "确认新密码：")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}