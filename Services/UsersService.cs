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

    public Users Login(string name, string password)
    {
        Users? user = _repository.GetUsers().Find(u => u.Name == name && u.password == password);
        if(user == null)
        {
            throw new System.Exception("User or password incorrect");
        }
        return user;
    }
    public Users AddTask(int id, AA1.Models.Task task)
    {
        _repository.GetUserById(id).tasks.Add(task);
        return _repository.GetUserById(id);
    }
    public Users DeleteTask(int id, int taskId)
    {
        _repository.GetUserById(id).tasks.RemoveAll(t => t.Id == taskId);
        return _repository.GetUserById(id);
    }
    public List<Items> GetInventory(int id)
    {
        return _repository.GetUserById(id).items;
    }
    public List<Items> GetEquippedItems(int id)
    {
        return _repository.GetUserById(id).equippedItems;
    }
    public Users EquipItem(int id, int itemId)
    {
        Users user = _repository.GetUserById(id);
        Items item = user.items.Find(i => i.Id == itemId);
        user.equippedItems.Add(item);
        user.items.Remove(item);
        return user;
    }
    public Users UnEquipItem(int id, int itemId)
    {
        Users user = _repository.GetUserById(id);
        Items item = user.equippedItems.Find(i => i.Id == itemId);
        user.items.Add(item);
        user.equippedItems.Remove(item);
        return user;
    }
    public Users TakeDamage(int id, int damage)
    {
        Users user = _repository.GetUserById(id);
        user.life -= damage;
        return user;
    }
    public Users Heal(int id, int heal)
    {
        Users user = _repository.GetUserById(id);
        user.life += heal;
        return user;
    }
    public Users levelUp(int id)
    {
        Users user = _repository.GetUserById(id);
        user.level++;
        return user;
    }
}