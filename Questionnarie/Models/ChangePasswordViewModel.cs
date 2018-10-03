using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tasks.Models
{
    public class ChangePasswordViewModel
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}