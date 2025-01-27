using AA1.Models;

namespace AA1.Services;

public interface ITaskService
{
    AA1.Models.Task CreateTask(string _title, string _description, int _difficulty, DateTime? _expirationDate);
    AA1.Models.Task GetTaskById(int id);
    bool IsTaskExpired(int id);
    bool IsTaskCompleted(int id);
    AA1.Models.Task CompleteTask(int id);
    AA1.Models.Task ExpireTask(int id);
}