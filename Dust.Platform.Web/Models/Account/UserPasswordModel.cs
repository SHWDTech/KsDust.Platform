using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Web.Models.Account
{
    public class UserPasswordModel
    {
        public string UserId { get; set; }

        [Display(Name = "用户登录名：")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "必须输入当前密码")]
        [Display(Name = "当前密码：")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "必须输入新密码")]
        [Display(Name = "新密码：")]
        [StringLength(32, ErrorMessage = "密码长度必须在6-32个字符之间", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare(nameof(Password), ErrorMessage = "两次输入的密码不一致")]
        [Display(Name = "确认新密码：")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}