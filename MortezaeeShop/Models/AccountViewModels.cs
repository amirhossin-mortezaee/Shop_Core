using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MortezaeeShop.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(300)]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        [Remote("VerifyEmail", "Account")]
        public string Email { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,20}$", ErrorMessage = "کلمه عبور باید شامل حرف و عدد باشد")]
        public string Password { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "تکرار کلمه عبور")]
        public string RePassword { get; set; }
    }

    public class LoginVieModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(300)]
        [EmailAddress]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }
        [Display(Name = "مرا به خاطر بسپار ")]
        public bool RememberMe { get; set; }
    }
}
