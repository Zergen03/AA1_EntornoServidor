using AA1.Models;
using Data;
using System.Text.Json;

namespace AA1.Data;

class UsersRepository : IUsersRepository
{

    private Dictionary<int, Users> _users = new Dictionary<int, Users>();
    private readonly string _filePath;

    public UsersRepository()
    {
        _filePath = Config.Get("USERS_JSON_PATH");
        if (string.IsNullOrEmpty(_filePath))
        {
            throw new Exception("Environment variable 'USERS_JSON_PATH' not found or empty.");
        }
        LoadUsers();
    }

    public List<Users> GetUsers()
    {
        return _users.Values.ToList();
    }

    public Users GetUserById(int id)
    {
        if (!_users.ContainsKey(id))
        {
            throw new KeyNotFoundException("Not user found");
        }
        return _users[id];
    }

    public Users GetUserByName(string name)
    {
        Users user = _users.Values.FirstOrDefault(u => u.Name == name);
        return user;
    }

    public Users CreateUser(Users user)
    {
        _users.Add(user.Id, user);
        SaveChanges();
        return user;
    }

    public Users UpdateUser(int id, Users user)
    {
        _users[id] = user;
        SaveChanges();
        return user;
    }

    public Users DeleteUser(int id)
    {
        Users user = _users[id];
        _users.Remove(id);
        SaveChanges();
        return user;
    }
    public void LoadUsers()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                throw new Exception("File not found");
            }
            string jsonString = File.ReadAllText(_filePath);
            if (string.IsNullOrEmpty(jsonString))
            {
                return;
            }
            var UsersToDeserialize = JsonSerializer.Deserialize<List<AA1.Models.Users>>(jsonString);
            if (UsersToDeserialize != null)
            {
                _users = UsersToDeserialize.ToDictionary(u => u.Id);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading users: {ex.Message}");
        }
    }
    public void SaveChanges()
    {
        var UsersToSerialize = _users.Values.ToList();
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(UsersToSerialize, options);
        File.WriteAllText(_filePath, jsonString);

    }
}