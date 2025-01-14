using AA1.Models;
using AA1.Data;

namespace AA1.Services;

public class TaskService : ITaskService
{
    private ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public bool IsTaskExpired(int id)
    {
        AA1.Models.Task task = _taskRepository.GetTaskById(id);
        return task.ExpirationDate < DateTime.Now;
    }

    public bool IsTaskCompleted(int id)
    {
        AA1.Models.Task task = _taskRepository.GetTaskById(id);
        return task.Completed;
    }

    public AA1.Models.Task CompleteTask(int id)
    {
        AA1.Models.Task task = _taskRepository.GetTaskById(id);
        task.Completed = true;
        return _taskRepository.UpdateTask(id, task);
    }

    public AA1.Models.Task ExpireTask(int id)
    {
        AA1.Models.Task task = _taskRepository.GetTaskById(id);
        task.Expired = true;
        return _taskRepository.UpdateTask(id, task);
    }
}
