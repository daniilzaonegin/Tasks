using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tasks.Constants;

namespace Tasks.Controllers
{
    public class NavController : Controller
    {
        public PartialViewResult Menu(MenuEnum selection = MenuEnum.TasksAssignedToMe, bool horizontalLayout =false)
        {
            ViewBag.Selection = selection;
            IEnumerable<MenuEnum> selections = new MenuEnum[] { MenuEnum.TasksAssignedToMe, MenuEnum.MyCompletedTasks, MenuEnum.MyActiveTasks, MenuEnum.MyArchiveTasks};
            string viewName = horizontalLayout ? "MenuHorizontal" : "Menu";
            return PartialView(viewName,selections);
        }
    }
}