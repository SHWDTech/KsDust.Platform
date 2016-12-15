using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dust.Platform.Service.Models
{
    [Serializable]
    public class User
    {
        [Required]
        [Display(Name = "用户登陆名")]
        public string LoginName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "密码不能少于{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码校验")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致。")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
    }
}