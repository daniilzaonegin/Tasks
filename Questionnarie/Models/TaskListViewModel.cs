using Tasks.Model;
using Tasks.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tasks.Constants;

namespace Tasks.Models
{
    public class TaskListViewModel
    {
        public IEnumerable<WorkTask> TaskList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public MenuEnum Selection { get; set; }
        public UserRoleEnum UserRole { get; set; }
    }
}