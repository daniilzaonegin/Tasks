using Tasks.Model.Entities;
using System.Collections.Generic;

namespace Tasks.Model.Abstract
{
    public interface IUserTasksRepository
    {
        User GetUser(string user, string pwd);
        string GetUserEmail(string UserName);
        IEnumerable<WorkTask> Tasks { get; }
        IEnumerable<string> GetUsers4Task { get; }
        void Save();
        void SaveTask(WorkTask t);
    }
}
