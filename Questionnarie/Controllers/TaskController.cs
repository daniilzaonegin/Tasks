using Microsoft.Owin.Security;
using Tasks.Model.Abstract;
using Tasks.Model.Entities;
using Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using Tasks.Abstract;
using System.Threading.Tasks;
using Tasks.Helpers;
using Tasks.Model;

namespace Tasks.Controllers
{
    public class TaskController : Controller
    {

        public TaskController(IUserTasksRepository repository, IEmailSender sender)
        {
            repo = repository;
            emailSender = sender;
        }
        // GET: Task
        public ActionResult CompleteTask(int taskId)
        {
            WorkTask workTask = repo.Tasks.Where(t => t.TaskId == taskId).FirstOrDefault();
            CompleteDialogViewModel task = new CompleteDialogViewModel() { TaskId = workTask.TaskId, Completed=workTask.Completed, CompleteMsg=workTask.CompleteMsg };
            return View("Task", task);
        }

        [HttpPost]
        public ActionResult CompleteTask(CompleteDialogViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Task", vm);
            }
            WorkTask taskToChange = repo.Tasks.Where(t => t.TaskId == vm.TaskId).FirstOrDefault();
            string taskCreatorEmail = repo.GetUserEmail(taskToChange.FromUserId);
            taskToChange.Completed = true;
            taskToChange.CompleteMsg = vm.CompleteMsg;
            repo.SaveTask(taskToChange);
            TempData["message"] = "Task is completed successfully!";

            if (taskCreatorEmail != null)
            {
                try
                {
                    emailSender.SendEmail(taskCreatorEmail, $"Your task:'{taskToChange.Summary}' was completed!",
                        String.Format("Task Subject: {0}\r\nTask completor: {1}\r\nTask Completion Message: {2}" +
                        "\r\nTask was due till:{3}\r\n\r\nCheckout here:{4}",                        
                        taskToChange.Subject,
                        taskToChange.ToUserId,
                        taskToChange.CompleteMsg,
                        taskToChange.When,
                        $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}"));
                    TempData["message"] = $"{TempData["message"]}<br/> Mail to the task creator was sent successfully!";
                }
                catch (SmtpException e)
                {
                    TempData["error"] += "<br/> Mail wasn't sent!";
                    TempData["error"] += "<br/> Exception: " + e.Message;
                }
            }

            return RedirectToAction(actionName: "TaskList", controllerName: "Home");
        }

        [Authorize(Roles = "TaskMaker")]
        public ActionResult NewTask()
        {

            IAuthenticationManager mng = Request.GetOwinContext().Authentication;
            string userName = mng.User.Identity.Name.Trim();            
            WorkTask task = new WorkTask() { FromUserId = userName };
            IEnumerable<string> Users4Task = repo.GetUsers4Task;
            SelectList toUsers = new SelectList(Users4Task);

            NewTaskViewModel vm = new NewTaskViewModel() { NewTask=task, ToUserValues=toUsers };

            return View("NewTask", vm);
        }

        [HttpPost]
        public ActionResult NewTask(NewTaskViewModel t)
        {
            if(!ModelState.IsValid)
            {
                NewTaskViewModel vm = new NewTaskViewModel() { NewTask = t.NewTask, ToUserValues = new SelectList(repo.GetUsers4Task) };
                return View(vm);
            }

            repo.SaveTask(t.NewTask);
            TempData["message"] = "Task was saved successfully!";

            string email = repo.GetUserEmail(t.NewTask.ToUserId);

            if (email != null)
            {
                try
                {
                    emailSender.SendEmail(email, "You have a new task:"+t.NewTask.Summary, 
                        String.Format("Task Subject: {0}\r\nTask Creator: {1}\r\nTask Due Time: {2}\r\n\r\nCheckout here:{3}",
                        t.NewTask.Subject,
                        t.NewTask.FromUserId,
                        t.NewTask.When.ToString(),
                        $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}"));
                    TempData["message"] = $"{TempData["message"]}<br/> Mail to the task resolver was sent successfully!";
                }
                catch (SmtpException e)
                {
                    TempData["error"] += "<br/>Mail wasn't sent!";
                    TempData["error"] += "<br/>Exception: " + e.Message;
                }
            }
            return RedirectToAction("TaskList", "Home"); 
          
        }
    
        private IUserTasksRepository repo;
        private IEmailSender emailSender;
    }
}