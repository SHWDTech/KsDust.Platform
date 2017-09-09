using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Web.Models.Account
{
    public class NewUserModel
    {
        public string Id { get; set; }

        [Display(Name = "用户名称")]
        [Required(ErrorMessage = "必须填写用户名称")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "必须填写登陆密码")]
        [Display(Name = "登陆密码")]
        [DataType(DataType.Password)]
        [StringLength(32, ErrorMessage = "密码长度必须在6-32个字符之间", MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare(nameof(Password), ErrorMessage = "两次输入的密码不一样")]
        [Display(Name = "确认登陆密码")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name = "用户所属角色")]
        public string UserRole { get; set; } = string.Empty;

        public string UserRelateEntity { get; set; }

        [Display(Name = "联系电话")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}