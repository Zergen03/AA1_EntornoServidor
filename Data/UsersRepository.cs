using AA1.Models;
using System.Text.Json;

namespace AA1.Data;

class UsersRepository : IUsersRepository
{

    private Dictionary<int, Users> _users = new Dictionary<int, Users>();
    private readonly string _filePath = Environment.GetEnvironmentVariable("USERS_JSON_PATH") ?? "../../../ddbb/Users.json";


    public UsersRepository()
    {
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
        return user;
    }

    public Users DeleteUser(int id)
    {
        Users user = _users[id];
        _users.Remove(id);
        return user;
    }
    public void LoadUsers()
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