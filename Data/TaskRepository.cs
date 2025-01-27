using AA1.Models;
using System.Text.Json;

namespace AA1.Data;

public class TaskRepository : ITaskRepository
{
    private Dictionary<int, AA1.Models.Task> _tasks = new Dictionary<int, AA1.Models.Task>();
    private readonly string _filePath = Environment.GetEnvironmentVariable("TASKS_JSON_PATH") ?? "./ddbb/Tasks.json";

    public TaskRepository()
    {
        LoadTasks();
    }

    public List<AA1.Models.Task> GetTasks()
    {
        return _tasks.Values.ToList();
    }

    public AA1.Models.Task GetTaskById(int id)
    {
        if (!_tasks.ContainsKey(id))
        {
            throw new System.Exception("Task not found");
        }
        return _tasks[id];
    }

    public AA1.Models.Task CreateTask(AA1.Models.Task task)
    {
        _tasks.Add(task.Id, task);
        SaveChanges();
        return task;
    }

    public AA1.Models.Task UpdateTask(int id, AA1.Models.Task task)
    {
        _tasks[id] = task;
        SaveChanges();
        return task;
    }

    public AA1.Models.Task DeleteTask(int id)
    {
        AA1.Models.Task task = _tasks[id];
        _tasks.Remove(id);
        SaveChanges();
        return task;
    }
    public void LoadTasks()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return;
            }
            string jsonString = File.ReadAllText(_filePath);
            if (string.IsNullOrEmpty(jsonString))
            {
                return;
            }
            var tasksToDeserialize = JsonSerializer.Deserialize<List<AA1.Models.Task>>(jsonString);
            if (tasksToDeserialize != null)
            {
                _tasks = tasksToDeserialize.ToDictionary(t => t.Id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading tasks: {ex.Message}");
        }
    }
    public void SaveChanges()
    {
        var tasksToSerialize = _tasks.Values.ToList();
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(tasksToSerialize, options);
        File.WriteAllText(_filePath, jsonString);
    }
}