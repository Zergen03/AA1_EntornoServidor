using AA1.Models;

namespace AA1.Data;

public interface ITaskRepository{
    List<AA1.Models.Task> GetTasks();
    AA1.Models.Task GetTaskById(int id);
    AA1.Models.Task CreateTask(AA1.Models.Task task);
    AA1.Models.Task UpdateTask(int id, AA1.Models.Task task);
    AA1.Models.Task DeleteTask(int id);
    void LoadTasks();
    void SaveChanges();
}