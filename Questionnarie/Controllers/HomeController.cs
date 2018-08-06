using Microsoft.Owin.Security;
using Tasks.Model;
using Tasks.Model.Abstract;
using Tasks.Model.Entities;
using Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Tasks.Constants;

namespace Tasks.Controllers
{
    public class HomeController : Controller
    {
        private IUserTasksRepository repo;
        public int PageSize = 4;

        public HomeController(IUserTasksRepository repository)
        {
            repo = repository;
        }

        // GET: Home
        public ActionResult TaskList(MenuEnum Selection= MenuEnum.TasksAssignedToMe, int page = 1)
        {
            IAuthenticationManager mng = Request.GetOwinContext().Authentication;
            //string userName = mng.User.;
            IEnumerable<WorkTask> tasks = null;
            List<UserRoleEnum> userRoles = new List<UserRoleEnum>();
            Enum.TryParse(mng.User.Claims.Where(c => c.Type == ClaimTypes.Role).FirstOrDefault().Value, out UserRoleEnum userRole);
            string userName = mng.User.Identity.Name;
            switch (Selection)
            {
                case MenuEnum.MyActiveTasks:
                    tasks = repo.Tasks.Where(t => t.FromUserId == userName && (t.When.Date == DateTime.Now.Date || t.Completed==false));
                    break;
                case MenuEnum.MyArchiveTasks:
                    tasks = repo.Tasks.Where(t => t.FromUserId == userName && (t.When.Date != DateTime.Now.Date && t.Completed == true));
                    break;
                case MenuEnum.TasksAssignedToMe:
                    tasks = repo.Tasks.Where(t => t.ToUserId == userName && (t.When.Date == DateTime.Now.Date || t.Completed == false));
                    break;
                case MenuEnum.MyCompletedTasks:
                    tasks = repo.Tasks.Where(t => t.ToUserId == userName && (t.When.Date != DateTime.Now.Date && t.Completed == true));
                    break;
                //default:
                //    tasks = repo.Tasks.Where(t => t.ToUserId == userName && (t.When.Date == DateTime.Now.Date || t.Completed == false));
                //    break;
            }

            TaskListViewModel model = new TaskListViewModel()
            {
               TaskList=tasks.Skip((page - 1) * PageSize).Take(PageSize),
               UserRole = userRole,
               PagingInfo = new PagingInfo {  CurrentPage=page, ItemsPerPage=PageSize, TotalItems=tasks.Count()},
               Selection=Selection
            };

            return View(model);
        }

    }
} 