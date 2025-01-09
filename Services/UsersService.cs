using AA1.Models;
using AA1.Data;

namespace AA1.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;
    public UsersService(IUsersRepository repository)
    {
        _repository = repository;
    }
    public Users AddTask(int id, AA1.Models.Task task)
    {
        if (!_repository.UserExists(id)) { throw new Exception("User not found"); }
        _repository.GetUserById(id).tasks.Add(task);
        return _repository.GetUserById(id);
    }
    public Users DeleteTask(int id, int taskId)
    {
        throw new System.NotImplementedException();
    }
    public Users CompleteTask(int userId, int taskId)
    {
        throw new System.NotImplementedException();
    }
    public Users AddItem(int id, Items item)
    {
        throw new System.NotImplementedException();
    }
    public Users DeleteItem(int id, int itemId)
    {
        throw new System.NotImplementedException();
    }
    public List<Items> GetInventory(int id)
    {
        throw new System.NotImplementedException();
    }
    public List<Items> GetEquippedItems(int id)
    {
        throw new System.NotImplementedException();
    }
    public Users EquipItem(int id, int itemId)
    {
        throw new System.NotImplementedException();
    }
    public Users UnEquipItem(int id, int itemId)
    {
        throw new System.NotImplementedException();
    }
    public Users TakeDamage(int id, int damage)
    {
        throw new System.NotImplementedException();
    }
    public Users Heal(int id, int heal)
    {
        throw new System.NotImplementedException();
    }
    public Users levelUp(int id)
    {
        throw new System.NotImplementedException();
    }
}