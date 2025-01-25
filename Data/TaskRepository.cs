using AA1.Models;
using System.Text.Json;

namespace AA1.Data;

public class TaskRepository : ITaskRepository
{
    private Dictionary<int, AA1.Models.Task> _tasks = new Dictionary<int, AA1.Models.Task>();
    private readonly string _filePath = Environment.GetEnvironmentVariable("USERS_JSON_PATH") ?? "ddbb/Task.json";

    public List<AA1.Models.Task> GetTasks()
    {
        return _tasks.Values.ToList();
    }

    public AA1.Models.Task GetTaskById(int id)
    {
        return _tasks[id];
    }

    public AA1.Models.Task CreateTask(AA1.Models.Task task)
    {
        _tasks.Add(task.Id, task);
        return task;
    }

    public AA1.Models.Task UpdateTask(int id, AA1.Models.Task task)
    {
        _tasks[id] = task;
        return task;
    }

    public AA1.Models.Task DeleteTask(int id)
    {
        AA1.Models.Task task = _tasks[id];
        _tasks.Remove(id);
        return task;
    }
    public void LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return;
        }
        string content = File.ReadAllText(_filePath);
        var tasksToDeserialize = JsonSerializer.Deserialize<List<AA1.Models.Task>>(content);
        _tasks = tasksToDeserialize.ToDictionary(t => t.Id);
    }
    public void SaveChanges()
    {
        var tasksToSerialize = _tasks.Values.ToList();
        var options = new JsonSerializerOptions { WriteIndented = true };
        string content = JsonSerializer.Serialize(tasksToSerialize, options);
        File.WriteAllText(_filePath, content);
    }
}