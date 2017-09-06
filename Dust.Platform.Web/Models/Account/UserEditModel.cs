﻿using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Web.Models.Account
{
    public class UserEditModel
    {
        public string Id { get; set; }

        [Display(Name = "用户名称")]
        [Required(ErrorMessage = "必须填写用户名称")]
        public string UserName { get; set; } = string.Empty;

        [Display(Name = "用户所属角色")]
        public string UserRole { get; set; } = string.Empty;

        public string UserRelateEntity { get; set; }

        [Display(Name = "联系电话")]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}