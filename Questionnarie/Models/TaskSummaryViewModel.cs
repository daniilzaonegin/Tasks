using Tasks.Model;
using Tasks.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tasks.Models
{
    public class TaskSummaryViewModel
    {
        public UserRoleEnum Role { get; set; }
        public WorkTask Task { get; set; }
    }
}