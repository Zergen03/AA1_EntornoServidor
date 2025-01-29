using AA1.Models;
using AA1.Data;
using AA1.DTOs;

namespace AA1.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;
    private readonly ITaskService _taskService;
    public UsersService(IUsersRepository repository, ITaskService taskService)
    {
        _repository = repository;
        _taskService = taskService;
    }

    public LoginResultDTO Login(string _name, string _password)
    {
        Users? user = _repository.GetUserByName(_name);
        if (user == null || user.password != _password)
        {
            throw new System.Exception("User or password incorrect");
        }
        int _damageTaken = CheckExpiredTasks(user);
        LoginResultDTO loginResult = new LoginResultDTO
        {
            User = user,
            RemainingLife = user.life,
            DamageTaken = _damageTaken
        };
        return loginResult;
    }

    public LoginResultDTO Register(string _name, string _password)
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

    public void DeleteUser(int id)
    {
        if (_repository.GetUserById(id) == null)
        {
            throw new System.Exception("User not found");
        }
        _repository.DeleteUser(id);
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
            return _repository.UpdateUser(id, user);
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
            foreach (var task in user.Tasks)
            {
                _tasks.Add(task.Key, task.Value);
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
            return _repository.UpdateUser(id, user);
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    public Users BuyItem(int id, Items item)
    {
        Users user = _repository.GetUserById(id);
        if (user.gold < item.Value)
        {
            throw new System.Exception("Not enough gold");
        }
        if (item == null)
        {
            throw new System.Exception("Invalid item");
        }
        if (user.items.ContainsKey(item.Id) || user.equippedItems.ContainsKey(item.Id))
        {
            throw new System.Exception("Item already bought");
        }
        user.gold -= item.Value;
        user.items.Add(item.Id, item.Name);
        _repository.UpdateUser(id, user);
        return user;
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
            foreach (var item in user.items)
            {
                _items.Add(item.Key, item.Value);
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
            foreach (var item in user.equippedItems)
            {
                _items.Add(item.Key, item.Value);
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
            _repository.UpdateUser(id, user);
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
            if (!user.equippedItems.ContainsKey(itemId))
            {
                throw new System.Exception("Item not found");
            }
            user.items.Add(itemId, user.equippedItems[itemId]);
            user.equippedItems.Remove(itemId);
            _repository.UpdateUser(id, user);
            return user;
        }
        catch (Exception e)
        {
            throw new System.Exception(e.Message);
        }
    }
    private Users TakeDamage(int id, int damage)
    {
        Users user = _repository.GetUserById(id);
        user.life -= damage;
        _repository.UpdateUser(id, user);
        return user;
    }
    public Users Heal(int id, int heal)
    {
        Users user = _repository.GetUserById(id);
        user.life += heal;
        _repository.UpdateUser(id, user);
        return user;
    }
    public Users GainXp(int id, int xp)
    {
        Users user = _repository.GetUserById(id);
        double nextLevel = 50 * user.level + (Math.Pow(user.level, 2) * 10);
        if (user.xp + xp >= nextLevel)
        {
            user = LevelUp(id);
            user.xp = user.xp + xp - (int)nextLevel;
        }
        else
        {
            user.xp += xp;
        }
        _repository.UpdateUser(id, user);
        return user;
    }
    private Users LevelUp(int id)
    {
        Users user = _repository.GetUserById(id);
        user.level++;
        _repository.UpdateUser(id, user);
        return user;
    }
    public Users GainGold(int id, int gold)
    {
        Users user = _repository.GetUserById(id);
        user.gold += gold;
        _repository.UpdateUser(id, user);
        return user;
    }

    private int CheckExpiredTasks(Users user)
    {
        int damage = 0;
        foreach (var task in user.Tasks)
        {
            AA1.Models.Task taskToCheck = _taskService.GetTaskById(task.Key);
            if (_taskService.IsTaskExpired(taskToCheck.Id))
            {
                TakeDamage(user.Id, taskToCheck.lostLife);
                damage += taskToCheck.lostLife;
                user.Tasks.Remove(task.Key);
            }
        }
        return damage;
    }
}