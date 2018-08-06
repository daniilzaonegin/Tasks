using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tasks.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Please enter your login!")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Password cannot be empty")]       
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}