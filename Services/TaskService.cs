using AA1.Models;
using AA1.Data;

namespace AA1.Services;

public class TaskService : ITaskService
{
    private ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
        foreach (AA1.Models.Task task in _taskRepository.GetTasks())
        {
            if (IsTaskExpired(task.Id))
            {
                ExpireTask(task.Id);
            }
        }
    }

    public AA1.Models.Task CreateTask(string _title, string _description, int _difficulty, DateTime? _expirationDate)
    {
        try
        {
            AA1.Models.Task task = new AA1.Models.Task(_title, _description, _difficulty, _expirationDate);
            return _taskRepository.CreateTask(task);
        }
        catch (System.Exception e)
        {
            throw new System.Exception($"There was an error creating the task: {e.Message}");
        }
    }

    public AA1.Models.Task GetTaskById(int id)
    {
        if (id < 0)
        {
            throw new System.Exception("Invalid id");
        }
        AA1.Models.Task Task = _taskRepository.GetTaskById(id);
        if (Task == null)
        {
            throw new System.Exception("Task not found");
        }
        return Task;
    }

    public void DeleteTask(int id)
    {
        if (id < 0)
        {
            throw new System.Exception("Invalid id");
        }
        if (_taskRepository.GetTaskById(id) == null)
        {
            throw new System.Exception("Task not found");
        }
        _taskRepository.DeleteTask(id);
    }

    public bool IsTaskExpired(int id)
    {
        AA1.Models.Task task = _taskRepository.GetTaskById(id);
        if (task == null) { throw new System.Exception("Task not found"); }
        return task.ExpirationDate < DateTime.Now;
    }

    public bool IsTaskCompleted(int id)
    {
        AA1.Models.Task task = _taskRepository.GetTaskById(id);
        if (task == null) { throw new System.Exception("Task not found"); }
        return task.Completed;
    }

    public AA1.Models.Task CompleteTask(int id)
    {
        AA1.Models.Task task = _taskRepository.GetTaskById(id);
        if (task == null) { throw new System.Exception("Task not found"); }
        task.Completed = true;
        return _taskRepository.UpdateTask(id, task);
    }

    public AA1.Models.Task ExpireTask(int id)
    {
        AA1.Models.Task task = _taskRepository.GetTaskById(id);
        if (task == null) { throw new System.Exception("Task not found"); }
        task.Expired = true;
        return _taskRepository.UpdateTask(id, task);
    }
}
