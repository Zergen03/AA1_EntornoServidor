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
            if (user.Tasks.ContainsKey(task.Id))
            {
                throw new System.Exception("Task already exists");
            }
            user.Tasks.Add(task.Id, task.Title);
            return user;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public Dictionary<int, string> GetTasks(int id)
    {
        try
        {
            Users user = _repository.GetUserById(id);
            if (user.Tasks.Count == 0)
            {
                throw new System.Exception("No tasks found");
            }
            Dictionary<int, string> _tasks = new Dictionary<int, string>();
            for (int i = 0; i < user.Tasks.Count; i++)
            {
                _tasks.Add(i + 1, user.Tasks[i]);
            }
            return _tasks;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public Users DeleteTask(int id, int taskId)
    {
        try
        {
            Users user = _repository.GetUserById(id);
            if (!user.Tasks.ContainsKey(taskId))
            {
                throw new System.Exception("Task not found");
            }
            user.Tasks.Remove(taskId);
            return user;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public Dictionary<int, string> AddItemToInventory(int id, Items item)
    {
        try
        {
            Users user = _repository.GetUserById(id);
            if (item == null)
            {
                throw new System.Exception("Invalid item");
            }
            user.items.Add(item.Id, item.Name);
            return user.items;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public Dictionary<int, string> GetInventory(int id)
    {
        try
        {
            Users user = _repository.GetUserById(id);
            if (user.items.Count == 0)
            {
                throw new System.Exception("No items found");
            }
            Dictionary<int, string> _items = new Dictionary<int, string>();
            for (int i = 0; i < user.items.Count; i++)
            {
                _items.Add(i + 1, user.items[i]);
            }
            return _items;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public Dictionary<int, string> GetEquippedItems(int id)
    {
        try
        {
            Users user = _repository.GetUserById(id);
            if (user.equippedItems.Count == 0)
            {
                throw new System.Exception("No items found");
            }
            Dictionary<int, string> _items = new Dictionary<int, string>();
            for (int i = 0; i < user.equippedItems.Count; i++)
            {
                _items.Add(i + 1, user.equippedItems[i]);
            }
            return _items;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public Users EquipItem(int id, int itemId)
    {
        try
        {
            Users user = _repository.GetUserById(id);
            if (!user.items.ContainsKey(itemId))
            {
                throw new System.Exception("Item not found");
            }
            user.equippedItems.Add(itemId, user.items[itemId]);
            user.items.Remove(itemId);
            return user;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public Users UnEquipItem(int id, int itemId)
    {
        try
        {
            Users user = _repository.GetUserById(id);
            if (!user.items.ContainsKey(itemId))
            {
                throw new System.Exception("Item not found");
            }
            user.items.Add(itemId, user.equippedItems[itemId]);
            user.equippedItems.Remove(itemId);
            return user;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
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
    public Users GainXp(int id, int xp)
    {
        Users user = _repository.GetUserById(id);
        user.xp += xp;
        return user;
    }
    public Users levelUp(int id)
    {
        Users user = _repository.GetUserById(id);
        user.level++;
        return user;
    }
}