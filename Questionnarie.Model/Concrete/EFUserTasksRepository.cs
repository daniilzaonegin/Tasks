using Tasks.Model.Abstract;
using Tasks.Model.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tasks.Model.Concrete
{
    public class EFUserTasksRepository : IUserTasksRepository
    {
        private EFDbContext context;

        public EFUserTasksRepository()
        {
            context = new EFDbContext();

            //DataBaseLogger logger = new DataBaseLogger(@"c:\temp\efLog.txt");
            //logger.Setup();
            context.Database.Log = System.Console.WriteLine;
        }
        public User GetUser(string user, string pwd)
        {
            return context.Users.Where(u => u.UserName == user && u.Password == pwd).FirstOrDefault();
        }
        public User GetUserByToken(string user, string token)
        {
            return context.Users.Where(u => u.UserName == user && u.PasswordResetToken == token).FirstOrDefault();
        }
        public IEnumerable<User> GetUserByEmail(string email)
        {
            return context.Users.Where(u => u.Email == email);
        }
        public string GetUserEmail(string UserName)
        {
            return context.Users.Where(u => u.UserName == UserName).FirstOrDefault()?.Email;
        }


        public void Save()
        {
            context.SaveChanges();
        }

        public void SaveTask(WorkTask t)
        {
            if(t.TaskId==0)
            {
                context.Tasks.Add(t);
            }
            else
            {
                WorkTask task = context.Tasks.Find(t.TaskId);
                if(task!=null)
                {
                    task.Completed = t.Completed;
                    task.CompleteMsg = t.CompleteMsg;
                    task.FromUserId = t.FromUserId;
                    task.Subject = t.Subject;
                    task.ToUserId = t.ToUserId;
                    task.When = t.When;
                    task.Summary = t.Summary;
                }
            }
            context.SaveChanges();
        }

        public IEnumerable<WorkTask> Tasks {
            get { return context.Tasks; }
        }

        public IEnumerable<string> GetUsers4Task {
            get
            {
                return context.Users.Where(x => x.RoleString.Contains(UserRoleEnum.TaskResolver.ToString())).Select(x => x.UserName).Distinct();
            }
        }
    }
}
