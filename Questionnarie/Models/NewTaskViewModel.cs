using Tasks.Model;
using Tasks.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Tasks.Models
{
    public class NewTaskViewModel
    {
        public IEnumerable<SelectListItem> ToUserValues { get; set; }
        public WorkTask NewTask { get; set; }
    }
}