using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tasks.Models
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage ="Please enter your e-mail!")]
        [Display(Name ="Please enter your e-mail")]
        public string Email { get; set; }
        public string Token { get; set; }
    }
}