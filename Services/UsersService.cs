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

    public Users Login(string _name, string _password)
    {
        Users? user = _repository.GetUserByName(_name);
        if (user == null || user.password != _password)
        {
            throw new System.Exception("User or password incorrect");
        }
        return user;
    }

    public Users Register(string _name, string _password)
    {
        if (_repository.GetUserByName(_name) == null)
        {
            Users user = new Users(_name, _password);
            _repository.CreateUser(user);
        }
        else
        {
            if (_name != "admin")
            {
                throw new System.Exception("User already exists");
            }
        }
        return Login(_name, _password);
    }

    public Users AddTask(int id, AA1.Models.Task task)
    {
        try
        {
            Users user = _repository.GetUserById(id);
            if (task == null)
            {
                throw new System.Exception("Invalid task");
            }
            if(user.Tasks.ContainsKey(task.Id))
            {
                throw new System.Exception("Task already exists");
            }
            user.Tasks.Add(task.Id, task.Title);
            return user;
        }
        catch(Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public List<AA1.Models.Task> GetTasks(int id)
    {
        throw new System.NotImplementedException();
    }
    public Users DeleteTask(int id, int taskId)
    {
        throw new NotImplementedException();
        // _repository.GetUserById(id).tasks.RemoveAll(t => t.Id == taskId);
        // return _repository.GetUserById(id);
    }
    public List<Items> GetInventory(int id)
    {
        throw new NotImplementedException();
        // return _repository.GetUserById(id).items;
    }
    public List<Items> GetEquippedItems(int id)
    {
        throw new NotImplementedException();
        // return _repository.GetUserById(id).equippedItems;
    }
    public Users EquipItem(int id, int itemId)
    {
        throw new NotImplementedException();
        // Users user = _repository.GetUserById(id);
        // Items item = user.items.Find(i => i.Id == itemId);
        // user.equippedItems.Add(item);
        // user.items.Remove(item);
        // return user;
    }
    public Users UnEquipItem(int id, int itemId)
    {
        throw new NotImplementedException();
        // Users user = _repository.GetUserById(id);
        // Items item = user.equippedItems.Find(i => i.Id == itemId);
        // user.items.Add(item);
        // user.equippedItems.Remove(item);
        // return user;
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